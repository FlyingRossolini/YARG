-- yargDB.vwWateringSchedule source
CREATE
OR
REPLACE ALGORITHM = UNDEFINED VIEW `vwWateringSchedule` AS
select
	hex(`ws`.`id`)      AS `id`
  , hex(`ws`.`potID`)   AS `potID`
  , `p`.`name`          AS `potName`
  , `p`.`queuePosition` AS `potQueuePosition`
  , (
		select
			`p`.`speed`
		from
			`Pot` `p`
		where
			`p`.`name` = 'Reservoir'
	)
	                             AS `ebbSpeed`
  , `p`.`speed`                  AS `flowSpeed`
  , `p`.`antiShockRamp`          AS `flowAntiShockRamp`
  , `p`.`expectedFlowRate`       AS `flowExpectedFlowRate`
  , `p`.`pumpFlowErrorThreshold` AS `flowPumpFlowErrorThreshold`
  , `p`.`pulsesPerLiter`         AS `flowPulsesPerLiter`
  , `p`.`currentCapacity`        AS `ebbAmount`
  , (
		select
			`p`.`antiShockRamp`
		from
			`Pot` `p`
		where
			`p`.`name` = 'Reservoir'
	)
	AS `ebbAntiShockRamp`
  , (
		select
			`p`.`expectedFlowRate`
		from
			`Pot` `p`
		where
			`p`.`name` = 'Reservoir'
	)
	AS `ebbExpectedFlowRate`
  , (
		select
			`p`.`pumpFlowErrorThreshold`
		from
			`Pot` `p`
		where
			`p`.`name` = 'Reservoir'
	)
	AS `ebbPumpFlowErrorThreshold`
  , (
		select
			`p`.`pulsesPerLiter`
		from
			`Pot` `p`
		where
			`p`.`name` = 'Reservoir'
	)
	AS `ebbPulsesPerLiter`
  , case
		when current_timestamp() between `x`.`sunriseYesterday` and `x`.`sunsetYesterday`
			then addtime(cast(`x`.`sunriseYesterday` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`rollover` day
		when current_timestamp() between `x`.`sunriseToday` and `x`.`sunsetToday`
			then addtime(cast(`x`.`sunriseToday` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`rollover` day
		when current_timestamp() between `x`.`sunsetYesterday` and `x`.`sunriseToday`
			then addtime(cast(`x`.`sunriseToday` as    date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`rollover` day
			else addtime(cast(`x`.`sunriseTomorrow` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`rollover` day
	end AS `efStartTime`
  , case
		when current_timestamp() between `x`.`sunriseYesterday` and `x`.`sunsetYesterday`
			then addtime(cast(`x`.`sunriseYesterday` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`efDuration` minute + interval `ws`.`rollover` day
		when current_timestamp() between `x`.`sunriseToday` and `x`.`sunsetToday`
			then addtime(cast(`x`.`sunriseToday` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`efDuration` minute + interval `ws`.`rollover` day
		when current_timestamp() between `x`.`sunsetYesterday` and `x`.`sunriseToday`
			then addtime(cast(`x`.`sunriseToday` as    date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`efDuration` minute + interval `ws`.`rollover` day
			else addtime(cast(`x`.`sunriseTomorrow` as date), cast(`ws`.`efStartTime` as time)) + interval `ws`.`efDuration` minute + interval `ws`.`rollover` day
	end               AS `efEndTime`
  , `ws`.`efDuration` AS `efDuration`
  , `ws`.`efAmount`   AS `efAmount`
from
	((`WateringSchedule` `ws`
	join
		`Pot` `p`
		on
			(
				`ws`.`potID` = `p`.`id`
			)
	)
	join
		(
			select
				addtime(cast(current_timestamp() - interval 1 day as date), cast(`gs`.`sunriseTime` as time))                                       AS `sunriseYesterday`
			  , addtime(cast(current_timestamp() - interval 1 day as date), cast(`gs`.`sunriseTime` as time)) + interval `lcY`.`daylightHours` hour AS `sunsetYesterday`
			  , addtime(cast(current_timestamp() as                  date), cast(`gs`.`sunriseTime` as time))                                       AS `sunriseToday`
			  , addtime(cast(current_timestamp() as                  date), cast(`gs`.`sunriseTime` as time)) + interval `lc`.`daylightHours` hour  AS `sunsetToday`
			  , addtime(cast(current_timestamp() + interval 1 day as date), cast(`gs`.`sunriseTime` as time))                                       AS `sunriseTomorrow`
			  , addtime(cast(current_timestamp() + interval 1 day as date), cast(`gs`.`sunriseTime` as time)) + interval `lcT`.`daylightHours` hour AS `sunsetTomorrow`
			  , `gs`.`sunriseTime`                                                                                                                  AS `sunriseTime`
			  , `lcY`.`daylightHours`                                                                                                               AS `daylightHoursY`
			  , `lc`.`daylightHours`                                                                                                                AS `daylightHours`
			from
				((((((((`GrowSeason` `gs`
				join
					`Crop` `c`
					on
						(
							`gs`.`cropID` = `c`.`id`
						)
				)
				join
					`Recipe` `r`
					on
						(
							`gs`.`recipeID` = `r`.`id`
						)
				)
				join
					`RecipeStep` `rs`
					on
						(
							`rs`.`recipeID`       = `r`.`id`
							and `rs`.`weekNumber` = greatest(1, floor((to_days(current_timestamp()) - to_days(`gs`.`startDate`)) / 7))
						)
				)
				join
					`LightCycle` `lc`
					on
						(
							`rs`.`lightCycleID` = `lc`.`id`
						)
				)
				join
					`RecipeStep` `rsY`
					on
						(
							`rsY`.`recipeID`       = `r`.`id`
							and `rsY`.`weekNumber` = greatest(1, floor((to_days(current_timestamp() - interval 1 day) - to_days(`gs`.`startDate`)) / 7))
						)
				)
				join
					`RecipeStep` `rsT`
					on
						(
							`rsT`.`recipeID`       = `r`.`id`
							and `rsT`.`weekNumber` = greatest(1, floor((to_days(current_timestamp() + interval 1 day) - to_days(`gs`.`startDate`)) / 7))
						)
				)
				join
					`LightCycle` `lcY`
					on
						(
							`rsY`.`lightCycleID` = `lcY`.`id`
						)
				)
				join
					`LightCycle` `lcT`
					on
						(
							`rsT`.`lightCycleID` = `lcT`.`id`
						)
				)
		)
		`x`)
order by
	`ws`.`efStartTime`
;
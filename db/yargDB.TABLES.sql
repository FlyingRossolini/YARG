-- yargDB.BackupInfo definition
CREATE TABLE `BackupInfo`
	(
		`lastBackupEndTime` datetime DEFAULT NULL
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.BotHeartbeat definition
CREATE TABLE `BotHeartbeat`
	(
		`id` varbinary(16) NOT NULL
	  , `hostname`           varchar(100) DEFAULT NULL
	  , `uptimeMillis`       bigint(20) unsigned DEFAULT NULL
	  , `task`               varchar(100) DEFAULT NULL
	  , `stackHighWaterMark` bigint(20) unsigned DEFAULT NULL
	  , `vccVoltage`         float DEFAULT NULL
	  , `rssi`               int(11) DEFAULT NULL
	  , `freeHeap`           bigint(20) unsigned DEFAULT NULL
	  , `heapSize`           bigint(20) unsigned DEFAULT NULL
	  , `temperature`        float DEFAULT NULL
	  , `createdBy`          varchar(100) NOT NULL
	  , `createDate`         datetime NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.BotHello definition
CREATE TABLE `BotHello`
	(
		`id`             binary(16) NOT NULL
	  , `uptimeMillis`   bigint(20) unsigned DEFAULT NULL
	  , `macAddress`     varchar(17) DEFAULT NULL
	  , `hostname`       varchar(100) DEFAULT NULL
	  , `cpuFreqMHz`     int(11) DEFAULT NULL
	  , `flashChipSize`  bigint(20) unsigned DEFAULT NULL
	  , `flashChipSpeed` bigint(20) unsigned DEFAULT NULL
	  , `vccVoltage`     float DEFAULT NULL
	  , `freeFlash`      bigint(20) unsigned DEFAULT NULL
	  , `expiryDate`     datetime DEFAULT NULL
	  , `createdBy`      varchar(100) DEFAULT NULL
	  , `createDate`     datetime DEFAULT NULL
	  , `changedBy`      varchar(100) DEFAULT NULL
	  , `changeDate`     datetime DEFAULT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.ChemicalType definition
CREATE TABLE `ChemicalType`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `isH2O2`     tinyint(1) NOT NULL
	  , `isPhUp`     tinyint(1) NOT NULL
	  , `isPhDown`   tinyint(1) NOT NULL
	  , `sorting`    tinyint(4) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , `isActive`   tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Command definition
CREATE TABLE `Command`
	(
		`id`              binary(16) NOT NULL
	  , `commandText`     varchar(100) NOT NULL
	  , `commandDate`     datetime NOT NULL
	  , `priority`        tinyint(4) NOT NULL
	  , `isAcknowledged`  tinyint(1) NOT NULL
	  , `acknowledgeDate` datetime DEFAULT NULL
	  , `isCompleted`     tinyint(1) NOT NULL
	  , `completeDate`    datetime DEFAULT NULL
	  , `isErrored`       tinyint(1) NOT NULL
	  , `ErrorDate`       datetime DEFAULT NULL
	  , `createdBy`       varchar(100) NOT NULL
	  , `createDate`      datetime NOT NULL
	  , `changedBy`       varchar(100) NOT NULL
	  , `changeDate`      date NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Crop definition
CREATE TABLE `Crop`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , `isActive`   tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.FeedingChartType definition
CREATE TABLE `FeedingChartType`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `sorting`    tinyint(4) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , `isActive`   tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.FertigationEventRecord definition
CREATE TABLE `FertigationEventRecord`
	(
		`commandID`              binary(16) NOT NULL
	  , `feDate`                 datetime(6) DEFAULT NULL
	  , `cafeDate`               datetime(6) DEFAULT NULL
	  , `ebbPump_RunDate`        datetime(6) DEFAULT NULL
	  , `ebbFlowmeter_DoneDate`  datetime(6) DEFAULT NULL
	  , `ebbPump_DoneDate`       datetime(6) DEFAULT NULL
	  , `flowPump_StartDate`     datetime(6) DEFAULT NULL
	  , `flowPump_RunDate`       datetime(6) DEFAULT NULL
	  , `flowFlowmeter_DoneDate` datetime(6) DEFAULT NULL
	  , `flowPump_DoneDate`      datetime(6) DEFAULT NULL
	  , `isError`                bit(1) DEFAULT NULL
	  , `errorDate`              datetime(6) DEFAULT NULL
	  , PRIMARY KEY (`commandID`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.LightCycle definition
CREATE TABLE `LightCycle`
	(
		`id`            binary(16) NOT NULL
	  , `name`          varchar(100) NOT NULL
	  , `daylightHours` tinyint(4) NOT NULL
	  , `createdBy`     varchar(100) NOT NULL
	  , `createDate`    datetime NOT NULL
	  , `changedBy`     varchar(100) NOT NULL
	  , `changeDate`    datetime NOT NULL
	  , `isActive`      tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.LimitType definition
CREATE TABLE `LimitType`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `sorting`    tinyint(4) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , `isActive`   tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Location definition
CREATE TABLE `Location`
	(
		`id`                  binary(16) NOT NULL
	  , `name`                varchar(100) NOT NULL
	  , `sorting`             tinyint(4) NOT NULL
	  , `isShowOnLandingPage` tinyint(1) DEFAULT NULL
	  , `createdBy`           varchar(100) NOT NULL
	  , `createDate`          datetime NOT NULL
	  , `changedBy`           varchar(100) NOT NULL
	  , `changeDate`          datetime NOT NULL
	  , `isActive`            tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.MeasurementType definition
CREATE TABLE `MeasurementType`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `sorting`    tinyint(4) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , `isActive`   tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Pot definition
CREATE TABLE `Pot`
	(
		`id`                     binary(16) NOT NULL
	  , `name`                   varchar(100) NOT NULL
	  , `queuePosition`          tinyint(4) NOT NULL
	  , `totalCapacity`          decimal(6,2) NOT NULL
	  , `speed`                  tinyint(3) unsigned NOT NULL
	  , `currentCapacity`        decimal(6,2) NOT NULL
	  , `antiShockRamp`          smallint(6) NOT NULL
	  , `expectedFlowRate`       decimal(6,2) NOT NULL
	  , `pumpFlowErrorThreshold` tinyint(4) NOT NULL
	  , `pulsesPerLiter`         smallint(6) NOT NULL
	  , `isReservoir`            bit(1) DEFAULT NULL
	  , `createdBy`              varchar(100) NOT NULL
	  , `createDate`             datetime NOT NULL
	  , `changedBy`              varchar(100) NOT NULL
	  , `changeDate`             datetime NOT NULL
	  , `isActive`               tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci
;

-- yargDB.RPIHeartbeat definition
CREATE TABLE `RPIHeartbeat`
	(
		`id` varbinary(16) NOT NULL
	  , `macAddress`   varchar(17) DEFAULT NULL
	  , `hostname`     varchar(100) DEFAULT NULL
	  , `cpuUsage`     decimal(10,2) DEFAULT NULL
	  , `memoryUsage`  decimal(10,2) DEFAULT NULL
	  , `diskUsage`    decimal(10,2) DEFAULT NULL
	  , `uptimeMillis` bigint(20) unsigned DEFAULT NULL
	  , `temperature`  decimal(10,2) DEFAULT NULL
	  , `loadAverage`  decimal(10,2) DEFAULT NULL
	  , `voltage`      decimal(10,2) DEFAULT NULL
	  , `createdBy`    varchar(100) NOT NULL
	  , `createDate`   datetime NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RPIHello definition
CREATE TABLE `RPIHello`
	(
		`id` varbinary(16) NOT NULL
	  , `macAddress`                varchar(17) DEFAULT NULL
	  , `hostname`                  varchar(100) DEFAULT NULL
	  , `osName`                    varchar(100) DEFAULT NULL
	  , `osVersion`                 varchar(100) DEFAULT NULL
	  , `bootloaderFirmwareVersion` varchar(100) DEFAULT NULL
	  , `cpuModel`                  varchar(100) DEFAULT NULL
	  , `cpuCores`                  tinyint(4) DEFAULT NULL
	  , `cpuTemperature`            decimal(10,2) DEFAULT NULL
	  , `cpuSerialNumber`           varchar(100) DEFAULT NULL
	  , `totalRAM`                  bigint(20) unsigned DEFAULT NULL
	  , `totalDiskSpace`            bigint(20) unsigned DEFAULT NULL
	  , `totalUsedDiskSpace`        bigint(20) unsigned DEFAULT NULL
	  , `uptimeMillis`              bigint(20) unsigned DEFAULT NULL
	  , `firstBootDateTime`         datetime DEFAULT NULL
	  , `expiryDate`                datetime DEFAULT NULL
	  , `createdBy`                 varchar(100) NOT NULL
	  , `createDate`                datetime NOT NULL
	  , `changedBy`                 varchar(100) DEFAULT NULL
	  , `changeDate`                datetime DEFAULT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Recipe definition
CREATE TABLE `Recipe`
	(
		`id`         binary(16) NOT NULL
	  , `name`       varchar(100) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , PRIMARY KEY (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Chemical definition
CREATE TABLE `Chemical`
	(
		`id`              binary(16) NOT NULL
	  , `name`            varchar(100) NOT NULL
	  , `chemicalTypeID`  binary(16) NOT NULL
	  , `manufacturer`    varchar(100) NOT NULL
	  , `mixPriority`     tinyint(4) DEFAULT NULL
	  , `mixTime`         tinyint(4) NOT NULL
	  , `pricePerL`       decimal(19,4) NOT NULL
	  , `inStockAmount`   decimal(19,4) NOT NULL
	  , `minReorderPoint` decimal(19,4) NOT NULL
	  , `createdBy`       varchar(100) NOT NULL
	  , `createDate`      datetime NOT NULL
	  , `changedBy`       varchar(100) NOT NULL
	  , `changeDate`      datetime NOT NULL
	  , `isActive`        tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `Chemical_FK` (`chemicalTypeID`)
	  , CONSTRAINT `Chemical_FK` FOREIGN KEY (`chemicalTypeID`) REFERENCES `ChemicalType` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.GrowSeason definition
CREATE TABLE `GrowSeason`
	(
		`id`          binary(16) NOT NULL
	  , `name`        varchar(100) NOT NULL
	  , `startDate`   datetime DEFAULT NULL
	  , `endDate`     datetime DEFAULT NULL
	  , `sunriseTime` datetime NOT NULL
	  , `cropID`      binary(16) NOT NULL
	  , `recipeID`    binary(16) DEFAULT NULL
	  , `isComplete`  tinyint(1) NOT NULL
	  , `createdBy`   varchar(100) NOT NULL
	  , `createDate`  datetime NOT NULL
	  , `changedBy`   varchar(100) NOT NULL
	  , `changeDate`  datetime NOT NULL
	  , `isActive`    tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `Season_FK` (`cropID`)
	  , KEY `GrowSeason_FK` (`recipeID`)
	  , KEY `GrowSeason_startDate_IDX` (`startDate`) USING BTREE
	  , CONSTRAINT `Season_FK` FOREIGN KEY (`cropID`) REFERENCES `Crop` (`id`) ON
UPDATE
	CASCADE
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Jar definition
CREATE TABLE `Jar`
	(
		`id`              binary(16) NOT NULL
	  , `mixFanPosition`  tinyint(4) NOT NULL
	  , `chemicalID`      binary(16) NOT NULL
	  , `mixTimesPerDay`  tinyint(4) NOT NULL
	  , `duration`        tinyint(4) NOT NULL
	  , `capacity`        decimal(19,4) NOT NULL
	  , `currentAmount`   decimal(19,4) NOT NULL
	  , `mixFanOverSpeed` int(11) NOT NULL
	  , `createdBy`       varchar(100) NOT NULL
	  , `createDate`      datetime NOT NULL
	  , `changedBy`       varchar(100) NOT NULL
	  , `changeDate`      datetime NOT NULL
	  , `isActive`        tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `Jar_FK` (`chemicalID`)
	  , CONSTRAINT `Jar_FK` FOREIGN KEY (`chemicalID`) REFERENCES `Chemical` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Measurement definition
CREATE TABLE `Measurement`
	(
		`id`                binary(16) NOT NULL
	  , `growSeasonID`      binary(16) NOT NULL
	  , `locationID`        binary(16) NOT NULL
	  , `measurementTypeID` binary(16) NOT NULL
	  , `limitUCL`          decimal(6,2) DEFAULT NULL
	  , `measuredValue`     decimal(6,2) NOT NULL
	  , `limitLCL`          decimal(6,2) DEFAULT NULL
	  , `createdBy`         varchar(100) NOT NULL
	  , `createDate`        datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `Measurement_FK` (`locationID`)
	  , KEY `Measurement_FK_1` (`measurementTypeID`)
	  , KEY `Measurement_FK_2` (`growSeasonID`)
	  , CONSTRAINT `Measurement_FK` FOREIGN KEY (`locationID`) REFERENCES `Location` (`id`)
	  , CONSTRAINT `Measurement_FK_1` FOREIGN KEY (`measurementTypeID`) REFERENCES `MeasurementType` (`id`)
	  , CONSTRAINT `Measurement_FK_2` FOREIGN KEY (`growSeasonID`) REFERENCES `GrowSeason` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.MixingFanSchedule definition
CREATE TABLE `MixingFanSchedule`
	(
		`id`              binary(16) NOT NULL
	  , `jarID`           binary(16) NOT NULL
	  , `mfStartTime`     datetime NOT NULL
	  , `mfDuration`      tinyint(4) NOT NULL
	  , `isAcknowledged`  tinyint(1) NOT NULL
	  , `acknowledgeDate` datetime DEFAULT NULL
	  , `isCompleted`     tinyint(1) NOT NULL
	  , `completeDate`    datetime DEFAULT NULL
	  , `isErrorState`    tinyint(1) NOT NULL
	  , `errorDate`       datetime DEFAULT NULL
	  , `createdBy`       varchar(100) NOT NULL
	  , `createDate`      datetime NOT NULL
	  , `changedBy`       varchar(100) NOT NULL
	  , `changeDate`      datetime NOT NULL
	  , `isActive`        tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `MixingFanSchedule_FK` (`jarID`)
	  , CONSTRAINT `MixingFanSchedule_FK` FOREIGN KEY (`jarID`) REFERENCES `Jar` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.Pump definition
CREATE TABLE `Pump`
	(
		`id` varbinary(16) NOT NULL
	  , `potID` varbinary(16) NOT NULL
	  , `make`           varchar(100) DEFAULT NULL
	  , `model`          varchar(100) DEFAULT NULL
	  , `serialNumber`   varchar(100) DEFAULT NULL
	  , `vendor`         varchar(100) DEFAULT NULL
	  , `price`          decimal(19,4) DEFAULT NULL
	  , `pulsesPerLiter` int(11) NOT NULL
	  , `purchaseDate`   datetime DEFAULT NULL
	  , `installDate`    datetime DEFAULT NULL
	  , `createDate`     datetime NOT NULL
	  , `createdBy`      varchar(100) NOT NULL
	  , `changeDate`     datetime NOT NULL
	  , `changedBy`      varchar(100) NOT NULL
	  , `isActive`       tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `Pump_FK` (`potID`)
	  , CONSTRAINT `Pump_FK` FOREIGN KEY (`potID`) REFERENCES `Pot` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.PumpWorkLog definition
CREATE TABLE `PumpWorkLog`
	(
		`id`            binary(16) NOT NULL
	  , `pumpID`        binary(16) NOT NULL
	  , `startDate`     datetime(3) NOT NULL
	  , `endDate`       datetime(3) NOT NULL
	  , `antiShockRamp` smallint(6) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `PumpWorkLog_FK` (`pumpID`)
	  , CONSTRAINT `PumpWorkLog_FK` FOREIGN KEY (`pumpID`) REFERENCES `Pump` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RPIServiceYargHeartbeat definition
CREATE TABLE `RPIServiceYargHeartbeat`
	(
		`id` varbinary(16) NOT NULL
	  , `rpiHeartbeatID` varbinary(16) NOT NULL
	  , `yargAppCurrentTasks` smallint(6) DEFAULT NULL
	  , `yargAppTaskLimit`    smallint(6) DEFAULT NULL
	  , `yargAppCpuCount`     varchar(100) DEFAULT NULL
	  , `yargAppStatus`       varchar(100) DEFAULT NULL
	  , `createdBy`           varchar(100) NOT NULL
	  , `createDate`          datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RPIServiceYargHeartbeat_FK` (`rpiHeartbeatID`)
	  , CONSTRAINT `RPIServiceYargHeartbeat_FK` FOREIGN KEY (`rpiHeartbeatID`) REFERENCES `RPIHeartbeat` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RecipeChemList definition
CREATE TABLE `RecipeChemList`
	(
		`id`         binary(16) NOT NULL
	  , `recipeID`   binary(16) NOT NULL
	  , `chemicalID` binary(16) NOT NULL
	  , `mixtime`    tinyint(4) NOT NULL
	  , `createdBy`  varchar(100) NOT NULL
	  , `createDate` datetime NOT NULL
	  , `changedBy`  varchar(100) NOT NULL
	  , `changeDate` datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RecipeChemList_FK` (`recipeID`)
	  , KEY `RecipeChemList_FK_1` (`chemicalID`)
	  , CONSTRAINT `RecipeChemList_FK` FOREIGN KEY (`recipeID`) REFERENCES `Recipe` (`id`)
	  , CONSTRAINT `RecipeChemList_FK_1` FOREIGN KEY (`chemicalID`) REFERENCES `Chemical` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RecipeStep definition
CREATE TABLE `RecipeStep`
	(
		`id`                     binary(16) NOT NULL
	  , `recipeID`               binary(16) NOT NULL
	  , `weekNumber`             tinyint(4) NOT NULL
	  , `lightCycleID`           binary(16) NOT NULL
	  , `irrigationEventsPerDay` tinyint(4) NOT NULL
	  , `soakTime`               tinyint(4) NOT NULL
	  , `isMorningSip`           tinyint(1) NOT NULL
	  , `isEveningSip`           tinyint(1) NOT NULL
	  , `createdBy`              varchar(100) NOT NULL
	  , `createDate`             datetime NOT NULL
	  , `changedBy`              varchar(100) NOT NULL
	  , `changeDate`             datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RecipeStep_FK` (`recipeID`)
	  , KEY `RecipeStep_FK_1` (`lightCycleID`)
	  , KEY `RecipeStep_weekNumber_IDX` (`weekNumber`) USING BTREE
	  , CONSTRAINT `RecipeStep_FK` FOREIGN KEY (`recipeID`) REFERENCES `Recipe` (`id`)
	  , CONSTRAINT `RecipeStep_FK_1` FOREIGN KEY (`lightCycleID`) REFERENCES `LightCycle` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RecipeStepAmount definition
CREATE TABLE `RecipeStepAmount`
	(
		`id`           binary(16) NOT NULL
	  , `recipeStepID` binary(16) NOT NULL
	  , `chemicalID`   binary(16) NOT NULL
	  , `amount`       decimal(19,4) NOT NULL
	  , `createdBy`    varchar(100) NOT NULL
	  , `createDate`   datetime NOT NULL
	  , `changedBy`    varchar(100) NOT NULL
	  , `changeDate`   datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RecipeStepAmount_FK` (`recipeStepID`)
	  , KEY `RecipeStepAmount_FK_1` (`chemicalID`)
	  , CONSTRAINT `RecipeStepAmount_FK` FOREIGN KEY (`recipeStepID`) REFERENCES `RecipeStep` (`id`)
	  , CONSTRAINT `RecipeStepAmount_FK_1` FOREIGN KEY (`chemicalID`) REFERENCES `Chemical` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RecipeStepLimit definition
CREATE TABLE `RecipeStepLimit`
	(
		`id`                binary(16) NOT NULL
	  , `recipeStepID`      binary(16) NOT NULL
	  , `locationID`        binary(16) NOT NULL
	  , `measurementTypeID` binary(16) NOT NULL
	  , `limitTypeID`       binary(16) NOT NULL
	  , `value`             decimal(19,4) NOT NULL
	  , `createdBy`         varchar(100) NOT NULL
	  , `createDate`        datetime NOT NULL
	  , `changedBy`         varchar(100) NOT NULL
	  , `changeDate`        datetime NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RecipeStepLimit_FK` (`recipeStepID`)
	  , KEY `RecipeStepLimit_FK_1` (`locationID`)
	  , KEY `RecipeStepLimit_FK_2` (`measurementTypeID`)
	  , KEY `RecipeStepLimit_FK_3` (`limitTypeID`)
	  , CONSTRAINT `RecipeStepLimit_FK` FOREIGN KEY (`recipeStepID`) REFERENCES `RecipeStep` (`id`)
	  , CONSTRAINT `RecipeStepLimit_FK_1` FOREIGN KEY (`locationID`) REFERENCES `Location` (`id`)
	  , CONSTRAINT `RecipeStepLimit_FK_2` FOREIGN KEY (`measurementTypeID`) REFERENCES `MeasurementType` (`id`)
	  , CONSTRAINT `RecipeStepLimit_FK_3` FOREIGN KEY (`limitTypeID`) REFERENCES `LimitType` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.RemoteProbe definition
CREATE TABLE `RemoteProbe`
	(
		`id`                 binary(16) NOT NULL
	  , `locationID`         binary(16) NOT NULL
	  , `measurementTypeID`  binary(16) NOT NULL
	  , `remoteProbeAddress` varchar(3) NOT NULL
	  , `gtUCLCommand`       varchar(100) DEFAULT NULL
	  , `ltLCLCommand`       varchar(100) DEFAULT NULL
	  , `createdBy`          varchar(100) NOT NULL
	  , `createDate`         datetime NOT NULL
	  , `changedBy`          varchar(100) NOT NULL
	  , `changeDate`         datetime NOT NULL
	  , `isActive`           tinyint(1) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `RemoteProbe_FK` (`locationID`)
	  , KEY `RemoteProbe_FK_1` (`measurementTypeID`)
	  , CONSTRAINT `RemoteProbe_FK` FOREIGN KEY (`locationID`) REFERENCES `Location` (`id`)
	  , CONSTRAINT `RemoteProbe_FK_1` FOREIGN KEY (`measurementTypeID`) REFERENCES `MeasurementType` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargDB.WateringSchedule definition
CREATE TABLE `WateringSchedule`
	(
		`id`          binary(16) NOT NULL
	  , `potID`       binary(16) NOT NULL
	  , `efStartTime` datetime NOT NULL
	  , `efDuration`  tinyint(4) NOT NULL
	  , `efAmount`    decimal(3,1) NOT NULL
	  , `rollover`    tinyint(4) NOT NULL
	  , PRIMARY KEY (`id`)
	  , KEY `WateringSchedule_FK` (`potID`)
	  , CONSTRAINT `WateringSchedule_FK` FOREIGN KEY (`potID`) REFERENCES `Pot` (`id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;
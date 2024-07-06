CREATE DEFINER=`yarguser`@`%` FUNCTION `yargDB`.`CalculateSunriseSunset`(currentDate DATETIME, sunriseTime TIME, daylightHoursY INT, daylightHours INT )
RETURNS datetime
BEGIN
	DECLARE sunrise DATETIME;
	IF ( currentDate BETWEEN ADDTIME(DATE_SUB(currentDate, INTERVAL 1 DAY), sunriseTime)
	AND
	DATE_ADD(ADDTIME(DATE_SUB(currentDate, INTERVAL 1 DAY), sunriseTime), INTERVAL daylightHoursY HOUR) )
	OR
	(
		currentDate BETWEEN ADDTIME(DATE(currentDate), sunriseTime)
		AND
		DATE_ADD(ADDTIME(DATE(currentDate), sunriseTime), INTERVAL daylightHours HOUR)
	)
	THEN
	SET sunrise = ADDTIME(DATE(currentDate), sunriseTime);
	ELSE
	SET sunrise = ADDTIME(DATE_ADD(currentDate, INTERVAL 1 DAY), sunriseTime);
END
IF;
RETURN sunrise;
END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAcknowledgeMixingFanSchedule`(IN mixingFanScheduleID tinytext , IN remoteHostName TINYTEXT , IN ackDate datetime )
BEGIN
	update
		MixingFanSchedule
	set isAcknowledged  = TRUE
	  , acknowledgeDate = NOW()
	  , changedBy       = remoteHostName
	  , changeDate      = ackDate
	where
		id = unhex(replace(mixingFanScheduleID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAcknowledgeWateringSchedule`(IN wateringScheduleID tinytext , IN remoteHostName TINYTEXT , IN ackDate datetime )
BEGIN
	update
		WateringSchedule
	set isAcknowledged  = TRUE
	  , acknowledgeDate = NOW()
	  , changedBy       = remoteHostName
	  , changeDate      = ackDate
	where
		id = unhex(replace(wateringScheduleID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spActiveBucketCount`()
BEGIN
	SELECT
		count(id) as 'cntBucket'
	FROM
		Pot
	where
		isActive = true
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spActiveGrowSeason`()
BEGIN
	select
		case
			when
				(
					select
						count(id)
					from
						GrowSeason
					where
						isActive = true
				)
				> 0
				then TRUE
				else FALSE
		end as flgActiveGrowSeason
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddBotHeartbeat`( _id tinytext, _hostname varchar(100), _uptimeMillis bigint(20) unsigned, _task varchar(100), _stackHighWaterMark bigint(20) unsigned, _vccVoltage float, _rssi int, _freeHeap bigint(20) unsigned, _heapSize bigint(20) unsigned, _temperature float, _createdBy varchar(100), _createDate datetime)
BEGIN
	INSERT into BotHeartbeat
		( id
		  , hostname
		  , uptimeMillis
		  , task
		  , stackHighWaterMark
		  , vccVoltage
		  , rssi
		  , freeHeap
		  , heapSize
		  , temperature
		  , createdBy
		  , createDate
		)
		values
		(unhex(replace(_id,'-',''))
		  , _hostname
		  , _uptimeMillis
		  , _task
		  , _stackHighWaterMark
		  , _vccVoltage
		  , _rssi
		  , _freeHeap
		  , _heapSize
		  , _temperature
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddBotHello`( _id tinytext, _uptimeMillis bigint(20) unsigned, _macAddress varchar(17), _hostname varchar(100), _cpuFreqMHz int(11), _flashChipSize bigint(20) unsigned, _flashChipSpeed bigint(20) unsigned, _vccVoltage float, _freeFlash bigint(20) unsigned, _createdBy varchar(100), _createDate datetime )
BEGIN
	INSERT INTO BotHello
		( id
		  , uptimeMillis
		  , macAddress
		  , hostname
		  , cpuFreqMHz
		  , flashChipSize
		  , flashChipSpeed
		  , vccVoltage
		  , freeFlash
		  , createdBy
		  , createDate
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , _uptimeMillis
		  , _macAddress
		  , _hostname
		  , _cpuFreqMHz
		  , _flashChipSize
		  , _flashChipSpeed
		  , _vccVoltage
		  , _freeFlash
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddChemical`(IN id tinytext , IN name TINYTEXT , IN manufacturer TINYTEXT , IN chemicalTypeID tinytext , in mixPriority tinyint(4) , in mixTime tinyint(4) , IN pricePerL decimal(19,4) , IN inStockAmount decimal(19,4) , IN minReorderPoint tinyint(4) , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO Chemical
		( id
		  , name
		  , manufacturer
		  , chemicalTypeID
		  , mixPriority
		  , mixTime
		  , pricePerL
		  , inStockAmount
		  , minReorderPoint
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		values
		(unhex(replace(id,'-',''))
		  , name
		  , manufacturer
		  , unhex(replace(chemicalTypeID,'-',''))
		  , mixPriority
		  , mixTime
		  , pricePerL
		  , inStockAmount
		  , minReorderPoint
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddChemicalType`(IN id tinytext , IN name TINYTEXT , IN sorting TINYINT(4) , IN isH2O2 bool , IN isPhUp bool , IN isPhDown bool , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	insert into ChemicalType
		( id
		  , name
		  , isH2O2
		  , isPhUp
		  , isPhDown
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		values
		( unhex(replace(id,'-',''))
		  , name
		  , isH2O2
		  , isPhUp
		  , isPhDown
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddCrop`(IN id tinytext , IN name TINYTEXT , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO Crop
		( id
		  , name
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddFeedingChartType`(IN id tinytext , IN name TINYTEXT , IN sorting TINYINT , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO FeedingChartType
		( id
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddGrowSeason`(IN id tinytext , IN name TINYTEXT , in startDate DATETIME , in endDate DATETIME , in sunriseTime DATETIME , in cropID tinytext , in recipeID tinytext , IN isComplete bool , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO GrowSeason
		( id
		  , name
		  , startDate
		  , endDate
		  , sunriseTime
		  , cropID
		  , recipeID
		  , isComplete
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , startDate
		  , endDate
		  , sunriseTime
		  , unhex(replace(cropID,'-',''))
		  , unhex(replace(recipeID, '-',''))
		  , isComplete
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddJar`(IN id tinytext ,IN mixFanPosition tinyint(4) ,IN chemicalID tinytext ,IN mixTimesPerDay tinyint(4) ,IN duration tinyint(4) ,IN capacity decimal(19,4) ,IN currentAmount decimal(19,4) ,IN mixFanOverSpeed int , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	insert into Jar
		( id
		  , mixFanPosition
		  , chemicalID
		  , mixTimesPerDay
		  , duration
		  , capacity
		  , currentAmount
		  , mixFanOverSpeed
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		values
		( unhex(replace(id,'-',''))
		  , mixFanPosition
		  , unhex(replace(chemicalID,'-',''))
		  , mixTimesPerDay
		  , duration
		  , capacity
		  , currentAmount
		  , mixFanOverSpeed
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddLightCycle`(IN id tinytext , IN name TINYTEXT , IN daylightHours TINYINT , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO LightCycle
		( id
		  , name
		  , daylightHours
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , daylightHours
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddLimit`(IN id tinytext , IN parentId tinytext , IN measurementTypeId tinytext , IN limitTypeId tinytext , IN limitValue decimal(6,2) , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME)
BEGIN
	INSERT INTO `Limit`
		( id
		  , parentId
		  , measurementTypeId
		  , limitTypeId
		  , limitValue
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , unhex(replace(parentId,'-',''))
		  , unhex(replace(measurementTypeId,'-',''))
		  , unhex(replace(limitTypeId,'-',''))
		  , limitValue
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddLimitType`(IN id tinytext , IN name TINYTEXT , IN sorting TINYINT , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO LimitType
		( id
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddLocation`(IN id tinytext , IN name TINYTEXT , IN sorting TINYINT(4) , IN flgIsShowOnLandingPage bool , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO Location
		( id
		  , name
		  , sorting
		  , isShowOnLandingPage
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , sorting
		  , isShowOnLandingPage
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddMeasurement`(IN _id tinytext , IN _growSeasonID tinytext , IN _locationID tinytext , IN _measurementTypeID tinytext , IN _measuredValue decimal(6,2) , IN _limitLCL decimal(19,2) , IN _limitUCL decimal(19,2) , IN _createdBy TINYTEXT , IN _createDate DATETIME)
BEGIN
	insert into Measurement
		( id
		  , growSeasonID
		  , locationID
		  , measurementTypeID
		  , measuredValue
		  , limitLCL
		  , limitUCL
		  , createdBy
		  , createDate
		)
		values
		( unhex(replace(_id,'-',''))
		  , unhex(replace(_growSeasonID,'-',''))
		  , unhex(replace(_locationID,'-',''))
		  , unhex(replace(_measurementTypeID,'-',''))
		  , _measuredValue
		  , _limitLCL
		  , _limitUCL
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddMeasurementType`(IN id tinytext , IN name TINYTEXT , IN sorting TINYINT , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO MeasurementType
		( id
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , sorting
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddMixingFanSchedule`(IN id tinytext , IN jarID tinytext , IN mfStartTime datetime , IN mfDuration tinyint , IN isErrorState BOOL , IN isAcknowledged BOOL , IN isCompleted BOOL , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME , IN isActive BOOL)
BEGIN
	INSERT INTO MixingFanSchedule
		( id
		  , jarID
		  , mfStartTime
		  , mfDuration
		  , isAcknowledged
		  , isCompleted
		  , isErrorState
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , unhex(replace(jarID,'-',''))
		  , mfStartTime
		  , mfDuration
		  , isAcknowledged
		  , isCompleted
		  , isErrorState
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddPot`(IN _id tinytext , IN _name TINYTEXT , IN _totalCapacity decimal(6,2) , IN _speed TINYINT , IN _currentCapacity decimal(6,2) , IN _antiShockRamp smallint , IN _expectedFlowRate decimal(6,2) , IN _pumpFlowErrorThreshold TINYINT , IN _pulsesPerLiter smallint , IN _isReservoir BOOL , IN _createdBy TINYTEXT , IN _createDate DATETIME , IN _changedBy TINYTEXT , IN _changeDate DATETIME , IN _isActive BOOL)
BEGIN
	DECLARE maxQueuePosition INT;
	SELECT
		IFNULL(MAX(queuePosition) + 1, 1)
	INTO
		maxQueuePosition
	FROM
		Pot
	;
	
	INSERT INTO Pot
		( id
		  , name
		  , queuePosition
		  , totalCapacity
		  , speed
		  , currentCapacity
		  , antiShockRamp
		  , expectedFlowRate
		  , pumpFlowErrorThreshold
		  , pulsesPerLiter
		  , isReservoir
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( UNHEX(REPLACE(_id, '-', ''))
		  , _name
		  , maxQueuePosition
		  , _totalCapacity
		  , _speed
		  , _currentCapacity
		  , _antiShockRamp
		  , _expectedFlowRate
		  , _pumpFlowErrorThreshold
		  , _pulsesPerLiter
		  , _isReservoir
		  , _createdBy
		  , _createDate
		  , _changedBy
		  , _changeDate
		  , isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddPump`(IN _id tinytext, _potID tinytext, _make varchar(100), _model varchar(100), _serialNumber varchar(100), _vendor varchar(100), _price decimal(19,4), _pulsesPerLiter int(11), _purchaseDate datetime, _installDate datetime, _createDate datetime, _createdBy varchar(100), _changeDate datetime, _changedBy varchar(100), _isActive tinyint(1))
BEGIN
	INSERT INTO Pump
		(id
		  , potID
		  , make
		  , model
		  , serialNumber
		  , vendor
		  , price
		  , pulsesPerLiter
		  , purchaseDate
		  , installDate
		  , createDate
		  , createdBy
		  , changeDate
		  , changedBy
		  , isActive
		)
		VALUES
		(unhex(replace(id,'-',''))
		  , unhex(replace(_potID,'-',''))
		  , _make
		  , _model
		  , _serialNumber
		  , _vendor
		  , _price
		  , _pulsesPerLiter
		  , CASE
				WHEN _purchaseDate IS NOT NULL
					THEN _purchaseDate
					ELSE NULL
			END
		  , CASE
				WHEN _installDate IS NOT NULL
					THEN _installDate
					ELSE NULL
			END
		  , _createDate
		  , _createdBy
		  , _changeDate
		  , _changedBy
		  , _isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRecipe`(IN id tinytext , IN name tinytext , IN createdBy TINYTEXT , IN createDate DATETIME , IN changedBy TINYTEXT , IN changeDate DATETIME)
BEGIN
	INSERT INTO Recipe
		( id
		  , name
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
		VALUES
		( unhex(replace(id,'-',''))
		  , name
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRecipeStep`(IN thisid tinytext , IN thisrecipeID tinytext , IN thisweekNumber tinyint(4) , IN thislightCycleID tinytext , IN thisirrigationEventsPerDay tinyint(4) , IN thissoakTime tinyint(4) , IN thisisMorningSip tinyint(1) , IN thisisEveningSip tinyint(1) , IN thiscreatedBy tinytext , IN thiscreateDate datetime , IN thischangedBy tinytext , IN thischangeDate datetime )
BEGIN
	insert into RecipeStep
		( id
		  , recipeID
		  , weekNumber
		  , lightCycleID
		  , irrigationEventsPerDay
		  , soakTime
		  , isMorningSip
		  , isEveningSip
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
		values
		( unhex(replace(thisid,'-',''))
		  , unhex(replace(thisrecipeID,'-',''))
		  , thisweekNumber
		  , unhex(replace(thislightCycleID,'-',''))
		  , thisirrigationEventsPerDay
		  , thissoakTime
		  , thisisMorningSip
		  , thisisEveningSip
		  , thiscreatedBy
		  , thiscreateDate
		  , thischangedBy
		  , thischangeDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRecipeStepAmount`(IN thisid tinytext , IN thisrecipeStepID tinytext , IN thischemicalID tinytext , IN thisamount decimal(19,4) , IN thiscreatedBy tinytext , IN thiscreateDate datetime , IN thischangedBy tinytext , IN thischangeDate datetime )
BEGIN
	insert into RecipeStepAmount
		( id
		  , recipeStepID
		  , chemicalID
		  , amount
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
		values
		( unhex(replace(thisid,'-',''))
		  , unhex(replace(thisrecipeStepID,'-',''))
		  , unhex(replace(thischemicalID,'-',''))
		  , thisamount
		  , thiscreatedBy
		  , thiscreateDate
		  , thischangedBy
		  , thischangeDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRecipeStepLimit`(IN thisid tinytext , IN thisrecipeStepID tinytext , IN thislocationID tinytext , IN thismeasurementTypeID tinytext , IN thislimitTypeID tinytext , IN thisvalue decimal(19,4) , IN thiscreatedBy tinytext , IN thiscreateDate datetime , IN thischangedBy tinytext , IN thischangeDate datetime )
BEGIN
	insert into RecipeStepLimit
		( id
		  , recipeStepID
		  , locationID
		  , measurementTypeID
		  , limitTypeID
		  , value
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		)
		values
		( unhex(replace(thisid,'-',''))
		  , unhex(replace(thisrecipeStepID,'-',''))
		  , unhex(replace(thislocationID,'-',''))
		  , unhex(replace(thismeasurementTypeID,'-',''))
		  , unhex(replace(thislimitTypeID,'-',''))
		  , thisvalue
		  , thiscreatedBy
		  , thiscreateDate
		  , thischangedBy
		  , thischangeDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRemoteProbe`(IN _id tinytext , IN _locationID tinytext , IN _measurementTypeID tinytext , IN _remoteProbeAddress TINYTEXT , IN _gtUCLCommand varchar(100) , IN _ltLCLCommand varchar(100) , IN _createdBy TINYTEXT , IN _createDate DATETIME , IN _changedBy TINYTEXT , IN _changeDate DATETIME , IN _isActive BOOL)
BEGIN
	INSERT INTO RemoteProbe
		( id
		  , locationID
		  , measurementTypeID
		  , remoteProbeAddress
		  , createdBy
		  , createDate
		  , changedBy
		  , changeDate
		  , isActive
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , unhex(replace(_locationID ,'-',''))
		  , unhex(replace(_measurementTypeID ,'-',''))
		  , _remoteProbeAddress
		  , _gtUCLCommand
		  , _ltLCLCommand
		  , _createdBy
		  , _createDate
		  , _changedBy
		  , _changeDate
		  , _isActive
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRPIHeartbeat`( _id tinytext, _macAddress VARCHAR(17), _hostname VARCHAR(100), _cpuUsage DECIMAL(10, 2), _memoryUsage DECIMAL(10, 2), _diskUsage DECIMAL(10, 2), _uptimeMillis bigint(20) unsigned, _temperature DECIMAL(10, 2), _loadAverage DECIMAL(10, 2), _voltage DECIMAL(10, 2), _createdBy VARCHAR(100), _createDate DATETIME )
BEGIN
	INSERT INTO RPIHeartbeat
		( id
		  , macAddress
		  , hostname
		  , cpuUsage
		  , memoryUsage
		  , diskUsage
		  , uptimeMillis
		  , temperature
		  , loadAverage
		  , voltage
		  , createdBy
		  , createDate
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , _macAddress
		  , _hostname
		  , _cpuUsage
		  , _memoryUsage
		  , _diskUsage
		  , _uptimeMillis
		  , _temperature
		  , _loadAverage
		  , _voltage
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRPIHello`( _id tinytext, _macAddress VARCHAR(17), _hostname VARCHAR(100), _osName VARCHAR(100), _osVersion VARCHAR(100), _bootloaderFirmwareVersion VARCHAR(100), _cpuModel VARCHAR(100), _cpuCores tinyint, _cpuTemperature decimal(10,2), _cpuSerialNumber varchar(100), _totalRAM bigint(20) unsigned, _totalDiskSpace bigint(20) unsigned, _totalUsedDiskSpace bigint(20) unsigned, _uptimeMillis bigint(20) unsigned, _firstBootDateTime datetime, _createdBy varchar(100), _createDate datetime)
BEGIN
	INSERT INTO RPIHello
		( id
		  , macAddress
		  , hostname
		  , osName
		  , osVersion
		  , bootloaderFirmwareVersion
		  , cpuModel
		  , cpuCores
		  , cpuTemperature
		  , cpuSerialNumber
		  , totalRAM
		  , totalDiskSpace
		  , totalUsedDiskSpace
		  , uptimeMillis
		  , firstBootDateTime
		  , createdBy
		  , createDate
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , _macAddress
		  , _hostname
		  , _osName
		  , _osVersion
		  , _bootloaderFirmwareVersion
		  , _cpuModel
		  , _cpuCores
		  , _cpuTemperature
		  , _cpuSerialNumber
		  , _totalRAM
		  , _totalDiskSpace
		  , _totalUsedDiskSpace
		  , _uptimeMillis
		  , _firstBootDateTime
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRPIServiceYargHeartbeat`( _id tinytext, _rpiHeartbeatID tinytext, _yargAppCurrentTasks smallint, _yargAppTaskLimit smallint, _yargAppCpuCount varchar(100), _yargAppStatus varchar(100), _createdBy varchar(100), _createDate datetime)
BEGIN
	INSERT INTO RPIServiceYargHeartbeat
		( id
		  , rpiHeartbeatID
		  , yargAppCurrentTasks
		  , yargAppTaskLimit
		  , yargAppCpuCount
		  , yargAppStatus
		  , createdBy
		  , createDate
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , unhex(replace(_rpiHeartbeatID,'-',''))
		  , _yargAppCurrentTasks
		  , _yargAppTaskLimit
		  , _yargAppCpuCount
		  , _yargAppStatus
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddRPIServiceYargHeartbeatPIHello`( _id tinytext, _rpiHeartbeatID tinytext, _yargAppCurrentTasks smallint, _yargAppTaskLimit smallint, _yargAppCpuCount varchar(100), _yargAppStatus varchar(100), _createdBy varchar(100), _createDate datetime)
BEGIN
	INSERT INTO RPIServiceYargHeartbeat
		( id
		  , rpiHeartbeatID
		  , yargAppCurrentTasks
		  , yargAppTaskLimit
		  , yargAppCpuCount
		  , yargAppStatus
		  , createdBy
		  , createDate
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , unhex(replace(_rpiHeartbeatID,'-',''))
		  , _yargAppCurrentTasks
		  , _yargAppTaskLimit
		  , _yargAppCpuCount
		  , _createdBy
		  , _createDate
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spAddWateringSchedule`(IN _id tinytext , IN _potID tinytext , IN _efStartTime datetime , IN _efDuration tinyint , IN _efAmount decimal(3,1) , IN _rollover tinyint )
BEGIN
	INSERT INTO WateringSchedule
		( id
		  , potID
		  , efStartTime
		  , efDuration
		  , efAmount
		  , rollover
		)
		VALUES
		( unhex(replace(_id,'-',''))
		  , unhex(replace(_potID,'-',''))
		  , _efStartTime
		  , _efDuration
		  , _efAmount
		  , _rollover
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spCompleteMixingFanSchedule`(IN mixingFanScheduleID tinytext , IN remoteHostName TINYTEXT , IN ackDate datetime)
BEGIN
	update
		MixingFanSchedule
	set isCompleted  = TRUE
	  , completeDate = now()
	  , changedBy    = remoteHostName
	  , changeDate   = ackDate
	where
		id = unhex(replace(mixingFanScheduleID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spCompleteWateringSchedule`(IN wateringScheduleID tinytext , IN remoteHostName TINYTEXT , IN ackDate datetime)
BEGIN
	update
		WateringSchedule
	set isCompleted  = TRUE
	  , completeDate = now()
	  , changedBy    = remoteHostName
	  , changeDate   = ackDate
	where
		id = unhex(replace(wateringScheduleID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spCreateFertigationEventRecord`(IN _commandID tinytext , IN _FEDate DATETIME)
BEGIN
	INSERT INTO FertigationEventRecord
		( commandID
		  , feDate
		)
		VALUES
		( unhex(replace(_commandID,'-',''))
		  , now(3)
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spCurrentIrrigationCalcs`()
BEGIN
	select
		gs.name                                                                        as growName
	  , gs.startDate                                                                   as growStartDate
	  , GREATEST(0,DATEDIFF(NOW(), gs.startDate) + 1)                                  AS growDay
	  , GREATEST(1,FLOOR(DATEDIFF(DATE_SUB(NOW(), INTERVAL 1 DAY), gs.startDate) / 7)) as growWeekYesterday
	  , GREATEST(1,FLOOR(DATEDIFF(NOW(), gs.startDate)                           / 7)) AS growWeek
	  , CASE
			WHEN (
					NOW() BETWEEN addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)) AND date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour)
				)
				THEN addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime))
			WHEN (
					NOW() BETWEEN addtime(date(now()),time(gs.sunriseTime)) AND date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
				)
				THEN addtime(date(now()),time(gs.sunriseTime))
			WHEN (
					NOW() BETWEEN date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour) and addtime(date(now()),time(gs.sunriseTime))
				)
				THEN addtime(date(now()),time(gs.sunriseTime))
				else addtime(cast(current_timestamp() + interval 1 day as date), cast(gs.sunriseTime as time))
		end as 'sunrise'
	  , CASE
			WHEN (
					NOW() BETWEEN addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)) AND date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour)
				)
				THEN date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour)
			WHEN (
					NOW() BETWEEN addtime(date(now()),time(gs.sunriseTime)) AND date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
				)
				THEN date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
			WHEN (
					NOW() BETWEEN date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour) and addtime(date(now()),time(gs.sunriseTime))
				)
				THEN date_add(addtime(                                    date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
				else addtime(cast(current_timestamp() + interval 1 day as date), cast(gs.sunriseTime as time)) + interval lcT.daylightHours hour
		end as 'sunset'
		-- , CalculateSunriseSunset(NOW(), time(gs.sunriseTime), lcY.daylightHours, lc.daylightHours) AS sunrise
		-- , CalculateSunriseSunset(NOW(), time(gs.sunriseTime), lcY.daylightHours, lc.daylightHours) + INTERVAL lcT.daylightHours HOUR AS sunset
	  , CASE
			WHEN (
					NOW() BETWEEN addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)) AND date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour)
				)
				or (
					NOW() BETWEEN addtime(date(now()),time(gs.sunriseTime)) AND date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
				)
				THEN 1
				else 0
		END                                                                                                           as isDay
	  , addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime))                                           as sunriseYesterday
	  , date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour) as sunsetYesterday
	  , addtime(date(now()),time(gs.sunriseTime))                                                                     as sunriseToday
	  , date_add(addtime(                                    date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)                           as sunsetToday
	  , addtime(cast(current_timestamp() + interval 1 day as date), cast(gs.sunriseTime as time))                                                        AS sunriseTomorrow
	  , addtime(cast(current_timestamp() + interval 1 day as date), cast(gs.sunriseTime as time)) + interval lcT.daylightHours hour                      AS sunsetTomorrow
	  , (
			select
				m.measuredValue
			from
				Measurement m
				join
					Location l
					on
						m.locationID = l.id
				join
					MeasurementType mt
					on
						mt.id = m.measurementTypeID
			where
				l.name         = "Habitat"
				and mt.name like "Temp%"
			order by
				m.createDate desc limit 1
		)
		as growRoomTemp
	  , (
			select
				m.measuredValue
			from
				Measurement m
				join
					Location l
					on
						m.locationID = l.id
				join
					MeasurementType mt
					on
						mt.id = m.measurementTypeID
			where
				l.name         = "Habitat"
				and mt.name like "Humidity%"
			order by
				m.createDate desc limit 1
		)
		as growRoomHumidity
	  , (
			select
				m.measuredValue
			from
				Measurement m
				join
					Location l
					on
						m.locationID = l.id
				join
					MeasurementType mt
					on
						mt.id = m.measurementTypeID
			where
				l.name      = "Reservoir"
				and mt.name ="Temperature"
			order by
				m.createDate desc limit 1
		)
		as reservoirTemp
	  , (
			select
				m.measuredValue
			from
				Measurement m
				join
					Location l
					on
						m.locationID = l.id
				join
					MeasurementType mt
					on
						mt.id = m.measurementTypeID
			where
				l.name      = "Reservoir"
				and mt.name ="Weight"
			order by
				m.createDate desc limit 1
		)
		as reservoirVolume
	  , gs.sunriseTime
	  , rs.irrigationEventsPerDay
	  , rs.soakTime
	  , rs.isMorningSip
	  , rs.isEveningSip
	  , lc.daylightHours as daylightPerDay
	  , hex(r.id)        as recipeID
	  , r.name           as recipeName
	  , c.name           as cropName
	  , hex(gs.id)       as growSeasonID
	  , CASE
			WHEN (
					NOW() BETWEEN addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)) AND date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)),time(gs.sunriseTime)),INTERVAL lcY.daylightHours hour)
				)
				or (
					NOW() BETWEEN addtime(date(now()),time(gs.sunriseTime)) AND date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL lc.daylightHours hour)
				)
				THEN
				(
					select
						case
							when NOW() BETWEEN ws.efStartTime and ws.efEndTime
								then 'Flooding'
							when TIMESTAMPDIFF(MINUTE,NOW(),ws.efStartTime) >= 7
								then 'Sunny'
							when TIMESTAMPDIFF(MINUTE,NOW(),ws.efStartTime) BETWEEN 4 and 7
								then 'Cloudy'
							when TIMESTAMPDIFF(MINUTE,NOW(),ws.efStartTime) < 4
								then 'Chance of showers'
						end
					from
						vwWateringSchedule ws
					where
						ws.efStartTime  >= NOW()
						or ws.efEndTime >= NOW()
					order by
						ws.efStartTime limit 1
				)
				ELSE 'Nighttime'
		END AS weatherText
	from
		GrowSeason gs
		join
			Crop c
			on
				gs.cropID = c.id
		join
			Recipe r
			on
				gs.recipeID = r.id
		join
			RecipeStep rs
			on
				rs.recipeID       = r.id
				and rs.weekNumber = greatest(1,FLOOR(DATEDIFF(NOW(), gs.startDate) / 7))
		join
			LightCycle lc
			on
				rs.lightCycleID = lc.id
		join
			RecipeStep rsY
			on
				rsY.recipeID       = r.id
				and rsY.weekNumber = GREATEST(1, FLOOR(DATEDIFF(DATE_SUB(NOW(), INTERVAL 1 DAY), gs.startDate) / 7))
		join
			LightCycle lcY
			on
				rsY.lightCycleID = lcY.id
		join
			RecipeStep rsT
			on
				rsT.recipeID       = r.id
				and rsT.weekNumber = greatest(1, floor((to_days(current_timestamp() + interval 1 day) - to_days(gs.startDate)) / 7))
		join
			LightCycle lcT
			on
				rsT.lightCycleID = lcT.id
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteAllWateringSchedules`()
BEGIN
	DELETE
	from
		WateringSchedule
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteByLogin`(IN thisid int , IN loginProvider varchar(256) , IN providerKey varchar(256) )
BEGIN
	DELETE
	FROM
		ApplicationExternalLogin
	WHERE
		User_Id            = thisid
		and Login_Provider = loginProvider
		and Provider_Key   = providerKey
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteChemical`(IN thisid tinytext)
BEGIN
	DELETE
	from
		Chemical
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteChemicalType`(IN thisid tinytext)
BEGIN
	DELETE
	from
		ChemicalType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteCrop`(IN thisid tinytext)
BEGIN
	DELETE
	from
		Crop
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteFeedingChartType`(IN thisid tinytext)
BEGIN
	DELETE
	from
		FeedingChartType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteGrowSeason`(IN thisid tinytext)
BEGIN
	DELETE
	from
		GrowSeason
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteJar`(IN thisid tinytext)
BEGIN
	delete
	from
		Jar
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteLightCycle`(IN thisid tinytext)
BEGIN
	DELETE
	from
		LightCycle
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteLimit`(IN thisid tinytext)
BEGIN
	DELETE
	from
		`Limit`
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteLimitType`(IN thisid tinytext)
BEGIN
	DELETE
	from
		LimitType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteLocation`(IN thisid tinytext)
BEGIN
	DELETE
	from
		Location
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteMeasurement`(IN thisid tinytext)
BEGIN
	DELETE
	from
		Measurement
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteMeasurementType`(IN thisid tinytext)
BEGIN
	DELETE
	from
		MeasurementType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteMixingFanScheduleByJarID`(IN thisid tinytext)
BEGIN
	delete
	from
		MixingFanSchedule
	where
		jarID = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteMixingFanSchedulesByJarID`(IN jarID tinytext)
BEGIN
	DELETE
	from
		MixingFanSchedule
	where
		id = unhex(replace(jarID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeletePot`(IN thisid tinytext)
BEGIN
	DELETE
	from
		Pot
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeletePotsFromWateringSchedule`(IN thisid tinytext)
BEGIN
	delete
	from
		WateringSchedule
	where
		potID = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeletePump`(IN _id tinytext)
BEGIN
	DELETE
	FROM
		Pump
	where
		id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteRecipe`(IN thisid tinytext)
BEGIN
	delete
		rsl.*
	from
		RecipeStepLimit rsl
	WHERE
		rsl.recipeStepID IN
		(
			SELECT
				id
			FROM
				RecipeStep
			where
				recipeID = unhex(replace(thisid,'-',''))
		)
	;
	
	delete
		rsa.*
	from
		RecipeStepAmount rsa
	WHERE
		rsa.recipeStepID IN
		(
			SELECT
				id
			FROM
				RecipeStep
			where
				recipeID = unhex(replace(thisid,'-',''))
		)
	;
	
	delete
	from
		RecipeStep
	WHERE
		recipeID = unhex(replace(thisid,'-',''))
	;
	
	delete
	from
		RecipeChemList
	WHERE
		recipeID = unhex(replace(thisid,'-',''))
	;
	
	delete
	from
		Recipe
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteRecipeStep`(IN thisid tinytext)
BEGIN
	delete
	from
		RecipeStep
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteRecipeStepAmount`(IN thisid tinytext)
BEGIN
	delete
	from
		RecipeStepAmount
	where
		recipeStepID = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteRecipeStepLimit`(IN thisid tinytext)
BEGIN
	delete
	from
		RecipeStepLimit
	where
		recipeStepID = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteRemoteProbe`(IN thisid tinytext)
BEGIN
	DELETE
	from
		RemoteProbe
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spDeleteWateringSchedule`(IN thisid tinytext)
BEGIN
	DELETE
	from
		WateringSchedule
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spFixPotQueuePositions`()
BEGIN
	update
		Pot as a
	  , (
			SELECT
				@a := 0
		)
		as b
	SET a.queuePosition = @a:=@a+1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetAppStatus`()
BEGIN
	select
		ryh.yargAppStatus as status
	from
		RPIServiceYargHeartbeat ryh
	order by
		createDate desc limit 1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetChemicalByID`(IN thisid tinytext)
BEGIN
	SELECT
		hex(c.id) as 'id'
	  , c.name
	  , c.manufacturer
	  , hex(c.chemicalTypeID) as 'chemicalTypeID'
	  , ct.name               as 'chemicalTypeName'
	  , c.mixPriority
	  , c.mixTime
	  , c.pricePerL
	  , c.inStockAmount
	  , c.minReorderPoint
	  , c.createdBy
	  , c.createDate
	  , c.changedBy
	  , c.changeDate
	  , c.isActive
	FROM
		Chemical c
		join
			ChemicalType ct
			on
				c.chemicalTypeID = ct.id
	where
		c.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetChemicals`()
BEGIN
	SELECT
		hex(c.id) as 'id'
	  , c.name
	  , c.manufacturer
	  , hex(c.chemicalTypeID) as 'chemicalTypeID'
	  , ct.name               as 'chemicalTypeName'
	  , c.mixPriority
	  , c.mixTime
	  , c.pricePerL
	  , c.inStockAmount
	  , c.minReorderPoint
	  , c.createdBy
	  , c.createDate
	  , c.changedBy
	  , c.changeDate
	  , c.isActive
	FROM
		Chemical c
		join
			ChemicalType ct
			on
				c.chemicalTypeID = ct.id
	ORDER BY
		ct.sorting
	  , c.name
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetChemicalTypeByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , isH2O2
	  , isPhUp
	  , isPhDown
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	FROM
		ChemicalType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetChemicalTypes`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , isH2O2
	  , isPhUp
	  , isPhDown
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	FROM
		ChemicalType
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetCropByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		Crop
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetCrops`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		Crop
	order by
		name
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetCurrentMixingFanSchedules`(IN dtTheDate DATETIME)
BEGIN
	update
		MixingFanSchedule
	set isAcknowledged  = FALSE
	  , acknowledgeDate = NULL
	  , isCompleted     = FALSE
	  , completeDate    = NULL
	  , changedBy       = 'HITMAN'
	  , changeDate      = NOW()
	where
		addtime(date(dtTheDate),DATE_FORMAT(dtTheDate, '%H:%i')) = addtime(date(dtTheDate),time(mfStartTime))
	;
	
	select
		hex(mfs.id)      as 'id'
	  , hex(mfs.jarID)   as 'jarID'
	  , c.name           as 'jarChemicalName'
	  , j.mixFanPosition as 'position'
	  , CASE
			WHEN (
					j.currentAmount / j.capacity
				)
				>= 0.75
				then 254
				else 254 - ((j.currentAmount / j.capacity) * 254 * 0.25)
		END as 'pumpSpeed'
	  , j.mixFanOverSpeed
	  , addtime(date(now()),time(mfs.mfStartTime))                                          as 'mfStartTime'
	  , date_add(addtime(date(now()),time(mfs.mfStartTime)),interval mfs.mfDuration minute) as 'mfEndTime'
	  , mfs.mfDuration
	  , mfs.isAcknowledged
	  , mfs.isCompleted
	  , mfs.isErrorState
	  , mfs.createdBy
	  , mfs.createDate
	  , mfs.changedBy
	  , mfs.changeDate
	  , mfs.isActive
	from
		MixingFanSchedule mfs
		join
			Jar j
			on
				mfs.jarID = j.id
		join
			Chemical c
			on
				j.chemicalID = c.id
	where
		addtime(date(dtTheDate),DATE_FORMAT(dtTheDate, '%H:%i')) = addtime(date(dtTheDate),time(mfs.mfStartTime))
	order by
		j.mixFanPosition
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetCurrentWateringScheduleCommand`(IN _date DATETIME)
BEGIN
	select
		vws.id               as 'commandID'
	  , vws.potID            as 'potID'
	  , vws.potQueuePosition as 'potNumber'
	  , vws.ebbSpeed         as 'ebbSpeed'
	  , vws.ebbAmount        as 'ebbAmount'
	  , vws.ebbAntiShockRamp as 'ebbAntiShockRamp'
	  , vws.ebbExpectedFlowRate 'ebbExpectedFlowRate'
	  , vws.ebbPumpFlowErrorThreshold  as 'ebbPumpErrorThreshold'
	  , vws.ebbPulsesPerLiter          as 'ebbPulsesPerLiter'
	  , vws.efDuration * 60 * 1000     as 'soakDuration'
	  , vws.flowSpeed                  as 'flowSpeed'
	  , vws.flowAntiShockRamp          as 'flowAntiShockRamp'
	  , vws.flowExpectedFlowRate       as 'flowExpectedFlowRate'
	  , vws.flowPumpFlowErrorThreshold as 'flowPumpErrorThreshold'
	  , vws.flowPulsesPerLiter         as 'flowPulsesPerLiter'
	from
		vwWateringSchedule vws
	where
		DATE_FORMAT(_date, '%Y-%m-%d %H:%i:00') = vws.efStartTime
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetCurrentWateringScheduleID`(IN dtTheDate DATETIME)
BEGIN
	select
		hex(ws.id) as 'id'
	from
		WateringSchedule ws
	where
		addtime(date(dtTheDate),DATE_FORMAT(dtTheDate, '%H:%i')) = date_add(addtime(date(dtTheDate),time(ws.efStartTime)),interval ws.rollover day)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetFeedingChartTypeByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		FeedingChartType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetFeedingChartTypes`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		FeedingChartType
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetGrowSeasonByID`(IN thisid tinytext)
BEGIN
	select
		hex(g.id) as 'id'
	  , g.name
	  , g.startDate
	  , g.endDate
	  , addtime(date(now()),time(g.sunriseTime)) as 'sunriseTime'
	  , hex(g.cropID)                            as 'cropID'
	  , c.name                                   as 'cropName'
	  , r.name                                   as 'recipeName'
	  , g.isComplete
	  , g.createdBy
	  , g.createDate
	  , g.changedBy
	  , g.changeDate
	  , g.isActive
	from
		GrowSeason g
		join
			Crop c
			on
				g.cropID = c.id
		join
			Recipe r
			on
				g.recipeID = r.id
	where
		g.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetGrowSeasons`()
BEGIN
	select
		hex(g.id) as 'id'
	  , g.name
	  , g.startDate
	  , g.endDate
	  , addtime(date(now()),time(g.sunriseTime)) as 'sunriseTime'
	  , hex(g.cropID)                            as 'cropID'
	  , c.name                                   as 'cropName'
	  , g.isComplete
	  , g.createdBy
	  , g.createDate
	  , g.changedBy
	  , g.changeDate
	  , g.isActive
	from
		GrowSeason g
		join
			Crop c
			on
				g.cropID = c.id
	order by
		g.startDate
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetJarByID`(IN thisid tinytext)
BEGIN
	select
		hex(j.id) as 'id'
	  , j.mixFanPosition
	  , hex(j.chemicalID) as 'chemicalID'
	  , c.name            as 'chemicalName'
	  , j.mixTimesPerDay
	  , j.duration
	  , j.capacity
	  , j.currentAmount
	  , j.mixFanOverSpeed
	  , j.createdBy
	  , j.createDate
	  , j.changedBy
	  , j.changeDate
	  , j.isActive
	FROM
		Jar j
		join
			Chemical c
			on
				j.chemicalID = c.id
	where
		j.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetJars`()
BEGIN
	select
		hex(j.id) as 'id'
	  , j.mixFanPosition
	  , hex(j.chemicalID) as 'chemicalID'
	  , c.name            as 'chemicalName'
	  , j.mixTimesPerDay
	  , j.duration
	  , j.capacity
	  , j.currentAmount
	  , j.mixFanOverSpeed
	  , j.createdBy
	  , j.createDate
	  , j.changedBy
	  , j.changeDate
	  , j.isActive
	FROM
		Jar j
		join
			Chemical c
			on
				j.chemicalID = c.id
	order by
		j.mixFanPosition
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLastRecipeStepAmountValue`(IN thisrecipeID tinytext , IN thischemicalID tinytext)
BEGIN
	select
		rsa.amount
	from
		RecipeStepAmount rsa
		join
			RecipeStep rs
			on
				rsa.recipeStepID = rs.id
	where
		rs.recipeID        = unhex(replace(thisrecipeID,'-',''))
		and rsa.chemicalID = unhex(replace(thischemicalID,'-',''))
	order by
		rs.weekNumber desc limit 1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLastRecipeStepByRecipeID`(IN thisid tinytext)
BEGIN
	SELECT
		weekNumber
	  , hex(lightCycleID) as 'lightCycleID'
	  , irrigationEventsPerDay
	  , soakTime
	  , isMorningSip
	  , isEveningSip
	FROM
		RecipeStep
	where
		recipeID = unhex(replace(thisid,'-',''))
	order by
		weekNumber DESC limit 1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLastRecipeStepLimitValue`(IN thisrecipeID tinytext , IN thislocationTypeID tinytext , IN thismeasurementTypeID tinytext , IN thislimitTypeID tinytext)
BEGIN
	select
		rsl.value
	from
		RecipeStepLimit rsl
		join
			RecipeStep rs
			on
				rsl.recipeStepID = rs.id
	where
		rs.recipeID               = unhex(replace(thisrecipeID,'-',''))
		and rsl.locationID        = unhex(replace(thislocationTypeID,'-',''))
		and rsl.measurementTypeID = unhex(replace(thismeasurementTypeID,'-',''))
		and rsl.limitTypeID       = unhex(replace(thislimitTypeID,'-',''))
	order by
		rs.weekNumber desc limit 1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLCL`(IN _weekNumber tinyint(4) , IN _recipeID tinytext , IN _locationID tinytext , IN _measurementTypeID tinytext)
BEGIN
	SELECT
		IFNULL(rsl.value, 0) AS 'value'
	FROM
		(
			SELECT
				rs.id
			FROM
				RecipeStep rs
			WHERE
				rs.weekNumber   = 7
				AND rs.recipeID = UNHEX(REPLACE(_recipeID, '-', ''))
		)
		AS subquery
		LEFT JOIN
			RecipeStepLimit rsl
			ON
				rsl.recipeStepID          = subquery.id
				AND rsl.locationID        = UNHEX(REPLACE(_locationID, '-', ''))
				AND rsl.measurementTypeID = UNHEX(REPLACE(_measurementTypeID, '-', ''))
				AND rsl.limitTypeID       = UNHEX(REPLACE('3a03a0f4-fe17-340e-a311-f553d5cef030', '-', ''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLightCycleByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , daylightHours
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		LightCycle
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLightCycles`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , daylightHours
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		LightCycle
	order by
		name
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLimitById`(IN thisid tinytext)
BEGIN
	SELECT
		hex(l.id)                as 'id'
	  , hex(l.parentId)          as 'parentId'
	  , hex(l.measurementTypeId) as 'measurementTypeId'
	  , mt.name                  as 'measurementTypeName'
	  , hex(l.limitTypeId)       as 'limitTypeId'
	  , lt.name                  as 'limitTypeName'
	  , l.limitValue
	  , l.createdBy
	  , l.createDate
	  , l.changedBy
	  , l.changeDate
	FROM
		`Limit` l
		join
			MeasurementType mt
			on
				l.measurementTypeId = mt.id
		join
			LimitType lt
			on
				l.limitTypeId = lt.id
	where
		l.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLimitsByParentId`(IN thisid tinytext)
BEGIN
	SELECT
		hex(l.id)                as 'id'
	  , hex(l.parentId)          as 'parentId'
	  , hex(l.measurementTypeId) as 'measurementTypeId'
	  , mt.name                  as 'measurementTypeName'
	  , hex(l.limitTypeId)       as 'limitTypeId'
	  , lt.name                  as 'limitTypeName'
	  , l.limitValue
	  , l.createdBy
	  , l.createDate
	  , l.changedBy
	  , l.changeDate
	FROM
		`Limit` l
		join
			MeasurementType mt
			on
				l.measurementTypeId = mt.id
		join
			LimitType lt
			on
				l.limitTypeId = lt.id
	where
		l.parentId = unhex(replace(thisid,'-',''))
	order by
		mt.sorting
	  , lt.sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLimitTypeById`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		LimitType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLimitTypes`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		LimitType
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLocationByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , isShowOnLandingPage
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		Location
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLocationByRemoteProbeAddress`(IN thisremoteProbeAddress tinytext)
BEGIN
	SELECT
		hex(locationID) as 'locationID'
	FROM
		RemoteProbe
	where
		remoteProbeAddress = thisremoteProbeAddress
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLocations`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , isShowOnLandingPage
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		Location
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetLocationsForRecipe`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	from
		Location
	where
		isShowOnLandingPage = true
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMeasurementByID`(IN thisid tinytext)
BEGIN
	SELECT
		hex(m.id)                as 'id'
	  , hex(m.growSeasonID)      as 'growSeasonID'
	  , hex(m.locationID)        as 'locationID'
	  , l.name                   as 'locationName'
	  , hex(m.measurementTypeID) as 'measurementTypeID'
	  , mt.name                  as 'measurementTypeName'
	  , m.measuredValue
	  , m.createdBy
	  , m.createDate
	from
		Measurement m
		join
			Location l
			on
				m.locationID = l.id
		join
			MeasurementType mt
			on
				m.measurementTypeID = mt.id
	where
		m.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMeasurements`()
BEGIN
	SELECT
		hex(m.id)                as 'id'
	  , hex(m.growSeasonID)      as 'growSeasonID'
	  , hex(m.locationID)        as 'locationID'
	  , l.name                   as 'locationName'
	  , hex(m.measurementTypeID) as 'measurementTypeID'
	  , mt.name                  as 'measurementTypeName'
	  , m.measuredValue
	  , m.createdBy
	  , m.createDate
	from
		Measurement m
		join
			Location l
			on
				m.locationID = l.id
		join
			MeasurementType mt
			on
				m.measurementTypeID = mt.id
	order by
		m.createDate
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMeasurementTypeByID`(IN thisid tinytext)
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		MeasurementType
	where
		id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMeasurementTypeByRemoteProbeAddress`(IN thisremoteProbeAddress tinytext)
BEGIN
	SELECT
		hex(measurementTypeID) as 'measurementTypeID'
	FROM
		RemoteProbe
	where
		remoteProbeAddress = thisremoteProbeAddress
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMeasurementTypes`()
BEGIN
	select
		hex(id) as 'id'
	  , name
	  , sorting
	  , createdBy
	  , createDate
	  , changedBy
	  , changeDate
	  , isActive
	from
		MeasurementType
	order by
		sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMixingFanScheduleByID`(IN thisid tinytext)
BEGIN
	select
		hex(mfs.id)                                          as 'id'
	  , hex(mfs.jarID)                                       as 'jarID'
	  , c.name                                               as 'jarChemicalName'
	  , j.mixFanPosition                                     as 'position'
	  , CONVERT(j.currentAmount / j.capacity * 254, INTEGER) as 'pumpSpeed'
	  , j.mixFanOverSpeed
	  , addtime(date(now()),time(mfs.mfStartTime))                                          as 'mfStartTime'
	  , date_add(addtime(date(now()),time(mfs.mfStartTime)),interval mfs.mfDuration minute) as 'mfEndTime'
	  , mfs.mfDuration
	  , mfs.isAcknowledged
	  , mfs.isCompleted
	  , mfs.isErrorState
	  , mfs.createdBy
	  , mfs.createDate
	  , mfs.changedBy
	  , mfs.changeDate
	  , mfs.isActive
	from
		MixingFanSchedule mfs
		join
			Jar j
			on
				mfs.jarID = j.id
		join
			Chemical c
			on
				j.chemicalID = c.id
	where
		mfs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMixingFanSchedules`()
BEGIN
	select
		hex(mfs.id)                                                                         as 'id'
	  , hex(mfs.jarID)                                                                      as 'jarID'
	  , c.name                                                                              as 'jarChemicalName'
	  , j.mixFanPosition                                                                    as 'position'
	  , addtime(date(now()),time(mfs.mfStartTime))                                          as 'mfStartTime'
	  , date_add(addtime(date(now()),time(mfs.mfStartTime)),interval mfs.mfDuration minute) as 'mfEndTime'
	  , mfs.mfDuration
	  , mfs.isAcknowledged
	  , mfs.isCompleted
	  , mfs.isErrorState
	  , mfs.createdBy
	  , mfs.createDate
	  , mfs.changedBy
	  , mfs.changeDate
	  , mfs.isActive
	from
		MixingFanSchedule mfs
		join
			Jar j
			on
				mfs.jarID = j.id
		join
			Chemical c
			on
				j.chemicalID = c.id
	order by
		mfs.mfStartTime
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetMixingFanSpeedFromJarPosition`(IN thisJarPosition tinyint)
BEGIN
	SELECT
		convert(currentAmount / capacity * 254, SIGNED) as 'mixingFanSpeed'
	FROM
		Jar
	where
		mixFanPosition = thisJarPosition
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetNextPotQueuePosition`()
BEGIN
	SELECT
		CAST(IFNULL(
		(
			SELECT
				MAX(queuePosition) + 1
			FROM
				Pot
		)
		, 1) AS SIGNED) AS queuePos
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetPotByID`(IN _id tinytext)
BEGIN
	select
		hex(p.id) as 'id'
	  , p.name
	  , p.queuePosition
	  , p.totalCapacity
	  , p.speed
	  , p.currentCapacity
	  , p.antiShockRamp
	  , p.expectedFlowRate
	  , p.pumpFlowErrorThreshold
	  , p.pulsesPerLiter
	  , p.isReservoir
	  , p.createdBy
	  , p.createDate
	  , p.changedBy
	  , p.changeDate
	  , p.isActive
	from
		Pot p
	where
		id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetPots`()
BEGIN
	select
		hex(p.id) as 'id'
	  , p.name
	  , p.queuePosition
	  , p.totalCapacity
	  , p.speed
	  , p.currentCapacity
	  , p.antiShockRamp
	  , p.expectedFlowRate
	  , p.pumpFlowErrorThreshold
	  , p.pulsesPerLiter
	  , p.createdBy
	  , p.isReservoir
	  , p.createDate
	  , p.changedBy
	  , p.changeDate
	  , p.isActive
	from
		Pot p
	where
		p.isReservoir = false
	order by
		p.queuePosition
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetPumps`()
BEGIN
	select
		hex(p.id)        as 'id'
	  , hex(p.potID)     as 'potID'
	  , p.make           as 'make'
	  , p.model          as 'model'
	  , p.serialNumber   as 'serialNumber'
	  , p.vendor         as 'vendor'
	  , p.price          as 'price'
	  , p.pulsesPerLiter as 'pulserPerLiter'
	  , p.purchaseDate   as 'purchaseDate'
	  , p.installDate    as 'installDate'
	  , p.createDate     as 'createDate'
	  , p.createdBy      as 'createdBy'
	  , p.changeDate     as 'changeDate'
	  , p.changedBy      as 'changedBy'
	  , p.isActive       as 'isActive'
	FROM
		Pump p
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeByID`(IN thisid tinytext)
BEGIN
	select
		hex(r.id)                 as 'id'
	  , hex(r.feedingChartTypeID) as 'feedingChartTypeID'
	  , fct.name                  as 'FeedingChartTypeName'
	  , r.weekNumber
	  , hex(r.lightCycleID) as 'lightCycleID'
	  , lc.name             as 'lightCycleName'
	  , hex(r.chemicalsID)  as 'chemicalsID'
	  , r.createdBy
	  , r.createDate
	  , r.changedBy
	  , r.changeDate
	from
		Recipe r
		join
			FeedingChartType fct
			on
				r.feedingChartTypeID = fct.id
		join
			LightCycle lc
			on
				r.lightCycleID = lc.id
	where
		r.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeChemListByRecipeID`(IN thisid tinytext)
BEGIN
	select
		hex(rcl.id)         as 'id'
	  , hex(rcl.recipeID)   as 'recipeID'
	  , hex(rcl.chemicalID) as 'chemicalID'
	  , c.name              as 'chemicalName'
	  , rcl.mixtime
	  , rcl.createdBy
	  , rcl.createDate
	  , rcl.changedBy
	  , rcl.changeDate
	from
		RecipeChemList rcl
		join
			Chemical c
			on
				rcl.chemicalID = c.id
		join
			ChemicalType ct
			on
				c.chemicalTypeID = ct.id
	where
		rcl.recipeID = unhex(replace(thisid,'-',''))
	order by
		ct.sorting
	  , c.name
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeChemListViewModels`()
BEGIN
	select
		hex(rcl.recipeID) as 'recipeID'
	  , hex(c.id)         as 'chemicalID'
	  , hex(ct.id)        as 'chemicalTypeID'
	  , ct.name           as 'chemicalTypeName'
	  , c.name            as 'chemicalName'
	  , c.mixPriority
	  , rcl.mixtime
	from
		RecipeChemList rcl
		join
			Chemical c
			on
				rcl.chemicalID = c.id
		join
			ChemicalType ct
			on
				c.chemicalTypeID = ct.id
	order by
		ct.sorting
	  , c.name
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipes`()
BEGIN
	select
		hex(r.id) as 'id'
	  , r.name
	  , r.createdBy
	  , r.createDate
	  , r.changedBy
	  , r.changeDate
	from
		Recipe r
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeStepAmountsByRecipeStepID`(IN thisid tinytext)
BEGIN
	select distinct
		(hex(rsa.id))         as 'id'
	  , hex(rsa.recipeStepID) as 'recipeStepID'
	  , rs.weekNumber
	  , hex(rsa.chemicalID) as 'chemicalID'
	  , c.name              as 'chemicalName'
	  , hex(ct.id)          as 'chemicalTypeID'
	  , rsa.amount
	  , rsa.createdBy
	  , rsa.createDate
	  , rsa.changedBy
	  , rsa.changeDate
	from
		RecipeStepAmount rsa
		join
			RecipeStep rs
			on
				rsa.recipeStepID = rs.id
		join
			RecipeChemList rcl
			on
				rs.recipeID = rcl.recipeID
		join
			Chemical c
			on
				rsa.chemicalID = c.id
		join
			ChemicalType ct
			on
				c.chemicalTypeID = ct.id
	where
		rsa.recipeStepID = unhex(replace(thisid,'-',''))
	order by
		rs.weekNumber
	  , c.mixPriority
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeStepLimitsByRecipeStepID`(IN thisid tinytext)
BEGIN
	select
		hex(rsl.id)                as 'id'
	  , hex(rsl.recipeStepID)      as 'recipeStepID'
	  , hex(rsl.locationID)        as 'locationID'
	  , l.name                     as 'locationName'
	  , l.sorting                  as 'locationSort'
	  , hex(rsl.measurementTypeID) as 'measurementTypeID'
	  , mt.name                    as 'measurementTypeName'
	  , mt.sorting                 as 'measurementSort'
	  , hex(rsl.limitTypeID)       as 'limitTypeID'
	  , lt.name                    as 'limitTypeName'
	  , lt.sorting                 as 'limitSort'
	  , rsl.value
	  , rsl.createdBy
	  , rsl.createDate
	  , rsl.changedBy
	  , rsl.changeDate
	from
		RecipeStepLimit rsl
		join
			RecipeStep rs
			on
				rsl.recipeStepID = rs.id
		join
			Location l
			on
				rsl.locationID = l.id
		join
			MeasurementType mt
			on
				rsl.measurementTypeID = mt.id
		join
			LimitType lt
			on
				rsl.limitTypeID = lt.id
	where
		rsl.recipeStepID = unhex(replace(thisid,'-',''))
	order by
		rs.weekNumber
	  , l.sorting
	  , mt.sorting
	  , lt.sorting
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRecipeStepsByRecipeID`(IN thisid tinytext)
BEGIN
	select
		hex(rs.id)       as 'id'
	  , hex(rs.recipeID) as 'recipeID'
	  , rs.weekNumber
	  , hex(rs.lightCycleID) as 'lightCycleID'
	  , lc.name              as 'lightCycleName'
	  , rs.irrigationEventsPerDay
	  , rs.soakTime
	  , rs.isMorningSip
	  , rs.isEveningSip
	  , rs.createdBy
	  , rs.createDate
	  , rs.changedBy
	  , rs.changeDate
	from
		RecipeStep rs
		join
			LightCycle lc
			on
				rs.lightCycleID = lc.id
	where
		rs.recipeID = unhex(replace(thisid,'-',''))
	ORDER by
		rs.weekNumber
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRemoteProbeByID`(IN _id tinytext)
BEGIN
	select
		hex(r.id)                as 'id'
	  , hex(r.locationID)        as 'locationID'
	  , l.name                   as 'locationName'
	  , hex(r.measurementTypeID) as 'measurementTypeID'
	  , mt.name                  as 'measurementTypeName'
	  , r.remoteProbeAddress
	  , r.ltLCLCommand
	  , r.gtUCLCommand
	  , r.createdBy
	  , r.createDate
	  , r.changedBy
	  , r.changeDate
	  , r.isActive
	from
		RemoteProbe r
		join
			Location l
			on
				r.locationID = l.id
		join
			MeasurementType mt
			on
				r.measurementTypeID = mt.id
	where
		r.id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetRemoteProbes`()
BEGIN
	select
		hex(r.id)                as 'id'
	  , hex(r.locationID)        as 'locationID'
	  , l.name                   as 'locationName'
	  , hex(r.measurementTypeID) as 'measurementTypeID'
	  , mt.name                  as 'measurementTypeName'
	  , r.remoteProbeAddress
	  , r.ltLCLCommand
	  , r.gtUCLCommand
	  , r.createdBy
	  , r.createDate
	  , r.changedBy
	  , r.changeDate
	  , r.isActive
	from
		RemoteProbe r
		join
			Location l
			on
				r.locationID = l.id
		join
			MeasurementType mt
			on
				r.measurementTypeID = mt.id
	order by
		r.remoteProbeAddress
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetSunriseToday`()
BEGIN
	select
		addtime(date(now()),time(gs.sunriseTime)) as 'sunriseToday'
	from
		GrowSeason gs
	where
		gs.isActive = true
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetSunsetToday`()
BEGIN
	select
		date_add(addtime(date(now()),time(gs.sunriseTime)),INTERVAL l.daylightHours hour) as 'sunsetToday'
	from
		GrowSeason gs
		join
			LightCycle l
			on
				gs.lightCycleID = l.id
	where
		gs.isActive = true
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetUCL`(IN _weekNumber tinyint(4) , IN _recipeID tinytext , IN _locationID tinytext , IN _measurementTypeID tinytext)
BEGIN
	SELECT
		IFNULL(rsl.value, 0) AS 'value'
	FROM
		(
			SELECT
				rs.id
			FROM
				RecipeStep rs
			WHERE
				rs.weekNumber   = 7
				AND rs.recipeID = UNHEX(REPLACE(_recipeID, '-', ''))
		)
		AS subquery
		LEFT JOIN
			RecipeStepLimit rsl
			ON
				rsl.recipeStepID          = subquery.id
				AND rsl.locationID        = UNHEX(REPLACE(_locationID, '-', ''))
				AND rsl.measurementTypeID = UNHEX(REPLACE(_measurementTypeID, '-', ''))
				and rsl.limitTypeID       = unhex(replace('3a03a0f4-fe17-340e-a311-f553d5cef030','-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetWateringScheduleByID`(IN thisid tinytext)
BEGIN
	select
		hex(ws.id)                                                                                                           as 'id'
	  , hex(ws.potID)                                                                                                        as 'potID'
	  , p.name                                                                                                               as 'potName'
	  , p.queuePosition                                                                                                      as 'potQueuePosition'
	  , date_add(addtime(date(now()),time(ws.efStartTime)),interval ws.rollover day)                                         as 'efStartTime'
	  , date_add(date_add(addtime(date(now()),time(ws.efStartTime)),interval ws.efDuration minute),interval ws.rollover day) as 'efEndTime'
	  , ws.efDuration
	  , ws.efAmount
	  , ws.isAcknowledged
	  , ws.isCompleted
	  , ws.isErrorState
	  , ws.createdBy
	  , ws.createDate
	  , ws.changedBy
	  , ws.changeDate
	  , ws.isActive
	from
		WateringSchedule ws
		join
			Pot p
			on
				ws.potID = p.id
	where
		ws.id = unhex(replace(thisid,'-',''))
	order by
		ws.efStartTime
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spGetWateringSchedules`()
BEGIN
	/*     DECLARE sunriseYesterdayTime DATETIME;
	DECLARE sunsetYesterdayTime DATETIME;
	DECLARE sunriseTodayTime DATETIME;
	DECLARE sunsetTodayTime DATETIME;
	-- Declare a handler for any potential exceptions
	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
	-- Handle exceptions here if needed
	END;
	-- Select and set the values for the variables
	SELECT
	addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)), time(gs.sunriseTime)) AS sunriseYesterday,
	date_add(addtime(date(DATE_SUB(NOW(), INTERVAL 1 DAY)), time(gs.sunriseTime)), INTERVAL lcY.daylightHours HOUR) AS sunsetYesterday,
	addtime(date(NOW()), time(gs.sunriseTime)) AS sunriseToday,
	date_add(addtime(date(NOW()), time(gs.sunriseTime)), INTERVAL lc.daylightHours HOUR) AS sunsetToday
	INTO
	sunriseYesterdayTime,
	sunsetYesterdayTime,
	sunriseTodayTime,
	sunsetTodayTime
	FROM
	GrowSeason gs
	JOIN
	Crop c ON gs.cropID = c.id
	JOIN
	Recipe r ON gs.recipeID = r.id
	JOIN
	RecipeStep rs ON rs.recipeID = r.id AND rs.weekNumber = GREATEST(1, FLOOR(DATEDIFF(NOW(), gs.startDate) / 7))
	JOIN
	LightCycle lc ON rs.lightCycleID = lc.id
	JOIN
	RecipeStep rsY ON rsY.recipeID = r.id AND rsY.weekNumber = GREATEST(1, FLOOR(DATEDIFF(DATE_SUB(NOW(), INTERVAL 1 DAY), gs.startDate) / 7))
	JOIN
	LightCycle lcY ON rsY.lightCycleID = lcY.id;
	select hex(ws.id) as 'id', hex(ws.potID) as 'potID', p.name  as 'potName', p.queuePosition as 'potQueuePosition',
	case when now() > sunriseTodayTime
	then date_add(addtime(date(sunriseTodayTime),time(ws.efStartTime)),interval ws.rollover day)
	else
	date_add(addtime(date(sunriseYesterdayTime),time(ws.efStartTime)),interval ws.rollover day)
	end as 'efStartTime',
	case when now() > sunriseTodayTime
	then date_add(date_add(addtime(date(sunriseTodayTime),time(ws.efStartTime)),interval ws.efDuration minute),interval ws.rollover day)
	else
	date_add(date_add(addtime(date(sunriseYesterdayTime),time(ws.efStartTime)),interval ws.efDuration minute),interval ws.rollover day)
	end as 'efEndTime',
	ws.efDuration ,
	ws.efAmount ,
	ws.isAcknowledged ,
	ws.isCompleted ,
	ws.isErrorState ,
	ws.createdBy , ws.createDate ,ws.changedBy ,ws.changeDate ,ws.isActive
	from WateringSchedule ws
	join Pot p on ws.potID = p.id
	order by ws.efStartTime;*/
	SELECT
		id
	  , potID
	  , potName
	  , potQueuePosition
	  , efStartTime
	  , efEndTime
	  , efDuration
	  , efAmount
	FROM
		vwWateringSchedule
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spIDofActiveGrowSeason`()
BEGIN
	select
		hex(id) as 'id'
	from
		GrowSeason gs
	WHERE
		isActive = true limit 1
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spPotCount`()
BEGIN
	select
		count(id) as 'cntPots'
	from
		Pot
	where
		isReservoir = 0
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateBackupInfo`(IN _lastBackupEndTime DATETIME)
BEGIN
	DECLARE existing_id datetime;
	SELECT
		lastBackupEndTime
	INTO
		existing_id
	FROM
		BackupInfo
	;
	
	IF existing_id IS NULL THEN
	-- If the record doesn't exist, insert a new record
	insert into BackupInfo
		(lastBackupEndTime
		)
		values
		(_lastBackupEndTime
		)
	;
	
	ELSE
	-- If the record exists, update the existing record
	update
		BackupInfo
	set lastBackupEndTime = _lastBackupEndTime
	;

END
IF;
END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateCAFEDateFertigationEventRecord`(IN _commandID TINYTEXT)
BEGIN
	UPDATE
		FertigationEventRecord f
	SET f.cafeDate = now(3)
	WHERE
		f.commandID = unhex(replace(_commandID,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateChemical`(IN thisid tinytext , IN thisname TINYTEXT , IN thismanufacturer TINYTEXT , IN thischemicalTypeID tinytext , IN thismixPriority tinyint(4) , IN thismixTime tinyint(4) , IN thispricePerL decimal(19,4) , IN thisinStockAmount decimal(19,4) , IN thisminReorderPoint tinyint(4) , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		Chemical c
	set c.name            = thisname
	  , c.manufacturer    = thismanufacturer
	  , c.chemicalTypeID  = unhex(replace(thischemicalTypeID,'-',''))
	  , c.mixPriority     = thismixPriority
	  , c.mixTime         = thismixTime
	  , c.pricePerL       = thispricePerL
	  , c.inStockAmount   = thisinStockAmount
	  , c.minReorderPoint = thisminReorderPoint
	  , c.changedBy       = thischangedBy
	  , c.changeDate      = thischangeDate
	  , c.isActive        = thisisActive
	where
		c.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateChemicalType`(IN thisid tinytext , IN thisname TINYTEXT , IN thisisH2O2 bool , IN thisisPhUp bool , IN thisisPhDown bool , IN thissorting TINYINT(4) , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		ChemicalType p
	SET p.name       = thisname
	  , p.isH2O2     = thisisH2O2
	  , p.isPhUp     = thisisPhUp
	  , p.isPhDown   = thisisPhDown
	  , p.sorting    = thissorting
	  , p.changedBy  = thischangedBy
	  , p.changeDate = thischangeDate
	  , p.isActive   = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateCrop`(IN thisid tinytext , IN thisname TINYTEXT , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		Crop p
	SET p.name       = thisname
	  , p.changedBy  = thischangedBy
	  , p.changeDate = thischangeDate
	  , p.isActive   = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateFeedingChartType`(IN thisid tinytext , IN thisname TINYTEXT , IN thissorting TINYINT , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		FeedingChartType f
	SET f.name       = thisname
	  , f.sorting    = thissorting
	  , f.changedBy  = thischangedBy
	  , f.changeDate = thischangeDate
	  , f.isActive   = thisisActive
	where
		f.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateFertigationEventRecord`( IN _commandID TINYTEXT, IN _CAFEDate DATETIME, IN _ebbPump_RunDate DATETIME, IN _ebbFlowmeter_DoneDate DATETIME, IN _ebbPump_DoneDate DATETIME, IN _flowPump_StartDate DATETIME, IN _flowPump_RunDate DATETIME, IN _flowFlowmeter_DoneDate DATETIME, IN _flowPump_DoneDate DATETIME, IN _isError bool, IN _errorDate DATETIME)
BEGIN
	IF _CAFEDate IS NOT NULL THEN
	UPDATE
		FertigationEventRecord f
	SET f.cafeDate = now(3)
	WHERE
		f.commandID          = unhex(replace(_commandID,'-',''))
		AND f.cafeDate IS NULL
	;

END
IF;
IF _ebbPump_RunDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.ebbPump_RunDate = now(3)
WHERE
	f.commandID                 = unhex(replace(_commandID,'-',''))
	AND f.ebbPump_RunDate IS NULL
;

END
IF;
IF _ebbFlowmeter_DoneDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.ebbFlowmeter_DoneDate = now(3)
WHERE
	f.commandID                       = unhex(replace(_commandID,'-',''))
	AND f.ebbFlowmeter_DoneDate IS NULL
;

END
IF;
IF _ebbPump_DoneDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.ebbPump_DoneDate = now(3)
WHERE
	f.commandID                  = unhex(replace(_commandID,'-',''))
	AND f.ebbPump_DoneDate IS NULL
;

END
IF;
IF _flowPump_StartDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.flowPump_StartDate = now(3)
WHERE
	f.commandID                    = unhex(replace(_commandID,'-',''))
	AND f.flowPump_StartDate IS NULL
;

END
IF;
IF _flowPump_RunDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.flowPump_RunDate = now(3)
WHERE
	f.commandID                  = unhex(replace(_commandID,'-',''))
	AND f.flowPump_RunDate IS NULL
;

END
IF;
IF _flowFlowmeter_DoneDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.flowFlowmeter_DoneDate = now(3)
WHERE
	f.commandID                        = unhex(replace(_commandID,'-',''))
	AND f.flowFlowmeter_DoneDate IS NULL
;

END
IF;
IF _flowPump_DoneDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.flowPump_DoneDate = now(3)
WHERE
	f.commandID                   = unhex(replace(_commandID,'-',''))
	AND f.flowPump_DoneDate IS NULL
;

END
IF;
IF _isError IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.isError = _isError
WHERE
	f.commandID         = unhex(replace(_commandID,'-',''))
	AND f.isError IS NULL
;

END
IF;
IF _errorDate IS NOT NULL THEN
UPDATE
	FertigationEventRecord f
SET f.errorDate = now(3)
WHERE
	f.commandID           = unhex(replace(_commandID,'-',''))
	AND f.errorDate IS NULL
;

END
IF;
END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateGrowSeason`(IN thisid tinytext , IN thisname TINYTEXT , in thisstartDate DATETIME , in thisendDate DATETIME , in thissunriseTime DATETIME , in thiscropID tinytext , in thisrecipeID tinytext , IN thisisComplete bool , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		GrowSeason p
	set p.name        = thisname
	  , p.startDate   = thisstartDate
	  , p.endDate     = thisendDate
	  , p.sunriseTime = thissunriseTime
	  , p.cropID      = unhex(replace(thiscropID ,'-',''))
	  , p.isComplete  = thisisComplete
	  , p.changedBy   = thischangedBy
	  , p.changeDate  = thischangeDate
	  , p.isActive    = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateHelloBot`(IN _expiryDate datetime , IN _changedBy TINYTEXT , IN _changeDate DATETIME)
BEGIN
	update
		BotHello
	set expiryDate = _expiryDate
	  , changeDate = _changeDate
	  , changedBy  = _changedBy
	where
		id =
		(
			select
				bh.id
			from
				BotHello bh
			where
				hostname = _changedBy
			order by
				createDate desc limit 1
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateHelloRPI`(IN _expiryDate datetime , IN _changedBy TINYTEXT , IN _changeDate DATETIME)
BEGIN
	update
		RPIHello
	set expiryDate = _expiryDate
	  , changeDate = _changeDate
	  , changedBy  = _changedBy
	where
		id =
		(
			select
				bh.id
			from
				RPIHello bh
			where
				hostname = _changedBy
			order by
				createDate desc limit 1
		)
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateJar`(IN thisid tinytext ,IN thismixFanPosition tinyint(4) ,IN thischemicalID tinytext ,IN thismixTimesPerDay tinyint(4) ,IN thisduration tinyint(4) ,IN thiscapacity decimal(19,4) ,IN thiscurrentAmount decimal(19,4) ,IN thismixFanOverSpeed int , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		Jar j
	set j.mixFanPosition  = thismixFanPosition
	  , j.chemicalID      = unhex(replace(thischemicalID,'-',''))
	  , j.mixTimesPerDay  = thismixTimesPerDay
	  , j.duration        = thisduration
	  , j.capacity        = thiscapacity
	  , j.currentAmount   = thiscurrentAmount
	  , j.mixFanOverSpeed = thismixFanOverSpeed
	  , j.changedBy       = thischangedBy
	  , j.changeDate      = thischangeDate
	  , j.isActive        = isActive
	where
		j.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateLightCycle`(IN thisid tinytext , IN thisname TINYTEXT , IN thisdaylightHours tinyint , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		LightCycle p
	SET p.name          = thisname
	  , p.daylightHours = thisdaylightHours
	  , p.changedBy     = thischangedBy
	  , p.changeDate    = thischangeDate
	  , p.isActive      = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateLimit`(IN thisid tinytext , IN thismeasurementTypeId tinytext , IN thislimitValue decimal(6,2) , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME )
BEGIN
	UPDATE
		`Limit` l
	set l.limitValue        = thislimitValue
	  , l.measurementTypeId = unhex(replace(measurementTypeId,'-',''))
	  , l.changedBy         = thischangedBy
	  , l.changeDate        = thischangeDate
	where
		l.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateLimitType`(IN thisid tinytext , IN thisname TINYTEXT , IN thissorting TINYINT , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		LimitType p
	SET p.name       = thisname
	  , p.sorting    = thissorting
	  , p.changedBy  = thischangedBy
	  , p.changeDate = thischangeDate
	  , p.isActive   = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateLocation`(IN thisid tinytext , IN thisname TINYTEXT , IN thissorting TINYINT(4) , IN thisisShowOnLandingPage BOOL , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		Location p
	SET p.name                = thisname
	  , p.sorting             = thissorting
	  , p.isShowOnLandingPage = thisisShowOnLandingPage
	  , p.changedBy           = thischangedBy
	  , p.changeDate          = thischangeDate
	  , p.isActive            = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateMeasurementType`(IN thisid tinytext , IN thisname TINYTEXT , IN thissorting TINYINT , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME , IN thisisActive BOOL)
BEGIN
	update
		MeasurementType p
	SET p.name       = thisname
	  , p.sorting    = thissorting
	  , p.changedBy  = thischangedBy
	  , p.changeDate = thischangeDate
	  , p.isActive   = thisisActive
	where
		p.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdatePot`(IN _id tinytext , IN _name TINYTEXT , IN _totalCapacity decimal(6,2) , IN _speed TINYINT , IN _currentCapacity decimal(6,2) , IN _antiShockRamp smallint , IN _expectedFlowRate decimal(6,2) , IN _pumpFlowErrorThreshold TINYINT , IN _pulsesPerLiter smallint , IN _isReservoir BOOL , IN _changedBy TINYTEXT , IN _changeDate DATETIME , IN _isActive BOOL )
BEGIN
	update
		Pot p
	SET p.name                   = _name
	  , p.totalCapacity          = _totalCapacity
	  , p.speed                  = _speed
	  , p.currentCapacity        = _currentCapacity
	  , p.antiShockRamp          = _antiShockRamp
	  , p.expectedFlowRate       = _expectedFlowRate
	  , p.pumpFlowErrorThreshold = _pumpFlowErrorThreshold
	  , p.pulsesPerLiter         = _pulsesPerLiter
	  , p.isReservoir            = _isReservoir
	  , p.changedBy              = thischangedBy
	  , p.changeDate             = thischangeDate
	  , p.isActive               = thisisActive
	where
		p.id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdatePump`(IN _id tinytext, _potID tinytext, _make varchar(100), _model varchar(100), _serialNumber varchar(100), _vendor varchar(100), _price decimal(19,4), _pulsesPerLiter int(11), _purchaseDate datetime, _installDate datetime, _changeDate datetime, _changedBy varchar(100), _isActive tinyint(1))
BEGIN
	UPDATE
		Pump p
	set p.potID          = unhex(replace(_potID,'-',''))
	  , p.make           = _make
	  , p.model          = _model
	  , p.serialNumber   = _serialNumber
	  , p.vendor         = _vendor
	  , p.pulsesPerLiter = _pulsesPerLiter
	  , p.installDate    =
		CASE
			WHEN _installDate IS NOT NULL
				THEN _installDate
		END
	  , p.purchaseDate =
		CASE
			WHEN _purchaseDate IS NOT NULL
				THEN _purchaseDate
		END
	  , p.changeDate = _changeDate
	  , p.changedBy  = _changedBy
	  , p.isActive   = _isActive
	where
		p.id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipe`(IN thisid tinytext , IN thisfeedingChartTypeID tinytext , IN thisweekNumber tinyint(4) , IN thischemicalsID tinytext , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		Recipe r
	set r.feedingChartTypeID = unhex(replace(thisfeedingChartTypeID ,'-',''))
	  , r.weekNumber         = thisweekNumber
	  , r.chemicalsID        = unhex(replace(chemicalsID ,'-',''))
	  , r.changedBy          = thischangedBy
	  , r.changeDate         = thischangeDate
	where
		r.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeName`(IN thisid tinytext , IN thisname tinytext , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		Recipe r
	set r.name       = thisname
	  , r.changedBy  = thischangedBy
	  , r.changeDate = thischangeDate
	where
		r.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepAmount`(IN thisid tinytext ,IN thisamount decimal(19,4) ,IN thischangedBy tinytext ,IN thischangeDate datetime)
BEGIN
	update
		RecipeStepAmount rsa
	set rsa.amount     = thisamount
	  , rsa.changedBy  = thischangedBy
	  , rsa.changeDate = thischangeDate
	where
		rsa.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepEveningSip`(IN thisid tinytext ,IN thisisEveningSip bool , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		RecipeStep rs
	set rs.isEveningSip = thisisEveningSip
	  , rs.changedBy    = thischangedBy
	  , rs.changeDate   = thischangeDate
	where
		rs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepIrrigation`(IN thisid tinytext ,IN thisirrigationEventsPerDay tinyint(4) , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		RecipeStep rs
	set rs.irrigationEventsPerDay = thisirrigationEventsPerDay
	  , rs.changedBy              = thischangedBy
	  , rs.changeDate             = thischangeDate
	where
		rs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepLightCycle`(IN thisid tinytext ,IN thislightCycleID TINYTEXT , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		RecipeStep rs
	set rs.lightCycleID = unhex(replace(thislightCycleID,'-',''))
	  , rs.changedBy    = thischangedBy
	  , rs.changeDate   = thischangeDate
	where
		rs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepLimit`(IN thisid tinytext ,IN thisvalue decimal(19,4) ,IN thischangedBy tinytext ,IN thischangeDate datetime )
BEGIN
	update
		RecipeStepLimit rsl
	set rsl.value      = thisvalue
	  , rsl.changedBy  = thischangedBy
	  , rsl.changeDate = thischangeDate
	where
		rsl.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepMorningSip`(IN thisid tinytext ,IN thisisMorningSip bool , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		RecipeStep rs
	set rs.isMorningSip = thisisMorningSip
	  , rs.changedBy    = thischangedBy
	  , rs.changeDate   = thischangeDate
	where
		rs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRecipeStepSoaktime`(IN thisid tinytext ,IN thissoakTime tinyint(4) , IN thischangedBy TINYTEXT , IN thischangeDate DATETIME)
BEGIN
	update
		RecipeStep rs
	set rs.soakTime   = thissoakTime
	  , rs.changedBy  = thischangedBy
	  , rs.changeDate = thischangeDate
	where
		rs.id = unhex(replace(thisid,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spUpdateRemoteProbe`(IN _id tinytext , IN _locationID tinytext , IN _measurementTypeID tinytext , IN _remoteProbeAddress varchar(100) , IN _gtUCLCommand varchar(100) , IN _ltLCLCommand varchar(100) , IN _changedBy varchar(100) , IN _changeDate DATETIME , IN _isActive BOOL)
BEGIN
	update
		RemoteProbe rp
	SET rp.locationID         = unhex(replace(_locationID,'-',''))
	  , rp.measurementTypeID  = unhex(replace(_measurementTypeID ,'-',''))
	  , rp.remoteProbeAddress = _remoteProbeAddress
	  , rp.ltLCLCommand       = _ltLCLCommand
	  , rp.gtUCLCommand       = _gtUCLCommand
	  , rp.changedBy          = _changedBy
	  , rp.changeDate         = _changeDate
	  , rp.isActive           = _isActive
	where
		rp.id = unhex(replace(_id,'-',''))
	;

END;
CREATE DEFINER=`yarguser`@`%` PROCEDURE `yargDB`.`spVerifyFertigationEventACK`(IN _commandID TINYTEXT)
BEGIN
	select
		case
			when fer.cafeDate is not null
				then 1
				else 0
		end as 'isVerified'
	from
		FertigationEventRecord fer
	where
		fer.commandID = unhex(replace(_commandID,'-',''))
	;

END;
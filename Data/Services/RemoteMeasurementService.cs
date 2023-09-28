using YARG.DAL;
using System;
using YARG.Data.Models.ViewModels;
using YARG.Models;
using YARG.Common_Types;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.Data.Services
{
    public class RemoteMeasurementService
    {
        private readonly RemoteProbeDAL _remoteProbeDAL;
        private readonly MeasurementDAL _measurementDAL;
        private readonly GrowSeasonDAL _growSeasonDAL;

        public RemoteMeasurementService(IConfiguration configuration)
        {
            _remoteProbeDAL = new(configuration);
            _measurementDAL = new(configuration);
            _growSeasonDAL = new(configuration);
        }

        public async Task AddRemoteMeasurement(RemoteMeasurementViewModel remoteMeasurement)
        {
            Guid _locationID = await _remoteProbeDAL.GetLocationIDByRemoteProbeAsync(remoteMeasurement.RemoteProbeAddress);
            Guid _measurementTypeID = await _remoteProbeDAL.GetMeasurementTypeIDByRemoteProbeAsync(remoteMeasurement.RemoteProbeAddress);

            CurrentIrrigationCalcs currentIrrigationCalcs = await _growSeasonDAL.GetCurrentIrrigationCalcs();           

            if (_locationID != Guid.Empty &&
                _measurementTypeID != Guid.Empty)
            {
                Measurement measurement = new();
                measurement.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                measurement.GrowSeasonID = currentIrrigationCalcs.GrowSeasonID;
                measurement.LocationID = _locationID;
                measurement.MeasurementTypeID = _measurementTypeID;
                measurement.MeasuredValue = remoteMeasurement.MeasuredValue;
                measurement.CreatedBy = remoteMeasurement.RemoteHostname;
                measurement.CreateDate = DateTime.Now;
                
                await _measurementDAL.AddMeasurementAsync(measurement);
            }
        }
    }
}

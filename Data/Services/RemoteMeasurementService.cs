using GardenMVC.DAL;
using System;
using GardenMVC.Data.Models.ViewModels;
using GardenMVC.Models;
using GardenMVC.Common_Types;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.Data.Services
{
    public class RemoteMeasurementService
    {
        private readonly RemoteProbeDAL _remoteProbeDAL;
        private readonly MeasurementDAL _measurementDAL;

        public RemoteMeasurementService(IConfiguration configuration)
        {
            _remoteProbeDAL = new(configuration);
            _measurementDAL = new(configuration);
        }

        public void AddRemoteMeasurement(RemoteMeasurementViewModel remoteMeasurement)
        {
            Guid _locationID = _remoteProbeDAL.GetLocationIDByRemoteProbe(remoteMeasurement.RemoteProbeAddress);
            Guid _measurementTypeID = _remoteProbeDAL.GetMeasurementTypeIDByRemoteProbe(remoteMeasurement.RemoteProbeAddress);

            if (_locationID != Guid.Empty &&
                _measurementTypeID != Guid.Empty)
            {
                Measurement measurement = new();
                measurement.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                measurement.LocationID = _locationID;
                measurement.MeasurementTypeID = _measurementTypeID;
                measurement.MeasuredValue = remoteMeasurement.MeasuredValue;
                measurement.CreatedBy = remoteMeasurement.RemoteHostname;
                measurement.CreateDate = DateTime.Now;
                
                _measurementDAL.AddMeasurement(measurement);
            }
        }
    }
}

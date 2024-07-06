using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YARG.DAL;
using YARG.Data.Services;
using YARG.Common_Types;

namespace YARG.Models
{
    public class MeasurementJob : IJob
    {
        private readonly IConfiguration _config;
        private readonly MqttPublisherService _mqttPublisherService;
        private readonly RemoteProbeDAL _remoteProbeDAL;

        public MeasurementJob(IConfiguration configuration, MqttPublisherService mqttMessagePublisher)
        {
            _config = configuration;
            _mqttPublisherService = mqttMessagePublisher;
            _remoteProbeDAL = new RemoteProbeDAL(configuration);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
            {
                try
                {
                    Console.WriteLine("Iterating remote probes...");
                    IEnumerable<RemoteProbe> remoteProbes = await _remoteProbeDAL.GetRemoteProbesAsync();

                    CurrentIrrigationCalcs currentIrrigationCalcs = new();

                    foreach (RemoteProbe remoteProbe in remoteProbes)
                    {
                        StringBuilder sb = new();

                        sb.Append(remoteProbe.LocationID);
                        sb.Append(':');
                        if (remoteProbe.MeasurementTypeID == Constants.MeasurementType_Temperature && currentIrrigationCalcs.IsDay)
                        {
                            sb.Append(Constants.MeasurementType_TempDay.ToString());
                        }
                        else if (remoteProbe.MeasurementTypeID == Constants.MeasurementType_Temperature && !currentIrrigationCalcs.IsDay)
                        {
                            sb.Append(Constants.MeasurementType_TempNight.ToString());
                        }
                        else if (remoteProbe.MeasurementTypeID == Constants.MeasurementType_Humidity && currentIrrigationCalcs.IsDay)
                        {
                            sb.Append(Constants.MeasurementType_HumidityDay.ToString());
                        }
                        else if (remoteProbe.MeasurementTypeID == Constants.MeasurementType_Humidity && !currentIrrigationCalcs.IsDay)
                        {
                            sb.Append(Constants.MeasurementType_HumidityNight.ToString());
                        }
                        else if (remoteProbe.MeasurementTypeID == Constants.MeasurementType_Weight)
                        {
                            sb.Append(Constants.MeasurementType_Weight.ToString());
                        }

                        try
                        {
                            await _mqttPublisherService.PublishMessageAsync("takeMeasurement/" + remoteProbe.RemoteProbeAddress, sb.ToString());
                            Console.WriteLine("Broadcasting MQTT " + sb.ToString() + " for takeMeasurement/" + remoteProbe.RemoteProbeAddress);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error!");
                            Console.WriteLine(ex.ToString() + " " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error!");
                    Console.WriteLine(ex.ToString() + " " + ex.Message);
                }
            }
        }
    }
}

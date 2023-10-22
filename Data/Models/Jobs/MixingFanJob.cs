using Microsoft.Extensions.Configuration;
using MQTTnet;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YARG.DAL;
using YARG.Data.Services;

namespace YARG.Models
{
    public class MixingFanJob : IJob
    {
        private readonly IConfiguration _config;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;
        private readonly MqttPublisherService _mqttPublisherService;

        public MixingFanJob(IConfiguration configuration, MqttPublisherService mqttMessagePublisher)
        {
            _config = configuration;
            _mixingFanScheduleDAL = new MixingFanScheduleDAL(configuration);
            _mqttPublisherService = mqttMessagePublisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
            {
                try
                {
                    //Console.WriteLine("Searching for mixing fan event(s) for " + DateTime.Now);
                    IEnumerable<MixingFanScheduleCommand> mixingFanScheduleCommands = await _mixingFanScheduleDAL.AreWeThereYetAsync();

                    //Do whatever stuff you want
                    if (mixingFanScheduleCommands != null)
                    {
                        foreach (MixingFanScheduleCommand mfsc in mixingFanScheduleCommands)
                        {
                            StringBuilder sb = new();
                            string MQTT_Topic_Suffix = "MF" + mfsc.FanNumber.ToString();
                            //Console.WriteLine("Found mixing fan event for " + MQTT_Topic_Suffix);

                            sb.Append(mfsc.PumpSpeed);
                            sb.Append(':');
                            sb.Append(mfsc.OverSpeed);
                            sb.Append(':');
                            sb.Append(mfsc.Duration);
                            sb.Append(':');
                            sb.Append(mfsc.MixingFanScheduleID);

                            try
                            {
                                // Use MqttMessagePublisher to publish the message
                                await _mqttPublisherService.PublishMessageAsync("mixingFan/" + MQTT_Topic_Suffix, sb.ToString());
                                //Console.WriteLine("Broadcasting MQTT " + sb.ToString() + " for mixingFan/" + MQTT_Topic_Suffix);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error!");
                                Console.WriteLine(ex.ToString() + " " + ex.Message);
                            }
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

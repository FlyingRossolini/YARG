using Microsoft.Extensions.Configuration;
using MQTTnet;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YARG.Common_Types;
using YARG.DAL;
using YARG.Data.Models.BusinessObjects;
using YARG.Data.Services;

namespace YARG.Models
{
    public class FertigationJob : IJob
    {
        private readonly IConfiguration _config;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly MqttPublisherService _mqttPublisherService;


        public FertigationJob(IConfiguration configuration, MqttPublisherService mqttMessagePublisher)
        {
            _config = configuration;
            _wateringScheduleDAL = new WateringScheduleDAL(configuration);
            _mqttPublisherService = mqttMessagePublisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
            {
                try
                {
                    Console.WriteLine("Searching for watering event for " + DateTime.Now);
                    FertigationEventCommand fertigationEventCommand = await _wateringScheduleDAL.AreWeThereYetAsync();

                    if (fertigationEventCommand != null)
                    {
                        string MQTT_Topic_Suffix = "FE" + fertigationEventCommand.PotNumber;
                        Console.WriteLine("Found watering event for " + MQTT_Topic_Suffix);

                        string mqttMessage = $"{fertigationEventCommand.CommandID}:{fertigationEventCommand.PotID}:" +
                            $"{fertigationEventCommand.EbbSpeed}:{fertigationEventCommand.EbbAmount}:" +
                            $"{fertigationEventCommand.EbbAntiShockRamp}:{fertigationEventCommand.EbbExpectedFlowRate}:" +
                            $"{fertigationEventCommand.EbbPumpErrorThreshold}:{fertigationEventCommand.EbbPulsesPerLiter}:" +
                            $"{fertigationEventCommand.SoakDuration}:{fertigationEventCommand.FlowSpeed}:" +
                            $"{fertigationEventCommand.FlowAntiShockRamp}:{fertigationEventCommand.FlowExpectedFlowRate}:" +
                            $"{fertigationEventCommand.FlowPumpErrorThreshold}:{fertigationEventCommand.FlowPulsesPerLiter}";

                        try
                        {
                            await _wateringScheduleDAL.CreateFertigationEventRecord(fertigationEventCommand.CommandID);

                            await _mqttPublisherService.PublishMessageAsync("fertigationEvent/" + MQTT_Topic_Suffix, mqttMessage);
                            Console.WriteLine("Broadcasting MQTT " + mqttMessage + " for fertigationEvent/" + MQTT_Topic_Suffix);

                            await Task.Delay(5000); // Introduce a 5-second delay

                            // check to see if we are good to go. (is CAFEDate set for this event)
                            if(!await _wateringScheduleDAL.VerifyFertigationEventACK(fertigationEventCommand.CommandID))
                            {
                                FertigationEventRecord fertigationEventRecord = new();
                                fertigationEventRecord.CommandID = fertigationEventCommand.CommandID;
                                fertigationEventRecord.IsError = true;
                                fertigationEventRecord.ErrorDate = DateTime.Now;
                                
                                await _wateringScheduleDAL.UpdateFertigationEventRecord(fertigationEventRecord);
                                Console.WriteLine("Fertigation event record updated.");

                                StringBuilder stringBuilder = new();
                                stringBuilder.Append("STOP");

                                await _mqttPublisherService.PublishMessageAsync("yargbot/ESTOP", stringBuilder.ToString());
                                Console.WriteLine("FE ACKs not found, broadcasting ESTOP.");



                            }
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

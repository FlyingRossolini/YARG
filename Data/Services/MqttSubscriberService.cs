using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Subscribing;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using YARG.Common_Types;
using YARG.DAL;
using YARG.Data.Models.MqttTopics;
using YARG.Data.Models.ViewModels;
using YARG.Models;

namespace YARG.Data.Services
{

    public class MqttSubscriberService : BackgroundService
    {
        private readonly IMqttClient _mqttClient;
        private readonly RemoteHostDBService _remoteHostDBService;
        private readonly LocationDAL _locationDAL;
        private readonly GrowSeasonDAL _growSeasonDAL;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly PumpWorklogDAL _pumpWorklogDAL;
        private readonly MqttPublisherService _mqttPublisherService;

   

        private FertigationEventAcknowledged _fertigationEventAcknowledged;
        private FertigationEventDone _fertigationEventDone;

        public MqttSubscriberService(IMqttClient mqttClient,IServiceProvider serviceProvider, RemoteHostDBService remoteHostDBService, IConfiguration configuration, MqttPublisherService mqttMessagePublisher)
        {
            _mqttClient = serviceProvider.GetRequiredService<IMqttClient>();
            _remoteHostDBService = remoteHostDBService;
            _mqttClient = mqttClient;
            _locationDAL = new(configuration);
            _growSeasonDAL = new(configuration);
            _wateringScheduleDAL = new(configuration);
            _pumpWorklogDAL = new(configuration);
            _mqttPublisherService = mqttMessagePublisher;
            _fertigationEventAcknowledged = new();
            _fertigationEventDone = new();        
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Global.SubscribeToTopics(_mqttClient);
            

            // Start listening for incoming MQTT messages
            _mqttClient.UseApplicationMessageReceivedHandler(HandleIncomingMessage);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private async void HandleIncomingMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            string topic = e.ApplicationMessage.Topic;

            Console.WriteLine($"Received message on topic '{topic}': {payload}");

            if (topic == "yargbot/acknowledge")
            {
                CommandTopic r = JsonConvert.DeserializeObject<CommandTopic>(payload);
                await _remoteHostDBService.AcknowledgeMixingFanSchedule(r);
                Console.WriteLine($"Written to db");
            }
            if (topic == "yargbot/done")
            {
                CommandTopic r = JsonConvert.DeserializeObject<CommandTopic>(payload);
                await _remoteHostDBService.CompleteMixingFanSchedule(r);
                Console.WriteLine($"Written to db");
            }
            if (topic == "yargbot/hello")
            {
                BotSaysHelloTopic botSaysHelloTopic = JsonConvert.DeserializeObject<BotSaysHelloTopic>(payload);
                botSaysHelloTopic.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                botSaysHelloTopic.CreatedBy = botSaysHelloTopic.Hostname;
                botSaysHelloTopic.CreateDate = DateTime.Now;
                

                await _remoteHostDBService.HelloBot(botSaysHelloTopic);
                Console.WriteLine($"Written to db");
            }
            if (topic == "yargbot/heartbeat")
            {
                BotHeartbeatTopic botHeartbeatTopic = JsonConvert.DeserializeObject<BotHeartbeatTopic>(payload);
                botHeartbeatTopic.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                botHeartbeatTopic.CreatedBy = "HITMAN";
                botHeartbeatTopic.CreateDate = DateTime.Now;

                await _remoteHostDBService.BotHeartbeat(botHeartbeatTopic);
                Console.WriteLine($"Written to db");
            }
            if (topic == "yargRPI/heartbeat")
            {
                RPIHeartbeatTopic rPIHeartbeatTopic = JsonConvert.DeserializeObject<RPIHeartbeatTopic>(payload);
                rPIHeartbeatTopic.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                rPIHeartbeatTopic.CreatedBy = "HITMAN";
                rPIHeartbeatTopic.CreateDate = DateTime.Now;

                await _remoteHostDBService.RPIHeartbeat(rPIHeartbeatTopic);

                if (rPIHeartbeatTopic.YargAppStatus is not null)
                {
                    RPIServiceYARG rPIServiceYARG = new();
                    rPIServiceYARG.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    rPIServiceYARG.RPIHeartbeatID = rPIHeartbeatTopic.ID;
                    rPIServiceYARG.YargAppCpuCount = rPIHeartbeatTopic.YargAppCpuCount;
                    rPIServiceYARG.YargAppCurrentTasks = rPIHeartbeatTopic.YargAppCurrentTasks;
                    rPIServiceYARG.YargAppStatus = rPIHeartbeatTopic.YargAppStatus;
                    rPIServiceYARG.YargAppTaskLimit = rPIHeartbeatTopic.YargAppTaskLimit;
                    rPIServiceYARG.CreatedBy = "HITMAN";
                    rPIServiceYARG.CreateDate = DateTime.Now;

                    await _remoteHostDBService.RPIServiceYargHeartbeat(rPIServiceYARG);

                }

                if (rPIHeartbeatTopic.LastBackupEndTime != DateTime.MinValue)
                {
                    BackupInfo backupInfo = new();
                    backupInfo.LastBackupEndTime = rPIHeartbeatTopic.LastBackupEndTime;

                    await _remoteHostDBService.UpdateBackupInfo(backupInfo);
                }

                Console.WriteLine($"Written to db");
            }
            if (topic == "yargRPI/hello")
            {
                RPIHelloTopic rPIHelloTopic = JsonConvert.DeserializeObject<RPIHelloTopic>(payload);
                rPIHelloTopic.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                rPIHelloTopic.CreatedBy = rPIHelloTopic.Hostname;
                rPIHelloTopic.CreateDate = DateTime.Now;

                await _remoteHostDBService.RPIHello(rPIHelloTopic);
                Console.WriteLine($"Written to db");
            }
            if (topic == "measuredResult")
            {
                try
                {
                    CurrentIrrigationCalcs currentIrrigationCalcs = await _growSeasonDAL.GetCurrentIrrigationCalcs();

                    Measurement measurement = JsonConvert.DeserializeObject<Measurement>(payload);
                    measurement.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    measurement.GrowSeasonID = currentIrrigationCalcs.GrowSeasonID;
                    measurement.LimitLCL = await _remoteHostDBService.GetLCL(currentIrrigationCalcs.GrowWeek, currentIrrigationCalcs.RecipeID, measurement.LocationID, measurement.MeasurementTypeID);
                    measurement.LimitUCL = await _remoteHostDBService.GetUCL(currentIrrigationCalcs.GrowWeek, currentIrrigationCalcs.RecipeID, measurement.LocationID, measurement.MeasurementTypeID);
                    measurement.CreatedBy = measurement.RemoteHostname;
                    measurement.CreateDate = DateTime.Now;

                    await _remoteHostDBService.AddMeasurement(measurement);
                    Console.WriteLine($"Written to db");

                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error!");
                    Console.WriteLine(ex.ToString() + " " + ex.Message);
                }
            }
            if (topic == "yargbot/FE_ebbFlowMeter_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_ebbFlowMeter_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_flowFlowMeter_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_flowFlowMeter_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_ebbPump_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_ebbPump_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_flowPump_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_flowPump_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_potOverflow_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_potOverflow_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_ebbSolenoids_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_ebbSolenoids_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_flowSolenoids_ACK")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventAcknowledged.FlgFE_flowSolenoids_ACK = true;

                await CheckForFE_Acknowledgement(commandTopic);
            }
            if (topic == "yargbot/FE_ebbFlowMeter_DN")
            {
                CommandTopic commandTopic = JsonConvert.DeserializeObject<CommandTopic>(payload);
                _fertigationEventDone.FlgFE_ebbFlowMeter_DN = true;

                await CheckForFE_Done(commandTopic);

            }
            if (topic == "yargbot/pumpWorklog")
            {
                PumpWorklogTopic pumpWorklogTopic = JsonConvert.DeserializeObject<PumpWorklogTopic>(payload);
                pumpWorklogTopic.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                //pumpWorklogTopic.FlowAmountmL = _pumpWorklogDAL.GetPPLByPumpId(pumpWorklogTopic.PumpID);
                pumpWorklogTopic.CreateDate = DateTime.Now;

                await _pumpWorklogDAL.AddPumpWorklog(pumpWorklogTopic);

                Console.WriteLine($"Pump worklog written to db.");
            }
            if (topic == "yargbot/ESTOP")
            {
                Console.WriteLine($"Bot offline detected!");
                ESTOPTopic eSTOPTopic = JsonConvert.DeserializeObject<ESTOPTopic>(payload);
                eSTOPTopic.ExpiryDate = DateTime.Now;
                eSTOPTopic.ChangeDate = DateTime.Now;

                await _remoteHostDBService.GoodbyeBot(eSTOPTopic);

            }
            if (topic == "yargRPI/ESTOP")
            {
                // Precious little that can be done at this point if any RPI is down at this point
                // might delete this code and simply go with yargbot/ESTOP only 

                Console.WriteLine($"RPI offline detected!");
                ESTOPTopic eSTOPTopic = JsonConvert.DeserializeObject<ESTOPTopic>(payload);
                eSTOPTopic.ExpiryDate = DateTime.Now;
                eSTOPTopic.ChangeDate = DateTime.Now;

                await _remoteHostDBService.RPIGoodbye(eSTOPTopic);

            }

        }

        private async Task CheckForFE_Acknowledgement(CommandTopic yargBot)
        {
            if (_fertigationEventAcknowledged.FlgAllAcknowledged)
            {
                await _wateringScheduleDAL.AcknowledgeWateringSchedule(yargBot);
                Console.WriteLine($"Fertigation event fully acknowledged, written to db.");

                await _mqttPublisherService.PublishMessageAsync("CAFE", "");
                Console.WriteLine($"Broadcasting CAFE message.");

            }

        }
        private async Task CheckForFE_Done(CommandTopic yargBot)
        {
            if (_fertigationEventDone.FlgAllDone)
            {
                await _wateringScheduleDAL.CompleteWateringSchedule(yargBot);
                Console.WriteLine($"Fertigation event done, written to db.");

            }
        }
    }
}
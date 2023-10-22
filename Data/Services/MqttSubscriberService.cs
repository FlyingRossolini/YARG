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
using YARG.Data.Models.BusinessObjects;
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

        private readonly FertigationEventRecord _fertigationEventRecord;
  
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
            //_fertigationEventAcknowledged = new();
            _fertigationEventRecord = new();
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
            if (topic == "yargbot/FE_ebbFlowmeter_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);

                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_ebbFlowmeter_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_ebbPump_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);

                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_ebbPump_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_ebbSolenoids_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);

                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_ebbSolenoids_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_flowFlowmeter_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);
                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_flowFlowmeter_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_flowPump_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);
                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_flowPump_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_flowSolenoids_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);
                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_flowSolenoids_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_potOverflow_ACK")
            {
                GenericFE_ACK x = JsonConvert.DeserializeObject<GenericFE_ACK>(payload);
                FertigationEventAcknowledged.CommandID = x.CommandID;
                FertigationEventAcknowledged.FlgFE_potOverflow_ACK = true;
                await CheckForFE_Acknowledgement();
            }
            if (topic == "yargbot/FE_ebbPump_RUN")
            {
                Guid commandID = JsonConvert.DeserializeObject<Guid>(payload);
                _fertigationEventRecord.CommandID = commandID;
                _fertigationEventRecord.EbbPump_RunDate = DateTime.Now;
                await _wateringScheduleDAL.UpdateFertigationEventRecord(_fertigationEventRecord);
            }
            if (topic == "yargbot/FE_ebbPump_DONE")
            {
                Guid commandID = JsonConvert.DeserializeObject<Guid>(payload);
                _fertigationEventRecord.CommandID = commandID;
                _fertigationEventRecord.EbbPump_DoneDate = DateTime.Now;
                await _wateringScheduleDAL.UpdateFertigationEventRecord(_fertigationEventRecord);
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
            //if (topic == "yargbot/ESTOP")
            //{
            //    Console.WriteLine($"Bot offline detected!");
            //    ESTOPTopic eSTOPTopic = JsonConvert.DeserializeObject<ESTOPTopic>(payload);
            //    eSTOPTopic.ExpiryDate = DateTime.Now;
            //    eSTOPTopic.ChangeDate = DateTime.Now;

            //    await _remoteHostDBService.GoodbyeBot(eSTOPTopic);

            //}
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

        private async Task CheckForFE_Acknowledgement()
        {
            Console.WriteLine($"Checking for all FE ACKs.");

            if (FertigationEventAcknowledged.FlgAllAcknowledged)
            {
                //_fertigationEventRecord.CommandID = FertigationEventAcknowledged.CommandID;
                //_fertigationEventRecord.CAFEDate = DateTime.Now;

                Guid x = FertigationEventAcknowledged.CommandID;
                
                await _wateringScheduleDAL.UpdateCAFEDateFertigationEventRecord(x);
                Console.WriteLine($"Fertigation event record; CAFE date updated.");

                await _mqttPublisherService.PublishMessageAsync("fertigationEvent/CAFE", FertigationEventAcknowledged.CommandID.ToString());
                Console.WriteLine($"Broadcasting CAFE message.");


                FertigationEventAcknowledged.ResetFlags();

            }

        }
    }

    public static class FertigationEventAcknowledged
    {
        [JsonProperty("CommandID")]
        public static Guid CommandID { get; set; }

        [JsonProperty("FE_ebbFlowmeter_ACK")]
        public static bool FlgFE_ebbFlowmeter_ACK { get; set; }

        [JsonProperty("FE_ebbPump_ACK")]
        public static bool FlgFE_ebbPump_ACK { get; set; }

        [JsonProperty("FE_flowFlowmeter_ACK")]
        public static bool FlgFE_flowFlowmeter_ACK { get; set; }

        [JsonProperty("FE_flowPump_ACK")]
        public static bool FlgFE_flowPump_ACK { get; set; }

        [JsonProperty("FE_potOverflow_ACK")]
        public static bool FlgFE_potOverflow_ACK { get; set; }

        [JsonProperty("FE_ebbSolenoids_ACK")]
        public static bool FlgFE_ebbSolenoids_ACK { get; set; }

        [JsonProperty("FE_flowSolenoids_ACK")]
        public static bool FlgFE_flowSolenoids_ACK { get; set; }


        public static bool FlgAllAcknowledged
        {
            get
            {
                return FlgFE_ebbFlowmeter_ACK &&
                       FlgFE_ebbPump_ACK &&
                       FlgFE_flowFlowmeter_ACK &&
                       FlgFE_flowPump_ACK &&
                       FlgFE_potOverflow_ACK &&
                       FlgFE_ebbSolenoids_ACK &&
                       FlgFE_flowSolenoids_ACK;
            }
        }

        public static void ResetFlags()
        {
            CommandID = Guid.Empty;
            FlgFE_ebbFlowmeter_ACK = false;
            FlgFE_ebbPump_ACK = false;
            FlgFE_flowFlowmeter_ACK = false;
            FlgFE_flowPump_ACK = false;
            FlgFE_potOverflow_ACK = false;
            FlgFE_ebbSolenoids_ACK = false;
            FlgFE_flowSolenoids_ACK = false;
        }
    }

}
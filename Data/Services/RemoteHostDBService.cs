using YARG.DAL;
using System;
using YARG.Data.Models.ViewModels;
using YARG.Models;
using YARG.Common_Types;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using YARG.Data.Models.MqttTopics;

namespace YARG.Data.Services
{
    public class RemoteHostDBService
    {
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;
        private readonly GrowSeasonDAL _growSeasonDAL;
        private readonly RemoteProbeDAL _remoteProbeDAL;
        private readonly BotDAL _botDAL;
        private readonly RPIDAL _rPIDAL;
        private readonly MeasurementDAL _measurementDAL;

        public RemoteHostDBService(IConfiguration configuration)
        {
            _wateringScheduleDAL = new(configuration);
            _mixingFanScheduleDAL = new(configuration);
            _remoteProbeDAL = new(configuration);
            _growSeasonDAL = new(configuration);
            _botDAL = new(configuration);
            _rPIDAL = new(configuration);
            _measurementDAL = new(configuration);
        }

        public async Task AcknowledgeWateringSchedule(CommandTopic remoteHostViewModel)
        {
            await _wateringScheduleDAL.AcknowledgeWateringSchedule(remoteHostViewModel);
        }

        public async Task CompleteWateringSchedule(CommandTopic remoteHostViewModel)
        {
            await _wateringScheduleDAL.CompleteWateringSchedule(remoteHostViewModel);
        }
        //public async Task ErrorWateringSchedule(RemoteHostErrorViewModel remoteHostErrorViewModel)
        //{

        //}
        public async Task AcknowledgeMixingFanSchedule(CommandTopic remoteHostCommandViewModel)
        {
            await _mixingFanScheduleDAL.AcknowledgeMixingFanScheduleAsync(remoteHostCommandViewModel);
        }
        public async Task CompleteMixingFanSchedule(CommandTopic remoteHostCommandViewModel)
        {
            await _mixingFanScheduleDAL.CompleteMixingFanScheduleAsync(remoteHostCommandViewModel);
        }

        public async Task ErrorMixingFanSchedule(RemoteHostErrorViewModel remoteHostErrorViewModel)
        {
            await Task.CompletedTask;
        }

        public async Task HelloBot(BotSaysHelloTopic botSaysHelloTopic)
        {
            await _botDAL.AddHelloBot(botSaysHelloTopic);
        }
        public async Task GoodbyeBot(ESTOPTopic eSTOPTopic)
        {
            await _botDAL.UpdateHelloBot(eSTOPTopic);
        }
        public async Task BotHeartbeat(BotHeartbeatTopic botHeartbeatTopic)
        {
            await _botDAL.AddBotHeartbeat(botHeartbeatTopic);
        }
        public async Task RPIHeartbeat(RPIHeartbeatTopic rPIHeartbeatTopic)
        {
            await _rPIDAL.AddRPIHeartbeat(rPIHeartbeatTopic);
        }
        public async Task RPIHello(RPIHelloTopic rPIHelloTopic)
        {
            await _rPIDAL.AddRPIHello(rPIHelloTopic);
        }
        public async Task RPIGoodbye(ESTOPTopic eSTOPTopic)
        {
            await _rPIDAL.UpdateRPIHello(eSTOPTopic);
        }
        public async Task RPIServiceYargHeartbeat(RPIServiceYARG rPIServiceYARG)
        {
            await _rPIDAL.AddRPIServiceYargHeartbeat(rPIServiceYARG);
        }
        public async Task<string> GetAppStatus()
        {
            return await _rPIDAL.GetYARGServiceStatus();
        }
        public async Task UpdateBackupInfo(BackupInfo backupInfo)
        {
            await _rPIDAL.UpdateBackupInfo(backupInfo);
        }
        public async Task AddMeasurement(Measurement measurement)
        {
            await _measurementDAL.AddMeasurementAsync(measurement);
        }
        public async Task<decimal> GetUCL(short growWeek, Guid recipeID, Guid locationID, Guid measurementTypeID)
        {
            return await _measurementDAL.GetUCL(growWeek, recipeID, locationID, measurementTypeID);
        }
        public async Task<decimal> GetLCL(short growWeek, Guid recipeID, Guid locationID, Guid measurementTypeID)
        {
            return await _measurementDAL.GetLCL(growWeek, recipeID, locationID, measurementTypeID);
        }
        public async Task<CurrentIrrigationCalcs> GetCurrentGrowInfo()
        {
            return await _growSeasonDAL.GetCurrentIrrigationCalcs();
        }
    }
}

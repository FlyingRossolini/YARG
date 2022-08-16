using YARG.DAL;
using System;
using YARG.Data.Models.ViewModels;
using YARG.Models;
using YARG.Common_Types;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.Data.Services
{
    public class RemoteHostService
    {
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;

        public RemoteHostService(IConfiguration configuration)
        {
            _wateringScheduleDAL = new(configuration);
            _mixingFanScheduleDAL = new(configuration);
        }

        public void AcknowledgeWateringSchedule(RemoteHostCommandViewModel remoteHostViewModel)
        {
            _wateringScheduleDAL.AcknowledgeWateringSchedule(remoteHostViewModel);
        }

        public void CompleteWateringSchedule(RemoteHostCommandViewModel remoteHostViewModel)
        {
            _wateringScheduleDAL.CompleteWateringSchedule(remoteHostViewModel);
        }
        public void ErrorWateringSchedule(RemoteHostErrorViewModel remoteHostErrorViewModel)
        {

        }
        public async Task AcknowledgeMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            await _mixingFanScheduleDAL.AcknowledgeMixingFanScheduleAsync(remoteHostCommandViewModel);
        }
        public async Task CompleteMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            await _mixingFanScheduleDAL.CompleteMixingFanScheduleAsync(remoteHostCommandViewModel);
        }

        public void ErrorMixingFanSchedule(RemoteHostErrorViewModel remoteHostErrorViewModel)
        {

        }


    }
}

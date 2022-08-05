using GardenMVC.DAL;
using System;
using GardenMVC.Data.Models.ViewModels;
using GardenMVC.Models;
using GardenMVC.Common_Types;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.Data.Services
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
        public void AcknowledgeMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            _mixingFanScheduleDAL.AcknowledgeMixingFanSchedule(remoteHostCommandViewModel);
        }
        public void CompleteMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            _mixingFanScheduleDAL.CompleteMixingFanSchedule(remoteHostCommandViewModel);
        }

        public void ErrorMixingFanSchedule(RemoteHostErrorViewModel remoteHostErrorViewModel)
        {

        }


    }
}

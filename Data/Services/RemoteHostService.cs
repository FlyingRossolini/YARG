using GardenMVC.DAL;
using System;
using GardenMVC.Data.Models.ViewModels;
using GardenMVC.Models;
using GardenMVC.Common_Types;

namespace GardenMVC.Data.Services
{
    public class RemoteHostService
    {
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;

        public RemoteHostService()
        {
            _wateringScheduleDAL = new();
            _mixingFanScheduleDAL = new();
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

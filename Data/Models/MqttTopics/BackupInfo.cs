﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class BackupInfo
    {
        public DateTime LastBackupEndTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class RPIServiceYARG
    {
        public Guid ID { get; set; }
        public Guid RPIHeartbeatID { get; set; }
        
        public short YargAppCurrentTasks { get; set; }

        public short YargAppTaskLimit { get; set; }

        public string YargAppCpuCount { get; set; }

        public string YargAppStatus { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

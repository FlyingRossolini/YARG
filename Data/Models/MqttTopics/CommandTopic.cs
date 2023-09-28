using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class CommandTopic
    {
        public string RemoteHostname { get; set; }
        public Guid CommandID { get; set; }
    }
}

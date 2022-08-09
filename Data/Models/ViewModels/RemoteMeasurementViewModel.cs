using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.ViewModels
{
    public class RemoteMeasurementViewModel
    {
        public string RemoteProbeAddress { get; set; }
        public decimal MeasuredValue { get; set; }
        public string RemoteHostname { get; set; }
    }
}

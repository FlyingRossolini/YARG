using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.ViewModels
{
    public class RemoteHostErrorViewModel
    {
        public string RemoteHostname { get; set; }
        public Guid CommandID { get; set; }
        public short ErrorCode { get; set; }
    }
}

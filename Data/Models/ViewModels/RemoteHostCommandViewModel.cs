using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.ViewModels
{
    public class RemoteHostCommandViewModel
    {
        public string RemoteHostname { get; set; }
        public Guid CommandID { get; set; }
    }
}

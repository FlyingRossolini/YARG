using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.BusinessObjects
{
    public class FertigationEventRecord
    {
        public Guid CommandID { get; set; }
        public DateTime FEDate { get; set; }
        public DateTime? CAFEDate { get; set; }
        public DateTime? EbbPump_RunDate { get; set; }
        public DateTime? EbbFlowmeter_DoneDate { get; set; }
        public DateTime? EbbPump_DoneDate { get; set; }
        public DateTime? FlowPump_StartDate { get; set; }
        public DateTime? FlowPump_RunDate { get; set; }
        public DateTime? FlowFlowmeter_DoneDate { get; set; }
        public DateTime? FlowPump_DoneDate { get; set; }
        public bool? IsError { get; set; }
        public DateTime? ErrorDate { get; set; }
    }
}

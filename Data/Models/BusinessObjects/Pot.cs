using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{

    public class Pot
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public byte QueuePosition { get; set; }

        public decimal TotalCapacity { get; set; }

        public byte Speed { get; set; }

        public decimal CurrentCapacity { get; set; }

        public short AntiShockRamp { get; set; }

        public decimal ExpectedFlowRate { get; set; }

        public byte PumpFlowErrorThreshold { get; set; }

        public short PulsesPerLiter { get; set; }

        public bool IsReservoir { get; set; }


        //public decimal 

        //[Required]
        //[Range(2, 13)]
        //[Display(Name = "Soak Duration (Mins)")]
        //public short EFDuration { get; set; }

        //[Required]
        //[Range(2, 20)]
        //[Display(Name = "Capacity (Liters)")]
        //public decimal EFAmount { get; set; }

        //[Required]
        //[Range(50, 100)]
        //[Display(Name = "Ebb speed")]
        //public short EbbSpeed { get; set; }

        //[Required]
        //[Range(50, 100)]
        //[Display(Name = "Flow speed")]
        //public short FlowSpeed { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ChangedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}

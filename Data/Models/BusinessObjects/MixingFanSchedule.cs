using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class MixingFanSchedule
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public Guid JarID { get; set; }

        public string JarChemicalName { get; set; }
        public short Position { get; set; }
        public short PumpSpeed { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime MFStartTime { get; set; }
        public DateTime MFEndTime { get; set; }

        [Required]
        [Range(2, 13)]
        [Display(Name = "Mix Duration (Mins)")]
        public short MFDuration { get; set; }
        public short Rollover { get; set; }

        public bool IsErrorState { get; set; }
        public bool IsAcknowledged { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime ErrorDate { get; set; }
        public DateTime AcknowledgeDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
    }
}

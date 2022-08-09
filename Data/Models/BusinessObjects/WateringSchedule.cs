using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class WateringSchedule
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public Guid PotID { get; set; }

        public string PotName { get; set; }
        public short PotQueuePosition { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime EFStartTime { get; set; }
        public DateTime EFEndTime { get; set; }

        [Required]
        [Range(2, 13)]
        [Display(Name = "Soak Duration (Mins)")]
        public short EFDuration { get; set; }
        public short Rollover { get; set; }

        [Required]
        [Range(2, 20)]
        [Display(Name = "Capacity (Liters)")]
        public decimal EFAmount { get; set; }
        public bool IsErrorState { get; set; }
        public bool IsAcknowledged { get; set; }
        public bool IsCompleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
    }
}

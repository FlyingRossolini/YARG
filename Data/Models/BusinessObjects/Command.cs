using System;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Command
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CommandText { get; set; }

        public DateTime CommandDate { get; set; }

        public short Priority { get; set; }

        public bool IsAcknowledged { get; set; }
        
        public DateTime AcknowledgeDate { get; set; }
        
        public bool IsCompleted { get; set; }
        
        public DateTime CompleteDate { get; set; }
        
        public bool IsErrored { get; set; }
        
        public DateTime ErrorDate { get; set; }

        public string CreatedBy { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public string ChangedBy { get; set; }
        
        public DateTime ChangeDate { get; set; }
        
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}

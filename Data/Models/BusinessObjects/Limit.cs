using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Limit
    {
        [Key]
        public Guid ID { get; set; }
        public Guid ParentID { get; set; }
        [Required]
        public Guid MeasurementTypeID { get; set; }
        public string MeasurementTypeName { get; set; }
        [Required]
        public Guid LimitTypeID { get; set; }
        public string LimitTypeName { get; set; }

        [Required]
        public decimal LimitValue { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}

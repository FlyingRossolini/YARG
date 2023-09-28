using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class RemoteProbe
    {
        [Key]
        public Guid ID { get; set; }
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
        public Guid MeasurementTypeID { get; set; }
        public string MeasurementTypeName { get; set; }
        [Required]
        [MaxLength(3)]
        public string RemoteProbeAddress { get; set; }
        public string GtUCLCommand { get; set; }
        public string LtLCLCommand { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}

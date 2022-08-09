using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Measurement
    {
        [Key]
        public Guid ID { get; set; }
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
        public Guid MeasurementTypeID { get; set; }
        public string MeasurementTypeName { get; set; }
        public decimal MeasuredValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class RecipeStepLimit
    {
        [Key]
        public Guid ID { get; set; }

        public Guid RecipeStepID { get; set; }

        public short WeekNumber { get; set; }

        public Guid LocationID { get; set; }
        
        public string LocationName { get; set; }

        public short LocationSort { get; set; }

        public Guid MeasurementTypeID { get; set; }
        
        public string MeasurementTypeName { get; set; }
        public short MeasurementSort { get; set; }

        public Guid LimitTypeID { get; set; }
        
        public string LimitTypeName { get; set; }

        public short LimitSort { get; set; }

        public decimal Value { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }

    }
}

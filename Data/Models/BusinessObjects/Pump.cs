using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{

    public class Pump
    {
        [Key]
        public Guid ID { get; set; }
        public Guid PotID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Vendor { get; set; }
        public decimal Price { get; set; }
        public int PulsesPerLiter { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? InstallDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangedBy { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}

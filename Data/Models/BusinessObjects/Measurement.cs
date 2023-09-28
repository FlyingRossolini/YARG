using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace YARG.Models
{
    public class Measurement
    {
        [Key]
        public Guid ID { get; set; }
        public Guid GrowSeasonID { get; set; }

        [JsonProperty("locationID")]
        public Guid LocationID { get; set; }

        public string LocationName { get; set; }

        [JsonProperty("measurementTypeID")]
        public Guid MeasurementTypeID { get; set; }

        public string MeasurementTypeName { get; set; }

        [JsonProperty("measuredValue")]
        public decimal MeasuredValue { get; set; }
        [JsonProperty("remoteHostname")]
        public string RemoteHostname { get; set; }

        public decimal LimitLCL { get; set; }
        public decimal LimitUCL { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

    }
}

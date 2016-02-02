using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndependentStudy.Models;
using System.ComponentModel.DataAnnotations;

namespace IndependentStudy.WebPages.Models
{
    public class DataRequest
    {
        [Required,Display(Name ="Buildings")]

        public int BuildingID { get; set; }

        [Required, Display(Name = "Meters")]
        public List<MeterType> MeterTypes { get; set; }

        [Required, Display(Name = "Start Date"), DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required, Display(Name = "End Date"),DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentStudy.Models
{

    public enum MeterType
    {
        Electricity, Steam, Water
    }
    public class BuildingMeterData
    {

        public BuildingMeterData()
        {
            DateValues = new Dictionary<DateTime, long>();
        }
        public int BuildingID { get; set; }

        public string BuildingName { get; set; }

        public MeterType MeterType { get; set; }

        public Dictionary<DateTime,long> DateValues { get; set; }

    }
}

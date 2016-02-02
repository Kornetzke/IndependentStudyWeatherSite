using IndependentStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentStudy.Service.ResponseObjects
{
    public class BuildingDataResponse
    {
        public BuildingDataResponse(MeterType meterType, int buildingID)
        {
           this.MeterType = meterType;
            this.BuildingID = buildingID;
            HistoricalData = new List<HistoricalData>();
        }

        public int BuildingID { get; set; }
        public MeterType MeterType { get; set; }
        public List<HistoricalData> HistoricalData { get; set; }
    }

    public class HistoricalData
    {
        public long time { get; set; }
        public long data { get; set; }
    }
}

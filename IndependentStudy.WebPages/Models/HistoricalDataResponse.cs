using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndependentStudy.Models;

namespace IndependentStudy.WebPages.Models
{
    public class HistoricalDataResponse
    {
        public HistoricalDataResponse()
        {
            HistoricalTemperatures = new Dictionary<DateTime, DateAndTemp>();
            buildingMeterDataList = new List<BuildingMeterData>();
        }
        public Dictionary<DateTime,DateAndTemp> HistoricalTemperatures { get; set; }
        public List<BuildingMeterData> buildingMeterDataList { get; set; }

        public string BuildingName { get; set; }
    }
}
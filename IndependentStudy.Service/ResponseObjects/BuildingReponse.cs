using IndependentStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentStudy.Service.ResponseObjects
{
    public class BuildingReponse
    {
        public List<Building> buildings { get; set; }
    }



    public class Building
    {
        public int buildingID { get; set; }
        public string name { get; set; }
        public int squareFeet { get; set; }
        public int occupancy { get; set; }
        public int capacity { get; set; }
        public int floors { get; set; }
        public int yearBuilt { get; set; }
        public string address { get; set; }
        public List<MeterType> meters { get; set; }
    }
}

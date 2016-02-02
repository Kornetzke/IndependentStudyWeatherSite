using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndependentStudy.Service;
using IndependentStudy.WebPages.Models;
using IndependentStudy.Models;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;

namespace IndependentStudy.Controllers
{
    public class HomeController : Controller
    {

        HistoricalCampusDataService campusService;
        HistoricalTemperatureDataService tempService;

        public HomeController()
        {
            campusService = new HistoricalCampusDataService();
            tempService = new HistoricalTemperatureDataService();
        }
        // GET: Default
        public ActionResult GetData()
        {

            var buildingResponse = campusService.GetAllBuildings();
            var meterTypes = Enum.GetNames(typeof(MeterType));
            

            ViewBag.BuildingsDropDown = new SelectList(buildingResponse.buildings, "BuildingID", "Name");
            ViewBag.MetersDropDown = new MultiSelectList(meterTypes);

            return View();
        }
        [HttpPost]
        public ActionResult GenerateGraph(DataRequest dataRequest)
        {
            if (dataRequest == null)
                return RedirectToAction("GetData");

            var buildingResponse = campusService.GetAllBuildings().buildings.Where(b => b.buildingID == dataRequest.BuildingID).FirstOrDefault();

            HistoricalDataResponse response = new HistoricalDataResponse();
            var tempList = tempService.GetHistoricalTemperatureData(dataRequest.StartTime, dataRequest.EndTime, limit:300);

            response.HistoricalTemperatures = tempService.ConvertResultsListIntoDateAndTempDictionary(tempList);

            
                foreach(MeterType meterType in dataRequest.MeterTypes)
                {
                    response.buildingMeterDataList.Add(campusService.GetResourceUsageOfBuilding(dataRequest.BuildingID, meterType, dataRequest.StartTime, dataRequest.EndTime));
                }

            response.BuildingName = buildingResponse.name;
            
            return View(response);
        }

        [HttpGet]
        public ActionResult GenerateGraph()
        {
            return RedirectToAction("GetData");
        }
    }
}
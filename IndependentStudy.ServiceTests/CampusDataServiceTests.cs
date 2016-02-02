using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndependentStudy.Service;
using IndependentStudy.Service.ResponseObjects;
using IndependentStudy.Models;

namespace IndependentStudy.ServiceTests
{
    [TestClass]
    public class CampusDataServiceTests
    {

        HistoricalCampusDataService service;

        [TestInitialize]
        public void SetUp()
        {
            service = new HistoricalCampusDataService();
        }

        [TestMethod]
        public void GetAllBuildings_Should_Return_Non_Null_BuildingResponse()
        {
            var buildings = service.GetAllBuildings();
            Assert.IsNotNull(buildings);
        }

        [TestMethod]
        public void GetResourceUsageOfBuilding_Should_Return_Non_Null_BuildingMeterData()
        {
            DateTime startDate = DateTime.Now.AddMonths(-4).Date;
            DateTime endDate = DateTime.Now.AddMonths(-3).Date;
            var data = service.GetResourceUsageOfBuilding(1, MeterType.Electricity, startDate, endDate);

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void ConvertBuidingDataResponse_Into_BuildingMeterData()
        {
            MeterType meterType = MeterType.Electricity;
            int buildingID = 2;
            long data = 5;
            long time = 1442102340000;
            DateTime dateTime = new DateTime(2015, 9, 12);
            BuildingDataResponse response = new BuildingDataResponse(meterType, buildingID);
            response.HistoricalData.Add(new HistoricalData()
            {
                data = data,
                time = time
            });

            BuildingMeterData meterData = service.ConvertBuildingDataResponse(response);

            Assert.AreEqual(buildingID, meterData.BuildingID);
            Assert.AreEqual(meterType, meterData.MeterType);
            Assert.AreEqual(1, meterData.DateValues.Count());

            Assert.IsTrue(meterData.DateValues.ContainsKey(dateTime));
            Assert.AreEqual(data, meterData.DateValues[dateTime]);

        }

        [TestMethod]
        public void DateTimeToUnixEpochTime_Should_Convert_DateTime_To_Unix_EpochTime()
        {
            Dictionary<DateTime, long> convertion = new Dictionary<DateTime, long>()
            {
                //grabbed these off a convertion website
                {new DateTime(2015,1,1), 1420092000000 },
                {new DateTime(2013,11,10),1384063200000 }
            };

            foreach( KeyValuePair<DateTime,long> convertionPair in convertion)
            {
                Assert.AreEqual(convertionPair.Value, service.ConvertDateTimeToUnixEpochTime(convertionPair.Key));
                
            }
        }

        [TestMethod]
        public void DateTimeToUnixEpochTime_Should_Convert_Unix_EpochTime_To_DateTime()
        {
            Dictionary<DateTime, long> convertion = new Dictionary<DateTime, long>()
            {
                //grabbed these off a convertion website
                {new DateTime(2015,1,1), 1420092000000 },
                {new DateTime(2013,11,10),1384063200000 }
            };

            foreach (KeyValuePair<DateTime, long> convertionPair in convertion)
            {
                Assert.AreEqual(convertionPair.Key, service.ConvertUnixEpochTimeToDateTime(convertionPair.Value));
            }
        }
    }
}

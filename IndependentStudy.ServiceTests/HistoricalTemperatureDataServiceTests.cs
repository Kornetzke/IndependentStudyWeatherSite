using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndependentStudy.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndependentStudy.Service.ResponseObjects;

namespace IndependentStudy.Service.Tests
{
    [TestClass()]
    public class HistoricalTemperatureDataServiceTests
    {

        HistoricalTemperatureDataService service;

        [TestInitialize]
        public void SetUp()
        {
            service = new HistoricalTemperatureDataService();
        }

        [TestMethod()]
        public void Get_Historical_Temperature_Data_Based_On_StartDate_And_EndDate()
        {
            DateTime startDate = new DateTime(2013, 3, 2);
            DateTime endDate = new DateTime(2013, 3, 2);

            var data = service.GetHistoricalTemperatureData(startDate, endDate, limit: 10);

            var data2 = service.GetHistoricalTemperatureData(startDate, endDate);


            Assert.IsNotNull(data);
            Assert.IsNotNull(data2);
            if (data.Count() != data2.Count())
            {
                Assert.Fail("Lists do not contain same number of Results");
            }
            for (int index = 0; index < data.Count(); index++)
            {
                if (data.ElementAt(index).Equals(data2.ElementAt(index)) == false)
                {
                    Assert.Fail("Results at :" + index + " are not equal");
                }
            }

        }

        [TestMethod]
        public void Convert_Number_Of_Tenths_Of_Degrees_In_Celsius_Into_Degrees_Fahrenheit()
        {
            Dictionary<int, decimal> convertions = new Dictionary<int, decimal>()
            {
                {1,32.18m},
                {-960,-140.8m},
                {120,53.6m},
                {10,33.8m},
                {-10,30.2m},
                {266,79.88m},
                {-355,-31.9m},
            };
            foreach (KeyValuePair<int, decimal> convertionPair in convertions)
            {
                Assert.AreEqual(convertionPair.Value, service.ConvertResultValueToFahrenheitTemp(convertionPair.Key),"ConvertResultValueToFahrenheitTemp failed for "+convertionPair.Key);
            }
        }

        [TestMethod]
        public void ResultList_Converts_Into_Dictionary_Of_DateAndTemp()
        {
            DateTime date = new DateTime(2013, 1, 1);
            int Tmax = 10, Tmin = 100;
            decimal convertedTmax = service.ConvertResultValueToFahrenheitTemp(Tmax);
            decimal convertedTmin = service.ConvertResultValueToFahrenheitTemp(Tmin);
            List<Result> resultList = new List<Result>()
            {
                new Result()
                {
                    datatype = Result.DataType.TMAX,
                    date = date,
                    value = Tmax
                },
                new Result()
                {
                    date = date,
                    datatype = Result.DataType.TMIN,
                    value = Tmin
                }
            };

            decimal expectedValue = (service.ConvertResultValueToFahrenheitTemp(Tmax) + service.ConvertResultValueToFahrenheitTemp(Tmin)) / 2m;

            var dateDictionary = service.ConvertResultsListIntoDateAndTempDictionary(resultList);

            Assert.AreEqual(1, dateDictionary.Count);
            Assert.IsTrue(dateDictionary.ContainsKey(date));
            Assert.AreEqual(convertedTmax, dateDictionary[date].HighTemp);
            Assert.AreEqual(convertedTmin, dateDictionary[date].LowTemp);
            Assert.AreEqual(expectedValue, dateDictionary[date].MeanTemperature);

        }

    }
}
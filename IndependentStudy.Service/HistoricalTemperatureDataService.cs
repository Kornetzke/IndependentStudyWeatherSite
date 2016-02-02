using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IndependentStudy.Service.ResponseObjects;
using IndependentStudy.Models;

namespace IndependentStudy.Service
{
    public class HistoricalTemperatureDataService
    {

        private const string apiUrl = "http://www.ncdc.noaa.gov/cdo-web/api/v2/data?datasetid=GHCND&datatypeid=TMAX,TMIN&locationid=ZIP:54022&startdate=?&enddate=?&limit=?&offset=?";
        private const string apiToken = "PFnuzFsokUgKgcUaKzTUUvqhcFoumuta";


        public List<Result> GetHistoricalTemperatureData(DateTime startDate, DateTime endDate, int offset = 0, int limit = 1000)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("startDate must be less than or equal to endDate");
            }


            string startDateStr = startDate.Year + "-" + startDate.Month.ToString("D2") + "-" + startDate.Day.ToString("D2");
            string endDateStr = endDate.Year + "-" + endDate.Month.ToString("D2") + "-" + endDate.Day.ToString("D2");

            string url;

            url = apiUrl.Replace("startdate=?", "startdate=" + startDateStr).Replace("enddate=?", "enddate=" + endDateStr).Replace("limit=?", "limit=" + limit).Replace("offset=?", "offset=" + offset);

            HistoricalTemperatureResponseData data = null;

            HttpWebResponse response = null;
            bool tooManyRequests = false;
            do
            {
                try
                {

                    tooManyRequests = false;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "GET";
                    request.Headers.Add("Token", apiToken);

                    response = (HttpWebResponse)request.GetResponse();

                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string responseString = sr.ReadToEnd().ToString();

                    data = JsonConvert.DeserializeObject<HistoricalTemperatureResponseData>(responseString);
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        response = (HttpWebResponse)e.Response;
                        if (response.StatusCode == (System.Net.HttpStatusCode)429)
                        {
                            //Too many requests, wait a little while and try again
                            tooManyRequests = true;
                            Debug.WriteLine("Too Many Request");
                        }
                        else
                        {
                            throw new Exception(("Errorcode: " + (int)response.StatusCode));
                        }

                    }
                    else
                    {
                        throw new Exception("Error: " + e.Status);
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            } while (tooManyRequests);


            int resultsGotten = data.metadata.resultset.limit + offset - (offset > 1 ? 1 : 0);

            if (data.metadata.resultset.count > resultsGotten)
            {
                data.results.AddRange(
                    GetHistoricalTemperatureData(startDate, endDate, offset: resultsGotten + 1, limit: limit));
                return data.results;
            }

            return data.results;
        }


        public Dictionary<DateTime, DateAndTemp> ConvertResultsListIntoDateAndTempDictionary(List<Result> resultList)
        {
            if (resultList.Count() % 2 != 0)
            {
                throw new Exception("resultList.Count() is not even, this means that there are not two values, Tmax and Tmin for each day");
            }
            Dictionary<DateTime, DateAndTemp> dateTempMap =
                new Dictionary<DateTime, DateAndTemp>(resultList.Count() / 2);

            //A date will have two results, one result will have TMAX, another will have TMIN
            // in order to get each day we will skip over every two indexes in the list
            // the first result will have the  TMAX value, second result will have TMIN value
            for (int index = 0; index < resultList.Count(); index += 2)
            {
                Result resultMax = resultList.ElementAt(index);
                Result resultMin = resultList.ElementAt(index+1);
                DateAndTemp dat = new DateAndTemp(resultMax.date, ConvertResultValueToFahrenheitTemp(resultMax.value), ConvertResultValueToFahrenheitTemp(resultMin.value));

                dateTempMap.Add(dat.Date, dat);
            }


            return dateTempMap;
        }

        /// <summary>
        /// Since the ncdc.noaa api returns the value of temperature in the number of tenths of a degree in celsius. We must convert this into degrees in fahrenheit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Temperature in fahrenheit</returns>
        public decimal ConvertResultValueToFahrenheitTemp(int value)
        {
            return (value / 10.0m) * 1.8m + 32;
        }
    }
}

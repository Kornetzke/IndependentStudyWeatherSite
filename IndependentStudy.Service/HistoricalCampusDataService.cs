using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndependentStudy.Service.ResponseObjects;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using IndependentStudy.Models;

namespace IndependentStudy.Service
{
    public class HistoricalCampusDataService
    {
        //
        public BuildingReponse GetAllBuildings()
        {

            string url = "https://apps.uwrf.edu/CampusResourcesAPI/building";


            BuildingReponse data = null;

            HttpWebResponse response = null;

            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());
                string responseString = "{\"buildings\":" + sr.ReadToEnd().ToString() + "}";

                data = JsonConvert.DeserializeObject<BuildingReponse>(responseString);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    throw new Exception(("Errorcode: " + (int)response.StatusCode));


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



            return data;

        }

        public BuildingMeterData GetResourceUsageOfBuilding(int buildingID, MeterType meterType, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("startDate must be less than endDate");
            }

            //startDate and endDate need to be in milliseconds since 1970 (Unix Epoch time)
            double startMillis = Math.Round(ConvertDateTimeToUnixEpochTime(startDate));

            double endMillis = Math.Round(ConvertDateTimeToUnixEpochTime(endDate));

            string url = "https://apps.uwrf.edu/CampusResourcesAPI/building/" + buildingID + "/meter/" + meterType + "?startTime=" + startMillis + "&endTime=" + endMillis + "&interval=day";

            BuildingDataResponse buildingDataResponse = null;

            HttpWebResponse response = null;

            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());
                string responseString = "{\"HistoricalData\":" + sr.ReadToEnd().ToString() + "}";
                
                buildingDataResponse = JsonConvert.DeserializeObject<BuildingDataResponse>(responseString);
                
                buildingDataResponse.BuildingID = buildingID;
                buildingDataResponse.MeterType = meterType ;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    throw new Exception(("Errorcode: " + (int)response.StatusCode));


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
            return ConvertBuildingDataResponse(buildingDataResponse);
        }


        public BuildingMeterData ConvertBuildingDataResponse(BuildingDataResponse response)
        {
            BuildingMeterData meterData = new BuildingMeterData();
            meterData.BuildingID = response.BuildingID;
            meterData.MeterType = response.MeterType;

            foreach(HistoricalData data in response.HistoricalData)
            {
                DateTime dateValue = ConvertUnixEpochTimeToDateTime(data.time).ToUniversalTime().Date;
                meterData.DateValues.Add(dateValue, data.data);
            }
            
            return meterData;
        }

        public double ConvertDateTimeToUnixEpochTime(DateTime datetime)
        {
            return datetime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public DateTime ConvertUnixEpochTimeToDateTime(long unixTime)
        {
            var dateTime= new DateTime(1970, 1, 1).AddMilliseconds(unixTime).ToLocalTime();
            return dateTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentStudy.Service.ResponseObjects
{
    public class HistoricalTemperatureResponseData
    {
        public Metadata metadata { get; set; }
        public List<Result> results { get; set; }
    }

    public class Resultset
    {
        public int offset { get; set; }
        public int count { get; set; }
        public int limit { get; set; }
    }

    public class Metadata
    {
        public Resultset resultset { get; set; }
    }

    
    public class Result
    {
        public enum DataType
        {
            TMAX, TMIN
        }
        public DateTime date { get; set; }
        public DataType datatype { get; set; }
        public string station { get; set; }
        public string attributes { get; set; }
        public int value { get; set; }

        public override int GetHashCode()
        {
            return date.Millisecond + value;
        }

        public override bool Equals(object obj)
        {
            bool areEqual = false;
            Result other = obj as Result;
            if (other != null && this.date == other.date && this.datatype == other.datatype && this.station == other.station && this.attributes == other.attributes && this.value == other.value)
            {
                areEqual = true;
            }

            return areEqual;
        }
    }

}

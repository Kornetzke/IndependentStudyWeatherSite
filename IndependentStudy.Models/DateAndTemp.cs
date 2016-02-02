using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentStudy.Models
{
    public class DateAndTemp
    {
        public DateAndTemp(DateTime date)
        {
            Date = date;
        }

        public DateAndTemp(DateTime date, decimal highTemp, decimal lowTemp)
        {
            this.Date = date;
            this.HighTemp = highTemp;
            this.LowTemp = lowTemp;
        }

        public DateTime Date { get; set; }
        public decimal HighTemp { get; set; }
        public decimal LowTemp { get; set; }
        public decimal MeanTemperature
        {
            get
            {
                return (HighTemp + LowTemp) / 2;
            }
        }
    }
}

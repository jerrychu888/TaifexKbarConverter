using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbarConverter
{
    static class TimeUtil
    {
        /// <summary>
        /// dateTime = dateTime.Truncate(TimeSpan.FromMilliseconds(1)); // Truncate to whole ms
        /// dateTime = dateTime.Truncate(TimeSpan.FromSeconds(1)); // Truncate to whole second
        /// dateTime = dateTime.Truncate(TimeSpan.FromMinutes(1)); // Truncate to whole minute
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        private static DateTime GetNthDayOfWeek(int year, int month, DayOfWeek dayOfWeek, int n)
        {
            // 從本月的第一天開始
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // 計算從本月第一天到第一個指定星期幾需要增加的天數
            int daysToAdd = ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;

            // 取得本月第一個指定的星期幾日期
            DateTime firstOccurrence = firstDayOfMonth.AddDays(daysToAdd);

            // 計算第 N 個指定星期幾的日期
            DateTime nthOccurrence = firstOccurrence.AddDays(7 * (n - 1));

            // 確保日期在同一個月內
            if (nthOccurrence.Month != month)
            {
                throw new ArgumentOutOfRangeException("The month does not have a " + n + "th " + dayOfWeek);
            }

            return nthOccurrence;
        }

        public static string CalculateDeliveryMonth(DateTime inputDate)
        {
            int year = inputDate.Year;
            int month = inputDate.Month;

            // 取得本月的第三個星期三
            DateTime thirdWednesday = GetNthDayOfWeek(year, month, DayOfWeek.Wednesday, 3);

            if (inputDate.Date <= thirdWednesday.Date)
            {
                // 如果日期在第三個星期三之前（包含該日），結算月為當月
                return inputDate.ToString("yyyyMM");
            }
            else
            {
                // 如果日期在第三個星期三之後，結算月為下個月
                DateTime nextMonthDate = inputDate.AddMonths(1);
                return nextMonthDate.ToString("yyyyMM");
            }
        }
    }


}

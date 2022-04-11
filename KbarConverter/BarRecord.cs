using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbarConverter
{
    public class BarRecord
    {
        public float Open;
        public float High;
        public float Low;
        public float Close;
        public int Volume;
        public DateTime dt;
        public string strDateTime;

        public void AddVolume(int vol)
        {
            Volume += vol;
        }

        public static List<BarRecord> SwitchTimeframeFrom1MinBar(List<BarRecord> sourceBars, int newTimeframeMin, bool isFromStartMinute = true)
        {
            Dictionary<DateTime, BarRecord> dicSwitchBarRecord = new Dictionary<DateTime, BarRecord>();
            List<BarRecord> retBars = new List<BarRecord>();

            foreach (var eachBar in sourceBars)
            {
                if (isFromStartMinute)
                {

                    var shiftTimeKey = eachBar.dt.AddMinutes(-(eachBar.dt.Minute % newTimeframeMin));

                    if (!dicSwitchBarRecord.ContainsKey(shiftTimeKey))
                    {
                        dicSwitchBarRecord.Add(shiftTimeKey, GetCloneBar(eachBar));
                        dicSwitchBarRecord[shiftTimeKey].dt = shiftTimeKey;
                        retBars.Add(dicSwitchBarRecord[shiftTimeKey]);
                    }
                    else
                    {

                        dicSwitchBarRecord[shiftTimeKey].High = Math.Max(dicSwitchBarRecord[shiftTimeKey].High, eachBar.High);
                        dicSwitchBarRecord[shiftTimeKey].Low = Math.Min(dicSwitchBarRecord[shiftTimeKey].Low, eachBar.Low);
                        dicSwitchBarRecord[shiftTimeKey].Close = eachBar.Close;
                        dicSwitchBarRecord[shiftTimeKey].Volume += eachBar.Volume;

                    }
                }

                if (!isFromStartMinute)
                {

                    DateTime shiftTimeKey = DateTime.MinValue;

                    if ((eachBar.dt.Minute % newTimeframeMin) != 0)
                        shiftTimeKey = eachBar.dt.AddMinutes(newTimeframeMin - (eachBar.dt.Minute % newTimeframeMin));
                    else
                        shiftTimeKey = eachBar.dt;

                    if (!dicSwitchBarRecord.ContainsKey(shiftTimeKey))
                    {
                        dicSwitchBarRecord.Add(shiftTimeKey, GetCloneBar(eachBar));
                        dicSwitchBarRecord[shiftTimeKey].dt = shiftTimeKey;
                        retBars.Add(dicSwitchBarRecord[shiftTimeKey]);


                    }
                    else
                    {

                        dicSwitchBarRecord[shiftTimeKey].High = Math.Max(dicSwitchBarRecord[shiftTimeKey].High, eachBar.High);
                        dicSwitchBarRecord[shiftTimeKey].Low = Math.Min(dicSwitchBarRecord[shiftTimeKey].Low, eachBar.Low);
                        dicSwitchBarRecord[shiftTimeKey].Close = eachBar.Close;
                        dicSwitchBarRecord[shiftTimeKey].Volume += eachBar.Volume;

                    }
                }

            }

            return retBars;
        }

        public static BarRecord GetCloneBar(BarRecord br)
        {
            BarRecord newBar = new BarRecord();
            newBar.dt = new DateTime(br.dt.Ticks);
            newBar.Open = br.Open;
            newBar.High = br.High;
            newBar.Low = br.Low;
            newBar.Close = br.Close;
            newBar.Volume = br.Volume;
            return newBar;
        }
    }

}

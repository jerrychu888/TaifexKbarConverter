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
    }

}

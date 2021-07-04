using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KbarConverter
{
    public partial class Form1 : Form
    {
        DirectoryInfo DIR = new DirectoryInfo(Environment.CurrentDirectory + "/input");
        string _deliverMonth = "201812";
        string _deliverMonthMode = "assign"; //hot, assign
        Dictionary<string, List<TickData>> _dicTickData = new Dictionary<string, List<TickData>>();
        bool _isFullTime = false;
        List<BarRecord> _Bars = new List<BarRecord>();
        int _convert_minutes = 1;
        string _outputName = "output";
        int _ExtractTypeFullTimeData = 1; //是否抓取下午盤資料 , 每份檔案的開頭是前一天的午盤 15:00~~~隔天的13:45 , 1:早盤,2:午盤,3:全時盤
        public Form1()
        {
            InitializeComponent();
            cbxFullTimeType.SelectedIndex = 0;
        }

        private void Init()
        {
            foreach (FileInfo fi in DIR.GetFiles())
            {
                ReadDataFromTXTickData(fi.FullName, Path.GetFileNameWithoutExtension(fi.Name).Split('_').ToArray());
            }

            ConvertKBarFromTick();
            WriteToCSV();
        }

        private void WriteToCSV()
        {
            using (StreamWriter sw = new StreamWriter(string.Format("{0}_{1}k.csv",_outputName, _convert_minutes)))
            {
                sw.WriteLine("Time,Open,High,Low,Close,Volume");

                foreach (var item in _Bars)
                {
                    sw.WriteLine($"{item.dt.ToString("yyyy/MM/dd HH:mm:ss")},{item.Open},{item.High},{item.Low},{item.Close},{item.Volume}");
                }
                sw.Close();
            }
        }

        private void ReadDataFromTXTickData(string fname,string[] DayKey)
        {
            StreamReader reader = new StreamReader(fname, System.Text.Encoding.Default);
            string str;
            string[] strArray;
            string InsertDayKey = DayKey[1] + "/" + DayKey[2] + "/" + DayKey[3];
            int countLineNumber = 1;
            reader.ReadLine();


            //如果讀取所有近月,讀一份新檔案的時候要清空
            if (_deliverMonthMode == "hot")
            {
                _deliverMonth = "";
            }


            while ((str = reader.ReadLine()) != null)
            {
                countLineNumber++;

                if (!str.Contains(','))
                    continue;
                strArray = str.Split(',');

                if (strArray.Count() < 6)
                    continue;

                if (strArray[1].Trim() != "TX")
                    continue;

                if (strArray[2].Trim().Count() > 7)
                    continue;

                if (_ExtractTypeFullTimeData == 1) //只抓早盤
                {
                    if (strArray[0] != DayKey[1] + DayKey[2] + DayKey[3])
                        continue;

                    if (strArray[3].CompareTo("084500") < 0)
                        continue;
                }

                if (strArray[3].CompareTo("134459") >= 0)
                    Console.WriteLine(str);


                //if (strArray[3].CompareTo("123000") >= 0)
                //{
                //    Console.Write(countLineNumber+":");
                //    Console.WriteLine(str);
                    
                //}


                if (_deliverMonthMode == "hot" && _deliverMonth=="")
                {
                    _deliverMonth = strArray[2].Trim();
                }
                //if (strArray[2].Trim() != _deliverMonth)
                //    continue;

                if (strArray[2].Trim() != _deliverMonth)
                    break;                

                TickData td = new TickData();
                td.strDtTime = InsertDayKey + " " + strArray[3].Trim().Substring(0, 2) + ":" + strArray[3].Trim().Substring(2, 2) + ":" + strArray[3].Trim().Substring(4, 2);
                td.fprice = float.Parse(strArray[4]);
                td.iVolume = int.Parse(strArray[5])/2;
                td.DtTime = DateTime.Parse(td.strDtTime);
                if (!_dicTickData.ContainsKey(InsertDayKey))
                    _dicTickData.Add(InsertDayKey, new List<TickData>());

                _dicTickData[InsertDayKey].Add(td);
                //list.Add(str);
                //Console.WriteLine(str);
            }
        }

        private void ConvertKBarFromTick()
        {
            foreach (KeyValuePair<string, List<TickData>> item in _dicTickData) //把所有資料轉成KLine
            {
                DateTime tempDT = new DateTime(int.Parse(item.Key.Substring(0, 4)), int.Parse(item.Key.Substring(5, 2)), int.Parse(item.Key.Substring(8, 2)), 8, 45, 00);
                int TimeDiff = -1;
                BarRecord BrData = null;
                float prePrice = -1;
                foreach (TickData tickData in item.Value)
                {
                    tickData.DtTime = tickData.DtTime.Truncate(TimeSpan.FromMinutes(1));

                    TimeSpan TsDiffDay = new TimeSpan(tickData.DtTime.Ticks - tempDT.Ticks);
                    if ((((int)(TsDiffDay.TotalMinutes)) - ((int)(TsDiffDay.TotalMinutes)) % _convert_minutes) != TimeDiff)
                    {
                        if (_isFullTime)
                        {

                        }
                        else
                        {
                            if (tickData.DtTime.Hour > 13) continue;
                            if (tickData.DtTime.Hour < 8) continue;

                            if (!(tickData.DtTime.Hour == 13 && tickData.DtTime.Minute == 45)) //超過1345在else處理,併入最後一分K
                            {
                                TimeDiff = (((int)(TsDiffDay.TotalMinutes)) - ((int)(TsDiffDay.TotalMinutes)) % _convert_minutes);
                                BrData = new BarRecord();
                                _Bars.Add(BrData);


                                BrData.dt = tickData.DtTime.AddMinutes(_convert_minutes);
                                //BrData.dt = BrData.dt.AddSeconds(-BrData.dt.Second);
                                BrData.Open = tickData.fprice;
                                BrData.High = tickData.fprice;
                                BrData.Low = tickData.fprice;
                                BrData.Close = tickData.fprice;
                                BrData.AddVolume(tickData.iVolume);
                                
                            }
                            else // 把多出來的資料併入最後一根K
                            {
                                if (tickData.fprice > BrData.High)
                                    BrData.High = tickData.fprice;
                                else if (tickData.fprice < BrData.Low)
                                    BrData.Low = tickData.fprice;

                                BrData.Close = tickData.fprice;
                                BrData.AddVolume(tickData.iVolume);
                                
                            }
                        }
                    }
                    else
                    {
                        if (tickData.fprice > BrData.High)
                            BrData.High = tickData.fprice;
                        else if (tickData.fprice < BrData.Low)
                            BrData.Low = tickData.fprice;

                        BrData.Close = tickData.fprice;
                        BrData.AddVolume(tickData.iVolume);
                        
                    }
                }
                Console.WriteLine(_Bars.Count);
                Console.WriteLine("Bar MinuteBar Add over");
            }
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (tbxDeliveryDate.Text == "yyyyMM")
            {
                _deliverMonthMode = "hot";
            }
            else
            {
                _deliverMonthMode = "assign";
                _deliverMonth = tbxDeliveryDate.Text;
            }

            _ExtractTypeFullTimeData = cbxFullTimeType.SelectedIndex+1;

            _convert_minutes = int.Parse(tbxMin.Text);
            _outputName = tbxOutputName.Text;
            Init();
        }
    }
}

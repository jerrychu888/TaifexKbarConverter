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

        const int REGULAR_TIME = 1;
        const int AFTER_REGULAR_TIME = 2;
        const int FULL_TIME = 3;

        string _dataTimeOutputColumnFormat = "OneColumn"; //OneColumn, TwoColumn
        string _kbarOutputShift = "Shift"; //NoShift, Shift


        public Form1()
        {
            InitializeComponent();
            cbxFullTimeType.SelectedIndex = 2;
            cbxDataType.SelectedIndex = 0;
            cbxHeaderFormat.SelectedIndex = 0;
            cbxDataTimeOutputColumnFormat.SelectedIndex = 0;
            cbxKbarOutputShift.SelectedIndex = 0;
        }

        private void Init()
        {
            foreach (FileInfo fi in DIR.GetFiles())
            {
                ReadDataFromTXTickData(fi.FullName, Path.GetFileNameWithoutExtension(fi.Name).Split('_').ToArray());
            }
            Console.WriteLine("Finish read");
            ConvertKBarFromTick();
            WriteToCSV();
        }

        private void WriteToCSV()
        {
            using (StreamWriter sw = new StreamWriter(string.Format("{0}_{1}k.csv", _outputName, _convert_minutes)))
            {
                if(_dataTimeOutputColumnFormat == "OneColumn")
                    sw.WriteLine("Time,Open,High,Low,Close,Volume");
                else if (_dataTimeOutputColumnFormat == "TwoColumn")
                    sw.WriteLine("Date,Time,Open,High,Low,Close,Volume");

                foreach (var item in _Bars)
                {
                    if(_dataTimeOutputColumnFormat== "OneColumn")
                        sw.WriteLine($"{item.dt.ToString("yyyy/MM/dd HH:mm:ss")},{item.Open},{item.High},{item.Low},{item.Close},{item.Volume}");
                    else if (_dataTimeOutputColumnFormat == "TwoColumn")
                        sw.WriteLine($"{item.dt.ToString("yyyy/MM/dd")},{item.dt.ToString("HH:mm:ss")},{item.Open},{item.High},{item.Low},{item.Close},{item.Volume}");
                    
                    //sw.WriteLine($"{item.dt.ToString("yyyy/MM/dd HH:mm:ss")},{item.Open},{item.High},{item.Low},{item.Close},{item.Volume}");
                }
                sw.Close();
            }
        }

        private void ReadDataFromTXTickData(string fname, string[] DayKey)
        {
            StreamReader reader = new StreamReader(fname, System.Text.Encoding.Default);
            string str;
            string[] strArray;
            string InsertDayKey = DayKey[1] + "/" + DayKey[2] + "/" + DayKey[3];
            DateTime fileNameDate = new DateTime(int.Parse(DayKey[1]), int.Parse(DayKey[2]), int.Parse(DayKey[3]));
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

                if (_ExtractTypeFullTimeData == REGULAR_TIME) //只抓早盤
                {
                    
                    if (strArray[0] != DayKey[1] + DayKey[2] + DayKey[3]) //前一天下午15~23:59:59
                        continue;

                    if (strArray[3].CompareTo("084500") < 0) //當天00:00:00~05:00:00
                        continue;
                }
                else if(_ExtractTypeFullTimeData == AFTER_REGULAR_TIME)
                {
                    if (strArray[3].CompareTo("084500") >= 0 && strArray[3].CompareTo("134500") <= 0)
                        continue;
                }
                else if(_ExtractTypeFullTimeData == FULL_TIME)
                {

                }

                //if (strArray[3].CompareTo("134459") >= 0)
                //    Console.WriteLine(str);


                //if (strArray[3].CompareTo("123000") >= 0)
                //{
                //    Console.Write(countLineNumber+":");
                //    Console.WriteLine(str);

                //}


                if (_deliverMonthMode == "hot" && _deliverMonth == "")
                {
                    //_deliverMonth = strArray[2].Trim();
                    _deliverMonth = TimeUtil.CalculateDeliveryMonth(fileNameDate);
                }
                //if (strArray[2].Trim() != _deliverMonth)
                //    continue;

                if (strArray[2].Trim() != _deliverMonth)
                    break;

                TickData td = new TickData();
                string tickDateKey = strArray[0].Substring(0,4) + "/" + strArray[0].Substring(4, 2) + "/" + strArray[0].Substring(6, 2);
                td.strDtTime = tickDateKey + " " + strArray[3].Trim().Substring(0, 2) + ":" + strArray[3].Trim().Substring(2, 2) + ":" + strArray[3].Trim().Substring(4, 2);
                td.fprice = float.Parse(strArray[4]);
                td.iVolume = int.Parse(strArray[5]) / 2;
                td.DtTime = DateTime.Parse(td.strDtTime);
                if (!_dicTickData.ContainsKey(tickDateKey))
                    _dicTickData.Add(tickDateKey, new List<TickData>());

                _dicTickData[tickDateKey].Add(td);
                //list.Add(str);
                //Console.WriteLine(str);
            }
        }
        private void ConvertKBarFromTick()
        {
            foreach (KeyValuePair<string, List<TickData>> item in _dicTickData) // 把所有資料轉成 KLine
            {
                DateTime tempDT = new DateTime(
                    int.Parse(item.Key.Substring(0, 4)),
                    int.Parse(item.Key.Substring(5, 2)),
                    int.Parse(item.Key.Substring(8, 2)),
                    8, 45, 0
                );

                int previousTimeDiff = -1;
                BarRecord currentBar = null;

                foreach (TickData tickData in item.Value)
                {
                    // 截斷時間至分鐘級
                    tickData.DtTime = tickData.DtTime.Truncate(TimeSpan.FromMinutes(1));

                    // 計算時間差
                    int totalMinutesDiff = (int)((tickData.DtTime - tempDT).TotalMinutes);

                    // 根據 isShift 決定時間對齊方式
                    int alignedTimeDiff = -1;
                    if (_kbarOutputShift == "NoShift")
                    {
                        // 0~4 分對齊到 0 分
                        alignedTimeDiff = (totalMinutesDiff / _convert_minutes) * _convert_minutes;
                    }
                    else if(_kbarOutputShift == "Shift")
                    {
                        // 0~4 分 Shift 到 5 分
                        alignedTimeDiff = ((totalMinutesDiff + _convert_minutes) / _convert_minutes) * _convert_minutes;
                    }

                    // 如果時間區間發生變化，建立新的 KBar
                    if (alignedTimeDiff != previousTimeDiff)
                    {
                        if (_isFullTime || (tickData.DtTime.Hour >= 8 && tickData.DtTime.Hour <= 13))
                        {
                            // 處理例外時間段
                            if (tickData.DtTime.Hour == 13 && tickData.DtTime.Minute == 46)
                            {
                                tickData.DtTime = tickData.DtTime.AddMinutes(-2);
                            }
                            else if (tickData.DtTime.Hour == 13 && tickData.DtTime.Minute == 45)
                            {
                                tickData.DtTime = tickData.DtTime.AddMinutes(-1);
                            }

                            // 創建新 KBar
                            previousTimeDiff = alignedTimeDiff;
                            currentBar = new BarRecord();
                            _Bars.Add(currentBar);

                            // 設定 KBar 基本資料
                            currentBar.dt = tempDT.AddMinutes(alignedTimeDiff);
                            currentBar.Open = tickData.fprice;
                            currentBar.High = tickData.fprice;
                            currentBar.Low = tickData.fprice;
                            currentBar.Close = tickData.fprice;
                            currentBar.AddVolume(tickData.iVolume);
                        }
                    }
                    else
                    {
                        // 更新當前 KBar
                        if (tickData.fprice > currentBar.High)
                            currentBar.High = tickData.fprice;

                        if (tickData.fprice < currentBar.Low)
                            currentBar.Low = tickData.fprice;

                        currentBar.Close = tickData.fprice;
                        currentBar.AddVolume(tickData.iVolume);
                    }
                }

                Console.WriteLine(_Bars.Count);
                Console.WriteLine("Bar MinuteBar Add over");
            }
        }
        private void ConvertKBarFromTick_old()
        {
            foreach (KeyValuePair<string, List<TickData>> item in _dicTickData) //把所有資料轉成KLine
            {
                DateTime tempDT = new DateTime(int.Parse(item.Key.Substring(0, 4)), int.Parse(item.Key.Substring(5, 2)), int.Parse(item.Key.Substring(8, 2)), 8, 45, 00);
                int TimeDiff = -1; //為了判斷是否要新增一根KBar
                BarRecord BrData = null;
                float prePrice = -1;
                foreach (TickData tickData in item.Value)
                {
                    tickData.DtTime = tickData.DtTime.Truncate(TimeSpan.FromMinutes(1));

                    TimeSpan TsDiffDay = new TimeSpan(tickData.DtTime.Ticks - tempDT.Ticks);

                    // Explanation: The modulo operation (%) is performed before the subtraction (-) due to operator precedence.
                    // This means that (int)(TsDiffDay.TotalMinutes) % _convert_minutes is calculated first, and then subtracted from (int)(TsDiffDay.TotalMinutes).
                    if ( ( ( (int)(TsDiffDay.TotalMinutes) ) - ( (int)(TsDiffDay.TotalMinutes) ) % _convert_minutes) != TimeDiff) //EX: 46- ( 46-45 %5)
                    {
                        if (_isFullTime)
                        {
                            if (tickData.DtTime.Hour == 13 && tickData.DtTime.Minute == 46)
                            {
                                tickData.DtTime = tickData.DtTime.AddMinutes(-2);
                            }
                            else if (tickData.DtTime.Hour == 13 && tickData.DtTime.Minute == 45)
                            {
                                tickData.DtTime = tickData.DtTime.AddMinutes(-1);
                            }

                            TimeDiff = (((int)(TsDiffDay.TotalMinutes)) - ((int)(TsDiffDay.TotalMinutes)) % _convert_minutes);
                            BrData = new BarRecord();
                            _Bars.Add(BrData);

                            BrData.dt = tickData.DtTime.AddMinutes(_convert_minutes);
                            BrData.Open = tickData.fprice;
                            BrData.High = tickData.fprice;
                            BrData.Low = tickData.fprice;
                            BrData.Close = tickData.fprice;
                            BrData.AddVolume(tickData.iVolume);
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

                                if(tickData.DtTime.Minute==46)
                                    Console.WriteLine();

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

            _ExtractTypeFullTimeData = cbxFullTimeType.SelectedIndex + 1;
            if(_ExtractTypeFullTimeData == FULL_TIME || _ExtractTypeFullTimeData == AFTER_REGULAR_TIME)
            {
                _isFullTime = true;
            }

            _dataTimeOutputColumnFormat = cbxDataTimeOutputColumnFormat.Text;
            _kbarOutputShift = cbxKbarOutputShift.Text;

            _convert_minutes = int.Parse(tbxMin.Text);
            _outputName = tbxOutputName.Text;

            if (cbxDataType.Text == "Tick")
                Init();
            else
                TransferKbar();
        }

        void TransferKbar()
        {
            var rawData = OpenFileBrowser();
            if (rawData != null)
            {
                int newTimeframe = int.Parse(tbxMin.Text);
                _Bars = BarRecord.SwitchTimeframeFrom1MinBar(rawData, newTimeframe, false);
                WriteToCSV();
            }
            else
            {
                MessageBox.Show("原始資料異常");
            }

        }

        List<BarRecord> OpenFileBrowser()
        {
            FileInfo f;
            StreamReader sr;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = (Environment.CurrentDirectory+@"\Data\");
            openFileDialog1.InitialDirectory = (Environment.CurrentDirectory);
            openFileDialog1.Filter = "txt files (*.csv)|*.csv|" + "All files (*.*)|*.*";
            //"說明   附檔名提示 真的系統filter的檔名" + ...
            openFileDialog1.Title = "開啟歷史資料";
            openFileDialog1.Multiselect = false; //是否可以選多個檔案
            openFileDialog1.FilterIndex = 1; //以第幾個filer為預設
            openFileDialog1.RestoreDirectory = true;
            List<BarRecord> _lstBarRecord = new List<BarRecord>();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                f = new FileInfo(openFileDialog1.FileName);
                var _fileName = openFileDialog1.FileName;
                _lstBarRecord.Clear();
                sr = f.OpenText();
                var header = sr.ReadLine();
                string str; //讀一整行用
                string[] str2; //分隔用
                switch (header.Split(',').Length)
                {
                    case 6:
                        cbxHeaderFormat.SelectedIndex = 1;
                        break;
                    case 7:
                        cbxHeaderFormat.SelectedIndex = 2;
                        break;
                    default:
                        return null;
                }

                if (cbxHeaderFormat.SelectedIndex == 1) //Time,Open,High,Low,Close,Volume
                {
                    while (sr.Peek() >= 0)
                    {
                        BarRecord Br_fromfile = new BarRecord();
                        DateTime dt_fromfile = new DateTime();
                        str = sr.ReadLine(); //讀出來是string
                        str2 = str.Split(','); //經由分隔的符號 分別讀成string 構成一個string[]
                        string strTimekey;

                        //2012/04/02 10:43,7856,7857,7855,7856,51,10:43:00
                        //Br_fromfile.BarDateTime=DateTime.Parse(str2[7]);

                        Br_fromfile.Open = float.Parse(str2[1]);
                        Br_fromfile.High = float.Parse(str2[2]);
                        Br_fromfile.Low = float.Parse(str2[3]);
                        Br_fromfile.Close = float.Parse(str2[4]);
                        Br_fromfile.Volume = int.Parse(str2[5]);
                        strTimekey = str2[0];

                        dt_fromfile = DateTime.Parse(strTimekey);
                        Br_fromfile.dt = dt_fromfile;
                        _lstBarRecord.Add(Br_fromfile);
                    }
                    sr.Close();
                    //Console.WriteLine("TEST READ TIME");
                    //foreach (DateTime ddtt in _dicBarRecord.Keys)
                    //{
                    //    Console.WriteLine("{0}", ddtt);

                    //}
                    return _lstBarRecord;
                }
                else if(cbxHeaderFormat.SelectedIndex == 2) //Date,Time,Open,High,Low,Close,TotalVolume
                {
                    while (sr.Peek() >= 0)
                    {
                        BarRecord Br_fromfile = new BarRecord();
                        DateTime dt_fromfile = new DateTime();
                        str = sr.ReadLine(); //讀出來是string
                        str2 = str.Split(','); //經由分隔的符號 分別讀成string 構成一個string[]
                        string strTimekey;

                        //2012/04/02 10:43,7856,7857,7855,7856,51,10:43:00
                        //Br_fromfile.BarDateTime=DateTime.Parse(str2[7]);

                        Br_fromfile.Open = float.Parse(str2[2]);
                        Br_fromfile.High = float.Parse(str2[3]);
                        Br_fromfile.Low = float.Parse(str2[4]);
                        Br_fromfile.Close = float.Parse(str2[5]);
                        Br_fromfile.Volume = int.Parse(str2[6]);
                        strTimekey = str2[0] + " " + str2[1];

                        dt_fromfile = DateTime.Parse(strTimekey);
                        Br_fromfile.dt = dt_fromfile;
                        _lstBarRecord.Add(Br_fromfile);
                    }
                    sr.Close();
                    return _lstBarRecord;
                }
                else
                {
                    MessageBox.Show("請補一下Data Header");
                    return null;
                }
                
            }
            else
            {
                return null;
            }

        }

        private void cbxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxDataType.Text== "Tick")
            {
                cbxHeaderFormat.SelectedIndex = 0;
                cbxHeaderFormat.Enabled = false;
            }
            else if(cbxDataType.Text == "1minK")
            {
                cbxHeaderFormat.SelectedIndex = 1;
                cbxHeaderFormat.Enabled = true;
                tbxMin.Text = "5";
            }
        }
    }
}

using BANG.CYBOSPLUS.DAL;
using BANG.CYBOSPLUS.Trader;
using BANG.Common.Log;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorService
{

    public partial class SimulatorService : ServiceBase
    {
        string dbConnString;
        string ExeSecond;
        string StockGroup;
        string SimType;

        List<string> gStockCodeList;
        BANG.Common.Timer.MyThreadTimer myTimer;
        BANG.Common.Timer.MyThreadTimer myTimerSIGNAL;

        string INSERTTODAYTRADESUMMARY = string.Empty;

        BANG.Common.Log.FileLog flog;

        string Signal_OldRawid = "0";

        bool isHoliday = false;

        public SimulatorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            myTimer = new BANG.Common.Timer.MyThreadTimer(1000);
            myTimerSIGNAL = new BANG.Common.Timer.MyThreadTimer(1000);

            gStockCodeList = new List<string>();

            dbConnString = GetDbConnStr();
            ExeSecond = GetExeSecond();
            StockGroup = GetStockGroup();
            SimType = GetSimType();

            flog = new BANG.Common.Log.FileLog("SimulService" + StockGroup, "C:\\Log", BANG.Common.Log.LogFileInteval.DAILY);
            flog.write(LogType.INFORMATION, "Simulator 시작");

            SqlDal sd = new SqlDal(dbConnString);
            //gStockCodeList = sd.GetStockCode("GROUP");
            gStockCodeList = sd.GetStockCode(StockGroup);

            isHoliday = sd.isHoliday();
            
            EventLog.WriteEntry("SimulatorSVC", "종목수 : " + gStockCodeList.Count.ToString() + Environment.NewLine + string.Format("{0} , {1} , {2}", SimType, ExeSecond, StockGroup));
            flog.write(LogType.INFORMATION, "종목수 : " + gStockCodeList.Count.ToString() + Environment.NewLine + string.Format("{0} , {1} , {2}", SimType, ExeSecond, StockGroup));

            INSERTTODAYTRADESUMMARY = sd.GetConfigValue("TRADER", "INSERTTODAYTRADESUMMARY");

            if (SimType == "ALL" || SimType == "SIGNAL")
            {
                myTimerSIGNAL.OnTick += myTimerSIGNAL_OnTick;
                myTimerSIGNAL.Start();
            }

            if (SimType == "ALL" || SimType == "DATA")
            {
                myTimer.OnTick += myTimer_OnTick;
                myTimer.Start();
            }
        }

        void myTimerSIGNAL_OnTick(object state)
        {
            try
            {
                if (DateTime.Now.Second == int.Parse(ExeSecond))
                {

                    DateTime sDateSIGNAL = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
                    DateTime eDateSIGNAL = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0);
                    DateTime Date0930 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0);

                    if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                            DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "주말에는 쉽니다");
                        return;
                    }

                    // SIGNAL BASED TRADING
                    if (DateTime.Now >= sDateSIGNAL &&
                        DateTime.Now < eDateSIGNAL
                        )
                    {
                        GeniusBaseSignal gs = new GeniusBaseSignal(dbConnString, gStockCodeList, LogWrite, StockGroup);

                        flog.write(LogType.INFORMATION, "GeniusBaseSignal Start! OldRawid : " + Signal_OldRawid);
                        Signal_OldRawid = gs.Process(Signal_OldRawid);

                    }
                    else
                    {
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "09:00 ~ 15:00 까지만 영업합니다");
                        return;
                    }
                }

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }
        }

        private void myTimer_OnTick(object state)
        {
            try
            {
                myTimer.Stop();

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                {
                    SqlDal sd = new SqlDal(dbConnString);
                    gStockCodeList = sd.GetStockCode(StockGroup);
                }
                else if (DateTime.Now.Second == int.Parse(ExeSecond))
                {
                    DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
                    DateTime eDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 15, 0);

                    DateTime EndMarketSDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 45, 0);
                    DateTime EndMarketEDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 55, 0);

                    DateTime DailyAllSellDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 10, 0);
                    DateTime ResetOrderStockDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);

                    DateTime Date0930 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0);

                    //if (DateTime.Now.Hour == sDate.Hour && DateTime.Now.Minute == sDate.Minute)
                    //    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "Simulator 시작");

                    //if (DateTime.Now.Hour == eDate.Hour && DateTime.Now.Minute == eDate.Minute)
                    //    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "Simulator 종료");

                    if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                        DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                        isHoliday
                       )
                    {
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "주말에는 쉽니다");
                        myTimer.Start();
                        return;
                    }

                    // DATA BASED TRADING
                    if (DateTime.Now.Hour == int.Parse(INSERTTODAYTRADESUMMARY.Substring(0, 2))
                        && DateTime.Now.Minute == int.Parse(INSERTTODAYTRADESUMMARY.Substring(2, 2))
                      )
                    {
                        if (StockGroup == "GROUP1")
                        {
                            SqlDal sd = new SqlDal(dbConnString);
                            sd.InsertTodayTradeSummary();
                            flog.write(LogType.INFORMATION, "TradeSummary");

                            sd.ResetBalance();
                            flog.write(LogType.INFORMATION, "ResetBalance");

                            sd.SaveScoreMstHistory();
                            flog.write(LogType.INFORMATION, "SaveScoreMstHistory");
                        }

                    }
                    else if (DateTime.Now.Hour == ResetOrderStockDate.Hour && DateTime.Now.Minute == ResetOrderStockDate.Minute)
                    {
                        if (StockGroup == "GROUP1")
                        {
                            SqlDal sd = new SqlDal(dbConnString);
                            sd.ResetOrderStock();
                            flog.write(LogType.INFORMATION, "ResetOrderStock");
                        }
                    }
                    else if (DateTime.Now >= sDate &&
                        DateTime.Now < eDate
                        )
                    {
                        // StockCur 기반 매수여부 확인
                        //BANG.CYBOSPLUS.Trader.gini_STOCKCUR giniStockCur = new BANG.CYBOSPLUS.Trader.gini_STOCKCUR(dbConnString, gStockCodeList);
                        //giniStockCur.Process();

                        //BANG.CYBOSPLUS.Trader.gini_STOCKMST giniStockMst = new BANG.CYBOSPLUS.Trader.gini_STOCKMST(dbConnString, gStockCodeList);
                        //giniStockMst.Process();

                        if (DateTime.Now.Hour == DailyAllSellDate.Hour && DateTime.Now.Minute >= DailyAllSellDate.Minute)
                        {
                            SqlDal sd = new SqlDal(dbConnString);

                            if (sd.GetisDailyTrading())
                            {
                                sd.SellActionOverDay();
                                sd.SellAction1440();
                                flog.write(LogType.INFORMATION, "당일청산");
                            }
                            else
                                flog.write(LogType.INFORMATION, "당일청산 Flag=N");
                        }
                        else if (DateTime.Now < Date0930)
                        {
                            SqlDal sd = new SqlDal(dbConnString);

                            if (sd.GetisStopMinusSellIn0930())
                            {
                                sd.SellActionOverDay();
                                sd.SellActionIn0930();
                            }
                            else
                            {
                                Genius g = new Genius(dbConnString, gStockCodeList, LogWrite);
                                g.Process();

                                //System.Threading.Thread t = new System.Threading.Thread(g.Process);
                                //t.Start();
                            }
                        }
                        else
                        {
                            Genius g = new Genius(dbConnString, gStockCodeList, LogWrite);
                            g.Process();

                            //System.Threading.Thread t = new System.Threading.Thread(g.Process);
                            //t.Start();
                        }
                    }
                    else if (DateTime.Now >= EndMarketSDate &&
                             DateTime.Now < EndMarketEDate)
                    {
                        SqlDal sd = new SqlDal(dbConnString);
                        if (sd.GetisDailyTrading())
                        {
                            sd.SellActionOverDay();
                            sd.SellAction1440();
                            flog.write(LogType.INFORMATION, "장종료후 당일청산 시도");
                        }
                    }
                    else
                    {
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + "09:00 ~ 15:00 까지만 영업합니다");
                        myTimer.Start();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }

            myTimer.Start();
        }

        protected override void OnStop()
        {
            myTimer.Stop();
            myTimerSIGNAL.Stop();
        }

        public void LogWrite(LogType logType, string msg)
        {
            flog.write(logType, msg);
        }

        private string GetDbConnStr()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["ConnString"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private string GetExeSecond()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["ExeSecond"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private string GetStockGroup()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["StockGroup"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private string GetSimType()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["SimType"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }
    }
}

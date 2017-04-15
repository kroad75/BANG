using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BANG.CYBOSPLUS.DataCollector.StocksRequest;
using BANG.CYBOSPLUS.DAL;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BANG.CYBOSPLUS.DataCollector.StocksSubscribe;
using System.Collections.Specialized;
using System.Configuration;

namespace BANG.CYBOSPLUS.DataCollector
{
    class Program
    {
        static string dbConnString = GetDbConnStr();
        static string StockGroup = GetStockGroup();
        static string LogDir = GetLogDir();
        static string ShutDownTimeStr = GetShutDownTime();
        static DateTime sDateTime = GetStartTime();
        static DateTime eDateTime = GetEndTime();
        static List<string> StockCodes = GetStockCodes(StockGroup);


        static void Main(string[] args)
        {
            try
            {
                //WriteMsg("DataCollector 시작합니다.", LogType.INFORMATION);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : DataCollector 시작합니다");

                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                    DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return;
                }

                //Request 타입 호출
                //Console.WriteLine("Request 타입 호출");
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Request 타입 호출");
                StockRequestData sqd = new StockRequestData
                {
                    dbConnString = dbConnString,
                    LogDir = LogDir,
                    sDate = sDateTime,
                    eDate = eDateTime,
                    StockCodes = StockCodes
                };

                RequestStockRunner rsr = new RequestStockRunner(sqd);
                Thread t = new Thread(rsr.RequestStockData);
                t.Start();


                //WriteMsg("StocksRequest 에게 수집을 요청했습니다.", LogType.INFORMATION);

                // Subscribe 타입 호출
                //Console.WriteLine("Subscribe 타입 호출");

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Subscribe 타입 호출");
                SubscribeStocksRunner ssr = new SubscribeStocksRunner(dbConnString, LogDir, sDateTime, eDateTime, StockCodes);
                ssr.Subscribe();

                while (true)
                {
                    //if (DateTime.Now.Minute == 0)
                    //{
                    //    UpdateRemainSubscribe();
                    //}

                    //if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 20)
                    //{
                    //    ssr.UnSubscribe();
                    //    rsr.selfKill = true;
                    //    t.Join(1000 * 60 * 5);
                    //    break;
                    //}

                    Thread.Sleep(1000 * 60);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("DataCollector", ex.ToString());
                //WriteMsg(ex.ToString(), LogType.ERROR);
            }

        }


        //static void rsr_OnCybosConnectionOff()
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (!underReconnecting)
        //        {
        //            underReconnecting = true;

        //            WriteMsg("접속이 끊어졌습니다.", LogType.WARNING);

        //            if (ssr != null)
        //                ssr.Unsubscribe();

        //            if (rsr != null)
        //            {
        //                rsr.Stop();

        //                // 기존 cybos process 들을 종료시키고
        //                Process[] cpP = Process.GetProcessesByName("CpStart");
        //                Process[] diP = Process.GetProcessesByName("DibServer");

        //                WriteMsg("기존 사이보스 프로세스 종료 시도", LogType.WARNING);

        //                if (cpP != null && diP != null)
        //                {
        //                    foreach (Process p in cpP)
        //                    {
        //                        p.Kill();
        //                        while (!p.HasExited)
        //                        {
        //                            Thread.CurrentThread.Join(1);
        //                        }
        //                    }

        //                    foreach (Process p in diP)
        //                    {
        //                        p.Kill();
        //                        while (!p.HasExited)
        //                        {
        //                            Thread.CurrentThread.Join(1);
        //                        }
        //                    }
        //                }

        //                WriteMsg("기존 사이보스 프로세스 종료 성공", LogType.WARNING);


        //                // 다시 실행시킨다
        //                WriteMsg("새로운 사이보스 프로세스 시작", LogType.WARNING);

        //                Process cybos = Process.Start(@"C:\Daishin\STARTER\ncStarter.exe", @"/prj:cp");
        //                Thread.CurrentThread.Join(1000 * 5);
        //                SetForegroundWindow(cybos.MainWindowHandle);
        //                SendKeys.SendWait("{ENTER}");
        //                Thread.CurrentThread.Join(1000 * 20);
        //                SendKeys.SendWait("olive12");
        //                Thread.CurrentThread.Join(1000 * 5);
        //                SendKeys.SendWait("{ENTER}");
        //                Thread.CurrentThread.Join(1000 * 5);
        //                SendKeys.SendWait("{ENTER}");
        //                Thread.CurrentThread.Join(1000 * 30);
        //                rsr.Run();
        //            }

        //            if (ssr != null)
        //                ssr.Subscribe();

        //            underReconnecting = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteMsg(ex.ToString(), LogType.ERROR);
        //    }

        //}

        private static List<string> GetStockCodes(string aStockGroup)
        {
            //throw new NotImplementedException();
            try
            {
                SqlDal sd = new SqlDal(dbConnString);
                return sd.GetStockCode(aStockGroup);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
            return new List<string>();
        }

        private static string GetLogDir()
        {
            //throw new NotImplementedException();
            try
            {
                SqlDal sd = new SqlDal(dbConnString);
                return sd.GetLogDir();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
            return "";
        }

        private static string GetShutDownTime()
        {
            //throw new NotImplementedException();
            try
            {
                SqlDal sd = new SqlDal(dbConnString);
                return sd.GetSysdownTime();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
            return "";
        }

        private static DateTime GetStartTime()
        {
            //throw new NotImplementedException();
            try
            {
                SqlDal sd = new SqlDal(dbConnString);
                return sd.GetStartTime();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
            return new DateTime();
        }

        private static DateTime GetEndTime()
        {
            //throw new NotImplementedException();
            try
            {
                SqlDal sd = new SqlDal(dbConnString);
                return sd.GetEndTime();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
            return new DateTime();
        }

        private static string GetDbConnStr()
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
                EventLog.WriteEntry("DataCollector", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private static string GetStockGroup()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["STOCKGROUP"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private static void UpdateRemainSubscribe()
        {
            //throw new NotImplementedException();
            try
            {
                CPUTILLib.CpCybos ccbos = new CPUTILLib.CpCybos();
                SqlDal sd = new SqlDal(dbConnString);
                sd.UpdateRemainSubscribe(new object[] { ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_SUBSCRIBE).ToString() });

                sd.Close();
                sd = null;

                ccbos = null;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("DataCollector", e.ToString());
            }
        }

        //static void WriteMsg(string msg, LogType lt)
        //{
        //    try
        //    {
        //        FileLog flm = new FileLog("[DataCollector]", LogDir, LogFileInteval.HOURLY);
        //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
        //        flm.write(lt, msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("DataCollector", ex.ToString());
        //    }
        //}
    }
}

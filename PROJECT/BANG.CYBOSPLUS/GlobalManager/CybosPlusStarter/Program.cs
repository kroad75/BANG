using BANG.Common.Timer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CybosPlusStarter
{
    class Program
    {
        static MyThreadTimer tMainTimer = null;
        static string CybosPlusEXEPath = GetCybosPlusEXEPath();
        static string DataCollectEXEPath = GetDataCollectEXEPath();
        static string OrderEXEPath = GetOrderEXEPath();
        //static BANG.CYBOSPLUS.DAL.SqlDal sd = new BANG.CYBOSPLUS.DAL.SqlDal(GetDbConnStr());
        static int StartHour = GetStartHour();
        static int StartMin = GetStartMin();
        static int EndHour = GetEndHour();
        static int EndMin = GetEndMin();



        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : 프로그램 시작");
                if (tMainTimer == null)
                {
                    tMainTimer = new MyThreadTimer(1000 * 60);
                    tMainTimer.OnTick += new MyThreadTimer.tick(tMainTimer_OnTick);
                    tMainTimer.Start();
                }

                while (true)
                {
                    System.Threading.Thread.Sleep(1000 * 60);
                    //System.Threading.Thread.CurrentThread.Join(1);
                    //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                    //Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("CybosPlusStarter", e.ToString());
            }
        }

        private static void tMainTimer_OnTick(object state)
        {
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                        DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return;
                }

                // 7시 20분에 cp 실행
                //if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 40)
                //{
                //    tMainTimer.Stop();

                //    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp 시작");
                //    ProcessStartInfo psi = new ProcessStartInfo();
                //    psi.FileName = CybosPlusEXEPath;
                //    Process.Start(psi);

                //    tMainTimer.Start();
                //}

                StartHour = GetStartHour();
                StartMin = GetStartMin();
                EndHour = GetEndHour();
                EndMin = GetEndMin();

                // 8시 40분에 cp / dc 실행
                if (DateTime.Now.Hour == StartHour && DateTime.Now.Minute == StartMin)
                {
                    tMainTimer.Stop();

                    if (CybosPlusEXEPath != null && CybosPlusEXEPath != "" && CybosPlusEXEPath.Length > 0)
                    {
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp 시작");
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = CybosPlusEXEPath;
                        Process.Start(psi);
                        Thread.CurrentThread.Join(1000 * 60 * 5);
                    }

                    if (DataCollectEXEPath != null && DataCollectEXEPath != "" && DataCollectEXEPath.Length > 0)
                    {
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : dc 시작");
                        ProcessStartInfo psi2 = new ProcessStartInfo();
                        psi2.FileName = DataCollectEXEPath;
                        Process.Start(psi2);
                        Thread.CurrentThread.Join(1000 * 60);
                    }

                    if (OrderEXEPath != null && OrderEXEPath != "" && OrderEXEPath.Length > 0)
                    {
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Order 시작");
                        ProcessStartInfo psi3 = new ProcessStartInfo();
                        psi3.FileName = OrderEXEPath;
                        Process.Start(psi3);
                    }

                    tMainTimer.Start();
                }

                // 16시 30분에 dc 종료
                if (DateTime.Now.Hour == EndHour && DateTime.Now.Minute == EndMin)
                {
                    tMainTimer.Stop();
                    // dc kill
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dc 시작");
                    Process[] DCProcess = Process.GetProcessesByName("DataCollector");
                    if (DCProcess != null)
                    {
                        foreach (Process p in DCProcess)
                        {
                            p.Kill();
                            while (!p.HasExited)
                            {
                                System.Threading.Thread.CurrentThread.Join(1000);
                            }
                        }
                    }
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dc 완료");
                    //    tMainTimer.Start();
                    //}

                    Thread.CurrentThread.Join(1000 * 60);

                    //// 18시 30분에 cp 종료
                    //if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 40)
                    //{
                    //    tMainTimer.Stop();
                    // cp kill
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill cp 시작");
                    Process[] cpP = Process.GetProcessesByName("CpStart");
                    if (cpP != null)
                    {
                        foreach (Process p in cpP)
                        {
                            p.Kill();
                            while (!p.HasExited)
                            {
                                System.Threading.Thread.CurrentThread.Join(1000);
                            }
                        }
                    }
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill cp 완료");

                    Thread.CurrentThread.Join(1000 * 60);

                    // dib kill
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dib 시작");
                    Process[] diP = Process.GetProcessesByName("DibServer");
                    if (diP != null)
                    {
                        foreach (Process p in diP)
                        {
                            p.Kill();
                            while (!p.HasExited)
                            {
                                System.Threading.Thread.CurrentThread.Join(1000);
                            }
                        }
                    }
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dib 완료");

                    tMainTimer.Start();
                }

                #region Old Source

                //if ((DateTime.Now.Hour >= 8 && DateTime.Now.Minute >= 40) 
                //    && (DateTime.Now.Hour <= 16 && DateTime.Now.Minute <= 30))
                //{
                //    if (sd.GetNeedRestartDataCollector() == "1")
                //    {
                //        tMainTimer.Stop();

                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : restart 시작");

                //        // dc kill
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dc 시작");
                //        Process[] DCProcess = Process.GetProcessesByName("DataCollector");
                //        if (DCProcess != null)
                //        {
                //            foreach (Process p in DCProcess)
                //            {
                //                p.Kill();
                //                while (!p.HasExited)
                //                {
                //                    System.Threading.Thread.CurrentThread.Join(1000);
                //                }
                //            }
                //        }
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dc 완료");

                //        Thread.CurrentThread.Join(1000 * 5);

                //        // cp kill
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill cp 시작");
                //        Process[] cpP = Process.GetProcessesByName("CpStart");
                //        if (cpP != null)
                //        {
                //            foreach (Process p in cpP)
                //            {
                //                p.Kill();
                //                while (!p.HasExited)
                //                {
                //                    System.Threading.Thread.CurrentThread.Join(1000);
                //                }
                //            }
                //        }
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill cp 완료");

                //        Thread.CurrentThread.Join(1000 * 5);

                //        // dib kill
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dib 시작");
                //        Process[] diP = Process.GetProcessesByName("DibServer");
                //        if (diP != null)
                //        {
                //            foreach (Process p in diP)
                //            {
                //                p.Kill();
                //                while (!p.HasExited)
                //                {
                //                    System.Threading.Thread.CurrentThread.Join(1000);
                //                }
                //            }
                //        }
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : kill dib 완료");

                //        Thread.CurrentThread.Join(1000 * 60);

                //        // cp start
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp start");
                //        ProcessStartInfo psi_a = new ProcessStartInfo();
                //        psi_a.FileName = CybosPlusEXEPath;
                //        Process.Start(psi_a);
                //        Thread.CurrentThread.Join(1000 * 60);

                //        // dc start
                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : dc start");
                //        ProcessStartInfo psi_b = new ProcessStartInfo();
                //        psi_b.FileName = DataCollectEXEPath;
                //        Process.Start(psi_b);

                //        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : restart 완료");
                //        sd.ResetRestartMe_DataCollector();

                //        tMainTimer.Start();
                //    }
                //}

                #endregion
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("CybosPlusStarter", e.ToString());
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + e.ToString());
            }
        }

        private static string GetDbConnStr()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
            return appSettings["ConnString"].ToString();
        }

        private static string GetCybosPlusEXEPath()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
            return appSettings["CybosPlusEXEPath"].ToString();
        }

        private static string GetOrderEXEPath()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
            return appSettings["OrderEXEPath"].ToString();
        }

        private static string GetDataCollectEXEPath()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
            return appSettings["DataCollectEXEPath"].ToString();
        }

        private static int GetStartHour()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            return int.Parse(appSettings["STARTTIME"].ToString().Split('|')[0]);
        }

        private static int GetStartMin()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            return int.Parse(appSettings["STARTTIME"].ToString().Split('|')[1]);
        }

        private static int GetEndHour()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            return int.Parse(appSettings["ENDTIME"].ToString().Split('|')[0]);
        }

        private static int GetEndMin()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            return int.Parse(appSettings["ENDTIME"].ToString().Split('|')[1]);
        }

    }
}

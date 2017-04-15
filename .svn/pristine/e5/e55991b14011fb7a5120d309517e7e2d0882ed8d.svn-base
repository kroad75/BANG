using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BANG.CYBOSPLUS.DAL;
using System.Diagnostics;

namespace BANG.CYBOSPLUS.DataCollector.StocksRequest
{
    public class CpSvr8081
    {
        //static FileLog flm = null;

        public static void RequestnSave(string dbConn, string LogDir)
        {
            List<object> Params = null;
            SqlDal sd = null;
            CPUTILLib.CpCybos ccbos = null;
            DSCBO1Lib.CpSvr8081 cp8081 = null;
            try
            {
                //WriteMsg(LogType.INFORMATION, LogDir, "CpSvr8081 요청시작", flm);
                ccbos = new CPUTILLib.CpCybos();

                int RemainsRequest = ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_NONTRADE_REQUEST);
                if (RemainsRequest < 5)
                {
                    while (RemainsRequest < 5)
                    {
                        //WriteMsg(LogType.WARNING, LogDir, "남은 요청갯수가 5개 미만이므로 5개 이상이 될때까지 대기합니다.");
                        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : 남은 요청갯수 " + RemainsRequest.ToString() + " 개");
                        Thread.CurrentThread.Join(1000);
                        RemainsRequest = ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_NONTRADE_REQUEST);
                    }

                    //WriteMsg(LogType.WARNING, LogDir, "남은 요청갯수가 5개 이상으로 복구되었습니다.");
                }

                cp8081 = new DSCBO1Lib.CpSvr8081();
                cp8081.SetInputValue(0, 1);
                cp8081.SetInputValue(1, 1);
                cp8081.SetInputValue(2, 1);
                cp8081.BlockRequest();

                sd = new SqlDal(dbConn);

                int count = (int)cp8081.GetHeaderValue(0);

                for (int i = 0; i < count; i++)
                {
                    Params = new List<object>();

                    Params.Add(i);

                    for (int j = 0; j < 10; j++)
                    {
                        Params.Add(cp8081.GetDataValue(j, i));
                    }

                    sd.SaveCp8081(Params.ToArray());
                }

                //WriteMsg(LogType.INFORMATION, LogDir, "CpSvr8081 데이터저장완료", flm);
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                //WriteMsg(LogType.ERROR, LogDir, cex.ToString());
                EventLog.WriteEntry("CpSvr8081", cex.ToString());

                if (sd == null)
                    sd = new SqlDal(dbConn);

                sd.SetRestartMe_DataCollector();
                throw cex;
            }
            catch (Exception ex)
            {
                //WriteMsg(LogType.ERROR, LogDir, ex.ToString());
                EventLog.WriteEntry("CpSvr8081", ex.ToString());
                throw ex;
            }
            finally
            {
                if (Params != null)
                {
                    Params.Clear();
                    Params = null;
                }

                if (sd != null)
                {
                    sd.Close();
                    sd = null;
                }

                if (ccbos != null)
                    ccbos = null;

                if (cp8081 != null)
                    cp8081 = null;
            }
        }

        //public static void WriteMsg(LogType logType, string LogDir, string msg)
        //{
        //    try
        //    {
        //        if (flm == null)
        //            flm = new FileLog("[CpSvr8081]", LogDir, LogFileInteval.HOURLY);
        //        flm.write(logType, msg);
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("CpSvr8081", ex.ToString());
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BANG.CYBOSPLUS.DAL;
using System.Diagnostics;

namespace BANG.CYBOSPLUS.DataCollector.StocksRequest
{
    public class CpSvr7225
    {
        //static FileLog flm = null;

        public static void RequestnSave(string dbConn, string LogDir)
        {
            List<object> Params = null;
            SqlDal sd = null;
            CPUTILLib.CpCybos ccbos = null;
            DSCBO1Lib.CpSvr7225 cp7225 = null;
            try
            {
                
                //WriteMsg(LogType.INFORMATION, LogDir, "CpSvr7225 요청시작", flm);

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

                cp7225 = new DSCBO1Lib.CpSvr7225();
                cp7225.SetInputValue(0, 0);
                cp7225.BlockRequest();

                Params = new List<object>();
                for (int i = 0; i < 11; i++)
                {
                    object o = cp7225.GetDataValue(i, 0);
                    Params.Add(o);
                }

                sd = new SqlDal(dbConn);
                sd.SaveCp7225(Params.ToArray<object>());
                //WriteMsg(LogType.INFORMATION, LogDir, "CpSvr7225 데이터저장완료", flm);
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                //WriteMsg(LogType.ERROR, LogDir, cex.ToString());
                EventLog.WriteEntry("CpSvr7225", cex.ToString());

                if (sd == null)
                    sd = new SqlDal(dbConn);

                sd.SetRestartMe_DataCollector();
                throw cex;
            }
            catch (Exception ex)
            {
                //WriteMsg(LogType.ERROR, LogDir, ex.ToString());
                EventLog.WriteEntry("CpSvr7225", ex.ToString());
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

                if (cp7225 != null)
                    cp7225 = null;
            }
        }

        //public static void WriteMsg(LogType logType, string LogDir, string msg)
        //{
        //    try
        //    {
        //        if (flm == null)
        //            flm = new FileLog("[CpSvr7225]", LogDir, LogFileInteval.HOURLY);
        //        flm.write(logType, msg);
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("CpSvr7225", ex.ToString());
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BANG.CYBOSPLUS.DAL;
using System.Diagnostics;

namespace BANG.CYBOSPLUS.DataCollector.StocksRequest
{
    public class StockMst
    {
        //static FileLog flm = null;

        public static void RequestnSave(string dbConn, string LogDir, List<string> StockCodes)
        {
            List<object> Params = null;
            SqlDal sd = null;
            CPUTILLib.CpCybos ccbos = null;
            DSCBO1Lib.StockMst stockMst = null;
            try
            {
                //WriteMsg(LogType.INFORMATION, LogDir, "StockMst 요청시작", flm);
                sd = new SqlDal(dbConn);
                ccbos = new CPUTILLib.CpCybos();

                foreach (string sCode in StockCodes)
                {
                    int RemainsRequest = ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_NONTRADE_REQUEST);

                    if (RemainsRequest < 5)
                    {
                        while (RemainsRequest < 5)
                        {
                            //WriteMsg(LogType.WARNING, LogDir, "남은 요청갯수가 5개 미만이므로 5개 이상이 될때까지 대기합니다");
                            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : 남은 요청갯수 " + RemainsRequest.ToString() + " 개");
                            Thread.CurrentThread.Join(1000);
                            RemainsRequest = ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_NONTRADE_REQUEST);
                        }

                        //WriteMsg(LogType.WARNING, LogDir, "남은 요청갯수가 5개 이상으로 복구되었습니다.\t" + sCode);
                    }

                    stockMst = new DSCBO1Lib.StockMst();
                    stockMst.SetInputValue(0, sCode);
                    stockMst.BlockRequest();

                    Params = new List<object>();

                    for (int i = 0; i < 71; i++)
                    {
                        if (i == 7 || i == 29 || i == 30 || i == 41 || i == 51) continue;
                        object o = stockMst.GetHeaderValue(i);
                        if (o == null) o = "0";
                        Params.Add(o);
                    }

                    if (Params[0] == null || Params[0].ToString() == "" || Params[0].ToString() == string.Empty || Params[0].ToString().Length == 0)
                        EventLog.WriteEntry("STOCKMST", "STOCKCODE is null : " + sCode);
                    else
                        sd.SaveStockMst(Params.ToArray());
                    //WriteMsg(LogType.INFORMATION, LogDir, string.Format("StockMst {0} 데이터저장완료", sCode), flm);
                }

                //WriteMsg(LogType.INFORMATION, LogDir, "StockMst 데이터저장완료", flm);
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                //WriteMsg(LogType.ERROR, LogDir, cex.ToString());
                EventLog.WriteEntry("StockMst", cex.ToString());

                if (sd == null)
                    sd = new SqlDal(dbConn);

                sd.SetRestartMe_DataCollector();
                throw cex;
            }
            catch (Exception ex)
            {
                //WriteMsg(LogType.ERROR, LogDir, ex.ToString());
                EventLog.WriteEntry("StockMst", ex.ToString());
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

                if (stockMst != null)
                    stockMst = null;
            }
        }

        //public static void WriteMsg(LogType logType, string LogDir, string msg)
        //{
        //    try
        //    {
        //        if (flm == null)
        //            flm = new FileLog("[StockMst]", LogDir, LogFileInteval.HOURLY);
        //        flm.write(logType, msg);
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("StockMst", ex.ToString());
        //    }
        //}
    }
}

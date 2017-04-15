using BANG.CYBOSPLUS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BANG.CYBOSPLUS.Trader
{
    public class gini_CPSVR8091S
    {
        List<string> cStockCode;
        string cConnString;

        public gini_CPSVR8091S(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_CPSVR8091S(string aConnString, List<string> aStockCode)
        {
            cStockCode = aStockCode;
            cConnString = aConnString;
        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestCpSvr8091S(aStockCode);
                DataSet dsBuy08 = sd.GetBUY08(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy08.Tables.Count > 0 && dsBuy08.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy08))
                        {
                            retbool = true;
                        }
                        else
                        {
                            retbool = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPSVR8091S-GoodTiming", e.ToString());
            }

            return retbool;
        }

        private void ChecknSell(string sCode)
        {
            try
            {
                //throw new NotImplementedException();
                SqlDal sd = new SqlDal(cConnString);

                //if (sd.SellAction(sCode) > 0)
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));

                sd.SellAction(sCode);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPSVR8091S-ChecknSell", e.ToString());
            }
        }

        public void Do_Buy(string sCode)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY08", sCode));
                SqlDal sd = new SqlDal(cConnString);
                sd.BuyAction(sCode, "BUY08");
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPSVR8091S-Do_Buy", e.ToString());
            }
        }

        public void Do_Buy(string sCode, string aBuyMethod)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - " + aBuyMethod, sCode));
                SqlDal sd = new SqlDal(cConnString);
                sd.BuyAction(sCode, aBuyMethod);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPSVR8091S-Do_Buy", e.ToString());
            }
        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy08)
        {
            bool retBool = false;

            if (long.Parse(dsCur.Tables[0].Rows[0]["SUM_ONLYBUY"].ToString())
                > long.Parse(dsBuy08.Tables[0].Rows[0]["SUM_CPSVR8091S_ONLYBUY"].ToString()))
            {
                if (long.Parse(dsCur.Tables[0].Rows[0]["SUM_FRNONLY"].ToString())
                    > long.Parse(dsBuy08.Tables[0].Rows[0]["SUM_CPSVR8091S_FRNONLY"].ToString()))
                {
                    retBool = true;
                }
            }

            return retBool;
        }
    }
}

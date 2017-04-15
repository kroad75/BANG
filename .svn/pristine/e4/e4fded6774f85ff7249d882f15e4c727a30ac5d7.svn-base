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
    public class gini_STOCKMST
    {
        List<string> cStockCode;
        string cConnString;

        public gini_STOCKMST(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_STOCKMST(string aConnString, List<string> aStockCode)
        {
            cStockCode = aStockCode;
            cConnString = aConnString;
        }

        public void Process()
        {
            if (cStockCode.Count > 0)
            {
                foreach (string sCode in cStockCode)
                {
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 확인", sCode));
                    Thread t = new Thread(this.ChecknBuy);
                    t.Start(sCode);
                    //t.Start("A000660");
                }
            }

        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestStockMst(aStockCode);
                DataSet dsBuy04 = sd.GetBUY04(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy04.Tables.Count > 0 && dsBuy04.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy04))
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
                EventLog.WriteEntry("Simulator-gini_STOCKMST-GoodTiming", e.ToString());
            }

            return retbool;
        }
        private void ChecknBuy(object aStockCode)
        {
            try
            {
                string sCode = aStockCode.ToString();

                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestStockMst(sCode);
                DataSet dsBuy01 = sd.GetBUY04(sCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy01.Tables.Count > 0 && dsBuy01.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(sCode))
                        {
                            ChecknSell(sCode);
                        }
                        else
                        {
                            if (BuyCheck(dsCur, dsBuy01))
                            {
                                if (DateTime.Now <= sd.GetBuyLimitTime())
                                {
                                    Do_Buy(sCode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator", e.ToString());
            }
        }

        private void ChecknSell(string sCode)
        {
            //throw new NotImplementedException();
            SqlDal sd = new SqlDal(cConnString);
            if (sd.SellAction(sCode) > 0)
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));
        }

        public void Do_Buy(string sCode)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY04", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY04");

        }

        public void Do_Buy(string sCode, string aBuyMethod)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - " + aBuyMethod, sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, aBuyMethod);

        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy04)
        {
            bool retBool = false;

            if (0 < long.Parse(dsBuy04.Tables[0].Rows[0]["AVG_STOCKMST_DIFFYESTERDAY"].ToString()))
            {
                if (long.Parse(dsCur.Tables[0].Rows[0]["FOREIGNERSELLTOTAL"].ToString())
                    < long.Parse(dsBuy04.Tables[0].Rows[0]["AVG_STOCKMST_FOREIGNERSELLTOTA"].ToString()))
                {
                    if (long.Parse(dsCur.Tables[0].Rows[0]["FOREIGNERBUYTOTAL"].ToString())
                        > long.Parse(dsBuy04.Tables[0].Rows[0]["AVG_STOCKMST_FOREIGNERBUYTOTAL"].ToString()))
                    {
                        retBool = true;

                    }
                }
            }

            return retBool;
        }
    }
}

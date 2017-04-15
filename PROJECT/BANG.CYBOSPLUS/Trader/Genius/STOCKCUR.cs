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
    public class gini_STOCKCUR
    {
        List<string> cStockCode;
        string cConnString;

        public gini_STOCKCUR(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_STOCKCUR(string aConnString, List<string> aStockCode)
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
                DataSet dsCur = sd.GetLatestStockCur(aStockCode);
                DataSet dsBuy01 = sd.GetBUY01(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy01.Tables.Count > 0 && dsBuy01.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy01))
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
                EventLog.WriteEntry("Trader-gini_STOCKCUR-GoodTiming", e.ToString());
            }

            return retbool;
        }

        private void ChecknBuy(object aStockCode)
        {
            try
            {
                string sCode = aStockCode.ToString();

                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestStockCur(sCode);
                DataSet dsBuy01 = sd.GetBUY01(sCode);

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
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY01", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY01");

        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy04)
        {
            bool retBool = false;

            if (long.Parse(dsCur.Tables[0].Rows[0]["STACKEDVOLUME"].ToString())
                    > long.Parse(dsBuy04.Tables[0].Rows[0]["SUM_STACKEDVOLUME"].ToString()))
            {
                if (long.Parse(dsCur.Tables[0].Rows[0]["STACKEDMONEY"].ToString())
                    > long.Parse(dsBuy04.Tables[0].Rows[0]["SUM_STACKEDMONEY"].ToString()))
                {
                    if (long.Parse(dsCur.Tables[0].Rows[0]["STACKEDSELLVOLUME"].ToString())
                        < long.Parse(dsBuy04.Tables[0].Rows[0]["SUM_STACKEDSELLVOLUME"].ToString()))
                    {
                        if (long.Parse(dsCur.Tables[0].Rows[0]["STACKEDBUYVOLUME"].ToString())
                            > long.Parse(dsBuy04.Tables[0].Rows[0]["SUM_STACKEDBUYVOLUME"].ToString()))
                        {
                            retBool = true;
                        }

                    }
                }
            }

            return retBool;
        }
    }

}

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
    public class gini_STOCKINDEXIS
    {
        List<string> cStockCode;
        string cConnString;

        public gini_STOCKINDEXIS(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_STOCKINDEXIS(string aConnString, List<string> aStockCode)
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
                DataSet dsCur = sd.GetLatestStockIndexIS(aStockCode);
                DataSet dsBuy02 = sd.GetBUY02(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy02.Tables.Count > 0 && dsBuy02.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy02))
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
                EventLog.WriteEntry("Simulator-gini_STOCKINDEXIS-GoodTiming", e.ToString());
            }

            return retbool;
        }
        private void ChecknBuy(object aStockCode)
        {
            //try
            //{
            //    string sCode = aStockCode.ToString();

            //    SqlDal sd = new SqlDal(cConnString);
            //    DataSet dsCur = sd.GetLatestStockMst(sCode);
            //    DataSet dsBuy01 = sd.GetBUY04(sCode);

            //    if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
            //    {
            //        if (dsBuy01.Tables.Count > 0 && dsBuy01.Tables[0].Rows.Count > 0)
            //        {
            //            if (HasStock(sCode))
            //            {
            //                ChecknSell(sCode);
            //            }
            //            else
            //            {
            //                if (BuyCheck(dsCur, dsBuy01))
            //                {
            //                    if (DateTime.Now <= sd.GetBuyLimitTime())
            //                    {
            //                        Do_Buy(sCode);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    EventLog.WriteEntry("Simulator", e.ToString());
            //}
        }

        private void ChecknSell(string sCode)
        {
            //throw new NotImplementedException();
            //SqlDal sd = new SqlDal(cConnString);
            //if (sd.SellAction(sCode) > 0)
            //    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));
        }

        public void Do_Buy(string sCode)
        {

            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY04", sCode));
            //SqlDal sd = new SqlDal(cConnString);
            //sd.BuyAction(sCode, "BUY04");

        }

        public void Do_Buy(string sCode, string aBuyMethod)
        {

            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - " + aBuyMethod, sCode));
            //SqlDal sd = new SqlDal(cConnString);
            //sd.BuyAction(sCode, aBuyMethod);

        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy02)
        {
            bool retBool = true;

            foreach (DataRow dr in dsCur.Tables[0].Rows)
            {
                string iCode = dr["INDUSTRYCODE"].ToString();
                
                double IDX = double.Parse(dr["IDX"].ToString());
                double DIFF = double.Parse(dr["DIFF"].ToString());
                double VOL = double.Parse(dr["VOL"].ToString());
                double MONEY = double.Parse(dr["MONEY"].ToString());

                if (iCode.Trim().ToUpper() == "U151")
                {
                    if (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U151_DIFF"].ToString()))
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U152")
                {
                    if (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U152_DIFF"].ToString()))
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U153")
                {
                    if (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U153_DIFF"].ToString()))
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U154")
                {
                    if (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U154_DIFF"].ToString()))
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U155")
                {
                    if (
                        (DIFF  < double.Parse(dsBuy02.Tables[0].Rows[0]["U155_DIFF"].ToString()))
                     || (VOL   < double.Parse(dsBuy02.Tables[0].Rows[0]["U155_VOL"].ToString()))
                     || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U155_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U156")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U156_DIFF"].ToString()))
                     || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U156_VOL"].ToString()))
                     || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U156_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U157")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_DIFF"].ToString()))
                     || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_VOL"].ToString()))
                     || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U177")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U177_DIFF"].ToString()))
                     //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_VOL"].ToString()))
                     //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U178")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U178_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U179")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U179_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U157_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U180")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_DIFF"].ToString()))
                        || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_VOL"].ToString()))
                        || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U181")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U181_DIFF"].ToString()))
                        || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U181_VOL"].ToString()))
                        || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U181_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U182")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U182_DIFF"].ToString()))
                        || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U182_VOL"].ToString()))
                        || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U182_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U186")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U186_DIFF"].ToString()))
                        || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U186_VOL"].ToString()))
                        || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U186_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U187")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U187_DIFF"].ToString()))
                        || (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U187_VOL"].ToString()))
                        || (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U187_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U501")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U501_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U502")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U502_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U503")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U503_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }
                else if (iCode.Trim().ToUpper() == "U504")
                {
                    if (
                        (DIFF < double.Parse(dsBuy02.Tables[0].Rows[0]["U504_DIFF"].ToString()))
                        //|| (VOL < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_VOL"].ToString()))
                        //|| (MONEY < double.Parse(dsBuy02.Tables[0].Rows[0]["U180_MONEY"].ToString()))
                    )
                    {
                        retBool = false;
                        break;
                    }
                }

            }
            return retBool;
        }
    }
}


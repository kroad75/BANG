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
    public class gini_CHECKSIGNAL
    {
        string cConnString;
        string _SIGNAL = string.Empty;
        public string SIGNAL
        {
            get
            {
                return _SIGNAL;
            }
        }

        public gini_CHECKSIGNAL(string aConnString)
        {
            cConnString = aConnString;
        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCheckSignal = sd.GetCpMarketWatchByStockCode(aStockCode);

                if (dsCheckSignal.Tables.Count > 0 && dsCheckSignal.Tables[0].Rows.Count > 0)
                {
                    if (HasStock(aStockCode))
                    {
                        // 해당 종목 보유시에는 구매 Timing 이 아님
                        retbool = false;
                    }
                    else if (BuyCheck(dsCheckSignal))
                    {
                        retbool = true;
                    }
                    else
                    {
                        retbool = false;
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_STOCKMST-GoodTiming", e.ToString());
            }

            return retbool;
        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCheckSignal)
        {
            bool retBool = false;

            SortedList<string, string> signalChar = new SortedList<string, string>();
            StringBuilder sbSignalChar = new StringBuilder();

            foreach (DataRow dr in dsCheckSignal.Tables[0].Rows)
            {
                string sCh = dr["MARKETWATCHCODE"].ToString();

                if (!signalChar.Keys.Contains(sCh))
                {
                    signalChar.Add(sCh, sCh);
                }
            }

            if (signalChar.Keys.Count > 0)
            {
                retBool = true;
                foreach(string sCh in signalChar.Values)
                {
                    sbSignalChar.Append(sCh + ",");
                }
            }

            _SIGNAL = sbSignalChar.ToString(0, sbSignalChar.Length - 1);

            return retBool;
        }
    }
}

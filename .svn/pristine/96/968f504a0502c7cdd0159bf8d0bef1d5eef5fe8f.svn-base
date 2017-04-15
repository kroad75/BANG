using BANG.Common.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using XA_DATASETLib;

namespace OrderForm
{
    public class ETRADE_SELL
    {
        string ACCOUNTNUMBER;
        string STOCKCODE;
        long ORDERQTY;
        bool ISEXEC;

        LOGWRITER LR;

        bool isSucc = true;

        // Sell 주문용
        XAQueryClass XAQ_00600 = new XAQueryClass();

        public ETRADE_SELL(string aAccountnumber, string aStockCode, long aOrderQty, bool aIsExec, LOGWRITER lr)
        {
            ACCOUNTNUMBER = aAccountnumber;
            STOCKCODE = aStockCode;
            ORDERQTY = aOrderQty;
            ISEXEC = aIsExec;

            LR = new LOGWRITER(lr);
        }

        public bool SELL()
        {
            try
            {
                // 매도주문
                XAQ_00600.ResFileName = ".\\Res\\CSPAT00600.res";
                XAQ_00600.ReceiveMessage += XAQ_00600_ReceiveMessage;

                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, ACCOUNTNUMBER);

                if (ISEXEC)
                    XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji1212");
                else
                    XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji12!@");

                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, STOCKCODE);
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, ORDERQTY.ToString());
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "1");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");

                LR(LogType.INFORMATION, string.Format("매도 주문요청 종목코드 : {0} , 수량 : {1} ", STOCKCODE, ORDERQTY.ToString()));
                if (XAQ_00600.Request(false) < 0)
                {
                    LR(LogType.ERROR, "주문전송 오류 " + STOCKCODE);
                    isSucc = false;
                }

                return isSucc;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("ETRADE_SELL-SELL", e.ToString());
                return false;
            }
        }

        private void XAQ_00600_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            try
            {
                LR(LogType.INFORMATION, string.Format("매도주문 요청 응답 : {0} {1} {2}", bIsSystemError.ToString(), nMessageCode, szMessage));
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("ETRADE_SELL-ReceiveMessage", e.ToString());
            }
        }

    }
}

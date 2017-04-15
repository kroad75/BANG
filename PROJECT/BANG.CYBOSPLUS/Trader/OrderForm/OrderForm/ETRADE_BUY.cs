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
    public delegate void LOGWRITER(LogType lt, string msg);
    public class ETRADE_BUY
    {
        string ACCOUNTNUMBER;
        string STOCKCODE;
        long ORDERQTY;
        long AMOUNT;
        bool ISEXEC;

        LOGWRITER LR;

        //bool isWait = true;
        bool isSucc = true;    

        // 100% 현금 조회용
        XAQueryClass XAQ_02200 = new XAQueryClass();

        // Buy 주문용
        XAQueryClass XAQ_00600 = new XAQueryClass();

        public ETRADE_BUY(string aAccountnumber, string aStockCode, long aOrderQty, long aAmount, bool aIsExec, LOGWRITER lr)
        {
            try
            {
                ACCOUNTNUMBER = aAccountnumber;
                STOCKCODE = aStockCode;
                ORDERQTY = aOrderQty;
                AMOUNT = aAmount;
                ISEXEC = aIsExec;

                LR = new LOGWRITER(lr);
            }
            catch(Exception e)
            {
                EventLog.WriteEntry("ETRADE_BUY-ETRADE_BUY", e.ToString());
            }
        }

        public bool BUY()
        {
            try
            {
                //XAQ_02200.ResFileName = ".\\Res\\CSPAQ02200.res";
                //XAQ_02200.ReceiveMessage += XAQ_02200_ReceiveMessage;
                //XAQ_02200.SetFieldData("CSPAQ02200InBlock1", "MgmtBrnNo", 0, " ");
                //XAQ_02200.SetFieldData("CSPAQ02200InBlock1", "AcntNo", 0, ACCOUNTNUMBER);

                //if (ISEXEC)
                //    XAQ_02200.SetFieldData("CSPAQ02200InBlock1", "Pwd", 0, "miji1212");
                //else
                //    XAQ_02200.SetFieldData("CSPAQ02200InBlock1", "Pwd", 0, "miji12!@");

                //XAQ_02200.Request(false);

                //while(isWait)
                //{
                //    Thread.Sleep(1);
                //}

                // 매수주문
                XAQ_00600.ResFileName = ".\\Res\\CSPAT00600.res";
                XAQ_00600.ReceiveMessage += XAQ_00600_ReceiveMessage;

                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, ACCOUNTNUMBER);

                if (ISEXEC)
                    XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji1212");
                else
                    XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji12!@");

                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, STOCKCODE);
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, ORDERQTY.ToString());
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "2");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");
                XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");

                LR(LogType.INFORMATION, string.Format("매수 주문요청 종목코드 : {0} , 수량 : {1} ", STOCKCODE, ORDERQTY.ToString()));
                if (XAQ_00600.Request(false) < 0)
                {
                    LR(LogType.ERROR, "주문전송 오류 " + STOCKCODE);
                    isSucc = false;
                }

                //isWait = false;

                return isSucc;
            }
            catch(Exception e)
            {
                EventLog.WriteEntry("ETRADE_BUY-BUY", e.ToString());
                return false;
            }
        }

        private void XAQ_00600_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            try
            {
                LR(LogType.INFORMATION, string.Format("매수주문 요청 응답 : {0} {1} {2}", bIsSystemError.ToString(), nMessageCode, szMessage));
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("ETRADE_BUY-ReceiveMessage", e.ToString());
            }
        }

        #region 주문가능 금액 Received
		//void XAQ_02200_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        //{
        //    LR(LogType.INFORMATION, string.Format("100% 주문가능금액 요청 응답 {0} {1} {2}", bIsSystemError.ToString(), nMessageCode, szMessage));

        //    if (!bIsSystemError)
        //    {
        //        long AbleAmout = long.Parse(XAQ_02200.GetFieldData("CSPAQ02200OutBlock2", "MgnRat100pctOrdAbleAmt", 0));
        //        LR(LogType.INFORMATION, string.Format("100% 주문가능금액 : {0} ", AbleAmout.ToString()));

        //        if (AMOUNT > AbleAmout)
        //        {
        //            LR(LogType.WARNING, "잔액부족 " + STOCKCODE + " 주문금액 : " + AMOUNT.ToString() + " 잔액 : " + AbleAmout.ToString());

        //            isSucc = false;
        //            isWait = false;
                    
        //            return;
        //        }

        //        // 매수주문
        //        XAQ_00600.ResFileName = ".\\Res\\CSPAT00600.res";
        //        //XAQ_00600.ReceiveMessage += XAQ_00600_ReceiveMessage;

        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, ACCOUNTNUMBER);

        //        if (ISEXEC)
        //            XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji1212");
        //        else
        //            XAQ_00600.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, "miji12!@");

        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, STOCKCODE);
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, ORDERQTY.ToString());
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "2");
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03");
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");
        //        XAQ_00600.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");

        //        LR(LogType.INFORMATION, string.Format("매수 주문요청 종목코드 : {0} , 수량 : {1} ", STOCKCODE, ORDERQTY.ToString()));
        //        if (XAQ_00600.Request(false) < 0)
        //        {
        //            LR(LogType.ERROR, "주문전송 오류 " + STOCKCODE);
        //            isSucc = false;
        //        }

        //        isWait = false;

        //    } 
	
        //}
    #endregion
    }
}

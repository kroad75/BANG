using BANG.Common.Log;
using BANG.CYBOSPLUS.DAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XA_DATASETLib;
using XA_SESSIONLib;

namespace OrderForm
{
    public partial class Order : Form
    {
        delegate void SetTextCallback(string text);

        XASessionClass myXASessionClass = new XASessionClass();

        // 주문용

        // 100% 가용금액 조회용
        XAQueryClass XAQ_02200 = new XAQueryClass();

        //bool isTimerProcess = false;

        string ETRADEAccountNumber;
        BANG.Common.Timer.MyThreadTimer myTimer;
        BANG.Common.Timer.MyThreadTimer myTimerServer;

        string ConnectionString;
        string logPath;
        BANG.Common.Log.FileLog flog;

        public Order()
        {
            InitializeComponent();

            myXASessionClass._IXASessionEvents_Event_Login += new _IXASessionEvents_LoginEventHandler(myXASessionClass__IXASessionEvents_Event_Login);

            ConnectionString = GetDbConnStr();
            logPath = GetLogPath();
            flog = new BANG.Common.Log.FileLog("ORDERFORM", logPath, BANG.Common.Log.LogFileInteval.DAILY);

            AutoLogIn_Exec();
            button2.BackColor = Color.Green;

            myTimer = new BANG.Common.Timer.MyThreadTimer(1100);
            myTimer.OnTick += myTimer_OnTick;
            myTimer.Start();

            myTimerServer = new BANG.Common.Timer.MyThreadTimer(1000 * 60 * 10);
            myTimerServer.OnTick += myTimerServer_OnTick;
            myTimerServer.Start();

        }

        void myTimerServer_OnTick(object state)
        {
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                                DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return;
                }

                DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
                DateTime eDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 40, 0);

                if (DateTime.Now >= sDate &&
                    DateTime.Now <= eDate
                    )
                {
                    LogWrite(LogType.INFORMATION, "SERVER DISCONNECT");
                    myXASessionClass.DisconnectServer();

                    LogWrite(LogType.INFORMATION, "SERVER CONNECT");
                    AutoLogIn_Exec();
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-myTimerServer_OnTick", e.ToString());
            }
        }

        void SellAction(object aDr)
        {
            try
            {
                DataRow dr = (DataRow)aDr;

                string StockCode = dr["STOCKCODE"].ToString();
                long volume = long.Parse(dr["VOLUME"].ToString());
                long price = long.Parse(dr["SELL_POINT"].ToString());

                ETRADE_SELL es = new ETRADE_SELL(ETRADEAccountNumber, dr["STOCKCODE"].ToString(), volume, rbExec.Checked, LogWrite);

                if (es.SELL())
                {
                    LogWrite(LogType.INFORMATION, string.Format("{0} 시장가 {1} 주 매도주문성공", dr["STOCKCODE"].ToString(), volume.ToString()));

                    SqlDal sd = new SqlDal(ConnectionString);
                    sd.MarkSellFlag(dr["RAWID"].ToString());
                    sd.PlusBalance(price * volume);
                }
                else
                    LogWrite(LogType.ERROR, string.Format("{0} 시장가 {1} 주 매도주문실패", dr["STOCKCODE"].ToString(), volume.ToString()));

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-SellAction", e.ToString());
            }

        }

        string GetDbConnStr()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["ConnString"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        string GetLogPath()
        {
            //throw new NotImplementedException();
            //GlobalConfigProvider.GlobalConfigProviderClient gcp = new GlobalConfigProvider.GlobalConfigProviderClient();
            try
            {

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
                return appSettings["LogDir"].ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm", e.ToString());
            }

            return "";

            //return "Data Source=192.168.0.37;Initial Catalog=CYBOSPLUS;user id=sa;password=miji12!@";
        }

        private void myXASessionClass__IXASessionEvents_Event_Login(string szCode, string szMsg)
        {
            try
            {
                if (szCode == "0000")
                {
                    ETRADEAccountNumber = myXASessionClass.GetAccountList(0);

                    tbAccountNumber.Text = ETRADEAccountNumber;
                    button2.Enabled = true;
                    LogWrite(LogType.INFORMATION, "로그인 성공");
                }
                else
                {
                    LogWrite(LogType.INFORMATION, "로그인 실패 : " + szCode + " , " + szMsg);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-myXASessionClass__IXASessionEvents_Event_Login", e.ToString());
            }
        }

        void AutoLogIn_Exec()
        {
            try
            {
                bool isConnect = false;
                if (rbExec.Checked)
                {
                    isConnect = myXASessionClass.ConnectServer("demo.etrade.co.kr", 20001);
                }
                else
                {
                    isConnect = myXASessionClass.ConnectServer("hts.etrade.co.kr", 20001);
                    //isConnect = myXASessionClass.ConnectServer("127.0.0.1", 20001);
                }

                if (!isConnect)
                {
                    //MessageBox.Show(myXASessionClass.GetLastError().ToString(), "SERVER ERROR");
                    LogWrite(LogType.ERROR, myXASessionClass.GetLastError().ToString());
                }

                // ETRADE 로그인

                bool isLoginOK = false;
                if (rbExec.Checked)
                {
                    isLoginOK = myXASessionClass.Login("kroad75w", "miji12!@", "miji1212!@", 0, false);
                }
                else
                {
                    isLoginOK = myXASessionClass.Login("kroad75", "miji12!@", "miji1212!@", 0, true);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-AutoLogIn_Exec", e.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbAccountNumber.Text == "")
            {
                bool isConnect = false;
                if (rbExec.Checked)
                {
                    isConnect = myXASessionClass.ConnectServer("demo.etrade.co.kr", 20001);
                }
                else
                {
                    isConnect = myXASessionClass.ConnectServer("hts.etrade.co.kr", 20001);
                    //isConnect = myXASessionClass.ConnectServer("127.0.0.1", 20001);
                }

                if (!isConnect)
                {
                    MessageBox.Show(myXASessionClass.GetLastError().ToString(), "SERVER ERROR");
                    LogWrite(LogType.ERROR, myXASessionClass.GetLastError().ToString());
                }

                // ETRADE 로그인

                bool isLoginOK = false;
                if (rbExec.Checked)
                {
                    isLoginOK = myXASessionClass.Login("kroad75w", "miji12!@", "miji1212!@", 0, false);
                }
                else
                {
                    isLoginOK = myXASessionClass.Login("kroad75", "miji12!@", "miji1212!@", 0, true);
                }

                LogWrite(LogType.INFORMATION, isLoginOK.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.BackColor == Color.Red)
            {
                // 거래시작
                button2.BackColor = Color.Green;
                //isTimerProcess = true;

                myTimer.Start();
            }
            else
            {
                // 거래중단
                button2.BackColor = Color.Red;
                //isTimerProcess = false;

                myTimer.Stop();
            }
        }

        void myTimer_OnTick(object state)
        {
            try
            {
                //if (isTimerProcess)
                //{
                myTimer.Stop();

                //if (!myXASessionClass.IsConnected())
                //{
                //    LogWrite(LogType.INFORMATION, "서버 Disconnected");
                //    myXASessionClass.DisconnectServer();
                //    AutoLogIn_Exec();

                //    //for (int i = 0; i < 3000; i++)
                //    //    Thread.Sleep(1);
                //}
                //else
                //    //LogWrite(LogType.INFORMATION, "서버 Connected");

                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                                DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    myTimer.Start();
                    return;
                }

                DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
                DateTime eDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 30, 0);

                DateTime EndMarketSDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 10, 0);
                DateTime EndMarketEDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 30, 0);

                if ((DateTime.Now >= sDate && DateTime.Now < eDate)
                    || (DateTime.Now >= EndMarketSDate && DateTime.Now < EndMarketEDate)
                    )
                {

                    SqlDal sd = new SqlDal(ConnectionString);
                    DataSet dsSellItem = sd.GetSellItem();

                    if (dsSellItem != null && dsSellItem.Tables.Count > 0 && dsSellItem.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsBuyItem.Tables[0].Rows)
                        //{
                        //Thread t = new Thread(OrderAction);
                        //t.Start(dr);
                        SellAction(dsSellItem.Tables[0].Rows[0]);
                        //}
                    }
                    else //if (DateTime.Now <= sd.GetBuyLimitTime())
                    {
                        DataSet dsBuyItem = sd.GetBuyItem();

                        if (dsBuyItem != null && dsBuyItem.Tables.Count > 0 && dsBuyItem.Tables[0].Rows.Count > 0)
                        {
                            if (dsBuyItem.Tables[0].Rows[0]["buy_method"].ToString().ToUpper().StartsWith("BUY"))
                            {
                                if (sd.isOverTotalEarningRate())
                                {
                                    LogWrite(LogType.INFORMATION, string.Format("전체 목표수익율 달성으로 가상주문 : {0} ", dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()));
                                    OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                    myTimer.Start();
                                    return;
                                }

                                if (sd.isOverStockEarningRate(dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()))
                                {
                                    LogWrite(LogType.INFORMATION, string.Format("개별종목 목표수익율 달성으로 가상주문 : {0} ", dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()));
                                    OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                    myTimer.Start();
                                    return;
                                }

                                if (sd.GetHasTodayFailHistoryInSomeMin(dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString(), dsBuyItem.Tables[0].Rows[0]["buy_method"].ToString()))
                                {
                                    LogWrite(LogType.INFORMATION, string.Format("개별종목 동일매수조건으로 5분내 Fail 이력존재 가상주문 : {0} ", dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()));
                                    OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                    myTimer.Start();
                                    return;
                                }
                            }

                            if (sd.GetIsSideCar() == true)
                            {
                                LogWrite(LogType.WARNING, "자체 SIDE CAR 발동");
                                OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                myTimer.Start();
                                return;
                            }

                            if (sd.GetIsSideCar2() == true)
                            {
                                LogWrite(LogType.WARNING, "자체 SIDE CAR - 2 발동");
                                OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                myTimer.Start();
                                return;
                            }

                            if (sd.isDayBottom(dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()) == true)
                            {
                                LogWrite(LogType.WARNING, "하한가 종목 매수 금지");
                                OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                myTimer.Start();
                                return;
                            }

                            if (sd.isGoodStockStatus(dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()) == false)
                            {
                                LogWrite(LogType.WARNING, "종목상태 불량(CB,관리종목,투자경고,거래정지,불성실공시)");
                                OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                myTimer.Start();
                                return;
                            }

                            if (sd.isTradingToday(dsBuyItem.Tables[0].Rows[0]["STOCKCODE"].ToString()) == false)
                            {
                                LogWrite(LogType.WARNING, "당일 거래이력 없음");
                                OrderActionVirtual(dsBuyItem.Tables[0].Rows[0]);
                                myTimer.Start();
                                return;
                            }


                            OrderAction(dsBuyItem.Tables[0].Rows[0]);

                            //}
                        }
                    }
                }
                //}
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-myTimer_OnTick", e.ToString());
            }

            myTimer.Start();
        }

        //void SellAction(object aDr)
        //{
        //    DataRow dr = (DataRow)aDr;

        //    SellETRADE(dr["STOCKCODE"].ToString(),2.3,-1.7);

        //    SqlDal sd = new SqlDal(ConnectionString);
        //    sd.MarkSellFlag(dr["RAWID"].ToString());
        //}

        void OrderAction(object aDr)
        {
            try
            {
                DataRow dr = (DataRow)aDr;
                long amount = long.Parse(dr["AMOUNT"].ToString());
                long price = long.Parse(dr["BUY_POINT"].ToString());
                long volume = long.Parse(dr["VOLUME"].ToString());

                SqlDal sd = new SqlDal(ConnectionString);

                if (sd.isCanBuy(price * volume))
                {
                    ETRADE_BUY eb = new ETRADE_BUY(ETRADEAccountNumber, dr["STOCKCODE"].ToString(), volume, amount, rbExec.Checked, LogWrite);

                    if (eb.BUY())
                    {
                        sd.MinusBalance(price * volume);
                        LogWrite(LogType.INFORMATION, string.Format("{0} 시장가 {1} 주 매수주문성공", dr["STOCKCODE"].ToString(), volume.ToString()));
                    }
                    else
                        LogWrite(LogType.ERROR, string.Format("{0} 시장가 {1} 주 매수주문실패", dr["STOCKCODE"].ToString(), volume.ToString()));


                    sd.MarkBuyFlag(dr["RAWID"].ToString());
                }
                else
                {
                    LogWrite(LogType.ERROR, string.Format("Balance 부족으로 {0} 매수주문 취소", dr["STOCKCODE"].ToString()));
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-OrderAction", e.ToString());
            }

        }

        void OrderActionVirtual(object aDr)
        {
            try
            {
                DataRow dr = (DataRow)aDr;
                long amount = long.Parse(dr["AMOUNT"].ToString());
                long price = long.Parse(dr["BUY_POINT"].ToString());
                long volume = long.Parse(dr["VOLUME"].ToString());

                SqlDal sd = new SqlDal(ConnectionString);

                if (sd.isCanBuy(price * volume))
                {
                    //ETRADE_BUY eb = new ETRADE_BUY(ETRADEAccountNumber, dr["STOCKCODE"].ToString(), volume, amount, rbExec.Checked, LogWrite);

                    //if (eb.BUY())
                    {
                        sd.MinusBalance(price * volume);
                        LogWrite(LogType.INFORMATION, string.Format("[가상주문] {0} 시장가 {1} 주 매수주문성공", dr["STOCKCODE"].ToString(), volume.ToString()));
                    }
                    //else
                    //    LogWrite(LogType.ERROR, string.Format("{0} 시장가 {1} 주 매수주문실패", dr["STOCKCODE"].ToString(), volume.ToString()));


                    sd.MarkBuyFlag(dr["RAWID"].ToString());
                }
                else
                {
                    LogWrite(LogType.ERROR, string.Format("[가상주문] Balance 부족으로 {0} 매수주문 취소", dr["STOCKCODE"].ToString()));
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("[가상주문] OrderForm-OrderAction", e.ToString());
            }

        }


        //public void SellETRADE(string aStockCode,double aPlusRate,double aMinusRate)
        //{
        //    // 보유종목 수익률 검사
        //    XAQueryClass XAQ_T0424 = new XAQueryClass();
        //    XAQ_T0424.ResFileName = ".\\Res\\t0424.res";
        //    XAQ_T0424.SetFieldData("t0424InBlock", "accno", 0, ETRADEAccountNumber);

        //    if (rbExec.Checked)
        //        XAQ_T0424.SetFieldData("t0424InBlock", "passwd", 0, "miji1212");
        //    else
        //        XAQ_T0424.SetFieldData("t0424InBlock", "passwd", 0, "miji12!@");

        //    // aEarnRate 이상 수익률 미체결 잔고(매도) 여부 검사

        //    // 매도 주문
        //}

        private void Order_Load(object sender, EventArgs e)
        {
            SqlDal sd = new SqlDal(ConnectionString);
            System.Collections.Generic.List<String> sl = sd.GetAllStockCode();

            LogWrite(LogType.INFORMATION, "종목수 " + sl.Count.ToString());

            foreach (string code in sl)
            {
                LogWrite(LogType.INFORMATION, code);
            }


        }

        public void LogWrite(LogType logType, string msg)
        {
            try
            {
                SetText(string.Format("{0} : [{1}]\t{2}{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    logType == LogType.ERROR ? "ERROR" : logType == LogType.INFORMATION ? "INFORMATION" : "WARNING"
                    , msg, Environment.NewLine));

                flog.write(logType, msg);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-LogWrite", e.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //BuyETRADE("A060980", 64800, 1000000);
            //myTimer_OnTick(null);

            //AutoLogIn_Exec();
            ETRADE_BUY eb = new ETRADE_BUY(ETRADEAccountNumber, textBox1.Text, long.Parse(textBox3.Text), long.Parse(textBox2.Text), true, LogWrite);
            eb.BUY();
            //AutoLogOut_Exec();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //SellETRADE("A060980", 2.3, -1.7);
            //myTimerSell_OnTick(null);
        }

        public void SetText(string text)
        {
            try
            {
                if (this.tbMsg.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.tbMsg.AppendText(text);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("OrderForm-SetText", e.ToString());
            }
        }

        private void Order_FormClosing(object sender, FormClosingEventArgs e)
        {
            myTimer.Stop();
            myTimerServer.Stop();
        }


    }
}


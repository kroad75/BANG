using BANG.Common.Log;
using BANG.CYBOSPLUS.Trader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeniusTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dbConnString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.48)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(sid=cp2)));User Id=cybosplus;Password=miji1212;";
            List<string> gStockCodeList = new List<string>();
            gStockCodeList.Add("A114090");

            Genius g = new Genius(dbConnString, gStockCodeList, LogWrite);
            g.Process();
        }

        public void LogWrite(LogType logType, string msg)
        {
            //flog.write(logType, msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string dbConnString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.48)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(sid=cp2)));User Id=cybosplus;Password=miji1212;";

            //GeniusBaseSignal g = new GeniusBaseSignal(dbConnString, LogWrite);

            //string oldRawid = g.Process(textBox1.Text);
            //textBox1.Text = oldRawid;
        }
    }
}

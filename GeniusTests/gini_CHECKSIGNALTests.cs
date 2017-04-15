using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BANG.CYBOSPLUS.Trader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BANG.CYBOSPLUS.Trader.Tests
{
    [TestClass()]
    public class gini_CHECKSIGNALTests
    {
        [TestMethod()]
        public void isGoodTimingTest()
        {
            //Assert.Fail();
            //gini_CHECKSIGNAL gS = new gini_CHECKSIGNAL("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.48)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(sid=cp2)));User Id=cybosplus;Password=miji1212;");
            //bool ret = gS.isGoodTiming("A002720");
            //string sigChar = gS.SIGNAL;

            gini_GRADIENT gS = new gini_GRADIENT("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.48)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(sid=cp2)));User Id=cybosplus;Password=miji1212;");
            bool ret = gS.isGoodTiming("A002720");
            string sigChar = gS.BIAS;

        }
    }
}

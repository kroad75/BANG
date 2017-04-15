using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyConditionTester
{
    class Program
    {
        static string ConnString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.48)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(sid=cp2)));User Id=cybosplus;Password=miji1212;";

        const int PLUS = 0;
        const int MINUS = 1;
        const int NODATA = -1;

        static void Main(string[] args)
        {
            

            int[, ,] kb3 = new int[2, 2, 2];
            int last, last2, last3;

            BANG.CYBOSPLUS.DAL.SqlDal sd = new BANG.CYBOSPLUS.DAL.SqlDal(ConnString);

            DataSet ds = sd.GetPredicRawData("A000060");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                last = last2 = last3 = NODATA;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string chr = dr["RESULT"].ToString();

                    if(chr=="+")
                    {
                        last3 = last2;
                        last2 = last;
                        last = PLUS;
                    }
                    else
                    {
                        last3 = last2;
                        last2 = last;
                        last = MINUS;
                    }

                    learning(last, last2, last3, kb3);
                    prediction(last, last2, last3, kb3);
                }

                Console.WriteLine("학습결과");
                Console.WriteLine("+++ : " + kb3[PLUS, PLUS, PLUS]);
                Console.WriteLine("++- : " + kb3[PLUS, PLUS, MINUS]);
                Console.WriteLine("+-+ : " + kb3[PLUS, MINUS, PLUS]);
                Console.WriteLine("+-- : " + kb3[PLUS, MINUS, MINUS]);
                Console.WriteLine("-++ : " + kb3[MINUS, PLUS, PLUS]);
                Console.WriteLine("-+- : " + kb3[MINUS, PLUS, MINUS]);
                Console.WriteLine("--+ : " + kb3[MINUS, MINUS, PLUS]);
                Console.WriteLine("--- : " + kb3[MINUS, MINUS, MINUS]);
            }
        }

        private static void prediction(int last, int last2, int last3, int[, ,] kb3)
        {
            if (last != NODATA && last2 != NODATA && last3 != NODATA)
            {
                if (kb3[last, last2, PLUS] >= kb3[last, last2, MINUS])
                    Console.WriteLine("+");
                else
                    Console.WriteLine("-");
            }
        }

        private static void learning(int last, int last2, int last3, int[, ,] kb3)
        {
            if (last != NODATA && last2 != NODATA && last3 != NODATA)
                ++kb3[last, last2, last3];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BANG.Common.Log
{

    public enum LogFileInteval
    {
        HOURLY, DAILY, MONTHLY
    };

    public enum LogType
    {
        INFORMATION, WARNING, ERROR
    };

    public class FileLog
    {
        static Object oLock = new object();
        LogFileInteval _lfi;
        FileStream fs;
        StreamWriter sw;

        string _logPath;
        string _prefix;

        public string PREFIX
        {
            get
            {
                return _prefix;
            }
        }
        public string LOGPATH
        {
            get
            {
                return _logPath;
            }
        }
        public LogFileInteval LOGFILEINTEVAL
        {
            get
            {
                return _lfi;
            }
        }

        public FileLog(string prefix, string logpath, LogFileInteval lfi)
        {
            this._prefix = prefix;

            if (!logpath.EndsWith("\\"))
                logpath = logpath + "\\";
            this._logPath = logpath + prefix + "\\";
            DirectoryInfo di = new DirectoryInfo(this._logPath);
            if (!di.Exists)
                di.Create();

            this._lfi = lfi;
        }

        public bool write(LogType lt, string msg)
        {
            lock (oLock)
            {
                bool retBool = true;
                string FileName = string.Empty;
                try
                {
                    //FileStream fs = new FileStream("", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    if (this.LOGFILEINTEVAL == LogFileInteval.MONTHLY)
                    {
                        FileName = string.Format("{0}{1}{2}.log",
                            this.PREFIX
                            , DateTime.Now.Year.ToString()
                            , DateTime.Now.Month.ToString("00")
                            );
                    }
                    else if (this.LOGFILEINTEVAL == LogFileInteval.DAILY)
                    {
                        FileName = string.Format("{0}{1}{2}{3}.log",
                            this.PREFIX
                            , DateTime.Now.Year.ToString()
                            , DateTime.Now.Month.ToString("00")
                            , DateTime.Now.Day.ToString("00")
                            );
                    }
                    else if (this.LOGFILEINTEVAL == LogFileInteval.HOURLY)
                    {
                        FileName = string.Format("{0}{1}{2}{3}{4}.log",
                            this.PREFIX
                            , DateTime.Now.Year.ToString()
                            , DateTime.Now.Month.ToString("00")
                            , DateTime.Now.Day.ToString("00")
                            , DateTime.Now.Hour.ToString("00")
                            );
                    }


                    fs = new FileStream(this.LOGPATH + FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs);

                    if (lt == LogType.ERROR)
                    {
                        sw.WriteLine("[{0}]\t[{1}]\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "ERROR", msg);
                    }
                    else if (lt == LogType.INFORMATION)
                    {
                        sw.WriteLine("[{0}]\t[{1}]\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "INFORMATION", msg);
                    }
                    else if (lt == LogType.WARNING)
                    {
                        sw.WriteLine("[{0}]\t[{1}]\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "WARNING", msg);
                    }
                }

                catch (Exception e)
                {
                    EventLog.WriteEntry("Bang.Common.LogManager.FileLogManager", e.ToString());
                    retBool = false;
                }
                finally
                {
                    sw.Close();
                    fs.Close();
                }

                return retBool;
            }
        }
    }
}

using DbConnectionClassLib.Tables;
using HelperClassLib;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbInstance = DbConnectionClassLib.Data.DatabaseInstance;
using static HelperClassLib.Helpers.HelperClass;

namespace ConsoleApp
{
    class Program
    {
        private static int counter = 0;
        private static System.Timers.Timer aTimer;
        private static double _timerIntervalmilisec = 60 * 1000; // 60 mp
        public static bool OnTimedEventIsRunning = false;

        static void Main(string[] args)
        {
            string strBCTresult = "";
            try
            {
                //ConsoleHelper.UpdateGitRevision();return;
                Console.WriteLine("====== CONSOLE APP START " + DateTime.Now.ToDateTimeString() + " ======\n");
                //DbConnectionClassLib.Data.SqlCommands.Get_DbLog();
                //return;
                ConsoleHelper.UpdateGitRevision();
                CreateTimer();
                RefreshTimerInterval();
#if LOCALDOCKERMYSQL
                Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP STARTED ", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"));
#endif
                strBCTresult = BackgroundConsoleTask().Result;
                Console.WriteLine("CONSOLE APP Main() STOPPED \n" + strBCTresult + "\n===============================\n\n");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Exception ConsoleAppException = new Exception(" ****** EXCEPTION ConsoleApp Main() " + DateTime.Now.ToDateTimeString(), ex);
                Helper.LogException(ConsoleAppException);
                throw ex;
            }
        }

        private static void RefreshTimerInterval()
        {
            _timerIntervalmilisec = 300000; // 5perc
            try
            {
                string strIntervalSec = DbInstance.Db.db_settings.Where(s => s.Key == "INTERVALSEC").Select(s => s.Value).FirstOrDefault();
                if (double.TryParse(strIntervalSec, out double tmpInterval))
                {
                    tmpInterval = tmpInterval * 1000;
                    Console.WriteLine("\n\nRefreshTimerInterval " + tmpInterval + "ms");
                }
                _timerIntervalmilisec = tmpInterval;
            }
            catch
            {

            }
            finally
            {
                aTimer.Interval = _timerIntervalmilisec;
            }
        }

        private async static Task<string> BackgroundConsoleTask()
        {
            string strBgTaskResult = "";
            try
            {
                CreateLastUpdateRow();
                int cntr = 0;
                bool stop = false;
                while (stop == false)
                {
                    if (cntr == 0)
                        Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP STARTED ", "DOCKER CONSOLE APP STARTED :" + DateTime.Now.ToDateTimeString());
                    DbInstance.DbRefresh();
                    DBSettings CARUNNING = DbInstance.Db.db_settings.Where(s => s.Key == "CARUNNING").FirstOrDefault();
                    if (CARUNNING != null && !string.IsNullOrEmpty(CARUNNING.Value) && CARUNNING.Value.ToUpper() == "EXIT")
                    {
                        aTimer.Enabled = false;
                        CARUNNING.Value = "EXITED: " + DateTime.Now.ToDateTimeString();
                        strBgTaskResult = CARUNNING.Value;
                        DbInstance.Db.SaveChanges();
                        stop = true;
                        break;
                    }
                    if (CARUNNING != null && !string.IsNullOrEmpty(CARUNNING.Value) && CARUNNING.Value.ToUpper() == "FALSE")
                    {
                        Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP STOPPED ", "IF CARUNNING.Value.ToUpper() == \"FALSE\"", true);
                        CARUNNING.Value = "STOPPED: " + DateTime.Now.ToDateTimeString();
                        DbInstance.Db.SaveChanges();
                        aTimer.Enabled = false;
                    }
                    else
                    {
                        aTimer.Enabled = true;
                        //Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP RUNNING ", "DOCKER CONSOLE APP RUNNING CNTR= " + cntr + "  " + DateTime.Now.ToDateTimeString());
                    }
                    await Task.Delay(60000);
                    Console.WriteLine("CONSOLE APP BackgroundConsoleTask run... " + DateTime.Now.ToDateTimeString());
                    cntr++;
                }
            }
            catch (Exception ex)
            {
                strBgTaskResult += "\nMETHOD: " + new StackTrace().GetFrame(0).GetMethod();
                strBgTaskResult += Helper.LogException(ex);
                strBgTaskResult += "\nTIME " + DateTime.Now.ToDateTimeString();
                Console.WriteLine(strBgTaskResult);
                //aTimer.Enabled = false;
                Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP EXCEPTION ", "DOCKER CONSOLE APP EXCEPTION :" + DateTime.Now.ToDateTimeString() + "\n" + strBgTaskResult);
            }
            return strBgTaskResult;
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (OnTimedEventIsRunning)
                    return;
                OnTimedEventIsRunning = true;

                var SCHEDULED_FUNCTION_LASTSTART = DbInstance.Db.db_settings.Where(s => s.Key == "SCHEDULED_FUNCTION_LASTSTART").FirstOrDefault();
                if (SCHEDULED_FUNCTION_LASTSTART == null)
                {
                    SCHEDULED_FUNCTION_LASTSTART = new DBSettings
                    {
                        Key = "SCHEDULED_FUNCTION_LASTSTART",
                        Value = DateTime.Now.ToDateTimeString()
                    };
                    DbInstance.Db.db_settings.Add(SCHEDULED_FUNCTION_LASTSTART);
                }
                else
                {
                    SCHEDULED_FUNCTION_LASTSTART.Value = DateTime.Now.ToDateTimeString();
                }
                DbInstance.Db.SaveChanges();

                Console.WriteLine("( ======  OnTimedEvent: CNTR=" + (counter++).ToString() + "  -  " + DateTime.Now.ToDateTimeString() + "  ======");

                RefreshTimerInterval();

                if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (DateTime.Now.TimeOfDay >= new TimeSpan(16, 30, 0) && DateTime.Now.TimeOfDay <= new TimeSpan(21, 0, 0))
                        PortfolioHelper.UpdatePortfolioHunStock();
                }

                bool forcedRun = DbInstance.Db.db_settings.Where(d => d.Key == "UPDATEPRICE").Select(d => bool.Parse(d.Value)).FirstOrDefault();
                if (forcedRun || (DateTime.Today.DayOfWeek != DayOfWeek.Sunday && DateTime.Today.DayOfWeek != DayOfWeek.Monday))
                {
                    if (forcedRun || (DateTime.Now.TimeOfDay >= new TimeSpan(2, 20, 0) && DateTime.Now.TimeOfDay <= new TimeSpan(5, 0, 0)))
                        AlphaVantageHelper.UpdateClosePrices();
                }   

                var SCHEDULED_FUNCTION_LASTRUN = DbInstance.Db.db_settings.Where(s => s.Key == "SCHEDULED_FUNCTION_LASTRUN").FirstOrDefault();
                if (SCHEDULED_FUNCTION_LASTRUN == null)
                {
                    SCHEDULED_FUNCTION_LASTRUN = new DBSettings
                    {
                        Key = "SCHEDULED_FUNCTION_LASTRUN",
                        Value = DateTime.Now.ToDateTimeString()
                    };
                    DbInstance.Db.db_settings.Add(SCHEDULED_FUNCTION_LASTRUN);
                }
                else
                {
                    SCHEDULED_FUNCTION_LASTRUN.Value = DateTime.Now.ToDateTimeString();
                }
                DbInstance.Db.SaveChanges();

                // Force a garbage collection to occur for this demo.
                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ****** EXCEPTION ConsoleApp OnTimedEvent() " + ex);
                Exception ConsoleAppException = new Exception("EXCEPTION ConsoleApp OnTimedEvent() " + DateTime.Now.ToDateTimeString(), ex);
                Helper.LogException(ConsoleAppException);
                //throw ex;
            }
            finally
            {
                OnTimedEventIsRunning = false;
            }
        }

        public static void CreateTimer()
        {
            try
            {
                Console.WriteLine("CreateTimer: " + DateTime.Now.ToDateTimeString());
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = _timerIntervalmilisec;
                aTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Exception ConsoleAppException = new Exception(" ****** EXCEPTION ConsoleApp CreateTimer() " + DateTime.Now.ToDateTimeString(), ex);
                Helper.LogException(ConsoleAppException);
                //throw ex;
            }
        }

        public static void CreateLastUpdateRow()
        {
            try
            {
                Console.WriteLine("CreateLastUpdateRow" + DateTime.Now.ToDateTimeString());
                var SETTING_LASTUPDATE = DbInstance.Db.db_settings.Where(s => s.Key == "LASTUPDATE").FirstOrDefault();
                if (SETTING_LASTUPDATE == null)
                {
                    SETTING_LASTUPDATE = new DBSettings
                    {
                        Key = "LASTUPDATE",
                        Value = DateTime.Now.ToDateTimeString()
                    };
                    DbInstance.Db.db_settings.Add(SETTING_LASTUPDATE);
                }
                else
                {
                    SETTING_LASTUPDATE.Value = DateTime.Now.ToDateTimeString();
                }
                DbInstance.Db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Exception ConsoleAppException = new Exception(" ****** EXCEPTION ConsoleApp CreateLastUpdateRow() " + DateTime.Now.ToDateTimeString(), ex);
                Helper.LogException(ConsoleAppException);
                //throw ex;
            }
        }
    }

}


/*
SELECT * FROM portfolio_hunstock_data 
JOIN portfolio_hunstock ON portfolio_hunstock.id = portfolio_hunstock_data.StockId
WHERE Date > '2019-09-01'
AND portfolio_hunstock.name in ('OTP', 'RICHTER')
ORDER BY name, Date
 * 
    string str = "CONSOLE APP " + DateTime.Now.ToString() + "\n";
    Console.WriteLine(str);
    //GetSymbols();
  
    Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP STARTED", str);
    str = string.Join(", ", Db.Users.Select(u => u.UserName).ToList());
    Console.WriteLine(str);
    Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP USERS", str);
                
    while (true)
    {
        string d = "CONSOLE APP " + DateTime.Now.ToString() + "\n";
        Console.WriteLine(d);
        Email.SendPost("kovacsaurel@gmail.com", "DOCKER CONSOLE APP TESZT", d);
        Thread.Sleep(10000);
    }

//MultiCondTest();
//Email.Send("kovacsaurel@gmail.com", "FTM_TESZT", "msg AEFAFGESARG");
//Email.SendPost("kovacsaurel@gmail.com", "FTM_TESZT", "msg     adfsadf  AEFAFGESARG    " + DateTime.Now.ToStr() );
//Console.WriteLine("SendPost");

//Email.Send2("admin@collectioninventory.com", "FTM_TESZT", "msg AEFAFGESARG");
//Console.WriteLine("Send2");

//context.Tesztek.Add(new Teszt() { text = "TESZZTTTT" });
//context.SaveChanges();


//Console.WriteLine("users.Count :" + users.Count);

//HelperClassLib.Ranking.Otp();
*/

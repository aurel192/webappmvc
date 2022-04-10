using DbConnectionClassLib.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.IO;
using static DbConnectionClassLib.Data.DatabaseInstance;

namespace HelperClassLib.Helpers
{
    public partial class HelperClass
    {
        private static HelperClass helper;
        public static HelperClass Helper
        {
            get
            {
                if (helper == null)
                {
                    helper = new HelperClass();
                }
                return helper;
            }
        }

        public void Logger(string contents)
        {
            try
            {
                contents = "\n" + contents + "\n";
                Console.WriteLine(contents);
                //set up a filestream
                FileStream fs = new FileStream(Constants.logpath + "log_" + DateTime.Today.ToDateStringWithoutSpecialCharacters() + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                //set up a streamwriter for adding text
                StreamWriter sw = new StreamWriter(fs);
                //find the end of the underlying filestream
                sw.BaseStream.Seek(0, SeekOrigin.End);
                //add the text 
                sw.WriteLine(contents);
                //add the text to the underlying filestream
                sw.Flush();
                //close the writer
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logger Exception:" + ex.Message);
            }
        }

        public string LogException(Exception e, HttpRequest Request = null, ApplicationUser user = null)
        {
            string str = "DATE: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                if (user != null)
                    str += "\n\nUSER: " + user.ToString();
                try { str += "\n\nMACHINE: " + System.Environment.MachineName; } catch { }
                if (Request != null)
                {
                    str += "\nPATH: " + Request.Path;
                    str += "\nMETHOD: " + Request.Method;
                    if (Request.Form != null)
                    {
                        str += "\n\nPOST DATA:";
                        foreach (var key in Request.Form.Keys)
                        {
                            StringValues v = "";
                            Request.Form.TryGetValue(key, out v);
                            str += "\n" + key + " = " + v.ToString();
                        }
                    }
                }
                string strException = ExceptionToString(e);
                str += strException;
                Console.WriteLine(str);
                Helper.Logger(str);
                Email.SendPost("error@collectioninventory.com", "FTM EXCEPTION", str);
            }
            catch (Exception ex)
            {
                str += "\n\nError in LogExcepion: " + ex.Message + "\n\n";
            }
            return str;
        }

        public static string ExceptionToString(Exception e)
        {
            string str = "\n\n======================  EXCEPTION ======================";
            while (e != null)
            {
                str += "\n\nMESSAGE: " + e.Message;
                str += "\n\nTYPE: " + e.GetType();
                str += "\n\nSOURCE: " + e.Source;
                str += "\n\nSTACKTRACE: " + e.StackTrace;
                if (e.InnerException != null)
                {
                    str += "\n\n======================  INNER EXCEPTION ======================";
                }
                e = e.InnerException;
            }
            return str;
        }

        public string ReadStringFromFile(string path)
        {
            string text = System.IO.File.ReadAllText(path);
            return text;
        }

        public static string GetRevision()
        {
            try
            {
                string commit = Db.db_settings.Where(s => s.Key == "COMMIT").Select(s => s.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(commit))
                    commit = commit.Substring(0, 6);
                return commit;
            }
            catch
            {
                return "";
            }
        }
    }
}

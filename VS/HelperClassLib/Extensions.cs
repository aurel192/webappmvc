using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HelperClassLib
{
    public static class Extensions
    {
        public static string ToDateStringWithoutSpecialCharacters(this DateTime date)
        {
            return date.Year.ToString() + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0');
        }

        public static string ToDateTimeString(this DateTime dateTime, bool UTC = false)
        {
            if (UTC)
                return dateTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void CopyTo(this object S, object T)
        {
            foreach (var pS in S.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name) continue;
                    (pT.GetSetMethod()).Invoke(T, new object[] { pS.GetGetMethod().Invoke(S, null) });
                }
            };
        }

        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);
        }

        public static string SubstringToMaxLength(this string str, int maxlength)
        {
            string substring = "";
            if (str.Length > maxlength)
                substring = str.Substring(0, maxlength);
            else 
                substring = str;
            return substring;
        }

        public static string ToStr(this DateTime d)
        {
            if (d == null)
            {
                return "1900-01-01";
            }
            return d.ToString("yyyy-MM-dd");
        }

        public static bool InRange(this DateTime date, DateTime from, DateTime to)
        {
            return (date >= from && date <= to);
        }

        public static float ToFloat(this double dValue)
        {
            if (float.IsPositiveInfinity(Convert.ToSingle(dValue)))
            {
                return float.MaxValue;
            }
            if (float.IsNegativeInfinity(Convert.ToSingle(dValue)))
            {
                return float.MinValue;
            }
            return Convert.ToSingle(dValue);
        }

        public static bool IsSet(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static int ToInt(this string str)
        {
            int ret = -1;
            int.TryParse(str, out ret);
            return ret;
        }

        public static DateTime ToDate(this string str, DateTime? alt = null)
        {
            try
            {
                var DateComponents = str.Split('-').ToList();
                if (DateComponents.Count == 3)
                    return new DateTime(DateComponents.ElementAt(0).ToInt(), DateComponents.ElementAt(1).ToInt(), DateComponents.ElementAt(2).ToInt());
            }
            catch (Exception ex) {
                Helpers.HelperClass.Helper.LogException(ex);
            }
            if (alt.HasValue)
                return alt.Value;
            else
                return new DateTime(1970,01,01);
        }

        public static string UpperCaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }

        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static IEnumerable<DateTime> GetDaysInWeek(this DateTime checkDay, DayOfWeek startDayOfWeek = DayOfWeek.Monday)
        {
            int days = startDayOfWeek - checkDay.DayOfWeek;
            DateTime startDate = checkDay.AddDays(days);
            for (var i = 0; i < 7; i++)
            {
                yield return startDate.AddDays(i).Date;
            }
        }

        public static bool IsValidDate(this DateTime date)
        {
            return (date > new DateTime(1900, 1, 1));
        }

    }
}

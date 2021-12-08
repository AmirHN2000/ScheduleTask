using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ScheduleTask.Data;

namespace ScheduleTask.Utils
{
    public static class ExtensionMethods
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                .Value.ToString();
        }
        
        public static int CompareTime(this NecessaryTime time1, NecessaryTime time2){
            if (time1.Hour>time2.Hour)
            {
                return 1;
            }
            else if (time1.Hour==time2.Hour)
            {
                if (time1.Minute>time2.Minute)
                {
                    return 1;
                }
                else if (time1.Hour==time2.Hour)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int CompareDate(this DateTime date1, DateTime date2)
        {
            if (date1.Year>date2.Year)
            {
                return 1;
            }
            else if (date1.Year==date2.Year)
            {
                if (date1.Month>date2.Month)
                {
                    return 1;
                }
                else if (date1.Month==date2.Month)
                {
                    if (date1.Day>date2.Day)
                    {
                        return 1;
                    }
                    else if (date1.Day==date2.Day)
                    {
                        return 0;
                    }
                }
            }
            return -1;
        }
        
        public static string PersianToEnglish(this string persianStr)
        {
            Dictionary<char, char> lettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',['۱'] = '1',['۲'] = '2',['۳'] = '3',['۴'] = '4',['۵'] = '5',['۶'] = '6',
                ['۷'] = '7',['۸'] = '8',['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                persianStr = persianStr.Replace(item, lettersDictionary[item]);
            }
            return persianStr;
        }

        public static DateTime ConvertPersianToGregorianDate(this string persianDate)
        {
            var split = persianDate.Split('/');
            var arr = new int[3];
            for (var i = 0; i < split.Length; i++)
            {
                arr[i] = int.Parse(split[i].PersianToEnglish());
            }

            var pCalender = new PersianCalendar();
            return new DateTime(arr[0],arr[1],arr[2],pCalender);
        }
        
        public static List<string> GetErrorFromModelError(this ModelStateDictionary modelState)
        {
            List<string> errors=new List<string>();
            foreach (var modelStateValue in modelState.Values)
            foreach (var modelError in modelStateValue.Errors)
            {
                errors.Add(modelError.ErrorMessage);
            }
            
            return errors;
        }
    }

    public static class Time
    {
        public static string GetTimeString(byte hour, byte minute)
        {
            var h = hour.ToString();
            var m = minute.ToString();
            if (h.Length==1)
            {
                h = "0" + h;
            }
            if (m.Length==1)
            {
                m = "0" + m;
            }

            return h + ":" + m;
        }
    } 
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Relational.Core.CONVERTERS
{
   internal class SQLHelper
    {
        internal static string WrapSQLName(string s)
        {
            if (IsSQLFunction(s))
            {
                return s;
            }

            if (s != null && !s.StartsWith("["))
            {
                s = "[" + s;
                if (!s.EndsWith("]"))
                {
                    s += "]";
                }
            }
            return s;
        }

        internal static string UNWrapSQLName(string s)
        {
            if (s != null && s.StartsWith("["))
            {
                s = s.Remove(0, 1);
                if (s.EndsWith("]"))
                {
                    s = s.Remove(s.Length - 1, 1);
                }
            }
            return s;
        }


        private static bool IsSQLFunction(string s)
        {
            if (s == null)
                return false;
            if (s.ToUpper().Trim().StartsWith("DATENAME"))
                return true;

            if (s.ToUpper().Trim().StartsWith("CASE"))
                return true;

            return false;
        }


        internal static string JoinColumnsCollection(List<string> columnscollection)
        {
            List<string> result = new List<string>();
            foreach (string s in columnscollection)
            {
                result.Add(WrapSQLName(s));
            }
            return string.Join(",", result);
        }
    }
   public static class SanitizeSQL
   {
       public static string Sanitize(this string stringValue)
       {
           if (null == stringValue)
               return stringValue;
           return stringValue
                       .RegexReplace("-{2,}", "-")                 // transforms multiple --- in - use to comment in sql scripts
                       .RegexReplace(@"[*/]+", string.Empty)      // removes / and * used also to comment in sql scripts
                       .RegexReplace(@"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
       }

       private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith)
       {
           return Regex.Replace(stringValue, matchPattern, toReplaceWith);
       }

       private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
       {
           return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
       }
   }
}

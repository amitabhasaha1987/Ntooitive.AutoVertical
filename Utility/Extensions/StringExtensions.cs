using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class StringExtensions
    {
        public static string GetSafeValue(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }

        public static int WordCount(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            var collection = Regex.Matches(str, @"[\S]+");
            return collection.Count;
        }

        public static int WordCountLoop(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            int c = 0;

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str[i - 1]) == true)
                {
                    if (char.IsLetterOrDigit(str[i]) == true ||
                        char.IsPunctuation(str[i]))
                    {
                        c++;
                    }
                }
            }

            if (str.Length > 2)
            {
                c++;
            }

            return c;
        }

        public static int CharacterCount(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            int result = 0;
            bool lastWasSpace = false;

            foreach (char c in str)
            {
                if (char.IsWhiteSpace(c))
                {
                    // A.
                    // Only count sequential spaces one time.
                    if (lastWasSpace == false)
                    {
                        result++;
                    }
                    lastWasSpace = true;
                }
                else
                {
                    // B.
                    // Count other characters every time.
                    result++;
                    lastWasSpace = false;
                }
            }

            return result;
        }

        public static int NonSpaceCharacterCount(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            int result = 0;

            foreach (char c in str)
            {
                if (!char.IsWhiteSpace(c))
                {
                    result++;
                }
            }

            return result;
        }

        public static string PictureUrlToSize(this string url, int size)
        {
            var pieces = url.Split('/');

            var rep = pieces[pieces.Length - 2];

            if (!rep.StartsWith("s"))
            {
                return url.Replace(pieces[pieces.Length - 2], string.Format("{0}/s{1}x{1}", rep, size));
            }

            return url.Replace(pieces[pieces.Length - 2], string.Format("s{0}x{0}", size));

            //return Regex.Replace(url, @"", string.Format("", size)); ;
        }

        public static string GenderToPronoun(this string gender)
        {
            if (!string.IsNullOrWhiteSpace(gender))
            {
                switch (gender.ToLower())
                {
                    case "male": return "he";
                    case "female": return "she";
                }
            }

            return "she/he";
        }

        public static string GenderToAdjective(this string gender)
        {
            if (!string.IsNullOrWhiteSpace(gender))
            {
                switch (gender.ToLower())
                {
                    case "male": return "his";
                    case "female": return "her";
                }
            }

            return "her/his";
        }

        public static string ReplaceFirstAtStart(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos != 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}

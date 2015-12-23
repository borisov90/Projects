using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Text;

namespace ETEMModel.Helpers.Extentions
{
    public static class Extentions
    {
        #region stringExtentions
        public static bool IsNotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        public static string[] SplitByCharArr(this string text, char[] chars)
        {
            return text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitByConstantChar(this string text)
        {
            return text.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int[] SplitByConstantCharAndParse(this string text)
        {
            return text.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
        }

        public static List<int> SplitByConstantCharAndTryParse(this string text, char[] chars)
        {
            string[] splitedData = text.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            List<int> listParsedItems = new List<int>();
            for (int i = 0; i < splitedData.Length; i++)
            {
                int parsedResultValue;
                bool parseResult = int.TryParse(splitedData[i], out parsedResultValue);
                if (parseResult)
                {
                    listParsedItems.Add(parsedResultValue);
                }
            }

            return listParsedItems;
        }

        public static List<int> SplitByConstantCharAndTryParse(this string text)
        {
            string[] splitedData = text.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            List<int> listParsedItems = new List<int>();
            for (int i = 0; i < splitedData.Length; i++)
            {
                int parsedResultValue;
                bool parseResult = int.TryParse(splitedData[i], out parsedResultValue);
                if (parseResult)
                {
                    listParsedItems.Add(parsedResultValue);
                }
            }

            return listParsedItems;
        }

        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        public static string RemoveSpaces(this string s)
        {
            return s.Replace(" ", "");
        }

        public static bool IsNumber(this string s, bool floatpoint)
        {
            int i;
            double d;
            string withoutWhiteSpace = s.RemoveSpaces();
            if (floatpoint)
                return double.TryParse(withoutWhiteSpace, NumberStyles.Any,
                    Thread.CurrentThread.CurrentUICulture, out d);
            else
                return int.TryParse(withoutWhiteSpace, out i);
        }

        /// <summary>

        /// Reduce string to shorter preview which is optionally ended by some string (...).
        /// </summary>
        /// <param name="s">string to reduce</param>
        /// <param name="count">Length of returned string including endings.</param>
        /// <param name="endings">optional edings of reduced text</param>

        /// <example>
        /// string description = "This is very long description of something";
        /// string preview = description.Reduce(20,"...");
        /// produce -> "This is very long..."
        /// </example>
        /// <returns></returns>

        public static string Reduce(this string s, int count, string endings)
        {
            if (count < endings.Length)
                throw new Exception("Failed to reduce to less then endings length.");
            int sLength = s.Length;
            int len = sLength;
            if (endings != null)
                len += endings.Length;
            if (count > sLength)
                return s; //it's too short to reduce
            s = s.Substring(0, sLength - len + count);
            if (endings != null)
                s += endings;
            return s;
        }

        /// <summary>
        /// true, if is valid email address
        /// archive/2006/11/30/ExtensionMethodsCSharp.aspx
        /// </summary>
        /// <param name="s">email address to test</param>
        /// <returns>true, if is valid email address</returns>

        public static bool IsValidEmailAddress(this string text)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(text);
        }

        /// <summary>
        /// Checks if url is valid. 
        /// from http://www.osix.net/modules/article/?id=586
        /// and changed to match http://localhost
        /// 
        /// complete (not only http) url regex can be found 
        /// at http://internet.ls-la.net/folklore/url-regexpr.html
        /// </summary>
        /// <param name="text"></param>

        /// <returns></returns>
        public static bool IsValidUrl(this string url)
        {
            string strRegex = "^(https?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
        + "|" // allows either IP or domain
        + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
        + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
        + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
        + "(:[0-9]{1,5})?" // port number- :80
        + "((/?)|" // a slash isn't required if there is no file name
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            return new Regex(strRegex).IsMatch(url);
        }

        /// <summary>
        /// Check if url (http) is available.
        /// </summary>
        /// <param name="httpUri">url to check</param>
        /// <example>

        /// string url = "www.codeproject.com;
        /// if( !url.UrlAvailable())
        ///     ...codeproject is not available
        /// </example>
        /// <returns>true if available</returns>
        public static bool UrlAvailable(this string httpUrl)
        {
            if (!httpUrl.StartsWith("http://") || !httpUrl.StartsWith("https://"))
                httpUrl = "http://" + httpUrl;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(httpUrl);
                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse myHttpWebResponse =
                   (HttpWebResponse)myRequest.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// true, if the string contains only digits or float-point.
        /// Spaces are not considred.
        /// </summary>
        /// <param name="s">input string</param>

        /// <param name="floatpoint">true, if float-point is considered</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumberOnly(this string s, bool floatpoint)
        {
            s = s.Trim();
            if (s.Length == 0)
                return false;
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    if (floatpoint && (c == '.' || c == ','))
                        continue;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Remove accent from strings 
        /// </summary>
        /// <example>
        ///  input:  "Příliš žluťoučký kůň úpěl ďábelské ódy."
        ///  result: "Prilis zlutoucky kun upel dabelske ody."
        /// </example>
        /// <param name="s"></param>
        /// <remarks>founded at http://stackoverflow.com/questions/249087/
        /// how-do-i-remove-diacritics-accents-from-a-string-in-net</remarks>
        /// <returns>string without accents</returns>

        public static string RemoveDiacritics(this string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static bool isPhone(this string input)
        {
            var match = Regex.Match(input,
              @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", RegexOptions.IgnoreCase);
            return match.Success;
        }


        public static string extractEmail(this string input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input)) return string.Empty;

            var match = Regex.Match(input, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            return match.Success ? match.Value : string.Empty;
        }

        #endregion

        #region Decimal
        public static string ToStringFormatted(this decimal input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetDefaultNumberFormatInfo());

            return result;
        }
        public static string ToStringFormatted4(this decimal input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetDefaultNumberFormatInfo4());

            return result;
        }
        public static string ToStringNotFormatted(this decimal input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 2));

            return result;
        }
        public static string ToStringNotFormatted4(this decimal input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 4));

            return result;
        }
        public static string ToStringFormatted(this Decimal? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetDefaultNumberFormatInfo());
            }

            return result;
        }
        public static string ToStringFormatted4(this Decimal? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetDefaultNumberFormatInfo4());
            }

            return result;
        }
        public static string ToStringNotFormatted(this Decimal? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 2));
            }

            return result;
        }
        public static string ToStringNotFormatted4(this Decimal? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 4));
            }

            return result;
        } 
        #endregion
        
        #region Int
        public static string ToStringFormatted(this int input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetDefaultNumberIntFormatInfo());

            return result;
        }
        public static string ToStringNotFormatted(this int input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 0));

            return result;
        }
        public static string ToStringNotFormatted(this int? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 0));
            }

            return result;
        }
        public static string ToStringFormatted(this int? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetDefaultNumberIntFormatInfo());
            }

            return result;
        } 
        #endregion

        #region Double
        public static string ToStringFormatted(this double input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetDefaultNumberFormatInfo());

            return result;
        }
        public static string ToStringFormatted4(this double input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetDefaultNumberFormatInfo4());

            return result;
        }
        public static string ToStringNotFormatted(this double input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 2));

            return result;
        }
        public static string ToStringNotFormatted4(this double input)
        {
            string result = string.Empty;

            result = input.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 4));

            return result;
        }
        public static string ToStringNotFormatted(this double? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 2));
            }

            return result;
        }
        public static string ToStringNotFormatted4(this double? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetNumberFormatInfo("", ",", 4));
            }

            return result;
        }
        public static string ToStringFormatted(this double? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetDefaultNumberFormatInfo());
            }

            return result;
        }
        public static string ToStringFormatted4(this double? input)
        {
            string result = string.Empty;

            if (input.HasValue)
            {
                result = input.Value.ToString("N", BaseHelper.GetDefaultNumberFormatInfo4());
            }

            return result;
        }
        #endregion
    }
}
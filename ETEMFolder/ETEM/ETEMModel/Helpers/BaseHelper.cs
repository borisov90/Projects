using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Web.Configuration;
//using log4net;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using ASNS.ZipComponent.Zip;
using ETEMModel.Helpers.Admin;
using System.Web.UI;
using System.Threading;
using System.Linq.Expressions;
using System.IO.Packaging;
using ETEMModel.Helpers.Common;
using System.Net.Mail;
using NLog;
using System.Text.RegularExpressions;

namespace ETEMModel.Helpers
{
    public partial class BaseHelper
    {
        private static Logger log = LogManager.GetLogger("BaseHelper");

        public static string[] range0ot10stot = new string[] { "", "една" };
        public static string[] range0ot10 = new string[] { "нула", "едно", "две", "три", "четири", "пет", "шест", "седем", "осем", "девет" };
        public static string[] range10ot20 = new string[] { "десет", "единадесет", "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет", "деветнадесет" };
        public static string[] range10ot100 = new string[] { "", "десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шестдесет", "седемдесет", "осемдесет", "деветдесет" };
        public static string[] range100ot1000 = new string[] { "", "сто", "двеста", "триста", "четиристотин", "петстотин", "шестстотин", "седемстотин", "осемстотин", "деветстотин" };


        public static NumberFormatInfo GetDefaultNumberFormatInfo()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 2;

            return nfi;
        }

        public static NumberFormatInfo GetDefaultNumberFormatInfo4()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 4;

            return nfi;
        }

        public static NumberFormatInfo GetDefaultNumberIntFormatInfo()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberDecimalDigits = 0;

            return nfi;
        }

        public static NumberFormatInfo GetNumberFormatInfo(string groupSeparator, string separator, int digits)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = groupSeparator;
            nfi.NumberDecimalSeparator = separator;
            nfi.NumberDecimalDigits = digits;

            return nfi;
        }

        public static NumberFormatInfo GetNumberFormatInfo(string separator, int digits)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = separator;
            nfi.NumberDecimalDigits = digits;

            return nfi;
        }

        public static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.DateSeparator = ".";
            dtfi.ShortDatePattern = Constants.SHORT_DATE_PATTERN;

            return dtfi;
        }

        /// <summary>
        /// Връща Hashtable с ключ първата част от стринга и стойност втората част 
        /// </summary>
        /// <param name="parsingString">Стринга е с следния формат: key1=val1&key2=val2&key3=val3</param>
        /// <returns></returns>
        public static Hashtable ParseStringByAmpersand(string parsingString)
        {
            Hashtable result = new Hashtable();

            string val;
            string key;

            string[] keys_values = parsingString.Split('&');

            foreach (string key_value in keys_values)
            {
                string[] kv = key_value.Split('=');

                key = kv[0];
                val = kv[1];

                result.Add(key, val);
            }


            return result;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetCaptionString(string key)
        {
            ResourceManager resource = new ResourceManager("ETEMModel.App_GlobalResources.captions", Assembly.GetExecutingAssembly());

            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("bg-BG");


            string result = "#" + key + "#";

            if (resource.GetString(key) != null)
            {
                result = resource.GetString(key);
            }

            return result;
        }


        public static string CustomTrim(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                return inputString.Trim();
            }
            return inputString;
        }

        public static void Log(string message)
        {

            log.Debug(message);
        }


        public static void LogDebug(string message)
        {
            //log.Debug(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
            log.Debug("\tMessage: " + message);
        }

        public static void LogError(string message)
        {
            SendMailHelper sendMailHelper = new SendMailHelper();

            sendMailHelper.SubjectBG = "INFO: Автоматизирана университетска информационна система";
            sendMailHelper.BodyBG = message;

            ETEMModel.Services.Admin.Administration AdminClientRef = new ETEMModel.Services.Admin.Administration();

            AdminClientRef.SendMailToAdministrator(sendMailHelper);


            log.Error(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
        }

        public static void LogToMail(string message)
        {
            SendMailHelper sendMailHelper = new SendMailHelper();

            sendMailHelper.SubjectBG = "INFO: Автоматизирана университетска информационна система";
            sendMailHelper.BodyBG = message;

            ETEMModel.Services.Admin.Administration AdminClientRef = new ETEMModel.Services.Admin.Administration();

            AdminClientRef.SendMailToAdministrator(sendMailHelper);
        }



        public static List<string> OrderByStringThenByNumberFromTheString(List<string> list)
        {
            List<string> ordered = list.OrderBy(x => string.Join("", x.Where(s => char.IsLetter(s))))
                                        .ThenBy(x => int.Parse(string.Join("", x.Where(s => char.IsDigit(s)))))
                                        .ToList();

            return ordered;
        }

        public static void LogTrace(string message)
        {

            log.Trace(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
        }

        public static long ExtractNumberFromString(string text)
        {
            Match match = Regex.Match(text, @"(\d+)");
            if (match == null)
            {
                return 0;
            }

            int value;
            if (!int.TryParse(match.Value, out value))
            {
                return 0;
            }

            return value;
        }


        public static bool ShowErrors(List<CallContext> listCallContext, BulletedList list, Panel pnl)
        {
            bool result = true;

            List<string> listErrors = new List<string>();

            foreach (var item in listCallContext)
            {
                if (item.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i];
                    }
                    listErrors.AddRange(currentItemErrors);
                }
                else if (item.ResultCode == ETEMEnums.ResultEnum.Warning)
                {
                    listErrors.Add(item.Message);
                }
            }
            if (listErrors.Count > 0)
            {
                foreach (var error in listErrors)
                {
                    var listItem = new ListItem(error);
                    listItem.Attributes.Add("class", "lbResultSaveError");
                    list.Items.Add(listItem);
                }

                pnl.Visible = true;

                result = false;
            }

            return result;
        }

        public static string Decrypt(string encryptedQueryString)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(encryptedQueryString);
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
                des.Key = MD5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(Constants.ENCRYPTION_KEY));
                des.IV = Constants.ENCRYPTION_IV;
                return Encoding.UTF8.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                    buffer,
                    0,
                    buffer.Length
                    )
                    );
            }
            catch (CryptographicException)
            {
                throw new CryptographicException();
            }
            catch (FormatException)
            {
                throw new FormatException();
            }

        }

        public static string Encrypt(string serializedQueryString)
        {

            byte[] buffer = Encoding.UTF8.GetBytes(serializedQueryString);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            des.Key = MD5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(Constants.ENCRYPTION_KEY));
            des.IV = Constants.ENCRYPTION_IV;
            string encriptString = Convert.ToBase64String(
                des.CreateEncryptor().TransformFinalBlock(
                buffer,
                0,
                buffer.Length
                )
                );
            return System.Web.HttpUtility.UrlEncode(encriptString);
        }

        public static Object toDateTimeForDB(string date, string time)
        {
            string returnDate = "";
            string returnTime = "";
            if (isEmptyString(date))
            {
                return DBNull.Value;
            }
            else
            {
                string[] ddmmyy = date.Split('.');

                if (ddmmyy.Length <= 1)
                {
                    ddmmyy = date.Split('/');
                }

                returnDate = ddmmyy[0] + "." + ddmmyy[1] + "." + ddmmyy[2].Replace("г", "").Trim();
            }
            if (!isEmptyString(date))
            {
                string[] hhmm = time.Split(':');

                if (hhmm.Length >= 2)
                {
                    if ((hhmm[0].Length < 3) && hhmm[1].Length < 3)
                    {
                        returnTime = hhmm[0] + ":" + hhmm[1];
                    }
                }
            }
            return returnDate + " " + returnTime;
        }

        private static string StripHTML(string htmlString)
        {

            string pattern = @"<(.|\n)*?>";

            return System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, string.Empty);

        }

        public static DateTime? TextAsDateParseExact(string date)
        {

            DateTime tmpDate;
            if (DateTime.TryParseExact(date, Constants.SHORT_DATE_PATTERN, CultureInfo.InvariantCulture, DateTimeStyles.None, out tmpDate))
            {
                return tmpDate;
            }
            else
            {
                return null;
            }


        }

        public static string[] toStringDateTime(Object datatime)
        {
            string[] dateAndTime = new string[] { "", "" };
            try
            {
                Convert.ToDateTime(datatime);
            }
            catch
            {
                return dateAndTime;
            }

            if (datatime == null || datatime == DBNull.Value)
            {
                return dateAndTime;
            }
            string divider = ".";
            string date = "";
            System.DateTime currentTime = Convert.ToDateTime(datatime);

            if (currentTime.Day < 10)
                date += "0" + currentTime.Day;
            else
                date += currentTime.Day;
            date += divider;

            if (currentTime.Month < 10)
                date += "0" + currentTime.Month;
            else
                date += currentTime.Month;
            date += divider;

            date += currentTime.Year;

            dateAndTime[0] = date;

            if (currentTime.Hour < 10)
                dateAndTime[1] = "0" + currentTime.Hour.ToString();
            else
                dateAndTime[1] = currentTime.Hour.ToString();

            dateAndTime[1] += ":";

            if (currentTime.Minute < 10)
                dateAndTime[1] += "0" + currentTime.Minute.ToString();
            else
                dateAndTime[1] += currentTime.Minute.ToString();

            return dateAndTime;
        }

        public static bool isEmptyString(string str)
        {
            if (str == null || str.Trim().Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public string ConvertCyrToLatin(string arg)
        {
            StringBuilder builder = new StringBuilder(arg, arg.Length + 10);
            //convert lower-case letters
            builder.Replace("а", "a");
            builder.Replace("б", "b");
            builder.Replace("в", "v");
            builder.Replace("г", "g");
            builder.Replace("д", "d");
            builder.Replace("е", "e");
            builder.Replace("ж", "zh");
            builder.Replace("з", "z");
            builder.Replace("и", "i");
            builder.Replace("й", "j");
            builder.Replace("к", "k");
            builder.Replace("л", "l");
            builder.Replace("м", "m");
            builder.Replace("н", "n");
            builder.Replace("о", "o");
            builder.Replace("п", "p");
            builder.Replace("р", "r");
            builder.Replace("с", "s");
            builder.Replace("т", "t");
            builder.Replace("у", "u");
            builder.Replace("ф", "f");
            builder.Replace("х", "h");
            builder.Replace("ц", "c");
            builder.Replace("ч", "ch");
            builder.Replace("ш", "sh");
            builder.Replace("щ", "sht");
            builder.Replace("ъ", "y");
            builder.Replace("ь", "j");
            builder.Replace("ю", "ju");
            builder.Replace("я", "ja");

            //convert upper-case letters
            builder.Replace("А", "A");
            builder.Replace("Б", "B");
            builder.Replace("В", "V");
            builder.Replace("Г", "G");
            builder.Replace("Д", "D");
            builder.Replace("Е", "E");
            builder.Replace("Ж", "Zh");
            builder.Replace("З", "Z");
            builder.Replace("И", "I");
            builder.Replace("Й", "J");
            builder.Replace("К", "K");
            builder.Replace("Л", "L");
            builder.Replace("М", "M");
            builder.Replace("Н", "N");
            builder.Replace("О", "O");
            builder.Replace("П", "P");
            builder.Replace("Р", "R");
            builder.Replace("С", "S");
            builder.Replace("Т", "T");
            builder.Replace("У", "U");
            builder.Replace("Ф", "F");
            builder.Replace("Х", "H");
            builder.Replace("Ц", "C");
            builder.Replace("Ч", "Ch");
            builder.Replace("Ш", "Sh");
            builder.Replace("Щ", "Sht");
            builder.Replace("Ъ", "Y");
            builder.Replace("Ь", "J");
            builder.Replace("Ю", "Ju");
            builder.Replace("Я", "Ja");
            builder.Replace("-", "-");

            return builder.ToString();
        }

        static public decimal? ChechAndConvertDecimalForDB(object obj)
        {
            if (obj.ToString() == "")
                return null;
            else
                return Convert.ToDecimal(obj);
        }

        static public string ChechAndConvertToDecimal(object obj)
        {
            if (obj == null)
                return "";
            else
                return Convert.ToString(obj);
        }

        public static string CheckFolderName(string name)
        {
            string[] symbols = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };

            foreach (string symbol in symbols)
            {
                name = name.Replace(symbol, "");
            }

            return name;

        }

        public static string CheckAndReplaceStringName(string name, string replaceSymbol)
        {
            string[] symbols = { " ", "\\", "/", ",", ".", ":", ";", "*", "?", "'", "\"", "<", ">", "|", "„", "”", "\"", "“" };

            foreach (string symbol in symbols)
            {
                name = name.Replace(symbol, replaceSymbol);
            }

            return name;

        }

        /// <summary>
        /// JavaScript за отваряне на pop-up прозорец, в който ще се визуализират данните.		
        /// Properties: resizable=yes,scrollbars=yes,menubar=no,status=no,width=600,height=600,location=no,toolbar=no
        /// </summary>
        public static string JS_SCRIPT_POPUP_PAGE(string page, string parameters, string newWindowParam)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                parameters = Encrypt(parameters);
            }

            return "window.open(\"" + page + "?" + parameters + "\",'_blank','" + newWindowParam + "');";
        }

        public static string JS_SCRIPT_CONFIRM_MESSAGE(string message)
        {
            return "return confirm('" + message + "');";
        }

        public static string JS_SCRIPT_IS_NUMERIC
        {
            get
            {
                string script = "<script type=\"text/javascript\" language=\"javascript\">" +
                                 "function isNumeric(evt, isDouble) { " +
                                     "var c = (evt.which) ? evt.which : event.keyCode; " +
                                     "if ((c >= 48  && c <= 57) || (isDouble && (c == 44 || c == 46)) || c == 8) { " +
                                         "return true; " +
                                     "} " +
                                     "return false; " +
                                 "} " +
                                "$(function () { " +
                                    "$('.two-digits').keyup(function () { " +
                                        "if ($(this).val().indexOf('.') != -1) { " +
                                            "if ($(this).val().split('.')[1].length > 2) { " +
                                                "if (isNaN(parseFloat(this.value))) return; " +
                                                "this.value = parseFloat(this.value).toFixed(2); " +
                                            "} " +
                                        "} " +
                                        "if ($(this).val().indexOf(',') != -1) { " +
                                            "if ($(this).val().split(',')[1].length > 2) { " +
                                                "if (isNaN(parseFloat(this.value))) return; " +
                                                "this.value = parseFloat(this.value).toFixed(2); " +
                                            "} " +
                                        "} " +
                                        "return this; " +
                                    "}); " +
                                "}); " +
                                "</script>";

                return script;
            }
        }

        public static string JS_SCRIPT_MODAL_WINDOW
        {
            get
            {
                string script = "$('<div id=\"modalWindow\" class=\"blind\"></div>').insertAfter(\".global-container\");";

                return script;
            }
        }

        /// <summary>
        /// Escapes characters to make a MySQL readable query. Taken from MySqlDriverCS, MySQLUtils.cs
        /// </summary>
        /// <param name="str">The string to translate</param>
        /// <returns>The quoted escaped string</returns>
        internal static string MySqlEscapeString(string str)
        {
            StringBuilder ret = new StringBuilder();
            foreach (char c in str)
            {
                if (c == '\0')
                    ret.Append("\\0");
                else if (c == '\n')
                    ret.Append("\\n");
                else if (c == '\t')
                    ret.Append("\\t");
                else if (c == '\b')
                    ret.Append("\\b");
                else if (c == '\r')
                    ret.Append("\\r");
                else if (c == '\'')
                    ret.Append("''");
                else if (c == '\"')
                    ret.Append("\\\"");
                else if (c == '\\')
                    ret.Append("\\\\");
                else
                    ret.Append(c);
            }
            return ret.ToString();
        }

        public static int? ConvertToInt(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return new Nullable<int>();
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = 0;

                int tmpValue = 0;
                if (int.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return new Nullable<int>();
                }
            }


            //if (obj == null || obj == DBNull.Value)
            //{
            //    return new Nullable<int>();
            //}
            //else
            //{
            //    int tmpValue = 0;
            //    if (int.TryParse(obj.ToString(), out tmpValue))
            //    {
            //        return tmpValue;
            //    }
            //    else
            //    {
            //        return new Nullable<int>();
            //    }
            //}
        }

        public static int ConvertToIntOrZero(object obj)
        {

            if (obj == null || obj == DBNull.Value)
            {
                return 0;
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = 0;

                int tmpValue = 0;
                if (int.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return 0;
                }
            }
           
        }

        public static int ConvertToIntOrMinValue(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return int.MinValue;
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = 0;

                int tmpValue = 0;
                if (int.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return int.MinValue;
                }
            }


            //if (obj == null || obj == DBNull.Value)
            //{
            //    return int.MinValue;
            //}
            //else
            //{
            //    int tmpValue = int.MinValue;
            //    if (int.TryParse(obj.ToString(), out tmpValue))
            //    {
            //        return tmpValue;
            //    }
            //    else
            //    {
            //        return int.MinValue;
            //    }
            //}
        }

        public static decimal? ConvertToDecimal(object obj, int decimalDigits)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return new Nullable<decimal>();
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = decimalDigits;

                decimal tmpValue = 0;
                if (decimal.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return new Nullable<decimal>();
                }
            }
        }

        public static decimal? ConvertToDecimal(object obj)
        {
            return ConvertToDecimal(obj, 2);
        }

        public static decimal? ConvertToDecimalAndRound(object obj, int decimalDigits)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return new Nullable<decimal>();
            }
            else
            {
                decimal tmpValue = 0;
                if (decimal.TryParse(obj.ToString().Replace(",", "."), out tmpValue))
                {
                    return Math.Round(tmpValue, decimalDigits, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return new Nullable<decimal>();
                }
            }
        }

        public static decimal ConvertToDecimalOrZero(object obj, int decimalDigits)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return decimal.Zero;
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = decimalDigits;

                decimal tmpValue = 0;
                if (decimal.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return decimal.Zero;
                }
            }
        }

        public static decimal ConvertToDecimalOrZero(object obj)
        {
            return ConvertToDecimalOrZero(obj, 2);
        }

        public static decimal ConvertToDecimalOrMinValue(object obj, int decimalDigits)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return decimal.MinValue;
            }
            else
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                nfi.NumberDecimalDigits = decimalDigits;

                decimal tmpValue = decimal.MinValue;
                if (decimal.TryParse(obj.ToString().Replace(",", "."), NumberStyles.Any, nfi, out tmpValue))
                {
                    return tmpValue;
                }
                else
                {
                    return decimal.MinValue;
                }
            }
        }

        public static decimal ConvertToDecimalOrMinValue(object obj)
        {
            return ConvertToDecimalOrMinValue(obj, 2);
        }

        public static void CheckAndSetSelectedValue(ListControl listControlToSet, object idToSelect, bool addEmptyItem)
        {
            bool hasSearchItem = false;
            bool hasEmptyItem = false;
            foreach (ListItem item in listControlToSet.Items)
            {
                if (idToSelect != null &&
                    !string.IsNullOrEmpty(idToSelect.ToString()) && item.Value.Equals(idToSelect.ToString()))
                {
                    hasSearchItem = true;
                    break;
                }
                else if (item.Value.Equals(Constants.INVALID_ID_STRING))
                {
                    hasEmptyItem = true;
                }
            }

            if (hasSearchItem)
            {
                listControlToSet.SelectedValue = idToSelect.ToString();
            }
            else
            {
                if (hasEmptyItem)
                {
                    listControlToSet.SelectedValue = Constants.INVALID_ID_STRING;
                }
                else if (addEmptyItem)
                {
                    ListItem emptyItem = new ListItem();
                    emptyItem.Value = Constants.INVALID_ID_STRING;
                    emptyItem.Text = GetCaptionString("UI_Please_Select");

                    listControlToSet.Items.Insert(0, emptyItem);
                    listControlToSet.SelectedValue = Constants.INVALID_ID_STRING;
                }
            }
        }

        public static void CheckAndSetSelectedValue(ListControl listControlToSet, object idToSelect)
        {
            CheckAndSetSelectedValue(listControlToSet, idToSelect, true);
        }

        public static string RoundOutStringToFixedLength(string str, int length, string symbol)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            while (str.Length < length)
            {
                str += symbol;
            }

            return str;
        }

        public static string GetStringWithFixedMaxLength(string str, int length)
        {
            if (str == null)
            {
                return string.Empty;
            }
            if (str.Length <= length)
            {
                return str;
            }
            if (str.Length > length)
            {
                return str.Substring(0, length);
            }

            return str;
        }

        public static string RoundWithExactNumberFloatingPoint(string str, int digitsAfterPoint, string symbol = "0")
        {
            StringBuilder srtBeforeRound = new StringBuilder(str);
            if (str.Contains('.') || str.Contains(','))
            {
                srtBeforeRound = srtBeforeRound.Replace(',', '.');
                int pointIndex = -1;
                for (int i = 0; i < srtBeforeRound.Length; i++)
                {
                    if (srtBeforeRound[i] == '.')
                    {
                        pointIndex = i;
                        break;
                    }
                }

                string digitsAfterFloatingPoint = str.Substring(pointIndex + 1, str.Length - 1 - pointIndex);
                StringBuilder digits = new StringBuilder(digitsAfterFloatingPoint);
                if (digits.Length > digitsAfterPoint)
                {
                    while (digits.Length != digitsAfterPoint)
                    {
                        digits.Length--;
                    }
                }
                else
                {
                    while (digits.Length != digitsAfterPoint)
                    {
                        digits.Append(symbol);
                    }
                }

                StringBuilder result = new StringBuilder();
                result.Append(str.Substring(0, pointIndex));
                if (digitsAfterPoint > 0)
                {
                    result.Append('.');
                }

                result.Append(digits);

                return result.ToString();
            }
            else
            {
                StringBuilder result = new StringBuilder(str);
                result.Append('.');
                for (int i = 0; i < digitsAfterPoint; i++)
                {
                    result.Append(symbol);
                }

                return result.ToString();

            }

        }


        public static Control FindControlById(Control holder, string idControl)
        {
            if (holder.ID == idControl)
            {
                return holder;
            }
            if (holder.HasControls())
            {
                Control temp;
                foreach (Control subcontrol in holder.Controls)
                {
                    temp = FindControlById(subcontrol, idControl);
                    if (temp != null)
                    {
                        return temp;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="hdnRowId"></param>
        /// <param name="checkBoxName"></param>
        /// <param name="onlyEnabled">Gets only selected cells wich are not disabled</param>
        /// <returns></returns>
        public static List<int> GetSelectedGridRowIds(System.Web.UI.WebControls.GridView gridView, string hdnRowId, string checkBoxName, bool? onlyEnabled = null)
        {

            List<int> listIds = new List<int>();


            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxName) as CheckBox;
                    if (cbxGroupedDiscplines.Checked && (onlyEnabled == true ? cbxGroupedDiscplines.Enabled : true))
                    {
                        HiddenField hdnRowMasterKey = FindControlById(row, hdnRowId) as HiddenField;
                        if (hdnRowMasterKey != null)
                        {
                            listIds.Add(Convert.ToInt32(hdnRowMasterKey.Value));
                        }
                    }

                }
            }

            return listIds;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="hdnRowId"></param>
        /// <param name="checkBoxName"></param>
        /// <param name="onlyEnabled">Gets only not selected cells wich are not disabled</param>
        /// <returns></returns>
        public static List<int> GetNotSelectedGridRowIds(System.Web.UI.WebControls.GridView gridView, string hdnRowId, string checkBoxName, bool? onlyEnabled = null)
        {

            List<int> listIds = new List<int>();


            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxName) as CheckBox;
                    if (!cbxGroupedDiscplines.Checked && (onlyEnabled == true ? cbxGroupedDiscplines.Enabled : true))
                    {
                        HiddenField hdnRowMasterKey = FindControlById(row, hdnRowId) as HiddenField;
                        if (hdnRowMasterKey != null)
                        {
                            listIds.Add(Convert.ToInt32(hdnRowMasterKey.Value));
                        }
                    }

                }
            }

            return listIds;

        }


        /// <summary>
        /// Returns list of dictionary key value pairs. The data is retrieved from selected rows in a grid
        /// </summary>
        /// <param name="gridView">the actual grid view object</param>
        /// <param name="checkBoxName">the id of the check box whom we check if it is selected</param>
        /// <param name="listNeededHiddenFieldsIds">list of the ids of the hidden fields containing the data</param>
        /// <param name="onlyEnabled"> not mandatory , if there are disabledled rows we can skip them </param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetSelectedGridRowData(System.Web.UI.WebControls.GridView gridView, string checkBoxName, List<string> listNeededHiddenFieldsIds, bool? onlyEnabled = null)
        {

            List<Dictionary<string, string>> listKeyValues = new List<Dictionary<string, string>>();


            for (int i = 0, dicIndex = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxName) as CheckBox;
                    if (cbxGroupedDiscplines.Checked && (onlyEnabled == true ? cbxGroupedDiscplines.Enabled : true))
                    {
                        listKeyValues.Add(new Dictionary<string, string>());
                        for (int j = 0; j < listNeededHiddenFieldsIds.Count; j++)
                        {

                            HiddenField currentHidden = FindControlById(row, listNeededHiddenFieldsIds[j]) as HiddenField;
                            if (currentHidden != null)
                            {
                                listKeyValues[dicIndex].Add(listNeededHiddenFieldsIds[j], currentHidden.Value);
                            }

                        }

                        dicIndex++;
                    }

                }
            }

            return listKeyValues;

        }

        public static void SelectOrDeselectAllEnabledGridCheckBox(System.Web.UI.WebControls.GridView gridView, bool makeItSelect, string targetCheckBoxName)
        {

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow row = gridView.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, targetCheckBoxName) as CheckBox;
                    if (cbxGroupedDiscplines.Enabled == true)
                    {
                        cbxGroupedDiscplines.Checked = makeItSelect;
                    }


                }
            }
        }

        public static string ConvertNumberToWord(double number)
        {
            return ConvertNumberToWord(number, string.Empty, string.Empty, string.Empty, string.Empty);
        }
        public static string ConvertNumberToWord(double number, string valutaEdText, string valutaText)
        {
            return ConvertNumberToWord(number, valutaEdText, valutaText, string.Empty, string.Empty);
        }
        public static string ConvertNumberToWord(double number, string valutaEdText, string valutaText, string stotEdText, string stotText)
        {
            string valutaed = (string.IsNullOrEmpty(valutaEdText) ? string.Empty : " " + valutaEdText);
            string valutamn = (string.IsNullOrEmpty(valutaText) ? string.Empty : " " + valutaText);
            string stoted = (string.IsNullOrEmpty(stotEdText) ? string.Empty : " " + stotEdText);
            string stotmn = (string.IsNullOrEmpty(stotText) ? string.Empty : " " + stotText);

            int intLeva = (int)number;
            int intStot = (int)(Math.Round(number * 100, 2) - intLeva * 100);

            string withwordstemp = "";		//инициализираме променливите с празен стринг
            string withwordsStot = "";

            //------ Обработваме стотинките --------
            if (intStot > 0)
            {
                string tmpStotWords = "";
                if (intStot < 10)
                {
                    if (intStot == 1)
                    {
                        tmpStotWords = range0ot10stot[intStot] + " " + stotEdText;
                    }
                    else
                    {
                        tmpStotWords = f0to10(intStot);

                        tmpStotWords += " " + stotText;
                    }
                }

                if (intStot >= 10 && intStot < 100)
                    tmpStotWords = f10to100(intStot);

                withwordsStot = " и " + tmpStotWords;
            }

            //-------- Обработка на левовете --------
            withwordstemp = "";								//инициализиране на променливите с празен стринг
            string withwordsfinal = "";

            if (intLeva < 10)									//проверка дали числото е по-малко от 10. Ако е - обработваме го с функцията до 10
                withwordstemp = f0to10(intLeva);					//започваме формирането на крайния стринг - първо вписваме левовете с думи


            if (intLeva >= 10 && intLeva < 100)
                withwordstemp = f10to100(intLeva);


            if (intLeva >= 100 && intLeva < 1000)
                withwordstemp = f100to1000(intLeva);


            if (intLeva >= 1000 && intLeva < 1000000)
                withwordstemp = f1000to1000000(intLeva);

            if (intLeva >= 1000000 && intLeva < 100000000)
                withwordstemp = f1000000to100000000(intLeva);

            //------- Формиране на окончателния стринг за сумата --------------
            //проверка дали левовете да повече от 1 и формиране на правилния стринг за
            //левовете - ед. и мн. число на думата лев
            if (intLeva == 1)
            {
                withwordsfinal = withwordstemp + valutaed + withwordsStot;
            }
            else
            {
                withwordsfinal = withwordstemp + valutamn + withwordsStot;
            }

            return withwordsfinal;
        }
        public static string f0to10(int x)
        {
            return range0ot10[x];
        }
        private static string f10to100(int x)
        {
            string withwords;

            if (x >= 10 && x < 20)
                withwords = range10ot20[x - 10];
            else
                if (x % 10 == 0)
                    withwords = range10ot100[x / 10];
                else
                    withwords = range10ot100[x / 10] + " и " + f0to10(x % 10);
            return withwords;
        }
        private static string f10to100stot(int X)
        {
            string withwords = "";
            if (X >= 10 && X < 20)
                withwords = range10ot20[X - 10];
            else
            {
                if ((X % 10) == 0)
                    withwords = range10ot100[X / 10];
                else
                {
                    if (X % 10 == 1)
                        withwords = range10ot100[X / 10] + " и " + "една";
                    if ((X % 10) == 2)
                        withwords = range10ot100[X / 10] + " и " + "две";
                    if (X % 10 > 2)
                        withwords = range10ot100[X / 10] + " и " + f0to10(X % 10);
                }
            }
            return withwords;
        }
        private static string f100to1000(int X)
        {
            string withwords = "";

            if (X % 100 == 0) //проверка дали трицифреното число е кръгло, напр. 300
                withwords = range100ot1000[X / 100];
            else
            {
                if (X % 10 > 0 && ((X % 100) / 10) == 0)//единици <>0, десетици =0
                    withwords = range100ot1000[X / 100] + " и " + f0to10(X % 10);
                if (X % 10 > 0 && ((X % 100) / 10) > 0)//единици <>0, десетици<>0
                    withwords = range100ot1000[X / 100] + " " + f10to100(X % 100);
                if (X % 10 == 0 && ((X % 100) / 10) > 0) ///единици=0, десетици<>0
                    withwords = range100ot1000[X / 100] + " и " + f10to100(X % 100);

            }
            return withwords;
        }
        private static string f1000to1000000(int X)
        {
            string withwords = "", thousandswords = "", hundredswords = "";
            int thousands, hundreds;

            thousands = X / 1000;
            hundreds = X % 1000;

            if (thousands == 1) thousandswords = "хиляда";
            if (thousands == 2) thousandswords = "две хиляди";

            if (thousands > 2 && thousands < 10)
            {
                thousandswords = f0to10(thousands);
                thousandswords = thousandswords + " хиляди";
            }
            if (thousands >= 10 && thousands < 100)
            {
                thousandswords = f10to100(thousands);
                thousandswords = thousandswords + " хиляди";
            }
            if (thousands >= 100 && thousands < 1000)
            {
                thousandswords = f100to1000(thousands);
                thousandswords = thousandswords + " хиляди";
            }
            if (hundreds != 0)
            {
                if (hundreds < 10)
                {
                    hundredswords = f0to10(hundreds);
                }
                if (hundreds >= 10 && hundreds < 100)
                {
                    hundredswords = (f10to100(hundreds));
                }
                if (hundreds >= 100 && hundreds < 1000)
                {
                    hundredswords = (f100to1000(hundreds));
                }
                if (hundreds % 100 == 0)
                {
                    withwords = thousandswords + " и " + hundredswords;
                }
                else
                    withwords = thousandswords + " " + hundredswords;
            }
            else
                withwords = thousandswords;

            return withwords;
        }
        private static string f1000000to100000000(int X)
        {
            string withwords = "", millionwords = "", thousandswords = "";
            int million, thousands;

            million = X / 1000000;
            thousands = X % 1000000;

            if (million == 1) millionwords = "милион";
            if (million == 2) thousandswords = "два милиона";

            if (million > 2 && million < 10)
            {
                millionwords = f0to10(million);
                millionwords = millionwords + " милиона";
            }
            if (million >= 10 && million < 100)
            {
                millionwords = f10to100(million);
                millionwords = millionwords + " милиона";
            }
            if (million >= 100 && million < 1000)
            {
                millionwords = f100to1000(million);
                millionwords = millionwords + " милиона";
            }
            if (thousands != 0)
            {
                if (thousands < 10)
                {
                    thousandswords = f0to10(thousands);
                }
                if (thousands >= 10 && thousands < 100)
                {
                    thousandswords = (f10to100(thousands));
                }
                if (thousands >= 100 && thousands < 1000)
                {
                    thousandswords = (f100to1000(thousands));
                }
                if (thousands >= 1000 && thousands < 1000000)
                {
                    thousandswords = (f1000to1000000(thousands));
                }
                if (thousands % 100 == 0)
                {
                    withwords = millionwords + " и " + thousandswords;
                }
                else
                    withwords = millionwords + " " + thousandswords;
            }
            else
                withwords = millionwords;

            return withwords;
        }

        public static int ConvertStringNumberToIntNumber(string _Number)
        {
            if (string.IsNullOrEmpty(_Number))
            {
                return 0;
            }

            string result = string.Empty;

            bool isStartNumber = false;
            foreach (char c in _Number)
            {
                if (Char.IsDigit(c) && Convert.ToInt32(c) != 0 && !isStartNumber)
                {
                    isStartNumber = true;

                    result += c;
                }
                else if (Char.IsDigit(c) && isStartNumber)
                {
                    result += c;
                }
            }

            return Convert.ToInt32(result);
        }

        //Копира съдържанието на една директория в друга
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            //Проверка дали директорията в която ще се записва съществува ако не се създава
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            List<string> listDirInfo = new List<string>();
            foreach (FileInfo fi in target.GetFiles())
            {
                listDirInfo.Add(fi.Name);
            }

            //Копиране на всеки файл в неговата нова директория
            {
                foreach (FileInfo fi in source.GetFiles())
                {
                    //FileInfo  newFile =  fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);

                    if (!listDirInfo.Contains(fi.Name))
                    {
                        FileInfo newFile = fi.CopyTo(target.FullName + "\\" + fi.Name, true);
                        File.SetAttributes(newFile.FullName, FileAttributes.Normal);
                    }
                }
            }

            //Копиране на всяка под-директория чрез рекурсия
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Метода копира фаил в директория необьодимо е да се подаде пътя до директорията и пътя до файла
        /// не е необьодимо директорията да се създава предварително 
        /// </summary>
        /// <param name="targetDirPath">път до директорията</param>
        /// <param name="sourceFilePath">път до файла които трябва да бъде копиран</param>
        public static void CopyFileToDirectory(string targetDirPath, string sourceFilePath)
        {
            FileInfo fileToCopy = new FileInfo(sourceFilePath);

            System.IO.DirectoryInfo targetDir =
                    Directory.CreateDirectory(targetDirPath);

            FileInfo copyedFile = fileToCopy.CopyTo(targetDir.FullName + "/" + fileToCopy.Name, true);
            File.SetAttributes(copyedFile.FullName, FileAttributes.Normal);

        }

        //Взима пълния път рекурсивно до всички файлове в подадената му дериктория
        public static ArrayList GenerateFileList(string Dir)
        {
            ArrayList mid = new ArrayList();
            bool empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                mid.Add(file);
                empty = false;
            }

            if (empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0) // if the directory is completly empty, add it
                {
                    mid.Add(Dir + @"\");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // do this recurcivly
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    mid.Add(obj);
                }
            }
            return mid; // return file list
        }

        //Запивва директорията която му е подадена в зип файл в папката където се намира подадената за зип директория
        //и връща пълния път до зипнатия файл
        public static string ZipFolder(DirectoryInfo sourceFolder)
        {
            ArrayList ar = GenerateFileList(sourceFolder.FullName); // generate file list

            int TrimLength = (Directory.GetParent(sourceFolder.FullName)).ToString().Length; // find number of chars to remove
            // from orginal file path
            TrimLength += 1; //remove '\'

            byte[] buffer;
            string zipfilename = sourceFolder.FullName + ".zip";

            ZipOutputStream zipOutStream = new ZipOutputStream(File.Create(zipfilename)); // create zip stream

            ZipEntry zipEntry;

            foreach (string file in ar) // for each file, generate a zipentry
            {
                zipEntry = new ZipEntry(file.Remove(0, TrimLength));
                zipOutStream.PutNextEntry(zipEntry);

                if (!file.EndsWith(@"\")) // if a file ends with '\' it's a directory
                {
                    FileStream fstream = File.OpenRead(file);
                    using (fstream)
                    {
                        buffer = new byte[fstream.Length];
                        fstream.Read(buffer, 0, buffer.Length);
                        zipOutStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            zipOutStream.Flush();
            zipOutStream.Finish();
            zipOutStream.Close();

            return zipfilename;
        }

        //Запивва директорията която му е подадена в зип файл в папката където се намира подадената за зип директория
        //и връща пълния път до зипнатия файл
        public static string ZipFolder(DirectoryInfo sourceFolder, string zipNameSuffix)
        {
            ArrayList ar = GenerateFileList(sourceFolder.FullName); // generate file list

            int TrimLength = (Directory.GetParent(sourceFolder.FullName)).ToString().Length; // find number of chars to remove
            // from orginal file path
            TrimLength += 1; //remove '\'

            byte[] buffer;
            string zipfilename = sourceFolder.FullName + "_" + zipNameSuffix + ".zip";

            ZipOutputStream zipOutStream = new ZipOutputStream(File.Create(zipfilename)); // create zip stream

            ZipEntry zipEntry;

            foreach (string file in ar) // for each file, generate a zipentry
            {
                zipEntry = new ZipEntry(file.Remove(0, TrimLength));
                zipOutStream.PutNextEntry(zipEntry);

                if (!file.EndsWith(@"\")) // if a file ends with '\' it's a directory
                {
                    FileStream fstream = File.OpenRead(file);
                    using (fstream)
                    {
                        buffer = new byte[fstream.Length];
                        fstream.Read(buffer, 0, buffer.Length);
                        zipOutStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }

            zipOutStream.Flush();
            zipOutStream.Finish();
            zipOutStream.Close();

            return zipfilename;
        }

        public static void AddFileToZip(string zipFilename, string fileToAdd)
        {


            using (System.IO.Packaging.Package zip = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                string destFilename = ".\\" + Path.GetFileName(fileToAdd);
                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
                if (zip.PartExists(uri))
                {
                    zip.DeletePart(uri);
                }
                PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
                using (FileStream fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
                {


                    using (Stream dest = part.GetStream())
                    {
                        CopyStream(fileStream, dest);
                    }
                }
            }
        }



        public static string TrimStringToNeededLenht(string text, int length)
        {
            if (text.Length > length)
            {
                return text.Substring(0, length);
            }
            else
            {
                return text;
            }

        }

        private static void CopyStream(System.IO.FileStream inputStream, System.IO.Stream outputStream)
        {
            long bufferSize = inputStream.Length;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            long bytesWritten = 0;

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesWritten += bufferSize;
            }
        }

        public static Control FindFirstParentOfById(Control childControl, string parentId)
        {
            if (childControl == null)
            {
                return null;
            }

            if (childControl.ID == parentId)
            {
                return childControl;
            }

            if (childControl != null)
            {
                Control temp;
                temp = FindFirstParentOfById(childControl.Parent, parentId);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;



        }

        /// <summary>
        /// working properly only for ASP controls , because When using user controls asp set it's own type over the user control type
        /// </summary>
        /// <param name="childControl"></param>
        /// <param name="parentControlType"></param>
        /// <returns></returns>
        public static Control FindFirstParentOfSpecificType(Control childControl, Type parentControlType)
        {
            if (childControl == null)
            {
                return null;
            }

            if ((childControl).GetType() == parentControlType)
            {
                return childControl;
            }

            if (childControl != null)
            {
                Control temp;
                temp = FindFirstParentOfSpecificType(childControl.Parent, parentControlType);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;
        }


        public static Control FindFirstParentOfSpecificTypeForUserControl(Control childControl, Type parentControlType)
        {
            if (childControl == null)
            {
                return null;
            }

            if ((childControl).GetType().BaseType == parentControlType)
            {
                return childControl;
            }

            if (childControl != null)
            {
                Control temp;
                temp = FindFirstParentOfSpecificTypeForUserControl(childControl.Parent, parentControlType);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;
        }



        

        internal static bool CheckTimeInPeriod(int time, int startPeriod, int endPeriod)
        {
            return time >= startPeriod && time <= endPeriod;

        }

        /// <summary>
        /// Връща брой седмици в месеца
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int Weeks(int year, int month)
        {

            int weekCount = 0;

            DateTime endDateOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

            weekCount = GetWeekInMonth(endDateOfMonth);

            return weekCount;


        }
        /// <summary>
        /// Връща поредна седмица в месеца
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekInMonth(DateTime date)
        {
            DateTime tempdate = date.AddDays(-date.Day + 1);

            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNumStart = ciCurr.Calendar.GetWeekOfYear(tempdate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNum - weekNumStart + 1;

        }

        public static string GetDayOfWeek(DateTime date)
        {
            return date.ToString("dddd", new CultureInfo("bg-BG"));

        }

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }


        public static string YearWeekDayToDateTime(int year, int dayOfWeek, int week, int month)
        {

            DateTime startOfYear = new DateTime(year, month, 1);
            DateTime endOfYear = startOfYear.AddMonths(1).AddDays(-1);


            List<UMSCalendar> listCalendar = new List<UMSCalendar>();

            while (startOfYear <= endOfYear)
            {
                UMSCalendar item = new UMSCalendar();

                item.Year = year;
                item.Month = month;
                item.Week = GetWeekInMonth(startOfYear);
                item.Date = startOfYear;
                item.DayOfWeek = startOfYear.DayOfWeek;
                item.Day = startOfYear.Day;

                startOfYear = startOfYear.AddDays(1);

                listCalendar.Add(item);
            }

            UMSCalendar calendarItem = listCalendar.FirstOrDefault(d => d.Week == week + 1 && (int)d.DayOfWeek == dayOfWeek);

            // The +7 and %7 stuff is to avoid negative numbers etc.
            //int daysToFirstCorrectDay = ((dayOfWeek - (int)startOfYear.DayOfWeek) + 7) % 7;

            //string theDay = startOfYear.AddDays(7 * (week - 1) + daysToFirstCorrectDay).ToString("dd.MM.yyyy");
            string theDay = (calendarItem != null) ? calendarItem.Date.ToString("dd.MM.yyyy") : "";

            return theDay;
        }

        public static IQueryable<T> DoDynamicWhere<T>(IQueryable<T> list, Dictionary<string, object> criteria)
        {
            var temp = list;

            //create a predicate for each supplied criterium and filter on it.
            foreach (var key in criteria.Keys)
            {
                temp = temp.Where(BuildPredicate<T>(key, criteria[key]));
            }

            return temp;
        }


        public static string RemoveNewLines(string inputString)
        {
            if (inputString != null)
            {
                Regex regexNewline = new Regex("(\r\n|\r|\n)");
                string result = regexNewline.Replace(inputString, "");
                return result;
            }
            else
            {
                return string.Empty;
            }
        }




        //Create i.<prop> == <value> dynamically
        public static Expression<Func<T, bool>> BuildPredicate<T>(string property, object value)
        {
            var itemParameter = Expression.Parameter(typeof(T), "i");

            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.MakeMemberAccess(
                        itemParameter,
                        typeof(T).GetProperty(property)),
                    Expression.Constant(value)
                ),
                itemParameter);

            return expression;
        }

        public static string GetMarkInWords(decimal mark)
        {
            string result = string.Empty;

            if (mark >= new decimal(5.50) && mark <= new decimal(6.00))
            {
                result = GetCaptionString("Mark_InWords_Еxcellent");
            }
            else if (mark >= new decimal(4.50) && mark < new decimal(5.50))
            {
                result = GetCaptionString("Mark_InWords_VeryGood");
            }
            else if (mark >= new decimal(3.50) && mark < new decimal(4.50))
            {
                result = GetCaptionString("Mark_InWords_Good");
            }
            else if (mark >= new decimal(3) && mark < new decimal(3.50))
            {
                result = GetCaptionString("Mark_InWords_Moderate");
            }
            else if (mark >= new decimal(2.00) && mark < new decimal(2.99))
            {
                result = GetCaptionString("Mark_InWords_Poor");
            }

            return result;
        }

        public static string GetMarkInWordsInEnglish(decimal mark)
        {
            string result = string.Empty;

            if (mark >= new decimal(5.50) && mark <= new decimal(6.00))
            {
                result = "Еxcellent";
            }
            else if (mark >= new decimal(4.50) && mark < new decimal(5.50))
            {
                result = "Very Good";
            }
            else if (mark >= new decimal(3.50) && mark < new decimal(4.50))
            {
                result = "Good";
            }
            else if (mark >= new decimal(3) && mark < new decimal(3.50))
            {
                result = "Moderate";
            }
            else if (mark >= new decimal(2.00) && mark < new decimal(2.99))
            {
                result = "Poor";
            }

            return result;
        }

        public static string GetECTSMark(decimal mark)
        {
            string result = string.Empty;

            if (mark >= new decimal(5.50) && mark <= new decimal(6.00))
            {
                result = "A";
            }
            else if (mark >= new decimal(4.50) && mark < new decimal(5.50))
            {
                result = "B";
            }
            else if (mark >= new decimal(3.50) && mark < new decimal(4.50))
            {
                result = "C";
            }
            else if (mark >= new decimal(3.25) && mark < new decimal(3.49))
            {
                result = "D";
            }
            else if (mark >= new decimal(3) && mark < new decimal(3.24))
            {
                result = "E";
            }
            else if (mark >= new decimal(2.25) && mark < new decimal(2.99))
            {
                result = "Fx";
            }
            else if (mark >= new decimal(2.00) && mark < new decimal(2.24))
            {
                result = "F";
            }

            return result;
        }



        public static void WriteLineInOutput(string outputStr)
        {
            System.Diagnostics.Debug.WriteLine(outputStr);
        }

        internal static string RemoveTrailingZeros(string number)
        {
            string fixedNumber = number;
            if (number.Contains(',') || number.Contains('.'))
            {
                fixedNumber = fixedNumber.TrimEnd('0');
                fixedNumber = fixedNumber.TrimEnd('.');
                fixedNumber = fixedNumber.TrimEnd(',');
            }
            return fixedNumber;
        }

        /// <summary>
        /// get gender by the egn of person, it assumes that egn is correct
        /// </summary>
        /// <param name="egn"></param>
        /// <returns></returns>
        public static ETEMEnums.SexEnum GetGenderByEgn(string egn)
        {
            try
            {
                int neededDigit = int.Parse(egn[8].ToString());
                if (neededDigit % 2 == 0)
                {
                    return ETEMEnums.SexEnum.Man;
                }
                else
                {
                    return ETEMEnums.SexEnum.Woman;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The egn is not in a correct format", ex);
            }
        }


        /// <summary>
        /// Get gender by egn boolean
        /// </summary>
        /// <param name="egn"></param>
        /// <returns></returns>
        public static bool GetGenderByEgnIsMan(string egn)
        {
            try
            {
                int neededDigit = int.Parse(egn[8].ToString());
                if (neededDigit % 2 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogToMail("The egn is not in a correct format " + egn);
                throw new ArgumentException("The egn is not in a correct format", ex);
                
            }
        }

        public static bool IsEgnValid(string egn)
        {
            bool isValid = true;
            if (egn.Length != Constants.EGN_LEN)
            {
                isValid = false;
            }

            for (int i = 0; i < egn.Length; i++)
            {
                bool isDigit = char.IsDigit(egn[i]);

                if (!isDigit)
                {
                    isValid = false;
                    break;
                }

            }

            return isValid;
        }

        public static string GetNumberOfCharAsString(char _Char, int _Number)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _Number; i++)
            {
                sb.Append(_Char);
            }

            return sb.ToString();
        }

        public static string CreateNextSequaceNumberWithLeadingZerosFixLen(int numberLength, int nextValue)
        {
            string result = nextValue.ToString().PadLeft(numberLength, '0');
            return result;
        }

        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var candidateDate = date;
            while (candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                candidateDate = candidateDate.AddDays(-1);
            }

            return TextAsDateParseExact(candidateDate.ToString("dd.MM.yyyy")).Value;
        }

        public static Control FindControlRecursive(Control control, string id)
        {
            if (control == null) return null;
            //try to find the control at the current level
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                //search the children
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive(child, id);

                    if (ctrl != null) break;
                }
            }
            return ctrl;
        }

        public static IList<T> GetAllControlsInWebControlRecusrvive<T>(WebControl control) where T : WebControl
        {
            var rtn = new List<T>();
            foreach (WebControl item in control.Controls)
            {
                var ctr = item as T;
                if (ctr != null)
                {
                    rtn.Add(ctr);
                }
                else
                {
                    rtn.AddRange(GetAllControlsInWebControlRecusrvive<T>(item));
                }

            }
            return rtn;
        }

        public static IList<T> GetAllControlsRecusrvive<T>(Control control) where T : Control
        {
            var rtn = new List<T>();
            foreach (Control item in control.Controls)
            {
                var ctr = item as T;
                if (ctr != null)
                {
                    rtn.Add(ctr);
                }
                else
                {
                    rtn.AddRange(GetAllControlsRecusrvive<T>(item));
                }

            }
            return rtn;
        }

        public static void SetAllControlsEnabledOrDisabledWithoutExceptions<T>(Control control,
                                                                               List<string> listExceptionsNames,
                                                                               bool isEnabled) where T : Control
        {
            IList<T> controls = GetAllControlsRecusrvive<T>(control);

            foreach (Control item in controls)
            {
                if ((item is WebControl) && (listExceptionsNames == null ||
                    (listExceptionsNames != null && !listExceptionsNames.Contains(item.ID))))
                {
                    (item as WebControl).Enabled = isEnabled;
                }
            }
        }


        /// <summary>
        /// this methdos give results like "два семестъра , две години, една година, един семестър..." , depending on the iput parameters
        /// </summary>
        /// <param name="genderForm">the form of the output we need</param>
        /// <param name="number"> the number from 0-10</param>
        /// <param name="additonSingle">text in single form</param>
        /// <param name="additionMultiple">text in Multiple form</param>
        /// <returns></returns>
        public static string GetWordProperForm(ETEMEnums.WordGenderForm genderForm, int number, string additonSingle = null, string additionMultiple = null)
        {
            Dictionary<WordData, string> listData = new Dictionary<WordData, string>()
            {
                {new WordData( ETEMEnums.WordGenderForm.FemaleForm,0 ),"нула" + " " +additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.FemaleForm,1 ),"една" + " " +additonSingle},
                 {new WordData( ETEMEnums.WordGenderForm.MaleForm,  1 ),"един"+ " " +additonSingle},
                 {new WordData( ETEMEnums.WordGenderForm.ChildForm, 1 ),"едно"+ " " +additonSingle},
                 {new WordData( ETEMEnums.WordGenderForm.MaleForm,2 ),"два"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.FemaleForm,2 ),"два"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,2 ),"две"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,3 ),"три"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,4 ),"четири"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,5 ),"пет"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,6 ),"шест"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,7 ),"седем"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,8 ),"осем"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,9 ),"девет"+ " " + additionMultiple},
                 {new WordData( ETEMEnums.WordGenderForm.DontMatter,10 ),"десет"+ " " + additionMultiple},
            };

            WordData data = new WordData(genderForm, number);
            return listData[data];
        }

        public static string GetCurrentDateConcatenated()
        {
            var dateString = DateTime.Now.ToString(Constants.DATE_PATTERN_FOR_FILE_ADMINUNI_EXPORT_SUFFIX);
            return dateString;
        }

        public static string RemoveIlligalCharFromFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), " "));
        }

        #region Mappings between extensions and mime types
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region Big freaking list of mime types
            // combination of values from Windows 7 Registry and 
            // from C:\Windows\System32\inetsrv\config\applicationHost.config
            // some added, including .7z and .dat
            {".323", "text/h323"},
            {".3g2", "video/3gpp2"},
            {".3gp", "video/3gpp"},
            {".3gp2", "video/3gpp2"},
            {".3gpp", "video/3gpp"},
            {".7z", "application/x-7z-compressed"},
            {".aa", "audio/audible"},
            {".AAC", "audio/aac"},
            {".aaf", "application/octet-stream"},
            {".aax", "audio/vnd.audible.aax"},
            {".ac3", "audio/ac3"},
            {".aca", "application/octet-stream"},
            {".accda", "application/msaccess.addin"},
            {".accdb", "application/msaccess"},
            {".accdc", "application/msaccess.cab"},
            {".accde", "application/msaccess"},
            {".accdr", "application/msaccess.runtime"},
            {".accdt", "application/msaccess"},
            {".accdw", "application/msaccess.webapplication"},
            {".accft", "application/msaccess.ftemplate"},
            {".acx", "application/internet-property-stream"},
            {".AddIn", "text/xml"},
            {".ade", "application/msaccess"},
            {".adobebridge", "application/x-bridge-url"},
            {".adp", "application/msaccess"},
            {".ADT", "audio/vnd.dlna.adts"},
            {".ADTS", "audio/aac"},
            {".afm", "application/octet-stream"},
            {".ai", "application/postscript"},
            {".aif", "audio/x-aiff"},
            {".aifc", "audio/aiff"},
            {".aiff", "audio/aiff"},
            {".air", "application/vnd.adobe.air-application-installer-package+zip"},
            {".amc", "application/x-mpeg"},
            {".application", "application/x-ms-application"},
            {".art", "image/x-jg"},
            {".asa", "application/xml"},
            {".asax", "application/xml"},
            {".ascx", "application/xml"},
            {".asd", "application/octet-stream"},
            {".asf", "video/x-ms-asf"},
            {".ashx", "application/xml"},
            {".asi", "application/octet-stream"},
            {".asm", "text/plain"},
            {".asmx", "application/xml"},
            {".aspx", "application/xml"},
            {".asr", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".atom", "application/atom+xml"},
            {".au", "audio/basic"},
            {".avi", "video/x-msvideo"},
            {".axs", "application/olescript"},
            {".bas", "text/plain"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".c", "text/plain"},
            {".cab", "application/octet-stream"},
            {".caf", "audio/x-caf"},
            {".calx", "application/vnd.ms-office.calx"},
            {".cat", "application/vnd.ms-pki.seccat"},
            {".cc", "text/plain"},
            {".cd", "text/plain"},
            {".cdda", "audio/aiff"},
            {".cdf", "application/x-cdf"},
            {".cer", "application/x-x509-ca-cert"},
            {".chm", "application/octet-stream"},
            {".class", "application/x-java-applet"},
            {".clp", "application/x-msclip"},
            {".cmx", "image/x-cmx"},
            {".cnf", "text/plain"},
            {".cod", "image/cis-cod"},
            {".config", "application/xml"},
            {".contact", "text/x-ms-contact"},
            {".coverage", "application/xml"},
            {".cpio", "application/x-cpio"},
            {".cpp", "text/plain"},
            {".crd", "application/x-mscardfile"},
            {".crl", "application/pkix-crl"},
            {".crt", "application/x-x509-ca-cert"},
            {".cs", "text/plain"},
            {".csdproj", "text/plain"},
            {".csh", "application/x-csh"},
            {".csproj", "text/plain"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".cur", "application/octet-stream"},
            {".cxx", "text/plain"},
            {".dat", "application/octet-stream"},
            {".datasource", "application/xml"},
            {".dbproj", "text/plain"},
            {".dcr", "application/x-director"},
            {".def", "text/plain"},
            {".deploy", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dgml", "application/xml"},
            {".dib", "image/bmp"},
            {".dif", "video/x-dv"},
            {".dir", "application/x-director"},
            {".disco", "text/xml"},
            {".dll", "application/x-msdownload"},
            {".dll.config", "text/xml"},
            {".dlm", "text/dlm"},
            {".doc", "application/msword"},
            {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".dot", "application/msword"},
            {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {".dsp", "application/octet-stream"},
            {".dsw", "text/plain"},
            {".dtd", "text/xml"},
            {".dtsConfig", "text/xml"},
            {".dv", "video/x-dv"},
            {".dvi", "application/x-dvi"},
            {".dwf", "drawing/x-dwf"},
            {".dwp", "application/octet-stream"},
            {".dxr", "application/x-director"},
            {".eml", "message/rfc822"},
            {".emz", "application/octet-stream"},
            {".eot", "application/octet-stream"},
            {".eps", "application/postscript"},
            {".etl", "application/etl"},
            {".etx", "text/x-setext"},
            {".evy", "application/envoy"},
            {".exe", "application/octet-stream"},
            {".exe.config", "text/xml"},
            {".fdf", "application/vnd.fdf"},
            {".fif", "application/fractals"},
            {".filters", "Application/xml"},
            {".fla", "application/octet-stream"},
            {".flr", "x-world/x-vrml"},
            {".flv", "video/x-flv"},
            {".fsscript", "application/fsharp-script"},
            {".fsx", "application/fsharp-script"},
            {".generictest", "application/xml"},
            {".gif", "image/gif"},
            {".group", "text/x-ms-group"},
            {".gsm", "audio/x-gsm"},
            {".gtar", "application/x-gtar"},
            {".gz", "application/x-gzip"},
            {".h", "text/plain"},
            {".hdf", "application/x-hdf"},
            {".hdml", "text/x-hdml"},
            {".hhc", "application/x-oleobject"},
            {".hhk", "application/octet-stream"},
            {".hhp", "application/octet-stream"},
            {".hlp", "application/winhlp"},
            {".hpp", "text/plain"},
            {".hqx", "application/mac-binhex40"},
            {".hta", "application/hta"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".htt", "text/webviewhtml"},
            {".hxa", "application/xml"},
            {".hxc", "application/xml"},
            {".hxd", "application/octet-stream"},
            {".hxe", "application/xml"},
            {".hxf", "application/xml"},
            {".hxh", "application/octet-stream"},
            {".hxi", "application/octet-stream"},
            {".hxk", "application/xml"},
            {".hxq", "application/octet-stream"},
            {".hxr", "application/octet-stream"},
            {".hxs", "application/octet-stream"},
            {".hxt", "text/html"},
            {".hxv", "application/xml"},
            {".hxw", "application/octet-stream"},
            {".hxx", "text/plain"},
            {".i", "text/plain"},
            {".ico", "image/x-icon"},
            {".ics", "application/octet-stream"},
            {".idl", "text/plain"},
            {".ief", "image/ief"},
            {".iii", "application/x-iphone"},
            {".inc", "text/plain"},
            {".inf", "application/octet-stream"},
            {".inl", "text/plain"},
            {".ins", "application/x-internet-signup"},
            {".ipa", "application/x-itunes-ipa"},
            {".ipg", "application/x-itunes-ipg"},
            {".ipproj", "text/plain"},
            {".ipsw", "application/x-itunes-ipsw"},
            {".iqy", "text/x-ms-iqy"},
            {".isp", "application/x-internet-signup"},
            {".ite", "application/x-itunes-ite"},
            {".itlp", "application/x-itunes-itlp"},
            {".itms", "application/x-itunes-itms"},
            {".itpc", "application/x-itunes-itpc"},
            {".IVF", "video/x-ivf"},
            {".jar", "application/java-archive"},
            {".java", "application/octet-stream"},
            {".jck", "application/liquidmotion"},
            {".jcz", "application/liquidmotion"},
            {".jfif", "image/pjpeg"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".jpb", "application/octet-stream"},
            {".jpe", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"},
            {".json", "application/json"},
            {".jsx", "text/jscript"},
            {".jsxbin", "text/plain"},
            {".latex", "application/x-latex"},
            {".library-ms", "application/windows-library+xml"},
            {".lit", "application/x-ms-reader"},
            {".loadtest", "application/xml"},
            {".lpk", "application/octet-stream"},
            {".lsf", "video/x-la-asf"},
            {".lst", "text/plain"},
            {".lsx", "video/x-la-asf"},
            {".lzh", "application/octet-stream"},
            {".m13", "application/x-msmediaview"},
            {".m14", "application/x-msmediaview"},
            {".m1v", "video/mpeg"},
            {".m2t", "video/vnd.dlna.mpeg-tts"},
            {".m2ts", "video/vnd.dlna.mpeg-tts"},
            {".m2v", "video/mpeg"},
            {".m3u", "audio/x-mpegurl"},
            {".m3u8", "audio/x-mpegurl"},
            {".m4a", "audio/m4a"},
            {".m4b", "audio/m4b"},
            {".m4p", "audio/m4p"},
            {".m4r", "audio/x-m4r"},
            {".m4v", "video/x-m4v"},
            {".mac", "image/x-macpaint"},
            {".mak", "text/plain"},
            {".man", "application/x-troff-man"},
            {".manifest", "application/x-ms-manifest"},
            {".map", "text/plain"},
            {".master", "application/xml"},
            {".mda", "application/msaccess"},
            {".mdb", "application/x-msaccess"},
            {".mde", "application/msaccess"},
            {".mdp", "application/octet-stream"},
            {".me", "application/x-troff-me"},
            {".mfp", "application/x-shockwave-flash"},
            {".mht", "message/rfc822"},
            {".mhtml", "message/rfc822"},
            {".mid", "audio/mid"},
            {".midi", "audio/mid"},
            {".mix", "application/octet-stream"},
            {".mk", "text/plain"},
            {".mmf", "application/x-smaf"},
            {".mno", "text/xml"},
            {".mny", "application/x-msmoney"},
            {".mod", "video/mpeg"},
            {".mov", "video/quicktime"},
            {".movie", "video/x-sgi-movie"},
            {".mp2", "video/mpeg"},
            {".mp2v", "video/mpeg"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".mp4v", "video/mp4"},
            {".mpa", "video/mpeg"},
            {".mpe", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpf", "application/vnd.ms-mediapackage"},
            {".mpg", "video/mpeg"},
            {".mpp", "application/vnd.ms-project"},
            {".mpv2", "video/mpeg"},
            {".mqv", "video/quicktime"},
            {".ms", "application/x-troff-ms"},
            {".msi", "application/octet-stream"},
            {".mso", "application/octet-stream"},
            {".mts", "video/vnd.dlna.mpeg-tts"},
            {".mtx", "application/xml"},
            {".mvb", "application/x-msmediaview"},
            {".mvc", "application/x-miva-compiled"},
            {".mxp", "application/x-mmxp"},
            {".nc", "application/x-netcdf"},
            {".nsc", "video/x-ms-asf"},
            {".nws", "message/rfc822"},
            {".ocx", "application/octet-stream"},
            {".oda", "application/oda"},
            {".odc", "text/x-ms-odc"},
            {".odh", "text/plain"},
            {".odl", "text/plain"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/oleobject"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".one", "application/onenote"},
            {".onea", "application/onenote"},
            {".onepkg", "application/onenote"},
            {".onetmp", "application/onenote"},
            {".onetoc", "application/onenote"},
            {".onetoc2", "application/onenote"},
            {".orderedtest", "application/xml"},
            {".osdx", "application/opensearchdescription+xml"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p7c", "application/pkcs7-mime"},
            {".p7m", "application/pkcs7-mime"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7s", "application/pkcs7-signature"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcast", "application/x-podcast"},
            {".pct", "image/pict"},
            {".pcx", "application/octet-stream"},
            {".pcz", "application/octet-stream"},
            {".pdf", "application/pdf"},
            {".pfb", "application/octet-stream"},
            {".pfm", "application/octet-stream"},
            {".pfx", "application/x-pkcs12"},
            {".pgm", "image/x-portable-graymap"},
            {".pic", "image/pict"},
            {".pict", "image/pict"},
            {".pkgdef", "text/plain"},
            {".pkgundef", "text/plain"},
            {".pko", "application/vnd.ms-pki.pko"},
            {".pls", "audio/scpls"},
            {".pma", "application/x-perfmon"},
            {".pmc", "application/x-perfmon"},
            {".pml", "application/x-perfmon"},
            {".pmr", "application/x-perfmon"},
            {".pmw", "application/x-perfmon"},
            {".png", "image/png"},
            {".pnm", "image/x-portable-anymap"},
            {".pnt", "image/x-macpaint"},
            {".pntg", "image/x-macpaint"},
            {".pnz", "image/png"},
            {".pot", "application/vnd.ms-powerpoint"},
            {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {".ppa", "application/vnd.ms-powerpoint"},
            {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {".ppm", "image/x-portable-pixmap"},
            {".pps", "application/vnd.ms-powerpoint"},
            {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {".prf", "application/pics-rules"},
            {".prm", "application/octet-stream"},
            {".prx", "application/octet-stream"},
            {".ps", "application/postscript"},
            {".psc1", "application/PowerShell"},
            {".psd", "application/octet-stream"},
            {".psess", "application/xml"},
            {".psm", "application/octet-stream"},
            {".psp", "application/octet-stream"},
            {".pub", "application/x-mspublisher"},
            {".pwz", "application/vnd.ms-powerpoint"},
            {".qht", "text/x-html-insertion"},
            {".qhtm", "text/x-html-insertion"},
            {".qt", "video/quicktime"},
            {".qti", "image/x-quicktime"},
            {".qtif", "image/x-quicktime"},
            {".qtl", "application/x-quicktimeplayer"},
            {".qxd", "application/octet-stream"},
            {".ra", "audio/x-pn-realaudio"},
            {".ram", "audio/x-pn-realaudio"},
            {".rar", "application/octet-stream"},
            {".ras", "image/x-cmu-raster"},
            {".rat", "application/rat-file"},
            {".rc", "text/plain"},
            {".rc2", "text/plain"},
            {".rct", "text/plain"},
            {".rdlc", "application/xml"},
            {".resx", "application/xml"},
            {".rf", "image/vnd.rn-realflash"},
            {".rgb", "image/x-rgb"},
            {".rgs", "text/plain"},
            {".rm", "application/vnd.rn-realmedia"},
            {".rmi", "audio/mid"},
            {".rmp", "application/vnd.rn-rn_music_package"},
            {".roff", "application/x-troff"},
            {".rpm", "audio/x-pn-realaudio-plugin"},
            {".rqy", "text/x-ms-rqy"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".ruleset", "application/xml"},
            {".s", "text/plain"},
            {".safariextz", "application/x-safari-safariextz"},
            {".scd", "application/x-msschedule"},
            {".sct", "text/scriptlet"},
            {".sd2", "audio/x-sd2"},
            {".sdp", "application/sdp"},
            {".sea", "application/octet-stream"},
            {".searchConnector-ms", "application/windows-search-connector+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".settings", "application/xml"},
            {".sgimb", "application/x-sgimb"},
            {".sgml", "text/sgml"},
            {".sh", "application/x-sh"},
            {".shar", "application/x-shar"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".sitemap", "application/xml"},
            {".skin", "application/xml"},
            {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {".slk", "application/vnd.ms-excel"},
            {".sln", "text/plain"},
            {".slupkg-ms", "application/x-ms-license"},
            {".smd", "audio/x-smd"},
            {".smi", "application/octet-stream"},
            {".smx", "audio/x-smd"},
            {".smz", "audio/x-smd"},
            {".snd", "audio/basic"},
            {".snippet", "application/xml"},
            {".snp", "application/octet-stream"},
            {".sol", "text/plain"},
            {".sor", "text/plain"},
            {".spc", "application/x-pkcs7-certificates"},
            {".spl", "application/futuresplash"},
            {".src", "application/x-wais-source"},
            {".srf", "text/plain"},
            {".SSISDeploymentManifest", "text/xml"},
            {".ssm", "application/streamingmedia"},
            {".sst", "application/vnd.ms-pki.certstore"},
            {".stl", "application/vnd.ms-pki.stl"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".svc", "application/xml"},
            {".swf", "application/x-shockwave-flash"},
            {".t", "application/x-troff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".testrunconfig", "application/xml"},
            {".testsettings", "application/xml"},
            {".tex", "application/x-tex"},
            {".texi", "application/x-texinfo"},
            {".texinfo", "application/x-texinfo"},
            {".tgz", "application/x-compressed"},
            {".thmx", "application/vnd.ms-officetheme"},
            {".thn", "application/octet-stream"},
            {".tif", "image/tiff"},
            {".tiff", "image/tiff"},
            {".tlh", "text/plain"},
            {".tli", "text/plain"},
            {".toc", "application/octet-stream"},
            {".tr", "application/x-troff"},
            {".trm", "application/x-msterminal"},
            {".trx", "application/xml"},
            {".ts", "video/vnd.dlna.mpeg-tts"},
            {".tsv", "text/tab-separated-values"},
            {".ttf", "application/octet-stream"},
            {".tts", "video/vnd.dlna.mpeg-tts"},
            {".txt", "text/plain"},
            {".u32", "application/octet-stream"},
            {".uls", "text/iuls"},
            {".user", "text/plain"},
            {".ustar", "application/x-ustar"},
            {".vb", "text/plain"},
            {".vbdproj", "text/plain"},
            {".vbk", "video/mpeg"},
            {".vbproj", "text/plain"},
            {".vbs", "text/vbscript"},
            {".vcf", "text/x-vcard"},
            {".vcproj", "Application/xml"},
            {".vcs", "text/plain"},
            {".vcxproj", "Application/xml"},
            {".vddproj", "text/plain"},
            {".vdp", "text/plain"},
            {".vdproj", "text/plain"},
            {".vdx", "application/vnd.ms-visio.viewer"},
            {".vml", "text/xml"},
            {".vscontent", "application/xml"},
            {".vsct", "text/xml"},
            {".vsd", "application/vnd.visio"},
            {".vsi", "application/ms-vsi"},
            {".vsix", "application/vsix"},
            {".vsixlangpack", "text/xml"},
            {".vsixmanifest", "text/xml"},
            {".vsmdi", "application/xml"},
            {".vspscc", "text/plain"},
            {".vss", "application/vnd.visio"},
            {".vsscc", "text/plain"},
            {".vssettings", "text/xml"},
            {".vssscc", "text/plain"},
            {".vst", "application/vnd.visio"},
            {".vstemplate", "text/xml"},
            {".vsto", "application/x-ms-vsto"},
            {".vsw", "application/vnd.visio"},
            {".vsx", "application/vnd.visio"},
            {".vtx", "application/vnd.visio"},
            {".wav", "audio/wav"},
            {".wave", "audio/wav"},
            {".wax", "audio/x-ms-wax"},
            {".wbk", "application/msword"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wcm", "application/vnd.ms-works"},
            {".wdb", "application/vnd.ms-works"},
            {".wdp", "image/vnd.ms-photo"},
            {".webarchive", "application/x-safari-webarchive"},
            {".webtest", "application/xml"},
            {".wiq", "application/xml"},
            {".wiz", "application/msword"},
            {".wks", "application/vnd.ms-works"},
            {".WLMP", "application/wlmoviemaker"},
            {".wlpginstall", "application/x-wlpg-detect"},
            {".wlpginstall3", "application/x-wlpg3-detect"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wmd", "application/x-ms-wmd"},
            {".wmf", "application/x-msmetafile"},
            {".wml", "text/vnd.wap.wml"},
            {".wmlc", "application/vnd.wap.wmlc"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wmlsc", "application/vnd.wap.wmlscriptc"},
            {".wmp", "video/x-ms-wmp"},
            {".wmv", "video/x-ms-wmv"},
            {".wmx", "video/x-ms-wmx"},
            {".wmz", "application/x-ms-wmz"},
            {".wpl", "application/vnd.ms-wpl"},
            {".wps", "application/vnd.ms-works"},
            {".wri", "application/x-mswrite"},
            {".wrl", "x-world/x-vrml"},
            {".wrz", "x-world/x-vrml"},
            {".wsc", "text/scriptlet"},
            {".wsdl", "text/xml"},
            {".wvx", "video/x-ms-wvx"},
            {".x", "application/directx"},
            {".xaf", "x-world/x-vrml"},
            {".xaml", "application/xaml+xml"},
            {".xap", "application/x-silverlight-app"},
            {".xbap", "application/x-ms-xbap"},
            {".xbm", "image/x-xbitmap"},
            {".xdr", "text/plain"},
            {".xht", "application/xhtml+xml"},
            {".xhtml", "application/xhtml+xml"},
            {".xla", "application/vnd.ms-excel"},
            {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {".xlc", "application/vnd.ms-excel"},
            {".xld", "application/vnd.ms-excel"},
            {".xlk", "application/vnd.ms-excel"},
            {".xll", "application/vnd.ms-excel"},
            {".xlm", "application/vnd.ms-excel"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".xlt", "application/vnd.ms-excel"},
            {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {".xlw", "application/vnd.ms-excel"},
            {".xml", "text/xml"},
            {".xmta", "application/xml"},
            {".xof", "x-world/x-vrml"},
            {".XOML", "text/plain"},
            {".xpm", "image/x-xpixmap"},
            {".xps", "application/vnd.ms-xpsdocument"},
            {".xrm-ms", "text/xml"},
            {".xsc", "application/xml"},
            {".xsd", "text/xml"},
            {".xsf", "text/xml"},
            {".xsl", "text/xml"},
            {".xslt", "text/xml"},
            {".xsn", "application/octet-stream"},
            {".xss", "application/xml"},
            {".xtp", "application/octet-stream"},
            {".xwd", "image/x-xwindowdump"},
            {".z", "application/x-compress"},
            {".zip", "application/x-zip-compressed"},
            #endregion
        };
        #endregion

        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
    }

    public class WordData
    {
        public ETEMEnums.WordGenderForm GenderWordFrom { get; set; }
        public ETEMEnums.WordNumber WordNumber { get; set; }

        public WordData(ETEMEnums.WordGenderForm genderForm, int number)
        {
            if (number <= 2)
            {
                this.GenderWordFrom = genderForm;
            }
            else
            {
                //if the number is bigger than one the gender word form don't matter
                this.GenderWordFrom = ETEMEnums.WordGenderForm.DontMatter;
            }

            this.Number = number;
        }
        public int Number { get; set; }

        public override bool Equals(object other)
        {
            var comparingObject = other as WordData;
            if (comparingObject == null)
                return false;

            //if (Number>1)
            //{
            //    this.GenderWordFrom = UMSEnums.WordGenderForm.DontMatter;
            //    return Number == comparingObject.Number;

            //}
            //else
            //{
            return (int)GenderWordFrom == (int)comparingObject.GenderWordFrom && Number == comparingObject.Number;
            //}

        }

        public override int GetHashCode()
        {

            return 17 * GenderWordFrom.GetHashCode() + Number.GetHashCode();
        }
    }

    public class UMSCalendar
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int Day { get; set; }
        public DateTime Date { get; set; }
    }
}

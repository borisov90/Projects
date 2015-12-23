using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ETEMModel.Helpers
{
    public partial class BaseHelper
    {
        //it is needed for the diplomas of the phds
        public static string GetDateOfInWords(DateTime date)
        {
            StringBuilder dateInWords = new StringBuilder();
            int day = date.Day;
            string dayInWords = "";
            switch (day)
            {
                case 1: dayInWords = "Първи"; break;
                case 2: dayInWords = "Втори"; break;
                case 3: dayInWords = "Трети"; break;
                case 4: dayInWords = "Четвърти"; break;
                case 5: dayInWords = "Пети"; break;
                case 6: dayInWords = "Шести"; break;
                case 7: dayInWords = "Седми"; break;
                case 8: dayInWords = "Осми"; break;
                case 9: dayInWords = "Девети"; break;
                case 10: dayInWords = "Десети"; break;
                case 11: dayInWords = "Единадесети"; break;
                case 12: dayInWords = "Дванадесети"; break;
                case 13: dayInWords = "Тринадесети"; break;
                case 14: dayInWords = "Четиринадесети"; break;
                case 15: dayInWords = "Петнадесети"; break;
                case 16: dayInWords = "Шестнадесети"; break;
                case 17: dayInWords = "Седемнадесети"; break;
                case 18: dayInWords = "Осемнадесети"; break;
                case 19: dayInWords = "Девенадесети"; break;
                case 20: dayInWords = "Двадесети"; break;
                case 21: dayInWords = "Двадесет и първи"; break;
                case 22: dayInWords = "Двадесет и втори"; break;
                case 23: dayInWords = "Двадесет и трети"; break;
                case 24: dayInWords = "Двадесет и четвърти"; break;
                case 25: dayInWords = "Двадесет и пети"; break;
                case 26: dayInWords = "Двадесет и шести"; break;
                case 27: dayInWords = "Двадесет и седми"; break;
                case 28: dayInWords = "Двадесет и осми"; break;
                case 29: dayInWords = "Двадесет и девети"; break;
                case 30: dayInWords = "Тридесети"; break;
                case 31: dayInWords = "Тридесет и първи"; break;

                default:
                    break;
            }


            int month = date.Month;
            string monthInWords = "";
            switch (month)
            {
                case 1: monthInWords = "януари"; break;
                case 2: monthInWords = "февруари"; break;
                case 3: monthInWords = "март"; break;
                case 4: monthInWords = "април"; break;
                case 5: monthInWords = "май"; break;
                case 6: monthInWords = "юни"; break;
                case 7: monthInWords = "юли"; break;
                case 8: monthInWords = "август"; break;
                case 9: monthInWords = "септември"; break;
                case 10: monthInWords = "октомври"; break;
                case 11: monthInWords = "ноември"; break;
                case 12: monthInWords = "декември"; break;
                default:
                    break;
            }


            int year = date.Year;

            string yearInWords = GetYearInWords(year);

            dateInWords.Append(string.Format("{0} {1} {2} {3}", dayInWords, monthInWords, yearInWords, "година"));

            return dateInWords.ToString();
        }

        public static string GetYearInWords(int year)
        {
            StringBuilder yearInWords = new StringBuilder();
            int hiledi = year / 1000;

            int hilediIStotici = (year / 100);
            int stotici = hilediIStotici % 10;
            int deseticiAndEdinici = (year % 100);
            int desetici = (deseticiAndEdinici / 10);
            int edinici = (year % 10);

            if (stotici == 0 && desetici == 0 && edinici == 0)
            {

                switch (hiledi)
                {
                    case 1: yearInWords.Append("хиляда "); break;
                    case 2: yearInWords.Append("две хилeдна "); break;
                    case 3: yearInWords.Append("три хиледна "); break;
                    default:
                        break;
                }

            }

            else
            {

                switch (hiledi)
                {
                    case 1: yearInWords.Append("хиляда "); break;
                    case 2: yearInWords.Append("две хиляди "); break;
                    case 3: yearInWords.Append("три хиляди "); break;
                    default:
                        break;

                }

                switch (stotici)
                {

                    case 1: yearInWords.Append("сто "); break;
                    case 2: yearInWords.Append("двеста "); break;
                    case 3: yearInWords.Append("триста "); break;
                    case 4: yearInWords.Append("четиристотин "); break;
                    case 5: yearInWords.Append("петстотин "); break;
                    case 6: yearInWords.Append("шестотин "); break;
                    case 7: yearInWords.Append("седемстотин "); break;
                    case 8: yearInWords.Append("осемстотин "); break;
                    case 9: yearInWords.Append("деветстотин "); break;
                }
                if (edinici == 0 && desetici != 0 && hilediIStotici != 0)
                {
                    yearInWords.Append("и ");
                }

                if (edinici != 0)
                {
                    switch (desetici)
                    {

                        case 1: edinici = year % 100; break;
                        case 2: yearInWords.Append("двадесет "); break;
                        case 3: yearInWords.Append("тридесет "); break;
                        case 4: yearInWords.Append("четиридесет "); break;
                        case 5: yearInWords.Append("петдесет "); break;
                        case 6: yearInWords.Append("шестдесет "); break;
                        case 7: yearInWords.Append("седемдесет "); break;
                        case 8: yearInWords.Append("осемдесет "); break;
                        case 9: yearInWords.Append("деведесет "); break;
                    }
                }
                else
                {
                    switch (desetici)
                    {

                        case 1: edinici = year % 100; break;
                        case 2: yearInWords.Append("двадесета "); break;
                        case 3: yearInWords.Append("тридесета "); break;
                        case 4: yearInWords.Append("четиридесета "); break;
                        case 5: yearInWords.Append("петдесета "); break;
                        case 6: yearInWords.Append("шестдесета "); break;
                        case 7: yearInWords.Append("седемдесета "); break;
                        case 8: yearInWords.Append("осемдесета "); break;
                        case 9: yearInWords.Append("деведесета "); break;
                    }
                }

                if ((edinici > 0 && edinici < 20))
                {
                    yearInWords.Append("и ");
                }
                switch (edinici)
                {
                    case 0:
                        {
                            if (desetici != 0 || hilediIStotici != 0)
                                break;
                            yearInWords.Append("нула");
                            break;
                        }
                    case 1: yearInWords.Append("първа"); break;
                    case 2: yearInWords.Append("втора"); break;
                    case 3: yearInWords.Append("трета"); break;
                    case 4: yearInWords.Append("четвърта"); break;
                    case 5: yearInWords.Append("пета"); break;
                    case 6: yearInWords.Append("шеста"); break;
                    case 7: yearInWords.Append("седма"); break;
                    case 8: yearInWords.Append("осма"); break;
                    case 9: yearInWords.Append("девета"); break;
                    case 10: yearInWords.Append("десета"); break;
                    case 11: yearInWords.Append("единадесета"); break;
                    case 12: yearInWords.Append("дванадесета"); break;
                    case 13: yearInWords.Append("тринадесета"); break;
                    case 14: yearInWords.Append("четиринадесета"); break;
                    case 15: yearInWords.Append("петнадесета"); break;
                    case 16: yearInWords.Append("шестнадесета"); break;
                    case 17: yearInWords.Append("седемнадесета"); break;
                    case 18: yearInWords.Append("осемнадесета"); break;
                    case 19: yearInWords.Append("деветнадесета"); break;

                }
            }

            return yearInWords.ToString();

        }
    }
}
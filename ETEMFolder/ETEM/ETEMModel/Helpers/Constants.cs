using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace ETEMModel.Helpers
{
    public static class Constants
    {

        public static string INVALID_HDN_MASTER_ROW = "";
        public static int ID_DISCIPLINE_NAME = 1;
        public static string INVALID_ID_STRING = "-1";
        public static int INVALID_ID = -1;
        public static int? INVALID_ID_NULLABLE = -1;
        public static int INVALID_ID_ZERO = 0;
        public static string INVALID_ID_ZERO_STRING = "0";
        public static string NOT_DEFINE = "->Not define<-";
        public static string NOT_SELECTED_LIST_VALUE_BG = "Моля, изберете!";
        public static string NOT_SELECTED_LIST_VALUE_SHORT = "-----";

        public static string SORTING_ASC = "ASC";
        public static string SORTING_DESC = "DESC";
        public static string TRUE_VALUE = "1";
        public static string TRUE_VALUE_TEXT = "True";
        public static string FALSE_VALUE_TEXT = "False";
        public static string FALSE_VALUE = "2";
        public static string DATE_SEPARATOR = ".";
        public static string TIME_SEPARATOR = ":";
        public static string DIRECTORY_SLASH = "\\";
        public static string ZeroYear = "Подговителна ";
        public static string ZeroYearKeyValue = "ZeroYear";
        public static new char[] Bulgarian_Vowels = new char[] { 'a', 'е', 'у', 'и', 'о', 'ъ', 'я', 'ю' };



        public static string DATE_PATTERN_MONTH_AS_WORD = "dd MMMM yyyy";
        public static CultureInfo CULTURE_INFO_EN = CultureInfo.CreateSpecificCulture("en-US");
        public static CultureInfo CULTURE_INFO_BG = CultureInfo.CreateSpecificCulture("bg-BG");

        public static string LOND_DATE_FORMAT_BY_SPACE = "dd MMMM yyyy";
        public static string SHORT_BG_DATE_PATTERN_SLASHES = "dd/MM/yyyy";
        public static string SHORT_AMERICAN_DATE_PATTERN_SLASHES = "MM/dd/yyyy";
        public static string SHORT_DATE_PATTERN = "dd.MM.yyyy";
        public static string SHORT_DATE_PATTERN_SLASHES = "dd/MM/yyyy";
        public static string SHORT_DATE_TIME_PATTERN = "dd.MM.yyyy HH:mm:ss";
        public static string DATE_SHORT_PATTERN_FOR_FILE_SUFFIX = "yyyyMMdd";
        public static string DATE_PATTERN_FOR_FILE_SUFFIX = "yyyyMMddHHmmss";
        public static string DATE_PATTERN_FOR_FILE_ADMINUNI_EXPORT_SUFFIX = "yyyyMMdd";
        public static string SHORT_DATE_PATTERN_CSV = "dd.MM.yyyy 'г.'";
        public static string SHORT_DATE_TIME_PATTERN_CSV = "dd.MM.yyyy HH:mm";
        public static char[] CHAR_SEPARATORS = { ' ', ',', '.', ';', '\'', ':', '/', '-' };
        public static char[] CHAR_SEPARATORS_SHORT = { ',', ';', '\'', '/', '-' };
        public static char[] CHAR_SEPARATOR_COMMA_AS_ARRAY = { ',' };
        public static string ALL_CHARACTERS = "~";

        public static string ERROR_MESSAGES_SEPARATOR = "|";

        public static string DECIMAL_NUMBER_TWO_PATTERN = "#.###.###.###.###.##0,00";
        public static string DECIMAL_NUMBER_FOUR_PATTERN = "#.###.###.###.###.##0,0000";


        public static List<Tuple<int, string, string>> MAIN_COLORS = new List<Tuple<int, string, string>>()
        {
            new Tuple<int,string,string>(0,"Lime","Лайм"),
            new Tuple<int,string,string>(1,"Gray","Сиво"),
            new Tuple<int,string,string>(2,"Green","Зелено"),
            new Tuple<int,string,string>(3,"Orange","Оранжево"),
            new Tuple<int,string,string>(4,"Red","Червено"),
            new Tuple<int,string,string>(5,"Olive","Маслинено"),
            new Tuple<int,string,string>(6,"Maroon","Кестеняво"),
            new Tuple<int,string,string>(7,"BlueViolet","Виолетово"),
            new Tuple<int,string,string>(8,"Brown","Кафяво"),
            new Tuple<int,string,string>(9,"Coral","Бледо оранжево"),
            new Tuple<int,string,string>(10,"Cyan","Светло синьо"),
            new Tuple<int,string,string>(11,"DarkBlue","Тъмно синьо"),
            new Tuple<int,string,string>(12,"Chocolate","Шоколадово"),
            new Tuple<int,string,string>(13,"Gold","Златисто"),
            new Tuple<int,string,string>(14,"Linen","Лин"),
            new Tuple<int,string,string>(15,"Magenta","Магента"),
        };


        #region LOG ACTIONS
        public static string ACTION_DELETE = "DELETE";
        public static string ACTION_UPDATE = "UPDATE";
        public static string ACTION_INSERT = "INSERT";
        #endregion


        public static string ADMIN_UNI_SPECIALITY_INTERNAL_REPORT_NAME = "BROI_PO_SPECIALNOSTI";
        public static string LECTURER = "Lecturer";
        public static string FILE_TXT_EXTENTION = ".txt";
        public static string FILE_DOCX_EXTENSION = ".docx";
        public static string FILE_XLSX_EXTENSION = ".xlsx";
        public static string FILE_ZIP_EXTENSION = ".zip";
        public static string FILE_CSV_EXTENSION = ".csv";
        public static string FILE_JPG_EXTENSION = ".jpg";
        public static string Empty_Word_Property = "....................";
        public static string DOCX_CUSTOM_PROPERTY_PREFIX = "Custom";
        public static string DEFAULT_PASSWORD = "ums";

        #region Admin Uni Export

        public static string EXPORT_EMPTY_STRING_NO_SEPARATOR = "''";
        public static string EXPORT_EMPTY_STRING_WITH_SEPARATOR = "''| ";
        public static string EXPORT_MISSING_MANDATORY_FIELD = "EXPORT_MISSING_MANDATORY_FIELD";
        public static string EXPORT_GRAD_STUDENT_CHANGE_NEEDED_CODE = "16";
        public static string EXPORT_GRAD_STUDENT_NO_CHANGE_NEEDED_CODE = "12";
        public static string EXPORT_DIPLOMA_IMAGES_FOLDER = "DocImages";
        public static int EXPORT_MIN_DIPLOMA_IMAGE_WIDTH = 1600;
        public static int EXPORT_MIN_DIPLOMA_IMAGE_HEIGHT = 1200;
        public static float EXPORT_MIN_DIPLOMA_IMAGE_PIXEL_PER_INCH = 72f;

        #endregion

        public static string Curriculum_Session_Abstract_Filter = "Curriculum_Session_Abstract_Filter";
        public static string Curriculum_Session_Manual_Filter = "Curriculum_Session_Manual_Filter";

        public static int EGN_LEN = 10;
        public static int Hundred = 100;
        public static int MAX_SCHEDULER_DAYS = 20;
        public static int MAX_CLASSES_COUNT_PER_DAY = 15;
        public static int MIN_CLASSES_COUNT_PER_DAY = 5;
        public static int MAX_BREAK_DURATION = 45;
        public static int MIN_BREAK_DURATION = 5;
        public static int CLASS_BREAK_STEP = 5;
        public static int MAX_CLASS_DURATION = 90;
        public static int MIN_CLASS_DURATION = 30;
        public static int CLASS_DURATION_STEP = 15;
        public static int STEP_HOURS_CLASSES_START = 15;
        public static int MAX_HOUR_CLASSES_START = 9;
        public static int MIN_HOUR_CLASSES_START = 7;
        public static int SIXTY_MINUTES = 60;
        public static int CLASS_SMALLEST_PART = 5;
        public static double FIVE_PIXELS_AS_DOUBLE = 5.615;
        public static int ONE_CELL_HEIGHT = 77;
        public static int EMPTY_SCHEDULE_CELL_HEIGHT = 80;
        public static int ONE_CELL_WIDTH = 62;
        public static int POSSIBLE_MISTAKE = 5;
        public static readonly string SESSION_USER_PROPERTIES = "USER_PROPERTIES";
        public static readonly string SESSION_PERSON_TYPE = "PERSON_TYPE";
        public static readonly string SESSION_USER_ACADEMICPERIOD = "SESSION_USER_ACADEMICPERIOD";
        public static string PERSON_LECTURER = "Lecturer";
        public static string ASSISTANT = "Assistant";
        public static string DOCTOR_OF_SIENCE = "PhDSecond";
        public static string ASSISTANT_PROFFESOR = "AssistantProfessor";
        public static string DOCENT = "Docent";
        public static string PROFFESOR = "Professor";
        public static int ZERO = 0;
        public static string DIGIT_ONE = "1";
        public static string DIGIT_TWO = "2";
        public static string DIGIT_THREE = "3";
        public static string DIGIT_FOUR = "4";
        public static string DIGIT_FIVE = "5";
        public static int FIVE_MINITE_PERIODS_PER_DAY = 131;
        public static double DivisorZeroHalf = 0.5;
        public static decimal SemesterWeeksLength = 15;
        public static string ONE_HOUR_MINUTES_STRING = "60";
        public static double HALF_OF_THE_DAY_CLASSES = 6;

        public static readonly string EXPORT_FONT_FAMILY_ERATO_SP_MEDIUM = "EratoSP_Medium";
        public static readonly string EXPORT_FONT_FAMILY_CALIBRI = "Calibri";
        public static readonly string EXPORT_FONT_FAMILY_CAMBRIA = "Cambria";
        public static readonly string EXPORT_NUMBER_FORMAT = "### ### ### ##0.00";
        public static readonly string EXPORT_NUMBER_FORMAT_WITH_CURRENCY = "#####0.00 лв.";
        public static readonly string DATE_FORMAT_MMDDYYY = "MM/dd/yyyy";
        public static readonly string EXPORT_DATE_FORMAT = "dd.MM.yyyy";
        public static readonly string EXPORT_DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm";
        public static readonly string EXPORT_SEPARATOR = "|";
        public static readonly string EXPORT_BRACKET = "\'";


        public static string SESSION_LOADED_PREVIOUS_EDUCATION = "SESSION_LOADED_PREVIOUS_EDUCATION";

        public static decimal DECIMAL_DIGIT_TWO = 2;

        public static decimal STUDENT_CANDIDATE_MIN_EXAM_MARK = 3;
        public static decimal STUDENT_CANDIDATE_MIN_EXAM_MARKS_SUM = 6;

        public static string FACULTYNO_PREFIX_BACHELOR_FII = "И-";
        public static string FACULTYNO_PREFIX_MASTER_FII = "ИМ-";
        public static string FACULTYNO_PREFIX_BACHELOR_FPI = "П";
        public static string FACULTYNO_PREFIX_MASTER_FPI = "ПМ-";


        #region KeyValuesIntCodes
        public static string CourseTypeIntCode = "Course";
        public static string SemesterTypeIntCode = "Semester";
        public static string DayOfWeekKeyTypeIntCode = "DayOfWeek";
        #endregion

        #region SEQUENCES NEXT VALUES
        public static string StreamNextValue = "StreamNextValue";
        public static string StreamManualNextValue = "StreamManualNextValue";
        public static string CampusApplicationRegNumber = "CampusApplicationRegNumber";
        public static string ScholarShipApplicationRegNumber = "ScholarShipApplicationRegNumber";
        public static string ArtModelApplicationNumber = "ArtModelApplicationNumber";
        public static string RequestRegNumber = "RequestRegNumber";
        public static string IDNNumber = "IDNNumber";

        #endregion

        //URL encryption/decryption key and IV
        // The key used for generating the encrypted string
        internal const string ENCRYPTION_KEY = "gdfgsdfg$!ertert$dfgsdfg!io;$1kahjfh";//random keyboard strokes		
        // The Initialization Vector for the DES encryption routine
        internal static readonly byte[] ENCRYPTION_IV = new byte[8] { 213, 12, 133, 149, 56, 91, 191, 213 };//some random numbers

        /// <summary>
        /// Връща стринг за ключа на списъка на ресурси на формите в application-a
        /// </summary>
        public static string APPLICATION_FORM_RESOURCES = "APPLICATION_FORM_RESOURCES";
        public static string APPLICATION_KEYTYPE_LIST = "APPLICATION_KEYTYPE_LIST";
        public static string APPLICATION_KEYVALUE_LIST = "APPLICATION_KEYVALUE_LIST";
        public static string APPLICATION_SETTING_LIST = "APPLICATION_SETTING_LIST";
        public static string APPLICATION_MODULES_LIST = "APPLICATION_MODULES_LIST";
        public static string APPLICATION_MENU_NODE_LIST = "APPLICATION_MENU_NODE_LIST";

        public static string APPLICATION_CRONPROCESSEXECUTION = "APPLICATION_CRONPROCESSEXECUTION";
        public static string APPLICATION_PERMITTEDACTION_LIST = "APPLICATION_PERMITTEDACTION_LIST";
        public static string APPLICATION_ALL_SESSIONS = "APPLICATION_ALL_SESSIONS";

        public static string[] alphabetCyrillic = { "~", "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ь", "Ъ", "Ю", "Я" };
        public static string[] alphabetLatin = { "~", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "Q", "X", "Y", "Z" };
        public static string[] daysOfWeekCyrillic = { "Понеделник", "Вторник", "Сряда", "Четвъртък", "Петък", "Събота" };
        public static Dictionary<int, string> DAY_OF_WEEK_AS_DICTIONARY = new Dictionary<int, string>()
            {
                {0,"Понеделник"},
                {1,"Вторник"},
                {2,"Сряда"},
                {3,"Четвъртък"},
                {4,"Петък"},
                {5,"Събота"},
            };
        public static Dictionary<int, string> SCHEDULE_DETAILS_STATUSES = new Dictionary<int, string>()
            {
                {1,BaseHelper.GetCaptionString("SCHEDULE_DETAILS_UNCOMFIRMED")},
                {2,BaseHelper.GetCaptionString("SCHEDULE_DETAILS_COMFIRMED")},
                {3,BaseHelper.GetCaptionString("SCHEDULE_DETAILS_SAVED")},
            };


        public static string DOWNLOAD_PAGE_PATH = "../Share/DownloadFile.aspx";

        #region Списък със страници
        //Административен модул
        public static string UMS_ADMIN_USERLIST = "UMS_ADMIN_USERLIST";
        public static string UMS_ADMIN_PERSONLIST = "UMS_ADMIN_PERSONLIST";
        public static string UMS_ADMIN_MAINDATAUNI = "UMS_ADMIN_MAINDATAUNI";
        public static string UMS_ADMIN_APPSETTING = "UMS_ADMIN_APPSETTING";
        public static string UMS_ADMIN_ROLELIST = "UMS_ADMIN_ROLELIST";
        public static string UMS_ADMIN_PERMITTEDACTIONLIST = "UMS_ADMIN_PERMITTEDACTIONLIST";
        public static string UMS_ADMIN_SPECIALITYCODELIST = "UMS_ADMIN_SPECIALITYCODELIST";
        public static string UMS_ADMIN_MENUNODEPAGE = "UMS_ADMIN_MENUNODEPAGE";
        public static string UMS_ADMIN_ONLINE_USERSLIST = "UMS_ADMIN_ONLINE_USERSLIST";
        public static string UMS_ADMIN_ADMINACTIVITIES = "UMS_ADMIN_ADMINACTIVITIES";
        public static string UMS_ADMIN_LECTUERERS_FACULTY_PROTOCOL = "UMS_ADMIN_LECTUERERS_FACULTY_PROTOCOL";
        public static string UMS_ADMIN_SCHEDULE_GENERAL_DISCIPLINES = "UMS_ADMIN_SCHEDULE_GENERAL_DISCIPLINES";
        public static string UMS_ADMIN_SCHEDULE_LANGEAGES = "UMS_ADMIN_SCHEDULE_LANGEAGES";
        public static string UMS_ADMIN_STREAM_AND_GROUP = "UMS_ADMIN_STREAM_AND_GROUP";
        public static string UMS_ADMIN_PRINT = "UMS_ADMIN_PRINT";
        public static string UMS_ADMIN_SCHEDULE_ADDITIONAL_SPECIALTY = "UMS_ADMIN_SCHEDULE_ADDITIONAL_SPECIALTY";
        public static string UMS_ADMIN_EXAM_PROTOCOL = "UMS_ADMIN_EXAM_PROTOCOL";
        public static string UMS_ADMIN_EXAM_PROTOCOL_DOCTOR = "UMS_ADMIN_EXAM_PROTOCOL_DOCTOR";
        public static string UMS_ADMIN_DISCIPLINELIST = "UMS_ADMIN_DISCIPLINELIST";
        public static string UMS_ADMIN_ALLOWIPLIST = "UMS_ADMIN_ALLOWIPLIST";
        public static string UMS_ADMIN_MODULELIST = "UMS_ADMIN_MODULELIST";
        public static string UMS_ADMIN_DOWNLOADLOGFILE = "UMS_ADMIN_DOWNLOADLOGFILE";
        public static string UMS_ADMIN_GROUPLIST = "UMS_ADMIN_GROUPLIST";
        public static string UMS_ADMIN_ADMINUNISTUDENTLIST = "UMS_ADMIN_ADMINUNISTUDENTLIST";



        //Модул Учебни дейности
        public static string UMS_EXAM_SCHEDULE_CALENDAR_LIST = "UMS_EXAM_SCHEDULE_CALENDAR_LIST";

        public static string UMS_AVERAGE_STUDENT_GRADE = "UMS_AVERAGE_STUDENT_GRADE";

        //Модул Административни дейности
        public static string UMS_ADMINISTRATIVEACTIVITIES_CAMPUSAPPLICATIONLIST = "UMS_ADMINISTRATIVEACTIVITIES_CAMPUSAPPLICATIONLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_CAMPUSADMISSIONLIST = "UMS_ADMINISTRATIVEACTIVITIES_CAMPUSADMISSIONLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_CAMPUSAPPLICATIONREPORTLIST = "UMS_ADMINISTRATIVEACTIVITIES_CAMPUSAPPLICATIONREPORTLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPAPPLICATIONLIST = "UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPAPPLICATIONLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPADMISSIONLIST = "UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPADMISSIONLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPPAYMENTLIST = "UMS_ADMINISTRATIVEACTIVITIES_SCHOLARSHIPPAYMENTLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_DISCIPLINE_CLASSROOM = "UMS_ADMINISTRATIVEACTIVITIES_DISCIPLINE_CLASSROOM";
        public static string UMS_ADMINISTRATIVEACTIVITIES_CIVILCONTRACTLIST = "UMS_ADMINISTRATIVEACTIVITIES_CIVILCONTRACTLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_REQUESTLIST = "UMS_ADMINISTRATIVEACTIVITIES_REQUESTLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_RANGELIST = "UMS_ADMINISTRATIVEACTIVITIES_RANGELIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_SURVEYLIST = "UMS_ADMINISTRATIVEACTIVITIES__SURVEYLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_SURVEYSENTLIST = "UMS_ADMINISTRATIVEACTIVITIES__SURVEYSENTLIST";

        //Модул Модели
        public static string UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELLIST = "UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELAPPLICATIONLIST = "UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELAPPLICATIONLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELCONTRACTLIST = "UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELCONTRACTLIST";
        public static string UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELCHARGINGLIST = "UMS_ADMINISTRATIVEACTIVITIES_ARTMODEL_ARTMODELCHARGINGLIST";
        public static string UMS_ARTMODELS_ARTMODELAPPLICATIONPRINT = "UMS_ARTMODELS_ARTMODELAPPLICATIONPRINT";

        //Модел Служители
        public static string UMS_ADMINISTRATIVEACTIVITIES_EMPLOYELIST = "UMS_ADMINISTRATIVEACTIVITIES_EMPLOYELIST";

        //Външни експерти
        public static string UMS_EXTERNALEXPERTS_EXTERNALEXPERTLIST = "UMS_EXTERNALEXPERTS_EXTERNALEXPERTLIST";



        public static string UMS_LearningActivity_WeeklyPlot = "UMS_LearningActivity_WeeklyPlot";


        public static string UMS_UI_NOPERMISSION = "UMS_UI_NOPERMISSION";
        public static string UMS_UI_INTERNALPAGEINFO = "UMS_UI_INTERNALPAGEINFO";


        public static string UMS_LEARNINGACTIVITY_TIMESCHEDULE = "UMS_LEARNINGACTIVITY_TIMESCHEDULE";

        //Модул Основни страници
        public static string UMS_SHARE_LOGIN = "UMS_SHARE_LOGIN";
        public static string UMS_SHARE_WELCOME = "UMS_SHARE_WELCOME";
        public static string UMS_SHARE_LOGOUT = "UMS_SHARE_LOGOUT";
        public static string UMS_SHARE_UPLOADFILE = "UMS_SHARE_UPLOADFILE";
        public static string UMS_SHARE_TESTPAGE = "UMS_SHARE_TESTPAGE";
        public static string UMS_SHARE_NOTIFICATIONLIST = "UMS_SHARE_NOTIFICATIONLIST";
        public static string UMS_SHARE_MULTIFILESUPLOAD = "MULTIFILESUPLOAD";

        //Modul PhD
        public static string UMS_PHD_PHDLIST = "UMS_PHD_PHDLIST";
        public static string UMS_PHD_PHDCOURSELIST = "UMS_PHD_PHDCOURSELIST";

        //Modul Lecturer
        public static string UMS_LECTURERS_LECTURERLIST = "UMS_LECTURERS_LECTURERLIST";
        public static string UMS_LECTURERS_LECTURERPAYMENTLIST = "UMS_LECTURERS_LECTURERPAYMENTLIST";
        public static string UMS_LECTURERS_CONTRACTLECTURERLIST = "UMS_LECTURERS_CONTRACTLECTURERLIST";
        public static string UMS_LECTURERS_LECTURERREPORTLIST = "UMS_LECTURERS_LECTURERREPORTLIST";
        public static string UMS_LECTURERS_WEEKSCHEDULE = "UMS_LECTURERS_WEEKSCHEDULE";
        public static string UMS_LECTURERS_LECTURERREPORTPRINT = "UMS_LECTURERS_LECTURERREPORTPRINT";
        public static string UMS_LECTURERS_LECTURERWORKLOADLIST = "UMS_LECTURERS_LECTURERWORKLOADLIST";
        public static string UMS_PRINT_STUDENT_SCHEDULE = "UMS_PRINT_STUDENT_SCHEDULE";
        public static string UMS_LECTURERS_LECTURERFILE = "UMS_LECTURERS_LECTURERFILE";

        //Modul Student
        public static string UMS_STUDENTS_PREPARATIONCOURSELIST = "UMS_STUDENTS_PREPARATIONCOURSELIST";
        public static string UMS_STUDENTS_PREPARATIONCOURSEAPPLICATIONLIST = "UMS_STUDENTS_PREPARATIONCOURSEAPPLICATIONLIST";
        public static string UMS_STUDENTS_STUDENTCANDIDATELIST = "UMS_STUDENTS_STUDENTCANDIDATELIST";
        public static string UMS_STUDENTS_SPECIALITYFORAPPLICATIONLIST = "UMS_STUDENTS_SPECIALITYFORAPPLICATIONLIST";
        public static string UMS_STUDENTS_COURSELIST = "UMS_STUDENTS_COURSELIST";
        public static string UMS_STUDENTS_STUDENTSURVEYLIST = "UMS_STUDENTS_STUDENTSURVEYLIST";
        public static string UMS_STUDENTS_STUDENTFILE = "UMS_STUDENTS_STUDENTFILE";
        public static string UMS_STUDENTS_REQUESTCANDIDATEPREPARATORYCOURSES = "REQUESTCANDIDATEPREPARATORYCOURSES";
        public static string UMS_STUDENTS_GRADUATED_LIST = "UMS_STUDENTS_GRADUATED_LIST";
        public static string UMS_STUDENTS_ASSURANCE = "UMS_STUDENTS_ASSURANCE";
        public static string UMS_STUDENTS_STUDENTSURVEYDATAPRINT = "UMS_STUDENTS_STUDENTSURVEYDATAPRINT";

        //Mодул Входящи и Изходящи документи 
        public static string UMS_INOUTDOCUMENT_INOUTDOCUMENTLIST = "UMS_INOUTDOCUMENT_INOUTDOCUMENTLIST";
        public static string UMS_INOUTDOCUMENT_ORDERLIST = "UMS_INOUTDOCUMENT_ORDERLIST";
        public static string UMS_INOUTDOCUMENT_TASKLIST = "UMS_INOUTDOCUMENT_TASKLIST";
        public static string UMS_INOUTDOCUMENT_INCOMINGDOCUMENTDATAPRINT = "UMS_INOUTDOCUMENT_INCOMINGDOCUMENTDATAPRINT";
        public static string UMS_INOUTDOCUMENT_OUTGOINGDOCUMENTDATAPRINT = "UMS_INOUTDOCUMENT_OUTGOINGDOCUMENTDATAPRINT";
        public static string UMS_INOUTDOCUMENT_ORDERDATAPRINT = "UMS_INOUTDOCUMENT_ORDERDATAPRINT";

        //Модул Музей
        public static string UMS_MUSEUM_LIST = "UMS_MUSEUM_LIST";

        //Модул Проекти
        public static string UMS_PROJECTS_LIST = "UMS_PROJECTS_LIST";

        //Модул 
        public static string UMS_EVENTS_LIST = "UMS_EVENTS_LIST";

        //Модул Export
        public static string UMS_EXPORT_ADMIN_UNI = "UMS_EXPORT_ADMIN_UNI";

        //СПРАВКИ ПРИКАЧЕНИ ФАЙЛОВЕ
        public static string UMS_ADMIN_ACCOUNTINGATTACHMENTLIST = "UMS_ADMIN_ACCOUNTINGATTACHMENTLIST";
        public static string UMS_SHARE_ATTACHMENTLIST = "UMS_SHARE_ATTACHMENTLIST";

        //ETEM
        //Модул (Изчисляване на разходите) Cost calculation
        public static string ETEM_COSTCALCULATION_DIEPRICESLIST             = "ETEM_COSTCALCULATION_DIEPRICESLIST";
        public static string ETEM_COSTCALCULATION_DIEPRICELISTDETAILSLIST   = "ETEM_COSTCALCULATION_DIEPRICELISTDETAILSLIST";
        public static string ETEM_COSTCALCULATION_MATERIALPRICESLIST        = "ETEM_COSTCALCULATION_MATERIALPRICESLIST";
        public static string ETEM_COSTCALCULATION_DIEFORMULALIST            = "ETEM_COSTCALCULATION_DIEFORMULALIST";
        public static string ETEM_COSTCALCULATION_PROFILESLIST              = "ETEM_COSTCALCULATION_PROFILESLIST";
        public static string ETEM_COSTCALCULATION_PRODUCTIVITYANDSCRAPLIST  = "ETEM_COSTCALCULATION_PRODUCTIVITYANDSCRAPLIST";
        public static string ETEM_COSTCALCULATION_PRODUCTIVITYANDSCRAPDETAILLIST = "ETEM_COSTCALCULATION_PRODUCTIVITYANDSCRAPDETAILLIST";

        public static string ETEM_COSTCALCULATION_OFFERSLIST                = "ETEM_COSTCALCULATION_OFFERSLIST";

        public static string ETEM_COSTCALCULATION_SAPDATALIST               = "ETEM_COSTCALCULATION_SAPDATALIST";
        public static string ETEM_COSTCALCULATION_SAPDATAEXPENSESLIST       = "ETEM_COSTCALCULATION_SAPDATAEXPENSESLIST";
        public static string ETEM_COSTCALCULATION_SAPDATAQUANTITYLIST       = "ETEM_COSTCALCULATION_SAPDATAQUANTITYLIST";
        public static string ETEM_COSTCALCULATION_COMMISSIONSBYAGENTSLIST   = "ETEM_COSTCALCULATION_COMMISSIONSBYAGENTSLIST";

        public static string ETEM_COSTCALCULATION_AVERAGEOUTTURNOVERTIMELIST= "ETEM_COSTCALCULATION_AVERAGEOUTTURNOVERTIMELIST";

        #endregion

        #region Списък с модули
        public static string MODULE_ADMIN = "MODULE_ADMIN";
        public static string MODULE_FINANCE = "MODULE_FINANCE";
        public static string MODULE_MUSEUM = "MODULE_MUSEUM";
        public static string MODULE_ADMINISTRATIVEACTIVITIES = "MODULE_ADMINISTRATIVEACTIVITIES";
        public static string MODULE_STUDENT = "MODULE_STUDENT";
        public static string MODULE_LECTURER = "MODULE_LECTURER";
        public static string MODULE_INOUTDOCUMENT = "MODULE_INOUTDOCUMENT";
        public static string MODULE_SHARE = "MODULE_SHARE";
        public static string MODULE_UI = "MODULE_UI";
        public static string MODULE_LEARNINGACTIVITY = "MODULE_LEARNINGACTIVITY";
        public static string MODULE_SURVEY = "MODULE_SURVEY";

        public static string MODULE_PHD = "MODULE_PHD";
        public static string MODULE_PROJECTS = "MODULE_PROJECTS";
        public static string MODULE_EVENTS = "MODULE_EVENTS";

        public static string MODULE_EXPORT = "MODULE_EXPORT";

        public static string MODULE_REPORTS = "MODULE_REPORTS"; //СПРАВКИ
        
        
        public static string MODULE_NOMENCLATURES = "MODULE_NOMENCLATURES"; //Номенклатури
        public static string MODULE_SUPPORT_HISTORY = "MODULE_SUPPORT_HISTORY"; //Поддръжка § История
        public static string MODULE_SETTINGS = "MODULE_SETTINGS"; //Настройки
        public static string MODULE_PERMISSION = "MODULE_PERMISSION"; //Права в системата
        public static string MODULE_COST_CALCULATION = "MODULE_COST_CALCULATION"; //Изчисляване на разходите

        #endregion



        public static int ForeignerLocationPrefix = 100000;

        public static string SessionClear = "SessionClear";





        public static int NEW_PASSWORD_LENGTH = 6;

        public static int NUMBER_Non_Alphanumeric_Characters = 1;
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public static class ETEMEnums
    {
        #region AppStartupErrorCode
        public enum AppStartupErrorCode
        {
            None = 0,
            InitializePropNamesError,
            RefreshRolesFromDbError,
            ReadDbInfoError,
            BackgroundThreadStartError,
        }
        #endregion

        #region Result
        public enum ResultEnum
        {
            Success,
            Error,
            Warning
        }
        #endregion

        #region PagingRowsCount
        public enum PagingRowsCountEnum
        {
            TenRowsPerPage,
            TwentyRowsPerPage,
            ThirtyRowsPerPage,
            FortyRowsPerPage,
            FiftyRowsPerPage,
            AllRows
        }
        #endregion

        #region UserStatuses
        public enum UserStatusEnum
        {
            Active,
            Inactive,
            TemporarilyInactive
        }
        #endregion

        #region GroupSecuritySettings
        public enum GroupSecuritySettings
        {
            Admin,
            CostCalculation,
            Share
        }
        #endregion

        #region AttachmentDocumentType
        public enum AttachmentDocumentTypeEnum
        {
            AdmissionInformation
        }
        #endregion

        #region FinanceReportType
        public enum FinanceReportTypeEnum
        {
            FinanceReport
        }
        #endregion

        #region DisciplineCodes
        public enum DisciplineCodes
        {
            ForeignLanguage
        }
        #endregion

        #region SecuritySettings
        public enum SecuritySettings
        {
            /***************************************************/
            /*Действия за прикачени на файлове ***************/
            /**************************************************/

            AttachmentAdmissionInfoList,
            AttachmentAdmissionInfoView,
            AttachmentAdmissionInfoSave,
            /***********************************/
            /*Глобални Действия ***************/
            /**********************************/


            
            /*Вход в системата*/
            Login,
            /*Достъп до log файла, активни потребители, модули, IP адреси*/
            ShowDownloadLogFile,
            ShowOnlineUsersList,
            ShowModuleList,
            ShowAllowIPList,

            /***********************************/
            /* Действия за KeyValue,KeyType*/
            /**********************************/
            KeyTypeSave,
            KeyValueSave,

            /***********************************/
            /* Действия за Роли*/
            /**********************************/
            AddUserRole,
            RolesMenuSave,
            RoleShowList,
            RoleEditView,
            RoleSave,
            RemoveUserRole,

            /********************************/
            /*Действия настройки           */
            /********************************/
            SettingSave,
            PermittedActionSave,
            PermittedActionMergeSettings,
            SettingMergeSettings,
            UserSave,
            LoginAS,
            NavUrlSave,
            MenuNodeSave,
            RemoveMenuNode,
            UserListShow,
            ChangeCurrentPeriodInSession,
            UpdateStudentInELearning,
            PersonSave,
            OnlineUsersListDelete,
            NotificationSave,
            AllowIPSave,
            ModuleSave,
            EmployeSave,

            //* Модул Изчисляване на разходите (Cost calculation) *//

            /********************************************/
            /* Действия за Die Price Lists by Vendors */
            /******************************************/
            DiePricesListView,
            DiePriceListPreview,
            DiePriceListSave,
            DiePriceListImportDetails,
            DiePriceListDelete,
            /***********************************************/
            /* Действия за Die Price Lists by Dimensions */
            /*********************************************/
            DiePriceListDetailsListView,
            DiePriceListDetailsPreview,
            DiePriceListDetailsSave,
            DiePriceListDetailsDelete,
            
            /******************************************/
            /* Действия за LME&PREMIUM Price Lists */
            /****************************************/
            MaterialPricesListView,
            MaterialPriceListPreview,
            MaterialPriceListSave,
            MaterialPriceListDelete,

            /******************************************/
            /* Действия за Productivity & Scrap */
            /****************************************/
            ProductivityAndScrapListView,
            ProductivityAndScrapPreview,
            ProductivityAndScrapSave,
            ProductivityAndScrapDelete,
            ProductivityAndScrapImport,

            /*******************************************/
            /* Действия за SAP Data by Cost Centers */
            /*****************************************/
            SAPDataListView,
            SAPDataPreview,
            SAPDataSave,
            SAPDataImport,
            SAPDataDelete,

            /*******************************************/
            /* Действия за Expenses by Cost Centers */
            /*****************************************/
            SAPDataExpenseListView,
            SAPDataExpensePreview,
            SAPDataExpenseSave,
            SAPDataExpenseDelete,

            /********************************************/
            /* Действия за Quantities by Cost Centers */
            /******************************************/
            SAPDataQuantityListView,
            SAPDataQuantityPreview,
            SAPDataQuantitySave,
            SAPDataQuantityDelete,

            /********************************************/
            /* Действия за Quantities by Cost Centers */
            /******************************************/
            CommissionsByAgentListView,
            CommissionsByAgentPreview,
            CommissionsByAgentSave,
            CommissionsByAgentDelete,

            /***************************************/
            /* Действия за Прикачване на файл */
            /**************************************/
            UploadedFileSave,
          
            /***************************************/
            /* Действия за групи съобщения*/
            /**************************************/
            GroupShowList,
            GroupSave,
            GroupPreview,
            GroupPersonAddDelete,
            UsingFullFunctionalityNotif,
            /********************************************/
            /*Действие за прикачане на документи/справки/
            / * **************************************/
            AttachmentShowList,
            AttachmentPreview,
            AttachmentSave,
            /*********************************/
            /* Действия за формули */
            /********************************/
            DieFormulaListView,
            DieFormulaPreview,
            DieFormulaSave,
            /*********************************/
            /* Действия за ОФЕРТТА */
            /********************************/
            OfferSave,
            ShowOfferFullList,
            /*********************************/
            /* Действия за профили */
            /********************************/
            ProfilesShowList,
            ProfilesPreview,
            ProfilesSave,
            /*********************************/
            /* Действия за профил валидации*/
            /********************************/
            ProfileSettingValidationSave,

             /******************************************/
            /* Действия за LME&PREMIUM Price Lists */
            /****************************************/
            AverageOutturnOverTimeListView,
            AverageOutturnOverTimeListPreview,
            AverageOutturnOverTimeListSave,
            AverageOutturnOverTimeListDelete,
        }
        #endregion

        #region File Size
        public enum TypeFileSize
        {
            B,
            KB,
            MB,
            GB,
        }

        #endregion

        #region AppSettings
        public enum AppSettings
        {
            AutomaticDebugLogin,
            CurrentYear,
            CurrentPeriod,
            CronProcessStart,
            CronProcessStartPeriod,
            UserIDBindWithSystem,
            EmailForReciveError,
            EmailForSending,
            PasswordMinLength,
            PageSize,
            CurrentUNIId,
            ResourcesFolderName,
            WebResourcesFolderName,
            FolderTemplates,
            DocumentScholarshipPayroll,
            WebApplicationName,
            WebSessionTimeOut,            
            /***********************************************/
            /* Данни за e-mail сървър*/
            /**********************************************/
            SendExternalMail,
            MailServer,
            MailServerPort,
            MailFromPassword,
            MailFromPasswordNew,
            DefaultEmail,
            MailServerPop3,
            MailServerPop3Port,
            MailDeliverySubsystemEmail,
            WaitCheckMailDeliveryInMinutes,
            /***********************************************/
            /* Данни за Домайна сървър*/
            /**********************************************/
            DomainName,

            /***********************************************/
            /* Данни за изпитна сисия */
            /**********************************************/

            ExamCalendarSubFolder,
            /***********************************************/
            /* Данни за изпитна експорт */
            /**********************************************/
            ExportStudents,
            ExportAcadStaff,
            ExportGraduateStudents,
            /***********************************************/
            /* Данни за проекти */
            /**********************************************/
            ProjectsSubForlder,
            /***********************************************/
            /* Данни за учебни планове */
            /**********************************************/
            MakeLogInDB,//Подоробен лог в базата данни

            // ETEM
            TemplateDiePriceListDetails,
            TemplateSAPExpensesAndQuantities,
            Template_SAP_ProductivityAndScrap,
            ConsumptionRatio,
            ScrapValuePercent,
            PressProducitivity_KG_MH,
            PackagingProducitivity_KG_MH,
            Administration_expenses,
            Sales_expenses,
            Financial_fixed_expenses,
            Interest,
            Material_cost_for_packaging,
            Ratio_consumption_packaging,
            SavingsRate,
            LengthOfFinalPC_MIN,
            LengthOfFinalPC_MAX,
            Tickness_MIN,
            Tickness_MAX,
            Weight_Per_Meter_MIN,
            Weight_Per_Meter_MAX
        }
        #endregion

        #region AppSettingsClass
        public enum AppSettingsClass
        {
            Boolean,
            Integer,
            String,
            Double,
            Date,
            List,
            EMail
        }
        #endregion
        
        #region KeyTypes
        public enum KeyTypeEnum
        {
            AcadRank,
            BasicClass,
            BasicSchoolType,
            BudgetFrom,
            CampusApplicationStatus,
            ScholarShipApplicationStatus,
            EducationDuration,
            EducationForm,
            FamilyRelation,
            HomeState,
            IdentityNumberType,
            Nationality,
            NotificationStatus,
            NSICode,
            //OPS,
            PermanentAddress,
            PersonType,
            ProfArea,
            ProfGroup,
            ReasonForAcc,
            SchoolTypeZNP,
            Sex,
            StudentDegree,
            SpecNo,
            StatusStudent,
            StudyType,
            TypeSE,
            UserStatus,
            YES_NO,
            CategoryDiscipline,
            AcademicYear,
            Semester,
            ApplicationReason,
            CampusRoomType,
            CampusApplicantType,
            ScholarShipType,
            ScholarShipCategory,
            Maritalstatus,
            Course,
            Month,
            StudentCandidateAppReason,
            StudentCandidatesExportListType,
            CourseAttendingType,
            StudentCandidatesRankingType,
            EmailSubject,
            EmailBody,
            RequestMaterialType,
            RequestMaterialCategory,
            RequestMaterialStatus,
            RequestedItemCategory,
            RequestedItemGroup,
            PagingRowsCount,
            InOutDocType,
            ApprovalType,
            ApprovalDecision,
            ApprovalObjectType,
            ContractStatus,
            Section,
            RequestPaymentType,
            AdmissionInfoDocumentsType,
            NumberOfCavities,
            ProfileType,
            ProfileCategory,
            ProfileComplexity,
            CostCenter,
            ExpensesType,
            QuantityType,
            ExpensesTypeGroup
        }
        #endregion

        #region Countries
        public enum CountryCodeEnum
        {
            NONE = 0,
            AD,
            AE,
            AF,
            AG,
            AI,
            AL,
            AM,
            AN,
            AO,
            AQ,
            AR,
            AS,
            AT,
            AU,
            AW,
            AZ,
            BA,
            BB,
            BD,
            BE,
            BF,
            BG,
            BH,
            BI,
            BJ,
            BM,
            BN,
            BO,
            BR,
            BS,
            BT,
            BV,
            BW,
            BY,
            BZ,
            CA,
            CC,
            CD,
            CF,
            CG,
            CH,
            CI,
            CK,
            CL,
            CM,
            CN,
            CO,
            CR,
            CU,
            CV,
            CX,
            CY,
            CZ,
            DE,
            DJ,
            DK,
            DM,
            DO,
            DZ,
            EC,
            EE,
            EG,
            EH,
            ER,
            ES,
            ET,
            FI,
            FJ,
            FK,
            FM,
            FO,
            FR,
            GA,
            GB,
            GD,
            GE,
            GF,
            GG,
            GH,
            GI,
            GL,
            GM,
            GN,
            GP,
            GQ,
            GR,
            GS,
            GT,
            GU,
            GW,
            GY,
            HK,
            HM,
            HN,
            HR,
            HT,
            HU,
            ID,
            IE,
            IL,
            IM,
            IN,
            IO,
            IQ,
            IR,
            IS,
            IT,
            JE,
            JM,
            JO,
            JP,
            KE,
            KG,
            KH,
            KI,
            KM,
            KN,
            KP,
            KR,
            KW,
            KY,
            KZ,
            LA,
            LB,
            LC,
            LI,
            LK,
            LR,
            LS,
            LT,
            LU,
            LV,
            LY,
            MA,
            MC,
            MD,
            ME,
            MG,
            MH,
            MK,
            ML,
            MM,
            MN,
            MO,
            MP,
            MQ,
            MR,
            MS,
            MT,
            MU,
            MV,
            MW,
            MX,
            MY,
            MZ,
            NA,
            NC,
            NE,
            NF,
            NG,
            NI,
            NL,
            NO,
            NP,
            NR,
            NU,
            NZ,
            OM,
            PA,
            PE,
            PF,
            PG,
            PH,
            PK,
            PL,
            PM,
            PN,
            PR,
            PS,
            PT,
            PW,
            PY,
            QA,
            RE,
            RO,
            RS,
            RU,
            RW,
            SA,
            SB,
            SC,
            SD,
            SE,
            SG,
            SH,
            SI,
            SJ,
            SK,
            SL,
            SM,
            SN,
            SO,
            SR,
            ST,
            SV,
            SY,
            SZ,
            TC,
            TD,
            TF,
            TG,
            TH,
            TJ,
            TK,
            TL,
            TM,
            TN,
            TO,
            TR,
            TT,
            TV,
            TW,
            TZ,
            UA,
            UG,
            UM,
            US,
            UY,
            UZ,
            VA,
            VC,
            VE,
            VG,
            VI,
            VN,
            VU,
            WF,
            WS,
            XK,
            YE,
            YT,
            YU,
            YU1,
            ZA,
            ZM,
            ZW
        }
        #endregion

        #region Districts
        public enum DistrictCodeEnum
        {
            BGS,
            BLG,
            DOB,
            GAB,
            HKV,
            JAM,
            KNL,
            KRZ,
            LOV,
            MON,
            PAZ,
            PDV,
            PER,
            PVN,
            RAZ,
            RSE,
            SFO,
            SHU,
            SLS,
            SLV,
            SML,
            SOF,
            SZR,
            TGV,
            VAR,
            VID,
            VRC,
            VTR,
            FRN = 29,
            NAN = 30
        }
        #endregion

        #region Municipalities
        public enum MunicipalitiesEnum
        {
            FRN1,
            NONE
        }
        #endregion

        #region PersonTypes
        public enum PersonTypeEnum
        {
            Aall,
            Administrator,
            AdministrativeActivity,
            Lecturer,
            Student,
            ArtModel,
            PhD,
            StudentCandidate,
            Employe,
            ExternalExpert
        }
        #endregion

        #region Roles
        public enum RoleEnum
        {
            Administrator,
            AdministrativeActivity,
            Clark,
            ClarkОrders,
            DepartmentSupervisior,
            Employee,
            ExpertCampuses,
            ExpertScholarShips,
            General,
            HostCampuses,
            HR,
            Lecturer,
            Museum,
            OrganizerМodels,
            PhD,
            Student,
            SupplyActivities,
            Requests,
            SUPPORT
        }
        #endregion
                
        #region IdentityNumberType
        public enum IdentityNumberTypeEnum
        {
            EGN,
            LNK,
            IDN
        }
        #endregion
        
        #region Sex
        public enum SexEnum
        {
            Man,
            Woman
        }
        #endregion

        #region YesNo
        public enum YesNoEnum
        {
            Yes,
            No
        }
        #endregion

        #region Months
        public enum MonthEnum
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        #endregion

        #region EmailType
        public enum EmailTypeEnum
        {
            StudentCandidatesRanked,
            PotentialStudentCandidates,
            Students,
            StudentCandidates,
            Lecturers,
            GroupStudentsLecturersEmployeesPhds,
            Users,
            PhD
        }
        #endregion

        #region EmailSubject
        public enum EmailSubjectEnum
        {
            StudentCandidateRanked,
            WrongSentEmails
        }
        #endregion

        #region EmailBody
        public enum EmailBodyEnum
        {
            StudentCandidateRanked,
            WrongSentEmails
        }
        #endregion
               
        /// <summary>
        /// It's for single and plural count of the words
        /// </summary>
        #region SinglePlular
        public enum WordNumber
        {
            Single,
            Plural,
            DontMatter,
        }
        #endregion

        /// <summary>
        /// word form
        /// </summary>
        #region SinglePlular
        public enum WordGenderForm
        {
            MaleForm,
            FemaleForm,
            ChildForm,
            DontMatter,
        }
        #endregion

        #region SchedulerCssThemes
        public enum SchedulerCssThemes
        {
            calendar_green,
            //calendar_blue,    
            calendar_white,
            calendar_transparent,
            //scheduler_green,   
            //scheduler_blue,    
            //scheduler_white,    
            //scheduler_transparent,    
            //scheduler_8
        }
        #endregion
            
        #region ApprovalType
        public enum ApprovalTypeEnum
        {
            Approval,
            Confirmation
        }
        #endregion

        #region ApprovalObjectType
        public enum ApprovalObjectTypeEnum
        {
            NONE,
            Request
        }
        #endregion
        
        #region ApprovalDecision
        public enum ApprovalDecisionEnum
        {
            Approve,
            Disapprove,
            BackForCorrection,
            StartApproval
        }
        #endregion

        #region Section
        public enum SectionEnum
        {
            AdministrativeSection,
            EconomicsSection,
            AcademicMethodicalSection,
            FinancialAccountingSection
        }
        #endregion

        #region NumberOfCavities
        public enum NumberOfCavitiesEnum
        {
            OneCavity,
            TwoCavities,
            FourCavities
        }
        #endregion

        #region ProfileType
        public enum ProfileTypeEnum
        {
            ProfileTypeCIR,
            ProfileTypeF,
            ProfileTypeGAMA,
            ProfileTypeH,
            ProfileTypeUO,
            ProfileTypeREC,
            ProfileTypeT,
            ProfileTypeZ
        }
        #endregion

        #region ProfileCategory
        public enum ProfileCategoryEnum
        {
            FLAT,
            PORTHOLE
        }
        #endregion

        #region CostCenter
        public enum CostCenterEnum
        {
            Breda,
            COMETAL1,
            COMETAL2,
            COMETAL3,
            DIESDepartment,
            Farrel,
            Gautchi,
            Packaging,
            QCExtrusion,
            SMS1,
            SMS2
        }
        #endregion

        #region ExpensesType
        public enum ExpensesTypeEnum
        {
            DepartmentExpensesFixed,
            DepartmentExpensesVariable,
            DirectDepreciation,
            DirectLaborVariable,
            ElectricityVariable,
            EnergyFixed,
            FactoryExpensesFixed,
            FactoryServicesExpensesFixed,
            IndirectDepreciation,
            InternalTransportationExpensesFixed,
            InternalTransportationExpensesVariable,
            MaintenanceExpensesFixed,
            MaintenanceExpensesVariable,
            MaintenSparePartsFixed,
            MaintenSparePartsVariable,
            NaturalGasVariable,
            OtherFixedCostsFixed,
            OtherVariableCostVariable,
            PackagingMaterialsVariable,
            ProdMaterialVariable,
            SalariesPersonnelExpensesFixed,
            SecondaryFactoryMachineryExpensesFixed,
            SecondaryFactoryMachineryExpensesVariable
        }
        #endregion

        #region QuantityType
        public enum QuantityTypeEnum
        {
            Electricity,
            LabourHours,
            MachineHours,
            NaturalGas,
            ProductionQuantity
        }
        #endregion

        #region ExpensesTypeGroup
        public enum ExpensesTypeGroupEnum
        {
            DirectDepreciationGroup,
            DirectLaborGroup,
            ElectricityGroup,
            FeesAndOtherMaintenanceExpensesGroup,
            IndirectDepartmentExpensesGroup,
            IndirectDepreciationGroup,
            MaintenSparePartGroup,
            NaturalGasGroup,
            OtherFixedCostsGroup,
            OtherIndirectExpensesGroup,
            ProdMaterialGroup
        }
        #endregion

        #region CalculationType
        public enum CalculationType
        {
            EUR_MH,
            EUR_TON,
        }
        #endregion

        public enum DataRowType
        {
            Data,
            Total
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using MoreLinq;
using ETEMModel.Models.DataView;



namespace ETEM.Controls.Common
{
    public partial class SMCDropDownList : BaseUserControl
    {
        public event EventHandler SelectedIndexChanged;

        private bool showButton = true;

        public bool ShowButton
        {
            get { return showButton; }
            set { showButton = value; }
        }

        private bool addingDefaultValue = true;

        public bool AddingDefaultValue
        {
            get { return addingDefaultValue; }
            set { addingDefaultValue = value; }
        }


        private bool externalDataSourceType = false;
        public bool ExternalDataSourceType
        {
            get { return externalDataSourceType; }
            set { externalDataSourceType = value; }
        }



        private List<KeyValue> list = new List<KeyValue>();

        public List<KeyValue> List
        {
            get { return list; }
            set { list = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string WhereKeyValueIntCodesEQ { get; set; }
        public string WhereKeyValueIntCodesNotEQ { get; set; }
        public string WeekNotEQ { get; set; }

        public string CurrentUNIID
        {
            get { return this.hdnUNIID.Value; }
            set { this.hdnUNIID.Value = value; }
        }

        public string CustomFieldUseEverything
        {
            get { return this.hdnCustomField.Value; }
            set { this.hdnCustomField.Value = value; }
        }

        public string DefaultValueByName
        {
            get { return this.hdnDefaultValue.Value; }
            set { this.hdnDefaultValue.Value = value; }
        }

        public string DataSourceType { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public string KeyTypeIntCode { get; set; }
        public string KeyValueDataTextField { get; set; }
        public string KeyValueDataValueField { get; set; }
        public string KeyValueDefault { get; set; }
        public string KeyValueDefaultColumn { get; set; }
        public string AdditionalParam { get; set; }
        public string PerioActualStart { get; set; }
        public string PerioActualEnd { get; set; }



        public Dictionary<string, int> DictionaryAdditionalParam { get; set; }

        public bool UseShortDefaultValue { get; set; }

        public bool SetItemTextAsTooltip { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.SelectedIndexChanged != null)
            {
                this.DropDownList.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
            }

            if (string.IsNullOrEmpty(this.DataSourceType))
            {
                this.DataSourceType = "KeyValue";
            }
            if (!string.IsNullOrEmpty(this.KeyValueDataTextField))
            {
                this.DropDownList.DataTextField = this.KeyValueDataTextField;
            }
            if (!string.IsNullOrEmpty(this.KeyValueDataValueField))
            {
                this.DropDownList.DataValueField = this.KeyValueDataValueField;
            }
            if (this.DataSourceType == "KeyValue" && string.IsNullOrEmpty(this.KeyValueDefaultColumn))
            {
                this.KeyValueDefaultColumn = "KeyValueIntCode";
            }

            this.btnKeyValue.Visible = this.ShowButton;
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            list = LoadList();
            if (!string.IsNullOrEmpty(this.WhereKeyValueIntCodesEQ))
            {
                list = ReduceListBySelectedValues(list, this.WhereKeyValueIntCodesEQ, true);
            }
            if (!string.IsNullOrEmpty(this.WhereKeyValueIntCodesNotEQ))
            {
                list = ReduceListBySelectedValues(list, this.WhereKeyValueIntCodesNotEQ, false);
            }
            this.DropDownList.DataSource = list;
            this.DropDownList.DataBind();

            if (this.SetItemTextAsTooltip)
            {
                foreach (ListItem item in this.DropDownList.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }

                this.DropDownList.Attributes.Add("onmouseover", "this.title=this.options[this.selectedIndex].title");
            }

            if (!string.IsNullOrEmpty(DefaultValueByName))
            {
                var defaultValue = list.FirstOrDefault(s => s.Name == DefaultValueByName);
                if (defaultValue != null)
                {
                    if (string.IsNullOrEmpty(this.KeyValueDataValueField))
                    {
                        this.DropDownList.SelectedValue = defaultValue.idKeyValue.ToString();
                    }
                    else
                    {
                        this.DropDownList.SelectedValue = defaultValue.GetType().GetProperty(this.KeyValueDataValueField).GetValue(defaultValue, null).ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.KeyValueDefaultColumn) && !string.IsNullOrEmpty(this.KeyValueDefault))
            {
                var defaultValue = list.AsQueryable<KeyValue>().Where(BaseHelper.BuildPredicate<KeyValue>(this.KeyValueDefaultColumn, this.KeyValueDefault)).FirstOrDefault();
                if (defaultValue != null)
                {
                    if (string.IsNullOrEmpty(this.KeyValueDataValueField))
                    {
                        this.DropDownList.SelectedValue = defaultValue.idKeyValue.ToString();
                    }
                    else
                    {
                        this.DropDownList.SelectedValue = defaultValue.GetType().GetProperty(this.KeyValueDataValueField).GetValue(defaultValue, null).ToString();
                    }
                }
            }
        }

        private List<KeyValue> ReduceListBySelectedValues(List<KeyValue> allValues, string whereKeyValueIntCodes, bool isInclude)
        {
            List<KeyValue> selectedValues = new List<KeyValue>();
            string[] keyValueIntCodes = whereKeyValueIntCodes.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < allValues.Count; i++)
            {
                if (isInclude)
                {
                    if (keyValueIntCodes.Any(k => k == allValues[i].KeyValueIntCode) || allValues[i].idKeyValue == Constants.INVALID_ID)
                    {
                        selectedValues.Add(allValues[i]);
                    }
                }
                else
                {
                    if (!keyValueIntCodes.Any(k => k == allValues[i].KeyValueIntCode))
                    {
                        selectedValues.Add(allValues[i]);
                    }
                }
            }

            //            AddDefaultValue(selectedValues);

            return selectedValues;
        }

        private List<KeyValue> LoadList()
        {
            if (!externalDataSourceType)
            {
                //return new List<KeyValue>();
            }

            if (DataSourceType == "KeyValue")
            {
                list = this.ownerPage.GetAllKeyValueByKeyTypeIntCode(KeyTypeIntCode).OrderBy(e => e.Name).ToList();
                if (this.AddingDefaultValue)
                {
                    AddDefaultValue(list, this.UseShortDefaultValue);
                }
                if (OrderBy != null)
                {
                    list = BaseClassBL<KeyValue>.Sort(list, this.OrderBy, this.OrderDirection != null ? this.OrderDirection : "asc").ToList();
                }
            }
            else if (DataSourceType == "Role")
            {


                list = (from e in this.AdminClientRef.GetAllRoles("Name", this.GridViewSortDirection)
                        select new KeyValue
                        {
                            idKeyValue = e.idRole,
                            Name = e.Name
                        }).ToList<KeyValue>();

                AddDefaultValue(list);
            }
            //else if (DataSourceType == "UNI")
            //{
            //    CallContext resultContext = new CallContext();

            //    resultContext.CurrentYear = this.CurrentYear;
            //    resultContext.CurrentPeriod = this.CurrentPeriod;

            //    UNI currentUni = this.AdminClientRef.GetCurrentUNI(resultContext);
            //    KeyValue kvUni = new KeyValue
            //    {
            //        idKeyType = currentUni.idUNI,
            //        Name=currentUni.UniName
            //    };

            //     list = new List<KeyValue>();
            //     list.Add(kvUni);

            //     AddDefaultValue(list);
            //}
            else if (DataSourceType == "VendorDiePriceList")
            {
                DateTime? dateActiveTo = null;
                if (!string.IsNullOrWhiteSpace(this.AdditionalParam))
                {
                    dateActiveTo = Convert.ToDateTime(this.AdditionalParam, BaseHelper.GetDateTimeFormatInfo());
                }

                var data = this.CostCalculationRef.GetAllDiePriceList(new List<AbstractSearch>(), dateActiveTo, string.Empty, string.Empty);

                list = (from d in data
                        select new KeyValue
                        {
                            idKeyValue = d.idDiePriceList,
                            Name = d.VendorName + " / from " + d.DateFromString + (d.DateTo.HasValue ? " - to " + d.DateToString : string.Empty)
                        }).ToList();

                AddDefaultValue(list);
            }
            
            else if (DataSourceType == "Country")
            {
                var data = this.AdminClientRef.GetAllCountries();

                //премахваме от списъка с държави текста "без гражданство" AdditionalParam="Number <> 0"
                if (!string.IsNullOrEmpty(this.AdditionalParam) && this.AdditionalParam == "WithoutEmptyCountry")
                {
                    list = (from c in data
                            where c.CodeISO3166 == ETEMEnums.CountryCodeEnum.BG.ToString()
                            select new KeyValue
                            {
                                idKeyValue = c.idCountry,
                                Name = c.Name

                            }
                            ).Union(from c in data
                                    where c.CodeISO3166 != ETEMEnums.CountryCodeEnum.NONE.ToString() && c.CodeISO3166 != ETEMEnums.CountryCodeEnum.BG.ToString()
                                    select new KeyValue
                                    {
                                        idKeyValue = c.idCountry,
                                        Name = c.Name

                                    }
                            ).ToList<KeyValue>();
                }

                else
                {
                    list = (from c in data
                            where c.CodeISO3166 == ETEMEnums.CountryCodeEnum.NONE.ToString()
                            select new KeyValue
                            {
                                idKeyValue = c.idCountry,
                                Name = c.Name

                            }).Union(from c in data
                                     where c.CodeISO3166 == ETEMEnums.CountryCodeEnum.BG.ToString()
                                     select new KeyValue
                                     {
                                         idKeyValue = c.idCountry,
                                         Name = c.Name

                                     }
                            ).Union(from c in data
                                    where c.CodeISO3166 != ETEMEnums.CountryCodeEnum.NONE.ToString() && c.CodeISO3166 != ETEMEnums.CountryCodeEnum.BG.ToString()
                                    select new KeyValue
                                    {
                                        idKeyValue = c.idCountry,
                                        Name = c.Name

                                    }
                            ).ToList<KeyValue>();
                }

                AddDefaultValue(list);
            }
            else if (DataSourceType == "District")
            {
                if (string.IsNullOrEmpty(this.AdditionalParam))
                {
                    list = (from d in this.AdminClientRef.GetAllDistricts()
                            select new KeyValue
                            {
                                idKeyValue = d.idDistrict,
                                Name = d.Name

                            }).ToList<KeyValue>();
                }
                else
                {
                    list = (from d in this.AdminClientRef.GetDistrictsByCountryId(this.AdditionalParam)
                            select new KeyValue
                            {
                                idKeyValue = d.idDistrict,
                                Name = d.Name

                            }).ToList<KeyValue>();
                }

                AddDefaultValue(list);
            }
            else if (DataSourceType == "Municipality")
            {
                if (string.IsNullOrEmpty(this.AdditionalParam))
                {
                    list = (from m in this.AdminClientRef.GetAllMunicipalities()
                            select new KeyValue
                            {
                                idKeyValue = m.idMunicipality,
                                Name = m.Name

                            }).ToList<KeyValue>();


                }
                else
                {
                    string idFilter = Constants.INVALID_ID_STRING;
                    if (this.AdditionalParam.Contains("Country"))
                    {
                        if (this.AdditionalParam.Contains("="))
                        {
                            idFilter = this.AdditionalParam.Split('=')[1];
                        }
                        else
                        {
                            idFilter = this.AdditionalParam;
                        }

                        list = (from m in this.AdminClientRef.GetMunicipalitiesByCountryId(idFilter)
                                select new KeyValue
                                {
                                    idKeyValue = m.idMunicipality,
                                    Name = m.Name

                                }).ToList<KeyValue>();
                    }
                    else if (this.AdditionalParam.Contains("District"))
                    {
                        if (this.AdditionalParam.Contains("="))
                        {
                            idFilter = this.AdditionalParam.Split('=')[1];
                        }
                        else
                        {
                            idFilter = this.AdditionalParam;
                        }

                        list = (from m in this.AdminClientRef.GetMunicipalitiesByDistrictId(idFilter)
                                select new KeyValue
                                {
                                    idKeyValue = m.idMunicipality,
                                    Name = m.Name

                                }).ToList<KeyValue>();
                    }
                    else
                    {
                        list = new List<KeyValue>();
                    }
                }

                AddDefaultValue(list);
            }
            else if (DataSourceType == "EKATTE")
            {
                if (string.IsNullOrEmpty(this.AdditionalParam))
                {
                    list = (from e in this.AdminClientRef.GetAllEKATTEs()
                            select new KeyValue
                            {
                                idKeyValue = e.idEKATTE,
                                Name = e.Name

                            }).ToList<KeyValue>();
                }
                else
                {
                    list = (from e in this.AdminClientRef.GetEKATTEsByMunicipalityId(this.AdditionalParam)
                            select new KeyValue
                            {
                                idKeyValue = e.idEKATTE,
                                Name = e.Name

                            }).ToList<KeyValue>();
                }

                AddDefaultValue(list);
            }

            else if (DataSourceType == "Location")
            {
                if (string.IsNullOrEmpty(this.AdditionalParam))
                {
                    list = (from l in this.AdminClientRef.GetAllLocations()
                            select new KeyValue
                            {
                                idKeyValue = l.idLocation,
                                Name = l.Name

                            }).ToList<KeyValue>();
                }
                else
                {
                    list = (from l in this.AdminClientRef.GetLocationsByMunicipalityId(this.AdditionalParam)
                            select new KeyValue
                            {
                                idKeyValue = l.idLocation,
                                Name = l.Name

                            }).ToList<KeyValue>();
                }

                AddDefaultValue(list);
            }
            else if (DataSourceType == "LocationView")
            {
                if (string.IsNullOrEmpty(this.AdditionalParam))
                {
                    list = (from lv in this.AdminClientRef.GetAllLocationViews()
                            select new KeyValue
                            {
                                idKeyValue = lv.idLocation,
                                Name = lv.LocationTypeName

                            }).ToList<KeyValue>();
                }
                else
                {
                    list = (from lv in this.AdminClientRef.GetLocationViewsByMunicipalityId(this.AdditionalParam)
                            select new KeyValue
                            {
                                idKeyValue = lv.idLocation,
                                Name = lv.LocationTypeName

                            }).ToList<KeyValue>();
                }

                AddDefaultValue(list);
            }



            else if (DataSourceType == "WeekOfMonth")
            {
                list = new List<KeyValue>();

                int reportYear = Int32.Parse(CustomFieldUseEverything.Split('|')[0]);
                int reportMonth = Int32.Parse(CustomFieldUseEverything.Split('|')[1]);

                int weeksCount = BaseHelper.Weeks(reportYear, reportMonth);

                for (int i = 0; i < weeksCount; i++)
                {
                    int week = i + 1;

                    if (string.IsNullOrEmpty(this.WeekNotEQ) || !this.WeekNotEQ.Contains(i.ToString()))
                    {
                        list.Add(new KeyValue()
                        {
                            Name = "Седмица " + week,
                            idKeyValue = i
                        }
                        );
                    }


                }
            }
            else if (DataSourceType == "Group")
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                if (!string.IsNullOrEmpty(AdditionalParam))
                {
                    searchCriteria.Add(
                    new BooleanSearch
                    {
                        Comparator = BooleanComparators.Equal,
                        Property = "SharedAccess",
                        SearchTerm = Convert.ToBoolean("true")
                    });
                }


                list = (from d in this.AdminClientRef.GetGroupDataView(searchCriteria, "", "")
                        select new KeyValue
                        {
                            idKeyValue = d.idGroup,
                            Name = d.GroupName

                        }).DistinctBy(o => o.idKeyValue).ToList<KeyValue>();

                AddDefaultValue(list);
            }

            else if (DataSourceType == "Modules")
            {
                list = (from e in this.AdminClientRef.GetAllModule(new List<AbstractSearch>(), "ModuleName", this.GridViewSortDirection)
                        select new KeyValue
                        {
                            idKeyValue = e.idModule,
                            Name = e.ModuleName

                        }).ToList<KeyValue>();

                AddDefaultValue(list);
            }
            else if (DataSourceType == "PRESS")
            { 
                list = this.ownerPage.GetAllKeyValueByKeyTypeIntCode(KeyTypeIntCode).Where(s=>s.DefaultValue1=="Press").OrderBy(e => e.Name).ToList();

                if (this.AddingDefaultValue)
                {
                    AddDefaultValue(list, this.UseShortDefaultValue);
                }
                if (OrderBy != null)
                {
                    list = BaseClassBL<KeyValue>.Sort(list, this.OrderBy, this.OrderDirection != null ? this.OrderDirection : "asc").ToList();
                }
            }
            else if (DataSourceType == "ProfileSetting")
            {
                var data = this.ownerPage.CostCalculationRef.GetProfilesList(new List<AbstractSearch>(), "", "");

                list = (from ps in data
                        select new KeyValue
                        {
                            idKeyValue = ps.idProfileSetting,
                            Name = ps.ProfileName

                        }).ToList<KeyValue>();

                AddDefaultValue(list);
            }
            else
            {
                list = new List<KeyValue>();
            }

            return list;
        }

        private static void AddDefaultValue(List<KeyValue> list)
        {
            AddDefaultValue(list, false);
        }

        public static void AddDefaultValue(List<KeyValue> list, bool useShortDefaultValue)
        {
            if (list.FirstOrDefault(s => s.idKeyValue == ETEMModel.Helpers.Constants.INVALID_ID) == null)
            {
                list.Insert(0, new KeyValue()
                {
                    idKeyValue = ETEMModel.Helpers.Constants.INVALID_ID,
                    Name = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue1 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue2 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue3 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue4 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue5 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    DefaultValue6 = (useShortDefaultValue ? Constants.NOT_SELECTED_LIST_VALUE_SHORT : "Please select"),
                    KeyValueIntCode = Constants.INVALID_ID_STRING
                });
            }
        }

        public void SetDefaultValue(string defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(defaultValue))
            {
                if (this.DropDownListCTRL != null && this.DropDownListCTRL.Items != null)
                {
                    var listItemSearched = this.DropDownListCTRL.Items.FindByValue(defaultValue);

                    if (listItemSearched != null)
                    {
                        this.DropDownListCTRL.SelectedValue = defaultValue;
                    }
                }
            }
        }

        public DropDownList DropDownListCTRL
        {
            get { return this.DropDownList; }
        }

        /// <summary>
        /// Set selected value by text,if exist in the ddl select options
        /// </summary>
        public string SelectedValueText
        {
            get { return this.DropDownList.SelectedItem != null ? this.DropDownList.SelectedItem.Text : ""; }
            set
            {
                var item = this.DropDownList.Items.FindByText(value);

                if (item != null)
                {
                    this.DropDownList.SelectedValue = item.Value;
                }
            }

        }



        /// <summary>
        /// Set selected value by id,if exist in the ddl select options
        /// </summary>
        public string SelectedValue
        {
            get { return this.DropDownList.SelectedValue; }
            set
            {
                var item = this.DropDownList.Items.FindByValue(value);

                if (item != null)
                {
                    this.DropDownList.SelectedValue = item.Value;
                    this.DropDownList.ToolTip = item.Text;

                }
            }
        }

        public int SelectedValueINT
        {
            get
            {
                int result;
                bool isParsable = int.TryParse(this.DropDownList.SelectedValue, out result);
                if (isParsable)
                {
                    return result;
                }
                else
                {
                    return Constants.INVALID_ID;
                }
            }
        }

        public int? SelectedValueNullINT
        {
            get
            {
                int result;
                bool isParsable = int.TryParse(this.DropDownList.SelectedValue, out result);
                if (isParsable &&
                    this.DropDownList.SelectedValue != Constants.INVALID_ID_STRING && this.DropDownList.SelectedValue != Constants.INVALID_ID_ZERO_STRING)
                {
                    return result;
                }
                else
                {
                    return new Nullable<int>();
                }
            }
        }

        public bool AutoPostBack
        {
            get { return this.DropDownList.AutoPostBack; }
            set { this.DropDownList.AutoPostBack = value; }
        }



        public string CssClassDropDown
        {
            get { return this.DropDownList.CssClass; }
            set { this.DropDownList.CssClass = value; }
        }

        public string CssClassButton
        {
            get { return this.btnKeyValue.CssClass; }
            set { this.btnKeyValue.CssClass = value; }
        }

        public bool DropDownEnabled
        {
            get { return this.DropDownList.Enabled; }
            set { this.DropDownList.Enabled = value; }
        }

        protected void btnKeyValue_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.KeyTypeIntCode))
            {
                this.KeyType.CurrentEntityMasterID = this.ownerPage.AdminClientRef.GetKeyTypeByIntCode(this.KeyTypeIntCode).idKeyType.ToString();
                this.KeyType.currentCallerDropDownList = this;
                this.KeyType.UserControlLoad();
                this.KeyType.Visible = true;
            }

        }

        public int SelectedIndex
        {
            get { return this.DropDownList.SelectedIndex; }
            set { this.DropDownList.SelectedIndex = value; }
        }

        public Unit Width
        {
            get { return this.DropDownList.Width; }
            set { this.DropDownList.Width = value; }
        }

    }
}
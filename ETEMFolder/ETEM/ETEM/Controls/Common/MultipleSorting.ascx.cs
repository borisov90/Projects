using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers.Common;
using ETEMModel.Helpers;

namespace ETEM.Controls.Common
{
    public partial class MultipleSorting : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void UserControlLoad(GridView gridView)
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            List<MultipleSortingClass> listColumns = new List<MultipleSortingClass>();

            if (this.gvGridColumns.Rows.Count == 0 && gridView != null)
            {
                if (gridView.HeaderRow != null && gridView.HeaderRow.Cells.Count > 0)
                {
                    int countSortingColumns = 0;
                    for (int i = 0; i < gridView.HeaderRow.Cells.Count; i++)
                    {
                        if (gridView.HeaderRow.Cells[i].Controls.Count > 0 && gridView.HeaderRow.Cells[i].Controls[0] is LinkButton &&
                            (gridView.HeaderRow.Cells[i].Controls[0] as LinkButton).CommandName == "Sort")
                        {
                            countSortingColumns++;

                            LinkButton lnkColumn = gridView.HeaderRow.Cells[i].Controls[0] as LinkButton;
                            listColumns.Add(new MultipleSortingClass()
                                            {
                                                ColumnName = lnkColumn.Text,
                                                ColumnCode = lnkColumn.CommandArgument,
                                                ListSortDirections = new List<SortDirectionClass>()
                                                {
                                                    new SortDirectionClass()
                                                    {
                                                        SortDirectionName = BaseHelper.GetCaptionString("SortDirectionAsc"),
                                                        SortDirectionCode = Constants.SORTING_ASC
                                                    },
                                                    new SortDirectionClass()
                                                    {
                                                        SortDirectionName = BaseHelper.GetCaptionString("SortDirectionDesc"),
                                                        SortDirectionCode = Constants.SORTING_DESC
                                                    }
                                                }
                                            }
                            );
                        }
                    }

                    List<ItemClass> listSequenceNumbers = new List<ItemClass>();
                    listSequenceNumbers.Add(new ItemClass()
                                                {
                                                    Text = Constants.NOT_SELECTED_LIST_VALUE_SHORT,
                                                    Value = Constants.INVALID_ID_STRING
                                                }
                                            );
                    for (int i = 0; i < countSortingColumns; i++)
                    {
                        listSequenceNumbers.Add(new ItemClass()
                                                    {
                                                        Text = (i + 1).ToString(),
                                                        Value = (i + 1).ToString()
                                                    }
                                                );
                    }

                    foreach (MultipleSortingClass multipleSortingClass in listColumns)
                    {
                        multipleSortingClass.ListItems = listSequenceNumbers;
                    }
                }

                this.gvGridColumns.DataSource = listColumns;
                this.gvGridColumns.DataBind();
            }

            this.pnlSorting.Visible = true;
        }

        protected void btnSort_Click(object sender, EventArgs e)
        {
            string sortExpression = string.Empty;

            string sortExprSingle = string.Empty;

            List<KeyValuePair<string, int>> listColumnsBySequenceNumber = new List<KeyValuePair<string, int>>();

            CheckBox chbxSelect = new CheckBox();
            HiddenField hdnColumnCode = new HiddenField();
            DropDownList ddlSortDirection = new DropDownList();
            DropDownList ddlSequenceNumber = new DropDownList();
            foreach (GridViewRow row in this.gvGridColumns.Rows)
            {
                chbxSelect = row.FindControl("chbxSelect") as CheckBox;
                if (chbxSelect != null && chbxSelect.Checked)
                {
                    hdnColumnCode = row.FindControl("hdnColumnCode") as HiddenField;
                    if (hdnColumnCode != null)
                    {
                        sortExprSingle = hdnColumnCode.Value;

                        ddlSortDirection = row.FindControl("ddlSortDirection") as DropDownList;
                        if (ddlSortDirection != null)
                        {
                            sortExprSingle += " " + ddlSortDirection.SelectedValue;
                        }
                        ddlSequenceNumber = row.FindControl("ddlSequenceNumber") as DropDownList;
                        if (ddlSequenceNumber != null &&
                            listColumnsBySequenceNumber.Where(w => w.Key == sortExprSingle).Count() == 0)
                        {
                            listColumnsBySequenceNumber.Add(new KeyValuePair<string, int>(sortExprSingle,
                                                                                          Convert.ToInt32(ddlSequenceNumber.SelectedValue)));
                        }
                    }
                }
            }

            string[] arrSortExpressions = listColumnsBySequenceNumber.Where(w => !string.IsNullOrEmpty(w.Key)).
                                                                      OrderBy(s => s.Value).Select(s => s.Key).ToArray();

            sortExpression = string.Join(", ", arrSortExpressions);

            this.ownerPage.MultipleSortingClick(sortExpression);

            this.pnlSorting.Visible = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearSortingForm();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlSorting.Visible = false;
        }

        private void ClearSortingForm()
        {
            CheckBox chbxSelect = new CheckBox();
            DropDownList ddlSortDirection = new DropDownList();
            DropDownList ddlSequenceNumber = new DropDownList();
            foreach (GridViewRow row in this.gvGridColumns.Rows)
            {
                chbxSelect = row.FindControl("chbxSelect") as CheckBox;
                if (chbxSelect != null)
                {
                    chbxSelect.Checked = false;
                }
                ddlSortDirection = row.FindControl("ddlSortDirection") as DropDownList;
                if (ddlSortDirection != null)
                {
                    ddlSortDirection.SelectedIndex = 0;
                }
                ddlSequenceNumber = row.FindControl("ddlSequenceNumber") as DropDownList;
                if (ddlSequenceNumber != null)
                {
                    ddlSequenceNumber.SelectedIndex = 0;
                }
            }
        }

        protected void chbxSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox currChbxSelect = sender as CheckBox;

            if (currChbxSelect == null)
            {
                return;
            }

            int maxNumber = 0;
            CheckBox chbxSelect = new CheckBox();
            DropDownList ddlSequenceNumber = new DropDownList();
            foreach (GridViewRow row in this.gvGridColumns.Rows)
            {
                chbxSelect = row.FindControl("chbxSelect") as CheckBox;
                if (chbxSelect != null && chbxSelect.Checked)
                {
                    maxNumber++;
                }
            }

            string currRowIndexString = currChbxSelect.ClientID.Substring(currChbxSelect.ClientID.Length - 1);

            int currRowIndex = Convert.ToInt32(currRowIndexString);

            DropDownList currDdlSequenceNumber = this.gvGridColumns.Rows[currRowIndex].FindControl("ddlSequenceNumber") as DropDownList;
            if (currDdlSequenceNumber != null)
            {
                if (currChbxSelect.Checked)
                {
                    currDdlSequenceNumber.SelectedValue = (maxNumber).ToString();
                }
                else
                {
                    foreach (GridViewRow row in this.gvGridColumns.Rows)
                    {
                        ddlSequenceNumber = row.FindControl("ddlSequenceNumber") as DropDownList;
                        if (ddlSequenceNumber != null && currDdlSequenceNumber.SelectedValue != Constants.INVALID_ID_STRING &&
                            Convert.ToInt32(ddlSequenceNumber.SelectedValue) > Convert.ToInt32(currDdlSequenceNumber.SelectedValue))
                        {
                            ddlSequenceNumber.SelectedValue = (Convert.ToInt32(ddlSequenceNumber.SelectedValue) - 1).ToString();
                        }
                    }

                    currDdlSequenceNumber.SelectedIndex = 0;
                }
            }
        }

        protected void ddlSequenceNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList currDdlSequenceNumber = sender as DropDownList;

            if (currDdlSequenceNumber == null)
            {
                return;
            }

            string currRowIndexString = currDdlSequenceNumber.ClientID.Substring(currDdlSequenceNumber.ClientID.Length - 1);

            int currRowIndex = Convert.ToInt32(currRowIndexString);

            CheckBox currChbxSelect = this.gvGridColumns.Rows[currRowIndex].FindControl("chbxSelect") as CheckBox;
            if (currDdlSequenceNumber == null || !currChbxSelect.Checked)
            {
                return;
            }

            int currNumber = Convert.ToInt32(currDdlSequenceNumber.SelectedValue);

            Dictionary<string, int> dictSequenceNumberByDdlClientID = new Dictionary<string, int>();

            CheckBox chbxSelect = new CheckBox();
            DropDownList ddlSequenceNumber = new DropDownList();
            foreach (GridViewRow row in this.gvGridColumns.Rows)
            {
                chbxSelect = row.FindControl("chbxSelect") as CheckBox;
                if (chbxSelect != null && chbxSelect.Checked)
                {
                    ddlSequenceNumber = row.FindControl("ddlSequenceNumber") as DropDownList;

                    if (ddlSequenceNumber != null && ddlSequenceNumber.SelectedValue != Constants.INVALID_ID_STRING)
                    {
                        if (currDdlSequenceNumber.ClientID != ddlSequenceNumber.ClientID)
                        {
                            dictSequenceNumberByDdlClientID.Add(ddlSequenceNumber.ClientID, Convert.ToInt32(ddlSequenceNumber.SelectedValue));
                        }
                    }
                }
            }

            dictSequenceNumberByDdlClientID = dictSequenceNumberByDdlClientID.OrderBy(o => o.Value).ToDictionary(x => x.Key, y => y.Value);

            for (int i = 0; i < dictSequenceNumberByDdlClientID.Count; i++)
            {
                if (currNumber != Constants.INVALID_ID)
                {
                    dictSequenceNumberByDdlClientID[dictSequenceNumberByDdlClientID.Keys.ElementAt(i)] = (currNumber == (i + 1) ? (i + 2) : (i + 1));
                }
            }

            foreach (GridViewRow row in this.gvGridColumns.Rows)
            {
                chbxSelect = row.FindControl("chbxSelect") as CheckBox;
                if (chbxSelect != null && chbxSelect.Checked)
                {
                    ddlSequenceNumber = row.FindControl("ddlSequenceNumber") as DropDownList;

                    if (ddlSequenceNumber != null && ddlSequenceNumber.SelectedValue != Constants.INVALID_ID_STRING)
                    {
                        if (currDdlSequenceNumber.ClientID != ddlSequenceNumber.ClientID)
                        {
                            ddlSequenceNumber.SelectedValue = dictSequenceNumberByDdlClientID[ddlSequenceNumber.ClientID].ToString();
                        }
                    }
                }
            }
        }
    }
}
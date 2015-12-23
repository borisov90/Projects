using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Common
{
    public class MultipleSortingClass
    {
        public string ColumnName { get; set; }
        public string ColumnCode { get; set; }
        public List<SortDirectionClass> ListSortDirections { get; set; }
        public List<ItemClass> ListItems { get; set; }

        public MultipleSortingClass()
        {
            this.ListSortDirections = new List<SortDirectionClass>();
            this.ListItems = new List<ItemClass>();
        }
    }

    public class ItemClass
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
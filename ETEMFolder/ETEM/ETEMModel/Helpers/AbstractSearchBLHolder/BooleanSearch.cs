using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace ETEMModel.Helpers.AbstractSearchBLHolder
{
    public class BooleanSearch : AbstractSearch
    {
        public bool? SearchTerm { get; set; }

        public BooleanComparators Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {
            if (!this.SearchTerm.HasValue)
            {
                return null;
            }

            Expression searchExpression = this.GetFilterExpression(property);

            return searchExpression;
        }

        private Expression GetFilterExpression(MemberExpression property)
        {
            switch (this.Comparator)
            {
                case BooleanComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm.Value));
                case BooleanComparators.NotEqual:
                    return Expression.NotEqual(property, Expression.Constant(this.SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum BooleanComparators
    {
        [Display(Name = "==")]
        Equal,

        [Display(Name = "!=")]
        NotEqual
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace ETEMModel.Helpers.AbstractSearchBLHolder
{
    public class DecimalSearch : AbstractSearch
    {
        public object SearchTerm { get; set; }

        public DecimalComparators Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {
            if (this.SearchTerm == null)
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
                case DecimalComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(this.SearchTerm));
                case DecimalComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm));
                case DecimalComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm));
                case DecimalComparators.NotEqual:
                    return Expression.NotEqual(property, Expression.Constant(this.SearchTerm));
                case DecimalComparators.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm));
                case DecimalComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum DecimalComparators
    {
        [Display(Name = "<")]
        Less,

        [Display(Name = "<=")]
        LessOrEqual,

        [Display(Name = "==")]
        Equal,

        [Display(Name = "!=")]
        NotEqual,

        [Display(Name = ">=")]
        GreaterOrEqual,

        [Display(Name = ">")]
        Greater
    }
}
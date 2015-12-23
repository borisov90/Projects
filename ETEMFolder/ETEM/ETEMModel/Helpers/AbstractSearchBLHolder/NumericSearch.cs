using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ETEMModel.Helpers.AbstractSearchBLHolder
{
    public class NumericSearch : AbstractSearch
    {
        public object SearchTerm { get; set; }

        public NumericComparators Comparator { get; set; }

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
                case NumericComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(this.SearchTerm));
                case NumericComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm));
                case NumericComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm));
                case NumericComparators.NotEqual:
                    return Expression.NotEqual(property, Expression.Constant(this.SearchTerm));
                case NumericComparators.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm));
                case NumericComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum NumericComparators
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
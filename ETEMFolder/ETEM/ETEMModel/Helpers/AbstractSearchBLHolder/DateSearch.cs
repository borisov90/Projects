using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ETEMModel.Helpers.AbstractSearchBLHolder
{
    public class DateSearch : AbstractSearch
    {
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SearchTerm { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SecondSearchTerm { get; set; }

        public DateComparators Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {
            Expression firstSearchExpression = null;
            Expression secondSearchExpression = null;

            if (this.SearchTerm.HasValue)
            {
                firstSearchExpression = this.GetFilterExpression(property);
            }

            if (this.Comparator == DateComparators.InRange && this.SecondSearchTerm.HasValue)
            {
                secondSearchExpression = Expression.LessThanOrEqual(property, Expression.Constant(this.SecondSearchTerm.Value));
            }

            if (firstSearchExpression == null && secondSearchExpression == null)
            {
                return null;
            }
            else if (firstSearchExpression != null && secondSearchExpression != null)
            {
                var combinedExpression = Expression.AndAlso(firstSearchExpression, secondSearchExpression);
                return combinedExpression;
            }
            else if (firstSearchExpression != null)
            {
                return firstSearchExpression;
            }
            else
            {
                return secondSearchExpression;
            }
        }

        private Expression GetFilterExpression(MemberExpression property)
        {
            switch (this.Comparator)
            {
                case DateComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.GreaterOrEqual:
                case DateComparators.InRange:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum DateComparators
    {
        [Display(Name = "<")]
        Less,

        [Display(Name = "<=")]
        LessOrEqual,

        [Display(Name = "==")]
        Equal,

        [Display(Name = ">=")]
        GreaterOrEqual,

        [Display(Name = ">")]
        Greater,

        [Display(Name = "Range")]
        InRange
    }
}

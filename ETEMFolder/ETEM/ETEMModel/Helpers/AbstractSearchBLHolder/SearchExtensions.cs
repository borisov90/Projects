using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ETEMModel.Helpers.AbstractSearchBLHolder
{
    public static class SearchExtensions
    {
        public static IQueryable<T> ApplySearchCriterias<T>(this IQueryable<T> query, IEnumerable<AbstractSearch> searchCriterias)
        {
            foreach (var criteria in searchCriterias)
            {
                query = criteria.ApplyToQuery(query);
            }

            var result = query.ToArray();

            return query;
        }

        public static ICollection<AbstractSearch> GetDefaultSearchCriterias(this Type type)
        {
            var properties = type.GetProperties()
                                    .Where(p => p.CanRead && p.CanWrite)
                                    .OrderBy(p => p.Name);

            var searchCriterias = properties
                                    .Select(p => CreateSearchCriteria(p.PropertyType, p.Name))
                                    .Where(s => s != null)
                                    .ToList();

            return searchCriterias;
        }

        public static ICollection<AbstractSearch> AddCustomSearchCriteria<T>(this ICollection<AbstractSearch> searchCriterias, Expression<Func<T, object>> property)
        {
            Type propertyType = null;
            string fullPropertyPath = GetPropertyPath(property, out propertyType);

            AbstractSearch searchCriteria = CreateSearchCriteria(propertyType, fullPropertyPath);

            if (searchCriteria != null)
            {
                searchCriterias.Add(searchCriteria);
            }

            return searchCriterias;
        }

        private static AbstractSearch CreateSearchCriteria(Type type, string property)
        {
            AbstractSearch result = null;

            if (type.Equals(typeof(string)))
            {
                result = new TextSearch();
            }
            else if (type.Equals(typeof(int)) || type.Equals(typeof(int?)))
            {
                result = new NumericSearch();
            }
            else if (type.Equals(typeof(decimal)) || type.Equals(typeof(decimal?)) ||
                     type.Equals(typeof(double)) || type.Equals(typeof(double?)))
            {
                result = new DecimalSearch();
            }
            else if (type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?)))
            {
                result = new DateSearch();
            }
            else if (type.Equals(typeof(bool)) || type.Equals(typeof(bool?)))
            {
                result = new BooleanSearch();
            }

            if (result != null)
            {
                result.Property = property;
            }

            return result;
        }

        private static string GetPropertyPath<T>(Expression<Func<T, object>> expression, out Type targetType)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Please provide a lambda expression like 'n => n.PropertyName'", "expression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            targetType = propertyInfo.PropertyType;

            string property = memberExpression.ToString();

            return property.Substring(property.IndexOf('.') + 1);
        }
    }
}

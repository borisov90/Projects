using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public static class CustomLinq
    {
        public static IEnumerable<T> FindTriadByMiddle<T>(this IEnumerable<T> items, Predicate<T> matchFilling)
        {
            if (items == null)
                throw new ArgumentNullException("items cannot be null");
            if (matchFilling == null)
                throw new ArgumentNullException("matchFilling cannot be null");

            return FindTriadByMiddleImpl(items, matchFilling);
        }

        private static IEnumerable<T> FindTriadByMiddleImpl<T>(IEnumerable<T> items, Predicate<T> matchFilling)
        {
            using (var iter = items.GetEnumerator())
            {
                T previous = default(T);
                while (iter.MoveNext())
                {
                    if (matchFilling(iter.Current))
                    {
                        yield return previous;
                        yield return iter.Current;
                        if (iter.MoveNext())
                            yield return iter.Current;
                        else
                            yield return default(T);
                        yield break;
                    }
                    previous = iter.Current;
                }
            }
            // If we get here nothing has been found so return three default values
            yield return default(T); // Previous
            yield return default(T); // Current
            yield return default(T); // Next
        }


    }

    public static class GenericCustomLinq<T>
    {
        private static T FindPreviousElement(List<T> items, Func<T, bool> matchFilling)
        {

            items.Reverse();
            var previousItem = items.SkipWhile(matchFilling).Skip(1).FirstOrDefault();
            return previousItem;

        }


        //public static T? GetPrev(IEnumerable<T> list, Func<Tuple<T,T>,bool> predicate)
        //{
        //    var m = Enumerable.Range(1, list.Count()- 1)
        //          .Select(i => new Tuple<T,T>(list.ElementAt(i), list.ElementAt(i - 1)) ).FirstOrDefault(predicate);
        //    if (m != null)
        //    {
        //        return m.Item2;
        //    }
        //    else
        //    {
        //        return null;
        //    }


        //}

        /// <summary>
        /// returns the previos element if there is one, if not returns the one that we have passed
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentItem"></param>
        /// <returns>Tuple with the previous element and true value or the same element and false value</returns>
        public static Tuple<T, bool> GetPreviousIfHasOrSameIfNot(IEnumerable<T> list, T currentItem)
        {
            T previusElement = default(T);

            bool currentItemFound = false;

            for (int i = 0; i < list.Count(); i++)
            {
                if (Compare<T>(list.ElementAt(i), currentItem) == true)
                {
                    currentItemFound = true;
                }
            }
            if (currentItemFound == false)
            {
                throw new ArgumentException("The element don't match any of the items in the list");
            }

            for (int i = 0; i < list.Count(); i++)
            {

                if (i == 0 && Compare<T>(list.ElementAt(i), currentItem) == true)
                {
                    return new Tuple<T, bool>(currentItem, false);
                }
                else if (i > 0)
                {
                    previusElement = list.ElementAt(i - 1);
                    if (Compare<T>(list.ElementAt(i), currentItem) == true)
                    {
                        return new Tuple<T, bool>(previusElement, true);
                    }
                }
            }

            return new Tuple<T, bool>(currentItem, false);
        }

        public static Tuple<T, bool> GetNextIfHasOrSameIfNot(IEnumerable<T> list, T currentItem)
        {
            T nextElement = default(T);

            bool currentItemFound = false;

            for (int i = 0; i < list.Count(); i++)
            {
                if (Compare<T>(list.ElementAt(i), currentItem) == true)
                {
                    currentItemFound = true;
                }
            }
            if (currentItemFound == false)
            {
                throw new ArgumentException("The element don't match any of the items in the list");
            }

            for (int i = 0; i < list.Count(); i++)
            {

                if (i == list.Count() - 1 && Compare<T>(list.ElementAt(i), currentItem) == true)
                {
                    return new Tuple<T, bool>(currentItem, false);
                }
                else if (i < list.Count() - 1)
                {
                    nextElement = list.ElementAt(i + 1);
                    if (Compare<T>(list.ElementAt(i), currentItem) == true)
                    {
                        return new Tuple<T, bool>(nextElement, true);
                    }
                }
            }

            return new Tuple<T, bool>(currentItem, false);
        }

        static bool Compare<T>(T Object1, T object2)
        {
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (Object1 == null || object2 == null)
                return false;

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                        Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
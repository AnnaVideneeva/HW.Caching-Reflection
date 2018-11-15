using System.Collections;
using System.Linq;
using System.Reflection;

namespace Reflection
{
    /// <summary>
    ///  Represents an equality comparer class.
    /// </summary>
    public class EqualityComparer
    {
        /// <summary>
        /// Determines whether two objects of <typeparamref name="TObject"/> are equal.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lhs">The first object to compare.</param>
        /// <param name="rhs">The second object to compare.</param>
        /// <returns>True if the specified objects are equal; otherwise, false.</returns>
        public static bool Equals<TObject>(TObject lhs, TObject rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(null, lhs) != ReferenceEquals(null, rhs))
            {
                return false;
            }

            if (lhs.GetType() != rhs.GetType())
            {
                return false;
            }

            var properties = lhs.GetType().GetProperties();

            if (properties.Length == 0)
            {
                return lhs.Equals(rhs);
            }

            foreach(var property in properties)
            {
                if (!IsEqualsByProperty(property, lhs, rhs))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsEqualsByProperty<TObject>(PropertyInfo property, TObject lhs, TObject rhs)
        {
            var lhsValue = property.GetValue(lhs);
            var rhsValue = property.GetValue(rhs);
            var type = property.PropertyType;

            if (type.IsPrimitive || typeof(string) == type)
            {
                return EqualsPrimitiveType(lhsValue, rhsValue);
            }


            if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
            {
                return EqualsEnumerableType((IEnumerable)lhsValue, (IEnumerable)rhsValue);
            }

            if (type.IsValueType || type.IsClass)
            {
                return Equals(lhsValue, rhsValue);
            }

            return true;
        }

        private static bool EqualsPrimitiveType(object lhs, object rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(null, lhs) != ReferenceEquals(null, rhs))
            {
                return false;
            }

            if (!lhs.Equals(rhs))
            {
                return false;
            }

            return true;
        }

        private static bool EqualsEnumerableType(IEnumerable lhs, IEnumerable rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(null, lhs) != ReferenceEquals(null, rhs))
            {
                return false;
            }

            var lhsValues = lhs.Cast<object>();
            var rhsValues = rhs.Cast<object>();

            if (lhsValues.Count() != rhsValues.Count())
            {
                return false;
            }

            var lhsValuesEnumerator = lhsValues.GetEnumerator();
            var rhsValuesEnumerator = rhsValues.GetEnumerator();

            var count = lhsValues.Count();

            for (int i = 0; i < count; i++)
            {
                lhsValuesEnumerator.MoveNext();
                rhsValuesEnumerator.MoveNext();

                var lhsValuesCurrent = lhsValuesEnumerator.Current;
                var rhsValuesCurrent = rhsValuesEnumerator.Current;

                if (lhsValuesCurrent.GetType().IsPrimitive)
                {
                    return EqualsPrimitiveType(lhsValuesCurrent, rhsValuesCurrent);
                }

                if (lhsValuesCurrent.GetType().IsArray || typeof(IEnumerable).IsAssignableFrom(lhsValuesCurrent.GetType()))
                {
                    return EqualsEnumerableType((IEnumerable)lhsValuesCurrent, (IEnumerable)rhsValuesCurrent);
                }

                if (lhsValuesCurrent.GetType().IsValueType || lhsValuesCurrent.GetType().IsClass)
                {
                    return Equals(lhsValuesCurrent, rhsValuesCurrent);
                }

                lhsValues.GetEnumerator().MoveNext();
                rhsValues.GetEnumerator().MoveNext();
            }

            return true;
        }
    }
}
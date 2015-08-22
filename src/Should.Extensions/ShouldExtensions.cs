using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Should.Core.Exceptions;

namespace Should.Extensions
{
    public static class ShouldExtensions
    {
        public static void ShouldEqualEnumerable<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)
            where TActual : class
            where TExpected : class
        {
            actual.ShouldEqualEnumerable(expected, (a, e) => a.HasSamePropertyValues(e));
        }

        public static void ShouldEqualEnumerable<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, bool ignoreMissingProperties)
            where TActual : class
            where TExpected : class
        {
            actual.ShouldEqualEnumerable(expected, (a, e) => a.HasSamePropertyValues(e, ignoreMissingProperties));
        }

        public static void ShouldEqualEnumerable<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, Func<TActual, TExpected, bool> equalityTester)
            where TActual : class
            where TExpected : class
        {
            if (equalityTester == null)
                equalityTester = (a, e) => a.HasSamePropertyValues(e);

            if (expected == null)
            {
                actual.ShouldBeNull();
                return;
            }

            var actualList = actual == null ? null : actual.ToList();
            actualList.ShouldNotBeNull();
            var expectedList = expected.ToList();
            actualList.Count().ShouldEqual(expectedList.Count());

            foreach (var expectedItem in expectedList)
            {
                var actualItem = actualList.Where(a => equalityTester(a, expectedItem));

                actualItem.Count().ShouldEqual(1);
            }
        }

        public static void ShouldEqualEnumerableStruct<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
            where T : struct
        {
            if (expected == null)
            {
                actual.ShouldBeNull();
                return;
            }

            var actualList = actual == null ? null : actual.ToList();
            actualList.ShouldNotBeNull();
            var expectedList = expected.ToList();
            actualList.Count().ShouldEqual(expectedList.Count());

            foreach (var expectedItem in expectedList)
            {
                actualList.Contains(expectedItem).ShouldBeTrue();
            }
        }

        public static void ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected, bool ignoreMissingProperties = false)
            where TActual : class
            where TExpected : class
        {
            if (expected == null)
            {
                actual.ShouldBeNull();
                return;
            }

            actual.ShouldNotBeNull();

            if (!ignoreMissingProperties)
                actual.ShouldHaveSameProperties(expected);

            var expectedProperties = typeof(TExpected).GetProperties();
            var actualProperties = typeof(TActual).GetProperties();

            foreach (PropertyInfo expectedProperty in expectedProperties)
            {
                var actualProperty = actualProperties.FirstOrDefault(p => p.Name == expectedProperty.Name);
                if (actualProperty != null)
                {
                    var expectedValue = expectedProperty.GetValue(expected, null);
                    var actualValue = actualProperty.GetValue(actual, null);

                    if (expectedProperty.PropertyType.IsGenericType && expectedProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return;
                    }
                    if (expectedProperty.PropertyType == typeof(DateTime?))
                    {
                        ((DateTime?)actualValue).ShouldEqualDateTime(
                            ((DateTime?)expectedValue),
                            string.Format("{0} values are not equal!", expectedProperty.Name));
                    }
                    else if (expectedProperty.PropertyType == typeof(DateTime))
                    {
                        ((DateTime)actualValue).ShouldEqualDateTime(
                            ((DateTime)expectedValue),
                            string.Format("{0} values are not equal!", expectedProperty.Name));
                    }
                    else if (expectedProperty.PropertyType.IsEnum)
                    {
                        // Can't handle comparing underlying values of different enums types - yet.
                        if (expectedProperty.PropertyType == actualProperty.PropertyType)
                        {
                            actualValue.ShouldEqual(expectedValue, string.Format("{0} values are not equal!", expectedProperty.Name));
                        }
                    }
                    else if ((Nullable.GetUnderlyingType(actualProperty.PropertyType) ?? actualProperty.PropertyType)
                        .IsEnum &&
                        (Nullable.GetUnderlyingType(expectedProperty.PropertyType) ?? expectedProperty.PropertyType)
                        == (Nullable.GetUnderlyingType(actualProperty.PropertyType)
                        ?? actualProperty.PropertyType).GetEnumUnderlyingType())
                    {
                        var t = Nullable.GetUnderlyingType(expectedProperty.PropertyType)
                            ?? expectedProperty.PropertyType;
                        var convertedActualValue = actualValue == null ? null : Convert.ChangeType(actualValue, t);
                        convertedActualValue.ShouldEqual(expectedValue, string.Format("{0} values are not equal!", expectedProperty.Name));
                    }
                    else
                    {
                        actualValue.ShouldEqual(expectedValue, string.Format("{0} values are not equal!", expectedProperty.Name));
                    }
                }
            }
        }

        public static void ShouldHaveSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
            var expectedProperties = typeof(TExpected).GetProperties().Select(p => p.Name).ToList();
            var actualProperties = typeof(TActual).GetProperties().Select(p => p.Name).ToList();

            var extraExpectedProperties = expectedProperties.Except(actualProperties).ToList();
            var extraActualProperties = actualProperties.Except(expectedProperties).ToList();

            if (extraActualProperties.Any() || extraExpectedProperties.Any())
            {
                var sb = new StringBuilder("Property Mismatch:");
                var expectedTypeName = expected.GetType().Name;
                var actualTypeName = actual.GetType().Name;

                foreach (var property in extraActualProperties)
                {
                    sb.AppendLine();
                    sb.AppendFormat("    {0} is not a property of {1}", property, expectedTypeName);
                }

                foreach (var property in extraExpectedProperties)
                {
                    sb.AppendLine();
                    sb.AppendFormat("    {0} is not a property of {1}", property, actualTypeName);
                }

                throw new AssertException(sb.ToString());
            }
        }

        public static void ShouldEqualDateTime(this DateTime? actual, DateTime? expected, string errorMessage = "")
        {
            if (!expected.HasValue)
            {
                actual.HasValue.ShouldBeFalse(errorMessage);
                return;
            }

            actual.HasValue.ShouldBeTrue(errorMessage);
            actual.Value.ShouldEqualDateTime(expected.Value, errorMessage);
        }

        public static void ShouldEqualDateTime(this DateTime actual, DateTime expected, string errorMessage = "")
        {
            bool isShortDateTime = (actual.Second == 0);

            DateTime internalExpected = expected;
            if (isShortDateTime)
            {
                internalExpected = (expected.Second >= 30) ? expected.AddSeconds(60D - expected.Second) : expected;
            }

            actual.Year.ShouldEqual(internalExpected.Year, errorMessage + Environment.NewLine + "Years are not equal.");
            actual.Month.ShouldEqual(internalExpected.Month, errorMessage + Environment.NewLine + "Months are not equal.");
            actual.Day.ShouldEqual(internalExpected.Day, errorMessage + Environment.NewLine + "Days are not equal.");
            actual.Hour.ShouldEqual(internalExpected.Hour, errorMessage + Environment.NewLine + "Hours are not equal.");
            actual.Minute.ShouldEqual(internalExpected.Minute, errorMessage + Environment.NewLine + "Minutes are not equal.");

            if (!isShortDateTime)
            {
                actual.Second.ShouldEqual(expected.Second, errorMessage + Environment.NewLine + "Seconds are not equal.");
            }
        }

        public static void ShouldEqualDate(this DateTime actual, DateTime expected, string errorMessage = "")
        {
            actual.Year.ShouldEqual(expected.Year, errorMessage + Environment.NewLine + "Years are not equal.");
            actual.Month.ShouldEqual(expected.Month, errorMessage + Environment.NewLine + "Months are not equal.");
            actual.Day.ShouldEqual(expected.Day, errorMessage + Environment.NewLine + "Days are not equal.");
        }
    }
}

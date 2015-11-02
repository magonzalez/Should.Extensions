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
        public static void ShouldHaveSimilarItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)
            where TActual : class
            where TExpected : class
        {
            actual.ShouldHaveSameItems(expected, (a, e) => a.HasSameSharedPropertyValues(e));
        }

        public static void ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)
            where TActual : class
            where TExpected : class
        {
            actual.ShouldHaveSameItems(expected, (a, e) => a.HasSamePropertyValues(e));
        }

        public static void ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, Func<TActual, TExpected, bool> equalityTester)
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

        public static void ShouldHaveSameValueItems<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
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

        /// <summary>
        /// Asserts that any properties that are defined by both the actual and expected type have the same values.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        public static void ShouldHaveSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
           actual.ShouldBeNullIfExpectingNull(expected);

            CheckThatSharedPropertyValuesAreEqual(actual, expected);
        }

        /// <summary>
        /// Asserts that the actual value has exactly the same properties and values as the expected value.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        public static void ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
            actual.ShouldBeNullIfExpectingNull(expected);
            actual.ShouldHaveSameProperties(expected);

            CheckThatSharedPropertyValuesAreEqual(actual, expected);
        }

        /// <summary>
        /// Asserts that the actual type has exactly the same properties as the expected type.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
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

        /// <summary>
        /// Asserts that the actual value is, or is not, null depending on the expected value. If the expected
        /// value is null, asserts that the actual value is null as well. If the expected value is not null,
        /// asserts that the actual value is not null as well.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual actual, TExpected expected, string errorMessage = "")
            where TActual : class
            where TExpected : class
        {
            if (expected == null)
            {
                actual.ShouldBeNull();
                return;
            }

            actual.ShouldNotBeNull(errorMessage);
        }

        /// <summary>
        /// Asserts that the actual value is, or is not, null depending on the expected value. If the expected
        /// value is null, asserts that the actual value is null as well. If the expected value is not null,
        /// asserts that the actual value is not null as well.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual? actual, TExpected? expected, string errorMessage = "")
            where TActual : struct
            where TExpected : struct
        {
            if (!expected.HasValue)
            {
                actual.HasValue.ShouldBeFalse(errorMessage);
                return;
            }

            actual.HasValue.ShouldBeTrue(errorMessage);
        }

        /// <summary>
        /// Asserts that the actual DateTime? value is equal to the expected DateTime? value.
        /// </summary>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldEqualDateTime(this DateTime? actual, DateTime? expected, string errorMessage = "")
        {
            actual.ShouldBeNullIfExpectingNull(expected);

            if ((actual.HasValue) && (expected.HasValue))
                actual.Value.ShouldEqualDateTime(expected.Value, errorMessage);
        }

        /// <summary>
        /// Asserts that the actual DateTime value is equal to the expected DateTime value.
        /// </summary>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldEqualDateTime(this DateTime actual, DateTime expected, string errorMessage = "")
        {
            actual.ShouldEqualDate(expected, errorMessage);
            actual.ShouldEqualTime(expected, errorMessage);
        }

        /// <summary>
        /// Asserts that the date portion of the given actual DateTime value is equal to the date
        /// portion of the expected DateTime value.
        /// </summary>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldEqualDate(this DateTime actual, DateTime expected, string errorMessage = "")
        {
            actual.Year.ShouldEqual(expected.Year, errorMessage + Environment.NewLine + "Years are not equal.");
            actual.Month.ShouldEqual(expected.Month, errorMessage + Environment.NewLine + "Months are not equal.");
            actual.Day.ShouldEqual(expected.Day, errorMessage + Environment.NewLine + "Days are not equal.");
        }

        /// <summary>
        /// Asserts that the time portion of the given actual DateTime value is equal to the time
        /// portion of the expected DateTime value.
        /// </summary>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        /// <param name="errorMessage">The optional error message to attach to the generated exception on failure.</param>
        public static void ShouldEqualTime(this DateTime actual, DateTime expected, string errorMessage = "")
        {
            bool isShortDateTime = (actual.Second == 0);

            var internalExpected = expected;
            if (isShortDateTime)
            {
                internalExpected = (expected.Second >= 30) ? expected.AddSeconds(60D - expected.Second) : expected;
            }

            actual.Hour.ShouldEqual(internalExpected.Hour, errorMessage + Environment.NewLine + "Hours are not equal.");
            actual.Minute.ShouldEqual(internalExpected.Minute, errorMessage + Environment.NewLine + "Minutes are not equal.");

            if (!isShortDateTime)
            {
                actual.Second.ShouldEqual(expected.Second, errorMessage + Environment.NewLine + "Seconds are not equal.");
            }
        }

        /// <summary>
        /// Asserts that the properties  that are shared between the actual and expected values are the same.
        /// </summary>
        /// <typeparam name="TActual">The actual value's type.</typeparam>
        /// <typeparam name="TExpected">The expected value's type.</typeparam>
        /// <param name="actual">The actual value to validate.</param>
        /// <param name="expected">The expected valut to validate against.</param>
        private static void CheckThatSharedPropertyValuesAreEqual<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class

        {
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
    }
}

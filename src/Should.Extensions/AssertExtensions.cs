using System;

namespace Should.Extensions
{
    public static class AssertExtensions
    {
        public static bool HasSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
            try
            {
                actual.ShouldHaveSameSharedPropertyValues(expected);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool HasSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
            try
            {
                actual.ShouldHaveSamePropertyValues(expected);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool HasSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)
            where TActual : class
            where TExpected : class
        {
            try
            {
                actual.ShouldHaveSameProperties(expected);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}

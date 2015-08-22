using System;

namespace Should.Extensions
{
    public static class AssertExtensions
    {
        public static bool HasSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected, bool ignoreMissingProperties = false)
            where TActual : class
            where TExpected : class
        {
            try
            {
                actual.ShouldHaveSamePropertyValues(expected, ignoreMissingProperties);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}

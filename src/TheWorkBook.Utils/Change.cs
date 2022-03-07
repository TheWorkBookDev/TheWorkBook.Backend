namespace TheWorkBook.Utils
{
    internal static class Change
    {
        public static T? To<T>(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return default(T);

            if (typeof(T) == typeof(Int32)) {
                return (T)(object)Convert.ToInt32(s);
            }

            if (typeof(T) == typeof(long)) {
                return (T)(object)Convert.ToInt64(s);
            }

            if (typeof(T) == typeof(decimal)) {
                return (T)(object)Convert.ToDecimal(s);
            }

            if (typeof(T) == typeof(DateTime)) {
                return (T)(object)Convert.ToDateTime(s);
            }

            if (typeof(T) == typeof(bool)) {
                return (T)(object)(s.Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) || s.Equals("Y", StringComparison.InvariantCultureIgnoreCase));
            }

            return (T)(object)Convert.ToString(s);
        }
    }
}

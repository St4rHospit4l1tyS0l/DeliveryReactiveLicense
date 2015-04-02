namespace Infrastructure.Extensions
{
    public static class BooleanExt
    {
        public static string ToCheckVal(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}

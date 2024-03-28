using System.Data;

namespace sgd.util
{
    public static class IDataReaderExtension
    {
        public static T? GetData<T>(this IDataReader dr, string field)
        {
            T? value = default(T?);

            if (!DBNull.Value.Equals(dr[field])) value = (T)dr[field];

            return value;
        }

        public static string? GetData(this IDataReader dr, string field)
        {
            return dr.GetData<string?>(field);
        }
    }
}
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using NpgsqlTypes;

namespace API.Controllers
{
    public class GenericController
    {
        private readonly ParallelOptions _parallelOptions = new() { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 2) };

        public GenericController()
        {

        }

        internal List<string> GetColumnNameListFromEntity<T>()
        {
            return [.. typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance).Select(f => f.Name)];
        }

        internal string GetColumnNameStringFromEntity<T>()
        {
            var fields = GetColumnNameListFromEntity<T>();

            return string.Join(",", fields.Select(f => $"\"{f}\""));
        }

        internal List<T> PopulateListFromDataRowList<T>(List<DataRow> dataRowList, Func<DataRow, T> dataRowConstructor)
        {
            var returnConcurrentBag = new ConcurrentBag<T>();

            Parallel.ForEach(dataRowList, _parallelOptions, dataRow =>
            {
                returnConcurrentBag.Add(dataRowConstructor(dataRow));
            });

            return [.. returnConcurrentBag];
        }

        internal NpgsqlDbType GetNpgsqlDbType<T>()
        {
            var type = typeof(T);

            if (type == typeof(string)) return NpgsqlDbType.Text;
            if (type == typeof(int)) return NpgsqlDbType.Integer;
            if (type == typeof(short)) return NpgsqlDbType.Smallint;
            if (type == typeof(long)) return NpgsqlDbType.Bigint;
            if (type == typeof(decimal)) return NpgsqlDbType.Numeric;
            if (type == typeof(double)) return NpgsqlDbType.Double;
            if (type == typeof(float)) return NpgsqlDbType.Real;
            if (type == typeof(bool)) return NpgsqlDbType.Boolean;
            if (type == typeof(DateTime)) return NpgsqlDbType.Timestamp;

            throw new NotSupportedException($"Unsupported type: {type.FullName}");
        }
    }
}

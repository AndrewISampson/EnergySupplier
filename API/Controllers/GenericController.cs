using System.Collections.Concurrent;
using System.Data;
using System.Reflection;

namespace API.Controllers
{
    public class GenericController
    {
        private readonly ParallelOptions _parallelOptions = new() { MaxDegreeOfParallelism = Environment.ProcessorCount - 2 };

        public GenericController()
        {

        }

        internal string GetColumnListFromEntity<T>()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            return string.Join(",", fields.Select(f => $"\"{f.Name}\""));
        }

        internal List<T> PopulateListFromDataRowList<T>(List<DataRow> dataRowList, Func<DataRow, T> dataRowConstructor)
        {
            var returnConcurrentBag = new ConcurrentBag<T>();

            Parallel.ForEach(dataRowList, _parallelOptions, dataRow =>
            {
                returnConcurrentBag.Add(dataRowConstructor(dataRow));
            });

            return returnConcurrentBag.ToList();
        }
    }
}

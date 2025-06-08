using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace API.Controllers
{
    public class DatabaseController
    {
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=EnergySupplier;Username=postgres;Password=EnergySupplier01;";

        public DatabaseController()
        {
        }

        internal DataRow GetFirstOrDefault(string SQL)
        {
            return GetDataTable(SQL).Rows.Cast<DataRow>().FirstOrDefault();
        }

        internal DataRow GetFirst(string SQL)
        {
            return GetDataTable(SQL).Rows.Cast<DataRow>().First();
        }

        internal List<DataRow> GetList(string SQL)
        {
            return [.. GetDataTable(SQL).Rows.Cast<DataRow>()];
        }

        internal DataTable GetDataTable(string SQL)
        {
            var dataTable = new DataTable();
            var npgsqlConnection = OpenDatabaseConnection();

            using (npgsqlConnection)
            {
                using var npgsqlCommand = CreateDatabaseCommand(SQL, npgsqlConnection);
                using var npgsqlDataAdapter = new NpgsqlDataAdapter(npgsqlCommand);
                npgsqlDataAdapter.Fill(dataTable);
            }

            CloseDatabaseConnection(npgsqlConnection);

            return dataTable;
        }

        internal void ExecuteScalar(string SQL)
        {
            var npgsqlConnection = OpenDatabaseConnection();

            using (npgsqlConnection)
            {
                var npgsqlCommand = CreateDatabaseCommand(SQL, npgsqlConnection);
                npgsqlCommand.ExecuteScalar();
            }

            CloseDatabaseConnection(npgsqlConnection);
        }

        internal void BulkInsertDetail<T>(long createdByUserId, string schema, string table, List<string> columnList, Dictionary<long, Dictionary<long, List<T>>> detailDictionary)
        {
            var genericController = new GenericController();

            columnList.Remove("Id");
            var entityIdColumn = columnList.First(c => c.EndsWith("Id") && !c.EndsWith("AttributeId") && !c.EndsWith("UserId"));
            var entityAttributeIdColumn = columnList.First(c => c.EndsWith("AttributeId"));

            var insertDateTime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            var npgsqlConnection = OpenDatabaseConnection();

            using var transaction = npgsqlConnection.BeginTransaction();

            var copySQL = $"COPY \"{schema}\".\"{table}\" (\"CreatedDateTime\", \"CreatedByUserId\", \"EffectiveFromDateTime\", \"{entityIdColumn}\", \"{entityAttributeIdColumn}\", \"Description\") FROM STDIN (FORMAT BINARY)";

            try
            {
                using (var npgsqlBinaryImporter = npgsqlConnection.BeginBinaryImport(copySQL))
                {
                    foreach (var entityKeyValuePair in detailDictionary)
                    {
                        foreach (var attributeKeyValuePair in entityKeyValuePair.Value)
                        {
                            foreach (var description in attributeKeyValuePair.Value)
                            {
                                npgsqlBinaryImporter.StartRow();
                                npgsqlBinaryImporter.Write(insertDateTime, NpgsqlDbType.Timestamp); //CreatedDateTime
                                npgsqlBinaryImporter.Write(createdByUserId, NpgsqlDbType.Smallint); //CreatedByUserId
                                npgsqlBinaryImporter.Write(insertDateTime, NpgsqlDbType.Timestamp); //EffectiveFromDateTime
                                npgsqlBinaryImporter.Write(entityKeyValuePair.Key, NpgsqlDbType.Smallint); //EntityId
                                npgsqlBinaryImporter.Write(attributeKeyValuePair.Key, NpgsqlDbType.Smallint); //EntityAttributeId
                                npgsqlBinaryImporter.Write(description, genericController.GetNpgsqlDbType<T>()); //Description
                            }
                        }
                    }

                    npgsqlBinaryImporter.Complete();
                }

                Console.WriteLine("Before Commit");
                transaction.Commit();
                Console.WriteLine("After Commit");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                transaction.Rollback();
            }

            CloseDatabaseConnection(npgsqlConnection);
        }

        private NpgsqlCommand CreateDatabaseCommand(string SQL, NpgsqlConnection npgsqlConnection)
        {
            return new NpgsqlCommand(SQL, npgsqlConnection)
            {
                CommandTimeout = 0
            };
        }

        private NpgsqlConnection OpenDatabaseConnection()
        {
            var npgsqlConnection = new NpgsqlConnection(_connectionString);

            while (npgsqlConnection.State != ConnectionState.Open)
            {
                try
                {
                    npgsqlConnection.Open();
                }
                catch
                {

                }
            }

            return npgsqlConnection;
        }

        private static void CloseDatabaseConnection(NpgsqlConnection npgsqlConnection)
        {
            while (npgsqlConnection.State != ConnectionState.Closed)
            {
                try
                {
                    npgsqlConnection.Close();
                }
                catch
                {

                }
            }
        }
    }
}

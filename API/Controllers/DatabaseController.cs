using System.Data;
using Npgsql;

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
            return GetDataTable(SQL).Rows.Cast<DataRow>().ToList();
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

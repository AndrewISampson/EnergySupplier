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

        internal DataTable GetDataTable(string SQL)
        {
            var dataTable = new DataTable();

            using (var npgsqlConnection = OpenDatabaseConnection())
            {
                using (var npgsqlCommand = CreateDatabaseCommand(SQL, npgsqlConnection))
                {
                    using (var npgsqlDataAdapter = new NpgsqlDataAdapter(npgsqlCommand))
                    {
                        npgsqlDataAdapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        internal string ExecuteScalar()
        {
            var result = string.Empty;
            var npgsqlConnection = OpenDatabaseConnection();

            using (npgsqlConnection)
            {
                var npgsqlCommand = CreateDatabaseCommand("SELECT version()", npgsqlConnection);
                result = npgsqlCommand.ExecuteScalar()?.ToString();
            }

            CloseDatabaseConnection(npgsqlConnection);

            return result;
        }

        private NpgsqlCommand CreateDatabaseCommand(string SQL, NpgsqlConnection npgsqlConnection)
        {
            return new NpgsqlCommand(SQL, npgsqlConnection);
        }

        private NpgsqlConnection OpenDatabaseConnection()
        {
            var npgsqlConnection = new NpgsqlConnection(_connectionString);

            while (npgsqlConnection.State != System.Data.ConnectionState.Open)
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
            while (npgsqlConnection.State != System.Data.ConnectionState.Closed)
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

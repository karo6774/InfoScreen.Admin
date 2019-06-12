using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public static class Database
    {
        private static readonly string Server = LoadSecret("DB_SERVER");
        private static readonly string DatabaseName = LoadSecret("DB_DATABASE");
        private static readonly string Username = LoadSecret("DB_USERNAME");
        private static readonly string Password = LoadSecret("DB_PASSWORD");

        private static string ConnectionString =>
            $"Data Source={Server};Initial Catalog={DatabaseName};User ID={Username};Password={Password};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadOnly;MultiSubnetFailover=False";

        private static string LoadSecret(string name)
        {
            var path = Environment.GetEnvironmentVariable($"{name}_PATH");
            if (path != null)
                return File.ReadAllText(path);
            var value = Environment.GetEnvironmentVariable(name);
            if (value == null)
                throw new MissingSecretException(name);
            return value;
        }

        private static SqlConnection MakeConnection() => new SqlConnection(ConnectionString);

        public static async Task<DataSet> Query(string query, DataSet dataSet = null,
            Dictionary<string, object> parameters = null)
        {
            if (dataSet == null)
                dataSet = new DataSet();
            using (var conn = MakeConnection())
            {
                await conn.OpenAsync();
                var adapter = new SqlDataAdapter(query, conn);
                if (parameters != null)
                    foreach (var (key, val) in parameters)
                        adapter.SelectCommand.Parameters.AddWithValue(key, val);

                adapter.Fill(dataSet);
                return dataSet;
            }
        }
    }

    class MissingSecretException : Exception
    {
        public MissingSecretException(string name) :
            base($"Missing secret {name}. Set either {name} or {name}_PATH")
        {
        }
    }
}
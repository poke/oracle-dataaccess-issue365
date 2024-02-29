using Oracle.DataAccess.Client;
using System;
using System.Data;

public class Program
{
    const string ConnectionString = "Data Source=localhost/xepdb1;User Id=schema;Password=password";

    public static void Main()
    {
        Console.WriteLine("Access via DataTable on Oracle.ManagedDataAccess:");
        using (var connection = new OracleConnection(ConnectionString))
        {
            connection.Open();
            var query = "SELECT ID, VALUE FROM TEST";
            var adapter = new OracleDataAdapter(query, connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            var value = dataTable.Rows[0].Field<string>("VALUE");
            Console.WriteLine("  Item has value: {0}", value is null ? "null" : $"'{value}'");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

const string ConnectionString = "Data Source=localhost/xepdb1;User Id=schema;Password=password";

var dbContextOptions = new DbContextOptionsBuilder<ExampleContext>()
    .UseOracle(ConnectionString)
    .Options;

using (var db = new ExampleContext(dbContextOptions))
{
    try { db.Database.ExecuteSqlRaw("DROP TABLE TEST"); } catch { }
    db.Database.ExecuteSqlRaw("CREATE TABLE TEST(ID NUMBER(19), VALUE CLOB)");

    db.Set<Entity>().Add(new Entity
    {
        Id = 1,
        Value = "",
    });
    db.SaveChanges();
}

Console.WriteLine("Access via EF Core");
using (var db = new ExampleContext(dbContextOptions))
{
    var item = db.Set<Entity>().Find(1);
    Console.WriteLine("  Item has value: {0}", item.Value is null ? "null" : $"'{item.Value}'");
}

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

class ExampleContext : DbContext
{
    public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>(entity =>
        {
            entity.ToTable("TEST");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Value).HasColumnType("CLOB").HasColumnName("VALUE");
        });
    }
}

class Entity
{
    public int Id { get; set; }
    public string Value { get; set; }
}

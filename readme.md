# Example projects for Oracle.DataAccess 21c issue

## Instructions
1. Adjust the connection strings in both `Managed/Program.cs` and `Unmanaged/Program.cs`

2. Run the managed project (e.g. using Visual Studio or `dotnet run`)

   Observe the output:

   ```
   Access via EF Core
     Item has value: ''
   Access via DataTable on Oracle.ManagedDataAccess:
     Item has value: ''
   ```

3. Now run the unmanaged project (e.g. using Visual Studio)

   The output will stay at this seemingly forever:

   ```
   Access via DataTable on Oracle.ManagedDataAccess:
   ```

   The line `adapter.Fill(dataTable)` does not appear to complete.

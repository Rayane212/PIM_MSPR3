using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;
namespace PIM_MSPR3.Model
{
    public class ProviderRepos
    {
        private readonly IConfiguration? _configuration;

        public ProviderRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertProvider(ProviderEntity Provider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Providers " +
                "(CodeProvider, BusinessName, Address, City, CodePostal, PhoneNumber, mail, CodeUser) " +
                "values " +
                "(@CodeProvider, @BusinessName, @Address, @City, @CodePostal, @PhoneNumber, @mail, @CodeUser) ", Provider);
        }

        public List<ProviderEntity> GetAllProvider()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<ProviderEntity>("Select * from Providers").ToList(); ;
        }

        public ProviderEntity GetByCodeProvider(string CodeProvider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<ProviderEntity>("Select * from Providers where CodeProvider = @CodeProvider", new {CodeProvider = CodeProvider });

        }

        public int UpdateProvider(ProviderEntity Provider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Providers set BusinessName = @BusinessName," +
                " Address = @Address," +
                " City = @City," +                
                " CodePostal = @CodePostal," +                
                " PhoneNumber = @PhoneNumber," +
                " mail = @mail," +
                " CodeUser = @CodeUser" +
                " where CodeProvider = @CodeProvider", Provider);

        }

        public int DeleteProvider(string CodeProvider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Providers where CodeProvider = @CodeProvider", new { CodeProvider = CodeProvider });

        }

        public async Task<bool> ExistingProvider(string CodeProvider)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Providers WHERE CodeProvider = @CodeProvider",
                    new { CodeProvider = CodeProvider.ToUpper() });
                return result != default(int);
            }
        }
                        
    }
}

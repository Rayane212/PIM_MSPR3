using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;

namespace PIM_MSPR3.Model
{
    public class TaxRepos
    {
        private readonly IConfiguration? _configuration;

        public TaxRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertTax(TaxEntity Tax)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Tax " +
                "(CodeTax, Rate) " +
                "values " +
                "(@CodeTax, @Rate) ", Tax);
        }

        public List<TaxEntity> GetAllTax()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<TaxEntity>("Select * from Tax").ToList(); ;
        }

        public int UpdateTax(TaxEntity Tax)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Tax set Rate = @Rate," +                
                " where CodeTax = @CodeTax", Tax);

        }

        public int DeleteTax(string CodeTax)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Tax where CodeTax = @CodeTax", new { CodeTax = CodeTax });

        }

        public async Task<bool> ExistingTax(string CodeTax)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Prices WHERE CodeTax = @CodeTax",
                    new { CodeTax = CodeTax.ToUpper() });
                return result != default(int);
            }
        }
                
    }
}

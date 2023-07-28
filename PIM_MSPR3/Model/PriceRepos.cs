using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;

namespace PIM_MSPR3.Model
{
    public class PriceRepos
    {
        private readonly IConfiguration? _configuration;

        public PriceRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertPrice(PriceEntity Price)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Prices " +
                "(CodePrice, Currency, PriceWT, QtyMinimal) " +
                "values " +
                "(@CodePrice, @Currency, @PriceWT, @QtyMinimal) ", Price);
        }

        public List<PriceEntity> GetAllPrice(string CodePrice)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<PriceEntity>("Select * from Prices where CodePrice = @CodePrice", new { CodePrice = CodePrice }).ToList(); ;
        }

        public int UpdatePrice(PriceEntity Price)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Prices set Currency = @Currency," +
                " PriceWT = @PriceWT," +
                " QtyMinimal = @QtyMinimal" +
                " where CodePrice = @CodePrice", Price);

        }

        public int DeletePrice(string CodePrice)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Prices where CodePrice = @CodePrice", new { CodePrice = CodePrice });

        }

        public async Task<bool> ExistingPrice(string CodePrice)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Prices WHERE CodePrice = @CodePrice",
                    new { CodePrice = CodePrice.ToUpper() });
                return result != default(int);
            }
        }
                   
    }
}

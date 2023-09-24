using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;


namespace PIM_MSPR3.Model
{
    public class ItemRepos
    {
        private readonly IConfiguration? _configuration;

        public ItemRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertItem(ItemEntity Item)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Items " +
                "(CodeItem, CodeUniversal, WeightItem, OriginItem, UniteVenteItem, DeclinationItem, CodeProvider, CodePrice, CodeTax, CodeVolume, CodeDescriptive) " +
                "values " +
                "(@CodeItem, @CodeUniversal, @WeightItem, @OriginItem, @UniteVenteItem, @DeclinationItem, @CodeProvider, @CodePrice, @CodeTax, @CodeVolume, @CodeDescriptive) ", Item);
        }

        public List<ItemEntity> GetAllItem()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<ItemEntity>("Select * from Items").ToList(); ;
        }

        public ItemEntity GetItemByCodeUniversal(string codeUniversal)
        { 
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<ItemEntity>("Select * from Items where CodeUniversal = @CodeUniversal", new { CodeUniversal = codeUniversal});
        }

        public ItemEntity GetItemByCodeProvider(string CodeItem, string CodeProvider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<ItemEntity>("Select * from Items where CodeItem = @CodeItem and CodeProvider = @CodeProvider", new { CodeItem = CodeItem, CodeProvider = CodeProvider });

        }

        public int UpdateItem(ItemEntity Item)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Items set " +
                " WeightItem = @WeightItem," +
                " OriginItem = @OriginItem," +
                " UniteVenteItem = @UniteVenteItem," +
                " DeclinationItem = @DeclinationItem," +
                " CodeProvider = @CodeProvider," +
                " CodePrice = @CodePrice," +
                " CodeTax = @CodeTax," +
                " CodeVolume = @CodeVolume," +
                " CodeDescriptive = @CodeDescriptive " +
                "WHERE CodeItem = @CodeItem", Item);

        }

        public int DeleteItem(string CodeItem)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Items where CodeItem = @CodeItem", new { CodeItem = CodeItem });

        }

        public int DeleteItemByCodeProvider(string CodeProvider, string CodeUniversal)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Items where CodeProvider = @CodeProvider and CodeUniversal = @CodeUniversal", new { CodeProvider = CodeProvider, CodeUniversal = CodeUniversal });

        }


        public async Task<bool> ExistingItem(string CodeItem)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Items WHERE CodeItem = @CodeItem",
                    new { CodeItem = CodeItem.ToUpper()});
                return result != default(int);
            }
        }

        public async Task<bool> ExistingItemByCodeUniversal(string CodeUniversal)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Items WHERE CodeUniversal = @CodeUniversal",
                    new { CodeUniversal = CodeUniversal });
                return result != default(int);
            }
        }

    }
}

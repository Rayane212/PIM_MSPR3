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

        public ItemEntity GetItemByCodeProvider(string CodeItem, string CodeProvider)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<ItemEntity>("Select * from Items where CodeItem = @CodeItem and CodeProvider = @CodeProvider", new { CodeItem = CodeItem, CodeProvider = CodeProvider });

        }

        public int UpdateItem(ItemEntity Item)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Items set CodeUniversal = @CodeUniversal," +
                " WeightItem = @WeightItem," +
                " OriginItem = @OriginItem," +
                " UniteVenteItem = @UniteVenteItem," +
                " DeclinationItem = @DeclinationItem," +
                " CodeProvider = @CodeProvider," +
                " CodePrice = @CodePrice," +
                " CodeTax = @CodeTax," +
                " CodeVolume = @CodeVolume," +
                " CodeDescriptive = @CodeDescriptive," +
                "where CodeItem = @CodeItem", Item);

        }

        public int DeleteItem(string CodeItem)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Items where CodeItem = @CodeItem", new { CodeItem = CodeItem });

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

    }
}

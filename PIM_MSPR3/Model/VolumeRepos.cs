using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;

namespace PIM_MSPR3.Model
{
    public class VolumeRepos
    {
        private readonly IConfiguration? _configuration;

        public VolumeRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertVolume(VolumeEntity Volume)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Volumes " +
                "(CodeVolume, Descriptive, Weights, Dimensions) " +
                "values " +
                "(@CodeVolume, @Descriptive, @Weights, @Dimensions) ", Volume);
        }

        public List<VolumeEntity> GetAllVolume()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<VolumeEntity>("Select * from Volumes").ToList(); ;
        }

        public VolumeEntity GetVolumeByCodeVolume(string CodeVolume)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<VolumeEntity>("Select * from Volumes where CodeVolume = @CodeVolume", new { CodeVolume = CodeVolume });

        }

        public int UpdateVolume(VolumeEntity Volume)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Volumes set Descriptive = @Descriptive," +
                " Weights = @Weights," +
                " Dimensions = @Dimensions" +                
                " where CodeVolume = @CodeVolume", Volume);

        }

        public int DeleteVolume(string CodeVolume)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Volumes where CodeVolume = @CodeVolume", new { CodeVolume = CodeVolume });

        }

        public async Task<bool> ExistingVolume(string CodeVolume)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Volumes WHERE CodeVolume = @CodeVolume",
                    new { CodeVolume = CodeVolume.ToUpper() });
                return result != default(int);
            }
        }
                
    }
}

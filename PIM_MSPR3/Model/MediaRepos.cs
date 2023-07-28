using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;

namespace PIM_MSPR3.Model
{
    public class MediaRepos
    {
        private readonly IConfiguration? _configuration;

        public MediaRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertMedia(MediaEntity Media)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Medias " +
                "(CodeMedias, TypeMedias, FileMedias, CodeItem) " +
                "values " +
                "(@CodeMedias, @TypeMedias, @FileMedias, @CodeItem) ", Media);
        }

        public List<MediaEntity> GetAllMedia()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<MediaEntity>("Select * from Medias").ToList(); ;
        }

        public int UpdateMedias(MediaEntity Media)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Medias set TypeMedias = @TypeMedias," +
                " FileMedias = @FileMedias," +
                " CodeItem = @CodeItem" +
                " where CodeMedias = @CodeMedias", Media);

        }

        public int DeleteMedia(string CodeMedias)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Medias where CodeMedias = @CodeMedias", new { CodeMedias = CodeMedias });

        }

        public async Task<bool> ExistingPrice(string CodePrice)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Medias WHERE CodeMedias = @CodeMedias",
                    new { CodePrice = CodePrice.ToUpper() });
                return result != default(int);
            }
        }
                
    }
}

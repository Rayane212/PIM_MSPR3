using Microsoft.Data.SqlClient;
using Dapper;
using System.Data.SqlClient;

namespace PIM_MSPR3.Model
{
    public class UserRepos
    {
        private readonly IConfiguration? _configuration;

        public UserRepos(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public int InsertUser(UserEntity User)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Insert into Users " +
                "(CodeUser, NameUser, LastNameUser, Username, mailUser, PasswordUser) " +
                "values " +
                "(@CodeUser, @NameUser, @LastNameUser, @Username, @mailUser, @PasswordUser) ", User);
        }

        public List<UserEntity> GetAllUsers()
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Query<UserEntity>("Select * from Users").ToList(); ;
        }

        public UserEntity GetUserByCodeUser(string CodeUser)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.QueryFirstOrDefault<UserEntity>("Select * from Users where CodeUser = @CodeUser", new { CodeUser = CodeUser });

        }

        public int UpdateUser(UserEntity User)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("Update Users set NameUser = @NameUser," +
                " LastNameUser = @LastNameUser," +
                " Username = @Username," +
                " mailUser = @mailUser," +
                " PasswordUser = @PasswordUser" +
                " where CodeUser = @CodeUser", User);
            
        }

        public int DeleteUser(string CodeUser)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
            return oSqlConnection.Execute("delete from Users where CodeUser = @CodeUser", new { CodeUser = CodeUser });

        }

        public async Task<bool> ExistingUser(string CodeUser)
        {
            using (var connection = new SqlConnection(_configuration?.GetConnectionString("SQL")))
            {
                await connection.OpenAsync();
                var result = await connection.QuerySingleOrDefaultAsync<int>(
                    "SELECT 1 FROM Users WHERE CodeUser = @CodeUser",
                    new { CodeUser = CodeUser.ToUpper() });
                return result != default(int);
            }
        }
                
    }
}

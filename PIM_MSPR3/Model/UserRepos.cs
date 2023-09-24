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

        public async Task<UserEntity> InsertUserAsync(UserEntity user, string userId)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));

            user.PasswordUser = BCrypt.Net.BCrypt.HashPassword(user.PasswordUser);

            var insertUser = await oSqlConnection.QueryFirstOrDefaultAsync<UserEntity>(
                "INSERT INTO Users (NameUser, LastNameUser, Username, MailUser, PasswordUser, CodeUser) " +
                "VALUES (@NameUser, @LastNameUser, @Username, @MailUser, @PasswordUser, @CodeUser)",
                new { user.NameUser, user.LastNameUser, user.Username, user.MailUser, user.PasswordUser, CodeUser = userId });
            return insertUser;
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
        public async Task<UserEntity?> GetUserAuthAsync(string? mail, string? username, string password )
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));

            var user = await oSqlConnection.QuerySingleOrDefaultAsync<UserEntity>(
                "SELECT * FROM Users WHERE MailUser = @MailUser OR UserName = @Username",
                new { MailUser = mail, Username = username });

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordUser))
            {
                return user;
            }

            return null;
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

        public async Task<UserEntity> ExistingUser(UserEntity user)
        {
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));

            return await oSqlConnection.QuerySingleOrDefaultAsync<UserEntity>(
                   "SELECT * FROM Users WHERE MailUser = @MailUser OR Username = @Username", new { user.MailUser, user.Username });
        }
                
    }
}

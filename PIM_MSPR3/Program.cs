using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PIM_MSPR3.Model;
using StackTim_TP;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();

                      });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

// SignIn 
app.MapGet("/signIn", async (IConfiguration _config, HttpContext http, string username, string password ) =>  // Pour tester en front supprimer les var username et password des paramétre et mette la route en post 
{
    try
    {
        // Récupération des identifiants de l'utilisateur
        //string username = http.Request.Form["nomUtilisateur"];
        //string password = http.Request.Form["MotDePasse"];

        // Vérifier que les identifiants sont valides
        using var connection = new SqlConnection(builder.Configuration.GetConnectionString("SQL"));
        var user = await connection.QuerySingleOrDefaultAsync<UserEntity>(
            "SELECT * FROM Users WHERE MailUser = @MailUser OR UserName = @Username AND PasswordUser = @PasswordUser", new { MailUser = username, Username = username, PasswordUser = password });
        if (user == null)
        {
            http.Response.StatusCode = 401; // Code HTTP 401 Unauthorized
            await http.Response.WriteAsync("Adresse email ou mot de passe incorrect.");
            return;
        }

        // Création du token d'authentification
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JwtConfig:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.CodeUser.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.mailUser),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);


        // Retourner le token d'authentification dans la réponse du serveur
        http.Response.StatusCode = 200;
        http.Response.ContentType = "application/json";
        await http.Response.WriteAsync(JsonConvert.SerializeObject(new { Token = tokenString }));
    }
    catch (Exception ex)
    {
        http.Response.StatusCode = 400; // Code HTTP 400 Bad Request
        await http.Response.WriteAsync(ex.Message);
        return;
    }
});

// logout
app.MapPost("/logout", async (HttpContext http, IConfiguration _config) =>
{
    var token = http.Request.Headers["Authorization"].ToString().Split(" ")[1];
    var claims = JwtUtils.DecodeJwt(token, _config["JwtConfig:Secret"]);
    var userId = claims[ClaimTypes.NameIdentifier];
    if (token != "")
    {
        http.Request.Headers.Remove("Authorization");
        http.Response.Redirect("/");
        http.Response.StatusCode = 200;
        await http.Response.WriteAsync($"Utilisateur {userId} a été déconnecté.");
    }
    else
    {
        http.Response.StatusCode = 409;
        await http.Response.WriteAsync("Il y'a eu un problème.");
        return;
    }

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

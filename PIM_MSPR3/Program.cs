using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

// Ajouter le service de cache distribu�
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
    app.UseSession();

}

app.UseSession();
app.UseCors(MyAllowSpecificOrigins);
// SignIn 
app.MapPost("/signIn", async (IConfiguration _config, HttpContext http, string? mail ,string? username, string password ) =>  // Pour tester en front supprimer les var username et password des param�tre et mette la route en post 
{
    try
    {
        // R�cup�ration des identifiants de l'utilisateur
        //string username = http.Request.Form["Username"];
        //string password = http.Request.Form["Password"];

        // V�rifier que les identifiants sont valides
        using var connection = new SqlConnection(builder.Configuration.GetConnectionString("SQL"));
        var userRepos = new UserRepos(_config);
        var user = await userRepos.GetUserAuthAsync(mail, username, password);
        if (user == null)
        {
            http.Response.StatusCode = 401; // Code HTTP 401 Unauthorized
            await http.Response.WriteAsync("Adresse email ou mot de passe incorrect.");
            return;
        }

        // Cr�ation du token d'authentification
        var jwtUtils = new JwtUtils(_config);
        string tokenString = jwtUtils.CreateToken(user);

        http.Response.Headers.Add("Authorization", "Bearer " + tokenString);


        // Retourner le token d'authentification dans la r�ponse du serveur
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

// SignUp

app.MapPost("/signUp", async (IConfiguration _config, HttpContext http, UserEntity user) =>
{
    var userId = user.NameUser.ToUpper();
    using var connection = new SqlConnection(builder.Configuration.GetConnectionString("SQL"));
    var userRepos = new UserRepos(_config);
    var existingUser = await userRepos.ExistingUser(user);
    if (existingUser != null)
    {
        http.Response.StatusCode = 409; // Code HTTP 409 Conflict
        await http.Response.WriteAsync("");
        return;
    }
     await userRepos.InsertUserAsync(user, userId);

    http.Response.StatusCode = 200; // Code HTTP 200 OK
    await http.Response.WriteAsync($"{userId} user successfully created.");
});

// Read Items
app.MapGet("/GetAllItems", async (IConfiguration _config, HttpContext http) =>
{
    //var userId = http.Session.GetString("UserId");
    var token = http.Request.Headers["Authorization"].ToString().Split(" ")[1]; 
    var claims = JwtUtils.DecodeJwt(token, _config["JwtConfig:Secret"]);
    var userId = claims[ClaimTypes.NameIdentifier];

    if (userId == null || userId == "")
    {
        http.Response.StatusCode = 401;
        await http.Response.WriteAsync("Utilisateur non connect�.");
        return;
    }
    var oSqlConnection = new SqlConnection(_config.GetConnectionString("SQL"));
    var ok = oSqlConnection.Query<ItemEntity>("Select * from Items").ToList();

    http.Response.StatusCode = 200;
    await http.Response.WriteAsJsonAsync(ok);


});

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();

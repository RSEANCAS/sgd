using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using sgd.bl.Contrato;
using sgd.bl.Implementacion;
using sgd.da.Contrato;
using sgd.da.Implementacion;
using sgd.webapi.autenticador.Models.Config;
using System.Text;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     if (jwtIssuer != null && jwtKey != null)
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = jwtIssuer,
             ValidAudience = jwtIssuer,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
         };
     }
 });
//Jwt configuration ends here

// Add services to the container.
var corsPolicies = builder.Configuration.GetSection("Cors:Policies").Get<CorsPolicyConfig[]>();

if (corsPolicies != null)
{
    foreach (var policy in corsPolicies)
    {
        string Name = policy.Name;
        string[] Origins = policy.Origins;
        string[] Methods = policy.Methods;
        string[] Headers = policy.Headers;

        builder.Services.AddCors(options => { options.AddPolicy(name: Name, policy => { policy.WithOrigins(Origins).WithHeaders(Headers).WithMethods(Methods); }); });
    }
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAplicacionBl, AplicacionBl>();
builder.Services.AddSingleton<IAplicacionDa, AplicacionDa>();
builder.Services.AddSingleton<IUsuarioBl, UsuarioBl>();
builder.Services.AddSingleton<IUsuarioDa, UsuarioDa>();
builder.Services.AddSingleton<IAplicacionUsuarioBl, AplicacionUsuarioBl>();
builder.Services.AddSingleton<IAplicacionUsuarioDa, AplicacionUsuarioDa>();

var app = builder.Build();

if (corsPolicies != null)
{
    foreach (var policy in corsPolicies)
    {
        string Name = policy.Name;

        app.UseCors(Name);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

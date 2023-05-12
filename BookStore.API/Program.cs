using AutoMapper;
using BookStore.API.Helpers;
using BookStore.BLL.Extensions;
using BookStore.DAL.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using BookStore.API.Helpers.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookStore.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookStore.API.Helpers.Http;
using BookStore.API.Helpers.Http.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtConfiguration").GetSection("AuthSecret").Value));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7172", // Replace with your own issuer
            ValidAudience = "https://localhost:7172", // Replace with your own audience
            IssuerSigningKey = key
        };
    });


builder.Services.AddControllers();


builder.Services.AddDbContext<BOOK_STOREContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreConnection")));

builder.Host.ConfigureLogging((context, logging) =>
{
    logging.AddFileLogger(x =>
    {
        context.Configuration.GetSection("Logging")
            .GetSection("ResureFile").Bind(x);
    });

});

builder.Services.AddIdentity<AspNetUser, AspNetRole>()
    .AddEntityFrameworkStores<BOOK_STOREContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<AspNetUser>>();
builder.Services.AddScoped<IAuthentication, Authentication>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Store", Version = "v1" });
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.All());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
    c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
    {
        new OpenApiSecurityScheme
        {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
});
});
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddBLLDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseSe();
app.UseAuthentication();
//app.UseMiddleware<JwtMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BOOK STORE v1");
});
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using MusicLibrary.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Infrastructure.Repositories;
using MusicLibrary.Api.Services;
using Microsoft.AspNetCore.Http.Features;
using MusicLibrary.Application.Auth.Interfaces;
using MusicLibrary.Infrastructure.Tokens;
using MusicLibrary.Infrastructure.Email;
using MusicLibrary.Infrastructure.Security;
using MusicLibrary.Application.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Dbcontext
builder.Services.AddDbContext<MusicLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMediaRepository, MediaRepository>();

// Minio Service Registration
builder.Services.AddSingleton<IMinioService, MinioService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824; // 1 GB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Minio Service Registration
builder.Services.AddSingleton<IMinioService, MinioService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
builder.Services.AddSingleton<IConfirmationTokenGenerator, ConfirmationTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Token
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for MinIO testing without SSL

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
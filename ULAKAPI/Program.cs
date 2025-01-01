using Business.Abstract;
using Business.Concrete;
using Dataaccess;
using Dataaccess.Abstract;
using Dataaccess.Concrete;
using Dataaccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ULAKAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Dependency Injection
builder.Services.AddScoped<IUsersDal, EfUsersDal>();
builder.Services.AddScoped<IAesCryptingService, AesCryptingService>();
builder.Services.AddScoped<IRsaCryptingService, RsaCryptingService>();
builder.Services.AddScoped<ICryptingService, CryptingManager>();
builder.Services.AddScoped<IMessageDal, EfMessageDal>();
builder.Services.AddScoped<IMessageGroupsDal, EfMessageGroupsDal>();
builder.Services.AddScoped<IUserTeamsDal, EfUserTeamsDal>();
builder.Services.AddScoped<ISehirDal, EfSehirDal>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IMessageService, MessageManager>();
// CORS Configuration (Allow only specific origins)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3010") // Frontend adresinizi buraya ekleyin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // AllowCredentials ile kimlik doðrulama bilgilerine izin veriyoruz
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // SignalR servisini ekliyoruz

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();  // Authentication middleware
app.UseAuthorization();   // Authorization middleware

app.UseHttpsRedirection(); // HTTPS Redirection

app.UseCors("CorsPolicy");  // CORS policy kullanýmýný saðlayýn

app.MapControllers();

// SignalR hub'ýný burada tanýmlayýn
app.MapHub<ChatHub>("/chatHub")
    .RequireCors("CorsPolicy");
app.Run();

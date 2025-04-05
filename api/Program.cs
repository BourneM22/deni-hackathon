using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()    // Allow any origin
              .AllowAnyHeader()    // Allow any headers
              .AllowAnyMethod();   // Allow any HTTP methods (GET, POST, etc.)
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiDocument(options =>
{
    options.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme()
    {
        Description = "Bearer token authorization header",
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer"
    });

    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.Configure<MySqlModel>(builder.Configuration.GetSection("MySql"));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.Configure<FileConfig>(builder.Configuration.GetSection("FileConfig"));
builder.Services.Configure<ProfilePictureConfig>(builder.Configuration.GetSection("ProfilePictureConfig"));
builder.Services.Configure<SearchingConfig>(builder.Configuration.GetSection("SearchingConfig"));
builder.Services.Configure<ChatBotConfig>(builder.Configuration.GetSection("ChatBotConfig"));

builder.Services.AddTransient<DbConnection>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteTagService, NoteTagService>();
builder.Services.AddScoped<IPriorityService, PriorityService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<ISoundboardFilterService, SoundboardFilterService>();
builder.Services.AddScoped<ISoundbardService, SoundboardService>();
builder.Services.AddScoped<IChatBotService, ChatBotService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITextToSpeechService, TextToSpeechService>();
builder.Services.AddScoped<ISearchingService, SearchingService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtConfig:Key").Value!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


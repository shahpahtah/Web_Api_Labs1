using Data.EF;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Web.Middleware;
using Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add security definition
    c.AddSecurityDefinition("X-User-Role", new OpenApiSecurityScheme
    {
        Description = "Authorization header using the X-User-Role scheme",
        Name = "X-User-Role",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "apiKey"
    });

    // Add security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-User-Role"
                }
            },
            new string[] {}
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>(); // Swashbuckle.AspNetCore.Filters
}
); builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserProfileConnection")));
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // Set to 100MB
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseUserRole();
app.UseAuthorization();

app.MapControllers();

app.Run();

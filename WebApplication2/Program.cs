using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2.MDbContext;
using WebApplication2.Repositories;
using WebApplication2.Service;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header

            },
            new List<string>()
        }
    });
});




builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString")));

// or ReferenceHandler.Preserve if you need full cycle handling
builder.Services.AddScoped<IEmployeeRepository, EmployeeService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentService>();
builder.Services.AddScoped<IProjectRepository, ProjectService>();
builder.Services.AddScoped<IEmployeeProjectRepository, EmployeeProjectService>(); 
builder.Services.AddScoped<TokenService>(); 


builder.Services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthString")));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Employee")
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit=false;
    options.Password.RequireLowercase=false;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequireUppercase=false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>options.TokenValidationParameters=new TokenValidationParameters
    {
         ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
    
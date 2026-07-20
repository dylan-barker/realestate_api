using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.Services;
using RealEstateApi.Infrastructure.Data;
using RealEstateApi.Infrastructure.Repositories;
using RealEstateApi.Infrastructure.Services;
using RealEstateApi.Mappings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DbConnectionFactory(connectionString!));

builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IListingAddressRepository, ListingAddressRepository>();
builder.Services.AddScoped<IListingBuildingInfoRepository, ListingBuildingInfoRepository>();
builder.Services.AddScoped<IListingValuationRepository, ListingValuationRepository>();
builder.Services.AddScoped<IPropertyRunningCostsRepository, PropertyRunningCostsRepository>();
builder.Services.AddScoped<IListingRoomRepository, ListingRoomRepository>();
builder.Services.AddScoped<IListingParkingRepository, ListingParkingRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IListingOutdoorFeatureRepository, ListingOutdoorFeatureRepository>();

// Infrastructure Services
builder.Services.Configure<R2Options>(builder.Configuration.GetSection(R2Options.SectionName));
builder.Services.AddSingleton<IImageService, R2ImageService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Application Services
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IListingRoomService, ListingRoomService>();
builder.Services.AddScoped<IListingParkingService, ListingParkingService>();
builder.Services.AddScoped<IListingContactService, ListingContactService>();
builder.Services.AddScoped<IListingOutdoorFeatureService, ListingOutdoorFeatureService>();

// Auth
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
var signingKey = Encoding.UTF8.GetBytes(jwtOptions!.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKey)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

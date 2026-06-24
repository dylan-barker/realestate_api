using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.Services;
using RealEstateApi.Infrastructure.Data;
using RealEstateApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IListingAddressRepository, ListingAddressRepository>();
builder.Services.AddScoped<IListingBuildingInfoRepository, ListingBuildingInfoRepository>();
builder.Services.AddScoped<IListingValuationRepository, ListingValuationRepository>();
builder.Services.AddScoped<IPropertyRunningCostsRepository, PropertyRunningCostsRepository>();
builder.Services.AddScoped<IListingRoomRepository, ListingRoomRepository>();
builder.Services.AddScoped<IListingParkingRepository, ListingParkingRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Application Services
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IListingRoomService, ListingRoomService>();
builder.Services.AddScoped<IListingParkingService, ListingParkingService>();
builder.Services.AddScoped<IListingContactService, ListingContactService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

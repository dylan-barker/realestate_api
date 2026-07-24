using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingBuildingInfoRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingBuildingInfoRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ListingBuildingInfo?> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingBuildingInfo>(
            "SELECT * FROM ListingBuildingInfo WHERE ListingId = @ListingId", new { ListingId = listingId });
    }

    public async Task<ListingBuildingInfo> UpsertAsync(ListingBuildingInfo info)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingBuildingInfo>(
            "MERGE ListingBuildingInfo AS t " +
            "USING (SELECT @ListingId AS ListingId) AS s " +
            "ON t.ListingId = s.ListingId " +
            "WHEN MATCHED THEN UPDATE SET " +
            "ErfSize = @ErfSize, FloorArea = @FloorArea, ConstructionYear = @ConstructionYear, FacingId = @FacingId, ZoningId = @ZoningId " +
            "WHEN NOT MATCHED THEN INSERT (ListingId, ErfSize, FloorArea, ConstructionYear, FacingId, ZoningId) " +
            "VALUES (@ListingId, @ErfSize, @FloorArea, @ConstructionYear, @FacingId, @ZoningId) " +
            "OUTPUT INSERTED.*;",
            info);
    }
}

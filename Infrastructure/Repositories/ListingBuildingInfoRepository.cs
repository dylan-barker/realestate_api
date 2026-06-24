using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingBuildingInfoRepository : IListingBuildingInfoRepository
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
            "sp_ListingBuildingInfo_GetByListingId",
            new { ListingId = listingId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingBuildingInfo> UpsertAsync(ListingBuildingInfo info)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingBuildingInfo>(
            "sp_ListingBuildingInfo_Upsert",
            new
            {
                ListingId = info.ListingId,
                ErfSize = info.ErfSize,
                FloorArea = info.FloorArea,
                ConstructionYear = info.ConstructionYear,
                FacingId = info.FacingId,
                ZoningId = info.ZoningId
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}

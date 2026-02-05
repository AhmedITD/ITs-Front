using Microsoft.Extensions.Caching.Memory;
using RentARide.Application.DTOs.responses.VehicleResponse;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Infrastructure.Services;

public class VehicleTypeCacheService(IMemoryCache memoryCache) : IVehicleTypeCacheService
{
    private const string CacheKey = "VehicleTypes";
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(30);

    public async Task<IReadOnlyList<VehicleTypeDto>> GetOrSetTypesAsync(
        Func<CancellationToken, Task<IReadOnlyList<VehicleTypeDto>>> factory,
        CancellationToken cancellationToken = default)
    {
        return await memoryCache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultDuration;
            return await factory(cancellationToken);
        }) ?? new List<VehicleTypeDto>();
    }

    public void InvalidateTypes()
    {
        memoryCache.Remove(CacheKey);
    }
}

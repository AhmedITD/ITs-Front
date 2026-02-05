using RentARide.Application.DTOs.responses.AmenityResponse;
using RentARide.Domain.Entities;
using Mapster;

namespace RentARide.Application.Mapping;

public class AmenityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Amenity, AmenityDto>();
    }
}

using RentARide.Application.DTOs.responses.VehicleResponse;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Mapster;

namespace RentARide.Application.Mapping;

public class VehicleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Vehicle, VehicleDto>()
            .Map(d => d.Status, s => s.Status.ToString())
            .Map(d => d.VehicleTypeName, s => s.VehicleType.Name);

        config.NewConfig<VehicleType, VehicleTypeDto>();
    }
}

using RentARide.Application.DTOs.Responses.RentalResponse;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Mapster;

namespace RentARide.Application.Mapping;

public class RentalMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Rental, RentalDto>()
            .Map(d => d.Status, s => s.Status.ToString())
            .Map(d => d.VehicleModel, s => s.Vehicle.Model)
            .Map(d => d.AmenityNames, s => s.RentalAmenities.Select(ra => ra.Amenity.Name).ToList());

        config.NewConfig<Rental, RentalHistoryItemDto>()
            .Map(d => d.Status, s => s.Status.ToString())
            .Map(d => d.VehicleModel, s => s.Vehicle.Model)
            .Map(d => d.LicensePlate, s => s.Vehicle.LicensePlate);
    }
}

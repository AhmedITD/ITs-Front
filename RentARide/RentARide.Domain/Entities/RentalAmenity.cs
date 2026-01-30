namespace RentARide.Domain.Entities;

public class RentalAmenity
{
    public int RentalId { get; set; }
    public int AmenityId { get; set; }

    public Rental Rental { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}

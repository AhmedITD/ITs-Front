namespace RentARide.Domain.Entities;

public class Amenity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public ICollection<RentalAmenity> RentalAmenities { get; set; } = new List<RentalAmenity>();
}

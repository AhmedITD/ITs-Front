using RentARide.Domain.Enums;
using RentARide.Domain.Interfaces;

namespace RentARide.Domain.Entities;

public class Rental : IAuditable
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; }
    public Guid? InvoiceId { get; set; }

    public User User { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public Invoice? Invoice { get; set; }
    public ICollection<RentalAmenity> RentalAmenities { get; set; } = new List<RentalAmenity>();

    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

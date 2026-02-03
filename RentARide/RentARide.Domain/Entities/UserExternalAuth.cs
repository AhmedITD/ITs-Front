namespace RentARide.Domain.Entities;

/// <summary>
/// Links an external provider's stable user id (e.g. SuperQi userId from validation) to our User.
/// Ensures the same SuperQi user always resolves to the same RentARide user.
/// </summary>
public class UserExternalAuth
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Provider { get; set; }
    public required string ExternalUserId { get; set; }

    public User User { get; set; } = null!;
}

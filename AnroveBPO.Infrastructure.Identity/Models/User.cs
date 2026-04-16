using Microsoft.AspNetCore.Identity;

namespace AnroveBPO.Infrastructure.Identity.Models;

public class User : IdentityUser<Guid>
{
    public Guid? CustomerId { get; set; }
}
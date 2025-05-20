using Microsoft.AspNetCore.Identity;

namespace Microservice.IDP.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}
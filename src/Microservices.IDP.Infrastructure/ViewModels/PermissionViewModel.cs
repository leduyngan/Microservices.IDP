using Microservices.IDP.Infrastructure.Domains;

namespace Microservices.IDP.Infrastructure.ViewModels;

public class PermissionViewModel : EntityBase<long>
{
    public string Function { get; set; }

    public string RoleId { get; set; }

    public string Command { get; set; }
}
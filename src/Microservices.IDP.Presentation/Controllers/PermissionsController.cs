using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.IDP.Presentation.Controllers;

[ApiController]
[Route("api/roles/{roleId}/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    public PermissionsController(IRepositoryManager repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions(string roleId)
    {
        var result = await _repository.Permission.GetPermissionsByRole(roleId);
        return Ok(result);
    }
}
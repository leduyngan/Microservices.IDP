using System.ComponentModel.DataAnnotations;
using System.Net;
using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microservices.IDP.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.IDP.Presentation.Controllers;

[ApiController]
[Route("api/[controller]/roles/{roleId}")]
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
    
    [HttpPost]
    [ProducesResponseType(typeof(PermissionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreatePermission(string roleId, [FromBody] PermissionAddModel model)
    {
        var result = await _repository.Permission.CreatePermission(roleId, model);
        return result != null ? Ok(result) : NoContent();
        // return CreatedAtRoute("GetById", new { result.Id });
    }
    
    [HttpDelete("function/{function}/command/{command}")]
    [ProducesResponseType(typeof(PermissionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeletePermission(string roleId, [Required] string function, [Required] string command)
    {
        await _repository.Permission.DeletePermission(roleId, function, command);
        return NoContent();
    }
    
    [HttpPost("update-permissions")]
    [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdatePermissions(string roleId, [FromBody] IEnumerable<PermissionAddModel> permissions)
    {
        await _repository.Permission.UpdatePermissionsByRoleId(roleId, permissions);
        return NoContent();
    }
}
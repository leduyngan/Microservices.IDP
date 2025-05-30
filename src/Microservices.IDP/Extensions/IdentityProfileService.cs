using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microservices.IDP.Common;
using Microservices.IDP.Infrastructure.Common;
using Microservices.IDP.Infrastructure.Entities;
using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Microservices.IDP.Extensions;

public class IdentityProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;

    public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, UserManager<User> userManager, IRepositoryManager repositoryManager)
    {
        _claimsFactory = claimsFactory;
        _userManager = userManager;
        _repositoryManager = repositoryManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        if (user == null)
        {
            throw new ArgumentNullException("Userid not found!");
        }
        
        var principal = await _claimsFactory.CreateAsync(user);
        var claims = principal.Claims.ToList();
        var roles = await _userManager.GetRolesAsync(user);
        var permissionsQuery = await _repositoryManager.Permission.GetPermissionsByUser(user);
        var permissions = permissionsQuery.Select(x => PermissionHelper.GetPermission(x.Function, x.Command));
        
        
        claims.Add(new Claim(SystemConstants.Claims.UserName, user.UserName));
        claims.Add(new Claim(SystemConstants.Claims.FirstName, user.FirstName));
        claims.Add(new Claim(SystemConstants.Claims.LastName, user.LastName));
        claims.Add(new Claim(SystemConstants.Claims.UserId, user.Id));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim(ClaimTypes.Email, user.Email)); 
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new Claim(SystemConstants.Claims.Roles, string.Join(",", roles)));
        claims.Add(new Claim(SystemConstants.Claims.Permissions, JsonSerializer.Serialize(permissions)));
        
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
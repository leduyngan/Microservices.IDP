using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.IDP.Presentation.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    public PermissionsController(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
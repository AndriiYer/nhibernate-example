using Microsoft.AspNetCore.Mvc;

namespace Turnit.GenericStore.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
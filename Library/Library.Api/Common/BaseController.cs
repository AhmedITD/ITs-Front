using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Common;


[ApiController]
[Route("/[controller]")]
public abstract class BaseController : ControllerBase
{
    
}
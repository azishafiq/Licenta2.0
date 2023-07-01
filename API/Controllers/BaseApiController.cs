using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   // [ServiceFilter(typeof(LogUserActivity))]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
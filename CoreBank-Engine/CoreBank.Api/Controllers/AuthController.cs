using CoreBank.Application.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }
    }
}

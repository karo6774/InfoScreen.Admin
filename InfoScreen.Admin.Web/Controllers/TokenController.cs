using System.Collections.Generic;
using System.Threading.Tasks;
using InfoScreen.Admin.Web.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InfoScreen.Admin.Web.Controllers
{
    [EnableCors]
    [Route("/token")]
    public class TokenController : Controller
    {
        private readonly LoginService _login;

        public TokenController(LoginService login)
        {
            _login = login;
        }

        public async Task<IActionResult> Post([FromBody] Dictionary<string, object> data)
        {
            var username = (string) data["username"];
            var password = (string) data["password"];

            var token = await _login.Login(username, password);
            if (token == null)
            {
                Response.StatusCode = 422;
                return new EmptyResult();
            }

            return new JsonResult(new Dictionary<string, string> {{"token", token}});
        }
    }
}
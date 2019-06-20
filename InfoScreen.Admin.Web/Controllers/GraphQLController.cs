using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using InfoScreen.Admin.Web.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InfoScreen.Admin.Web.Controllers
{
    [EnableCors]
    [Route("/graphql")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly LoginService _login;
        private bool _enableAuth;

        private readonly Regex _headerRegex = new Regex("^Bearer\\s+(.+)$");

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, LoginService login,
            IConfiguration configuration)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _login = login;
            if (!bool.TryParse(configuration["EnableAuth"], out _enableAuth))
                _enableAuth = true;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query, [FromHeader] string authorization)
        {
            if (_enableAuth)
            {
                if (authorization == null)
                {
                    Response.StatusCode = 401;
                    Response.Headers["WWW-Authenticate"] = "Bearer";
                    return new EmptyResult();
                }

                var match = _headerRegex.Match(authorization);
                if (!match.Success)
                {
                    Response.StatusCode = 401;
                    Response.Headers["WWW-Authenticate"] = "Bearer";
                    return new EmptyResult();
                }

                var token = match.Groups[1].Value;
                var (success, id) = _login.VerifyToken(token);
                if (!success)
                {
                    Response.StatusCode = 403;
                    return new EmptyResult();
                }
            }

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                OperationName = query.OperationName,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors != null)
                foreach (var error in result.Errors)
                    Console.Error.Write(error);

            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
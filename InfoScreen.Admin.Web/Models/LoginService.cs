using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InfoScreen.Admin.Logic;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;

namespace InfoScreen.Admin.Web.Models
{
    public class LoginService
    {
        private static readonly string Secret = LoadSecret("JWT_SECRET");
        private static readonly string Algorithm = LoadSecret("JWT_ALGORITHM");

        private readonly IAdminRepository _admins;

        public LoginService(IAdminRepository admins)
        {
            _admins = admins;
        }

        public async Task<string> Login(string username, string password)
        {
            var admin = await _admins.FindByUsername(username);
            if (admin == null)
                return null;

            if (!admin.MatchesPassword(password))
                return null;

            IJwtAlgorithm algorithm;
            switch (Algorithm)
            {
                case "HMACSHA256":
                    algorithm = new HMACSHA256Algorithm();
                    break;
                default:
                    throw new ApplicationException($"Invalid JWT algorithm: {Algorithm}");
            }

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return new JwtBuilder()
                .WithAlgorithm(algorithm)
                .WithSecret(Secret)
                .AddClaim("exp", Math.Floor(((DateTime.Now+TimeSpan.FromDays(365)).ToUniversalTime() - epoch).TotalSeconds))
                .AddClaim("sub", admin.Id)
                .Build();
        }

        public (bool, int) VerifyToken(string token)
        {
            try
            {
                var data = new JwtBuilder()
                    .WithSecret(Secret)
                    .MustVerifySignature()
                    .Decode<Dictionary<string, string>>(token);
                return (true, int.Parse(data["sub"]));
            }
            catch (TokenExpiredException)
            {
                return (false, 0);
            }
            catch (SignatureVerificationException)
            {
                return (false, 0);
            }
        }

        private static string LoadSecret(string name)
        {
            var path = Environment.GetEnvironmentVariable($"{name}_PATH");
            if (path != null)
                return File.ReadAllText(path);
            var value = Environment.GetEnvironmentVariable(name);
            if (value == null)
                throw new MissingSecretException(name);
            return value;
        }

        class MissingSecretException : Exception
        {
            public MissingSecretException(string name) :
                base($"Missing secret {name}. Set either {name} or {name}_PATH")
            {
            }
        }
    }
}
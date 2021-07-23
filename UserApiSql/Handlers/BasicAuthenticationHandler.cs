using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UserApiSql.Helpers;
using UserApiSql.Interfaces;
using UserApiSql.Models;

namespace UserApiSql.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JwtService _jwtService;
        private readonly IUnitOfWork _uof;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUnitOfWork uof,
            JwtService jwtService)
            : base (options,logger,encoder,clock)
        {
            _uof = uof;
            _jwtService = jwtService;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = await _uof.UserRepository.Get(userId);

                var claims = new[] { new Claim(ClaimTypes.Name, user.EmailAddress) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticker = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticker);
            }
            catch
            {
                return AuthenticateResult.Fail("Authorization was not found");
            }
        }
    }
}

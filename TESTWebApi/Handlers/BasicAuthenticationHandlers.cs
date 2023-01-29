using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TESTWebApi.Models;

namespace TESTWebApi.Handlers
{
	public class BasicAuthenticationHandlers : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly dbConUsers _dbConnect;
		public BasicAuthenticationHandlers(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			dbConUsers dbConnect) : base(options, logger, encoder, clock ) 
		{
			_dbConnect = dbConnect;
		}
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authentication"))
				return AuthenticateResult.Fail("Authentication header was not found");

			try
			{
				var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authentication"]);
				var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
				string[] credential = Encoding.UTF8.GetString(bytes).Split(":");
				string emailA = credential[0];
				string password = credential[1];

				Users users = _dbConnect.Users.Where(users => users.UserEmail == emailA && users.UserPassword == password).FirstOrDefault();
				if (users == null)
					return AuthenticateResult.Fail("Invalid username or pasword");
				else {
					var claims = new[] { new Claim(ClaimTypes.Name, users.UserEmail) };
					var identity = new ClaimsIdentity(claims, Scheme.Name);
					var principal = new ClaimsPrincipal(identity);
					var ticket = new AuthenticationTicket(principal, Scheme.Name);
					return AuthenticateResult.Success(ticket);
				}

			}
			catch (Exception)
			{

				return AuthenticateResult.Fail("Error has occured");
			}
		}
	}
}

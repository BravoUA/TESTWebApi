using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TESTWebApi.Models;

namespace TESTWebApi.Controllers
{
    [ApiController]
	[Route("[controller]")]
	[Authorize]
	public class RSSController : Controller
	{


		private readonly dbConnect dbConnect;
		private readonly dbConUsers dbConUsers;
		public List<RSSChannel> rssChannels = new List<RSSChannel>();
		public List<UsersSubscribes> usersSubscribes = new List<UsersSubscribes>();
		public List<ChannelNews> channelNews = new List<ChannelNews>();
		public ParsPage Pars;
		public RSSController(dbConnect _dbConnect, dbConUsers _dbConUsers) {

			dbConUsers = _dbConUsers;
			dbConnect = _dbConnect;

		}

		[HttpPost("AddRSSChanne")]
		public async Task <IActionResult> Post(string Url)
		{
			string userEmail = HttpContext.User.Identity.Name;
			Users users = await dbConUsers.Users.Where(users => users.UserEmail == userEmail).FirstOrDefaultAsync();

			Pars = new ParsPage(dbConnect);
			Pars.parsPage(Url, users.Id);
			return Ok();
		}

		[HttpGet("GetAllRSS")]
		public async Task<IActionResult> GET()
		{
			string userEmail = HttpContext.User.Identity.Name;
			Users Users = await dbConUsers.Users.Where(users => users.UserEmail == userEmail).FirstOrDefaultAsync();
			ReturnData ReturnData = new ReturnData(dbConnect);
			return Ok(ReturnData.GetAllRSSSubscribes(Users.Id));
		}

		[HttpGet("GetAllUnreadNewsByDate")]
		public async Task<IActionResult> GET(DateTime dateTime)
		{
			string userEmail = HttpContext.User.Identity.Name;
			Users users = await dbConUsers.Users.Where(users => users.UserEmail == userEmail).FirstOrDefaultAsync();
			ReturnData ReturnData = new ReturnData(dbConnect);
			return Ok(ReturnData.GetAllUnreadNewsByDate(dateTime, users.Id));
		}

		[HttpPut("SetNewsAsRead")]
		public IActionResult PUT(int ID)
		{
			InsertData insertData = new InsertData(dbConnect);
			insertData.SetAsRead(ID);
			return Ok();
		}


	}
}

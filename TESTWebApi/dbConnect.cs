using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TESTWebApi.Models;

namespace TESTWebApi
{
	public class dbConnect : DbContext
	{
		public DbContext DbContext { get; set; }
		public DbSet<ChannelNews> ChannelNews { get; set; }
		public DbSet<RSSChannel> RSSChannel { get; set; }
		public DbSet<UsersSubscribes> UsersSubscribes  { get; set; }

		public dbConnect()
		{
			
		}
		public dbConnect(DbContextOptions<dbConnect> options) : base(options)
		{
			 Database.EnsureCreated();
		}







	}

}

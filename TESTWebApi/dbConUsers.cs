using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TESTWebApi.Models;

namespace TESTWebApi
{
	public class dbConUsers : DbContext
	{
		public DbContext DbContext { get; set; }
		public DbSet<Users> Users { get; set; }
		public DbSet<ChannelNews> ChannelNews { get; set; }
		public DbSet<RSSChannel> RSSChannel { get; set; }
		public DbSet<UsersSubscribes> UsersSubscribes { get; set; }
		public dbConUsers()
		{
			Database.EnsureCreated();
		}
		public dbConUsers(DbContextOptions<dbConUsers> options) : base(options)
		{
			
		}









	}

}

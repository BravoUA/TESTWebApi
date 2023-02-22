using TESTWebApi.Models;

namespace TESTWebApi
{
    public class InsertData
    {

		public List<ChannelNews> channelNews = new List<ChannelNews>();
		public ChannelNews channelNews2;
		public RSSChannel rssChannel;
		public List<RSSChannel> rssChannels;
		public UsersSubscribes usersSubscribes;
		public List<UsersSubscribes> UsersSubscribes;

		private readonly dbConnect dbConnect;

		public InsertData(dbConnect dbConnect1)
		{
			dbConnect = dbConnect1;
		}


		public void AddRssChannel(RSSChannel rssChannel, UsersSubscribes usersSubscribes, List<ChannelNews> channelNews) {
			try
			{
				dbConnect.RSSChannel.Add(rssChannel);
				dbConnect.SaveChanges();


				usersSubscribes.RSSChannelID = rssChannel.id;
				dbConnect.UsersSubscribes.Add(usersSubscribes);
				dbConnect.SaveChanges();
			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}
			try
			{
				for (int i = 0; i < channelNews.Count; i++)
				{
					dbConnect.ChannelNews.Add(channelNews[i]);
				}
				dbConnect.SaveChanges();

			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}

		}
		public void UpdateRssNews(List<ChannelNews> channelNews)
		{
		
			try
			{
				for (int i = 0; i < channelNews.Count; i++)
				{
					dbConnect.ChannelNews.Add(channelNews[i]);
				}
				dbConnect.SaveChanges();

			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}

		}
		public void DeleteRssNews()
		{

			try
			{
				dbConnect.ChannelNews.RemoveRange(dbConnect.ChannelNews);
				dbConnect.SaveChanges();

			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());
			}

		}
		public void SetAsRead(int ID)
		{
			try
			{


				channelNews = new List<ChannelNews>();

				channelNews = dbConnect.ChannelNews.ToList();

				ChannelNews ChannelNewsEdit = new ChannelNews();
				int Index = 0;
				for (int i = 0; i < channelNews.Count; i++)
				{
					if (ID == channelNews[i].id)
					{
						ChannelNewsEdit = channelNews[i];
						Index = i;
					}
				}
				ChannelNewsEdit.StatusView = 1;
				channelNews.Insert(Index, ChannelNewsEdit);
				dbConnect.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

using TESTWebApi.Models;

namespace TESTWebApi
{
    public class ReturnData
    {
        List<ChannelNews> channelNews = new List<ChannelNews>();
        ChannelNews channelNews2;
        RSSChannel rssChannel;
        List<RSSChannel> rssChannels;
        UsersSubscribes usersSubscribes;
        List<UsersSubscribes> UsersSubscribes;
        private readonly dbConnect dbConnect;

        public ReturnData(dbConnect dbConnect1)
        {
            dbConnect = dbConnect1;
        }
        public List<RSSChannel> GetAllRSSSubscribes(int UserID)
        {

            UsersSubscribes = new List<UsersSubscribes>();
            rssChannels = new List<RSSChannel>();
            UsersSubscribes = dbConnect.UsersSubscribes.ToList();
            UsersSubscribes = (from u in UsersSubscribes where u.UsersID == UserID select u).ToList();

            List<int> ids = new List<int>();
            foreach (var item in UsersSubscribes)
            {
                ids.Add(item.Id);
            }

            rssChannels = dbConnect.RSSChannel.ToList();
            List<RSSChannel> rssRes = new List<RSSChannel>();
            for (int i = 0; i < ids.Count; i++)
            {
                for (int j = 0; j < rssChannels.Count; j++)
                {
                    if (rssChannels[j].id == ids[i])
                    {
                        rssRes.Add(rssChannels[j]);
                    }
                }

            }
            return rssRes;
        }
        public List<ChannelNews> GetAllUnreadNewsByDate(DateTime dateTime, int UserID)
        {
            rssChannels = new List<RSSChannel>();
            rssChannels = GetAllRSSSubscribes(UserID);//user rssChannels
            List<int> ids = new List<int>();
            foreach (var item in rssChannels)
            {
                ids.Add(item.id);
            }
            channelNews = dbConnect.ChannelNews.ToList();
            DateOnly NormalDate = DateOnly.FromDateTime(dateTime);

            List<ChannelNews> rssRes = new List<ChannelNews>();
            for (int i = 0; i < ids.Count; i++)
            {
                for (int j = 0; j < channelNews.Count; j++)
                {
                    if (channelNews[j].RSSChannelID == ids[i])
                    {
                        rssRes.Add(channelNews[j]);
                    }
                }
            }
            rssRes = (from a in rssRes where a.StatusView == 0 && a.DateAdd >= NormalDate select a).ToList();
            return rssRes;
        }
    }
}

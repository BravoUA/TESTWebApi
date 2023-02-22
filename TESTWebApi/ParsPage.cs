using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TESTWebApi.Models;
using System.Xml;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace TESTWebApi
{
	public class ParsPage 
	{

		/*string itemsRegex = @"<item>(.*)</item>";
		string titelRegex = @"<title>(.*)</title>";
		string LinkRegex = @"<link>(.*)</link>";
		string DescriptionRegex = @"<description>(.*)</description>";
		string PublDateRegex = @"<pubDate>(.*)</pubDate>";*/

		public List<ChannelNews> channelNews = new List<ChannelNews>();
		public ChannelNews channelNews2;
		public RSSChannel rssChannel;
		public List<RSSChannel> rssChannels;
		public UsersSubscribes usersSubscribes;
		public List<UsersSubscribes> UsersSubscribes;

		int DELAY_FOR_UPDATA = 20000;
		private readonly dbConnect dbConnect;

		private InsertData insertData;


		
		public ParsPage(dbConnect dbConnect1)
		{
			dbConnect = dbConnect1;
			insertData = new InsertData(dbConnect);
		}

		public void parsPage(string URL, int UsersID)
		{

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(URL);
			usersSubscribes = new UsersSubscribes();
			rssChannel = new RSSChannel();
			XmlNodeList xmlNodeList = xDoc.SelectNodes("//channel");
			foreach (XmlNode node in xmlNodeList)
			{
				XmlNode xmlNode = node.SelectSingleNode("title");
				rssChannel.NameChannel = xmlNode.InnerText;
				XmlNode xmlNode2 = node.SelectSingleNode("link");
				rssChannel.Link = xmlNode2.InnerText;
				DateOnly today = DateOnly.FromDateTime(DateTime.Now);
				rssChannel.DateSubscribe = today;
				rssChannel.LinkforUpdate = URL;
				rssChannel.State = 0;

				usersSubscribes.UsersID = UsersID;

			}
			xmlNodeList = xDoc.SelectNodes("//channel/item");
			foreach (XmlNode node in xmlNodeList)
			{
				channelNews2 = new ChannelNews();
				XmlNode xmlNode = node.SelectSingleNode("title");
				channelNews2.Title = xmlNode.InnerText;

				XmlNode xmlNode2 = node.SelectSingleNode("link");
				channelNews2.Link = xmlNode2.InnerText;

				XmlNode xmlNode3 = node.SelectSingleNode("description");
				channelNews2.Description = xmlNode3.InnerText;

				XmlNode xmlNode4 = node.SelectSingleNode("pubDate");
				channelNews2.PublDate = xmlNode4.InnerText;

				channelNews2.StatusView = 0;
				DateOnly today = DateOnly.FromDateTime(DateTime.Now);
				channelNews2.DateAdd = today;
				channelNews2.RSSChannelID = rssChannel.id;

				channelNews.Add(channelNews2);
			}
			usersSubscribes.RSSChannelID = rssChannel.id;
			
			insertData.AddRssChannel(rssChannel, usersSubscribes, channelNews);
		}
		public async Task parsUpdate(dbConnect dbConnect, CancellationToken CancellationToken)
		{
			while (!CancellationToken.IsCancellationRequested)
			{
				if (dbConnect != null && dbConnect.ChannelNews.ToList().Count > 0)
				{

					List<RSSChannel> rssChannelList = new List<RSSChannel>();
					List<UsersSubscribes> UsersSubscribes = new List<UsersSubscribes>();
					rssChannelList = dbConnect.RSSChannel.ToList();
					UsersSubscribes = dbConnect.UsersSubscribes.ToList();
					insertData = new InsertData(dbConnect);
					insertData.DeleteRssNews();
					//dbConnect.Database.ExecuteSql($"DELETE from ChannelNews");

					var rssChannelLists = (from m in rssChannelList where m.LinkforUpdate != null select m.LinkforUpdate).ToList();

					if (rssChannelLists.Count == 0) { }
					else
					{
						Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
						XmlDocument xDoc = new XmlDocument();
						for (int i = 0; i < rssChannelLists.Count; i++)
						{
							channelNews = new List<ChannelNews>();
							xDoc.Load(rssChannelLists[i]);
							XmlNodeList xmlNodeList;

							xmlNodeList = xDoc.SelectNodes("//channel/item");
							foreach (XmlNode node in xmlNodeList)
							{
								channelNews2 = new ChannelNews();
								XmlNode xmlNode = node.SelectSingleNode("title");
								channelNews2.Title = xmlNode.InnerText;

								XmlNode xmlNode2 = node.SelectSingleNode("link");
								channelNews2.Link = xmlNode2.InnerText;

								XmlNode xmlNode3 = node.SelectSingleNode("description");
								channelNews2.Description = xmlNode3.InnerText;

								XmlNode xmlNode4 = node.SelectSingleNode("pubDate");
								channelNews2.PublDate = xmlNode4.InnerText;

								channelNews2.StatusView = 0;
								DateOnly today = DateOnly.FromDateTime(DateTime.Now);
								channelNews2.DateAdd = today;
								channelNews2.RSSChannelID = rssChannelList[i].id;

								channelNews.Add(channelNews2);
							}

							insertData.UpdateRssNews(channelNews);
						}

					}
				}
				await Task.Delay(DELAY_FOR_UPDATA);
			}
		}

		
	}
}

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TESTWebApi.Models;
using System.Xml;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;

namespace TESTWebApi
{
	public class ParsPage : IParsPage
	{

		/*string itemsRegex = @"<item>(.*)</item>";
		string titelRegex = @"<title>(.*)</title>";
		string LinkRegex = @"<link>(.*)</link>";
		string DescriptionRegex = @"<description>(.*)</description>";
		string PublDateRegex = @"<pubDate>(.*)</pubDate>";*/

		List<ChannelNews> channelNews = new List<ChannelNews>();
		ChannelNews channelNews2;
		RSSChannel rssChannel;
		List<RSSChannel> rssChannels;
		UsersSubscribes usersSubscribes;
		List<UsersSubscribes> UsersSubscribes;


		private readonly dbConnect dbConnect;


		public ParsPage() { }
		public ParsPage(dbConnect dbConnect1)
		{
			dbConnect = dbConnect1;
		}


		public void parsPage(string URL, int UsersID)
		{

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(URL);

			XmlNodeList xmlNodeList = xDoc.SelectNodes("//channel");
			foreach (XmlNode node in xmlNodeList)
			{

				usersSubscribes = new UsersSubscribes();
				rssChannel = new RSSChannel();
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
			/*	Regex regex = new Regex(itemsRegex, RegexOptions.Singleline);
				GroupCollection groups;
				MatchCollection matches = regex.Matches(XmlCode);
				MatchCollection matches2;
				if (matches.Count > 0)
				{
					foreach (Match match in matches)
					{ 				
						regex = new Regex(titelRegex, RegexOptions.Singleline);
						matches2 = regex.Matches(match.ToString());
						foreach (Match match2 in matches2)
						{
							groups = match2.Groups;
							channelNews2.Title = groups[1].ToString();
						}
						regex = new Regex(LinkRegex, RegexOptions.Singleline);
						matches2 = regex.Matches(match.ToString());
						foreach (Match match2 in matches2)
						{
							groups = match2.Groups;
							channelNews2.Link = groups[1].ToString();
						}
						regex = new Regex(DescriptionRegex, RegexOptions.Singleline);
						matches2 = regex.Matches(match.ToString());
						foreach (Match match2 in matches2)
						{
							groups = match2.Groups;
							channelNews2.Description = groups[1].ToString();
						}
						regex = new Regex(PublDateRegex, RegexOptions.Singleline);
						matches2 = regex.Matches(match.ToString());
						foreach (Match match2 in matches2)
						{
							groups = match2.Groups;
							channelNews2.PublDate = groups[1].ToString();
						}

						channelNews.Add(channelNews2);
					}
				}
				else
				{
					Console.WriteLine("No matches found");
				}

				*/
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
					dbConnect.Database.ExecuteSql($"DELETE from ChannelNews");




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

							for (int j = 0; j < channelNews.Count; j++)
							{
								dbConnect.ChannelNews.Add(channelNews[j]);
							}

							dbConnect.SaveChanges();
						}

					}
				}

				await Task.Delay(25000);
			}
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
			rssRes = (from a in rssRes where a.StatusView == 0 && a.DateAdd == NormalDate select a).ToList();
			return rssRes;
		}



		public void SetAsRead(int ID)
		{

			try
			{
				string sqlQ = "UPDATE ChannelNews SET StatusView = 1 WHERE id == " + ID;
				dbConnect.Database.ExecuteSqlRaw(sqlQ);
				dbConnect.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

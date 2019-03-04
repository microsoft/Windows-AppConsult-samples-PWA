using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed.Rss;
using StackExchange.Redis;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("redis");
            var db = connection.GetDatabase();

            List<string> news = new List<string>();
            string rss = string.Empty;
            rss = await db.StringGetAsync("feedRss");
            if (string.IsNullOrEmpty(rss))
            {
                HttpClient client = new HttpClient();
                rss = await client.GetStringAsync("https://techcommunity.microsoft.com/gxcuf89792/rss/board?board.id=WindowsDevAppConsult");
                await db.StringSetAsync("feedRss", rss);
            }
            else
            {
                news.Add("The RSS has been returned from the Redis cache");
            }

            
            using (var xmlReader = XmlReader.Create(new StringReader(rss), new XmlReaderSettings { Async = true }))
            {
                RssFeedReader feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == Microsoft.SyndicationFeed.SyndicationElementType.Item)
                    {
                        var item = await feedReader.ReadItem();
                        news.Add(item.Title);
                    }
                }
            }

            return news;
        } 
    }
}

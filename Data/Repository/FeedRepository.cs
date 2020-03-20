using Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace Data.Repository
{
    public class FeedRepository : IFeedRepository
    {
        const string adressFeed = "https://www.minutoseguros.com.br/blog/feed";

        public SyndicationFeed LoadFeed()
        {

            XmlReader reader = XmlReader.Create(adressFeed);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            return feed;
        }

    }
}


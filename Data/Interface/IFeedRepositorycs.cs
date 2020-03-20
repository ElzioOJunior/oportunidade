using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;

namespace Data.Interface
{
    public interface IFeedRepository
    {
         SyndicationFeed LoadFeed();
    }
}

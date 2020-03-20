using Domain.Dto;
using Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;

namespace Domain.Interface
{
    public interface IFeedDomain
    {
        List<FeedDto> LoadFeed();

        List<Word> SearchWordsByTopic(List<FeedDto> listaFeedDto);

        List<Word> SearchWordsAllTopics(List<FeedDto> listaFeedDto);
    }
}

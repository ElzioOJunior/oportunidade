using Data.Interface;
using Domain.Dto;
using Domain.Interface;
using Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain
{

    public class FeedDomain : IFeedDomain
    {
        private  readonly IFeedRepository _iFeedRepository;

        #region constructor
        public FeedDomain(IFeedRepository iFeedRepository)
        {
            _iFeedRepository = iFeedRepository;
        }

        #endregion

        #region public

        public List<Word> SearchWordsByTopic(List<FeedDto> listaFeedDto)
        {
            var listWord = new List<Word>();

            foreach (FeedDto feed in listaFeedDto)
            {
                var word = FindOcurrences(feed.text, feed.title, false);
                listWord.AddRange(word);
            }

            return listWord;
        }

        public List<Word> SearchWordsAllTopics(List<FeedDto> listaFeedDto)
        {
            var listWord = new List<Word>();

            foreach (FeedDto feed in listaFeedDto)
            {
                var word = FindOcurrences(feed.text, feed.title, true);
                listWord.AddRange(word);
            }

            return listWord.GroupBy(x => x.word)
                                                 .Select(
                                                     g => new Word()
                                                     {
                                                         topic = string.Empty,
                                                         word = g.Key,
                                                         qtd = g.Sum(s => s.qtd),
                                                     })
                                                 .OrderByDescending(c => c.qtd).Take(10).ToList();
        }

        public List<FeedDto> LoadFeed()
        {

            SyndicationFeed feed = _iFeedRepository.LoadFeed();
            var listaFeedDto = new List<FeedDto>();

            foreach (SyndicationItem item in feed.Items)
            {
                listaFeedDto.Add(BindingFeed(item));
            }

            return listaFeedDto;

        }
        #endregion

        #region private
        private List<Word> FindOcurrences(string topic, string titleTopic, bool groupTopics)
        {

            string[] words = topic.Split(' ').Where(c=> c.Length>3).ToArray();

            var listWords = new List<Word>();
            foreach (string word in words)
            {
                var newWord = BindingNewWord(word, topic, titleTopic);

                listWords.Add(newWord);

                topic = RemoveWordRepetitions(word, topic); 
            }
            if (!groupTopics)
            {
                return listWords.OrderByDescending(c => c.qtd).Take(10).ToList();
            }
            else
            {
                return listWords.OrderByDescending(c => c.qtd).ToList();
            }
        }

        private int FindWordOccurrences(string word, string topic)
        {
            int count = 0;
            foreach (string s in Regex.Split(topic, @"\W+").Where(c => c.Length > 3).ToArray())
            {
                if (s == word)
                {
                    count++;
                }
            }

            return count;
        }

        private string RemoveWordRepetitions(string word, string topic)
        {
            return Regex.Replace(topic, word, string.Empty);
        }

        private FeedDto BindingFeed(SyndicationItem item)
        {
            var feedDto = new FeedDto();

            feedDto.title = item.Title.Text;
            feedDto.text = Regex.Replace(item.Summary.Text, @"<[^>]*>", String.Empty);
            feedDto.id = Regex.Match(item.Id, @"\d+").Value;

            return feedDto;
        }


        private Word BindingNewWord(string word, string topic, string titleTopic)
        {
            var newWord = new Word();
            newWord.word = word;
            newWord.qtd = FindWordOccurrences(word, topic);
            newWord.topic = titleTopic;
            return newWord;
        }
    }
    #endregion

}


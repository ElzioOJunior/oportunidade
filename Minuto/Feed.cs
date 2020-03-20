using Data.Interface;
using Data.Repository;
using Domain;
using Domain.Dto;
using Domain.Interface;
using Domain.ValueObject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    class AppFeed 
    {
        static void Main(string[] args)
        {
            //IOC
            var serviceFeed = new ServiceCollection()
                .AddSingleton<IFeedDomain, FeedDomain>()
                .AddSingleton<IFeedRepository, FeedRepository>()
                .BuildServiceProvider();
            var feed = serviceFeed.GetService<IFeedDomain>();
            
            var listFeedDto = feed.LoadFeed();

            var listWordsByTopic = feed.SearchWordsByTopic(listFeedDto);

            Console.WriteLine(" ----******************** PALAVRAS MAIS USADAS POR TÓPICOS ********************----");

            string nometopico = string.Empty;

            foreach (Word word in listWordsByTopic)
            {
                if (!nometopico.Equals(word.topic))
                {
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine(String.Concat(" Tópico ", word.topic));
                }

                Console.WriteLine(String.Concat(" Qtd: ", word.qtd, " Palavra: ", word.word));

                nometopico = word.topic;
            }

            Console.WriteLine(" ----******************** PALAVRAS MAIS USADAS NO GERAL ********************----");

            var listWordsAllTopics = feed.SearchWordsAllTopics(listFeedDto);

            foreach (Word word in listWordsAllTopics)
            {
                Console.WriteLine(String.Concat(" Qtd: ", word.qtd, " Palavra: ", word.word));
            }

            Console.ReadKey();

        }

    }
}

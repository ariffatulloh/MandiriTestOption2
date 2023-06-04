using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Models.Responses;
using Refit;

namespace News.Interfaces
{
    /// <summary>
    /// News API.
    /// </summary>
    public interface INewsApi
    {
        /// <summary>
        /// Gets the news.
        /// </summary>
        /// <returns>The news.</returns>
        /// <param name="source">Source.</param>
        /// <param name="apikey">Apikey.</param>
        [Get("/v2/top-headlines?country={country}&category={categorySelected}&apiKey={apikey}")]
        [Headers("User-Agent: News-API-csharp/0.1")]
        Task<NewsResponse> GetNews(string country, string categorySelected, string apikey);

        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <returns>The sources.</returns>
        /// <param name="apikey">Apikey.</param>
        [Get("/v2/top-headlines/sources?apiKey={apikey}")]
        [Headers("User-Agent: News-API-csharp/0.1")]
        Task<SourceResponse> GetSources(string apikey);

        [Get("/v2/top-headlines/sources?apiKey={apikey}")]
        [Headers("User-Agent: News-API-csharp/0.1")]
        Task<SourceResponse> GetNewsBySearch(string apikey);
    }
}
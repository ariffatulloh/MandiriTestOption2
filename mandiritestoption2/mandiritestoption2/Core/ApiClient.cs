using System;
using System.Diagnostics;
using System.Threading.Tasks;
using News.Code;
using News.Interfaces;
using News.Models.Responses;
using Refit;
using Xamarin.Essentials;

namespace Cimbtestoption2.Core
{
	public class ApiClient
	{
        public async Task<NewsResponse> GetNews(String countrySelected,string categorySelected, string apikey)
        {
            NewsResponse rtn = new NewsResponse();

            try
            {
                var client = RestService.For<INewsApi>(Constants.UrlNews);
                
                rtn = await client.GetNews(countrySelected, categorySelected, apikey);
                
            }
            catch (ApiException ax)
            {
                Debug.WriteLine("Api error: " + ax.Message+ax.Uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Generic error: " + ex.Message );
            }

            return rtn;
        }
        public async Task<SourceResponse> GetSources(string apikey)
        {
            SourceResponse rtn = new SourceResponse();

            try
            {
                var client = RestService.For<INewsApi>(Constants.UrlNews);
                rtn = await client.GetSources(apikey);
            }
            catch (ApiException ax)
            {
                Debug.WriteLine("Api error: " + ax.Message+" "+ax.Uri+ax.Content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Generic error: " + ex.Message + ex.Data);
            }

            return rtn;
        }
        //public async Task<SourceResponse> GetSourcesBySearch(string search, string apikey)
        //{
        //    SourceResponse rtn = new SourceResponse();

        //    try
        //    {
        //        var client = RestService.For<INewsApi>(Constants.UrlNews);
        //        rtn = await client.GetSourcesBySearch(search,apikey);
        //    }
        //    catch (ApiException ax)
        //    {
        //        Debug.WriteLine("Api error: " + ax.Message + " " + ax.Uri + ax.Content);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Generic error: " + ex.Message + ex.Data);
        //    }

        //    return rtn;
        ////}
    }
}


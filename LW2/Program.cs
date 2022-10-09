using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace LW2
{
    class Program
    {
        static bool IsPageExists(string address, ref string htmlCode, out HttpStatusCode statusCode)
        {
            HttpWebResponse response;
            HttpWebRequest request;
            try
            {
                WebClient client = new WebClient();
                htmlCode = client.DownloadString(address);
            }
            catch (WebException ex)
            {
                    response = (HttpWebResponse)ex.Response;
                    statusCode = response.StatusCode;
                    return false;
            }

            request = (HttpWebRequest)WebRequest.Create(address);
            response = (HttpWebResponse)request.GetResponse();
            statusCode = response.StatusCode;

            return true;
        }

        static async void ParseUrl(string url, StreamWriter outFileOne, StreamWriter outFileTwo)
        {
            //Получаем весь html-код страницы и её статус.
            string htmlCode = "";
            HttpStatusCode statusCode;
            bool isExists = IsPageExists(url, ref htmlCode, out statusCode);

            if (isExists)
            {
                validCount++;
                outFileOne.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);

                //Чтобы настроить AngleSharp, мы определяем BrowsingContext, которому передаем Configuration.
                using var context = BrowsingContext.New(Configuration.Default);

                //Документ загружается с помощью OpenAsync.
                using var doc = await context.OpenAsync(req => req.Content(htmlCode));
                var links = doc.QuerySelectorAll<IHtmlAnchorElement>(selector);

                foreach (IHtmlAnchorElement element in links)
                {
                    if (!element.GetAttribute(attribute)?.StartsWith("http") ?? false)
                    {
                        element.Href = baseUrl + element.GetAttribute(attribute);
                    }
                    if ((element.GetAttribute(attribute)?.StartsWith(baseUrl) ?? true)
                        && ((element.GetAttribute(attribute) != null))
                        && (hrefTags.Add(element.GetAttribute(attribute))))
                    {
                        ParseUrl(hrefTags.Last(), outFileOne, outFileTwo);
                    }
                }
            }
            else
            {
                inValidCount++;
                outFileTwo.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);
            }
        }

        const string outputValid = @"..\..\..\OutputValid.txt";
        const string outputInvalid = @"..\..\..\OutputInvalid.txt";
        const string selector = "a";
        const string attribute = "href";
        const string baseUrl = "http://links.qatl.ru/";
        static HashSet<string> hrefTags = new HashSet<string>();
        static int validCount = 0;
        static int inValidCount = 0;

        static void Main(string[] args)
        {
            using StreamWriter swValid = new StreamWriter(outputValid);
            using StreamWriter swInvalid = new StreamWriter(outputInvalid);
            string url = baseUrl;

            ParseUrl(url, swValid, swInvalid);

            swValid.WriteLine("Links number: " + validCount);
            swValid.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            swInvalid.WriteLine("Links number: " + inValidCount);
            swInvalid.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
        }
    }
}

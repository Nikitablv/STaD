using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace LW2
{
    class Program
    {
        private static bool IsCorrectLink(ref IHtmlAnchorElement element)
        {
            return (element.GetAttribute(attribute)?.StartsWith(baseUrl) ?? false)
                && !(element.GetAttribute(attribute).Contains("tel:"))
                && !(element.GetAttribute(attribute).Contains("mail"))
                && !(element.GetAttribute(attribute).Contains("tg:"))
                && !(element.GetAttribute(attribute).Contains("@"))
                && !(element.GetAttribute(attribute).Contains(".mp4"))
               /* && !(element.GetAttribute(attribute).Contains("#"))*/
                && !(element.GetAttribute(attribute).Contains("?"));
        }
        private static void CorrectLink(string url, ref IHtmlAnchorElement element)
        {
            if ((!element.GetAttribute(attribute)?.StartsWith("http") ?? true) 
                && (!element.GetAttribute(attribute)?.StartsWith("/") ?? true)
                && (!element.GetAttribute(attribute)?.StartsWith(" ") ?? true)
                /*&& (!element.GetAttribute(attribute)?.StartsWith("#") ?? true)*/
                && (!element.GetAttribute(attribute)?.StartsWith("?") ?? true))
            {
                element.Href = url + element.GetAttribute(attribute);
            }
            if (element.GetAttribute(attribute)?.StartsWith("/") ?? false)
            {
                if ((!element.GetAttribute(attribute)?.StartsWith("/www.") ?? true)
                    && (!element.GetAttribute(attribute)?.StartsWith("//") ?? true))
                {
                    element.Href = baseUrlWithoutSlash + element.GetAttribute(attribute);
                }
            }
        }
        static bool IsPageExists(string address, ref string htmlCode)
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

            if (IsPageExists(url, ref htmlCode))
            {
                i++;
                validCount++;
                outFileOne.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);
                Console.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);

                //Чтобы настроить AngleSharp, мы определяем BrowsingContext, которому передаем Configuration.
                using var context = BrowsingContext.New(Configuration.Default);

                //Документ загружается с помощью OpenAsync.
                using var doc = await context.OpenAsync(req => req.Content(htmlCode));
                var links = doc.QuerySelectorAll<IHtmlAnchorElement>(selector);
                htmlCode = "";
                foreach (IHtmlAnchorElement element in links)
                {
                    IHtmlAnchorElement funcElement = element;
                    CorrectLink(url, ref funcElement);
                    if ((IsCorrectLink(ref funcElement)) && (hrefTags.Add(funcElement.GetAttribute(attribute))))
                    {
                        hrefLinks.Add(funcElement.GetAttribute(attribute));
                        //ParseUrl(hrefTags.Last(), outFileOne, outFileTwo);
                    }
                }
                ParseUrl(hrefLinks[i], outFileOne, outFileTwo);
            }
            else
            {
                inValidCount++;
                outFileTwo.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);
                Console.WriteLine(url + ": " + "{0} - {1}", (int)statusCode, statusCode);
                i++;
                ParseUrl(hrefLinks[i], outFileOne, outFileTwo);
            }
        }

        static int i = 0;
        const string outputValid = @"..\..\..\OutputValid.txt";
        const string outputInvalid = @"..\..\..\OutputInvalid.txt";
        const string selector = "a";
        const string attribute = "href";
        const string baseUrl = "http://links.qatl.ru/";
        const string baseUrlWithoutSlash = "http://links.qatl.ru";
        static HashSet<string> hrefTags = new HashSet<string>();
        static List<string> hrefLinks = hrefTags.ToList();
        static int validCount = 0;
        static int inValidCount = 0;
        static HttpStatusCode statusCode;

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            //засекаем время начала операции
            stopwatch.Start();

            using StreamWriter swValid = new StreamWriter(outputValid);
            using StreamWriter swInvalid = new StreamWriter(outputInvalid);
            string url = baseUrl;
            hrefTags.Add(url);

            ParseUrl(url, swValid, swInvalid);

            //останавливаем счётчик
            stopwatch.Stop();

            swValid.WriteLine("Links number: " + validCount);
            swValid.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            swValid.WriteLine("Program runtime: " + stopwatch.ElapsedMilliseconds / 1000 + " seconds");
            swInvalid.WriteLine("Links number: " + inValidCount);
            swInvalid.WriteLine(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            swInvalid.WriteLine("Program runtime: " + stopwatch.ElapsedMilliseconds / 1000 + " seconds");
        }
    }
}

using AngleSharp.Common;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Test.Models;

namespace Test.Data
{
    public class TextParser
    {
        private string HotelUrl = "https://www.hotels.ru/rus/hotels/russia/groznyy/hotels_list.htm";
        private string RestaurantUrl = "https://restaurantguru.ru/Grozny";
        private string SightUrl = "https://susanintop.com/evropa/rossija/chechnja/groznyj";

        private string HotelItemSelector = "grid";
        private string RestourantItemSelector = "restaurant_row show words_review_link  with_snippet   ";
        private string SightItemSelector = "sight sight-item";

        private string HotelNameSelector = "link hotel-name";
        private string RestourantNameSelector = "notranslate title_url";
        private string SightNameSelector = "sight sight-item";



        private HttpClient _client = new HttpClient();
        private HtmlParser _parser = new HtmlParser();

        private Infrastructure result = new Infrastructure();

        public Infrastructure ParseDocument()
        {
            Task a = LoadTextAsync(result);

            Task.WaitAll(a);

            return result;
        }

        private async Task LoadTextAsync(Infrastructure infrastructure)
        {
            HtmlParser htmlParser = new HtmlParser();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(HotelUrl);
                Task<string> content = response.Content.ReadAsStringAsync();
                content.Wait();

                IHtmlDocument document = htmlParser.ParseDocument(content.Result);

                infrastructure.Hotel = ParseHotel(document).ToList<Organization>();
            }
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(RestaurantUrl);
                Task<string> content = response.Content.ReadAsStringAsync();
                content.Wait();

                IHtmlDocument document = htmlParser.ParseDocument(content.Result);

                infrastructure.Restaurant = ParseRestaurnat(document).ToList<Organization>();
            }
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(SightUrl);
                Task<string> content = response.Content.ReadAsStringAsync();
                content.Wait();

                IHtmlDocument document = htmlParser.ParseDocument(content.Result);

                infrastructure.Restaurant = ParseSight(document).ToList<Organization>();
            }
        }


        private Organization[] ParseSight(IHtmlDocument document)
        {
            string hotelImageUrl = "load gallery-popup-wrapper hotel-popup-trigger";
            string hotelRatingClass = "rating";

            int itemCount = document.GetElementsByClassName(SightItemSelector).Length;

            List<string> Names = new List<string>();
            List<string> Urls = new List<string>();
            List<string> Ratings = new List<string>();

            Organization[] result = new Organization[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                result[i] = new Organization();
            }


            foreach (var element in document.GetElementsByClassName(SightNameSelector))
            {
                foreach (var h_first in element.Children)
                {
                    if (h_first.ChildElementCount > 2)
                        break;

                    foreach (var child in h_first.Children)
                    {
                        if (child.TagName == "A")
                        {
                            Names.Add(child.InnerHtml);
                        }
                    }
                }
            }

            foreach (var element in document.GetElementsByTagName("IMG"))
            {
                if (element.Parent?.NodeName == "DIV")
                    Urls.Add(element.GetAttribute("src") ?? "");
            }

            for (int i = 0; i < itemCount; i++)
            {
                result[i].Name = Names[i];

                try
                {
                    result[i].ImageURL = Urls[i];
                }
                catch (Exception)
                {
                    result[i].ImageURL = "";
                }
            }

            return result;
        }

        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------
        //-----------------------------------------------

        private Organization[] ParseHotel(IHtmlDocument document)
        {
            string hotelImageUrl = "load gallery-popup-wrapper hotel-popup-trigger";
            string hotelRatingClass = "rating";

            int itemCount = document.GetElementsByClassName(HotelItemSelector).Length;

            List<string> Names = new List<string>();
            List<string> Urls = new List<string>();
            List<string> Ratings = new List<string>();


            Organization[] result = new Organization[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                result[i] = new Organization();
            }


            foreach (var element in document.GetElementsByClassName(HotelNameSelector))
            {
                Names.Add(element.InnerHtml);
            }

            foreach (var element in document.GetElementsByTagName("img"))
            {
                if (element.Parent?.NodeName == "DIV")
                    Urls.Add(element.GetAttribute("src") ?? "");
            }

            foreach (var element in document.GetElementsByClassName(hotelRatingClass))
            {
                Ratings.Add(element.InnerHtml);
            }

            for (int i = 0; i < itemCount; i++)
            {
                result[i].Name = Names[i];
                result[i].ImageURL = Urls[i];
                result[i].Rating = Ratings[i];
            }

            return result;
        }

        private Organization[] ParseRestaurnat(IHtmlDocument document)
        {
            int itemCount = document.GetElementsByClassName(RestourantItemSelector).Length;
            string ImageUrl = "restaurant_pic";


            List<string> Names = new List<string>();
            List<string> Urls = new List<string>();
            List<string> Ratings = new List<string>();

            Organization[] result = new Organization[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                result[i] = new Organization();
            }

            foreach (var element in document.GetElementsByClassName(RestourantNameSelector))
            {
                Names.Add(element.InnerHtml);
            }

            foreach (var element in document.GetElementsByClassName(ImageUrl))
            {
                foreach (var child in element.Children)
                {
                    if (child.TagName == "IMG")
                        Urls.Add(child.GetAttribute("src"));
                }
            }

            for (int i = 0; i < result.Length; i++)
            {

                result[i].Name = Names[i];
                result[i].ImageURL = Urls[i];
            }

            return result;
        }
    }
}

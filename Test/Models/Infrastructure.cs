using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;

namespace Test.Models
{
    public class Infrastructure
    {
        public List<Organization> Restaurant { get; set; } = new List<Organization>();
        public List<Organization> Hotel { get; set; } = new List<Organization>();
        public List<Organization> Sights { get; set; } = new List<Organization>();
    }
}

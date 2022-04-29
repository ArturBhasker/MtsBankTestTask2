using HtmlAgilityPack;

namespace MtsBankTestTask2.CounterServices
{
    internal class HtmlCounterService : IHtmlCounterService
    {
        public int CountTag(HtmlDocument htmlDocument, string htmlTag)
        {
            return htmlDocument
                .DocumentNode
                .SelectNodes(htmlTag)
                .Count;
        }
    }
}
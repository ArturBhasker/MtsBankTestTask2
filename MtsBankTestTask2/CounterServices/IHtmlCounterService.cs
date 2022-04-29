using HtmlAgilityPack;

namespace MtsBankTestTask2.CounterServices
{
    internal interface IHtmlCounterService
    {
        int CountTag(HtmlDocument htmlDocument, string htmlTag);
    }
}
namespace MtsBankTestTask2.CounterServices
{
    internal interface ICommonCounterService
    {
        int CountSymbol(TextReader cssTextReader, char symbol);
    }
}
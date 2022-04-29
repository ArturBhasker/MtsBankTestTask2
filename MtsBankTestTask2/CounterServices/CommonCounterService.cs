namespace MtsBankTestTask2.CounterServices
{
    internal class CommonCounterService : ICommonCounterService
    {
        public int CountSymbol(TextReader cssTextReader, char symbol)
        {
            int symbolCount = 0;

            while (true)
            {
                int integer = cssTextReader.Read();

                if (integer == -1)
                    break;

                char character = (char) integer;

                if (character == symbol)
                {
                    symbolCount++;
                }
            }

            return symbolCount;
        }
    }
}
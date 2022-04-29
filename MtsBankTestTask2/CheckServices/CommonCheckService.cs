namespace MtsBankTestTask2.CheckServices
{
    internal class CommonCheckService : ICommonCheckService
    {
        public bool CheckFigureScopes(TextReader cssTextReader)
        {
            int leftScope = 0;
            int rightScope = 0;

            while (true)
            {
                int integer = cssTextReader.Read();

                if (integer == -1)
                    break;

                char character = (char) integer;
                switch (character)
                {
                    case '{':
                        leftScope++;
                        break;
                    case '}':
                        rightScope++;
                        break;
                }
            }

            return leftScope == rightScope;
        }
    }
}
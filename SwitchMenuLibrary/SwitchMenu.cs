namespace SwitchMenuLibrary
{
    public class SwitchMenu
    {
        private IEnumerable<string> _cases;
        private delegate void menu();
        private event menu _up, _down;
        private string _question, _label, _currentLabel;
        private int _currentCase;
        public SwitchMenu(IEnumerable<string> cases, string question)
        {
            _label = "   ";
            _currentLabel = "-->";
            _cases = cases;
            _question = question;
            _up = new menu(Back);
            _down = new menu(Next);
        }

        public SwitchMenu(IEnumerable<string> cases, string question, string currentLabel, string label) : this(cases, question)
        {
            _label = label;
            _currentLabel = currentLabel;
        }

        public SwitchMenu(IEnumerable<string> cases, string question, string currentLabel) : this(cases, question)
        {
            _currentLabel = currentLabel;
        }
        public int ShowMenu()
        {
            ConsoleKeyInfo key;
            do
            {
                ShowCases();
                key = Console.ReadKey();
                Event(key);
                Console.Clear();
            } while (key.Key != ConsoleKey.Enter);
            return _currentCase;
        }
        private void Event(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow)
                _up();
            else if (key.Key == ConsoleKey.DownArrow)
                _down();
        }
        private void ShowCases()
        {
            if (_question != null && _question != "")
                Console.WriteLine(_question);
            for (int i = 0; i < _cases.Count(); i++)
                Console.WriteLine((_currentCase == i ? _currentLabel : _label) + _cases.ElementAt(i));
        }
        private void Next()
        {
            _currentCase++;
            if (_currentCase > _cases.Count() - 1)
                _currentCase -= _cases.Count();
        }
        private void Back()
        {
            _currentCase--;
            if (_currentCase < 0)
                _currentCase += _cases.Count();
        }
    }
}

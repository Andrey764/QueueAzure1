using SwitchMenuLibrary;

namespace ConsoleApp1
{
    [Serializable]
    public class Lot
    {
        public string ?Currency { get; set; }
        public string ?SellersSurname { get; set; }
        public double Amount { get; set; }
        public Lot()
        {
           
        }
        public void CreateLot()
        {
            CurrencySelection();
            SetAmount();
            SetSellersSurname();
        }
        private void CurrencySelection()
        {
            string[] currencys = { "USD", "EUR", "CAD", "CNY", "SEK" };
            int count = new SwitchMenu(currencys, "Выбирете валюту").ShowMenu();
            Currency = currencys[count];
        }
        private void SetAmount()
        {
            bool value;
            double amount;
            do
            {
                Console.Write("Укажите суму денег: ");
                value = double.TryParse(Console.ReadLine(), out amount);
                if (value)
                    Amount = amount;
                else
                    Console.WriteLine("Вы ввели некоректное значение");
                Console.Clear();
            } while (!value);
        }
        private void SetSellersSurname()
        {
            bool value;
            string surname;
            do
            {
                Console.Write("Укажите Фамилию продавца: ");
                surname = Console.ReadLine();
                value = surname is not null && surname != "";
                if (value)
                    SellersSurname = surname;
                else
                    Console.WriteLine("Вы ввели некоректное значение или оставили это поле пустым");
                Console.Clear();
            } while (!value);
        }
        public override string ToString()
        {
            return $"{Amount} {Currency} продавец: {SellersSurname}";
        }
    }
}

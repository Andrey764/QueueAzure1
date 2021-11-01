using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using SwitchMenuLibrary;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class Main
    {
        private const string _connection = "Строка подключения";
        private static QueueClient? _queue;
        private const int MaxLengthMessage = 10;
        public static async Task RootAsync()
        {
            int count;
            string[] arr = { "Виход", "Получить очередь", "Добавить очередь", "Удалить очередь",
                "Добавить сообщение", "Получить сообщение", "удалить сообщение" , "О преложении" };
            do
            {
                count = new SwitchMenu(arr, "Выбирете действие").ShowMenu();
                switch (count)
                {
                    case 1: { GetQueue(); } break;
                    case 2: { await CreateQueueAsync(); } break;
                    case 3: { await DeleteQueueAsync(); } break;
                    case 4: { await GenerateMessage(); } break;
                    case 5: { await PullOutMessage(); } break;
                    case 6: { await PullOutMessageAndRemove(); } break;
                    case 7: { Info(); } break;
                }
            } while (count != 0);

        }
        private static void Info()
        {
            Console.WriteLine("Данное приложение может работать только с одной очередью.");
            Console.WriteLine("Вытягивание очереди приводит к переходу на указанную очередь.");
            Console.WriteLine("При создание новой очериди вы начинаете работать с ней.");
        }
        private static async Task PullOutMessageAndRemove()
        {
            const string question = "Выбирете сообщение каторое хотите удалить";
            var messages = (await _queue.ReceiveMessagesAsync(MaxLengthMessage)).Value;
            var collection = messages.Select(message => JsonSerializer.Deserialize<Lot>(message.MessageText).ToString());
            int count = new SwitchMenu(collection, question).ShowMenu();
            await _queue.DeleteMessageAsync(messages[count].MessageId, messages[count].PopReceipt);
            Console.WriteLine("сообщение удаленно");
        }

        private static async Task PullOutMessage()
        {
            foreach (PeekedMessage message in (await _queue.PeekMessagesAsync(MaxLengthMessage)).Value)
            {
                var lot = JsonSerializer.Deserialize<Lot>(message.MessageText);
                Console.WriteLine(lot);
            }
        }

        private static async Task GenerateMessage()
        {
            var lot = new Lot();
            lot.CreateLot();
            string jsonString = JsonSerializer.Serialize(lot);
            await SendMessageAsync(jsonString);
        }

        private static void GetQueue()
        {
            QueueServiceClient serviceClient = new QueueServiceClient(_connection);
            Console.Write("Укажите название очереди: ");
            _queue = serviceClient.GetQueueClient(Console.ReadLine());
            Console.WriteLine("Очередь полученна");
        }
        private static async Task CreateQueueAsync()
        {
            QueueServiceClient serviceClient = new QueueServiceClient(_connection);
            Console.Write("Укажите название очереди: ");
            _queue = await serviceClient.CreateQueueAsync(Console.ReadLine());
            Console.WriteLine("Очередь создана");
        }

        private static bool QueueIsNotNull(string ErrorMessage)
        {
            if (_queue != null)
                return true;
            Console.WriteLine(ErrorMessage);
            return false;
        }
        private static async Task DeleteQueueAsync()
        {
            if (QueueIsNotNull("Очередь не существует"))
                await _queue.DeleteAsync();
            Console.WriteLine("Очередь удалена");
        }

        private static async Task SendMessageAsync(string message)
        {
            if (QueueIsNotNull("Очередь не существует, некуда добавлять сообщения"))
                await _queue.SendMessageAsync(message);
            Console.WriteLine("сообщение отправленно");
        }
    }
}

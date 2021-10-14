using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads
{
    class Program
    {
        public static List<Client> clients = new List<Client>();
        public static List<Client> balances = new List<Client>();
        static void Main(string[] args)
        {
            balances.AddRange(clients);
            Client client = new Client();
            string choice = "1";
            while (choice != "5")
            {
                Console.Clear();
                TimerCallback callback = new TimerCallback(ShowChangeInBalance);
                Timer timer = new Timer(callback, clients, 0, 1000);
                System.Console.WriteLine("1. Добавить клиента\n2. Изменить баланс клиента\n3. Удалить клиента\n4. Показать баланс клиента");
                choice = Console.ReadLine();
                int id = 0;
                switch (choice)
                {
                    case "1":
                        System.Console.Write("Введите Id: ");
                        id = int.Parse(Console.ReadLine());
                        System.Console.Write("Введите баланс: ");
                        decimal balance = decimal.Parse(Console.ReadLine());
                        Thread newInsertThread = new Thread(new ThreadStart(() => { client.Insert(id, balance); }));
                        newInsertThread.Start();
                        break;
                    case "2":
                        System.Console.WriteLine("Введите Id: ");
                        id = int.Parse(Console.ReadLine());
                        System.Console.WriteLine("Введите новую сумму: ");
                        decimal balancec = decimal.Parse(Console.ReadLine());
                        Thread newUpdateThread = new Thread(new ThreadStart(() => { client.Update(id, balancec); }));
                        newUpdateThread.Start();
                        break;
                    case "3":
                        System.Console.Write("Введите Id: ");
                        id = int.Parse(Console.ReadLine());
                        Thread newDeleteThread = new Thread(new ThreadStart(() => { client.Delete(id); }));
                        newDeleteThread.Start();
                        break;
                    case "4":
                        System.Console.WriteLine("Введите Id: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        Thread newSelectThread = new Thread(new ThreadStart(() => { client.Select(id); }));
                        newSelectThread.Start();
                        break;
                }
                Console.ReadKey();
            }
        }
        static void ShowChangeInBalance(object obj)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Balance != balances[i].Balance)
                {
                    Console.ForegroundColor = (balances[i].Balance <= clients[i].Balance) ? ConsoleColor.Green : ConsoleColor.Red;
                    string raznica = (balances[i].Balance <= clients[i].Balance) ? $"+{clients[i].Balance - balances[i].Balance}" : $"{clients[i].Balance - balances[i].Balance}";
                    System.Console.WriteLine($"Id клиента: {clients[i].Id}\nСтарый баланс: {balances[i].Balance}  \nИзмененный баланс: {clients[i].Balance}\nРазница: " + raznica);
                    balances[i].Balance = clients[i].Balance;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
    class Client
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public void Insert(int id, decimal balance)
        {
            this.Id = id;
            this.Balance = balance;
            Program.clients.Add(new Client() { Id = id, Balance = balance });
            Program.balances.Add(new Client() { Id = id, Balance = balance });
            System.Console.WriteLine("Успешно добавлено клиент с Id " + id);
            return;
        }
        public void Update(int id, decimal balance)
        {
            for (int i = 0; i < Program.clients.Count; i++)
            {
                if (id == Program.clients[i].Id)
                {
                    Program.clients[i].Balance = balance;
                    return;
                }
            }
        }
        public void Select(int id)
        {
            for (int i = 0; i < Program.clients.Count; i++)
            {
                if (id == Program.clients[i].Id)
                {
                    System.Console.WriteLine(Program.clients[i].Id);
                    System.Console.WriteLine(Program.clients[i].Balance);
                    return;
                }
            }
        }
        public void Delete(int id)
        {
            for (int i = 0; i < Program.clients.Count; i++)
            {
                if (id == Program.clients[i].Id)
                {
                    Program.clients.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace homework5
{
	class Program
	{
		static UdpClient client;
		const int serverPort = 2365;
		static IPAddress serverAddress = IPAddress.Loopback;
		static IPEndPoint serverEP;

		static void Main(string[] args)
		{
			client = new UdpClient();
			serverEP = new IPEndPoint(serverAddress, serverPort);
			bool isRuning = true;
			while (isRuning)
			{
				Console.WriteLine("Главное меню:");
				Console.WriteLine("Список команд:" + Environment.NewLine + "Сделать заказ - 1" + Environment.NewLine
					+ "Узнать статус заказа - 2" + Environment.NewLine + "Отменить заказ - 3" + Environment.NewLine
					+ "Очистить консоль - 9" + Environment.NewLine + "Закончить программу - 10");
				Console.WriteLine("Введите команду:");
				string command = Console.ReadLine();
				switch (command)
				{
					case "1":
						AddOrder();
						break;
					case "2":
						StatusOrder();
						break;
					case "3":
						CancelOrder();
						break;
					case "9":
						Console.Clear();
						break;
					case "10":
						isRuning = false;
						break;
					default:
						Console.WriteLine("Неопознаная команда");
						break;
				}
			}
		}

		public static void AddOrder()
		{
			Console.WriteLine("Выбрана команда добавления заказа, продолжить?");
			Console.Write("(для продолжения введите y (или любой другой символ, исключая n), для прекращения введите n)");
			if (Console.ReadLine() == "n") return;
			string numCommand = "1";
			Console.WriteLine("Введите параметры: ");
			Console.Write("Описание: ");
			string description = Console.ReadLine();
			string dataString = numCommand + "_" + description;
			Console.WriteLine(SendMessage(dataString));
		}

		public static void StatusOrder()
		{
			Console.WriteLine("Выбрана команда показа статуса заказа, продолжить?");
			Console.WriteLine("Потребуется номер заказа, если не знаете/не помните, используйте поиск");
			Console.Write("(для продолжения введите y (или любой другой символ, исключая n), для прекращения введите n)");
			if (Console.ReadLine() == "n") return;
			string numCommand = "2";
			Console.WriteLine("Введите параметры: ");
			Console.Write("Номер заказа: ");
			string numOerder = Console.ReadLine();
			string dataString = numCommand + "_" + numOerder;
			Console.WriteLine(SendMessage(dataString));
		}

		public static void CancelOrder()
		{
			Console.WriteLine("Выбрана команда отмены заказа, продолжить?");
			Console.Write("(для продолжения введите y (или любой другой символ, исключая n), для прекращения введите n)");
			if (Console.ReadLine() == "n") return;
			string numCommand = "3";
			Console.WriteLine("Введите параметры: ");
			Console.Write("Номер заказа: ");
			string numOerder = Console.ReadLine();
			string dataString = numCommand + "_" + numOerder;
			Console.WriteLine(SendMessage(dataString));
		}		

		public static async Task<string> SendMessage(string Message)
		{
			byte[] datagram = Encoding.UTF8.GetBytes(Message);
			await client.SendAsync(datagram, datagram.Length, serverEP);
			UdpReceiveResult answerDatagram = await client.ReceiveAsync();
			string text = Encoding.UTF8.GetString(answerDatagram.Buffer);
			return text;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace homework5Server
{
	class Program
	{
		static UdpClient client;
		const int port = 2365;
		static bool isRunning;
		static AtistContext context = new AtistContext();
		public static Queue<UdpReceiveResult> commandsList = new Queue<UdpReceiveResult>();

		static async Task Listener()
		{
			while (isRunning)
			{
				UdpReceiveResult datagram = await client.ReceiveAsync();
				commandsList.Enqueue(datagram);
			}
		}

		static async Task RunCommand()
		{
			while (isRunning)
			{
				UdpReceiveResult datagram;
				bool flag = commandsList.TryDequeue(out datagram);
				if (flag)
				{
					string text = Encoding.UTF8.GetString(datagram.Buffer);
					Console.WriteLine($"got \"{text}\" from client");
					string[] commandList = text.Split("_");
					string answer = await decryptCommand(commandList);
					byte[] answerDatagram = Encoding.UTF8.GetBytes(answer);
					await client.SendAsync(answerDatagram, answerDatagram.Length, datagram.RemoteEndPoint);
				}
			}
			context.Dispose();
		}


		static async Task Main(string[] args)
		{
			client = new UdpClient(port);
			Console.WriteLine("Server start");
			try
			{
				/*context.Database.EnsureDeleted();*/
				context.Database.EnsureCreated();
				SetDB();
				Console.WriteLine("DB connected");
			}
			catch
			{
				Console.WriteLine("Problem with connect DB");
			}
			isRunning = true;
			Task runTask = Listener();
			Task runTask2 = RunCommand();
			Console.ReadLine();
			isRunning = false;
			await runTask;
			await runTask2;
		}

		public static void SetDB()
		{
			context.SaveChanges();
		}

		public static async Task<string> decryptCommand(string[] commandArray)
		{
			string answer = "";
			if (commandArray[0] == "1")
			{
				/*Сделать заказ*/
				try
				{
					await Task.Delay(2500);
					ArtOrder newOrder = new ArtOrder
					{
						Description = commandArray[1]
					};
					context.Add(newOrder);
					context.SaveChanges();
					ArtOrder order = context.Orders.SingleOrDefault(u => u.Description == commandArray[1]);
					answer = "Your order is registered. Your order number:" + order.Id.ToString();

					//answer = "Answer for query 1";
				}
				catch
				{
					answer = "Error";
				}
			}
			else if (commandArray[0] == "2")
			{
				/*Узнать статус заказа*/
				try
				{
					await Task.Delay(3500);
					ArtOrder Order = context.Orders.SingleOrDefault(ord => ord.Id == Convert.ToInt32(commandArray[1]));

					answer = $"Order status is {Order.Done}";
				}
				catch
				{
					answer = "Error";
				}
			}
			else if (commandArray[0] == "3")
			{
				/*Отменить заказ*/
				try
				{
					await Task.Delay(3500);
					ArtOrder Order = context.Orders.SingleOrDefault(ord => ord.Id == Convert.ToInt32(commandArray[1]));
					Order.Done = ArtOrder.Status.rejected;
					await context.SaveChangesAsync();

					answer = $"Order status is {Order.Done}";
				}
				catch
				{
					answer = "Error";
				}
			}


			return answer;
		}
	}
}
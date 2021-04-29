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
		static List<UdpReceiveResult> commandList = new List<UdpReceiveResult>();

		static async Task Work()
		{
			while (isRunning)
			{
				UdpReceiveResult datagram = await client.ReceiveAsync();
				//AddCommandInQueue(datagram);
				string text = Encoding.UTF8.GetString(datagram.Buffer);
				Console.WriteLine($"got \"{text}\" from client");
				string[] commandList = text.Split("_");
				string answer = await decryptCommand(commandList);
				byte[] answerDatagram = Encoding.UTF8.GetBytes(answer);
				await client.SendAsync(answerDatagram, answerDatagram.Length, datagram.RemoteEndPoint);
			}
			context.Dispose();
		}

		static async Task RunCommand()
		{
			/*while(isRunning)
			{
				UdpReceiveResult datagram = GetCommandFromQueue();
				string text = Encoding.UTF8.GetString(datagram.Buffer);
				Console.WriteLine($"got \"{text}\" from client");
				string[] commandList = text.Split("_");
				string answer = await decryptCommand(commandList);
				byte[] answerDatagram = Encoding.UTF8.GetBytes(answer);
				await client.SendAsync(answerDatagram, answerDatagram.Length, datagram.RemoteEndPoint);
			}
			context.Dispose();*/
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
			Task runTask = Work();
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

		public static void AddCommandInQueue(UdpReceiveResult datagram)
		{
			/* add command in queue */
			commandList.Add(datagram);
		}

		public static UdpReceiveResult GetCommandFromQueue()
		{
			UdpReceiveResult nextCommand;
			try 
			{
				nextCommand = commandList[0];
				ReducingTheQueue();
			}
			catch
			{
				Console.WriteLine("No commands in the queue");
			}
			return nextCommand;
		}

		public static void ReducingTheQueue()
		{
			for(int i = 1; i<commandList.Count; i++)
			{
				commandList[i - 1] = commandList[i];
			}
			commandList.Remove(commandList[commandList.Count - 1]);
		}

		public static async Task<string> decryptCommand(string[] commandArray)
		{
			string answer = "";
			if (commandArray[0] == "1")
			{
				/*Сделать заказ*/
				try
				{
					ArtOrder newOrder = new ArtOrder
					{
						Description = commandArray[1]
					};
					context.Add(newOrder);
					context.SaveChanges();
					ArtOrder order = context.Orders.SingleOrDefault(u => u.Description == commandArray[1]);
					answer = "Your order is registered. Your order number:" + order.Id.ToString();
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

					answer = "Order number: " + "Status: ";
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
					context.SaveChanges();

					answer = "Order number: " + "Status: ";
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

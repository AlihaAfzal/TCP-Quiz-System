using System;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

class QuizClient
{
    static void Main()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            NetworkStream stream = client.GetStream();

            Console.Write("Enter your name to join: ");
            string name = Console.ReadLine();
            SendMessage(stream, name);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string status = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (status != "ACCEPTED")
            {
                Console.WriteLine("Connection rejected by server.");
                return;
            }

            Console.WriteLine("Connected! Wait for questions...\n");

            while (true)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (data.StartsWith("END"))
                {
                    Console.WriteLine("\n" + data.Split('|')[1]);
                    break;
                }

                string[] q = data.Split('|');
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"QUESTION: {q[0]}");
                Console.WriteLine($"{q[1]} | {q[2]} | {q[3]} | {q[4]}");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("You have 30 seconds. Type Option (A, B, C, or D): ");

                Stopwatch timer = Stopwatch.StartNew();
                string answer = "";

                // Wait for input or 30s timeout
                while (timer.Elapsed.TotalSeconds < 30)
                {
                    if (Console.KeyAvailable)
                    {
                        answer = Console.ReadLine();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(answer))
                {
                    SendMessage(stream, "TIMEOUT");
                }
                else
                {
                    SendMessage(stream, answer);
                }

                // Receive feedback
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine("FEEDBACK: " + Encoding.UTF8.GetString(buffer, 0, bytesRead) + "\n");
            }
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void SendMessage(NetworkStream stream, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }
}
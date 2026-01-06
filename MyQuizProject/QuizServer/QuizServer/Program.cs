using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class QuizServer
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("--- Quiz Server Started ---");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            // Handle multiple clients using threading
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        // Receive Client Name
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string clientName = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        Console.WriteLine($"\n[Request] Client '{clientName}' wants to join. Accept? (y/n): ");
        string choice = Console.ReadLine();

        if (choice?.ToLower() == "y")
        {
            SendMessage(stream, "ACCEPTED");
            StartQuiz(stream);
        }
        else
        {
            SendMessage(stream, "REJECTED");
            client.Close();
        }
    }

    static void StartQuiz(NetworkStream stream)
    {
        // Question format: { Question, Opt A, Opt B, Opt C, Opt D, Correct Answer }
        string[,] questions = {
            { "What is the capital of Pakistan?", "A) Karachi", "B) Lahore", "C) Islamabad", "D) Quetta", "C" },
            { "Which language is used for C# development?", "A) Java", "B) C#", "C) Python", "D) Swift", "B" },
            { "Who is the founder of Microsoft?", "A) Steve Jobs", "B) Bill Gates", "C) Elon Musk", "D) Mark Zuckerberg", "B" },
            { "What is 5 + 7?", "A) 10", "B) 11", "C) 12", "D) 13", "C" },
            { "Which planet is known as the Red Planet?", "A) Earth", "B) Venus", "C) Mars", "D) Jupiter", "C" },
            { "What is the boiling point of water?", "A) 90C", "B) 80C", "C) 100C", "D) 120C", "C" },
            { "Largest ocean on Earth?", "A) Atlantic", "B) Indian", "C) Pacific", "D) Arctic", "C" },
            { "Symbol for Gold in Chemistry?", "A) Ag", "B) Au", "C) Pb", "D) Fe", "B" },
            { "Fastest land animal?", "A) Lion", "B) Tiger", "C) Cheetah", "D) Horse", "C" },
            { "Current year?", "A) 2024", "B) 2025", "C) 2026", "D) 2023", "C" }
        };

        int score = 0;
        for (int i = 0; i < questions.GetLength(0); i++)
        {
            // Send question and options
            string qData = $"{questions[i, 0]}|{questions[i, 1]}|{questions[i, 2]}|{questions[i, 3]}|{questions[i, 4]}";
            SendMessage(stream, qData);

            // Wait for answer
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string clientAnswer = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToUpper();

            if (clientAnswer == questions[i, 5])
            {
                score++;
                SendMessage(stream, "CORRECT! Great job.");
            }
            else if (clientAnswer == "TIMEOUT")
            {
                SendMessage(stream, $"TIME EXPIRED! The correct answer was {questions[i, 5]}.");
            }
            else
            {
                SendMessage(stream, $"WRONG! The correct answer was {questions[i, 5]}.");
            }
        }
        // Send final result
        SendMessage(stream, $"END|Final Result: {score}/10");
    }

    static void SendMessage(NetworkStream stream, string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }
}
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UDP_Server
{
    internal class Program
    {
        private static readonly string address = "127.0.0.1";

        static readonly int port = 8080;

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new(IPAddress.Parse(address), port);

            IPEndPoint? remoteEndPoint = null;

            UdpClient listener = new(ipPoint);

            Dictionary<string, string> dialogStr = new()
            {
                { "Hello","Hey !!!" },
                { "How are you?" , "I'm fine, thanks."},
                { "What's your name?" , "My name is John."},
                { "Where are you from?" , "I'm from New York."},
                { "What do you do for a living?" , "I'm a server."},
                { "What do you like?" , "I like to read and play sports."},
                { "Where are you going?" , "I'm going to the mall."},
                { "What time is it?" , "It's 10:00 AM."},
                { "How much does this cost?" , "It's $10."},
                { "Can you help me?" , "Sure, I'd be happy to help."},
                { "I'm lost." , "I can help you find your way."},
                { "I'm sorry." , "That's okay."},
                { "Thank you." , "You're welcome."},
                { "See you later." , "Bye." },
                { "Goodbye." , "Have a nice day."},
                { "Do you speak English?" , "Yes, I do."},
                { "Do you understand?" , "Yes, I understand." },
                { "Can you speak louder?" , "Sure, I can speak louder."},
                { "Can you repeat that?" , "Sure, I can repeat that." },
                { "I don't understand." , "I can explain it in a different way."},
                { "I'm not sure." , "Let's try to figure it out together."}

            };

            try
            {
                Console.WriteLine("Server started! Waiting for connection...");
                string message = string.Empty;
                while (true)
                {
                    byte[] data = listener.Receive(ref remoteEndPoint);
                    string msg = Encoding.Unicode.GetString(data);
                    Console.WriteLine($"Receive: {msg}");
                    if (dialogStr.ContainsKey(msg)) message = dialogStr[msg];
                    else message = "I do not understand you";
                    data = Encoding.Unicode.GetBytes(message);
                    listener.Send(data, data.Length, remoteEndPoint);
                    Console.WriteLine($"Send: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            listener.Close();
            listener.Dispose();
        }
    }
}
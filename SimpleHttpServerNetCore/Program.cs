using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace SimpleHttpServerNetCore
{
    class Program
    {
        static Socket httpServer;
        static int serverPort = 5004;
        public static void Main(string[] args)
        {
            httpServer = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Thread thread = new Thread(new ThreadStart(connectionThread));
            thread.Start();
            Console.WriteLine(@"Server Listening on:http:\\localhost:" + serverPort);
            void connectionThread()
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, serverPort);
                httpServer.Bind(endpoint);
                httpServer.Listen(1);
                listenServer();
            }
            void listenServer()
            {
                string data = "";
                byte[] bytes = new byte[2048];
                while (true)
                {
                    Socket client = httpServer.Accept();// Blocking Statement
                    int numBytes = client.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, numBytes);
                    if (data.IndexOf("\r\n") > -1)
                    {
                        //Console.WriteLine(data);
                        string path = data.Substring(5, 100).Split("HTTP")[0].ToString().Trim();
                        Console.WriteLine(path);
                        string resHeader = "HTTP/1.1 200 Life is Better\nServer:cirkinx\nContent-Type:text/html;charset:UTF-8\n\n";
                        string resBody = "<!DOCTYPE html><meta charset='utf-8'><title>Myserver</title><body>Test Server:" + DateTime.Now.ToString() + ": Request File:" + readFile(path) + "</body></html>";
                        string resStr = resHeader + resBody;

                        byte[] resData = Encoding.UTF8.GetBytes(resStr);
                        client.SendTo(resData, client.RemoteEndPoint);
                        client.Close();
                    }
                    Console.WriteLine(client.Connected.ToString() + "sadasf");
                }

            }
            static string readFile(string requestFileName)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + requestFileName;
                string readFile = "File Not Found";
                try
                {
                    using (var sr = new StreamReader(baseDirectory, Encoding.UTF8, true))
                    {
                        // Read the stream as a string, and write the string to the console.s
                        readFile = sr.ReadToEnd().ToString();
                    }
                    if (readFile.Length > 0)
                    {
                        return readFile;
                    }
                    else
                    {
                        return readFile;
                    }
                }
                catch (Exception)
                {

                    return readFile;
                }

            }
        }

    }
}

using System;
using System.Net.Sockets;
using System.Text;

namespace TCPClients
{
    class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        static void StartClient()
        {
            string serverIP = "127.0.0.1"; // Cambia esto por la dirección IP del servidor
            int serverPort = 5275; // Puerto donde el servidor TCP está escuchando

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(serverIP, serverPort);
                    Console.WriteLine("Conectado al servidor.");

                    NetworkStream stream = client.GetStream();

                    while (true)
                    {
                        Console.Write("Ingrese 'Hello' o 'Echo' para enviar al servidor: ");
                        string messageToSend = Console.ReadLine();

                        byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                        stream.Write(data, 0, data.Length);

                        byte[] responseBuffer = new byte[1024];
                        int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                        Console.WriteLine($"Respuesta del servidor: {response}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar con el servidor: {ex.Message}");
                }
            }
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BfdProtocolWithWebSocket
{
    internal static class TCPServer
    {
        internal static void Start(int port)
        {
            TcpListener server = null;
            try
            {
                // Iniciar el servidor TCP en el puerto especificado
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine($"Servidor TCP iniciado en el puerto {port}. Esperando conexiones entrantes...");

                // Ciclo infinito para aceptar conexiones entrantes
                while (true)
                {
                    // Aceptar una nueva conexión TCP entrante
                    TcpClient client = server.AcceptTcpClient();

                    // Crear un nuevo hilo para manejar la conexión con el cliente
                    Thread clientHandlerThread = new Thread(() => HandleTcpClient(client));
                    clientHandlerThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor TCP: {ex.Message}");
            }
            finally
            {
                server.Stop(); // Detener el servidor TCP cuando finaliza la ejecución
            }
        }

        private static void HandleTcpClient(TcpClient client)
        {
            try
            {
                // Obtener el flujo de red asociado con el cliente
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Leer datos del cliente mientras haya datos disponibles
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Convertir los datos recibidos en una cadena
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Mensaje recibido del cliente TCP: {dataReceived}");

                    // Procesar el mensaje recibido y obtener la respuesta
                    string response = ProcessReceivedMessage(dataReceived);
                    byte[] responseData = Encoding.UTF8.GetBytes(response);

                    try
                    {
                        // Intentar escribir la respuesta en el flujo de red para enviarla al cliente
                        stream.Write(responseData, 0, responseData.Length);
                    }
                    catch (IOException ex)
                    {
                        // Manejar la excepción causada por la conexión cerrada por el cliente
                        Console.WriteLine($"El cliente cerró la conexión: {ex.Message}");
                        break; // Salir del bucle mientras
                    }
                }

                // Cerrar la conexión con el cliente después de procesar todos los datos
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al manejar el cliente TCP: {ex.Message}");
            }
        }

        // Método para procesar el mensaje recibido y generar la respuesta correspondiente
        private static string ProcessReceivedMessage(string receivedMessage)
        {
            string response;

            // Verificar si el mensaje recibido es "Echo"
            if (receivedMessage.Trim().Equals("Echo", StringComparison.OrdinalIgnoreCase))
            {
                // Si es "Echo", generar la respuesta correspondiente
                response = "Echo received";
            }
            else
            {
                // Si no es "Echo", simplemente responder que el mensaje fue recibido
                response = $"{receivedMessage} received";
            }

            return response;
        }
    }
}

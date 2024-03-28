using BfdProtocolWithWebSocket; // Importar el espacio de nombres necesario
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BfdProtocolWithSockets
{
    public class BfdSocketManager
    {
        private const int bufferSize = 1024; // Tamaño del búfer de recepción
        private readonly Socket socket; // Socket para la comunicación
        private readonly byte[] buffer = new byte[bufferSize]; // Búfer para almacenar los datos recibidos
        private readonly BfdMessageHandler messageHandler; // Manejador de mensajes BFD
        private readonly BFDTimer inactivityTimer; // Temporizador para gestionar la inactividad de los vecinos

        // Constructor de la clase
        public BfdSocketManager(BfdMessageHandler messageHandler, TimeSpan inactivityTimeout)
        {
            this.messageHandler = messageHandler;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Crear un nuevo socket TCP
            inactivityTimer = new BFDTimer(inactivityTimeout, HandleInactivityTimeout); // Inicializar el temporizador de inactividad
        }

        // Método para iniciar la escucha en un puerto específico
        public void StartListening(int port)
        {
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, port)); // Asociar el socket a una dirección y puerto
                socket.Listen(10); // Comenzar a escuchar las conexiones entrantes con una cola de espera de tamaño 10

                // Comenzar a aceptar conexiones de forma asincrónica
                socket.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar el socket: {ex.Message}"); // Manejar cualquier excepción que ocurra al iniciar el socket
            }
        }

        // Método de devolución de llamada para aceptar conexiones entrantes
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Finalizar la operación asincrónica de aceptar una conexión
                Socket clientSocket = socket.EndAccept(ar);
                socket.BeginAccept(AcceptCallback, null); // Volver a comenzar a aceptar conexiones

                // Comenzar a recibir datos del cliente de forma asincrónica
                clientSocket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aceptar la conexión: {ex.Message}"); // Manejar cualquier excepción que ocurra al aceptar la conexión
            }
        }

        // Método de devolución de llamada para recibir datos del cliente
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState; // Obtener el socket del cliente
                int received = clientSocket.EndReceive(ar); // Finalizar la operación asincrónica de recibir datos

                if (received > 0)
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(buffer, dataBuffer, received);
                    string text = Encoding.ASCII.GetString(dataBuffer); // Convertir los datos recibidos en una cadena

                    // Mostrar el mensaje recibido en la consola
                    Console.WriteLine($"Mensaje recibido desde {clientSocket.RemoteEndPoint}: {text}");

                    // Reiniciar el temporizador de inactividad
                    inactivityTimer.Reset();

                    // Crear un objeto BfdMessage con el texto recibido
                    BfdMessage message = new BfdMessage(BfdMessage.MessageType.Hello, text);

                    // Manejar el mensaje entrante utilizando el manejador de mensajes
                    messageHandler.HandleIncomingMessage(message, ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString());
                }

                // Comenzar a recibir más datos del cliente de forma asincrónica
                clientSocket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recibir datos: {ex.Message}"); // Manejar cualquier excepción que ocurra al recibir datos
            }
        }

        // Método para manejar el tiempo de espera por inactividad del cliente
        private void HandleInactivityTimeout()
        {
            // Acciones a tomar cuando se detecta inactividad del cliente
            Console.WriteLine("Inactividad del cliente detectada. Desconectando...");
        }

        // Método para detener el temporizador de inactividad
        public void Stop()
        {
            inactivityTimer.Stop();
        }
    }
}

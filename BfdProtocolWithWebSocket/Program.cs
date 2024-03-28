using BfdProtocolWithSockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BfdProtocolWithWebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            const int bfdPort = 5274; // Puerto en el que escuchará el socket BFD
            const int tcpPort = 5275; // Puerto en el que escuchará el servidor TCP

            // Crear nodos BFD
            BfdNode localBfdNode = new BfdNode("192.168.0.1");
            BfdNeighbor remoteBfdNeighbor = new BfdNeighbor("192.168.0.2");

            // Crear un diccionario de vecinos con el vecino remoto
            Dictionary<string, BfdNeighbor> neighbors = new Dictionary<string, BfdNeighbor>();
            neighbors.Add(remoteBfdNeighbor.IPAddress, remoteBfdNeighbor);

            // Iniciar el socket manager y comenzar a escuchar en el puerto BFD
            TimeSpan tiempoInactividad = TimeSpan.FromMinutes(50);
            BfdMessageHandler messageHandler = new BfdMessageHandler(localBfdNode, neighbors, tiempoInactividad);
            BfdSocketManager socketManager = new BfdSocketManager(messageHandler, tiempoInactividad);
            socketManager.StartListening(bfdPort);


            // Crear ManualResetEvent para esperar a que el servidor TCP finalice
            ManualResetEvent tcpServerFinished = new ManualResetEvent(false);

            // Iniciar el servidor TCP en un hilo separado
            ThreadPool.QueueUserWorkItem(state =>
            {
                TCPServer.Start(tcpPort);
                tcpServerFinished.Set(); // Indicar que el servidor TCP ha finalizado
            });


            // Mostrar encabezado
            Console.WriteLine("=======================================================");
            Console.WriteLine("===           BFD Protocol con WebSocket            ===");
            Console.WriteLine("=======================================================");
            Console.WriteLine();

            // Información sobre el socket BFD
            Console.WriteLine($"Socket BFD iniciado. Escuchando en el puerto {bfdPort}.");

            // Información de la sesión BFD
            Console.WriteLine("=======================================================");
            Console.WriteLine($"Sesión BFD entre {localBfdNode.IPAddress} y {remoteBfdNeighbor.IPAddress} iniciada.");
            Console.WriteLine("=======================================================");
            Console.WriteLine();

            // Enviar el mensaje Hello
            Console.WriteLine("Enviando mensaje Hello...");
            messageHandler.SendHelloMessage();

            // Esperar un segundo para mostrar una separación en la consola
            Thread.Sleep(1000);

            // Enviar el mensaje Echo
            Console.WriteLine("Enviando mensaje Echo...");
            messageHandler.SendEchoMessage();

            // Esperar un segundo para mostrar una separación en la consola
            Thread.Sleep(1000);

            // Verificar la conectividad bidireccional esperando una respuesta Echo
            Console.WriteLine("Esperando respuesta Echo...");
            bool echoResponseReceived = messageHandler.WaitForEchoResponse(TimeSpan.FromMinutes(5)); // Esperar 5 minutos para la respuesta Echo

            if (echoResponseReceived)
            {
                Console.WriteLine("Respuesta Echo recibida. La conectividad bidireccional está operativa.");
            }
            else
            {
                Console.WriteLine("No se recibió respuesta Echo. Puede haber un problema en la conectividad bidireccional.");
            }
        }
    }
}

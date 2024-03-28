using System;
using System.Collections.Generic;

namespace BfdProtocolWithWebSocket
{
    public class BfdMessageHandler
    {
        private Dictionary<string, BfdNeighbor> neighbors;
        private BFDTimer tiempoInactividad;
        private BfdNode localBfdNode;

        // Propiedad para rastrear si el último mensaje recibido fue "Hello" o "Echo"
        public bool UltimomensajeRecibido { get; private set; }

        // Constructor
        public BfdMessageHandler(BfdNode localNode, Dictionary<string, BfdNeighbor> neighborList, TimeSpan inactivityTimeout)
        {
            localBfdNode = localNode;
            neighbors = neighborList;

            // Configurar el temporizador de inactividad
            tiempoInactividad = new BFDTimer(inactivityTimeout, HandleInactivityTimeout);
            tiempoInactividad.Start();
        }

        // Método para enviar un mensaje "Hello" a todos los vecinos
        public void SendHelloMessage()
        {
            foreach (var neighbor in neighbors.Values)
            {
                // Crear mensaje "Hello" con el contenido adecuado
                BfdMessage helloMessage = new BfdMessage(BfdMessage.MessageType.Hello, $"Hello from {localBfdNode.IPAddress} to {neighbor.IPAddress}");

                // Enviar el mensaje al vecino
                SendMessageToNeighborFromLocalNode(helloMessage, neighbor, localBfdNode);
                // Enviar el mensaje al vecino

            }
        }

        // Método para enviar un mensaje "Echo" a todos los vecinos
        public void SendEchoMessage()
        {
            foreach (var neighbor in neighbors.Values)
            {
                // Crear mensaje "Echo" con el contenido adecuado
                BfdMessage echoMessage = new BfdMessage(BfdMessage.MessageType.Echo, $"Echo from {localBfdNode.IPAddress} to {neighbor.IPAddress}");

                // Enviar el mensaje al vecino
                SendMessageToNeighborFromLocalNode(echoMessage, neighbor, localBfdNode);

            }
        }

        // Método para enviar un mensaje a un vecino específico

        private void SendMessageToNeighborFromLocalNode(BfdMessage message, BfdNeighbor neighbor, BfdNode localNode)
        {
            Console.WriteLine($"Enviando mensaje {message.Type} desde {localNode.IPAddress} a {neighbor.IPAddress}: {message.Content}");
        }


        // Método invocado cuando se detecta inactividad de un vecino
        private void HandleInactivityTimeout()
        {
            Console.WriteLine("Se detectó inactividad en un vecino.");
        }

        // Método para manejar un mensaje entrante
        public void HandleIncomingMessage(BfdMessage message, string ipAddress)
        {
            // Mostrar en consola el mensaje recibido
            Console.WriteLine($"Mensaje recibido desde {ipAddress}: {message.Content}");

            // Rastrear si el último mensaje recibido fue "Hello" o "Echo"
            UltimomensajeRecibido = message.Type == BfdMessage.MessageType.Hello || message.Type == BfdMessage.MessageType.Echo;

            // Mostrar en consola si el último mensaje fue "Hello" o "Echo"
            Console.WriteLine($"Último mensaje recibido fue {(UltimomensajeRecibido ? "Hello" : "Echo")}");

            // Implementar lógica adicional según el tipo de mensaje recibido
            switch (message.Type)
            {
                case BfdMessage.MessageType.Hello:
                    // Lógica para manejar un mensaje "Hello"
                    break;
                case BfdMessage.MessageType.Echo:
                    // Lógica para manejar un mensaje "Echo"
                    break;
                default:
                    Console.WriteLine("Tipo de mensaje no reconocido.");
                    break;
            }
        }

        public bool WaitForEchoResponse(TimeSpan timeout)
        {
            // Marcar el momento inicial
            DateTime startTime = DateTime.Now;

            // Esperar hasta que se reciba un mensaje Echo o se alcance el tiempo de espera
            while (DateTime.Now - startTime < timeout)
            {
                if (UltimomensajeRecibido && (DateTime.Now - startTime).TotalMilliseconds < timeout.TotalMilliseconds)
                {
                    return true; // Se recibió un mensaje Echo dentro del tiempo de espera
                }
            }

            return false; // No se recibió un mensaje Echo dentro del tiempo de espera
        }

    }
}
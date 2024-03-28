using System;

namespace BfdProtocolWithWebSocket
{
    public class BfdNode
    {
        // Propiedades de un nodo BFD
        public string IPAddress { get; private set; } // Dirección IP del nodo
        public bool IsUp { get; private set; } // Estado del nodo (activo o inactivo)
        public DateTime LastPacketReceivedTime { get; private set; } // Momento en que se recibió el último paquete desde el nodo

        // Constructor para inicializar un nodo BFD con una dirección IP
        public BfdNode(string ipAddress)
        {
            IPAddress = ipAddress; // Establecer la dirección IP
            IsUp = false; // Inicialmente el nodo se considera inactivo
            LastPacketReceivedTime = DateTime.MinValue; // Inicializar el tiempo del último paquete recibido como el mínimo valor posible
        }

        // Método para establecer el nodo como activo
        public void SetNodeUp()
        {
            IsUp = true; // Establecer el estado del nodo como activo
        }

        // Método para establecer el nodo como inactivo
        public void SetNodeDown()
        {
            IsUp = false; // Establecer el estado del nodo como inactivo
        }

        // Método para actualizar el tiempo del último paquete recibido desde el nodo
        public void UpdateLastPacketReceivedTime()
        {
            LastPacketReceivedTime = DateTime.Now; // Actualizar el tiempo del último paquete recibido como el tiempo actual
        }
    }
}

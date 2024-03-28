using System;

namespace BfdProtocolWithWebSocket
{
    public class BfdNeighbor
    {
        // Propiedades de un vecino BFD
        public string IPAddress { get; private set; } // Dirección IP del vecino
        public bool IsNeighborUp { get; private set; } // Estado del vecino (activo o inactivo)
        public DateTime LastPacketReceivedTime { get; private set; } // Momento en que se recibió el último paquete desde el vecino

        // Constructor para inicializar un vecino BFD con una dirección IP
        public BfdNeighbor(string ipAddress)
        {
            IPAddress = ipAddress; // Establecer la dirección IP
            IsNeighborUp = false; // Inicialmente el vecino se considera inactivo
            LastPacketReceivedTime = DateTime.MinValue; // Inicializar el tiempo del último paquete recibido como el mínimo valor posible
        }

        // Método para establecer el vecino como activo
        public void SetNeighborUp()
        {
            IsNeighborUp = true; // Establecer el estado del vecino como activo
        }

        // Método para establecer el vecino como inactivo
        public void SetNeighborDown()
        {
            IsNeighborUp = false; // Establecer el estado del vecino como inactivo
        }

        // Método para actualizar el tiempo del último paquete recibido desde el vecino
        public void UpdateLastPacketReceivedTime()
        {
            LastPacketReceivedTime = DateTime.Now; // Actualizar el tiempo del último paquete recibido como el tiempo actual
        }

        // Método ToString para obtener una representación de cadena del vecino
        public override string ToString()
        {
            return $"IP Address: {IPAddress}, Neighbor Up: {IsNeighborUp}, Last Packet Received Time: {LastPacketReceivedTime}";
        }
    }
}

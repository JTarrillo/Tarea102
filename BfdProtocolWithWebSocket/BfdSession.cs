using System;

namespace BfdProtocolWithWebSocket
{
    public class BfdSession
    {
        // Propiedades de la sesión BFD
        public string LocalNode { get; private set; } // Nodo local de la sesión
        public string RemoteNode { get; private set; } // Nodo remoto de la sesión
        public bool IsActive { get; private set; } // Estado de la sesión (activa o no)
        public TimeSpan DetectionTime { get; private set; } // Tiempo de detección de la sesión
        private BFDTimer detectionTimer; // Temporizador para la detección de enlaces

        // Constructor para inicializar la sesión BFD
        public BfdSession(string localNode, string remoteNode, TimeSpan detectionTime)
        {
            LocalNode = localNode;
            RemoteNode = remoteNode;
            IsActive = false; // Inicialmente la sesión está inactiva
            DetectionTime = detectionTime; // Establecer el tiempo de detección
            detectionTimer = new BFDTimer(detectionTime, HandleDetectionTimeout); // Inicializar el temporizador con la devolución de llamada para el tiempo de espera
        }

        // Método para iniciar la sesión BFD
        public void StartSession()
        {
            IsActive = true; // Establecer el estado de la sesión como activo
            detectionTimer.Start(); // Iniciar el temporizador para la detección de enlaces
            Console.WriteLine($"Sesión BFD entre {LocalNode} y {RemoteNode} iniciada.");
        }

        // Método para detener la sesión BFD
        public void StopSession()
        {
            IsActive = false; // Establecer el estado de la sesión como inactivo
            detectionTimer.Stop(); // Detener el temporizador de detección de enlaces
            Console.WriteLine($"Sesión BFD entre {LocalNode} y {RemoteNode} detenida.");
        }

        // Método para actualizar el tiempo de detección en la sesión BFD
        public void UpdateDetectionTime(TimeSpan newDetectionTime)
        {
            DetectionTime = newDetectionTime; // Actualizar el tiempo de detección
            detectionTimer.Stop(); // Detener el temporizador actual
            detectionTimer = new BFDTimer(newDetectionTime, HandleDetectionTimeout); // Inicializar un nuevo temporizador con el nuevo tiempo de detección
            Console.WriteLine($"Tiempo de detección actualizado para la sesión BFD entre {LocalNode} y {RemoteNode}.");
        }

        // Método para manejar el tiempo de espera de detección de enlaces
        private void HandleDetectionTimeout()
        {
            // Acciones a tomar cuando el temporizador de detección de enlaces expire
            Console.WriteLine("¡Se ha detectado una pérdida de enlace!");
        }
    }
}

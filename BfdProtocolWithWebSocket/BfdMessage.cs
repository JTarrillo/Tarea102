using System;

namespace BfdProtocolWithWebSocket
{
    // Clase que representa un mensaje BFD (Bidirectional Forwarding Detection)
    public class BfdMessage
    {
        // Enumeración que define los diferentes tipos de mensajes BFD
        public enum MessageType
        {
            Hello,  // Tipo de mensaje Hello
            Echo    // Tipo de mensaje Echo
        }

        // Propiedades para el tipo y contenido del mensaje
        public MessageType Type { get; set; }     // Propiedad para el tipo de mensaje
        public string Content { get; set; }        // Propiedad para el contenido del mensaje

        // Constructor que inicializa un mensaje BFD con su tipo y contenido
        public BfdMessage(MessageType type, string content)
        {
            Type = type;        // Asigna el tipo de mensaje
            Content = content;  // Asigna el contenido del mensaje
        }

        // Método para convertir un mensaje BFD a una cadena legible
        public override string ToString()
        {
            return $"Type: {Type}, Content: {Content}";  // Retorna una cadena que representa el mensaje BFD
        }
    }
}

# Documentación del Código BFD con WebSocket

## Introducción
Este documento proporciona una descripción detallada del código relacionado con el protocolo BFD (Bidirectional Forwarding Detection) implementado con WebSocket en C#.

## Clases Principales

### `BfdMessage`
- **Descripción:** Esta clase representa un mensaje BFD, que puede ser del tipo "Hello" o "Echo".
- **Propiedades:**
  - `Type`: Tipo de mensaje (enumeración `MessageType`).
  - `Content`: Contenido del mensaje.


### `BfdNeighbor`
- **Descripción:** Representa un vecino BFD asociado a una dirección IP.
- **Propiedades:**
  - `IPAddress`: Dirección IP del vecino.
  - `IsNeighborUp`: Estado del vecino (activo o inactivo).
  - `LastPacketReceivedTime`: Momento en que se recibió el último paquete desde el vecino.
- **Métodos:**
  - `SetNeighborUp()`: Establece el vecino como activo.
  - `SetNeighborDown()`: Establece el vecino como inactivo.
  - `UpdateLastPacketReceivedTime()`: Actualiza el tiempo del último paquete recibido.

### `BfdNode`
- **Descripción:** Representa un nodo BFD asociado a una dirección IP.
- **Propiedades:**
  - `IPAddress`: Dirección IP del nodo.
  - `IsUp`: Estado del nodo (activo o inactivo).
  - `LastPacketReceivedTime`: Momento en que se recibió el último paquete desde el nodo.
- **Métodos:**
  - `SetNodeUp()`: Establece el nodo como activo.
  - `SetNodeDown()`: Establece el nodo como inactivo.
  - `UpdateLastPacketReceivedTime()`: Actualiza el tiempo del último paquete recibido.

### `BfdMessageHandler`
- **Descripción:** Gestiona los mensajes BFD, envía mensajes "Hello" y "Echo" a los vecinos, y maneja la lógica de inactividad.
- **Propiedades:**
  - `UltimomensajeRecibido`: Indica si el último mensaje recibido fue "Hello" o "Echo".
- **Métodos:**
  - `SendHelloMessage()`: Envía un mensaje "Hello" a todos los vecinos.
  - `SendEchoMessage()`: Envía un mensaje "Echo" a todos los vecinos.
  - `HandleIncomingMessage(message, ipAddress)`: Maneja un mensaje entrante y realiza acciones según su tipo.
  - `WaitForEchoResponse(timeout)`: Espera una respuesta "Echo" dentro de un tiempo dado.

### `BfdSession`
- **Descripción:** Representa una sesión BFD entre dos nodos.
- **Propiedades:**
  - `LocalNode`: Nodo local de la sesión.
  - `RemoteNode`: Nodo remoto de la sesión.
  - `IsActive`: Estado de la sesión (activa o no).
  - `DetectionTime`: Tiempo de detección de la sesión.
- **Métodos:**
  - `StartSession()`: Inicia la sesión.
  - `StopSession()`: Detiene la sesión.
  - `UpdateDetectionTime(newDetectionTime)`: Actualiza el tiempo de detección.
  - `HandleDetectionTimeout()`: Maneja la detección de tiempo de espera de la sesión.

### `BfdSocketManager`
- **Descripción:** Gestiona la comunicación mediante sockets para el protocolo BFD.
- **Métodos:**
  - `StartListening(port)`: Inicia la escucha en un puerto específico.
  - `Stop()`: Detiene el temporizador de inactividad.

## Otros Componentes
- **`BFDTimer`**: Clase para gestionar un temporizador con un intervalo y una devolución de llamada.
- **`TCPServer`**: Clase para manejar un servidor TCP y procesar mensajes entrantes.
- **`Program`**: Clase principal que inicia el socket BFD, el servidor TCP y realiza interacciones BFD.

## Cliente TCP

### `Program`
- **Descripción:** Este programa representa un cliente TCP que se conecta a un servidor TCP y envía mensajes.
- **Funcionamiento:**
  - El método `StartClient()` inicia el cliente TCP.
    - Establece una conexión con el servidor TCP utilizando la dirección IP y el puerto especificados.
  - Entra en un bucle infinito donde espera la entrada del usuario para enviar mensajes al servidor.
    - Acepta la entrada del usuario, la convierte en bytes utilizando UTF-8 y la envía al servidor.
    - Espera una respuesta del servidor, la lee del flujo de red y la muestra al usuario.
- **Relación con BFD:**
  - El cliente TCP es una parte del sistema que se comunica con un servidor TCP que podría estar ejecutando un servicio relacionado con BFD (por ejemplo, un servidor que gestiona conexiones BFD).
  - El intercambio de mensajes entre el cliente y el servidor puede incluir mensajes específicos del protocolo BFD, como mensajes "Hello" o "Echo", dependiendo de la implementación del servidor y del propósito de la comunicación.

## Protocolo BFD

### Funcionamiento
- **Inicio de sesión BFD:**
  - Cuando se establece la conexión TCP entre el cliente y el servidor, podría iniciarse una sesión BFD entre los nodos representados por el cliente y el servidor.
  - Durante la inicialización de la sesión BFD, podrían intercambiarse mensajes de "Hello" para establecer la conectividad y sincronizar parámetros de sesión.
- **Mantenimiento de la sesión:**
  - Una vez que la sesión BFD está establecida, el cliente y el servidor pueden intercambiar mensajes de "Echo" para monitorear la conectividad bidireccional entre ellos.
  - Si el cliente no recibe una respuesta "Echo" dentro de un tiempo especificado, podría interpretarlo como una indicación de que la conexión con el servidor está inactiva o ha fallado.
- **Gestión de Vecinos y Nodos:**
  - El cliente TCP puede mantener información sobre vecinos y nodos remotos a través de clases como `BfdNeighbor` y `BfdNode`.
  - Puede utilizar esta información para gestionar la comunicación BFD y tomar decisiones basadas en el estado de los vecinos y nodos remotos.

### Relación con el Cliente TCP
- El cliente TCP puede iniciar y mantener una sesión BFD con un servidor TCP remoto.
- Utiliza los mensajes "Hello" y "Echo" según lo definido por el protocolo BFD para mantener la sesión y monitorear la conectividad.
- La comunicación TCP entre el cliente y el servidor proporciona el medio para el intercambio de mensajes BFD, permitiendo así la detección de enlaces activos y la gestión de la conectividad.


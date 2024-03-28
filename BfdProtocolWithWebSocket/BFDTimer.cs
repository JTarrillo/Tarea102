using System;
using System.Threading;

namespace BfdProtocolWithWebSocket
{
    public class BFDTimer
    {
        private Timer timer; // Objeto Timer para gestionar el temporizador
        private TimeSpan intervalo; // Intervalo de tiempo entre cada ejecución del temporizador
        private Action callback; // Método de devolución de llamada que se ejecutará cuando el temporizador expire

        // Constructor de la clase BFDTimer
        public BFDTimer(TimeSpan interval, Action callback)
        {
            this.intervalo = interval;
            this.callback = callback;
            this.timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite); // Crear el objeto Timer con valores predeterminados
        }

        // Propiedad para obtener el intervalo de tiempo del temporizador
        public TimeSpan Interval => intervalo;

        // Método para iniciar el temporizador
        public void Start()
        {
            timer.Change((int)intervalo.TotalMilliseconds, Timeout.Infinite); // Cambiar el estado del temporizador para iniciar la cuenta regresiva
        }

        // Método para detener el temporizador
        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite); // Detener el temporizador
        }

        // Método para reiniciar el temporizador
        public void Reset()
        {
            timer.Change((int)intervalo.TotalMilliseconds, Timeout.Infinite); // Reiniciar el temporizador estableciendo el intervalo especificado
        }

        // Método de devolución de llamada que se ejecuta cuando el temporizador expire
        private void TimerCallback(object state)
        {
            // Ejecutar la devolución de llamada si está definida
            callback?.Invoke();
        }
    }
}

using System;
using System.IO.Ports;
using System.Threading;

namespace SerialPort
{
    public class ReconnectComPort : IDisposable
    {
        private Timer _disconnectTimer;
        private bool _disposed;

        private readonly ComPort _comPort;

        /// <summary>
        /// Create an instance of the ReconnectComPort.
        /// </summary>
        /// <param name="portName">The name of the serial port to connect to.</param>
        /// <param name="baudRate">Sets the serial baud rate.</param>
        /// <param name="parity">Sets the parity-checking protocol.</param>
        /// <param name="dataBits">Sets the standard length of data bits per byte.</param>
        /// <param name="stopBits">Sets the standard number of stopbits per byte.</param>
        /// <param name="processReceivedByte">Action that will process the received byte queue. If this action is not specified, the ReceivedData collection must be processed.</param>
        public ReconnectComPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Action<byte> processReceivedByte = null)
        {
            _comPort = new ComPort(portName, baudRate, parity, dataBits, stopBits, processReceivedByte);
            _comPort.PortError+=_comPort_PortError;
        }

        void _comPort_PortError(Exception ex)
        {
            throw ex;
        }

        private void _disconnectTimer_Tick(object state)
        {
            if (_comPort.LastDataReceived < DateTime.Now.AddMinutes(-1))
            {
                //port is idle, maybe disconnected
                //try reconnecting to port
                StopCommunications();
                StartCommunications();
            }
        }

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="text">Text to write to the serial port.</param>
        public void SendData(string text)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");

            _comPort.SendData(text);
        }

        /// <summary>
        /// Writes the specified byte array to the serial port.
        /// </summary>
        /// <param name="bytes">The byte array that contains the data to write to the serial port.</param>
        public void SendData(byte[] bytes)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");

            _comPort.SendData(bytes);
        }

        /// <summary>
        /// Open the port, start the disconnect timer, and if the ProcessReceivedByte action was specified, start the collection monitor.
        /// </summary>
        public void StartCommunications()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");

            _comPort.StartCommunications();

            _disconnectTimer = new Timer(_disconnectTimer_Tick, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Stop the collection monitor if the ProcessReceivedByte action was specified, dispose of the timer, and close the port and dispose.
        /// </summary>
        public void StopCommunications()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");

            _comPort.StopCommunications();

            _disconnectTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            StopCommunications();

            _comPort.Dispose();
            _disconnectTimer.Dispose();

            _disposed = true;
        }
    }
}

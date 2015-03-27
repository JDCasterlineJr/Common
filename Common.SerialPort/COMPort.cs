using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.SerialPort
{
    public delegate void PortErrorEventHander(Exception ex);

    public delegate void PortConnectedEventHandler(string portName);

    /// <summary>
    /// Wrapper around <see cref="System.IO.Ports.SerialPort"/>.
    /// </summary>
    public class COMPort : IDisposable
    {
        public DateTime LastDataReceived { get; private set; }
        private readonly System.IO.Ports.SerialPort _port;
        private readonly Action<byte> _processReceivedByte;
        private CancellationTokenSource _ctsPortQueueMonitor;
        private BlockingCollection<byte> _receivedDataQueue = new BlockingCollection<byte>();
        private Timer _disconnectTimer;
        private bool _disposed;
        
        /// <summary>
        /// Represents the method that will handle the port error event of a <see cref="COMPort"/> object.
        /// </summary>
        public event PortErrorEventHander PortError;
        /// <summary>
        /// Represents the method that will handle the port connected event of a <see cref="COMPort"/> object.
        /// </summary>
        public event PortConnectedEventHandler PortConnected;

        /// <summary>
        /// Open a COMPort and 
        /// </summary>
        /// <param name="portName">Sets the serial baud rate.</param>
        /// <param name="baudRate"></param>
        /// <param name="parity">Sets the parity-checking protocol.</param>
        /// <param name="dataBits">Sets the standard length of data bits per byte.</param>
        /// <param name="stopBits">Sets the standard number of stopbits per byte.</param>
        /// <param name="processReceivedByte">Action that will process the received byte queue. If this action is not specified, the ReceivedData collection must be processed.</param>
        public COMPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits,
            Action<byte> processReceivedByte = null)
        {
            LastDataReceived = DateTime.MinValue;

            var portExists = System.IO.Ports.SerialPort.GetPortNames().Any(x => x == portName);
            if (!portExists)
            {
                if (PortError != null)
                    PortError(new Exception(string.Format("COM port {0} does not exist.", portName)));
                return;
            }

            _port = new System.IO.Ports.SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _port.DataReceived += _port_DataReceived;
            _port.ErrorReceived += _port_ErrorReceived;

            _processReceivedByte = processReceivedByte;
        }

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");

            StopCommunications();

            if(_ctsPortQueueMonitor!=null)
                _ctsPortQueueMonitor.Dispose();

            _port.Dispose();
            _disconnectTimer.Dispose();

            _disposed = true;
        }

        private void _port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (PortError != null) PortError(new Exception(e.EventType.ToString()));
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            LastDataReceived = DateTime.Now;

            var buffer = new byte[_port.BytesToRead];
            try
            {
                _port.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
                return;
            }

            buffer.ToList().ForEach(b => _receivedDataQueue.Add(b));
        }

        private void _disconnectTimer_Tick(object state)
        {
            if (LastDataReceived < DateTime.Now.AddMinutes(-1))
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

            try
            {
                _port.Write(text);
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
            }
        }

        /// <summary>
        /// Writes the specified byte array to the serial port.
        /// </summary>
        /// <param name="bytes">The byte array that contains the data to write to the serial port.</param>
        public void SendData(byte[] bytes)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");
            
            try
            {
                _port.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
            }
        }

        /// <summary>
        /// Open the port, start the disconnect timer, and if the ProcessReceivedByte action was specified, start the collection monitor.
        /// </summary>
        public void StartCommunications()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");
            
            try
            {
                if (_port.IsOpen)
                    _port.Close();

                _port.Open();

                if (PortConnected != null) PortConnected(_port.PortName);
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
                return;
            }

            if (_processReceivedByte != null)
                StartCollectionMonitor();

            _disconnectTimer = new Timer(_disconnectTimer_Tick, null, new TimeSpan(0, 0, 1, 0), new TimeSpan(0, 0, 1, 0));
        }

        /// <summary>
        /// Stop the collection monitor if the ProcessReceivedByte action was specified, dispose of the timer, and close the port and dispose.
        /// </summary>
        public void StopCommunications()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name, "Cannot use a disposed object.");
            
            if (_ctsPortQueueMonitor != null)
                _ctsPortQueueMonitor.Cancel();

            _disconnectTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            try
            {
                if (_port.IsOpen)
                    _port.Close();
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
            }

            ReceivedData = new BlockingCollection<byte>();
        }

        private void StartCollectionMonitor()
        {
            try
            {
                _ctsPortQueueMonitor = new CancellationTokenSource();

                Task.Factory.StartNew(delegate
                {
                    try
                    {
                        while (null != _ctsPortQueueMonitor && !_ctsPortQueueMonitor.Token.IsCancellationRequested)
                        {
                            var item = _receivedDataQueue.Take(_ctsPortQueueMonitor.Token);
                            _processReceivedByte(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (PortError != null) PortError(ex);
                    }
                }, _ctsPortQueueMonitor.Token);
            }
            catch (Exception ex)
            {
                if (PortError != null) PortError(ex);
            }
        }

        #region Properties

        /// <summary>
        /// The collection holding data received on the serial port. This must be 
        /// </summary>
        public BlockingCollection<byte> ReceivedData
        {
            get { return _receivedDataQueue; }
            set { _receivedDataQueue = value; }
        }

        #endregion
    }
}
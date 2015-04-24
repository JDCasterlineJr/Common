using System;
using System.IO.Ports;
using System.Threading;

namespace SerialPort
{
    class AutoPort
    {
        private ComPort _port;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baudRate">Sets the serial baud rate.</param>
        /// <param name="parity">Sets the parity-checking protocol.</param>
        /// <param name="dataBits">Sets the standard length of data bits per byte.</param>
        /// <param name="stopBits">Sets the standard number of stopbits per byte.</param>
        /// <param name="processReceivedByte">Action that will process the received byte queue. If this action is not specified, the ReceivedData collection must be processed.</param>
        /// <param name="validateBuffer">Function that will validate that the data in the buffer is the expected data. If this function is not specified, the first port that can be opened and receives data will be used.</param>
        public AutoPort(int baudRate, Parity parity, int dataBits, StopBits stopBits, Action<byte> processReceivedByte = null, Func<byte[], bool> validateBuffer = null)
        {
            var foundPortName = string.Empty;

            var portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (var portName in portNames)
            {
                using (var port = new ComPort(portName, baudRate, parity, dataBits, stopBits))
                {
                    try
                    {
                        port.StartCommunications();

                        //Sleep for one second to allow for buffer to receive data from device
                        Thread.Sleep(1000);

                        //process received data here
                        //if processed data is valid
                        //foundPortName = portName
                        //return

                        port.StopCommunications();
                    }
                    catch (Exception)
                    {
                        //ignore errors while we are looking for a valid port
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(foundPortName))
            {
                _port = new ComPort(foundPortName, baudRate, parity, dataBits, stopBits);
                _port.StartCommunications();
            }
            else
            {
                throw new Exception(string.Format("Could not find a COM port receiving expected data."));
            }
        }

        void _port_PortError(Exception ex)
        {
            throw ex;
        }
    }
}

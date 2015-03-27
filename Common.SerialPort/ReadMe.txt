
//Specify port name
var _port = new SerialPort(Settings.Default.PortName, Settings.Default.BaudRate, parity, Settings.Default.DataBits, stopBits, ProcessByte);

//Allow serial port to try to find valid port
var _port = new SerialPort(Settings.Default.BaudRate, parity, Settings.Default.DataBits, stopBits, ProcessByte, ValidateBuffer);

_port.PortConnected += _port_PortConnected;
_port.PortError += _port_PortError;
_port.StartCommunications();

private bool ValidateBuffer(byte[] bytes)
{
	//try to process data
	//if data is not valid return false
	//if data is valid return true
    return true;
}

private List<byte> _currentMessage = new List<byte>();
private void ProcessByte(byte item)
{
	//Process each byte received on the serial port here.
	//This is run on a different thread than receving data on the buffer.

	//Add current byte to current message
    _currentMessage.Add(item);

	//check if byte equal end of message byte
    if (item != 116) return;

	//Once an end of message byte is received, process the message

	//if needed convert message to string
    var str = Encoding.Default.GetString(_currentMessage.ToArray());

	//Clear current message after processing
    _currentMessage = new List<byte>();

}
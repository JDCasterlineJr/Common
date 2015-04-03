using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Common.Application
{
    /// <summary>
    /// Delegate that will return messages about the connectivity check process.
    /// </summary>
    /// <param name="message">Details about process.</param>
    public delegate void ConnectivityMessageHander(string message);

    /// <summary>
    /// A class used to verify an internet connection.
    /// </summary>
    public class Connectivity
    {
        /// <summary>
        /// Event that will return messages about the connectivity check process and any errors that occur.
        /// </summary>
        public static event ConnectivityMessageHander ConnectivityMessage;

        /// <summary>
        /// Retrieves the connected state of the local system.
        /// </summary>
        /// <param name="flag">Pointer to a variable that receives the connection description. </param>
        /// <param name="reserved">This parameter is reserved and must be 0.</param>
        /// <returns>Returns true if there is an active internet connection.</returns>
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int flag, int reserved);

        /// <summary>
        /// A method that wraps the InternetGetConnectedState api call.
        /// </summary>
        /// <returns>True if an active network connection is found.</returns>
        public static bool CanConnectToNetwork()
        {
            int flag;
            return InternetGetConnectedState(out flag, 0);
        }

        /// <summary>
        /// Pings the specified server.
        /// </summary>
        /// <param name="hostNameOrAddress">Host name or ip address to ping.</param>
        /// <returns>True if the host or address specified can be pinged successfully.</returns>
        public static bool CanConnectToHost(string hostNameOrAddress)
        {
            try
            {
                IPAddress address;

                var isIpAddress = IPAddress.TryParse(hostNameOrAddress, out address);
                if (!isIpAddress)
                {
                    var uri = new Uri(hostNameOrAddress);

                    if (uri.IsLoopback) return true;

                    if (uri.HostNameType == UriHostNameType.Dns)
                    {
                        address = Dns.GetHostEntry(uri.Host).AddressList.First();
                    }
                }

                var myPing = new Ping();
                var buffer = new byte[32];
                const int timeout = 10000;
                var pingOptions = new PingOptions();

                var reply = myPing.Send(address, timeout, buffer, pingOptions);
                if (reply != null)
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                    else
                    {
                        if (ConnectivityMessage != null)
                            ConnectivityMessage(string.Format("Could not reach address: {0} - Status: {1}",
                                reply.Address,
                                reply.Status));
                    }
                else
                {
                    if (ConnectivityMessage != null)
                        ConnectivityMessage("Could not reach host " + hostNameOrAddress);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// Wrapper for CanConnectoToNetwork and CanConnectToHost
        /// </summary>
        /// <param name="hostNameOrAddress">A String that identifies the host computer. The value specified for this parameter can be a host name or a string representation of an IP address.</param>
        /// <returns>True if connection to host can be made.</returns>
        public static bool HasConnectivity(string hostNameOrAddress = null)
        {
            if (ConnectivityMessage != null) ConnectivityMessage("Start connectivity check");

            if (CanConnectToNetwork())
            {
                if (ConnectivityMessage != null) ConnectivityMessage("Network available");

                return String.IsNullOrWhiteSpace(hostNameOrAddress) || CanConnectToHost(hostNameOrAddress);
            }
            else
            {
                if (ConnectivityMessage != null) ConnectivityMessage("Network not avaiable");
            }
            return false;
        }
    }
}
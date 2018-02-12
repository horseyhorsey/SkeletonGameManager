using OSCforSTD;
using System.Net;
using System.Net.Sockets;

namespace SkeletonGame.Engine
{
    public interface ISkeletonOSC
    {
        void Send(string msg, object value);
    }

    public class SkeletonOSC : ISkeletonOSC
    {
        private readonly OSCClient _oscForCore;

        public SkeletonOSC()
        {
            _oscForCore = new OSCClient();
        }

        /// <summary>
        /// Sends the specified MSG through OSC.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="value">The value.</param>
        public void Send(string msg, object value)
        {
            if (_oscForCore.Host == null)
            {
                _oscForCore.Host = GetLocalIp();
                _oscForCore.Port = 9000;
            }
            
            _oscForCore.Send(new OSCMessage(msg, value));
        }

        /// <summary>
        /// Gets the local ip for this computer. See <see cref="AddressFamily.InterNetwork"/>
        /// </summary>
        /// <returns></returns>
        private string GetLocalIp()
        {
            foreach (var item in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                    return item.ToString();
            }

            return null;
        }
    }
}

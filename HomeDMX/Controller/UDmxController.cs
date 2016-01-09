using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeDMX.Controller
{
    public class UDmxController : IDmxController
    {
        private const int MaxChannels = 512;
        private readonly byte[] _channels = new byte[MaxChannels];

        public bool IsConnected
        {
            get
            {
                return Connected();
            }
        }

        public int ChannelCount
        {
            get
            { return MaxChannels; }
        }

        #region DllImport
        [DllImport("Libs\\uDMX.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Configure();

        //[DllImport("Libs\\uDMX.dll", CallingConvention = CallingConvention.StdCall)]
        //private static extern bool ConfigureModal();

        [DllImport("Libs\\uDMX.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool ChannelSet(uint channel, uint value);

        //[DllImport("Libs\\uDMX.dll", CallingConvention = CallingConvention.StdCall)]
        //private static extern bool ChannelsSet(uint channelCnt, uint channel, uint[] value);

        [DllImport("Libs\\uDMX.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool Connected();

        #endregion

        public UDmxController()
        {
        }

        public byte this[int channel]
        {
            get { return Get(channel); }
            set { Set(channel, value); }
        }

        /// <summary>
        /// Shows plain configuration dialog
        /// </summary>
        public void ShowConfigDialog()
        {
            Configure();
        }

        public void Set(int channel, byte value)
        {
            if (channel < 0 || channel > MaxChannels)
                return;
            var uChannel = Convert.ToUInt32(channel);
            var uValue = Convert.ToUInt32(value);
            if (ChannelSet(uChannel, uValue))
            {
                _channels[channel] = value;
            }
        }

        public void Set(Dictionary<int, byte> values)
        {
            foreach (var key in values.Keys)
            {
                Set(key, values[key]);
            }
        }

        public void Set(List<byte> values)
        {
            Set(values, 0);
        }

        public void Set(List<byte> values, int startIndex)
        {
            var channel = startIndex;
            foreach (var value in values)
            {
                Set(channel, value);
            }
        }

        public byte Get(int channel)
        {
            if (channel < 0 || channel > MaxChannels)
                return 0;
            return _channels[channel];
        }

        public List<byte> GetAsList()
        {
            return _channels.ToList();
        }

        public Dictionary<int, byte> GetAsDictionary()
        {
            return _channels.Select((Value, Index) => new { Value, Index }).ToDictionary(x => x.Index, x => x.Value);
        }

        public void Reset()
        {
            for (int i = 0; i < _channels.Length; i++)
            {
                Set(i, 0);
            }
        }
    }
}

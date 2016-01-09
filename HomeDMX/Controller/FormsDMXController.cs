using System.Collections.Generic;

namespace HomeDMX.Controller
{
    class FormsDmxController : IDmxController
    {
        public byte this[int channel]
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsConnected { get; }
        public int ChannelCount { get; }
        public void Set(int channel, byte value)
        {
            throw new System.NotImplementedException();
        }

        public void Set(Dictionary<int, byte> values)
        {
            throw new System.NotImplementedException();
        }

        public void Set(List<byte> values)
        {
            throw new System.NotImplementedException();
        }

        public void Set(List<byte> values, int startIndex)
        {
            throw new System.NotImplementedException();
        }

        public byte Get(int channel)
        {
            throw new System.NotImplementedException();
        }

        public List<byte> GetAsList()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<int, byte> GetAsDictionary()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections.Generic;

namespace HomeDMX.Controller
{
    public interface IDMXController
    {
        byte this[int channel] { get; set; }
        bool IsConnected { get; }
        int ChannelCount { get; }

        void Set(int channel, byte value);
        void Set(Dictionary<int, byte> values);
        void Set(List<byte> values);
        void Set(List<byte> values, int startIndex);

        byte Get(int channel);
        List<byte> GetAsList();
        Dictionary<int, byte> GetAsDictionary();

        void Reset();


    }
}

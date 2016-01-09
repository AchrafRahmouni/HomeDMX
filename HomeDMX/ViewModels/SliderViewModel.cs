using HomeDMX.Controller;

namespace HomeDMX.ViewModels
{
    public class DmxViewModel : BaseViewModel
    {
        private readonly IDmxController _dmxController;
        private readonly int _channel;


        public DmxViewModel(IDmxController dmxController, int channel)
        {
            _dmxController = dmxController;
            _channel = channel;
        }

        public byte Value
        {
            get { return _dmxController[_channel]; }
            set
            {
                if (value == _dmxController[_channel])
                    return;
                _dmxController[_channel] = value;
                OnPropertyChanged();
            }
        }
    }
}
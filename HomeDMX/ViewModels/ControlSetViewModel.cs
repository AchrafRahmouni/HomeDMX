using System.Collections.ObjectModel;

namespace HomeDMX.ViewModels
{
    public class ControlSetViewModel : BaseViewModel
    {
        public ObservableCollection<DmxViewModel> DmxControllers { get; } = new ObservableCollection<DmxViewModel>();
    }
}
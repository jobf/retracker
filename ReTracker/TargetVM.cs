using System.ComponentModel;
using System.Windows.Input;
using BuzzGUI.Common;

namespace ReTracker
{
    public class TargetVm : INotifyPropertyChanged
    {
        public TargetVm()
        {
            _removeCommand = new RelayCommand(o => { PropertyChanged.Raise(this, "RemoveCommand"); }, o => true);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string MachineName { get; set; }
       
        private readonly ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand;
            }
        }
    }
}

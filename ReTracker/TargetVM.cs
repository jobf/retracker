using System.ComponentModel;
using System.Windows.Input;
using BuzzGUI.Common;

namespace ReTracker
{
    public class TargetVm : INotifyPropertyChanged
    {
        private readonly Target _model;
        public TargetVm(Target model)
        {
            _model = model;
            _init();
        }

        private void _init()
        {
            _removeCommand = new RelayCommand(o => { PropertyChanged.Raise(this, "RemoveCommand"); }, o => true);
            PropertyChanged.Raise(this, "MachineName");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string MachineName { get { return _model.Name;  } }
       
        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand;
            }
        }

    }
}

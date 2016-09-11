using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;

namespace ReTracker
{
    /// <summary>
    /// This is required to tell Buzz to show the GUI instead of a parameter window
    /// </summary>
    [MachineGUIFactoryDecl(PreferWindowedGUI = true, IsGUIResizable = false)]
    public class MachineGUIFactory : IMachineGUIFactory
    {
        public IMachineGUI CreateGUI(IMachineGUIHost host) { return new ReTrackerGUI(); }
    }


    /// <summary>
    /// Interaction logic for ReTrackerGUI.xaml
    /// </summary>
    public partial class ReTrackerGUI : UserControl, IMachineGUI, INotifyPropertyChanged
    {
        public ReTrackerGUI()
        {
            InitializeComponent();
            _targets = new List<TargetVm>();
            _machineNames = new List<string>();

            /* todo: better way of blacklisting or some other fix...
             * Ideally this would check that the instance of the machine is not the same instance of the machine this GUI is attached to.
             * However that relies on this.Machine to be ready, which at this time it is still null
             */
            var availableMachines = Global.Buzz.Song.Machines.Where(m => !IsBlacklistedMachine(m));
            foreach (var machine in availableMachines)
            {
                _machineNames.Add(machine.Name);
            }

            _addTargetCommand = new RelayCommand(_ => { _addTarget(); }, _ => _canAddTarget());
            
            DataContext = this;
        }

        private bool IsBlacklistedMachine(IMachine m)
        {
            return m.DLL.Info.Type == MachineType.Master || m.DLL.Name == "ReTracker";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedMachineName;
        public string SelectedMachine
        {
            get { return _selectedMachineName; }
            set
            {
                _selectedMachineName = value;
                PropertyChanged.Raise(this, "SelectedMachine");
            }
        }

        private List<string> _machineNames;
        public ObservableCollection<string> MachineNames
        {
            get
            {
                return new ObservableCollection<string>(_machineNames);
            }
        }

        private List<TargetVm> _targets;
        public IEnumerable<TargetVm> Targets
        {
            get
            {
                return new ObservableCollection<TargetVm>(_targets);
            }
        }

        private ReTrackerMachine ManagedMachine { get { return _machine.ManagedMachine as ReTrackerMachine; } }

        private IMachine _machine;
        public IMachine Machine
        {
            get { return _machine; }

            set
            {
                if (_machine != null)
                {
                    _machine.Graph.Buzz.Song.MachineAdded -= _song_MachineAdded;
                    _machine.Graph.Buzz.Song.MachineRemoved -= _song_MachineRemoved;
                }

                _machine = value;

                if (_machine != null)
                {
                    _machine.Graph.Buzz.Song.MachineAdded += _song_MachineAdded;
                    _machine.Graph.Buzz.Song.MachineRemoved += _song_MachineRemoved;
                    foreach (IMachine machine in ManagedMachine.GetTargets())
                    {
                        _targets.Add(new TargetVm(new Target(machine)));
                    }
                }
            }
        }

        private readonly ICommand _addTargetCommand;
        public ICommand AddTargetCommand
        {
            get
            {
                return _addTargetCommand;
            }
        }

        private void _song_MachineAdded(IMachine m)
        {
            if (!IsBlacklistedMachine(m))
            {
                _machineNames.Add(m.Name);
                PropertyChanged.Raise(this, "MachineNames");
            }
        }

        private void _song_MachineRemoved(IMachine m)
        {
            _machineNames = _machineNames.Where(mn => mn != m.Name).ToList();
            PropertyChanged.Raise(this, "MachineNames");
            RemoveTarget(m.Name);
        }

        private void RemoveTarget(string machineName)
        {
            ManagedMachine.RemoveTarget(machineName);
            _targets = _targets.Where(t => t.MachineName != machineName).ToList();
            PropertyChanged.Raise(this, "Targets");
        }

        private bool _canAddTarget()
        {
            return !string.IsNullOrEmpty(_selectedMachineName) && !ManagedMachine.HasTarget(_selectedMachineName);
        }

        private void _addTarget()
        {
            if (!_targets.Any(t => t.MachineName == _selectedMachineName))
            {
                var machine = _machine.Graph.Buzz.Song.Machines.SingleOrDefault(m => m.Name == _selectedMachineName);
                if(machine != null)
                {

                    var targetModel = new Target(machine);
                    ManagedMachine.AddTarget(targetModel);


                    var targetView = new TargetVm(targetModel);
                    targetView.PropertyChanged += Target_PropertyChanged;
                    _targets.Add(targetView);

                    PropertyChanged.Raise(this, "Targets");
                }
            }
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemoveCommand")
            {
                var m = sender as TargetVm;
                RemoveTarget(m.MachineName);
            }
        }
    }
}

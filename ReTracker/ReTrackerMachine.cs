using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Buzz.MachineInterface;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;

namespace ReTracker
{
    [MachineDecl(Name = "ReTracker", ShortName = "ReTracker", Author = "mirf", MaxTracks = 32)]
    public class ReTrackerMachine : IBuzzMachine, INotifyPropertyChanged
    {
        public class StateData
        {
            public List<Target.State> targets;
        }

        private readonly IBuzzMachineHost _host;
        private readonly Track[] _tracks = new Track[32];
        private BindingList<Target> _targets = new BindingList<Target>();
        private List<Target.State> _targetsToLoad;
        
        public ReTrackerMachine(IBuzzMachineHost host)
        {
            _host = host;
            for (int i = 0; i < _tracks.Length; i++)
                _tracks[i] = new Track();

        }

        [ParameterDecl]
        public void Note(Note v, int track)
        {
            _tracks[track].Note(v);
        }

        [ParameterDecl(MinValue = 0, MaxValue = 127, DefValue = 100)]
        public void Velocity(int v, int track)
        {
            _tracks[track].Velocity(v);
        }

        public void ImportFinished(IDictionary<string, string> machineNameMap)
        {
            if (_targetsToLoad == null)
                return;

            _targets = new BindingList<Target>(_targetsToLoad.Select(a => new Target(a)).ToList());
            _targetsToLoad = null;
        }

        public void Work()
        {
            if (Global.Buzz.Playing && _host.SubTickInfo.CurrentSubTick == 0 && _host.SubTickInfo.PosInSubTick == 0)
            {
                for (int t = 0; t < _host.Machine.ParameterGroups[2].TrackCount; t++)
                {
                    Track track = _tracks[t];
                    if (track.ShouldTrigger)
                    {
                        Target[] threadsafeTargets;
                        lock (_targets) threadsafeTargets = _targets.ToArray();

                        foreach (var tt in threadsafeTargets)
                        {
                            if (tt != null)
                            {
                                tt.SendNote(track.GetNote, track.GetVelocity);
                            }
                        }
                        track.ResetTrigger();
                    }
                }
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        public StateData MachineState
        {
            get
            {
                var targetsToSave = _targets.Select(a => a.GetState()).ToList();
                return new StateData { targets = targetsToSave };
            }
            set
            {
                _targetsToLoad = value.targets;
            }
        }

        public bool HasTarget(string machineName)
        {
            return _targets.Any(t => t.Name == machineName);
        }

        public IEnumerable<IMachine> GetTargets()
        {
            return _targets.Select(t => t.Machine);
        }

        public void AddTarget(Target target)
        {
            lock (_targets)
            {
                _targets.Add(target);
            }
        }

        public void RemoveTarget(string machineName)
        {
            lock (_targets)
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    if (_targets[i].Name == machineName)
                    {
                        _targets.RemoveAt(i);
                    }
                }
            }
        }
    }
}

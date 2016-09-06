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
        private readonly IBuzzMachineHost _host;
        private readonly Track[] _tracks = new Track[32];
        private readonly List<IMachine> _targets = new List<IMachine>();

        public ReTrackerMachine(IBuzzMachineHost host)
        {
            _host = host;
            for (int i = 0; i < _tracks.Length; i++)
                _tracks[i] = new Track();

        }

        [ParameterDecl(IsStateless = true)]
        public void Note(Note v, int track) { _tracks[track].Note(v); }

        [ParameterDecl(IsStateless = true, MinValue = 0, MaxValue = 127, DefValue = 100)]
        public void Volume(int v, int track) { _tracks[track].Volume(v); }

        public void AddTarget(string machineName)
        {
            lock (_targets)
            {
                if (!_targets.Any(t => t.Name == machineName))
                {
                    var target = Global.Buzz.Song.Machines.FirstOrDefault(m => m.Name == machineName);
                    _targets.Add(target);
                }
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

        public void Work()
        {
            if (Global.Buzz.Playing && _host.SubTickInfo.CurrentSubTick == 0 && _host.SubTickInfo.PosInSubTick == 0)
            {
                for (int t = 0; t < _host.Machine.ParameterGroups[2].TrackCount; t++)
                {
                    Track track = _tracks[t];
                    if (track.ShouldTrigger)
                    {
                        IMachine[] threadsafeTargets;
                        lock (_targets) threadsafeTargets = _targets.ToArray();

                        foreach (var tt in threadsafeTargets)
                        {
                            if (tt != null && track.VolumeGet > 0)
                            {
                                tt.SendMIDINote(0, track.NoteGet, track.VolumeGet);
                            }
                        }
                        track.ResetTrigger();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

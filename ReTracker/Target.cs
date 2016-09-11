using System;
using System.ComponentModel;
using System.Windows.Input;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;
using System.Linq;
using Buzz.MachineInterface;

namespace ReTracker
{
    public class Target
    {
        private IMachine _machine;
        public IMachine Machine { get { return _machine; } }

        public Target()
        {
        }

        public Target(State load)
        {
            // should inject this ?
            _machine = Global.Buzz.Song.Machines.SingleOrDefault(m => m.Name == load.MachineName);
        }

        public Target(IMachine target)
        {
            _machine = target;
        }

        public string Name { get { return _machine.Name; } }
        public void SendNote(int chan, Note note, int velocity)
        {
            _machine.ParameterGroups[2].Parameters[0].SetValue(chan, note.Value);
            _machine.ParameterGroups[2].Parameters[1].SetValue(chan, velocity);
            _machine.SendControlChanges();
        }

        public void SendMIDINote(int channel, int note, int velocity)
        {
            _machine.SendMIDINote(channel, note, velocity);
        }

        public struct State
        {
            public string MachineName;
        }

        public State GetState()
        {
            return new State
            {
                MachineName = Name
            };
        }
    }
}

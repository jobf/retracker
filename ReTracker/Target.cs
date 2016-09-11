using System.Collections.Generic;
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
            _resetChannels();
        }

        public string Name { get { return _machine.Name; } }
        public void SendNote(Note note, int velocity)
        {
            var channel = GetChannel();
            lock (_machine)
            {   _machine.ParameterGroups[2].Parameters[0].SetValue(channel, note.Value);
                _machine.ParameterGroups[2].Parameters[1].SetValue(channel, velocity);
                _machine.SendControlChanges();
            }
        }

        public void SendMIDINote(int channel, int note, int velocity)
        {
            _machine.SendMIDINote(channel, note, velocity);
        }

        private int _nextChannel;
        private List<int> _channels = new List<int>();

        private void _resetChannels()
        {
            for (int i = 0; i < _machine.TrackCount; i++)
            {
                _channels.Add(i);
            }
        }

        private int GetChannel()
        {
            if(_channels.Count == 0)
            {
                _resetChannels();
            }
            var channel = _channels[_nextChannel];
            _nextChannel++;
            if (_nextChannel == _machine.TrackCount)
            {
                _nextChannel = 0;
            }
            return channel;
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

using Buzz.MachineInterface;

namespace ReTracker
{
    internal class Track
    {
        private string _machineName;
        private Note _note;

        public int NoteGet
        {
            get
            {
                if (_note.Value == 255)
                {
                    return _lastNote;
                }
                _lastNote = _note.ToMIDINote();
                return _lastNote;
            }
        }

        private int _lastNote;


        private bool _trigger;
        public bool ShouldTrigger { get { return _trigger; } }

        private int _volume = 100;
        public int VolumeGet { get { return _volume; } }

        public string MachineName
        {
            get
            {
                return _machineName;
            }

            set
            {
                _machineName = value;
            }
        }

        public void Note(Note v)
        {
            _note = v;
            _trigger = true;
        }

        public void Volume(int v)
        {
            _volume = v;
            _trigger = true;
        }
        
        internal void ResetTrigger()
        {
            _trigger = false;
        }

        public bool NoteOff()
        {
            return _note.Value == 255;
        }
    }
}

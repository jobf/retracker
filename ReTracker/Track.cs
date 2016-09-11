using Buzz.MachineInterface;

namespace ReTracker
{
    public class Track
    {
        private string _machineName;
        private Note _note;

        public int NoteGet
        {
            get
            {
                if (NoteOff())
                {
                    _note.Value = (byte)_lastNote;
                    return _lastNote;
                }
                _lastNote = _note.ToMIDINote();
                return _lastNote;
            }
        }

        private int _lastNote;
        private byte _lastNoteValue;
        private bool _trigger;
        public bool ShouldTrigger { get { return _trigger; } }

        private int _velocity;
        public int GetVelocityMIDI
        {
            get
            {
                return NoteOff() ? 0 : _velocity;
            }
        }

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

        public Note GetNote
        {
            get
            {
                if (!NoteOff())
                {
                    _lastNoteValue = _note.Value;
                }
                return _note;
            }
        }
        public int GetVelocity { get { return _velocity; } }

        public void Note(Note v)
        {
            _note = v;
            _trigger = true;
        }

        public void Velocity(int v)
        {
            _velocity = v;
            if (NoteOff())
            {
                _note.Value = _lastNoteValue;
            }
            _trigger = true;
        }

        public void ResetTrigger()
        {
            _trigger = false;
        }

        public bool NoteOff()
        {
            return _note.Value == 255;
        }
    }
}

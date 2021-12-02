using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Song", menuName = "Music/Song", order = 1)]
    public class Song : ScriptableObject
    {

        [SerializeField] private string songName;

        [SerializeField] private AudioClip[] notes;

        private int _currentNoteIndex;

        public AudioClip GetNoteAndShift()
        {
            if (_currentNoteIndex >= GetLength())
            {
                Rewind();
            }
            return notes[_currentNoteIndex++];
        }

        public void Rewind()
        {
            _currentNoteIndex = 0;
        }

        public AudioClip GetNote(int index)
        {
            return notes[index];
        }

        public int GetLength()
        {
            return notes.Length;
        }

    }
}

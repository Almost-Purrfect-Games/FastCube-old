using System;
using System.Reflection;
using Audio;
using ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField] private Song song;

        private AudioSource _audioSource;

        private Synthesizer _synthesizer;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _synthesizer = new Synthesizer();
            song.Rewind();
        }

        public void PlayRandomTone()
        {
            var random = new Random();
            var noteNumber = random.Next(0, song.GetLength());
            _audioSource.PlayOneShot(song.GetNote(noteNumber));
        }

        public void PlaySongNote()
        {
            /*var note = song.GetNoteAndShift();
            if (note)
            {
                _audioSource.PlayOneShot(note);
            }*/
            _synthesizer.PlayNote(_audioSource);
        }

        public void Rewind()
        {
            song.Rewind();
        }

    }
}

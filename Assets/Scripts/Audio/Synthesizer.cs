using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class Synthesizer
    {

        private int _sampleRate = 44100;
        private int _frequency = 440;
        private int _position = 0;

        public void PlayNote(AudioSource audioSource)
        {
            var samples = new float[audioSource.clip.samples * audioSource.clip.channels];
            audioSource.clip.GetData(samples, 0);

            for (int i = 0; i < samples.Length; ++i)
            {
                Debug.Log(samples[i]);
                samples[i] = samples[i] * 0.5f;
            }

            Debug.Log(samples);

            audioSource.clip.SetData(samples, 0);

            audioSource.Play();
        }
    }
}

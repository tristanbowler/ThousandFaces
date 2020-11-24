using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioClipCombiner;

namespace AudioClipCombiner
{
    [System.Serializable]
    public class AudioLayer
    {
        public AudioClip[] clip;
        [HideInInspector] public bool expanded = false;
        [HideInInspector] public int clipNumber = 0;
        [HideInInspector] public string name = "No Clips Added";
        public float volume = 1;
        public float delay;
        [HideInInspector] public Int16[] samples;
        [HideInInspector] public Byte[] bytes;
        [HideInInspector] public int sampleCount;
        [HideInInspector] public int delayCount;
        [HideInInspector] public int onClip = 0;

        
        public void GetSamples(int clipNumber)
        {
            samples = AudioClipCombinerV2.GetSamplesFromClip(clip[clipNumber], volume);
            delayCount = (int) (delay * clip[clipNumber].frequency * clip[clipNumber].channels);
            sampleCount = delayCount + samples.Length;
        }
    }
}
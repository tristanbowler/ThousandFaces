using UnityEngine;
using UnityEditor;
using AudioClipCombiner;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioClipScriptableObject", menuName = "ScriptableObjects/InfinityPBR/AudioClipScriptableObject", order = 1)]
public class AudioClipScriptableObject : ScriptableObject
{
    public string exportName = "CombinedAudioClip";
    public string exportPath = "Assets/";
    public bool overwriteExports = false;
    public List<AudioLayer> audioLayers = new List<AudioLayer>();
    

    public void AddLayer()
    {
        #if UNITY_EDITOR
        audioLayers.Add(new AudioLayer());
        EditorUtility.SetDirty(this);
        #endif
    }

    public void PlayRandom(AudioClipCombinerController controller)
    {
        Vector3 playPosition = Vector3.zero;
        for (int i = 0; i < audioLayers.Count; i++)
        {
            if (audioLayers[i].clip.Length > 0)
            {
                int playIndex = Random.Range(0, audioLayers[i].clip.Length);
                AudioClip playClip = audioLayers[i].clip[playIndex];
                controller.PlayTheClip(playClip, audioLayers[i].delay, playPosition);
            }
        }
    }
}

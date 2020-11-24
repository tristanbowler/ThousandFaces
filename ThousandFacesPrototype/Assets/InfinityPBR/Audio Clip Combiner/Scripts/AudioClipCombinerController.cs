using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipCombinerController : MonoBehaviour
{
    public ScriptableObject so;

    public void PlayTheClip(AudioClip clip, float delay, Vector3 playPosition, float volume = 1f)
    {
        StartCoroutine(DoPlay(clip, delay, playPosition, volume));
    }

    IEnumerator DoPlay(AudioClip clip, float delay, Vector3 playPosition, float volume = 1f)
    {
        yield return new WaitForSeconds(delay);
        //Debug.Log("Play " + clip.name + " at " + playPosition);
        AudioSource.PlayClipAtPoint(clip, playPosition, volume);
    }

}

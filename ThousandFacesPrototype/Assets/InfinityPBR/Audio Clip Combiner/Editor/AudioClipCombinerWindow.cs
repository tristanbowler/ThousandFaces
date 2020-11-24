using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using AudioClipCombiner;

[Serializable]
public class AudioClipCombinerWindow : EditorWindow
{
    private AudioClipScriptableObject audioClipScriptableObject;                                                        // The Scriptable object we are working on
    private string pathToObject;
    private string nameOfObject;
    // TO DO:
    // Cache the path and name, then reload from that after the update
    private int objectID;                                                       

    [MenuItem("Window/InfinityPBR/Audio Clip Combiner V2")]    // adds a menu item to load this window
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AudioClipCombinerWindow window = (AudioClipCombinerWindow)EditorWindow.GetWindow(typeof(AudioClipCombinerWindow));
        window.Show();
    }

    void OnFocus()
    {
        SaveObjectInfo();
    }

    void SaveObjectInfo()
    {
        if (audioClipScriptableObject)
        {
            pathToObject = AssetDatabase.GetAssetPath(audioClipScriptableObject);
            nameOfObject = audioClipScriptableObject.name;
            objectID = audioClipScriptableObject.GetInstanceID();

            Debug.Log("pathToObject: " + pathToObject);
            Debug.Log("nameOfObject: " + nameOfObject);
            Debug.Log("objectID: " + objectID);
        }
    }

    void OnGUI()
    {
        EditorGUILayout.HelpBox("CREATE NEW SCRIPTABLE OBJECT\nTo create a new Editor Clip ScriptableObject, " +
                                "navigate to where you'd like it to exist in your project, right-click and choose " +
                                "Create/ScriptableObjects/InfinityPBR/AudioClipScriptableObject then rename it for your" +
                                " purposes.", MessageType.Info);

        if (!audioClipScriptableObject)
        {
            EditorGUILayout.HelpBox("ASSIGN SCRIPTABLE OBJECT\nAssign your scriptable object to the box below. " +
                                    "You can create a new Audio Clip ScriptableObject using the instructions above.", MessageType.Warning);
        }
        
        // User defined scriptable object to manage
        audioClipScriptableObject = EditorGUILayout.ObjectField(audioClipScriptableObject, typeof(AudioClipScriptableObject), false) as AudioClipScriptableObject;

        // If we have a scriptable object assigned, then draw the rest of the window
        if (audioClipScriptableObject)
        {
            EditorGUILayout.Separator();
            DisplayTitle();
            DisplayAudioLayers();
            DisplayNewAudioLayerButton();
        }
    }

    private void DisplayTitle()
    {
        // Display Name & details
        EditorGUILayout.LabelField(audioClipScriptableObject.name + ": " +
                                   audioClipScriptableObject.audioLayers.Count + " layers, " + ClipCount() + " combined clips will be exported");
    }

    private void DisplayAudioLayers()
    {
        for (int i = 0; i < audioClipScriptableObject.audioLayers.Count; i++)
        {
            AudioLayer audioLayer = audioClipScriptableObject.audioLayers[i]; // cache this
            audioLayer.expanded = EditorGUILayout.Foldout(audioLayer.expanded, "Layer " + i);
            if (audioLayer.expanded)
            {
                EditorGUILayout.LabelField("Audio Layer Expanded");
            }
        }
    }

    private void DisplayNewAudioLayerButton()
    {
        if (GUILayout.Button("Create New Audio Layer"))
        {
            SaveObjectInfo();
            audioClipScriptableObject.AddLayer();
            AssetDatabase.ImportAsset(pathToObject);
            
            //SaveObjectInfo();
            //audioClipScriptableObject.audioLayers.Add(new AudioLayer());
            //LoadObject();
        }
    }

    private void LoadObject()
    {
        audioClipScriptableObject = AssetDatabase.LoadAssetAtPath<AudioClipScriptableObject>(pathToObject);
        //AudioClipScriptableObject tempObject = audioClipScriptableObject;
        //audioClipScriptableObject = null;
        //audioClipScriptableObject = tempObject;
    }

    /// <summary>
    /// Counts the total number of clips to be exported once all combinations are included
    /// </summary>
    /// <returns></returns>
    private int ClipCount()
    {
        int clips = 0; // Default is zero clips
        bool first = false;
        //Debug.Log("audioClipScriptableObject: " + audioClipScriptableObject.name);
        //Debug.Log("Layers: " + audioClipScriptableObject.audioLayers.Count);
        for (int i = 0; i < audioClipScriptableObject.audioLayers.Count; i++)
        {
            if (audioClipScriptableObject.audioLayers[i].clip.Length > 0)
            {
                if (!first)
                {
                    // For the first layer, we only include the clips in this layer
                    clips = audioClipScriptableObject.audioLayers[i].clip.Length;
                    first = true;
                }
                else
                {
                    // Multiply the total number so far by the number in the latest layer
                    clips = clips * audioClipScriptableObject.audioLayers[i].clip.Length;
                }
            }
        }
        return clips;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using AudioClipCombiner;

[CustomEditor(typeof(AudioClipScriptableObject))]
[CanEditMultipleObjects]
[Serializable]
public class AudioClipScriptableObjectInspector : Editor
{

    private bool showDefault = false;
    private AudioClipScriptableObject so;

    public void OnEnable()
    {
        so = (AudioClipScriptableObject)target;
    }
    
    public override void OnInspectorGUI()
    {
        ShowHeader();
        EditorGUILayout.Separator();
        
        ShowMainManagement();
        EditorGUILayout.Separator();
        
        ShowLayers();
        EditorGUILayout.Separator();

        ShowExport();
        EditorGUILayout.Separator();


        /*GUI.color = Color.green;
        if(GUILayout.Button("Export Clip Combinations"))									// If this button is clicked...
            AudioClipCombinerV2.SaveNow();															// Call the function
        GUI.color = Color.white;
        */

        
        EditorGUILayout.Separator();
        ShowDefaultInspector();

        EditorUtility.SetDirty(this);
    }

    public void ShowExport()
    {
        if (!AudioClipCombinerV2.AllLayersHaveClips(so))
        {
            GUI.color = Color.grey;
            if (GUILayout.Button("Export Clips"))
            {
                
            }
            GUI.color = Color.white;
        }
        else
        {
            if (GUILayout.Button("Export Clips"))
            {
                AudioClipCombinerV2.audioClipScriptableObject = so;
                AudioClipCombinerV2.SaveNow();
            }
        }
        
    }

    public void ShowLayers()
    {
        EditorGUILayout.HelpBox(so.audioLayers.Count + " layers, will export " + AudioClipCombinerV2.TotalExports(so) + " clips.", MessageType.None);

        if (so.audioLayers.Count == 0)
        {
            GUI.color = Color.green;
        }

        /*
        if (GUILayout.Button("Add audio layer"))
        {
            so.AddLayer();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(so));
            Repaint();
            GUI.color = Color.white;
        }
        */
    }

    public void ShowMainManagement()
    {
        EditorGUILayout.HelpBox("Set the Exported Clips Name, and each individual clip will receive a sequential " +
                                "integer suffix. Toggle \"Overwrite Exports\" true to overwrite previously exported clips. " +
                                "Set your preferred export path.", MessageType.Info);
        
        if (so.exportPath == "" || so.exportPath == null)
        {
            so.exportPath = "Assets/";
        }
        //EditorGUILayout.BeginHorizontal();
        so.exportName = EditorGUILayout.TextField("Exported Clips Name:", so.exportName);
        so.overwriteExports = EditorGUILayout.Toggle("Overwrite Exports", so.overwriteExports);
        //EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set", GUILayout.Width(50)))
        {
            string newPath = EditorUtility.OpenFolderPanel("Choose New Export Path", so.exportPath, "Choose");
            if (newPath.Contains(Application.dataPath))
            {
                newPath = newPath.Replace(Application.dataPath, "");
                so.exportPath = "Assets" + newPath;
            }
            else
            {
                Debug.LogError("Please choose a location in the project.");
            }
        }
        EditorGUILayout.LabelField("Export Path: " + so.exportPath);
        EditorGUILayout.EndHorizontal();
    }

    public void ShowHeader()
    {
        Texture titleTexture = Resources.Load("titleAudioClipCombiner") as Texture;
        float newWidth = ((Screen.width / 2) - 50);
        float newHeight = newWidth * 0.15f;
        GUILayout.Label(titleTexture, GUILayout.Width(newWidth), GUILayout.Height(newHeight)); ;
    }

    public void ShowDefaultInspector()
    {
        showDefault = EditorGUILayout.Foldout(showDefault, "Default Inspector");
        if (showDefault)
        {
            DrawDefaultInspector();	
        }
    }
}
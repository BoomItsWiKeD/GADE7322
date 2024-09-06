using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//This script works with the editor. Keep it in the Assets/Editor folder.
//All it does is makes a button which calls a method.

[CustomEditor(typeof(MapMaker))] //Used for auto-update
public class Editor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        //Code to specify which script has a custom GUI in the inspector:
        MapMaker mapMaker = (MapMaker)target;
        
        //The following line of code can be uncommented if editor must not auto-update.
        //DrawDefaultInspector();
        
        //This code is used to make the editor auto-update when changing variables in the inspector:
        if (DrawDefaultInspector())
        {
            mapMaker.callNoiseMap();
        }
        
        //Making a button in the inspector:
        if (GUILayout.Button("Generate Map (Check if all values >0!!!)"))
        {
            mapMaker.callNoiseMap();
        }
    }
}

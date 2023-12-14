using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectTool
{
    [MenuItem("Custom/ScriptableObject/AnimNames")]
    public static void CreateAnims()
    {
        AnimScriptableObject anims = ScriptableObject.CreateInstance<AnimScriptableObject>();
        AssetDatabase.CreateAsset(anims, "Assets/ScriptableObjects/AnimNames.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

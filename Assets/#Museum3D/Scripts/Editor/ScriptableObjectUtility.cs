using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static string CreateAsset<T> (string path, string name = null) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
 
        name = name ?? "New " + typeof(T).ToString();
        if (!name.EndsWith (".asset"))
            name += ".asset";
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name);
 
		AssetDatabase.CreateAsset (asset, assetPathAndName);
 
		AssetDatabase.SaveAssets ();
        	AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;

        return assetPathAndName;
	}
}
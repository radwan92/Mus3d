using UnityEngine;
using UnityEditor;
using Mus3d;
using System.IO;
using System;
using Mus3d.Anim;

public class ScriptableCreator
{
    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Weapon")]
	public static void CreateWeapon ()
	{
		ScriptableObjectUtility.CreateAsset<Weapon> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Item")]
	public static void CreateItem ()
	{
		ScriptableObjectUtility.CreateAsset<Item> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Enemy")]
	public static void CreateEnemy ()
	{
		ScriptableObjectUtility.CreateAsset<Enemy> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Animation/Enemy")]
	public static void CreateAnimation_Enemy ()
	{
		ScriptableObjectUtility.CreateAsset<EnemyAnimationData> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Animation/EnemyAnimationDataSet")]
	public static void CreateAnimationSet_Enemy ()
	{
		ScriptableObjectUtility.CreateAsset<EnemyAnimationDataSet> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/SpriteSheet")]
	public static void CreateSpriteSheet ()
	{
		ScriptableObjectUtility.CreateAsset<SpriteSheet> (GetSelectedPathOrFallback ());
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
	[MenuItem("Assets/Create/Animation/EnemyAllDirections")]
	public static void CreateAnimation_Enemy_AllDirections ()
	{
        int x = 0;

        foreach (Direction direction in Enum.GetValues (typeof (Direction)))
        {
            string path            = GetSelectedPathOrFallback ();
            string directionName   = direction.ToString ();
            string stateName       = Path.GetFileNameWithoutExtension (path);
            string baseName        = Path.GetFileNameWithoutExtension (Path.GetDirectoryName (path));

		    var assetPath = ScriptableObjectUtility.CreateAsset<EnemyAnimationData> (path, baseName + "_" + stateName + "_" + directionName);
            var animationData = AssetDatabase.LoadAssetAtPath<EnemyAnimationData> (assetPath);
            animationData.Direction = direction;
            animationData.SpritePositions = new Vector2[] { new Vector2 (x, 0) };
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();
            x++;
        }
	}

    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    public static string GetSelectedPathOrFallback()
	{
		string path = "Assets/#Museum3D";
		
		foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if ( !string.IsNullOrEmpty(path) && File.Exists(path) ) 
			{
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}
}
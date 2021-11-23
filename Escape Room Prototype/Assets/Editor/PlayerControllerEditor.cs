#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        PlayerController playerController = (PlayerController)target;
        
        playerController.Facing = (TileSide)EditorGUILayout.EnumPopup("Facing", playerController.Facing);
        
        // tileGrid.Factory = (TileFactory)EditorGUILayout.ObjectField("Tile Factory", tileGrid.Factory, typeof(TileFactory), true);


        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif
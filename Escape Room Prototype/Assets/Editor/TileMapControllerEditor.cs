#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(TileMapController))]
public class TileMapControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        TileMapController controller = (TileMapController)target;
        
        controller.TileTemplate = (TileController)EditorGUILayout.ObjectField("Tile Template", controller.TileTemplate, typeof(TileController), true);
        controller.Container = (Transform)EditorGUILayout.ObjectField("Tile Container", controller.Container, typeof(Transform), true);

        if(GUILayout.Button("Rebuild Map"))
        {
            controller.BuildTiles();
        }

        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif
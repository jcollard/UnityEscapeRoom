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
        
        Vector2Int pos = EditorGUILayout.Vector2IntField("Position", new Vector2Int(playerController.Position.x, playerController.Position.y));
        playerController.Position = (pos.x, pos.y);

        GUILayout.BeginHorizontal();


        if (GUILayout.Button("<┐"))
        {
            playerController.RotateLeft();
        }

        if (GUILayout.Button("^"))
        {
            playerController.MoveForward();
        }


        if (GUILayout.Button("┌>"))
        {
            playerController.RotateRight();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-"))
        {
            playerController.MoveLeft();
        }

        if (GUILayout.Button("v"))
        {
            playerController.MoveBackward();
        }


        if (GUILayout.Button("->"))
        {
            playerController.MoveRight();
        }
        GUILayout.EndHorizontal();


        
        
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
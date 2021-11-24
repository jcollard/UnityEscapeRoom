#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(MapBuilderController))]
public class MapBuilderControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        MapBuilderController playerController = (MapBuilderController)target;
        
        playerController.TileMapController = (TileMapController)EditorGUILayout.ObjectField("Tile Map Controller", playerController.TileMapController, typeof(TileMapController), true);

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

        if (GUILayout.Button("Wall"))
        {
            playerController.TileMapController.Map.ToggleWall(playerController.Position, playerController.Facing);
            // TODO (jcollard 11/23/2021): We probably shouldn't rebuild the entire map.
            // It would be useful to have BuildTiles take in a range to rebuild. 
            // This requires caching the locations of each TileController so we can either
            // just delete those OR update them directly.
            playerController.TileMapController.BuildTiles();
        }

        if (GUILayout.Button("Init Tile")) 
        {
            playerController.TileMapController.Map.InitTileAt(playerController.Position);
            // TODO (jcollard 11/23/2021): Don't rebuild entire map
            playerController.TileMapController.BuildTiles();
        }

        if (GUILayout.Button("Remove Tile"))
        {
            playerController.TileMapController.Map.RemoveTile(playerController.Position);
            // TODO (jcollard 11/23/2021): Don't rebuild entire map
            playerController.TileMapController.BuildTiles();
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
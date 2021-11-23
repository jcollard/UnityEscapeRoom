using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public class TileRenderer : MonoBehaviour
{
    
    [SerializeField] // TODO: Should I clear this out?
    private Dictionary<TileSide, MeshRenderer> _tileRenderers;

    private Dictionary<TileSide, MeshRenderer> MeshRenderers
    {
        get
        {
            if (_tileRenderers == null) {
                _tileRenderers = InitRenderers();
            }
            return _tileRenderers;
        }
    }


    [SerializeField]
    private Material _WallTexture;
    public Material WallTexture
    {
        get => _WallTexture;
        set
        {
            _WallTexture = value;
            foreach (TileSide side in TileUtils.WALLS)
            {
                MeshRenderers[side].material = _WallTexture;
            }
        }
    }

    [SerializeField]
    private Material _BottomTexture;
    public Material BottomTexture
    {
        get => _BottomTexture;
        set
        {
            _BottomTexture = value;
            MeshRenderers[TileSide.Bottom].material = _BottomTexture;
        }
    }

    [SerializeField]
    private Material _TopTexture;

    public Material TopTexture
    {
        get => _TopTexture;
        set
        {
            _TopTexture = value;
            MeshRenderers[TileSide.Top].material = _TopTexture;
        }
    }
    

    public void Start()
    {
    }

    private Dictionary<TileSide, MeshRenderer> InitRenderers()
    {
        Dictionary<TileSide, MeshRenderer> renderers = new Dictionary<TileSide, MeshRenderer>();
        // TODO: Potentially optimize this to not run if it is already initialized
        Transform n = this.transform.Find("Walls").Find("North");
        Transform e = this.transform.Find("Walls").Find("East");
        Transform s = this.transform.Find("Walls").Find("South");
        Transform w = this.transform.Find("Walls").Find("West");
        Transform bottom = this.transform.Find("Bottom");
        Transform top = this.transform.Find("Top");
        renderers[TileSide.North] = n.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.East] = e.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.South] = s.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.West] = w.gameObject.GetComponent<MeshRenderer>();
        renderers[TileSide.Top] = top.GetComponent<MeshRenderer>();
        renderers[TileSide.Bottom] = bottom.GetComponent<MeshRenderer>();
        return renderers;
    }

    public void SetTileSideActive(TileSide side, bool isActive)
    {
        this.MeshRenderers[side].gameObject.SetActive(isActive);
    }

    public bool GetTileSideActive(TileSide side)
    {
        return this.MeshRenderers[side].gameObject.activeInHierarchy;
    }
}

public class TileUtils
{
    public static readonly TileSide[] WALLS = new TileSide[]{TileSide.North, TileSide.East, TileSide.South, TileSide.West};
    public static readonly TileSide[] ALL = new TileSide[]{TileSide.North, TileSide.East, TileSide.South, TileSide.West, TileSide.Top, TileSide.Bottom};

    public static readonly Dictionary<TileSide, string> LABEL = new Dictionary<TileSide, string>();

    static TileUtils()
    {
        LABEL[TileSide.North] = "North";
        LABEL[TileSide.East] = "East";
        LABEL[TileSide.South] = "South";
        LABEL[TileSide.West] = "West";
        LABEL[TileSide.Top] = "Top";
        LABEL[TileSide.Bottom] = "Bottom";
    }

}

public enum TileSide
{
    North,
    East,
    South,
    West,
    Top,
    Bottom
}

[CustomEditor(typeof(TileRenderer))]
public class TileRendererEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        TileRenderer tileRenderer = (TileRenderer)target;
        
        tileRenderer.WallTexture = (Material)EditorGUILayout.ObjectField("Wall Texture", tileRenderer.WallTexture, typeof(Material), false);
        tileRenderer.BottomTexture = (Material)EditorGUILayout.ObjectField("Bottom Texture", tileRenderer.BottomTexture, typeof(Material), false);
        tileRenderer.TopTexture = (Material)EditorGUILayout.ObjectField("Top Texture", tileRenderer.TopTexture, typeof(Material), false);

        foreach (TileSide side in TileUtils.ALL)
        {
            tileRenderer.SetTileSideActive(side, EditorGUILayout.Toggle(TileUtils.LABEL[side], tileRenderer.GetTileSideActive(side)));
        }
        
        
        // tileGrid.Factory = (TileFactory)EditorGUILayout.ObjectField("Tile Factory", tileGrid.Factory, typeof(TileFactory), true);


        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}

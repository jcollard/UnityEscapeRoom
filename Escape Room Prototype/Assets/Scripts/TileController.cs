using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.TileBuilder;

public class TileController : MonoBehaviour, ITile
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

    public bool HasSide(TileSide side)
    {
        return this.MeshRenderers[side].gameObject.activeInHierarchy;
    }

    public void SetSide(TileSide side, bool isWall)
    {
        this.MeshRenderers[side].gameObject.SetActive(isWall);
    }
}
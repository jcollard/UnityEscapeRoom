using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class TileMapController : MonoBehaviour
{

    public TileController TileTemplate;
    public Transform Container;

    [SerializeField]
    private Material _WallTexture;
    public Material WallTexture
    {
        get => _WallTexture;
        set => this.UpdateIfDifferent(value, ref _WallTexture);
    }

    [SerializeField]
    private Material _TopTexture;
    public Material TopTexture
    {
        get => _TopTexture;
        set => this.UpdateIfDifferent(value, ref _TopTexture);
    }

    [SerializeField]
    private Material _BottomTexture;
    public Material BottomTexture
    {
        get => _BottomTexture;
        set => this.UpdateIfDifferent(value, ref _BottomTexture);
    }
    
    private void UpdateIfDifferent<T>(T value, ref T any)
    {
        if (value == null || any == null || value.Equals(any))
        {
            return;
        }
        any = value;
        this.BuildTiles();
    }

    [SerializeField]
    private TileMapJSON _JSON; 
    public TileMapJSON JSON 
    {
        get => _JSON; 
        private set
        {
            this._JSON = value;        
        }
    }

    private TileMap _Map;
    public TileMap Map
    {
        get
        {
            if (this._Map == null || this._Map.IsEmpty)
            {
                this._Map = this.JSON.ToTileMap();
            }
            return this._Map;
        }
    }

    private TileController CreateTile(ITile tile)
    {
        TileController newTile = UnityEngine.Object.Instantiate<TileController>(TileTemplate);
        foreach (TileSide side in TileUtils.ALL)
        {
            newTile.SetSide(side, tile.HasSide(side) ? tile.GetSide(side) : WallType.None);
        }
        return newTile;
    }

    public void BuildTiles()
    {
        Collard.UnityUtils.DestroyImmediateChildren(this.Container);
        TileTemplate.gameObject.SetActive(false);
        foreach ((int x, int y) pos in this.Map.GetAllPos())
        {
            TileController tile = this.CreateTile(this.Map.GetTile(pos));
            tile.WallTexture = this.WallTexture;
            tile.TopTexture = this.TopTexture;
            tile.BottomTexture = this.BottomTexture;
            tile.gameObject.SetActive(true);
            tile.name = $"(x: {pos.x}, y: {pos.y})";
            tile.transform.parent = this.Container;
            tile.transform.localPosition = new Vector3((float)(pos.x * 10), 0, (float)(pos.y * 10));
        }
        this.JSON = new TileMapJSON(this.Map);
    }
}

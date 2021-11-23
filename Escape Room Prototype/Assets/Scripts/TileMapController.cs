using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class TileMapController : MonoBehaviour
{

    public TileController TileTemplate;
    public Transform Container;
    public TileMap<TileController> map;
    public TileMapController()
    {
        this.map = new TileMap<TileController>(this.CreateTile);
        this.map.InitTileAt((0, 0));
        this.map.InitTileAt((1, 0));
        this.map.InitTileAt((2, 0));
        this.map.InitTileAt((0, 1));
        this.map.InitTileAt((1, 1));
        this.map.InitTileAt((2, 1));
        this.map.RemoveWall((0, 0), TileSide.North);
        this.map.RemoveWall((0, 0), TileSide.East);
        this.map.RemoveWall((1, 0), TileSide.North);
        this.map.RemoveWall((1, 0), TileSide.East);
        this.map.RemoveWall((2, 0), TileSide.North);
        this.map.RemoveWall((0, 1), TileSide.East);
        this.map.RemoveWall((1, 1), TileSide.East);


    }

    private TileController CreateTile(ITile tile)
    {
        TileController newTile = UnityEngine.Object.Instantiate<TileController>(TileTemplate);
        foreach (TileSide side in TileUtils.ALL)
        {
            newTile.SetSide(side, tile.HasSide(side));
        }
        return newTile;
    }

    public void BuildTiles()
    {
        Collard.UnityUtils.DestroyImmediateChildren(this.Container);
        TileTemplate.gameObject.SetActive(false);
        foreach ((int x, int y) pos in this.map.GetAllPos())
        {
            TileController tile = this.map.ConstructTile(pos);
            tile.gameObject.SetActive(true);
            tile.name = $"(x: {pos.x}, y: {pos.y})";
            tile.transform.parent = this.Container;
            tile.transform.localPosition = new Vector3((float)(pos.x * 10), 0, (float)(pos.y * 10));
        }
    }
}

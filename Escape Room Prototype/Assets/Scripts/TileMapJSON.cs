using System.Collections.Generic;
using System;
using CaptainCoder.TileBuilder;
using UnityEngine;

[Serializable]
public class TileMapJSON
{
    public List<TileInfo> tiles = new List<TileInfo>();

    public TileMapJSON(TileMap map)
    {
        foreach ((int x, int y) in map.GetAllPos())
        {
            TileInfo info = new TileInfo();
            info.X = x;
            info.Y = y;
            ITile tile = map.GetTile((x, y));
            info.Top = tile.HasSide(TileSide.Top);
            info.Bottom = tile.HasSide(TileSide.Bottom);
            info.North = tile.HasSide(TileSide.North);
            info.South = tile.HasSide(TileSide.South);
            info.East = tile.HasSide(TileSide.East);
            info.West = tile.HasSide(TileSide.West);
            this.tiles.Add(info);
        }
    }

    public TileMap ToTileMap()
    {
        TileMap map = new TileMap();
        foreach (TileInfo info in this.tiles)
        {
            (int, int) pos = (info.X, info.Y);
            map.InitTileAt(pos);
            map.SetWall(pos, TileSide.North, info.North);
            map.SetWall(pos, TileSide.South, info.South);
            map.SetWall(pos, TileSide.East, info.East);
            map.SetWall(pos, TileSide.West, info.West);
            map.SetWall(pos, TileSide.Top, info.Top);
            map.SetWall(pos, TileSide.Bottom, info.Bottom);
        }
        return map;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this, true);
    }


    public static string ToJSON(TileMap map)
    {
        TileMapJSON jsonable = new TileMapJSON(map);
        return JsonUtility.ToJson(jsonable);
    }
}

[Serializable]
public class TileInfo
{
    public int X;
    public int Y;
    public bool Top;
    public bool Bottom;
    public bool North;
    public bool East;
    public bool South;
    public bool West;
}
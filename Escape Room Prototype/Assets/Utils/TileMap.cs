namespace CaptainCoder.TileBuilder
{
    using System.Collections.Generic;
    using System;

    public class TileMap<T> where T : ITile
    {


        private static readonly Dictionary<TileSide, (int, int, TileSide)> NEIGHBOR_INFO = new Dictionary<TileSide, (int, int, TileSide)>();

        static TileMap() {
            NEIGHBOR_INFO[TileSide.West]  = (-1,  0, TileSide.East);
            NEIGHBOR_INFO[TileSide.East]  = ( 1,  0, TileSide.West);
            NEIGHBOR_INFO[TileSide.South] = ( 0, -1, TileSide.South);
            NEIGHBOR_INFO[TileSide.North] = ( 0,  1, TileSide.North);

        }

        private readonly Dictionary<(int, int), T> tiles = new Dictionary<(int, int), T>();
        private readonly Func<ITile, T> TileFactory;

        public TileMap(Func<ITile, T> TileFactory)
        {
            this.TileFactory = TileFactory;
        }

        public bool HasTile((int x, int y) pos)
        {
            return this.tiles.ContainsKey(pos);
        }

        public T InitTileAt((int x, int y) pos)
        {
            if (this.tiles.TryGetValue(pos, out T tile))
            {
                return tile;
            }

            ITile newTile = new BasicTile();

            // If neighbors exist on any side, copy the wall configuration.
            foreach(TileSide mySide in TileUtils.ALL)
            {
                (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[mySide];
                if (this.HasTile((pos.x + offX, pos.y + offY)))
                {
                    newTile.SetSide(mySide, this.GetTile((pos.x + offX, pos.y + offY)).HasSide(neighborSide));
                }
            }

            tile = this.TileFactory(newTile);
            return tile;
        }

        public T GetTile((int x, int y) pos)
        {
            this.tiles.TryGetValue(pos, out T tile);
            return tile;
        }

        public TileMap<T> RemoveTile((int x, int y) pos)
        {
            this.tiles.Remove(pos);
            return this;
        }

        public TileMap<T> SetWall((int x, int y) pos, TileSide side, bool isWall)
        {
            if (!this.HasTile(pos))
            {
                this.InitTileAt(pos);
            }

            // Set the wall for this tile
            this.GetTile(pos).SetSide(side, isWall);

            // Check for neighbor, if the neighbor exists update neighbors wall to match.
            (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[side];
            if (this.HasTile((pos.x + offX, pos.y + offY)))
            {
                this.GetTile((pos.x + offX, pos.y + offY)).SetSide(neighborSide, isWall);
            }

            return this;
        }

        public TileMap<T> AddWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, true);
        }

        public TileMap<T> RemoveWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, false);
        }
    }

    public interface ITile
    {
        bool HasSide(TileSide side);
        void SetSide(TileSide side, bool isWall);
    }

    internal class BasicTile : ITile
    {
        private readonly Dictionary<TileSide, bool> IsWall = new Dictionary<TileSide, bool>();

        public BasicTile()
        {
            foreach (TileSide side in TileUtils.ALL)
            {
                IsWall[side] = true;
            }
        }

        public bool HasSide(TileSide side)
        {
            return this.IsWall[side];
        }

        public void SetSide(TileSide side, bool isWall)
        {
            this.IsWall[side] = isWall;
        }
    }

}
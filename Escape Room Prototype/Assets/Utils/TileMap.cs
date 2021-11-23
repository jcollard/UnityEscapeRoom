namespace CaptainCoder.TileBuilder
{
    using System.Collections.Generic;
    using System;

    public class TileMap<T> where T : ITile
    {
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
            if(this.tiles.TryGetValue(pos, out T tile))
            {
                return tile;
            }

            // TODO: Check neighbors and get the wall information from the neighbors.
            ITile newTile = new BasicTile();
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

        public TileMap<T> SetWall(int x, int y, TileSide side, bool isWall)
        {
            return this;
        }

        public TileMap<T> AddWall(int x, int y, TileSide side)
        {
            return this.SetWall(x, y, side, true);
        }

        public TileMap<T> RemoveWall(int x, int y, TileSide side)
        {
            return this.SetWall(x, y, side, false);
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
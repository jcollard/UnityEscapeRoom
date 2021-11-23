namespace CaptainCoder.TileBuilder
{
    using System.Collections.Generic;
    using System;
    using System.Text;

    [Serializable]
    public class TileMap
    {
        private static readonly Dictionary<TileSide, (int, int, TileSide)> NEIGHBOR_INFO = new Dictionary<TileSide, (int, int, TileSide)>();

        static TileMap()
        {
            NEIGHBOR_INFO[TileSide.West] = (-1, 0, TileSide.East);
            NEIGHBOR_INFO[TileSide.East] = (1, 0, TileSide.West);
            NEIGHBOR_INFO[TileSide.South] = (0, -1, TileSide.North);
            NEIGHBOR_INFO[TileSide.North] = (0, 1, TileSide.South);

        }

        public bool IsEmpty { get => tiles.Count == 0; }

        private readonly Dictionary<(int, int), ITile> tiles = new Dictionary<(int, int), ITile>();

        public IEnumerable<(int, int)> GetAllPos()
        {
            return this.tiles.Keys;
        }

        public bool HasTile((int x, int y) pos)
        {
            return this.tiles.ContainsKey(pos);
        }

        public ITile InitTileAt((int x, int y) pos)
        {
            if (this.tiles.TryGetValue(pos, out ITile tile))
            {
                return tile;
            }

            ITile newTile = new BasicTile();

            // If neighbors exist on any side, copy the wall configuration.
            foreach (TileSide mySide in TileUtils.WALLS)
            {
                (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[mySide];
                if (this.HasTile((pos.x + offX, pos.y + offY)))
                {
                    newTile.SetSide(mySide, this.GetTile((pos.x + offX, pos.y + offY)).HasSide(neighborSide));
                }
            }
            this.tiles[pos] = newTile;
            return newTile;
        }

        public ITile GetTile((int x, int y) pos)
        {
            this.tiles.TryGetValue(pos, out ITile tile);
            return tile;
        }

        public TileMap RemoveTile((int x, int y) pos)
        {
            this.tiles.Remove(pos);
            return this;
        }

        public TileMap SetWall((int x, int y) pos, TileSide side, bool isWall)
        {
            if (!this.HasTile(pos))
            {
                this.InitTileAt(pos);
            }

            // Set the wall for this tile
            this.GetTile(pos).SetSide(side, isWall);

            if (NEIGHBOR_INFO.ContainsKey(side))
            {
                // Check for neighbor, if the neighbor exists update neighbors wall to match.
                (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[side];
                if (this.HasTile((pos.x + offX, pos.y + offY)))
                {
                    this.GetTile((pos.x + offX, pos.y + offY)).SetSide(neighborSide, isWall);
                }
            }

            return this;
        }

        public TileMap AddWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, true);
        }

        public TileMap RemoveWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, false);
        }

        public TileMap ToggleWall((int x, int y) pos, TileSide side)
        {
            if (this.HasTile(pos))
            {
                return this.SetWall(pos, side, !this.GetTile(pos).HasSide(side));
            }
            return this;
        }
    }

    public interface ITile
    {
        bool HasSide(TileSide side);
        void SetSide(TileSide side, bool isWall);
    }

    [Serializable]
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
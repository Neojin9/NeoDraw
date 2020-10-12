using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using Terraria;
using Terraria.ID;


namespace NeoDraw.Undo {
    
    public class UndoStep {

        #region Variables

        #region Private Variables

        private readonly HashSet<int>      _locations;
        private readonly List<ChangedTile> _changedTiles;

        #endregion

        #region Public Variables

        public int Count { get { return _changedTiles.Count; } }

        public string  Action { get; set; }

        #endregion

        #endregion

        public UndoStep() {

            _changedTiles = new List<ChangedTile>();
            _locations    = new HashSet<int>();

        }

        public UndoStep(string action) {

            Action        = action;
            _changedTiles = new List<ChangedTile>();
            _locations    = new HashSet<int>();

        }

        #region Public Functions

        public void Add(ChangedTile newChange) {

            int hash = GetHash(newChange.Location.X, newChange.Location.Y);

            if (_locations.Contains(hash))
                return;

            _locations.Add(hash);
            _changedTiles.Add(newChange);

        }

        public IList<Point> GetChangedTilesList() {
            
            IList<Point> list = new List<Point>();

            for (int i = 0; i < _changedTiles.Count; i++)
                list.Add(_changedTiles[i].Location);

            return list;

        }

        public Tile GetTileAtIndex(int index) {
            
            if (index >= Count)
                return new Tile();

            return _changedTiles[index].Tile;

        }

        public void Remove(ChangedTile oldChange) {

            int hash = GetHash(oldChange.Location.X, oldChange.Location.Y);

            if (!_locations.Contains(hash))
                return;

            _locations.Remove(hash);
            _changedTiles.Remove(oldChange);

        }

        public void ResetFrames(bool wallToo = false) {

            if (_changedTiles == null)
                return;

            if (Count <= 0)
                return;
            
            WorldGen.noTileActions = true;
            WorldGen.destroyObject = true;

            for (int i = 0; i < _changedTiles.Count; i++) {

               WorldGen.SquareTileFrame(_changedTiles[i].Location.X, _changedTiles[i].Location.Y);

                if (wallToo)
                    WorldGen.SquareWallFrame(_changedTiles[i].Location.X, _changedTiles[i].Location.Y);

            }
            
            WorldGen.noTileActions = false;
            WorldGen.destroyObject = false;

        }

        public void Undo() {

            for (int i = 0; i < _changedTiles.Count; i++) {

                int x = _changedTiles[i].Location.X;
                int y = _changedTiles[i].Location.Y;

                Chest tempChest = null;
                Tile tempTile = new Tile(Main.tile[x, y]);

                if (Neo.IsTopLeft(x, y)) {

                    if (TileID.Sets.BasicChest[tempTile.type] || Main.tileContainer[tempTile.type]) {

                        int chestToDestroyIndex = Chest.FindChest(x, y);

                        if (chestToDestroyIndex != -1) {

                            tempChest = Main.chest[chestToDestroyIndex];
                            Chest.DestroyChestDirect(x, y, chestToDestroyIndex);

                        }

                    }

                }

                Main.tile[x, y] = new Tile(_changedTiles[i].Tile);

                if (Neo.IsTopLeft(x, y)) {

                    if (TileID.Sets.BasicChest[Main.tile[x, y].type] || Main.tileContainer[Main.tile[x, y].type]) {

                        int chestIndex = Chest.CreateChest(x, y);

                        if (chestIndex == -1)
                            chestIndex = Chest.FindChest(x, y);

                        if (chestIndex != -1)
                            Main.chest[chestIndex] = _changedTiles[i].Chest;

                    }

                }

                _changedTiles[i].Tile = new Tile(tempTile);
                _changedTiles[i].Chest = tempChest;

            }

            ResetFrames(true);

        }

        #endregion

        #region Private Functions

        private static int GetHash(int a, int b) {

            a = a >= 0 ? 2 * a : -2 * a - 1;
            b = b >= 0 ? 2 * b : -2 * b - 1;

            return a >= b ? a * a + a + b : a + b * b;

        }

        #endregion

    }

}

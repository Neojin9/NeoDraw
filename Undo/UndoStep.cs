using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;


namespace NeoDraw.Undo {
    
    public class UndoStep {

        #region Variables

        #region Private Variables

        private readonly HashSet<int>      _locations;
        private readonly List<ChangedTile> _changedTiles;

        #endregion

        #region Public Variables

        public static bool ResetFramesInProgress;

        public int Count => _changedTiles.Count;

        public string  Action { get; }

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

            foreach (ChangedTile tile in _changedTiles)
                list.Add(tile.Location);

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

            ResetFramesInProgress = true;

            foreach (ChangedTile tile in _changedTiles) {

                WorldGen.SquareTileFrame(tile.Location.X, tile.Location.Y);

                if (wallToo)
                    WorldGen.SquareWallFrame(tile.Location.X, tile.Location.Y);

            }

            ResetFramesInProgress = false;

        }

        public void Undo() {

            foreach (ChangedTile tile in _changedTiles) {

                int x = tile.Location.X;
                int y = tile.Location.Y;

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

                Main.tile[x, y] = new Tile(tile.Tile);

                if (Neo.IsTopLeft(x, y)) {

                    ushort type = Main.tile[x, y].type;

                    /*if (type == TileID.PartyMonolith) {
                        
                        if (Main.tile[x, y].frameY != 0 && !BirthdayParty.PartyIsUp) {
                            BirthdayParty.ToggleManualParty();
                        }

                    }
                    else*/ if (TileID.Sets.BasicChest[type] || Main.tileContainer[type]) {

                        int chestIndex = Chest.CreateChest(x, y);

                        if (chestIndex == -1)
                            chestIndex = Chest.FindChest(x, y);

                        if (chestIndex != -1)
                            Main.chest[chestIndex] = tile.Chest;

                    }

                }

                tile.Tile = new Tile(tempTile);
                tile.Chest = tempChest;

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

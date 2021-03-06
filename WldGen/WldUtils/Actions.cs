﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;
using static NeoDraw.UI.DrawInterface;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/23/2020

    public static class Actions {

        public class ContinueWrapper : GenAction {

            private GenAction _action;

            public ContinueWrapper(GenAction action) {
                _action = action;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                _action.Apply(origin, x, y, args);
                return UnitApply(origin, x, y, args);
            }

        }

        public class Count : GenAction {

            private Ref<int> _count;

            public Count(Ref<int> count) {
                _count = count;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                _count.Value++;
                return UnitApply(origin, x, y, args);
            }

        }

        public class Scanner : GenAction {

            private Ref<int> _count;

            public Scanner(Ref<int> count) {
                _count = count;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                _count.Value++;
                return UnitApply(origin, x, y, args);
            }

        }

        public class TileScanner : GenAction {

            private ushort[] _tileIds;

            private Dictionary<ushort, int> _tileCounts;

            public TileScanner(params ushort[] tiles) {

                _tileIds = tiles;
                _tileCounts = new Dictionary<ushort, int>();

                for (int i = 0; i < tiles.Length; i++)
                    _tileCounts[_tileIds[i]] = 0;

            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                Tile tile = _tiles[x, y];

                if (tile.active() && _tileCounts.ContainsKey(tile.type))
                    _tileCounts[tile.type]++;

                return UnitApply(origin, x, y, args);

            }

            public TileScanner Output(Dictionary<ushort, int> resultsOutput) {

                _tileCounts = resultsOutput;

                for (int i = 0; i < _tileIds.Length; i++)
                    if (!_tileCounts.ContainsKey(_tileIds[i]))
                        _tileCounts[_tileIds[i]] = 0;

                return this;

            }

            public Dictionary<ushort, int> GetResults() {
                return _tileCounts;
            }

            public int GetCount(ushort tileId) {

                if (!_tileCounts.ContainsKey(tileId))
                    return -1;

                return _tileCounts[tileId];

            }

        }

        public class Blank : GenAction {

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                return UnitApply(origin, x, y, args);
            }

        }

        public class Custom : GenAction {

            private CustomPerUnitAction _perUnit;

            public Custom(CustomPerUnitAction perUnit) {
                _perUnit = perUnit;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                return _perUnit(x, y, args) | UnitApply(origin, x, y, args);
            }

        }

        public class ClearMetadata : GenAction {

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].ClearMetadata();

                return UnitApply(origin, x, y, args);

            }

        }

        public class Clear : GenAction {

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].ClearEverything();

                return UnitApply(origin, x, y, args);

            }

        }

        public class ClearTile : GenAction {

            private bool _frameNeighbors;

            public ClearTile(bool frameNeighbors = false) {
                _frameNeighbors = frameNeighbors;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                
                AddChangedTile(x, y);
                ClearTile(x, y, _frameNeighbors);

                return UnitApply(origin, x, y, args);

            }

        }

        public class ClearWall : GenAction {

            private bool _frameNeighbors;

            public ClearWall(bool frameNeighbors = false) {
                _frameNeighbors = frameNeighbors;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                ClearWall(x, y, _frameNeighbors);

                return UnitApply(origin, x, y, args);

            }

        }

        public class HalfBlock : GenAction {

            private bool _value;

            public HalfBlock(bool value = true) {
                _value = value;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].halfBrick(_value);

                return UnitApply(origin, x, y, args);

            }

        }

        public class SetTile : GenAction {

            private ushort _type;

            private bool _doFraming;
            private bool _doNeighborFraming;

            public SetTile(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true) {

                _type = type;
                _doFraming = setSelfFrames;
                _doNeighborFraming = setNeighborFrames;

            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);

                _tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
                _tiles[x, y].type = _type;
                _tiles[x, y].active(active: true);

                if (_doFraming)
                    WorldGen.TileFrame(x, y, _doNeighborFraming);

                return UnitApply(origin, x, y, args);

            }

        }

        public class SetTileKeepWall : GenAction {

            private ushort _type;

            private bool _doFraming;

            private bool _doNeighborFraming;

            public SetTileKeepWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true) {
                
                _type = type;
                _doFraming = setSelfFrames;
                _doNeighborFraming = setNeighborFrames;

            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);

                ushort wall = _tiles[x, y].wall;
                int wallFrameX = _tiles[x, y].wallFrameX();
                int wallFrameY = _tiles[x, y].wallFrameY();

                _tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
                _tiles[x, y].type = _type;
                _tiles[x, y].active(active: true);

                if (wall > 0) {

                    _tiles[x, y].wall = wall;
                    _tiles[x, y].wallFrameX(wallFrameX);
                    _tiles[x, y].wallFrameY(wallFrameY);

                }

                if (_doFraming)
                    WorldGen.TileFrame(x, y, _doNeighborFraming);
                
                return UnitApply(origin, x, y, args);

            }

        }

        public class DebugDraw : GenAction {

            private Color _color;

            private SpriteBatch _spriteBatch;

            public DebugDraw(SpriteBatch spriteBatch, Color color = default) {
                _spriteBatch = spriteBatch;
                _color = color;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                _spriteBatch.Draw(Main.magicPixel, new Rectangle((x << 4) - (int)Main.screenPosition.X, (y << 4) - (int)Main.screenPosition.Y, 16, 16), _color);
                return UnitApply(origin, x, y, args);
            }

        }

        public class SetSlope : GenAction {

            private int _slope;

            public SetSlope(int slope) {
                _slope = slope;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                AddChangedTile(x, y);
                WorldGen.SlopeTile(x, y, _slope);
                return UnitApply(origin, x, y, args);
            }

        }

        public class SetHalfTile : GenAction {

            private bool _halfTile;

            public SetHalfTile(bool halfTile) {
                _halfTile = halfTile;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].halfBrick(_halfTile);

                return UnitApply(origin, x, y, args);

            }

        }

        public class PlaceTile : GenAction {

            private ushort _type;
            private int _style;

            public PlaceTile(ushort type, int style = 0) {
                _type = type;
                _style = style;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                PlaceTile(x, y, _type, ref DrawInterface.Undo, mute: true, forced: false, -1, _style);
                return UnitApply(origin, x, y, args);

            }

        }

        public class RemoveWall : GenAction {

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].wall = 0;

                return UnitApply(origin, x, y, args);

            }

        }

        public class PlaceWall : GenAction {

            private ushort _type;
            private bool _neighbors;

            public PlaceWall(ushort type, bool neighbors = true) {
                _type = type;
                _neighbors = neighbors;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].wall = _type;
                WorldGen.SquareWallFrame(x, y);

                if (_neighbors) {
                    WorldGen.SquareWallFrame(x + 1, y);
                    WorldGen.SquareWallFrame(x - 1, y);
                    WorldGen.SquareWallFrame(x, y - 1);
                    WorldGen.SquareWallFrame(x, y + 1);
                }

                return UnitApply(origin, x, y, args);

            }

        }

        public class SetLiquid : GenAction {

            private int _type;
            private byte _value;

            public SetLiquid(int type = 0, byte value = byte.MaxValue) {
                _type = type;
                _value = value;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                _tiles[x, y].liquidType(_type);
                _tiles[x, y].liquid = _value;

                return UnitApply(origin, x, y, args);

            }

        }

        public class SwapSolidTile : GenAction {

            private ushort _type;

            public SwapSolidTile(ushort type) {
                _type = type;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                Tile tile = _tiles[x, y];

                if (WorldGen.SolidTile(tile)) {

                    AddChangedTile(x, y);
                    tile.ResetToType(_type);

                    return UnitApply(origin, x, y, args);

                }

                return Fail();

            }

        }

        public class SetFrames : GenAction {

            private bool _frameNeighbors;

            public SetFrames(bool frameNeighbors = false) {
                _frameNeighbors = frameNeighbors;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {
                WorldGen.TileFrame(x, y, _frameNeighbors);
                return UnitApply(origin, x, y, args);
            }

        }

        public class Smooth : GenAction {

            private bool _applyToNeighbors;

            public Smooth(bool applyToNeighbors = false) {
                _applyToNeighbors = applyToNeighbors;
            }

            public override bool Apply(Point origin, int x, int y, params object[] args) {

                AddChangedTile(x, y);
                SmoothSlope(x, y, ref DrawInterface.Undo, _applyToNeighbors);

                return UnitApply(origin, x, y, args);

            }

        }

        public static GenAction Chain(params GenAction[] actions) {

            for (int i = 0; i < actions.Length - 1; i++)
                actions[i].NextAction = actions[i + 1];

            return actions[0];

        }

        public static GenAction Continue(GenAction action) => new ContinueWrapper(action);

    }

}

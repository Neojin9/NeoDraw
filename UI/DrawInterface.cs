using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NeoDraw.Core;
using NeoDraw.Undo;
using NeoDraw.WldGen.Dungeon;
using NeoDraw.WldGen.MicroBiomes;
using NeoDraw.WldGen.WldUtils;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;
using static NeoDraw.WldGen.WldUtils.WldUtils;
using static Terraria.ModLoader.ModContent;
using TOD = Terraria.ObjectData.TileObjectData;

namespace NeoDraw.UI {

    public class DrawInterface : UIState {

        #region Variables

        #region Constants

        private const byte BoxPadding = 10;
        private const byte ButtonHeight = 40;
        private const byte ButtonWidth = 40;
        private const byte HoverBoxPadding = 2;
        private const byte ListItemHeight = 20;
        private const byte MaxBrushSize = 30;
        private const byte SearchBarHeight = 32;
        private const byte SearchTop = 188;
        private const byte SelectionFrameCount = 6;
        private const byte SelectionFrameCounterMax = 8;
        private const byte StatusBarHeight = 35;
        private const byte TabHeight = 28;
        private const byte TextBoxMouseOffsetX = 10;
        private const byte TextBoxMouseOffsetY = 0;
        private const byte TextBoxSidePadding = 10;
        private const byte TextBoxTopPadding = 10;

        private const byte MiddleListTop = SearchTop - 2 + SearchBarHeight;
        private const byte TopBoxHeight = SearchTop - TabHeight + 1;

        private const float ActiveAlpha = 1.0f;
        private const float InactiveAlpha = 0.25f;
        private const float PaddingSide = 8.5f;

        private const int MaxFloodFill = 2500;

        public const int KeyDelay = 400;
        public const int KeyDelayQuick = 50;
        public const int ListWidth = 300;

        public const string AltKey = "ALT";

        #endregion Constants

        #region Private Variables

        private static bool _buttonClicked;
        private static bool FrameCounterUpdated;
        private static bool LeftClickHeld;
        private static bool _onScreenOnly;
        private static bool Pasting;
        private static bool RightClickHeld;
        private static bool _scrolling;
        private static bool _showGPS;
        private static bool _subScrolling;
        private static byte CurrentTab = Tabs.Tiles;
        private static byte DayNightOption;
        private static byte GridStyle;
        private static byte LeftClickHoldTime;
        private static byte RightClickHoldTime;
        private static byte SelectionFrameCounter = SelectionFrameCounterMax;
        private static byte SelectionFrameCurrent;

        private static byte[] CurrentSorter = new byte[4];

        private static DateTime _clickDelay = DateTime.MinValue;
        private static DateTime KeyPressDelay = DateTime.MinValue;

        private static int _brushSize = 1;
        private static int _currentSubOther = -1;
        private static int _currentSubStructure = -1;
        private static int _currentSubTile = -1;
        private static int _floodFillCounter;
        private static int _oldScreenHeight = -1;
        private static int _oldScreenWidth = -1;
        private static int _otherScroll;
        private static int _otherStyle;
        private static int _placeStyle;
        private static int _specialStyle;
        private static int statusBarTempMessageTimer;
        private static int _structureScroll;
        private static int _subScroll;
        private static int TileFrameY;
        private static int _tileScroll;
        private static int _wallScroll;

        private static string _hoverText;
        private static string statusBarTempMessage;

        private static Vector2 RulerStartPoint;
        private static Vector2 StartPoint;

        public static UndoStep _undo;
        private static UndoStep CurrentSelection;

        private static readonly int[] bigBrushCompatible = {
            TileID.CorruptThorns,         TileID.Cobweb,           TileID.JungleThorns,   TileID.InactiveStoneBlock, TileID.MetalBars,         TileID.CopperCoinPile,     TileID.SilverCoinPile,
            TileID.GoldCoinPile,          TileID.PlatinumCoinPile, TileID.LivingFire,     TileID.LivingCursedFire,   TileID.LivingDemonFire,   TileID.LivingFrostFire,    TileID.LivingIchor,
            TileID.LivingUltrabrightFire, TileID.ChimneySmoke,     TileID.CrimtaneThorns, TileID.PixelBox,           TileID.SillyStreamerBlue, TileID.SillyStreamerGreen, TileID.SillyStreamerPink
        };

        private static readonly int[] lineCompatible = {
            TileID.MinecartTrack, 427, 435, 436, 437, 438, 439, 445
        };

        private static readonly int[] wideLineCompatible = { TileID.PixelBox };

        private static readonly int[] superCover = {
            TileID.Chain, TileID.Rope, TileID.SilkRope, TileID.VineRope, TileID.WebRope, TileID.ConveyorBeltLeft, TileID.ConveyorBeltRight,// TileID.MinecartTrack,
            TileID.SillyStreamerBlue, TileID.SillyStreamerGreen, TileID.SillyStreamerPink
        };

        private static readonly int[] styleDown = {
            TileID.Torches,         TileID.ClosedDoor,  TileID.OpenDoor,    TileID.Chairs,     TileID.Platforms,   TileID.Pots,           TileID.Candles,               TileID.Chandeliers, TileID.Jackolanterns,
            TileID.HangingLanterns, TileID.Beds,        TileID.Bathtubs,    TileID.Lamps,      TileID.Candelabras, TileID.PressurePlates, TileID.Traps,                 TileID.MusicBoxes,  TileID.Sinks,
            TileID.Firework,        TileID.Painting6X4, TileID.Painting3X2, TileID.PlanterBox, TileID.LogicGate,   TileID.LogicSensor,    TileID.WeightedPressurePlate, TileID.Painting4X3
        };

        private static readonly List<TopButton> TopButtons = new List<TopButton>();
        private static readonly List<Point> SelectedPoints = new List<Point>();
        private static readonly HashSet<int> SelectedPointsHash = new HashSet<int>();

        private static List<ListItem> StructuresList;

        private static readonly Dictionary<string, List<string>> StructureSubList = new Dictionary<string, List<string>> {

            {StructureNames.Cavinator,               new List<string> { "1x", "2x", "3x", "4x", "5x", "10x", "20x", "30x", "40x", "50x" } },
            {StructureNames.CloudIsland,             new List<string> { "Default", "Desert", "Snow" } },
            {StructureNames.CrimsonEntrance,         new List<string> { "Enter Left", "Enter Right" } },
            {StructureNames.CrimsonStart,            new List<string> { "Auto", "Enter Left", "Enter Right" } },
            {StructureNames.Dungeon,                 new List<string> { "Random", "Blue", "Green", "Pink" } },
            {StructureNames.EnchantedSword,          new List<string> { "Random", "Fake Sword", "Real Sword" } },
            {StructureNames.GemCave,                 new List<string> { "Random", "Amethyst", "Topaz", "Sapphire", "Emerald", "Ruby", "Diamond", "All" } },
            {StructureNames.GraniteCave,             new List<string> { "Default", "With Lava", "Without Lava" } },
            //{StructureNames.Hive,                    new List<string> { "Default", "Tiny", "Small", "Medium", "Large", "Extra Large", "Huge", "Hive City", "Small W/ Queen", "Medium W/ Queen", "Large W/ Queen", "Extra Large W/ Queen", "Huge W/ Queen", "Hive City W/ Queen" } },
            {StructureNames.Hive,                    new List<string> { "Default", "Special", "Classic" } },
            {StructureNames.JungleShrine,            new List<string> { "Random", "Gold", "Iridescent", "Mudstone", "Rich Mahogany", "Tin" } },
            {StructureNames.Lakinater,               new List<string> { "Default", "Son of Lakinater" } },// "Tiny", "Small", "Medium", "Large", "Huge", "Oceanater" } },
            {StructureNames.LivingMahoganyTree,      new List<string> { "Default", "Tiny", "Small", "Medium", "Large", "Extra Large", "Huge", "World Tree", "Large W/ Room", "Extra Large W/ Room", "Huge W/ Room", "World Tree W/ Room" } },
            {StructureNames.LivingTree,              new List<string> { "Default", "Tiny", "Small", "Medium", "Large", "Extra Large", "Huge", "World Tree", "Large W/ Room", "Extra Large W/ Room", "Huge W/ Room", "World Tree W/ Room" } },
            {StructureNames.MakeHole,                new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "The Void" } },//, "Tiny Pool", "Small Pool", "Medium Pool", "Large Pool", "Huge Pool", "The Void Pool" } },
            {StructureNames.MarbleCave,              new List<string> { "Default", "With Lava", "Without Lava" } },
            {StructureNames.Meteor,                  new List<string> { "Default", "Tiny", "Small", "Large", "Extra Large", "Huge"} },
            {StructureNames.MossCave,                new List<string> { "Random", "Default", "Green", "Yellow", "Red", "Blue", "Purple" } },
            {StructureNames.Mountinater,             new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Olympus" } },
            {StructureNames.Pyramid,                 new List<string> { "Egyptian", "Mayan" } },
            {StructureNames.SpiderCave,              new List<string> { "Default", "500 Tile Limit", "2000 Tile Limit", "5000 Tile Limit" } },
            {StructureNames.UndergroundHouse,        new List<string> { "Default", "Old" } }

        };

        private static readonly List<ListItem> OthersList = new List<ListItem> {
            new ListItem(0, OtherNames.Converter),
            new ListItem(1, OtherNames.Liquid),
            new ListItem(2, OtherNames.Mech),
            new ListItem(3, OtherNames.PaintTile),
            new ListItem(4, OtherNames.PaintWall),
            new ListItem(5, OtherNames.ResetFrames),
            new ListItem(6, OtherNames.RoomCheck),
            new ListItem(7, OtherNames.Slope),
            new ListItem(8, OtherNames.Smooth),
            new ListItem(9, OtherNames.Spawn)
        };

        private static readonly List<string> PaintColors = new List<string> {
            "Red", "Orange", "Yellow", "Lime", "Green", "Teal", "Cyan", "SkyBlue", "Blue", "Purple", "Violet",
            "Pink", "DeepRed", "DeepOrange", "DeepYellow", "DeepLime", "DeepGreen", "DeepTeal", "DeepCyan", "DeepSkyBlue",
            "DeepBlue", "DeepPurple", "DeepViolet", "DeepPink", "Black", "White", "Grey", "Brown", "Shadow", "Negative"
        };

        private static readonly Dictionary<string, List<string>> OtherSubList = new Dictionary<string, List<string>> {
            {OtherNames.Mech, new List<string>   { "Actuator", "Wire - Blue", "Wire - Green", "Wire - Red", "Wire - Yellow" } },
            {OtherNames.Liquid, new List<string> { "Honey", "Lava", "Water" } },
            {OtherNames.Slope, new List<string>  { "Full", "Half Brick", "Up-Right", "Up-Left", "Down-Right", "Down-Left" } },
            {OtherNames.Spawn, new List<string>  { "Player", "World" } },
            {OtherNames.PaintTile, PaintColors},
            {OtherNames.PaintWall, PaintColors},
            {OtherNames.Converter, new List<string> { "Corruption", "Crimson", "Hallow", "Jungle", "Mushroom", "Purity" } }
        };

        private static VerticalSlider _middleVerticalSlider;
        private static VerticalSlider _bottomVerticalSlider;

        #region SearchBar Stuff

        private static FancyButton _search;
        private static FancyButton _searchBar;

        private static bool ReverseSortList = false;
        private static List<ListItem> ListFiltered;

        private static string SearchBoxText;
        private static string SearchString;

        private static string _searchAppend = "|";
        private static int _searchAppendCounter;

        #endregion SearchBar Stuff

        #region Sort Stuff

        private static readonly ListSorter<ListItem> SortByName = new ListSorter<ListItem>("Name", (i1, i2) => i1.Name.CompareTo(i2.Name), item => true);
        private static readonly ListSorter<ListItem> SortByID = new ListSorter<ListItem>("ID", (i1, i2) => i1.ID.CompareTo(i2.ID), item => true);

        private static readonly ListSorter<ListItem>[] ListSorters = new[] { SortByID, SortByName };

        #endregion

        #endregion Private Variables

        #region Public Variables

        public static bool LeftClicked;
        public static bool LeftDragging;
        public static bool LeftLongClick;
        public static bool RightClicked;
        public static bool RightDragging;
        public static bool RightLongClick;
        public static bool Searching;
        public static bool ShowDebugRegenDialog;
        public static bool ShowMapResetDialog;
        public static bool ShowMinimap;
        public static bool ShowStatusBar = true; // I should be able to remove this if I don't want to make the Status Bar optional
        public static bool SwitchedToEyedropper;
        public static bool SwitchedToGameZoom;
        public static bool SwitchedToMouseInWorldZoom;
        public static bool TileFramingTheWorld;

        public static byte DragDelay = 15;

        public static float Brightness = 0.75f;

        public static int HoveredListItemTimer;
        public static int ToolbarXoffset;
        public static int ToolbarYoffset;
        public static int WireStatus;

        public static List<int> MakeDirectional;

        public static List<string> SubOtherNames;
        public static List<string> SubStructureNames;
        public static List<string> SubTileNames;

        public static List<Tuple<Point, int>> InvalidTiles;

        public static string ButtonWithFocus;
        public static string HoveredListItem = "";

        public static StructureMap structures;

        public static Texture2D Active;
        public static Texture2D ButtonActive;
        public static Texture2D ButtonHover;
        public static Texture2D ButtonInactive;
        public static Texture2D Dark;
        public static Texture2D Highlight;
        public static Texture2D Inactive;
        public static Texture2D Light;
        public static Texture2D Lighter;
        public static Texture2D LineMarker;
        public static Texture2D SelectionDash;
        public static Texture2D Square;
        public static Texture2D UIButtonTextures;

        #endregion Public Variables

        #endregion Variables

        #region Fields

        #region Middle Box Fields

        private static int MiddleListBottom => Main.screenHeight - 240 - (ShowStatusBar ? StatusBarHeight : 0);

        private static int MiddleListLength => MiddleListBottom - MiddleListTop;

        private static int MiddleListItemsCount => (int)Math.Floor(MiddleListLength / (float)ListItemHeight - 1);

        private static int TileScrollMax => Math.Max((int)Math.Ceiling((double)TileNames.TileCount - MiddleListItemsCount), 0);

        private static int TileScroll {
            get { return _tileScroll; }
            set { _tileScroll = (int)MathHelper.Clamp(value, 0, TileScrollMax); }
        }

        private static int WallScrollMax => Math.Max((int)Math.Ceiling((double)WallID.Count - MiddleListItemsCount), 0);

        private static int WallScroll {
            get { return _wallScroll; }
            set { _wallScroll = (int)MathHelper.Clamp(value, 0, WallScrollMax); }
        }

        private static int StructureListItemsLength => Math.Min((int)Math.Floor(MiddleListLength / (float)ListItemHeight - 1), StructuresList.Count);

        private static int StructureScrollMax => Math.Max((int)Math.Ceiling((double)StructuresList.Count + 1 - MiddleListItemsCount), 0);

        private static int StructureScroll {
            get { return _structureScroll; }
            set { _structureScroll = (int)MathHelper.Clamp(value, 0, StructureScrollMax); }
        }

        private static int OtherListItemsLength => Math.Min((int)Math.Floor(MiddleListLength / (float)ListItemHeight - 1), OthersList.Count);

        private static int OtherScrollMax => Math.Max((int)Math.Ceiling((double)OthersList.Count + 1 - MiddleListItemsCount), 0);

        private static int OtherScroll {
            get { return _otherScroll; }
            set { _otherScroll = (int)MathHelper.Clamp(value, 0, OtherScrollMax); }
        }

        #endregion Middle Box Fields

        #region Bottom Box Fields

        private static int SubListBottom => Main.screenHeight - (ShowStatusBar ? StatusBarHeight : 0);

        private static int SubListLength => SubListBottom - MiddleListBottom;

        private static int SubListItemsLength {
            get {

                int count;

                if (CurrentTab == Tabs.Tiles) {
                    count = SubTileNames.Count;
                }
                else if (CurrentTab == Tabs.Structures) {
                    count = SubStructureNames.Count;
                }
                else if (CurrentTab == Tabs.Other) {
                    count = SubOtherNames.Count;
                }
                else {
                    count = 0;
                }

                return Math.Min((int)Math.Floor(SubListLength / (float)ListItemHeight - 1), count);

            }
        }

        private static int SubScrollMax {
            get {

                int count;

                if (CurrentTab == Tabs.Tiles) {
                    count = SubTileNames.Count;
                }
                else if (CurrentTab == Tabs.Structures) {
                    count = SubStructureNames.Count;
                }
                else if (CurrentTab == Tabs.Other) {
                    count = SubOtherNames.Count;
                }
                else {
                    count = 0;
                }

                return Math.Max((int)Math.Ceiling((double)count - SubListItemsLength), 0);

            }
        }

        private static int SubScroll {
            get { return _subScroll; }
            set { _subScroll = (int)MathHelper.Clamp(value, 0, SubScrollMax); }
        }

        #endregion Bottom Box Fields

        #region Brush Size Fields

        private static int BrushSizeMax => MaxBrushSize;

        private static int BrushSize {

            get {

                switch (CurrentTab) {

                    case Tabs.Tiles:

                        int type = NeoDraw.TileToCreate.GetValueOrDefault();

                        if (NeoDraw.TileToCreate.HasValue && ((!Main.tileFrameImportant[type] && Main.tileSolid[type]) || bigBrushCompatible.Contains(type)))
                            return _brushSize;

                        if (CurrentBrushShape == BrushShape.Line && NeoDraw.TileToCreate.HasValue && wideLineCompatible.Contains(type))
                            return _brushSize;

                        if (CurrentPaintMode == PaintMode.Erase)
                            return _brushSize;

                        return 1;

                    case Tabs.Walls:

                        return _brushSize;

                    case Tabs.Other:

                        switch (OtherToCreateName) {
                            
                            case OtherNames.Mech: {

                                if (CurrentBrushShape == BrushShape.Line)
                                    return 1;

                                break;
                            }
                            case OtherNames.Spawn: {
                                return 1;
                            }

                        }

                        return _brushSize;

                    default:

                        return 1;

                }

            }

            set {
                _brushSize = (int)MathHelper.Clamp(value, 1, BrushSizeMax);
            }

        }

        #endregion Brush Size Fields

        #region UI Alpha Value Fields

        private static float BoxAlpha => (MouseXoverToolbar || _scrolling || _subScrolling || Searching) ? ActiveAlpha : InactiveAlpha;

        private static float TextAlpha => (MouseXoverToolbar || _scrolling || _subScrolling) ? 1f : InactiveAlpha;

        #endregion UI Alpha Value Fields

        #region Various Fields

        public static bool AtBottomEdgeOfWorld => Main.screenPosition.Y > Main.bottomWorld - Main.screenHeight - StatusBarHeight - 650;

        public static bool AtLeftEdgeOfWorld => Main.screenPosition.X < 900;

        public static bool AtRightEdgeOfWorld => Main.screenPosition.X > Main.rightWorld - Main.screenWidth - 900;

        public static byte CurrentBrushShape { get; set; } = BrushShape.Square;

        public static byte CurrentPaintMode { get; set; } = PaintMode.Paint;

        public static string CurrentTabName {

            get {

                switch (CurrentTab) {

                    case 0: return "Tiles";
                    case 1: return "Walls";
                    case 2: return "Structures";
                    default: return "Other";

                }

            }

        }

        private static int HellLayer => Main.maxTilesY - 200;

        public static bool MouseOffScreen {
            
            get {

                bool switched = false;
                
                if (SwitchedToMouseInWorldZoom) {
                    SwitchToMouseInUIZoom();
                    switched = true;
                }

                bool offscreen = Main.MouseScreen.X < 0 || Main.MouseScreen.X > Main.screenWidth || Main.MouseScreen.Y < 0 || Main.MouseScreen.Y > Main.screenHeight;

                if (switched)
                    SwitchToMouseInWorldZoom();

                return offscreen;

            }

        }

        private static bool MouseXoverToolbar { get { return ((!AtLeftEdgeOfWorld && Main.mouseX <= ListWidth) || (AtLeftEdgeOfWorld && Main.mouseX >= Main.screenWidth - ListWidth)) && !MouseOffScreen; } }

        public static bool MouseYoverStatusbar { get { return (AtBottomEdgeOfWorld && Main.mouseY < StatusBarHeight) || (!AtBottomEdgeOfWorld && Main.mouseY > ((AtBottomEdgeOfWorld && !Main.mapFullscreen) ? 0 : Main.screenHeight - StatusBarHeight)); } }

        public static string OtherToCreateName => NeoDraw.OtherToCreate.HasValue ? OthersList[NeoDraw.OtherToCreate.GetValueOrDefault()].Name : "";

        public static string StructureToCreateName => NeoDraw.StructureToCreate.HasValue ? StructuresList[NeoDraw.StructureToCreate.GetValueOrDefault()].Name : "";

        public static bool TreatLikeStairs {

            get {

                if (NeoDraw.TileToCreate == null)
                    return false;

                int type = NeoDraw.TileToCreate.GetValueOrDefault();

                return TileID.Sets.Platforms[type] || type == TileID.Rope || type == TileID.SilkRope || type == TileID.VineRope || type == TileID.WebRope || type == TileID.Chain || type == TileID.SillyStreamerPink || type == TileID.SillyStreamerBlue || type == TileID.SillyStreamerGreen;

            }
        }

        public static ref UndoStep Undo {

            get {

                if (_undo == null)
                    _undo = new UndoStep();

                return ref _undo;

            }

        }

        #endregion

        #endregion Fields

        public override void OnInitialize() {

            MakeDirectional = new List<int>();

            SubTileNames = new List<string>();
            SubStructureNames = new List<string>();
            SubOtherNames = new List<string>();

            InvalidTiles = new List<Tuple<Point, int>>();

            structures = new StructureMap();

            Active           = GetTexture("NeoDraw/Textures/Active");
            ButtonActive     = GetTexture("NeoDraw/Textures/ButtonActive");
            ButtonInactive   = GetTexture("NeoDraw/Textures/ButtonInactive");
            ButtonHover      = GetTexture("NeoDraw/Textures/ButtonHover");
            Dark             = GetTexture("NeoDraw/Textures/Dark");
            Highlight        = GetTexture("NeoDraw/Textures/Highlight");
            Inactive         = GetTexture("NeoDraw/Textures/Inactive");
            Light            = GetTexture("NeoDraw/Textures/Light");
            Lighter          = GetTexture("NeoDraw/Textures/Lighter");
            LineMarker       = GetTexture("NeoDraw/Textures/LineMarker");
            SelectionDash    = GetTexture("NeoDraw/Textures/SelectionDash");
            Square           = GetTexture("NeoDraw/Textures/SelectionSquare");
            UIButtonTextures = GetTexture("NeoDraw/Textures/UIButtons");

            CreateSliders();
            CreateButtons();
            CreateSearchButton();
            CreateStructuresList();

            SearchBoxText = null;
            SearchString = null;

            for (int i = 0; i < CurrentSorter.Length; i++)
                CurrentSorter[i] = 0;

        }

        public override void Draw(SpriteBatch sb) {

            if (Main.playerInventory)
                return;

            CheckIfScreenSizeChanged();

            HandleClickLogic();

            HandleTimeChange();

            Main.chatRelease = false;
            FrameCounterUpdated = false;
            NeoTile.CaptureTileFrames = false;
            WorldGen.noTileActions = true;

            ToolbarXoffset = AtLeftEdgeOfWorld ? Main.screenWidth - ListWidth : 0;
            ToolbarYoffset = AtBottomEdgeOfWorld ? StatusBarHeight : 0;

            if (DungeonBuilder.MakingDungeon)
                DungeonBuilder.MultiPartDungeon();

            if (TileFramingTheWorld)
                TileFrameTheWorld();

            if (_undo != null && Main.mouseLeftRelease) {

                if (_undo.Count > 0)
                    NeoDraw.UndoManager.UndoPush(_undo);

                _undo = null;

            }

            if (SelectedPoints.Count > 0)
                UpdateSelectionFrame();

            //////////////////////////////////////////
            SwitchToGameZoom(); // SpriteBatch Change
            //////////////////////////////////////////

            DrawSelectedPoints(sb);

            if (Pasting)
                ShowPasteHighlight(sb);

            if (InvalidTiles.Count > 0)
                HighlightInvalidTiles(sb);

            if (GridStyle > 0)
                DrawLaserRuler(sb);

            if (RightDragging && !Main.keyState.PressingAlt()) {

                if (RulerStartPoint == default) {
                    SwitchToMouseInWorldZoom();
                    RulerStartPoint = Main.MouseWorld / 16;
                    SwitchToMouseInUIZoom();
                }

                ShowRuler(sb);

            }
            else {
                RulerStartPoint = default;
            }

            //////////////////////////////////////////
            SwitchToUIZoom(); // SpriteBatch Change
            //////////////////////////////////////////

            DrawWorldLineMarkers(sb);

            if (_showGPS)
                ShowGPS(sb);

            if (ShowMinimap)
                DrawMinimap(sb);

            bool toolbarHovered = DrawToolbar(sb);

            if (ShowStatusBar && !Main.mapFullscreen)
                DrawStatusBar(sb);

            if (_hoverText != null)
                MouseText(sb, _hoverText);

            if (!toolbarHovered && !Main.keyState.PressingCtrl()) { // TODO: Maybe make this not happen when QuickSwitching to Eyedropper?

                int sizeScrollBy = PlayerInput.ScrollWheelDelta / 120;
                BrushSize += sizeScrollBy;

                if (sizeScrollBy != 0) {

                    if (CurrentTab == Tabs.Structures) {

                        if (CurrentPaintMode == PaintMode.Paint) {

                            if (CurrentBrushShape == BrushShape.Square || CurrentBrushShape == BrushShape.Circle) {

                                if (NeoDraw.StructureToCreate.HasValue && NeoDraw.StructureToCreate.GetValueOrDefault() < StructuresList.Count) {

                                    switch (StructureToCreateName) {
                                        
                                        //case StructureNames.UndergroundHouse: WldGen.WldGen.GetMineHouseSize(); break;
                                        //case StructureNames.JungleShrine: WldGen.WldGen.GetJungleShrineSize(); break;
                                        case StructureNames.CloudIslandHouse: GetIslandHouseSize(); break;

                                    }

                                }

                            }

                        }

                    }

                }

            }

            if (HoveredListItem != "") {

                MouseText(sb, HoveredListItem);
                HoveredListItem = "";

            }

            if (ShowMapResetDialog) {

                string message = "Reset World Map?\nThis will hide all explored places on the map.\nThis action cannot be undone.";

                if (DrawMapResetDialog(sb, message)) {

                    NeoDraw.oldWorldMap.Clear();
                    Main.Map.Clear();

                    Main.clearMap = true;
                    Main.refreshMap = true;
                    Main.updateMap = true;

                }

                return;
            }
            else if (ShowDebugRegenDialog) {

                string message = "Regenerate current world?\nThis will reset all tiles, walls, etc. to new.\nThis action cannot be undone.";

                if (DrawMapResetDialog(sb, message)) {

                    //TileFramingTheWorld = true;

                    Terraria.World.Generation.WorldUtils.DebugRegen(); // TODO: Prevent this from resetting world progress

                    for (int i = 0; i < Main.maxTilesX; i++) // TODO: Make this show a progress message in the status bar while running
                        for (int j = 0; j < Main.maxTilesY; j++)
                            WorldGen.TileFrame(i, j);

                }

                return;
            }
            else {
                NeoDraw.ForceCursorToShow = false;
            }

            if (Main.LocalPlayer.mouseInterface || MouseXoverToolbar || MouseYoverStatusbar || Searching) // TODO: Should I check for SwitchedToEyedropper here?
                return;

            SwitchedToEyedropper = false;

            if (CurrentPaintMode == PaintMode.Paint && NeoDraw.Eyedropper.Current && StartPoint == default) { // Check for Quick Swap to Eyedropper

                if (!NeoPlayer.Moving) {

                    CurrentPaintMode = PaintMode.Eyedropper;
                    SwitchedToEyedropper = true;

                }
            }

            bool drawOutline = true;

            switch (CurrentTab) {

                case Tabs.Tiles:

                    if (NeoDraw.TileToCreate == null)
                        drawOutline = false;

                    break;

                case Tabs.Walls:

                    if (NeoDraw.WallToCreate == null)
                        drawOutline = false;

                    break;

                case Tabs.Structures:

                    if (NeoDraw.StructureToCreate == null)
                        drawOutline = false;

                    break;

                case Tabs.Other:

                    if (NeoDraw.OtherToCreate == null)
                        drawOutline = false;

                    break;

            }

            //////////////////////////////////////////
            SwitchToGameZoom(); // SpriteBatch Change
            //////////////////////////////////////////

            if (drawOutline || CurrentPaintMode != PaintMode.Paint || Main.keyState.PressingAlt())
                DrawBrushOutline(sb);

            //////////////////////////////////////////
            SwitchToUIZoom(); // SpriteBatch Change
            //////////////////////////////////////////

            if (DateTime.Now < _clickDelay) // TODO: Should I check for SwitchedToEyedropper here?
                return;

            if (Main.mouseLeft) {
                NeoTile.CaptureTileChanges = true;
                LeftClick();
                AddClickDelay(5);
                NeoTile.CaptureTileChanges = false;
            }
            else if (Main.mouseRight) {
                NeoTile.CaptureTileChanges = true;
                RightClick();
                AddClickDelay(5);
                NeoTile.CaptureTileChanges = false;
            }

            if (SwitchedToEyedropper)
                CurrentPaintMode = PaintMode.Paint;

        }

        #region UI Components

        public static void DrawBox(SpriteBatch sb, int xPos, int yPos, int width, int height, float alpha) {

            sb.Draw(Dark, new Rectangle(xPos, yPos, width, 2), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos, yPos + height - 2, width, 2), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos, yPos, 2, height), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos + width - 2, yPos, 2, height), Color.White * alpha);

            sb.Draw(Lighter, new Rectangle(xPos + 2, yPos + 2, width - 4, 2), Color.White * alpha);

            sb.Draw(Light, new Rectangle(xPos + 2, yPos + 4, width - 4, height - 6), Color.White * alpha);

        }

        private static void DrawTab(SpriteBatch sb, int xPos, int yPos, int width, int height, float alpha, int state = 0) {

            sb.Draw(Dark, new Rectangle(xPos + 2, yPos, width - 4, 2), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos + 2, yPos + height - 2, width - 4, 2), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos, yPos + 2, 2, height - 4), Color.White * alpha);
            sb.Draw(Dark, new Rectangle(xPos + width - 2, yPos + 2, 2, height - 4), Color.White * alpha);

            Texture2D texture = Inactive;

            if (state == 1) {
                texture = Active;
            }
            else if (state == 2) {
                texture = Highlight;
            }

            sb.Draw(texture, new Rectangle(xPos + 2, yPos + 2, width - 4, height - 4), Color.White * alpha);

        }

        #endregion UI Components

        #region Draw UI

        private static void CheckIfScreenSizeChanged() {

            if (_oldScreenWidth == -1) {

                _oldScreenWidth = Main.screenWidth;
                _oldScreenHeight = Main.screenHeight;

            }

            if (Main.screenWidth != _oldScreenWidth || Main.screenHeight != _oldScreenHeight) {

                UpdatePositions();

                _oldScreenWidth = Main.screenWidth;
                _oldScreenHeight = Main.screenHeight;

            }

        }

        private static void DrawWorldLineMarkers(SpriteBatch sb) {

            string label;
            Vector2 stringSize;

            if (Main.screenPosition.Y < (Main.worldSurface * 0.35) * 16 && Main.screenPosition.Y + Main.screenHeight > (Main.worldSurface * 0.35) * 16) { // Sky Layer

                sb.Draw(LineMarker, new Vector2(Main.screenWidth - LineMarker.Width, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - LineMarker.Height / 2f), Color.White * 0.5f);

                label = "Sky";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y - 30));

                label = "Surface";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)(Main.worldSurface * 0.35) * 16 - Main.screenPosition.Y + 12));

            }
            else if (Main.screenPosition.Y < Main.worldSurface * 16 && Main.screenPosition.Y + Main.screenHeight > Main.worldSurface * 16) { // World Surface Layer

                sb.Draw(LineMarker, new Vector2(Main.screenWidth - LineMarker.Width, (float)Main.worldSurface * 16 - Main.screenPosition.Y - LineMarker.Height / 2f), Color.White * 0.5f);

                label = "Surface";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)Main.worldSurface * 16 - Main.screenPosition.Y - 30));

                label = "Underground";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.worldSurface * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)Main.worldSurface * 16 - Main.screenPosition.Y + 12));

            }
            else if (Main.screenPosition.Y < Main.rockLayer * 16 && Main.screenPosition.Y + Main.screenHeight > Main.rockLayer * 16) { // Rock Layer

                sb.Draw(LineMarker, new Vector2(Main.screenWidth - LineMarker.Width, (float)Main.rockLayer * 16 - Main.screenPosition.Y - LineMarker.Height / 2f), Color.White * 0.5f);

                label = "Underground";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)Main.rockLayer * 16 - Main.screenPosition.Y - 30));

                label = "Cavern";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)Main.rockLayer * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)Main.rockLayer * 16 - Main.screenPosition.Y + 12));

            }
            else if (Main.screenPosition.Y < HellLayer * 16 && Main.screenPosition.Y + Main.screenHeight > HellLayer * 16) { // Hell Layer

                sb.Draw(LineMarker, new Vector2(Main.screenWidth - LineMarker.Width, (float)HellLayer * 16 - Main.screenPosition.Y - LineMarker.Height / 2f), Color.White * 0.5f);

                label = "Cavern";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)HellLayer * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)HellLayer * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)HellLayer * 16 - Main.screenPosition.Y - 30 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)HellLayer * 16 - Main.screenPosition.Y - 30 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)HellLayer * 16 - Main.screenPosition.Y - 30));

                label = "Underworld";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)HellLayer * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X - 2, (float)HellLayer * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)HellLayer * 16 - Main.screenPosition.Y + 12 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X + 2, (float)HellLayer * 16 - Main.screenPosition.Y + 12 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(Main.screenWidth - 20 - stringSize.X, (float)HellLayer * 16 - Main.screenPosition.Y + 12));

            }

            if (Main.screenPosition.X < 250 * 16 && Main.screenPosition.X + Main.screenWidth > 250 * 16) { // Left Ocean

                sb.DrawLine(250 * 16 - Main.screenPosition.X, Main.screenHeight - 50, 250 * 16 - Main.screenPosition.X, Main.screenHeight, Color.Yellow * 0.5f, 10);

                label = "Ocean";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(250 * 16 - Main.screenPosition.X - stringSize.X - 22 - 2, Main.screenHeight - 20 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(250 * 16 - Main.screenPosition.X - stringSize.X - 22 - 2, Main.screenHeight - 20 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(250 * 16 - Main.screenPosition.X - stringSize.X - 22 + 2, Main.screenHeight - 20 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(250 * 16 - Main.screenPosition.X - stringSize.X - 22 + 2, Main.screenHeight - 20 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2(250 * 16 - Main.screenPosition.X - stringSize.X - 22, Main.screenHeight - 20));

            }
            else if (Main.screenPosition.X < (Main.maxTilesX - 250) * 16 && Main.screenPosition.X + Main.screenWidth > (Main.maxTilesX - 250) * 16) { // Right Ocean

                sb.DrawLine((Main.maxTilesX - 250) * 16 - Main.screenPosition.X, Main.screenHeight - 50, (Main.maxTilesX - 250) * 16 - Main.screenPosition.X, Main.screenHeight, Color.Yellow * 0.5f, 10);

                label = "Ocean";
                stringSize = Main.fontMouseText.MeasureString(label);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2((Main.maxTilesX - 250) * 16 - Main.screenPosition.X + 22 - 2, Main.screenHeight - 20 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2((Main.maxTilesX - 250) * 16 - Main.screenPosition.X + 22 - 2, Main.screenHeight - 20 + 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2((Main.maxTilesX - 250) * 16 - Main.screenPosition.X + 22 + 2, Main.screenHeight - 20 - 2), Color.Black);
                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2((Main.maxTilesX - 250) * 16 - Main.screenPosition.X + 22 + 2, Main.screenHeight - 20 + 2), Color.Black);

                Drawing.StringShadowed(sb, Main.fontMouseText, label, new Vector2((Main.maxTilesX - 250) * 16 - Main.screenPosition.X + 22, Main.screenHeight - 20));

            }

            SwitchToGameZoom();

            float spawnX;
            float spawnY;
            Texture2D spawnTexture;

            if (Main.LocalPlayer.SpawnX >= 0 && Main.LocalPlayer.SpawnY >= 0) {

                spawnX = Main.LocalPlayer.SpawnX * 16 + 8;
                spawnY = Main.LocalPlayer.SpawnY * 16;
                spawnTexture = Main.npcToggleTexture[0];

                if (Main.screenPosition.X < spawnX && Main.screenPosition.X + Main.screenWidth > spawnX &&
                    Main.screenPosition.Y < spawnY && Main.screenPosition.Y + Main.screenHeight > spawnY) {

                    sb.Draw(spawnTexture, new Vector2(spawnX - Main.screenPosition.X - spawnTexture.Width / 2f, spawnY - Main.screenPosition.Y - spawnTexture.Height), Color.White);

                }

            }

            spawnX = Main.spawnTileX * 16 + 8;
            spawnY = Main.spawnTileY * 16;
            spawnTexture = Main.npcToggleTexture[1];

            if (Main.screenPosition.X < spawnX && Main.screenPosition.X + Main.screenWidth > spawnX && Main.screenPosition.Y < spawnY && Main.screenPosition.Y + Main.screenHeight > spawnY)
                sb.Draw(spawnTexture, new Vector2(spawnX - Main.screenPosition.X - spawnTexture.Width / 2f, spawnY - Main.screenPosition.Y - spawnTexture.Height), Color.White);

            SwitchToUIZoom();

        }

        private static void HoverLeftTopBox() {

            Main.LocalPlayer.mouseInterface = true;

        }

        private static void ShowGPS(SpriteBatch sb) {

            string gpsText;

            int num = (int)((Main.screenPosition.X + (Main.screenWidth / 2f)) * 2f / 16f - Main.maxTilesX);

            if (num > 0) {
                gpsText = "Position: " + num + " feet east";
                if (num == 1)
                    gpsText = "Position: " + num + " foot east";
            }
            else if (num < 0) {
                num *= -1;
                gpsText = "Position: " + num + " feet west";
                if (num == 1)
                    gpsText = "Position: " + num + " foot west";
            }
            else
                gpsText = "Position: Center";

            int num2 = (int)(((Main.screenPosition.Y + (Main.screenHeight / 2f)) * 2f / 16f) - Main.worldSurface * 2.0);

            gpsText += "\nDepth: " + (num2 == 0 ? "Level" : (Math.Abs(num2) + (Math.Abs(num2) == 1 ? " foot " : " feet ") + (num2 > 0 ? "below" : "above")));

            string text2 = "AM";
            double num3 = Main.time;

            if (!Main.dayTime)
                num3 += 54000.0;

            num3 = num3 / 86400.0 * 24.0;
            num3 = num3 - 7.5 - 12.0;

            if (num3 < 0.0)
                num3 += 24.0;
            if (num3 >= 12.0)
                text2 = "PM";

            int num5 = (int)num3;
            double num6 = num3 - num5;
            num6 = ((int)(num6 * 60.0));
            string text3 = string.Concat(num6);

            if (num6 < 10.0)
                text3 = "0" + text3;
            if (num5 > 12)
                num5 -= 12;
            if (num5 == 0)
                num5 = 12;
            if (Main.LocalPlayer.accWatch == 1)
                text3 = "00";
            else if (Main.LocalPlayer.accWatch == 2) {
                text3 = num6 < 30.0 ? "00" : "30";
            }

            gpsText += string.Concat("\n", Lang.inter[34], ": ", num5, ":", text3, " ", text2);

            List<string> list = new List<string>();

            bool overworld = true;

            int centerScreenX = (int)((Main.screenPosition.X + (Main.screenWidth / 2)) / 16f);
            int centerScreenY = (int)((Main.screenPosition.Y + (Main.screenHeight / 2)) / 16f);

            if (((Main.screenPosition.Y + (Main.screenHeight / 2)) / 16f) < Main.worldSurface + 10.0 && (centerScreenX < 380 || centerScreenX > Main.maxTilesX - 380)) {
                list.Add("Ocean");
                overworld = false;
            }

            if (Main.shroomTiles > 100) {
                list.Add("Glowshroom");
                overworld = false;
            }

            if (Main.sandTiles > 1000) {

                string desert = "";

                if (Main.evilTiles >= 200) {
                    desert = "Corrupt ";
                }
                else if (Main.bloodTiles >= 200) {
                    desert = "Crimson ";
                }
                else if (Main.holyTiles >= 100) {
                    desert = "Hallowed ";
                }

                if (centerScreenY > (3200f / 16f)) {
                    desert += "Underground Desert";
                }
                else {
                    desert += "Desert";
                }

                if (centerScreenY <= Main.worldSurface && Sandstorm.Happening)
                    desert += " - Sandstorm";

                list.Add(desert);

            }

            if (Main.holyTiles >= 100) {
                list.Add("Hallow");
                overworld = false;
            }

            if (Main.evilTiles >= 200) {
                list.Add("Corruption");
                overworld = false;
            }

            if (Main.bloodTiles >= 200) {
                list.Add("Crimson");
                overworld = false;
            }

            if (Main.jungleTiles >= 80) {
                list.Add("Jungle");
                overworld = false;
            }

            if (Main.snowTiles >= 300) {

                string snow = "";

                if (Main.evilTiles >= 200) {
                    snow = "Corrupt ";
                }
                else if (Main.bloodTiles >= 200) {
                    snow = "Crimson ";
                }
                else if (Main.holyTiles >= 100) {
                    snow = "Hallowed ";
                }

                if (centerScreenY > (3200f / 16f)) {
                    snow += "Underground Snow";
                }
                else {
                    snow += "Snow";
                }

                list.Add(snow);

                overworld = false;

            }

            if (Main.meteorTiles >= 50) {
                list.Add("Meteor");
                overworld = false;
            }

            if (Main.dungeonTiles >= 250 && (double)Main.screenPosition.Y + Main.screenHeight / 2f > Main.worldSurface * 16.0) {

                int num7 = (int)(Main.screenPosition.X + Main.screenWidth / 2f) / 16;
                int num8 = (int)(Main.screenPosition.Y + Main.screenHeight / 2f) / 16;

                if (Main.wallDungeon[Main.tile[num7, num8].wall]) {
                    list.Add("Dungeon");
                    overworld = false;
                }

            }

            if (Main.waterCandles > 0)
                list.Add("Water Candle");

            if (Main.peaceCandles > 0)
                list.Add("Peace Candle");

            if (Main.campfire)
                list.Add("Campfire");

            if (Main.heartLantern)
                list.Add("Heart Lantern");

            if (Main.musicBox > -1)
                list.Add("Music Box");

            if (centerScreenY <= Main.worldSurface * 0.35) {

                if (Main.raining) {
                    list.Add("Sky - Raining");
                }
                else {
                    list.Add("Sky");
                }

                overworld = false;

            }
            else if (centerScreenY >= Main.worldSurface * 0.35 && centerScreenY < Main.worldSurface) {

                if (Main.raining) {
                    list.Add("Surface - Raining");
                }
                else {
                    list.Add("Surface");
                }

            }
            else if (centerScreenY >= Main.worldSurface && centerScreenY < Main.rockLayer) {
                list.Add("Underground");
                overworld = false;
            }
            else if (centerScreenY >= Main.rockLayer && centerScreenY < (float)((Main.maxTilesY - 204) * 16)) {
                list.Add("Caverns");
                overworld = false;
            }
            else if (centerScreenY >= (float)((Main.maxTilesY - 204) * 16)) {
                list.Add("Hell");
                overworld = false;
            }

            if (overworld)
                list.Insert(list.Count - 1, "Overworld");

            gpsText += "\nCurrent Zone";

            if (list.Count > 1)
                gpsText += "s";

            gpsText += ": ";

            for (int j = 0; j < list.Count; j++) {

                if (j == 0) {
                    gpsText += list[j];
                }
                else {
                    gpsText += (", " + list[j]);
                }

            }

            DrawBox(sb, ListWidth, 0, (int)Main.fontMouseText.MeasureString(gpsText).X + 22, (int)Main.fontMouseText.MeasureString(gpsText).Y + 16, 0.5f);
            Drawing.StringShadowed(sb, Main.fontMouseText, gpsText, new Vector2(ListWidth + 11, ToolbarYoffset + 11));

        }

        private static void ShowHoveredTileInfo(SpriteBatch sb) {

            if (NeoDraw.TileToCreate.HasValue)
                return;

            if (!Main.keyState.PressingAlt())
                return;

            int lineNumber = 1;
            int lineHeight = 22;
            List<string> lines = new List<string>();

            string worldPos = $"Screen Pos: ({Main.mouseX}, {Main.mouseY})";
            string screenPos = $"Screen Pos: ({Main.mouseX + Main.screenPosition.X}, {Main.mouseY + Main.screenPosition.Y})";
            string tilePos = $"Tile Pos: ({Neo.TileTarget_Vector.X}, {Neo.TileTarget_Vector.Y})";

            lines.Add(tilePos);
            lines.Add(screenPos);
            lines.Add(worldPos);

            Tile tile = Neo.TileTarget_Tile;

            if (tile == null)
                return;

            int tileType = tile.type;

            if (tileType >= TileNames.OriginalTileCount)
                return;

            lines.Add($"Type: {tileType}, Name: {TileID.Search.GetName(tileType)}");
            lines.Add($"Active: {tile.active()}");

            string tileFrameImportant = "Frame Important: " + (Main.tileFrameImportant[tileType] ? "True" : "False");

            lines.Add(tileFrameImportant);
            lines.Add($"FrameX: {tile.frameX}, FrameY: {tile.frameY}");
            lines.Add($"IsTopLeft: {Neo.IsTopLeft(Neo.TileTarget_Vector.X, Neo.TileTarget_Vector.Y)}");

            TOD tileData = TOD.GetTileData(tileType, 0);

            if (Main.tileFrameImportant[tileType] && tileData != null) {

                int fullWidth = tileData.CoordinateFullWidth;
                int fullHeight = tileData.CoordinateFullHeight;

                bool isTop = tile.frameY % fullHeight == 0;
                bool isLeft = tile.frameX % fullWidth == 0;

                lines.Add($"Top: {isTop}, Left: {isLeft}");

                lines.Add("Size: [" + tileData.Width + ", " + tileData.Height + "]");
                lines.Add("Origin: " + tileData.Origin);
                lines.Add("Y Offset: " + tileData.DrawYOffset);
                lines.Add("Horizontal: " + tileData.StyleHorizontal);
                lines.Add("Wrap Limit: " + tileData.StyleWrapLimit);
                lines.Add("Style: " + tileData.Style);
                lines.Add("C. Width: " + tileData.CoordinateWidth);

                string heightList = "";

                foreach (int height in tileData.CoordinateHeights)
                    heightList += height + ", ";

                lines.Add("C. Height: " + heightList);
                lines.Add("C. Full Width: " + tileData.CoordinateFullWidth);
                lines.Add("C. Full Height: " + tileData.CoordinateFullHeight);
                lines.Add("Direction: " + tileData.Direction);
                lines.Add("Flip H: " + tileData.DrawFlipHorizontal);
                lines.Add("Flip V: " + tileData.DrawFlipVertical);
                lines.Add("Step Down: " + tileData.DrawStepDown);
                lines.Add("Line Skip: " + tileData.StyleLineSkip);
                lines.Add("Multiplier: " + tileData.StyleMultiplier);

            }

            lines.Add($"TBLR: ({tile.topSlope()}{tile.bottomSlope()}{tile.leftSlope()}{tile.rightSlope()}");

            lines.Add("Liquid: " + tile.liquid);
            lines.Add("Liquid Type: " + tile.liquidType());
            lines.Add("");
            lines.Add("Wall Type: " + tile.wall);
            lines.Add("Wall Color: " + tile.wallColor());

            foreach (string line in lines)
                Drawing.StringShadowed(sb, Main.fontMouseText, line, new Vector2(Main.screenWidth - 200, Main.screenHeight - 800 + lineNumber++ * lineHeight));

        }

        private static void ShowSelectedTileInfo(SpriteBatch sb) {

            if (NeoDraw.TileToCreate == null)
                return;

            if (!Main.keyState.PressingAlt())
                return;

            SwitchToUIZoom();

            sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);
            //sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11, Main.mouseY - 11), Color.White);

            SwitchToGameZoom();

            int lineNumber = 1;
            int lineHeight = 22;
            List<string> lines = new List<string>();

            string worldPos = $"Screen Pos: ({Main.mouseX}, {Main.mouseY})";
            string screenPos = $"Screen Pos: ({Main.mouseX + Main.screenPosition.X}, {Main.mouseY + Main.screenPosition.Y})";
            string tilePos = $"Tile Pos: ({Neo.TileTarget_Vector.X}, {Neo.TileTarget_Vector.Y})";

            lines.Add(tilePos);
            lines.Add(screenPos);
            lines.Add(worldPos);

            int tile = (int)NeoDraw.TileToCreate;

            string tileFrameImportant = "Frame Important: " + (Main.tileFrameImportant[tile] ? "True" : "False");

            lines.Add(tileFrameImportant);

            TOD tileData = TOD.GetTileData(tile, _placeStyle);

            if (Main.tileFrameImportant[tile] && tileData != null) {

                lines.Add("Size: [" + tileData.Width + ", " + tileData.Height + "]");
                lines.Add("Origin: " + tileData.Origin);
                lines.Add("Y Offset: " + tileData.DrawYOffset);
                lines.Add("Horizontal: " + tileData.StyleHorizontal);
                lines.Add("Wrap Limit: " + tileData.StyleWrapLimit);
                lines.Add("Style: " + tileData.Style);
                lines.Add("C. Width: " + tileData.CoordinateWidth);

                string heightList = "";

                foreach (int height in tileData.CoordinateHeights)
                    heightList += height + ", ";

                lines.Add("C. Height: " + heightList);
                lines.Add("C. Full Width: " + tileData.CoordinateFullWidth);
                lines.Add("C. Full Height: " + tileData.CoordinateFullHeight);
                lines.Add("Direction: " + tileData.Direction);
                lines.Add("Flip H: " + tileData.DrawFlipHorizontal);
                lines.Add("Flip V: " + tileData.DrawFlipVertical);
                lines.Add("Step Down: " + tileData.DrawStepDown);
                lines.Add("Line Skip: " + tileData.StyleLineSkip);
                lines.Add("Multiplier: " + tileData.StyleMultiplier);

            }

            foreach (string line in lines)
                Drawing.StringShadowed(sb, Main.fontMouseText, line, new Vector2(Main.screenWidth - 200, Main.screenHeight - 600 + lineNumber++ * lineHeight));

        }

        private static void DrawButtonsTop(SpriteBatch sb, bool checkHoverTop) {

            _hoverText = null;
            _buttonClicked = false;

            for (int index = 0; index < TopButtons.Count - 2; index++) {

                TopButtons[index].Update(checkHoverTop);
                TopButtons[index].DrawWithOffset(sb, ToolbarXoffset, ToolbarYoffset);

            }

            for (int index = TopButtons.Count - 2; index < TopButtons.Count; index++) {

                TopButtons[index].Update(true);
                TopButtons[index].DrawWithOffset(sb, 0, ToolbarYoffset);

            }

            if (!Main.mouseLeft)
                ButtonWithFocus = null;

        }

        /// <summary>
        /// Draws and updates the search bar.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="draw"></param>
        /// <param name="update"></param>
        private static void DrawSearchBar(SpriteBatch sb, bool draw, bool update) {

            _search.DrawWithOffset(sb, draw, update, BoxAlpha, ToolbarXoffset, ToolbarYoffset);
            _searchBar.DrawWithOffset(sb, draw, update, BoxAlpha, ToolbarXoffset, ToolbarYoffset);

        }

        private static void HoverLeftMiddleBox() {

            Main.LocalPlayer.mouseInterface = true;

            int scrollBy = PlayerInput.ScrollWheelDelta / (Main.keyState.PressingAlt() ? 12 : 120);

            if (CurrentTab == Tabs.Tiles) {
                TileScroll -= scrollBy;
            }
            else if (CurrentTab == Tabs.Walls) {
                WallScroll -= scrollBy;
            }
            else if (CurrentTab == Tabs.Structures) {
                StructureScroll -= scrollBy;
            }
            else if (CurrentTab == Tabs.Other) {
                OtherScroll -= scrollBy;
            }

            if (Keys.PageDown.Pressed() && DateTime.Now > KeyPressDelay) {

                int amount = 10;

                if (Main.keyState.PressingAlt())
                    amount += 15;

                if (CurrentTab == Tabs.Tiles) {
                    TileScroll += amount;
                }
                else if (CurrentTab == Tabs.Walls) {
                    WallScroll += amount;
                }
                else if (CurrentTab == Tabs.Structures) {
                    StructureScroll += amount;
                }
                else if (CurrentTab == Tabs.Other) {
                    OtherScroll += amount;
                }

                KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelay);

            }
            else if (Keys.PageUp.Pressed() && DateTime.Now > KeyPressDelay) {

                int amount = 10;

                if (Main.keyState.PressingAlt())
                    amount += 15;

                if (CurrentTab == Tabs.Tiles) {
                    TileScroll -= amount;
                }
                else if (CurrentTab == Tabs.Walls) {
                    WallScroll -= amount;
                }
                else if (CurrentTab == Tabs.Structures) {
                    StructureScroll -= amount;
                }
                else if (CurrentTab == Tabs.Other) {
                    OtherScroll -= amount;
                }

                KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelay);

            }

        }

        private static void DrawListMiddle(SpriteBatch sb, bool checkHoverMiddle) {

            int pos = SearchTop + BoxPadding + SearchBarHeight;

            int min;
            int max;
            int? selectedListItem;
            List<ListItem> listSource;

            switch (CurrentTab) {

                case Tabs.Tiles:
                    min = TileScroll;
                    max = TileScroll + MiddleListItemsCount;
                    selectedListItem = NeoDraw.TileToCreate;
                    listSource = NeoDraw.TilesList;
                    break;

                case Tabs.Walls:
                    min = WallScroll;
                    max = WallScroll + MiddleListItemsCount;
                    selectedListItem = NeoDraw.WallToCreate;
                    listSource = NeoDraw.WallsList;
                    break;

                case Tabs.Structures:
                    min = StructureScroll;
                    max = StructureScroll + StructureListItemsLength;
                    selectedListItem = NeoDraw.StructureToCreate;
                    listSource = StructuresList;
                    break;

                case Tabs.Other:
                    min = OtherScroll;
                    max = OtherScroll + OtherListItemsLength;
                    selectedListItem = NeoDraw.OtherToCreate;
                    listSource = OthersList;
                    break;

                default:
                    return;

            }

            if (ListFiltered != null)
                listSource = ListFiltered;

            if (listSource == null)
                return;

            for (int index = min; index < max && index < listSource.Count; index++) {

                int boxStyle = 0;

                if (selectedListItem == listSource[index].ID)
                    boxStyle = 1;

                string listItem = listSource[index].Name;

                if (checkHoverMiddle && new Rectangle(0 + ToolbarXoffset, pos - BoxPadding / 2 + ToolbarYoffset, ListWidth - 22, ListItemHeight).Contains(Main.MouseScreen.ToPoint())) {

                    boxStyle = 2;

                    // Use this if needed to display info about the hovered list item.
                    //if (listItem.Length > 25)
                    //    HoveredListItem = listSource[index].Name;

                    if (Main.mouseLeft && Main.mouseLeftRelease) {

                        switch (CurrentTab) {

                            case Tabs.Tiles:
                                NeoDraw.TileToCreate = listSource[index].ID;
                                _placeStyle = 0;
                                SubScroll = 0;
                                StartPoint = default;
                                break;

                            case Tabs.Walls:
                                NeoDraw.WallToCreate = listSource[index].ID;
                                SubScroll = 0;
                                StartPoint = default;
                                break;

                            case Tabs.Structures:
                                NeoDraw.StructureToCreate = listSource[index].ID;
                                _specialStyle = 0;
                                SubScroll = 0;
                                StartPoint = default;

                                if (NeoDraw.StructureToCreate < StructuresList.Count) {

                                    switch (StructuresList[(int)NeoDraw.StructureToCreate].Name) {

                                        //case StructureNames.UndergroundHouse: WorldGen.GetMineHouseSize(); break;
                                        //case StructureNames.JungleShrine: WorldGen.GetJungleShrineSize(); break;
                                        case StructureNames.CloudIslandHouse: GetIslandHouseSize(); break;

                                    }

                                }

                                break;

                            case Tabs.Other:

                                NeoDraw.OtherToCreate = listSource[index].ID;
                                _otherStyle = 0;
                                SubScroll = 0;
                                StartPoint = default;

                                if (NeoDraw.OtherToCreate < OthersList.Count) {

                                    switch (OtherToCreateName) {

                                        case OtherNames.Converter:
                                        case OtherNames.ResetFrames: {

                                                if (CurrentPaintMode != PaintMode.Paint)
                                                    CurrentPaintMode = PaintMode.Paint;

                                                break;

                                            }
                                        case OtherNames.RoomCheck:
                                        case OtherNames.Spawn: {

                                                if (CurrentBrushShape != BrushShape.Circle && CurrentBrushShape != BrushShape.Square)
                                                    CurrentBrushShape = BrushShape.Square;

                                                if (CurrentPaintMode != PaintMode.Paint)
                                                    CurrentPaintMode = PaintMode.Paint;

                                                break;

                                            }

                                    }

                                }

                                break;

                        }

                    }

                }

                if (boxStyle != 0)
                    DrawTab(sb, 2 + ToolbarXoffset, pos - HoverBoxPadding + ToolbarYoffset, ListWidth - 20, ListItemHeight + HoverBoxPadding * 2, BoxAlpha, boxStyle);

                Drawing.StringShadowed(sb, Main.fontMouseText, $"{listSource[index].ID:D3}: ", new Vector2(BoxPadding + ToolbarXoffset, pos + ToolbarYoffset), Color.LightGray * TextAlpha);

                if (listItem.Length > 25)
                    listItem = listItem.Substring(0, 24) + "...";

                Drawing.StringShadowed(sb, Main.fontMouseText, listItem, new Vector2(56 + ToolbarXoffset, pos + ToolbarYoffset), Color.White * TextAlpha);
                pos += ListItemHeight;

            }

        }

        private static void HoverTabs() {

            Main.LocalPlayer.mouseInterface = true;

        }

        private static void DrawTabs(SpriteBatch sb, bool checkHoverTabs) {

            Vector2 position = new Vector2(0 + ToolbarXoffset, SearchTop - TabHeight + ToolbarYoffset);
            string[] tabNames = new[] { "Tiles", "Walls", "Structures", "Other" };

            DrawBox(sb, (int)position.X, (int)position.Y, ListWidth, TabHeight, BoxAlpha);

            for (int i = 0; i < tabNames.Length; i++) {

                int state = 0;

                if (checkHoverTabs && new Rectangle((int)position.X, (int)position.Y, ListWidth / tabNames.Length, TabHeight).Contains(Main.MouseScreen.ToPoint())) {

                    state = 2;

                    if (Main.mouseLeft && Main.mouseLeftRelease) {

                        if (CurrentTab == i) {

                            CurrentSorter[i]++;

                            if (CurrentSorter[i] > 1)
                                CurrentSorter[i] = 0;

                            switch (i) {

                                case Tabs.Tiles: NeoDraw.TilesList.Sort(ListSorters[CurrentSorter[i]]); break;
                                case Tabs.Walls: NeoDraw.WallsList.Sort(ListSorters[CurrentSorter[i]]); break;
                                case Tabs.Structures: StructuresList.Sort(ListSorters[CurrentSorter[i]]); break;
                                case Tabs.Other: OthersList.Sort(ListSorters[CurrentSorter[i]]); break;

                            }

                            Refresh(true);

                        }
                        else {

                            CurrentTab = (byte)i;
                            NeoDraw.CurrentTab = i;
                            SubScroll = 0;

                            if (CurrentTab == Tabs.Structures)
                                if (CurrentPaintMode == PaintMode.Erase)
                                    CurrentPaintMode = PaintMode.Paint;

                            Refresh(false);

                        }

                    }

                }
                else if (CurrentTab == i) {

                    state = 1;

                }

                DrawTab(sb, (int)position.X, (int)position.Y, ListWidth / tabNames.Length, TabHeight, BoxAlpha, state);

                Vector2 stringSize = Main.fontMouseText.MeasureString(tabNames[i]);
                Drawing.StringShadowed(sb, Main.fontMouseText, tabNames[i], new Vector2(position.X + (ListWidth / (float)tabNames.Length / 2f), position.Y + TabHeight / 2f + 4), Color.White * BoxAlpha, 0.85f, new Vector2(stringSize.X / 2f, stringSize.Y / 2f));

                position.X += ListWidth / tabNames.Length;

            }

        }

        private static void DrawMiddleVerticalSlider(SpriteBatch sb) {

            bool flag = false;

            _middleVerticalSlider.Update();

            if (_middleVerticalSlider.canMouseOver && !_middleVerticalSlider.disabled)
                flag = _middleVerticalSlider.MouseOver(new Vector2(Main.mouseX, Main.mouseY));

            if (_scrolling)
                flag = true;

            _middleVerticalSlider.Draw(sb, flag);

            _scrolling = false;

            if (flag && Main.mouseLeft) {

                if (_middleVerticalSlider.buttonLock)
                    _scrolling = true;

                if (Main.mouseLeftRelease) {

                    _middleVerticalSlider.framesHeld = 0;
                    _middleVerticalSlider.Click();

                }
                else {

                    _middleVerticalSlider.framesHeld++;
                    _middleVerticalSlider.ClickHold();

                }

            }
            else {

                _middleVerticalSlider.framesHeld = 0;

            }

        }

        private static void HoverLeftBottomBox() {

            Main.LocalPlayer.mouseInterface = true;

            int subScrollBy = PlayerInput.ScrollWheelDelta / (Main.keyState.PressingAlt() ? 12 : 120);

            SubScroll -= subScrollBy;

            if (Keys.PageDown.Pressed() && DateTime.Now > KeyPressDelay) {

                SubScroll += 10;

                if (Main.keyState.PressingAlt())
                    SubScroll += 15;

                KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelayQuick);

            }
            else if (Keys.PageUp.Pressed() && DateTime.Now > KeyPressDelay) {

                SubScroll -= 10;

                if (Main.keyState.PressingAlt())
                    SubScroll -= 15;

                KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelayQuick);

            }

        }

        private static void DrawListBottom(SpriteBatch sb, bool checkHoverBottom) {

            if (CurrentTab == Tabs.Walls)
                return;

            int pos = MiddleListBottom + BoxPadding;

            for (int index = SubScroll; index < SubScroll + SubListItemsLength; index++) {

                int boxStyle = 0;

                int currentTabStyle;

                if (CurrentTab == Tabs.Tiles) {
                    currentTabStyle = _placeStyle;
                }
                else if (CurrentTab == Tabs.Structures) {
                    currentTabStyle = _specialStyle;
                }
                else if (CurrentTab == Tabs.Other) {
                    currentTabStyle = _otherStyle;
                }
                else {
                    currentTabStyle = -1;
                }

                if (currentTabStyle == index)
                    boxStyle = 1;

                if (checkHoverBottom && new Rectangle(0 + ToolbarXoffset, pos - BoxPadding / 2 + ToolbarYoffset, ListWidth - 22, ListItemHeight).Contains(Main.MouseScreen.ToPoint())) {

                    boxStyle = 2;

                    if (Main.mouseLeft && Main.mouseLeftRelease) {

                        if (CurrentTab == Tabs.Tiles) {
                            _placeStyle = index;
                        }
                        else if (CurrentTab == Tabs.Structures) {
                            _specialStyle = index;
                        }
                        else if (CurrentTab == Tabs.Other) {
                            _otherStyle = index;
                        }

                    }

                }

                if (boxStyle != 0)
                    DrawTab(sb, 2 + ToolbarXoffset, pos - HoverBoxPadding + ToolbarYoffset, ListWidth - 20, ListItemHeight + HoverBoxPadding * 2, BoxAlpha, boxStyle);

                Drawing.StringShadowed(sb, Main.fontMouseText, $"{index:D3}: ", new Vector2(BoxPadding + ToolbarXoffset, pos + ToolbarYoffset), Color.LightGray * TextAlpha);

                string listItem;

                switch (CurrentTab) {

                    case Tabs.Tiles:      listItem = SubTileNames.Get(index);    break;
                    case Tabs.Structures: listItem = SubStructureNames.Get(index); break;
                    case Tabs.Other:      listItem = SubOtherNames.Get(index);   break;
                    default:              listItem = "";                         break;

                }

                if (listItem.Length > 25)
                    listItem = listItem.Substring(0, 24) + "...";

                Drawing.StringShadowed(sb, Main.fontMouseText, listItem, new Vector2(56 + ToolbarXoffset, pos + ToolbarYoffset), Color.White * TextAlpha);
                pos += ListItemHeight;

            }

        }

        private static void DrawBottomVerticalSlider(SpriteBatch sb) {

            bool flag = false;

            _bottomVerticalSlider.Update();

            if (_bottomVerticalSlider.canMouseOver && !_bottomVerticalSlider.disabled)
                flag = _bottomVerticalSlider.MouseOver(Main.MouseScreen);

            if (_subScrolling)
                flag = true;

            _bottomVerticalSlider.Draw(sb, flag);

            _subScrolling = false;

            if (flag && Main.mouseLeft) {

                if (_bottomVerticalSlider.buttonLock)
                    _subScrolling = true;

                if (Main.mouseLeftRelease) {

                    _bottomVerticalSlider.framesHeld = 0;
                    _bottomVerticalSlider.Click();

                }
                else {

                    _bottomVerticalSlider.framesHeld++;
                    _bottomVerticalSlider.ClickHold();

                }

            }
            else {

                _bottomVerticalSlider.framesHeld = 0;

            }

        }

        public static void DrawStatusBar(SpriteBatch sb) {

            int SideStatusBarWidth = 96;

            int mainBarPosX = 0;
            int sideBarPosX = Main.screenWidth - SideStatusBarWidth - 1;
            int barPosY = (AtBottomEdgeOfWorld && !Main.mapFullscreen) ? 0 : Main.screenHeight - StatusBarHeight;

            DrawBox(sb, mainBarPosX, barPosY, Main.screenWidth - SideStatusBarWidth, StatusBarHeight, 1f);

            DrawBox(sb, sideBarPosX, barPosY, SideStatusBarWidth + 1, StatusBarHeight, 1f);

            string statusBarText = "";

            if (statusBarTempMessageTimer > 0) {

                statusBarText = statusBarTempMessage;
                statusBarTempMessageTimer--;

            }
            else {

                if (Main.mapFullscreen) {
                    statusBarText += "Double Click map to jump to clicked location.";
                }
                else if (NeoPlayer.Moving) {
                    statusBarText += "Hold SHIFT to move faster. Press HOME to center screen on Player.";
                }
                else if (CurrentPaintMode == PaintMode.Select) {

                    Tuple<List<Point>, Vector4> selection = GetSelectedPoints();

                    statusBarText += "Selection Tool:";

                    if (StartPoint != default) {

                        int horizontalLength = 1 + (int)(selection.Item2.Y - selection.Item2.X);
                        int verticalLength = 1 + (int)(selection.Item2.W - selection.Item2.Z);

                        statusBarText += $" [{horizontalLength}, {verticalLength}]";
                        statusBarText += " Click to place end point. Right Click to cancel.";

                    }
                    else {

                        statusBarText += " Click to place start point.";

                    }

                    statusBarText += " Use SHIFT to add to the existing selection, or CTRL to remove.";

                }
                else if (CurrentPaintMode == PaintMode.Eyedropper) {

                    statusBarText += "Click to switch to targeted";

                    switch (CurrentTab) {

                        case Tabs.Tiles: {

                                statusBarText += " tile.";
                                break;

                            }
                        case Tabs.Walls: {

                                statusBarText += " wall.";
                                break;

                            }
                        case Tabs.Structures: {

                                statusBarText = "Eyedropper does not work while Structures Tab is selected.";
                                break;

                            }
                        case Tabs.Other: {



                                break;

                            }

                    }

                }
                else if (CurrentPaintMode == PaintMode.MagicWand) {

                    statusBarText += "Selection Tool:";

                    int selectionCount = SelectedPointsHash.Count;

                    if (selectionCount > 0)
                        statusBarText += $" ({selectionCount})";

                    statusBarText += " Use SHIFT to add to the existing selection, or CTRL to remove.";

                }
                else if (CurrentBrushShape == BrushShape.Fill) {

                    statusBarText += "Flood Fill Tool:";

                    switch (CurrentTab) {

                        case Tabs.Tiles: {

                                statusBarText += $" Click to fill empty area with tile, or replace targeted tile. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                break;

                            }
                        case Tabs.Walls: {

                                statusBarText += $" Click to fill empty area with wall, or replace targeted wall. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                break;

                            }
                        case Tabs.Structures: {

                                statusBarText = "";

                                break;

                            }
                        case Tabs.Other: {

                                if (NeoDraw.OtherToCreate == null)
                                    break;

                                switch (OthersList[(int)NeoDraw.OtherToCreate].Name) {

                                    case OtherNames.Converter: {

                                            string convertType;

                                            switch (_otherStyle) {
                                                case 0: convertType = "Corruption"; break;
                                                case 1: convertType = "Crimson"; break;
                                                case 2: convertType = "Hallow"; break;
                                                case 3: convertType = "Jungle"; break;
                                                case 4: convertType = "Mushroom"; break;
                                                default: convertType = "Purity"; break;
                                            }

                                            statusBarText += $" Click to convert targeted tile to {convertType}. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                            break;

                                        }
                                    case OtherNames.Liquid: {

                                            string liquidType = "";

                                            switch (_otherStyle) {
                                                case 0: liquidType = "Honey"; break;
                                                case 1: liquidType = "Lava"; break;
                                                case 2: liquidType = "Water"; break;
                                            }

                                            statusBarText += $" Click to replace targeted liquid with {liquidType}. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                            break;

                                        }
                                    case OtherNames.Mech: {

                                            string mechType = "";

                                            switch (_otherStyle) {
                                                case 0: mechType = "Actuator"; break;
                                                case 1: mechType = "Blue Wire"; break;
                                                case 2: mechType = "Green Wire"; break;
                                                case 3: mechType = "Red Wire"; break;
                                                case 4: mechType = "Yellow Wire"; break;
                                            }

                                            break;

                                        }
                                    case OtherNames.PaintTile: {

                                            statusBarText += $" Click to fill tile area with paint. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                            break;

                                        }
                                    case OtherNames.PaintWall: {

                                            statusBarText += $" Click to fill wall area with paint. Use {AltKey} to continue filling off screen. Max fill: {MaxFloodFill}";

                                            break;

                                        }
                                    case OtherNames.ResetFrames: {

                                            break;

                                        }
                                    case OtherNames.RoomCheck: {

                                            break;

                                        }
                                    case OtherNames.Slope: {

                                            string slopeType = "";

                                            switch (_otherStyle) {
                                                case 0: slopeType = "Full"; break;
                                                case 1: slopeType = "Half Brick"; break;
                                                case 2: slopeType = "Up Right"; break;
                                                case 3: slopeType = "Up Left"; break;
                                                case 4: slopeType = "Down Right"; break;
                                                case 5: slopeType = "Down Left"; break;
                                            }

                                            break;

                                        }
                                    case OtherNames.Smooth: {

                                            break;

                                        }
                                    case OtherNames.Spawn: {

                                            string spawnType = "";

                                            switch (_otherStyle) {
                                                case 0: spawnType = "Player"; break;
                                                case 1: spawnType = "World"; break;
                                            }

                                            break;

                                        }

                                }

                                break;

                            }

                    }

                }
                else {

                    statusBarText += "Brush Size: (";

                    string lengthX = BrushSize.ToString();
                    string lengthY = BrushSize.ToString();

                    if (CurrentBrushShape == BrushShape.Line) {

                        if (StartPoint == default) {
                            lengthX = lengthY = "-";
                        }
                        else {
                            lengthY = (1 + (int)Math.Max(Math.Abs(Neo.TileTargetX - StartPoint.X), Math.Abs(Neo.TileTargetY - StartPoint.Y))).ToString();
                        }

                    }

                    statusBarText += lengthX + ", " + lengthY + ")";

                    if (CurrentBrushShape == BrushShape.Line && StartPoint != default) {

                        float angle = MathHelper.ToDegrees((float)Math.Atan2(Neo.TileTargetY - StartPoint.Y, Neo.TileTargetX - StartPoint.X)) * -1;
                        statusBarText += " " + angle.ToString("F1") + "° - Hold CTRL to snap angle in 45° increments.";

                    }

                    if (CurrentTab == Tabs.Other) {

                        switch (OtherToCreateName) {

                            case OtherNames.RoomCheck: {

                                    statusBarText = "Click a location to test if it is a valid room.";
                                    break;

                                }
                            case OtherNames.Slope: {

                                    statusBarText += " (Hold ALT to also apply to interior tiles)";
                                    break;

                                }
                            case OtherNames.Spawn: {

                                    if (_otherStyle == 0) {
                                        statusBarText = "Click a location to set the Player's spawn point.";
                                    }
                                    else if (_otherStyle == 1) {
                                        statusBarText = "Click a location to set the World's spawn point.";
                                    }
                                    else {
                                        statusBarText = "Select whether to place the Player or World spawn point.";
                                    }

                                    break;

                                }

                        }

                    }
                    else if (CurrentTab == Tabs.Structures) {

                        switch (StructureToCreateName) {

                            default: {
                                    statusBarText = "Click a location to place the structure.";
                                    break;
                                }

                        }

                    }

                }

            }

            Drawing.StringShadowed(sb, Main.fontMouseText, statusBarText, new Vector2(mainBarPosX + 8, barPosY + 9), Color.White);

            string currentTime = DateTime.Now.ToShortTimeString();
            Vector2 stringSize = Main.fontMouseText.MeasureString(currentTime);

            Drawing.StringShadowed(sb, Main.fontMouseText, DateTime.Now.ToShortTimeString(), new Vector2(Main.screenWidth - stringSize.X - 14, barPosY + 9), Color.White);

        }

        private static void DrawBrushOutline(SpriteBatch sb) {

            SwitchToUIZoom();

            if (GetInstance<NeoConfigServer>().DebugMode)
                ShowHoveredTileInfo(sb);

            SwitchToGameZoom();

            switch (CurrentPaintMode) {

                case PaintMode.Paint: case PaintMode.Erase: {

                        if (CurrentTab == Tabs.Structures) {

                            if (CurrentPaintMode == PaintMode.Paint) {

                                SwitchToUIZoom();

                                sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);

                                SwitchToGameZoom();

                                if (CurrentBrushShape == BrushShape.Square || CurrentBrushShape == BrushShape.Circle) {

                                    if (NeoDraw.StructureToCreate.HasValue && NeoDraw.StructureToCreate.GetValueOrDefault() < StructuresList.Count) {

                                        switch (StructuresList[(int)NeoDraw.StructureToCreate].Name) {

                                            case StructureNames.LavaTrap: {

                                                    int xStart = Neo.TileTargetX;
                                                    int yStart = Neo.TileTargetY;

                                                    int startY = yStart;

                                                    List<TextSnippet> snippets = new List<TextSnippet>();

                                                    while (!Main.tile[xStart, yStart].active() || !Main.tileSolid[Main.tile[xStart, yStart].type]) {

                                                        yStart++;

                                                        if (yStart > Main.maxTilesY) {
                                                            snippets.Add(new TextSnippet("Trap too close to bottom of world.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }

                                                        if (yStart - startY > 20) {
                                                            snippets.Add(new TextSnippet("Ground not found below.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }

                                                    }

                                                    snippets.Add(new TextSnippet("Ground below found.", Color.Green));

                                                    yStart--;

                                                    if (Main.tile[xStart, yStart].active() && !Main.tileCut[Main.tile[xStart, yStart].type] && Main.tile[xStart, yStart].type != TileID.Stalactite && Main.tile[xStart, yStart].type != TileID.SmallPiles && Main.tile[xStart, yStart].type != TileID.LargePiles && Main.tile[xStart, yStart].type != TileID.LargePiles2) {

                                                        snippets.Add(new TextSnippet("\nGround not clear.", Color.Red));
                                                        goto LavaTrapEnd;
                                                    }

                                                    snippets.Add(new TextSnippet("\nGround clear.", Color.Green));

                                                    int pressurePlateY = yStart;
                                                    startY = yStart;

                                                    yStart--;

                                                    while (!Main.tile[xStart, yStart].active() || !Main.tileSolid[Main.tile[xStart, yStart].type]) {

                                                        yStart--;

                                                        if (yStart < 10) {
                                                            snippets.Add(new TextSnippet("\nTrap too close to the top of the world.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }

                                                        if (startY - yStart > MaxLavaTrapHeight) {
                                                            snippets.Add(new TextSnippet("\nCeiling not close enough.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }

                                                    }

                                                    snippets.Add(new TextSnippet("Ceiling above found.", Color.Green));

                                                    int ceilingStartY = yStart;

                                                    if (pressurePlateY - ceilingStartY < MinLavaTrapHeight) {
                                                        snippets.Add(new TextSnippet("\nNot enough clearance between floor and ceiling.", Color.Red));
                                                        goto LavaTrapEnd;
                                                    }

                                                    snippets.Add(new TextSnippet("\nDistance between floor and ceiling is good.", Color.Green));

                                                    startY = yStart;

                                                    yStart--;

                                                    while (Main.tile[xStart, yStart].liquid < 255 || !Main.tile[xStart, yStart].lava()) {

                                                        yStart--;

                                                        if (yStart < 10) {
                                                            snippets.Add(new TextSnippet("\nTrap too close to top of world.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }


                                                        if (startY - yStart > 20) {
                                                            snippets.Add(new TextSnippet("No lava found above ceiling.", Color.Red));
                                                            goto LavaTrapEnd;
                                                        }

                                                    }

                                                    int lavaPoolStartY = yStart;

                                                    int lavaVolume = 0;

                                                    for (int i = xStart - LavaTrapVolumeCheckWidth; i <= xStart + LavaTrapVolumeCheckWidth; i++)
                                                        for (int j = yStart - LavaTrapVolumeCheckWidth * 2; j <= yStart; j++)
                                                            if (!Main.tile[i, j].active() && Main.tile[i, j].lava())
                                                                lavaVolume++;

                                                    if (lavaVolume < MinLavaTrapVolume) {
                                                        snippets.Add(new TextSnippet("Trap requires more lava.", Color.Red));
                                                        goto LavaTrapEnd;
                                                    }

                                                    snippets.Add(new TextSnippet("Lava supply sufficient.", Color.Green));

                                                    yStart++;

                                                    List<Point> actuatorPoints = new List<Point>();
                                                    List<Point> wirePoints = new List<Point>();

                                                    Point pressurePlatePoint = new Point(xStart, pressurePlateY);

                                                    while (yStart < pressurePlateY) {

                                                        if (yStart <= ceilingStartY && Main.tileSolid[Main.tile[xStart, yStart].type]) {
                                                            actuatorPoints.Add(new Point(xStart, yStart));
                                                        }
                                                        else {
                                                            wirePoints.Add(new Point(xStart, yStart));
                                                        }

                                                        yStart++;

                                                    }

                                                    foreach (Point actuatorPoint in actuatorPoints) {
                                                        sb.Draw(Square, new Rectangle((int)(actuatorPoint.X * 16 - Main.screenPosition.X), (int)(actuatorPoint.Y * 16 - Main.screenPosition.Y), 18, 18), Color.Yellow * 0.15f);
                                                    }

                                                    foreach (Point wirePoint in wirePoints) {
                                                        sb.Draw(Square, new Rectangle((int)(wirePoint.X * 16 - Main.screenPosition.X), (int)(wirePoint.Y * 16 - Main.screenPosition.Y), 18, 18), Color.Green * 0.15f);
                                                    }

                                                    sb.Draw(Square, new Rectangle((int)(pressurePlatePoint.X * 16 - Main.screenPosition.X), (int)(pressurePlatePoint.Y * 16 - Main.screenPosition.Y), 18, 18), Color.Brown * 0.15f);

LavaTrapEnd:

                                                    MouseText(sb, snippets.ToArray());

                                                    break;

                                                }
                                            case StructureNames.LivingTree:
                                            case StructureNames.LivingMahoganyTree: {

                                                    string mouseText = "";

                                                    int xStart = (int)Neo.TileTarget_Vector.X;
                                                    int yStart = (int)Neo.TileTarget_Vector.Y;

                                                    while (yStart + 1 < Main.maxTilesY && !WorldGen.SolidTile(xStart, yStart + 1))
                                                        yStart++;

                                                    if (!WorldGen.SolidTile(xStart, yStart + 1)) {
                                                        mouseText = $"No suitable ground below.\nHold {AltKey} key to force placement.";
                                                        goto DoneTesting;
                                                    }

                                                    if (Main.tile[xStart, yStart].active() && !Main.tileCut[Main.tile[xStart, yStart].type]) {
                                                        mouseText = $"Starting ground tile blocked.\nHold {AltKey} key to force placement.";
                                                        goto DoneTesting;
                                                    }

                                                    if (yStart < 150) {
                                                        mouseText = $"Too close to the top of the world.\nHold {AltKey} key to force placement.";
                                                        goto DoneTesting;
                                                    }

                                                    int farLeft = xStart - 50;
                                                    int farRight = xStart + 50;

                                                    for (int curX = farLeft; curX <= farRight; curX++) {
                                                        for (int curY = 5; curY < yStart - 5; curY++) {
                                                            if (Main.tile[curX, curY].active() && !Main.tileCut[Main.tile[curX, curY].type]) {
                                                                mouseText = $"Not enough side clearance.\nHold {AltKey} key to force placement.";
                                                                goto DoneTesting;
                                                            }
                                                        }
                                                    }

DoneTesting:

                                                    if (mouseText != "")
                                                        MouseText(sb, mouseText);

                                                    break;

                                                }
                                            /*case StructureNames.UndergroundHouse:

                                                for (int x = Neo.TileTargetX - WorldGen.LeftHalfWidth; x < Neo.TileTargetX + WorldGen.RightHalfWidth; x++)
                                                    for (int y = Neo.TileTargetY - WorldGen.TopHalfWidth; y < Neo.TileTargetY + WorldGen.BottomHalfWidth + 2; y++)
                                                        sb.Draw(Square, new Rectangle((int)((x + 1) * 16 - 2 - Main.screenPosition.X), (int)(y * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.Green * 0.5f);

                                                break;

                                            case StructureNames.JungleShrine:

                                                for (int x = Neo.TileTargetX - WorldGen.JungleShrineWidth - 2; x < Neo.TileTargetX + WorldGen.JungleShrineWidth + 1; x++)
                                                    for (int y = Neo.TileTargetY - WorldGen.JungleShrineHeight - 1; y < Neo.TileTargetY + WorldGen.JungleShrineHeight + 2; y++)
                                                        sb.Draw(Square, new Rectangle((int)((x + 1) * 16 - 2 - Main.screenPosition.X), (int)(y * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.Green * 0.5f);

                                                break;*/

                                            case StructureNames.CloudIslandHouse: {

                                                    Vector2 position = new Vector2(Neo.TileTargetX, Neo.TileTargetY);

                                                    position.X = Neo.TileTargetX + (IslandHouseWidth + 2) * IslandHouseRandomSide;

                                                    Find(new Point((int)position.X, Neo.TileTargetY), Searches.Chain(new Searches.Down(30), new Conditions.IsSolid()), out Point ground);

                                                    position.Y = ground.Y - 1;
                                                    position.X = Neo.TileTargetX;

                                                    int startX = (int)(position.X - IslandHouseWidth - 2 + (IslandHouseRandomSide == 1 ? -1 : 0));
                                                    int endX = (int)(position.X + IslandHouseWidth + 1f + (IslandHouseRandomSide == -1 ? 1 : 0));
                                                    int startY = (int)(position.Y - IslandHouseHeight - 2f);
                                                    int endY = (int)(position.Y + 3f);

                                                    Neo.WorldRestrain(ref startX, ref endX, ref startY, ref endY);

                                                    for (int x = startX; x < endX; x++)
                                                        for (int y = startY; y < endY; y++)
                                                            sb.Draw(Square, new Rectangle((int)((x + 1) * 16 - 2 - Main.screenPosition.X), (int)(y * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.Green * 0.15f);

                                                    int doorX = IslandHouseRandomSide == 1 ? endX : startX + 1;

                                                    for (int i = 3; i < 6; i++)
                                                        sb.Draw(Square, new Rectangle((int)(doorX * 16 - 2 - Main.screenPosition.X), (int)((endY - i) * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.Blue * 0.15f);

                                                    break;

                                                }
                                            case StructureNames.EpicTree: {

                                                    DrawEpicTreeOutline(sb);

                                                    break;

                                                }

                                        }

                                    }

                                }

                            }

                            return;

                        }
                        else if (CurrentTab == Tabs.Other) {

                            if (OtherToCreateName == OtherNames.RoomCheck || OtherToCreateName == OtherNames.Spawn) {

                                SwitchToUIZoom();

                                sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);

                                SwitchToGameZoom();

                                return;

                            }

                        }

                        ShowSelectedTileInfo(sb);

                        if (CurrentBrushShape == BrushShape.Square) {

                            if (CurrentPaintMode == PaintMode.Paint && BrushSize == 1 && (CurrentTab == Tabs.Tiles || CurrentTab == Tabs.Walls)) {

                                if (CurrentTab == Tabs.Tiles)
                                    DrawPlacingTile(sb);

                                if ((NeoDraw.TileToCreate == null || !Main.tileFrameImportant[(int)NeoDraw.TileToCreate]) && NeoDraw.TileToCreate.GetValueOrDefault(-1) != TileID.Cactus)
                                    sb.DrawRectangle(new Vector2((Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f)) * 16 - Main.screenPosition.X, (Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f)) * 16 - Main.screenPosition.Y), new Vector2(BrushSize * 16), Color.Black * Main.cursorAlpha, 3);

                            }
                            else {

                                for (int y = Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f); y < Neo.TileTargetY + (int)Math.Ceiling(BrushSize / 2f); y++)
                                    for (int x = Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f); x < Neo.TileTargetX + (int)Math.Ceiling(BrushSize / 2f); x++)
                                        Drawing.DrawBox(sb, x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y, 16, 16, Color.Yellow * 0.1f);

                                sb.DrawRectangle(new Vector2((Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f)) * 16 - Main.screenPosition.X, (Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f)) * 16 - Main.screenPosition.Y), new Vector2(BrushSize * 16), Color.Black * Main.cursorAlpha, 3);

                            }

                        }
                        else if (CurrentBrushShape == BrushShape.Circle) {

                            Vector2 centerTarget = new Vector2(Neo.TileTargetX * 16 + 8, Neo.TileTargetY * 16 + 8);

                            if (CurrentPaintMode == PaintMode.Paint && BrushSize == 1) {

                                if (CurrentTab == Tabs.Tiles)
                                    DrawPlacingTile(sb);

                                if (NeoDraw.TileToCreate == null || !Main.tileFrameImportant[(int)NeoDraw.TileToCreate])
                                    sb.DrawCircle(new Vector2(centerTarget.X + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.X, centerTarget.Y + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.Y), BrushSize * 8 + 8, 16, Color.Black * Main.cursorAlpha, 3f);

                            }
                            else {

                                for (int y = Neo.TileTargetY * 16 + 8 - BrushSize * 8; y < Neo.TileTargetY * 16 + 8 + BrushSize * 8; y += 16)
                                    for (int x = Neo.TileTargetX * 16 + 8 - BrushSize * 8; x < Neo.TileTargetX * 16 + 8 + BrushSize * 8; x += 16)
                                        if (Vector2.Distance(centerTarget, new Vector2(x + 8, y + 8)) < BrushSize * 8)
                                            Drawing.DrawBox(sb, x + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.X, y + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.Y, 16, 16, Color.Yellow * 0.1f);

                                sb.DrawCircle(new Vector2(centerTarget.X + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.X, centerTarget.Y + (_brushSize % 2 == 0 ? -8 : 0) - Main.screenPosition.Y), BrushSize * 8 + 8, 16, Color.Black * Main.cursorAlpha, 3f);

                            }

                        }
                        else if (CurrentBrushShape == BrushShape.Fill) {

                            SwitchToUIZoom();

                            sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);

                            SwitchToGameZoom();

                        }
                        else if (CurrentBrushShape == BrushShape.Line) {

                            SwitchToUIZoom();

                            sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);

                            SwitchToGameZoom();

                            if (StartPoint != default) {

                                List<Point> points = GetLinePoints();

                                Texture2D value = GetTexture("Terraria/Extra_68");

                                for (int i = 0; i < points.Count; i++) {

                                    Color color = new Color(0.24f, 0.8f, 0.9f, 0.5f) * 0.75f;

                                    sb.Draw(value, Main.ReverseGravitySupport(points[i].ToVector2() * 16f - Main.screenPosition, 16f), new Rectangle(0, 0, 18, 18), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

                                }

                            }

                        }

                        break;

                    }
                case PaintMode.Eyedropper: {

                        SwitchToUIZoom();

                        sb.Draw(UIButtonTextures, new Vector2(Main.mouseX, Main.mouseY - 32), new Rectangle(508, 4, 32, 32), Color.White);

                        SwitchToGameZoom();

                        break;

                    }
                case PaintMode.Select: {

                        SwitchToUIZoom();

                        sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);
                        
                        SwitchToGameZoom();

                        if (StartPoint != default) {

                            UpdateSelectionFrame();

                            Tuple<List<Point>, Vector4> selection = GetSelectedPoints();

                            List<Point> points = selection.Item1;

                            int i;

                            for (i = 0; i < points.Count; i++)
                                sb.Draw(Square, new Rectangle((int)(points[i].X * 16 - 2 - Main.screenPosition.X), (int)(points[i].Y * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.Yellow * 0.5f);

                            int minX = (int)selection.Item2.X;
                            int maxX = (int)selection.Item2.Y;
                            int minY = (int)selection.Item2.Z;
                            int maxY = (int)selection.Item2.W;

                            for (i = 0; i + minX <= maxX; i++) {

                                sb.Draw(SelectionDash, new Rectangle((int)((minX + i) * 16 - 2 - Main.screenPosition.X), (int)(minY * 16 - 2 - Main.screenPosition.Y), 16, 2),
                                        new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White);

                                sb.Draw(SelectionDash, new Rectangle((int)((minX + i) * 16 - 2 - Main.screenPosition.X), (int)(maxY * 16 + 16 - Main.screenPosition.Y), 16, 2),
                                    new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, 0f, default, SpriteEffects.FlipHorizontally, 0);

                            }

                            for (i = 0; i + minY < maxY; i++) {

                                sb.Draw(SelectionDash, new Rectangle((int)(minX * 16 - Main.screenPosition.X), (int)((minY + i) * 16 - Main.screenPosition.Y), 16, 2),
                                    new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.FlipHorizontally, 0f);

                                sb.Draw(SelectionDash, new Rectangle((int)(maxX * 16 + 16 - Main.screenPosition.X), (int)((minY + i) * 16 - 2 - Main.screenPosition.Y), 16, 2),
                                    new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.None, 0f);

                            }

                            sb.Draw(SelectionDash, new Rectangle((int)(minX * 16 - Main.screenPosition.X), (int)((minY + i) * 16 - Main.screenPosition.Y), 16, 2),
                                new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.FlipHorizontally, 0f);

                            sb.Draw(SelectionDash, new Rectangle((int)(maxX * 16 + 16 - Main.screenPosition.X), (int)((minY + i) * 16 - 2 - Main.screenPosition.Y), 20, 2),
                                new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.None, 0f);

                        }

                        break;

                    }
                case PaintMode.MagicWand: {

                        SwitchToUIZoom();

                        sb.Draw(GetTexture("Terraria/UI/Cursor_15"), new Vector2(Main.mouseX - 11 * Main.GameZoomTarget, Main.mouseY - 11 * Main.GameZoomTarget), null, Color.White, 0f, default, Main.GameZoomTarget, SpriteEffects.None, 1f);
                        
                        SwitchToGameZoom();

                        break;

                    }

            }

        }

        #endregion Draw UI

        #region OnClick

        private static void LeftClick() {

            if (_scrolling || _subScrolling)
                return;

            if (Pasting) {

                if (!Main.mouseLeftRelease)
                    return;

                DrawSelection();

                return;

            }

            if (CurrentPaintMode == PaintMode.Paint || CurrentPaintMode == PaintMode.Erase) {

                if (_undo == null)
                    _undo = new UndoStep();

                if (CurrentBrushShape == BrushShape.Square) {

                    for (int y = Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f); y < Neo.TileTargetY + (int)Math.Ceiling(BrushSize / 2f); y++) {

                        for (int x = Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f); x < Neo.TileTargetX + (int)Math.Ceiling(BrushSize / 2f); x++) {

                            if (CurrentPaintMode == PaintMode.Erase) {

                                switch (CurrentTab) {

                                    case Tabs.Tiles: {

                                            if (Main.tile[x, y].active() || Main.tile[x, y].liquid > 0) {

                                                EraseTile(x, y);

                                            }

                                            break;
                                        }
                                    case Tabs.Walls: {

                                            if (Main.tile[x, y].wall > 0)
                                                DrawWall(x, y, ref _undo, 0);

                                            break;

                                        }
                                    case Tabs.Structures: {
                                            break;
                                        }
                                    case Tabs.Other: {
                                            
                                            EraseOther(x, y);
                                            break;
                                        }

                                }

                            }
                            else {

                                switch (CurrentTab) {

                                    case Tabs.Tiles: {

                                            if (Main.tile[x, y].active() && Main.tile[x, y].type == NeoDraw.TileToCreate && NeoDraw.TileToCreate != TileID.MinecartTrack)
                                                break;

                                            DrawTile(x, y);

                                            break;

                                        }
                                    case Tabs.Walls: {

                                            if (Main.tile[x, y].wall != NeoDraw.WallToCreate)
                                                DrawWall(x, y, ref _undo);

                                            break;

                                        }
                                    case Tabs.Structures: {

                                            DrawStructure();

                                            break;

                                        }
                                    case Tabs.Other: {

                                            DrawOther(x, y);
                                            break;

                                        }

                                }

                            }

                        }

                    }

                    for (int y = Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f); y < Neo.TileTargetY + (int)Math.Ceiling(BrushSize / 2f); y++) {

                        for (int x = Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f); x < Neo.TileTargetX + (int)Math.Ceiling(BrushSize / 2f); x++) {

                            switch (CurrentTab) {

                                case Tabs.Tiles:
                                    WorldGen.SquareTileFrame(x, y);
                                    break;

                                case Tabs.Walls:
                                    WorldGen.SquareWallFrame(x, y);
                                    break;

                                case Tabs.Structures:
                                    break;

                                case Tabs.Other:
                                    WorldGen.SquareTileFrame(x, y);
                                    break;

                            }

                        }

                    }

                }
                else if (CurrentBrushShape == BrushShape.Circle) {

                    Vector2 centerTarget = new Vector2(Neo.TileTargetX * 16 + 8, Neo.TileTargetY * 16 + 8);

                    for (int y = Neo.TileTargetY * 16 + 8 - BrushSize * 8; y < Neo.TileTargetY * 16 + 8 + BrushSize * 8; y += 16) {

                        for (int x = Neo.TileTargetX * 16 + 8 - BrushSize * 8; x < Neo.TileTargetX * 16 + 8 + BrushSize * 8; x += 16) {

                            if (Vector2.Distance(centerTarget, new Vector2(x + 8, y + 8)) <= BrushSize * 8) {

                                if (CurrentPaintMode == PaintMode.Erase) {

                                    switch (CurrentTab) {

                                        case Tabs.Tiles:

                                            if (Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].active() ||
                                                Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].liquid > 0) {

                                                EraseTile(x, y, true);

                                            }

                                            break;

                                        case Tabs.Walls:

                                            if (Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].wall > 0)
                                                DrawWall(x, y, ref _undo, 0, true);

                                            break;

                                        case Tabs.Structures:
                                            break;

                                        case Tabs.Other:
                                            EraseOther(x, y, true);
                                            break;

                                    }



                                }
                                else {

                                    switch (CurrentTab) {

                                        case Tabs.Tiles:

                                            if (!Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].active() || Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].type != NeoDraw.TileToCreate) {

                                                DrawTile(x, y, -1, true);

                                            }

                                            break;

                                        case Tabs.Walls:

                                            if (Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].wall != NeoDraw.WallToCreate)
                                                DrawWall(x, y, ref _undo, -1, true);

                                            break;

                                        case Tabs.Structures:
                                            DrawStructure();
                                            break;

                                        case Tabs.Other:
                                            DrawOther(x, y, true);
                                            break;

                                    }

                                }

                            }

                        }

                    }

                    for (int y = Neo.TileTargetY * 16 + 8 - BrushSize * 8; y < Neo.TileTargetY * 16 + 8 + BrushSize * 8; y += 16) {

                        for (int x = Neo.TileTargetX * 16 + 8 - BrushSize * 8; x < Neo.TileTargetX * 16 + 8 + BrushSize * 8; x += 16) {

                            int curX = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                            int curY = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;

                            switch (CurrentTab) {

                                case Tabs.Tiles:
                                    WorldGen.SquareTileFrame(curX, curY);
                                    break;

                                case Tabs.Walls:
                                    WorldGen.SquareWallFrame(curX, curY);
                                    break;

                                case Tabs.Structures:
                                    break;

                                case Tabs.Other:
                                    WorldGen.SquareTileFrame(curX, curY);
                                    break;

                            }

                        }

                    }

                }
                else if (CurrentBrushShape == BrushShape.Line) {

                    if (!Main.mouseLeftRelease)
                        return;

                    if (CurrentTab == Tabs.Tiles) {

                        if (NeoDraw.TileToCreate == null)
                            return;

                        ushort type = (ushort)NeoDraw.TileToCreate;

                        if (Main.tileFrameImportant[type] && !lineCompatible.Contains(type) && !TileID.Sets.Platforms[type])
                            return;

                    }
                    else if (CurrentTab == Tabs.Walls) {

                        if (NeoDraw.WallToCreate == null)
                            return;

                    }
                    else if (CurrentTab == Tabs.Other) {

                        if (NeoDraw.OtherToCreate == null)
                            return;

                    }

                    if (StartPoint == default) {

                        StartPoint = Neo.TileTarget_Vector;

                    }
                    else {

                        if (CurrentTab == Tabs.Tiles) {
                            DrawLine();
                        }
                        else if (CurrentTab == Tabs.Walls) {
                            DrawLineWall();
                        }
                        else if (CurrentTab == Tabs.Other) {
                            DrawLineOther();
                        }

                        StartPoint = (Main.keyState.PressingAlt() || Main.keyState.PressingShift()) ? new Vector2(Neo.TileTargetX, Neo.TileTargetY) : default;

                    }

                }
                else if (CurrentBrushShape == BrushShape.Fill) {

                    if (!Main.mouseLeftRelease)
                        return;

                    switch (CurrentTab) {

                        case Tabs.Tiles:

                            bool canFloodFill = NeoDraw.TileToCreate.HasValue && Main.tileSolid[NeoDraw.TileToCreate.GetValueOrDefault()] && !Main.tileFrameImportant[NeoDraw.TileToCreate.GetValueOrDefault()];

                            if (CurrentPaintMode == PaintMode.Paint && (NeoDraw.TileToCreate == null || !canFloodFill))
                                return;

                            StartFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.type, CurrentPaintMode == PaintMode.Paint ? NeoDraw.TileToCreate.GetValueOrDefault() : -1);

                            break;

                        case Tabs.Walls:

                            if (CurrentPaintMode == PaintMode.Paint && NeoDraw.WallToCreate == null)
                                return;

                            StartWallFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.wall, CurrentPaintMode == PaintMode.Paint ? NeoDraw.WallToCreate.GetValueOrDefault() : 0);

                            break;

                        case Tabs.Structures:
                            break;

                        case Tabs.Other:

                            if (CurrentPaintMode == PaintMode.Paint && NeoDraw.OtherToCreate == null)
                                return;

                            StartOtherFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile, CurrentPaintMode == PaintMode.Paint ? NeoDraw.OtherToCreate.GetValueOrDefault() : -1);

                            break;

                    }

                }

            }
            else if (CurrentPaintMode == PaintMode.Eyedropper) { // TODO: Add Eyedropper support for selecting Substyle

                if (!Main.mouseLeftRelease)
                    return;

                switch (CurrentTab) {

                    case Tabs.Tiles: {

                            Tile tile = Neo.TileTarget_Tile;

                            if (!tile.active())
                                return;

                            NeoDraw.TileToCreate = tile.type;
                            TileScroll = tile.type;

                            if (ListFiltered != null) {

                                for (int i = 0; i < ListFiltered.Count; i++) {

                                    if (ListFiltered[i].ID == tile.type) {

                                        TileScroll = i;
                                        break;

                                    }

                                }

                            }

                            if (tile.type == 28) {

                                if (tile.frameY >= 1008) {
                                    _placeStyle = 9;
                                }
                                else if (tile.frameY >= 900) {
                                    _placeStyle = 8;
                                }
                                else if (tile.frameY >= 792) {
                                    _placeStyle = 7;
                                }
                                else if (tile.frameY >= 684) {
                                    _placeStyle = 6;
                                }
                                else if (tile.frameY >= 576) {
                                    _placeStyle = 5;
                                }
                                else if (tile.frameY >= 468) {
                                    _placeStyle = 4;
                                }
                                else if (tile.frameY >= 360) {
                                    _placeStyle = 3;
                                }
                                else if (tile.frameY >= 252) {
                                    _placeStyle = 2;
                                }
                                else if (tile.frameY >= 144) {
                                    _placeStyle = 1;
                                }
                                else {
                                    _placeStyle = 0;
                                }

                            }
                            else if (!Main.tileFrameImportant[tile.type]) {
                                _placeStyle = 0;
                            }
                            else if (tile.type < TileNames.OriginalTileCount) { // TODO: Fix this to match max tiles in v 3. Done. Did this fix?

                                TOD tileObjectData = TOD.GetTileData(tile);

                                if (tileObjectData == null) {

                                    _placeStyle = 0;

                                }
                                else if (tileObjectData.StyleHorizontal) {

                                    _placeStyle = tile.frameY / TOD.GetTileData(tile).CoordinateFullHeight;

                                }
                                else {

                                    _placeStyle = tile.frameX / TOD.GetTileData(tile).CoordinateFullWidth;

                                }

                            }

                            SubScroll = _placeStyle;

                            break;

                        }
                    case Tabs.Walls: {

                            NeoDraw.WallToCreate = Neo.TileTarget_Tile.wall;

                            WallScroll = (int)NeoDraw.WallToCreate;

                            if (ListFiltered != null) {

                                for (int i = 0; i < ListFiltered.Count; i++) {

                                    if (ListFiltered[i].ID == NeoDraw.WallToCreate) {

                                        WallScroll = i;
                                        break;

                                    }

                                }

                            }

                            break;

                        }
                    case Tabs.Structures: {

                            break;

                        }
                    case Tabs.Other: {

                            break;

                        } // TODO: Make Eyedropper work with the Other Tab

                }

            }
            else if (CurrentPaintMode == PaintMode.Select) {

                if (!Main.mouseLeftRelease)
                    return;

                if (StartPoint == default) {

                    StartPoint = Neo.TileTarget_Vector;

                }
                else {

                    Tuple<List<Point>, Vector4> selection = GetSelectedPoints();

                    List<Point> points = selection.Item1;

                    if (Main.keyState.PressingShift()) { // Add points


                        foreach (Point point in points) {

                            int hash = GetHash(point);

                            if (!SelectedPointsHash.Contains(hash)) {
                                SelectedPoints.Add(point);
                                SelectedPointsHash.Add(hash);
                            }

                        }

                    }
                    else if (Main.keyState.PressingCtrl()) { // Remove points <---- Updated this 9/7 Hopefully it doesn't cause any issues

                        foreach (Point point in points) {

                            int hash = GetHash(point);

                            if (SelectedPointsHash.Contains(hash)) {
                                SelectedPoints.Remove(point);
                                SelectedPointsHash.Remove(hash);
                            }

                        }

                    }
                    else { // New List of points

                        SelectedPoints.Clear();
                        SelectedPointsHash.Clear();

                        foreach (Point point in points) {

                            SelectedPoints.Add(point);
                            SelectedPointsHash.Add(GetHash(point));

                        }

                    }

                    StartPoint = default;

                }

            }
            else if (CurrentPaintMode == PaintMode.MagicWand) {

                if (!Main.mouseLeftRelease)
                    return;

                List<Point> points = StartFloodFillSelect();

                if (Main.keyState.PressingShift()) { // Add points

                    foreach (Point point in points) {

                        int hash = GetHash(point);

                        if (!SelectedPointsHash.Contains(hash)) {
                            SelectedPoints.Add(point);
                            SelectedPointsHash.Add(hash);
                        }

                    }

                }
                else if (Main.keyState.PressingCtrl()) { // Remove points <---- Updated this 9/7 Hopefully it doesn't cause any issues

                    foreach (Point point in points) {

                        int hash = GetHash(point);

                        if (SelectedPointsHash.Contains(hash)) {
                            SelectedPoints.Remove(point);
                            SelectedPointsHash.Remove(hash);
                        }

                    }

                }
                else { // New List of points

                    SelectedPoints.Clear();
                    SelectedPointsHash.Clear();

                    foreach (Point point in points) {

                        SelectedPoints.Add(point);
                        SelectedPointsHash.Add(GetHash(point));

                    }

                }

            }

        }

        private static void RightClick() {

            if (Main.keyState.PressingAlt()) {

                if (CurrentPaintMode == PaintMode.Paint) {

                    if (_undo == null)
                        _undo = new UndoStep();

                    if (CurrentBrushShape == BrushShape.Square) {

                        for (int y = Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f); y < Neo.TileTargetY + (int)Math.Ceiling(BrushSize / 2f); y++) {

                            for (int x = Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f); x < Neo.TileTargetX + (int)Math.Ceiling(BrushSize / 2f); x++) {

                                switch (CurrentTab) {

                                    case Tabs.Tiles:

                                        if (Main.tile[x, y].active() || Main.tile[x, y].liquid > 0) {

                                            EraseTile(x, y);

                                        }

                                        break;

                                    case Tabs.Walls:

                                        if (Main.tile[x, y].wall > 0)
                                            DrawWall(x, y, ref _undo, 0);

                                        break;

                                    case Tabs.Structures:
                                        break;

                                    case Tabs.Other:
                                        EraseOther(x, y);
                                        break;

                                }

                            }

                        }

                        for (int y = Neo.TileTargetY - (int)Math.Floor(BrushSize / 2f); y < Neo.TileTargetY + (int)Math.Ceiling(BrushSize / 2f); y++) {

                            for (int x = Neo.TileTargetX - (int)Math.Floor(BrushSize / 2f); x < Neo.TileTargetX + (int)Math.Ceiling(BrushSize / 2f); x++) {

                                switch (CurrentTab) {

                                    case Tabs.Tiles:
                                        WorldGen.SquareTileFrame(x, y);
                                        break;

                                    case Tabs.Walls:
                                        WorldGen.SquareWallFrame(x, y);
                                        break;

                                    case Tabs.Structures:
                                        break;

                                    case Tabs.Other:
                                        WorldGen.SquareTileFrame(x, y);
                                        break;

                                }

                            }

                        }

                    }
                    else if (CurrentBrushShape == BrushShape.Circle) {

                        Vector2 centerTarget = new Vector2(Neo.TileTargetX * 16 + 8, Neo.TileTargetY * 16 + 8);

                        for (int y = Neo.TileTargetY * 16 + 8 - BrushSize * 8; y < Neo.TileTargetY * 16 + 8 + BrushSize * 8; y += 16) {

                            for (int x = Neo.TileTargetX * 16 + 8 - BrushSize * 8; x < Neo.TileTargetX * 16 + 8 + BrushSize * 8; x += 16) {

                                if (Vector2.Distance(centerTarget, new Vector2(x + 8, y + 8)) <= BrushSize * 8) {

                                    switch (CurrentTab) {

                                        case Tabs.Tiles:

                                            if (Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].active() ||
                                                Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].liquid > 0) {

                                                EraseTile(x, y, true);

                                            }

                                            break;

                                        case Tabs.Walls:

                                            if (Main.tile[(x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16].wall > 0)
                                                DrawWall(x, y, ref _undo, 0, true);

                                            break;

                                        case Tabs.Structures:
                                            break;

                                        case Tabs.Other:
                                            EraseOther(x, y, true);
                                            break;

                                    }

                                }

                            }

                        }

                        for (int y = Neo.TileTargetY * 16 + 8 - BrushSize * 8; y < Neo.TileTargetY * 16 + 8 + BrushSize * 8; y += 16) {

                            for (int x = Neo.TileTargetX * 16 + 8 - BrushSize * 8; x < Neo.TileTargetX * 16 + 8 + BrushSize * 8; x += 16) {

                                switch (CurrentTab) {

                                    case Tabs.Tiles:
                                        WorldGen.SquareTileFrame((x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16);
                                        break;

                                    case Tabs.Walls:
                                        WorldGen.SquareWallFrame((x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16);
                                        break;

                                    case Tabs.Structures:
                                        break;

                                    case Tabs.Other:
                                        WorldGen.SquareTileFrame((x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16, (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16);
                                        break;

                                }

                            }

                        }

                    }
                    else if (CurrentBrushShape == BrushShape.Fill && Main.mouseLeftRelease) {

                        switch (CurrentTab) {

                            case Tabs.Tiles:

                                if (!Main.tileFrameImportant[Neo.TileTarget_Tile.type])
                                    return;

                                StartFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.type, -1);

                                break;

                            case Tabs.Walls:

                                StartWallFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.wall, 0);

                                break;

                            case Tabs.Structures:
                                break;

                            case Tabs.Other:

                                StartOtherFloodFill(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile, -1);

                                break;

                        }

                    }

                }
                else if (CurrentPaintMode == PaintMode.Erase) {

                    LeftClick();

                }

            }

        }

        #endregion OnClick

        #region Draw Functions

        private static void DrawLine() {

            int type = NeoDraw.TileToCreate.GetValueOrDefault(-1);

            bool getSuperCover = CurrentTab == Tabs.Other || superCover.Contains(type);

            List<Point> points = GetLinePoints(getSuperCover);

            if (CurrentPaintMode == PaintMode.Paint && type != -1) {

                if (TileID.Sets.Platforms[type]) {

                    bool wasDownRight = false;

                    for (int i = 0; i < points.Count; i++) {

                        int curX = points[i].X;
                        int curY = points[i].Y;

                        DrawTile(curX, curY);

                        if (i == 0)
                            continue;

                        byte slope = 0;

                        int prevX = points[i - 1].X;
                        int prevY = points[i - 1].Y;

                        bool headingRight = false;
                        bool headingLeft  = false;
                        bool headingUp    = false;
                        bool headingDown  = false;

                        if (curY > prevY) {
                            headingDown = true;
                        }
                        else if (curY < prevY) {
                            headingUp = true;
                        }

                        if (curX > prevX) {
                            headingRight = true;
                        }
                        else if (curX < prevX) {
                            headingLeft = true;
                        }

                        if (headingDown) {

                            if (headingLeft) {
                                slope = 2;
                            }
                            else if (headingRight) {
                                slope = 1;
                            }

                        }
                        else if (headingUp) {

                            if (headingLeft) {
                                slope = 1;
                            }
                            else if (headingRight) {
                                slope = 2;
                            }

                        }

                        if (slope == 0) {

                            if (wasDownRight) {

                                Main.tile[prevX, prevY].halfBrick(false);
                                Main.tile[prevX, prevY].slope(0);

                            }

                            Main.tile[curX, curY].halfBrick(false);
                            Main.tile[curX, curY].slope(0);

                        }
                        else {

                            if ((headingUp && headingLeft) || (headingDown && headingRight)) {
                        
                                Main.tile[prevX, prevY].halfBrick(false);
                                Main.tile[prevX, prevY].slope(slope);
                        
                            }

                            Main.tile[curX, curY].halfBrick(false);
                            Main.tile[curX, curY].slope(slope);

                        }

                        if ((headingUp && headingLeft) || (headingDown && headingRight)) {
                            wasDownRight = true;
                        }
                        else {
                            wasDownRight = false;
                        }

                    }

                    for (int i = 0; i < points.Count; i++)
                        WorldGen.TileFrame(points[i].X, points[i].Y, true); //SquareTileFrame(points[i].X, points[i].Y, ref _undo);

                    return;

                }
                else if (TreatLikeStairs) {

                    bool wasDownRight = false;

                    for (int i = 0; i < points.Count; i++) {

                        int curX = points[i].X;
                        int curY = points[i].Y;

                        DrawTile(curX, curY);

                        if (i == 0)
                            continue;

                        byte slope = 0;

                        int prevX = points[i - 1].X;
                        int prevY = points[i - 1].Y;

                        bool headingRight = false;
                        bool headingLeft = false;
                        bool headingUp = false;
                        bool headingDown = false;

                        if (curY > prevY) {
                            headingDown = true;
                        }
                        else if (curY < prevY) {
                            headingUp = true;
                        }

                        if (curX > prevX) {
                            headingRight = true;
                        }
                        else if (curX < prevX) {
                            headingLeft = true;
                        }

                        if (headingDown) {

                            if (headingLeft) {
                                slope = 2;
                            }
                            else if (headingRight) {
                                slope = 1;
                            }

                        }
                        else if (headingUp) {

                            if (headingLeft) {
                                slope = 1;
                            }
                            else if (headingRight) {
                                slope = 2;
                            }

                        }

                        if (slope == 0) {

                            if (wasDownRight) {

                                Main.tile[prevX, prevY].halfBrick(false);
                                Main.tile[prevX, prevY].slope(0);

                            }

                            Main.tile[curX, curY].halfBrick(false);
                            Main.tile[curX, curY].slope(0);

                        }
                        else {

                            if ((headingUp && headingLeft) || (headingDown && headingRight)) {

                                Main.tile[prevX, prevY].halfBrick(false);
                                Main.tile[prevX, prevY].slope(slope);

                            }

                            Main.tile[curX, curY].halfBrick(false);
                            Main.tile[curX, curY].slope(slope);

                        }

                        if ((headingUp && headingLeft) || (headingDown && headingRight)) {
                            wasDownRight = true;
                        }
                        else {
                            wasDownRight = false;
                        }

                    }

                    for (int i = 0; i < points.Count; i++)
                        WorldGen.TileFrame(points[i].X, points[i].Y, true); //SquareTileFrame(points[i].X, points[i].Y, ref _undo);

                    return;

                }

            }

            for (int i = 0; i < points.Count; i++) {

                if (CurrentTab == Tabs.Tiles) {

                    if (CurrentPaintMode == PaintMode.Paint) {
                        DrawTile(points[i].X, points[i].Y);
                    }
                    else if (CurrentPaintMode == PaintMode.Erase) {
                        EraseTile(points[i].X, points[i].Y);
                    }

                }
                else if (CurrentTab == Tabs.Other) {

                    if (CurrentPaintMode == PaintMode.Paint) {
                        DrawOther(points[i].X, points[i].Y);
                    }
                    else if (CurrentPaintMode == PaintMode.Erase) {
                        EraseOther(points[i].X, points[i].Y);
                    }

                }

            }

            for (int i = 0; i < points.Count; i++)
                WorldGen.TileFrame(points[i].X, points[i].Y, true); //SquareTileFrame(points[i].X, points[i].Y, ref _undo);

        }

        private static void DrawLineOther() {
            
            List<Point> points = GetLinePoints(true);

            for (int i = 0; i < points.Count; i++) {

                if (CurrentPaintMode == PaintMode.Paint) {
                    DrawOther(points[i].X, points[i].Y);
                }
                else if (CurrentPaintMode == PaintMode.Erase) {
                    EraseOther(points[i].X, points[i].Y);
                }

            }

            for (int i = 0; i < points.Count; i++)
                WorldGen.TileFrame(points[i].X, points[i].Y, true);
            
        }

        private static void DrawLineWall() {

            List<Point> points = GetLinePoints();

            for (int i = 0; i < points.Count; i++) {

                if (CurrentPaintMode == PaintMode.Paint) {
                    DrawWall(points[i].X, points[i].Y, ref _undo);
                }
                else if (CurrentPaintMode == PaintMode.Erase) {
                    DrawWall(points[i].X, points[i].Y, ref _undo, 0);
                }

            }

            for (int i = 0; i < points.Count; i++)
                WorldGen.SquareWallFrame(points[i].X, points[i].Y);

        }

        private static void DrawOther(int x, int y, bool circle = false) {

            if (NeoDraw.OtherToCreate == null)
                return;

            if (circle) {
                x = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                y = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
            }

            switch (OthersList[(int)NeoDraw.OtherToCreate].Name) {

                case OtherNames.Converter: {

                        int convertType;

                        switch (_otherStyle) {
                            case 0: convertType = 1; break;
                            case 1: convertType = 4; break;
                            case 2: convertType = 2; break;
                            case 3: convertType = 5; break;
                            case 4: convertType = 3; break;
                            default: convertType = 0; break;
                        }

                        Neo.ConvertTile(x, y, convertType, ref _undo);

                        break;

                    }
                case OtherNames.Liquid: {

                        if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type])
                            return;

                        int liquid = -1;

                        switch (_otherStyle) {

                            case 0: // Honey
                                liquid = 2;
                                break;

                            case 1: // Lava
                                liquid = 1;
                                break;

                            case 2: // Water
                                liquid = 0;
                                break;

                        }

                        if (liquid == -1)
                            return;

                        _undo.Add(new ChangedTile(x, y));

                        Main.tile[x, y].liquid = 255;

                        if (liquid == 1) {

                            Main.tile[x, y].lava(true);
                            Main.tile[x, y].honey(false);

                        }
                        else if (liquid == 2) {

                            Main.tile[x, y].lava(false);
                            Main.tile[x, y].honey(true);

                        }
                        else {

                            Main.tile[x, y].lava(false);
                            Main.tile[x, y].honey(false);

                        }

                        Liquid.AddWater(x, y);

                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendTileSquare(-1, x, y, 1);

                        break;

                    }
                case OtherNames.Mech: {

                        _undo.Add(new ChangedTile(x, y));

                        if (_otherStyle == 0) {
                            Main.tile[x, y].actuator(actuator: true);
                        }
                        else if (_otherStyle == 1) {
                            Main.tile[x, y].wire2(wire2: true);
                        }
                        else if (_otherStyle == 2) {
                            Main.tile[x, y].wire3(wire3: true);
                        }
                        else if (_otherStyle == 3) {
                            Main.tile[x, y].wire(wire: true);
                        }
                        else if (_otherStyle == 4) {
                            Main.tile[x, y].wire4(wire4: true);
                        }

                        break;

                    }
                case OtherNames.PaintTile: {

                        _undo.Add(new ChangedTile(x, y));

                        WorldGen.paintTile(x, y, (byte)(_otherStyle + 1), true);

                        break;

                    }
                case OtherNames.PaintWall: {

                        _undo.Add(new ChangedTile(x, y));

                        WorldGen.paintWall(x, y, (byte)(_otherStyle + 1), true);

                        break;

                    }
                case OtherNames.ResetFrames: {

                        //SquareTileFrame(x, y, ref _undo);
                        WorldGen.SquareTileFrame(x, y);

                        break;

                    }
                case OtherNames.RoomCheck: {

                        if (StartRoomCheck(x, y))
                            SetStatusBarTempMessage("Room is good.");

                        break;

                    }
                case OtherNames.Slope: {

                        _undo.Add(new ChangedTile(x, y));

                        if (_otherStyle == 0) { // Full

                            Main.tile[x, y].halfBrick(false);
                            Main.tile[x, y].slope(0);

                        }
                        else if (_otherStyle == 1) { // Half Brick

                            if ((!Main.tile[x, y - 1].active()) || Main.keyState.PressingAlt()) {

                                Main.tile[x, y].halfBrick(true);
                                Main.tile[x, y].slope(0);

                            }

                        }
                        else if (_otherStyle == 2) { // Up Right

                            if (((!Main.tile[x + 1, y].active() || !Main.tileSolid[Main.tile[x + 1, y].type]) && (!Main.tile[x, y - 1].active() || !Main.tileSolid[Main.tile[x, y - 1].type])) || Main.keyState.PressingAlt()) {

                                Main.tile[x, y].halfBrick(false);
                                Main.tile[x, y].slope(1);

                            }

                        }
                        else if (_otherStyle == 3) { // Up Left

                            if (((!Main.tile[x - 1, y].active() || !Main.tileSolid[Main.tile[x - 1, y].type]) && (!Main.tile[x, y - 1].active() || !Main.tileSolid[Main.tile[x, y - 1].type])) || Main.keyState.PressingAlt()) {

                                Main.tile[x, y].halfBrick(false);
                                Main.tile[x, y].slope(2);

                            }

                        }
                        else if (_otherStyle == 4) { // Down Right

                            if (((!Main.tile[x + 1, y].active() || !Main.tileSolid[Main.tile[x + 1, y].type]) && (!Main.tile[x, y + 1].active() || !Main.tileSolid[Main.tile[x, y + 1].type])) || Main.keyState.PressingAlt()) {

                                Main.tile[x, y].halfBrick(false);
                                Main.tile[x, y].slope(3);

                            }

                        }
                        else if (_otherStyle == 5) { // Down Left

                            if (((!Main.tile[x - 1, y].active() || !Main.tileSolid[Main.tile[x - 1, y].type]) && (!Main.tile[x, y + 1].active() || !Main.tileSolid[Main.tile[x, y + 1].type])) || Main.keyState.PressingAlt()) {

                                Main.tile[x, y].halfBrick(false);
                                Main.tile[x, y].slope(4);

                            }

                        }

                        break;

                    }
                case OtherNames.Smooth: {

                        Smoothing(x, x + 1, y, y + 1, ref _undo, false);

                        break;

                    }
                case OtherNames.Spawn: {

                        if (!Main.mouseLeftRelease)
                            return;

                        if (_otherStyle == 0) { // Player

                            SetSpawnPlayer(x, y);

                        }
                        else if (_otherStyle == 1) { // World

                            SetSpawnWorld(x, y);

                        }

                        break;

                    }

            }

        }

        private static void DrawStructure() {

            if (NeoDraw.StructureToCreate == null || NeoDraw.StructureToCreate >= StructuresList.Count || !Main.mouseLeftRelease)
                return;

            switch (StructureToCreateName) {

                case StructureNames.CampSite: {

                        Biomes<Campsite>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.CaveOpenater: {

                        CaveOpenater(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.Caverer: {

                        Caverer(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.Cavinator: {

                        int steps = 1;

                        switch (_specialStyle) {

                            case 0: steps = 1; break;
                            case 1: steps = 2; break;
                            case 2: steps = 3; break;
                            case 3: steps = 4; break;
                            case 4: steps = 5; break;
                            case 5: steps = 10; break;
                            case 6: steps = 20; break;
                            case 7: steps = 30; break;
                            case 8: steps = 40; break;
                            case 9: steps = 50; break;

                        }

                        Cavinator(Neo.TileTargetX, Neo.TileTargetY, steps, ref _undo);

                        break;

                    }
                case StructureNames.CloudIsland: {

                        CloudIsland(Neo.TileTargetX, Neo.TileTargetY, ref _undo, _specialStyle);
                        break;

                    }
                case StructureNames.CloudIslandHouse: {

                        IslandHouse(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.CloudLake: {

                        CloudLake(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.CorruptionStart: {

                        Biomes<CorruptionStart>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.CrimsonEntrance: {

                        int crimEntDir = 1;

                        switch (_specialStyle) {

                            case 0: crimEntDir = 1; break;
                            case 1: crimEntDir = -1; break;

                        }

                        CrimEnt(Neo.TileTarget_Vector, crimEntDir, ref _undo);

                        break;

                    }
                case StructureNames.CrimsonStart: {

                        int crimStartDir = 0;

                        switch (_specialStyle) {

                            case 0: crimStartDir = 0; break;
                            case 1: crimStartDir = 1; break;
                            case 2: crimStartDir = -1; break;

                        }

                        CrimStart(Neo.TileTargetX, Neo.TileTargetY, crimStartDir, ref _undo);

                        break;

                    }
                case StructureNames.DeadMansChest: {

                        Biomes<DeadMansChest>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo, true);
                        break;

                    }
                case StructureNames.Desert: {

                        Biomes<Desert>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.Dunes: {

                        Biomes<Dunes>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.Dungeon: {

                        if (Main.keyState.PressingAlt())
                            GetGoodWorldGen = true;

                        if (Main.keyState.PressingCtrl())
                            DrunkWorldGen = true;

                        DungeonBuilder.StartDungeonCreation(Neo.TileTargetX, Neo.TileTargetY, _specialStyle - 1);
                        break;

                    }
                case StructureNames.EnchantedSword: {

                        EnchantedSword.RealSword = _specialStyle;
                        Biomes<EnchantedSword>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.EpicTree: {

                        GrowEpicTree(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.GemCave: {

                        GemCave(Neo.TileTargetX, Neo.TileTargetY, ref _undo, _specialStyle - 1);
                        break;

                    }
                case StructureNames.GraniteCave: {

                        GraniteCave.Style = _specialStyle;
                        Biomes<GraniteCave>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.GraniteCavern: {

                        MarbleCavern.SwitchToGranite = true;
                        Biomes<MarbleCavern>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.HellFort: {

                        HellFort(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.Hive: {

                        if (_specialStyle == 1) {
                            Hive.DrunkWorldGen = true;
                        }
                        else {
                            Hive.DrunkWorldGen = false;
                        }

                        Biomes<Hive>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.HoneyPatch: {

                        Biomes<HoneyPatch>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.JungleShrine: {

                        MakeJungleShrine(Neo.TileTargetX, Neo.TileTargetY, ref _undo, _specialStyle - 1);
                        break;

                    }
                case StructureNames.Lakinater: {

                        if (_specialStyle == 0) {
                            Lakinater(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        }
                        else {
                            SonOfLakinater(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        }

                        break;

                    }
                case StructureNames.LavaTrap: {

                        LavaTrap(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.LivingMahoganyTree: {

                        GrowLivingTree(Neo.TileTargetX, Neo.TileTargetY, ref _undo, TileID.LivingMahogany, TileID.LivingMahoganyLeaves, WallID.LivingWood, _specialStyle, Main.keyState.PressingAlt());
                        break;

                    }
                case StructureNames.LivingTree: {

                        GrowLivingTree(Neo.TileTargetX, Neo.TileTargetY, ref _undo, TileID.LivingWood, TileID.LeafBlock, WallID.LivingWood, _specialStyle, Main.keyState.PressingAlt());
                        break;

                    }
                case StructureNames.LihzahrdTemple: {

                        makeTemple(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        templePart2(ref _undo);
                        _undo.ResetFrames();
                        //Biomes<makeTemple>.Place(Neo.TileTargetX, Neo.TileTargetY, ref _undo); // TODO: Update this to be a Biome
                        break;

                    }
                case StructureNames.MahoganyTree: {

                        Biomes<MahoganyTree>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.MakeHole: {

                        switch (_specialStyle) {

                            case  0: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  10, 20, ref _undo); break;
                            case  1: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  20, 20, ref _undo); break;
                            case  2: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  40, 30, ref _undo); break;
                            case  3: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  60, 30, ref _undo); break;
                            case  4: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  80, 30, ref _undo); break;
                            case  5: MakeHole(Neo.TileTargetX, Neo.TileTargetY, 100, 40, ref _undo); break;
                            //case  6: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  10, 20, ref _undo, true, true); break;
                            //case  7: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  20, 20, ref _undo, true, true); break;
                            //case  8: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  40, 30, ref _undo, true, true); break;
                            //case  9: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  60, 30, ref _undo, true, true); break;
                            //case 10: MakeHole(Neo.TileTargetX, Neo.TileTargetY,  80, 30, ref _undo, true, true); break;
                            //case 11: MakeHole(Neo.TileTargetX, Neo.TileTargetY, 100, 40, ref _undo, true, true); break;
                        }

                        break;

                    }
                case StructureNames.MarbleCave: {

                        GraniteCave.Style = _specialStyle + 3;
                        Biomes<GraniteCave>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.MarbleCavern: {

                        MarbleCavern.SwitchToGranite = false;
                        Biomes<MarbleCavern>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.Meteor: {

                        float meteorSize = 1f;

                        switch (_specialStyle) {

                            case 0: meteorSize = 1f;    break; // Default
                            case 1: meteorSize = 0.5f;  break; // Tiny
                            case 2: meteorSize = 0.75f; break; // Small
                            case 3: meteorSize = 1.25f; break; // Large
                            case 4: meteorSize = 1.5f;  break; // Extra Large
                            case 5: meteorSize = 2f;    break; // Huge

                        }

                        Meteor(Neo.TileTargetX, Neo.TileTargetY, ref _undo, meteorSize);

                        break;

                    }
                case StructureNames.MiningExplosives: {

                        Biomes<MiningExplosives>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        break;

                    }
                case StructureNames.Mountinater: {

                        Mountinater(Neo.TileTargetX, Neo.TileTargetY, ref _undo, _specialStyle);
                        break;

                    }
                case StructureNames.MossCave: {

                        MossCave(Neo.TileTargetX, Neo.TileTargetY, ref _undo, _specialStyle - 2);
                        //Biomes<NeonMoss>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo); // TODO: Update this to be a biome
                        break;

                    }
                case StructureNames.Oasis: {

                        PlaceOasis(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.OceanCave: {

                        OceanCave(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.Pyramid: {

                        if (_specialStyle == 0) {
                            Pyramid(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        }
                        else if (_specialStyle == 1) {
                            Pyramid(Neo.TileTargetX, Neo.TileTargetY, ref _undo, 226, 112, 28);
                        }

                        break;

                    }
                case StructureNames.SandTrap: {

                        SandTrap(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.ShroomPatch: {

                        ShroomPatch(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.SpiderCave: {

                        int maxLocations = -1;

                        switch (_specialStyle) {

                            case 1: {
                                    maxLocations = 500;
                                    break;
                                }
                            case 2: {
                                    maxLocations = 2000;
                                    break;
                                }
                            case 3: {
                                    maxLocations = 5000;
                                    break;
                                }

                        }

                        Spider(Neo.TileTargetX, Neo.TileTargetY, ref _undo, maxLocations);
                        break;

                    }
                case StructureNames.StonePatch: {

                        StonePatch(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;

                    }
                case StructureNames.UndergroundHouse: {

                        if (_specialStyle == 0) {
                            Biomes<CaveHouse>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        }
                        else {
                            Biomes<MineHouse>.Place(Neo.TileTargetX, Neo.TileTargetY, new StructureMap(), ref _undo);
                        }

                        break;

                    }
                case StructureNames.WateryIceThing: {
                        MakeWateryIceThing(Neo.TileTargetX, Neo.TileTargetY, ref _undo);
                        break;
                    }

            }

        }

        private static void DrawTile(int x, int y, int tileType = -1, bool circle = false) {

            if (tileType == -1) {

                if (NeoDraw.TileToCreate == null)
                    return;

                tileType = (int)NeoDraw.TileToCreate;

            }

            if (circle) {
                x = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                y = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
            }

            PlaceTile(x, y, tileType, ref _undo, true, true, -1, _placeStyle);

        }

        private static void DrawWall(int x, int y, ref UndoStep undo, int wallType = -1, bool circle = false) {

            if (wallType == -1) {

                if (NeoDraw.WallToCreate == null)
                    return;

                wallType = (int)NeoDraw.WallToCreate;

            }

            if (circle) {
                x = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                y = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
            }

            if (x <= 1 || y <= 1 || x >= Main.maxTilesX - 2 || y >= Main.maxTilesY - 2)
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            Tile tile = Main.tile[x, y];

            undo.Add(new ChangedTile(x, y));

            tile.wall = (byte)wallType;

            if (wallType == 0) {

                tile.wallColor(0);
                // TODO: Anything needed here?
                //if (TileInfo.checkWalls[tile.type] || tile.type == 240 || tile.type == 241 || tile.type == 242 || tile.type == 245 || tile.type == 246 || tile.type == 4 || tile.type == 136 || tile.type == 334)
                //    WorldGen.TileFrame(x, y);

            }

        }

        #endregion Draw Functions

        #region Erase Functions

        private static void EraseOther(int x, int y, bool circle = false) {

            if (NeoDraw.OtherToCreate == null)
                return;

            if (circle) {
                x = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                y = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
            }

            switch (OtherToCreateName) {

                case OtherNames.Liquid: {

                        if (Main.tile[x, y].liquid < 1)
                            return;

                        _undo.Add(new ChangedTile(x, y));

                        Main.tile[x, y].liquid = 0;

                        break;

                    }
                case OtherNames.Mech: {

                        _undo.Add(new ChangedTile(x, y));

                        if (_otherStyle == 0) {
                            Main.tile[x, y].actuator(actuator: false);
                        }
                        else if (_otherStyle == 1) {
                            Main.tile[x, y].wire2(wire2: false);
                        }
                        else if (_otherStyle == 2) {
                            Main.tile[x, y].wire3(wire3: false);
                        }
                        else if (_otherStyle == 3) {
                            Main.tile[x, y].wire(wire: false);
                        }
                        else if (_otherStyle == 4) {
                            Main.tile[x, y].wire4(wire4: false);
                        }

                        break;

                    }
                case OtherNames.PaintTile: {

                        _undo.Add(new ChangedTile(x, y));
                        WorldGen.paintTile(x, y, 0, true);

                        break;

                    }
                case OtherNames.PaintWall: {

                        _undo.Add(new ChangedTile(x, y));
                        WorldGen.paintWall(x, y, 0, true);

                        break;

                    }
                case OtherNames.Slope: {

                        _undo.Add(new ChangedTile(x, y));

                        Main.tile[x, y].halfBrick(false);
                        Main.tile[x, y].slope(0);

                        break;

                    }
                case OtherNames.Smooth: {

                        _undo.Add(new ChangedTile(x, y));

                        Main.tile[x, y].halfBrick(false);
                        Main.tile[x, y].slope(0);

                        break;

                    }

            }

        }

        private static void EraseSelection(bool clearSelection = true) {

            if (SelectedPoints == null || SelectedPoints.Count == 0)
                return;

            NeoTile.CaptureTileChanges = true;

            if (_undo == null)
                _undo = new UndoStep();

            switch (CurrentTab) {

                case Tabs.Tiles: {

                        for (int i = 0; i < SelectedPoints.Count; i++)
                            EraseTile(SelectedPoints[i].X, SelectedPoints[i].Y);

                        for (int i = 0; i < SelectedPoints.Count; i++)
                            WorldGen.SquareTileFrame(SelectedPoints[i].X, SelectedPoints[i].Y);

                        _undo.ResetFrames();

                        break;

                    }
                case Tabs.Walls: {

                        for (int i = 0; i < SelectedPoints.Count; i++)
                            DrawWall(SelectedPoints[i].X, SelectedPoints[i].Y, ref _undo, 0);

                        _undo.ResetFrames(true);

                        break;

                    }
                case Tabs.Structures: {

                        for (int i = 0; i < SelectedPoints.Count; i++) {

                            EraseTile(SelectedPoints[i].X, SelectedPoints[i].Y);
                            DrawWall(SelectedPoints[i].X, SelectedPoints[i].Y, ref _undo, 0);

                        }

                        _undo.ResetFrames(true);

                        break;

                    }
                case Tabs.Other: {

                        switch (OtherToCreateName) {

                            case "Liquid": {

                                    for (int i = 0; i < SelectedPoints.Count; i++) {

                                        if (_otherStyle == 0) { // Honey

                                            if (Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquidType() == 2) {

                                                _undo.Add(new ChangedTile(SelectedPoints[i].X, SelectedPoints[i].Y));
                                                Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquid = 0;

                                            }

                                        }
                                        else if (_otherStyle == 1) { // Lava

                                            if (Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquidType() == 1) {

                                                _undo.Add(new ChangedTile(SelectedPoints[i].X, SelectedPoints[i].Y));
                                                Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquid = 0;

                                            }

                                        }
                                        else if (_otherStyle == 2) { // Water

                                            if (Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquidType() == 0) {

                                                _undo.Add(new ChangedTile(SelectedPoints[i].X, SelectedPoints[i].Y));
                                                Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y].liquid = 0;

                                            }

                                        }

                                    }

                                    _undo.ResetFrames();

                                    break;

                                }
                            case "Mech": {

                                    for (int i = 0; i < SelectedPoints.Count; i++) {

                                        int x = SelectedPoints[i].X;
                                        int y = SelectedPoints[i].Y;

                                        if (_otherStyle == Mech.Actuator) { // Actuator

                                            if (Main.tile[x, y].actuator()) {

                                                _undo.Add(new ChangedTile(x, y));
                                                Main.tile[x, y].actuator(actuator: false);

                                            }

                                        }
                                        else if (_otherStyle == Mech.Wire2) { // Blue Wire

                                            if (Main.tile[x, y].wire2()) {

                                                _undo.Add(new ChangedTile(x, y));
                                                Main.tile[x, y].wire2(wire2: false);

                                            }

                                        }
                                        else if (_otherStyle == Mech.Wire3) { // Green Wire

                                            if (Main.tile[x, y].wire3()) {

                                                _undo.Add(new ChangedTile(x, y));
                                                Main.tile[x, y].wire3(wire3: false);

                                            }

                                        }
                                        else if (_otherStyle == Mech.Wire) { // Red Wire

                                            if (Main.tile[x, y].wire()) {

                                                _undo.Add(new ChangedTile(x, y));
                                                Main.tile[x, y].wire(wire: false);

                                            }

                                        }
                                        else if (_otherStyle == Mech.Wire4) { // Yellow Wire

                                            if (Main.tile[x, y].wire4()) {

                                                _undo.Add(new ChangedTile(x, y));
                                                Main.tile[x, y].wire4(wire4: false);

                                            }

                                        }

                                    }

                                    _undo.ResetFrames();

                                    break;

                                }

                        }

                        break;

                    }

            }

            if (clearSelection) {
                SelectedPoints.Clear();
                SelectedPointsHash.Clear();
            }

            NeoTile.CaptureTileChanges = false;

        }

        private static void EraseTile(int x, int y, bool circle = false) {

            if (circle) {
                x = (x - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
                y = (y - (CurrentBrushShape % 2 == 0 ? 7 : 0)) / 16;
            }

            WorldGen.gen = true;

            if (Main.tile[x, y].type == TileID.Pots) {
                ErasePot(x, y);
            }
            else {
                WorldGen.KillTile(x, y, false, false, true);
            }

            WorldGen.gen = false;

        }

        public static void ErasePot(int x, int y) {

            ushort tileType = Main.tile[x, y].type;

            int leftSide = 0;
            int top = y;

            for (leftSide += Main.tile[x, y].frameX / 18; leftSide > 1; leftSide -= 2) { }

            leftSide *= -1;
            leftSide += x;

            int frameY = Main.tile[x, y].frameY / 18;

            while (frameY > 1)
                frameY -= 2;

            top -= frameY;

            for (int m = leftSide; m < leftSide + 2; m++)
                for (int n = top; n < top + 2; n++)
                    if (Main.tile[m, n].type == tileType && Main.tile[m, n].active())
                        WorldGen.KillTile(m, n);

        }

        #endregion Erase Functions

        #region Fill Functions

        private static void StartFloodFill(int x, int y, int tileTypeToReplace, int newTileType) {

            if (Main.tile[x, y].active() && tileTypeToReplace == newTileType)
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();

            _undo = new UndoStep($"Fill - Paint ({CurrentTabName})");

            if (Main.tile[x, y].IsNotActive()) {
                FloodEmptyFill(x, y, -1, newTileType);
            } else {
                FloodFill(x, y, tileTypeToReplace, newTileType);
            }

            if (_undo.Count > 0) {
                _undo.ResetFrames();
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;
            }

        }

        private static void FloodFill(int x, int y, int tileTypeToReplace, int newTileType, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].type != tileTypeToReplace)
                return;

            if (Main.tile[x, y].IsNotActive())
                return;

            _undo.Add(new ChangedTile(x, y));

            if (newTileType == -1) {
                EraseTile(x, y);
            } else {
                PlaceTile(x, y, newTileType, ref _undo, true, true, -1, 0);
            }

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                FloodFill(x + 1, y, tileTypeToReplace, newTileType, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                FloodFill(x, y + 1, tileTypeToReplace, newTileType, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                FloodFill(x - 1, y, tileTypeToReplace, newTileType, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                FloodFill(x, y - 1, tileTypeToReplace, newTileType, 4);

        }

        private static void FloodEmptyFill(int x, int y, int tileTypeToReplace, int newTileType, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].active() && Main.tile[x, y].IsSolid())
                return;

            PlaceTile(x, y, newTileType, ref _undo);

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                FloodEmptyFill(x + 1, y, tileTypeToReplace, newTileType, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                FloodEmptyFill(x, y + 1, tileTypeToReplace, newTileType, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                FloodEmptyFill(x - 1, y, tileTypeToReplace, newTileType, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                FloodEmptyFill(x, y - 1, tileTypeToReplace, newTileType, 4);

        }

        private static void StartConvertFloodFill(int x, int y, int tileTypeToReplace) {

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].IsNotActive())
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();

            _undo = new UndoStep();

            ConvertFloodFill(x, y, tileTypeToReplace);

            if (_undo.Count > 0) {
                _undo.ResetFrames();
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;
            }

        }

        private static void ConvertFloodFill(int x, int y, int tileTypeToReplace, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].IsNotActive())
                return;

            if (Main.tile[x, y].type != tileTypeToReplace)
                return;

            int convertType;

            switch (_otherStyle) {
                case 0: convertType = 1; break;
                case 1: convertType = 4; break;
                case 2: convertType = 2; break;
                case 3: convertType = 5; break;
                case 4: convertType = 3; break;
                default: convertType = 0; break;
            }

            Neo.ConvertTile(x, y, convertType, ref _undo);

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                ConvertFloodFill(x + 1, y, tileTypeToReplace, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                ConvertFloodFill(x, y + 1, tileTypeToReplace, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                ConvertFloodFill(x - 1, y, tileTypeToReplace, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                ConvertFloodFill(x, y - 1, tileTypeToReplace, 4);

        }

        private static void StartOtherFloodFill(int x, int y, Tile toReplace, int newType) {

            if (newType == -1) { // Erase Fill

                if (NeoDraw.OtherToCreate == null)
                    return;

                switch (OthersList[(int)NeoDraw.OtherToCreate].Name) {

                    case OtherNames.Liquid:

                        if (toReplace.liquid < 1)
                            return;

                        StartLiquidFloodFill(x, y, toReplace.liquidType(), 0, 0);

                        break;

                    case OtherNames.Mech:

                        StartWireFloodFill(x, y, Main.tile[x, y].wire() ? 1 : Main.tile[x, y].wire2() ? 2 : Main.tile[x, y].wire3() ? 3 : 4, -1);

                        break;

                    case OtherNames.PaintTile:

                        StartPaintFloodFill(x, y, Main.tile[x, y].type, Main.tile[x, y].color(), 0);

                        break;

                    case OtherNames.PaintWall:

                        StartPaintFloodFill(x, y, Main.tile[x, y].wall, Main.tile[x, y].wallColor(), 0, true);

                        break;

                }

            }
            else {

                switch (OthersList[newType].Name) {

                    case OtherNames.Converter: {

                            StartConvertFloodFill(x, y, toReplace.type);

                            break;

                        }
                    case OtherNames.Liquid: {

                            if (toReplace.liquid < 1)
                                return;

                            if (_otherStyle == 0 && toReplace.liquidType() == 2)
                                return;

                            if (_otherStyle == 1 && toReplace.liquidType() == 1)
                                return;

                            if (_otherStyle == 2 && toReplace.liquidType() == 0)
                                return;

                            StartLiquidFloodFill(x, y, toReplace.liquidType(), _otherStyle == 0 ? 2 : _otherStyle == 1 ? 1 : 0, -1);

                            break;

                        }
                    case OtherNames.Mech: {

                            if (_otherStyle == 0)
                                return;

                            StartWireFloodFill(x, y, Main.tile[x, y].wire() ? 1 : Main.tile[x, y].wire2() ? 2 : Main.tile[x, y].wire3() ? 3 : 4, _otherStyle == 1 ? 2 : _otherStyle == 2 ? 3 : _otherStyle == 3 ? 1 : 4);

                            break;

                        }
                    case OtherNames.PaintTile: {

                            StartPaintFloodFill(x, y, Main.tile[x, y].type, Main.tile[x, y].color(), _otherStyle + 1);

                            break;

                        }
                    case OtherNames.PaintWall: {

                            StartPaintFloodFill(x, y, Main.tile[x, y].wall, Main.tile[x, y].wallColor(), _otherStyle + 1, true);

                            break;

                        }

                }

            }

        }

        private static void StartWallFloodFill(int x, int y, int wallTypeToReplace, int newWallType) {

            if (wallTypeToReplace == newWallType)
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();

            _undo = new UndoStep("Fill - Wall");

            WallFloodFill(x, y, wallTypeToReplace, newWallType);

            if (_undo.Count > 0) {
                _undo.ResetFrames(true);
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;
            }

        }

        private static void WallFloodFill(int x, int y, int wallTypeToReplace, int newWallType, int originDirection = -1) {

            if (x < 2 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 2 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX - 3 || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY - 3 || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type] && !Main.keyState.PressingCtrl())
                return;

            if (Main.tile[x, y].wall != wallTypeToReplace)
                return;

            DrawWall(x, y, ref _undo, newWallType);

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                WallFloodFill(x + 1, y, wallTypeToReplace, newWallType, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                WallFloodFill(x, y + 1, wallTypeToReplace, newWallType, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                WallFloodFill(x - 1, y, wallTypeToReplace, newWallType, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                WallFloodFill(x, y - 1, wallTypeToReplace, newWallType, 4);

        }

        private static void StartLiquidFloodFill(int x, int y, int liquidTypeToReplace, int newLiquidType, int liquidAmount) {

            if (Main.tile[x, y].liquidType() == newLiquidType && liquidAmount != 0)
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();
            _undo = new UndoStep("Fill - Liquid");

            LiquidFloodFill(x, y, liquidTypeToReplace, newLiquidType, liquidAmount);

            if (_undo.Count > 0) {
                _undo.ResetFrames();
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;
            }

        }

        private static void LiquidFloodFill(int x, int y, int liquidTypeToReplace, int newLiquidType, int liquidAmount, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].liquid < 1)
                return;

            if (Main.tile[x, y].liquidType() != liquidTypeToReplace)
                return;

            _undo.Add(new ChangedTile(x, y));

            if (liquidAmount == 0) {
                Main.tile[x, y].liquid = 0;
            } else {
                Main.tile[x, y].liquidType(newLiquidType);
            }

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                LiquidFloodFill(x + 1, y, liquidTypeToReplace, newLiquidType, liquidAmount, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                LiquidFloodFill(x, y + 1, liquidTypeToReplace, newLiquidType, liquidAmount, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                LiquidFloodFill(x - 1, y, liquidTypeToReplace, newLiquidType, liquidAmount, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                LiquidFloodFill(x, y - 1, liquidTypeToReplace, newLiquidType, liquidAmount, 4);

        }

        private static void StartWireFloodFill(int x, int y, int wireTypeToReplace, int newWireType) {

            if (newWireType == 1 && Main.tile[x, y].wire())
                return;

            if (newWireType == 2 && Main.tile[x, y].wire2())
                return;

            if (newWireType == 3 && Main.tile[x, y].wire3())
                return;

            if (newWireType == 4 && Main.tile[x, y].wire4())
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();
            _undo = new UndoStep("Fill - Wire");

            WireFloodFill(x, y, wireTypeToReplace, newWireType);

            if (_undo.Count > 0) {
                _undo.ResetFrames();
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;
            }

        }

        private static void WireFloodFill(int x, int y, int wireTypeToReplace, int newWireType, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (wireTypeToReplace == 1 && !Main.tile[x, y].wire())
                return;

            if (wireTypeToReplace == 2 && !Main.tile[x, y].wire2())
                return;

            if (wireTypeToReplace == 3 && !Main.tile[x, y].wire3())
                return;

            if (wireTypeToReplace == 4 && !Main.tile[x, y].wire4())
                return;

            if (newWireType == 1 && Main.tile[x, y].wire() && !Main.tile[x, y].wire2() && !Main.tile[x, y].wire3() && !Main.tile[x, y].wire4())
                return;

            if (newWireType == 2 && Main.tile[x, y].wire2() && !Main.tile[x, y].wire() && !Main.tile[x, y].wire3() && !Main.tile[x, y].wire4())
                return;

            if (newWireType == 3 && Main.tile[x, y].wire3() && !Main.tile[x, y].wire() && !Main.tile[x, y].wire2() && !Main.tile[x, y].wire4())
                return;

            if (newWireType == 4 && Main.tile[x, y].wire4() && !Main.tile[x, y].wire() && !Main.tile[x, y].wire2() && !Main.tile[x, y].wire3())
                return;

            _undo.Add(new ChangedTile(x, y));

            if (newWireType == -1) {

                if (wireTypeToReplace == 1) {
                    Main.tile[x, y].wire(wire: false);
                }
                else if (wireTypeToReplace == 2) {
                    Main.tile[x, y].wire2(wire2: false);
                }
                else if (wireTypeToReplace == 3) {
                    Main.tile[x, y].wire3(wire3: false);
                }
                else if (wireTypeToReplace == 4) {
                    Main.tile[x, y].wire4(wire4: false);
                }

            }
            else {

                if (wireTypeToReplace == 1) {
                    Main.tile[x, y].wire(wire: false);
                }
                else if (wireTypeToReplace == 2) {
                    Main.tile[x, y].wire2(wire2: false);
                }
                else if (wireTypeToReplace == 3) {
                    Main.tile[x, y].wire3(wire3: false);
                }
                else if (wireTypeToReplace == 4) {
                    Main.tile[x, y].wire4(wire4: false);
                }

                if (newWireType == 1) {
                    Main.tile[x, y].wire(wire: true);
                }
                else if (newWireType == 2) {
                    Main.tile[x, y].wire2(wire2: true);
                }
                else if (newWireType == 3) {
                    Main.tile[x, y].wire3(wire3: true);
                }
                else if (newWireType == 4) {
                    Main.tile[x, y].wire4(wire4: true);
                }

            }

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                WireFloodFill(x + 1, y, wireTypeToReplace, newWireType, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                WireFloodFill(x, y + 1, wireTypeToReplace, newWireType, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                WireFloodFill(x - 1, y, wireTypeToReplace, newWireType, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                WireFloodFill(x, y - 1, wireTypeToReplace, newWireType, 4);

        }

        private static List<Point> StartFloodFillSelect() {

            List<Point> points = new List<Point>();

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();

            if (CurrentTab == Tabs.Tiles || (CurrentTab == Tabs.Structures && Neo.TileTarget_Tile.active())) {

                if (Neo.TileTarget_Tile.IsNotActive())
                    return points;

                FloodFillSelectTile(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.type, ref points);

            }
            else if (CurrentTab == Tabs.Walls || (CurrentTab == Tabs.Structures && Neo.TileTarget_Tile.wall != 0)) {

                if (Neo.TileTarget_Tile.wall == 0)
                    return points;

                FloodFillSelectWall(Neo.TileTargetX, Neo.TileTargetY, Neo.TileTarget_Tile.wall, ref points);

            }

            return points;

        }

        private static void FloodFillSelectTile(int x, int y, int type, ref List<Point> points, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y].IsNotActive())
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].type != type)
                return;

            if (points.Contains(new Point(x, y)))
                return;

            points.Add(new Point(x, y));

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                FloodFillSelectTile(x + 1, y, type, ref points, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                FloodFillSelectTile(x, y + 1, type, ref points, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                FloodFillSelectTile(x - 1, y, type, ref points, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                FloodFillSelectTile(x, y - 1, type, ref points, 4);

        }

        private static void FloodFillSelectWall(int x, int y, int wallType, ref List<Point> points, int originDirection = -1) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y].wall != wallType)
                return;

            if (points.Contains(new Point(x, y)))
                return;

            points.Add(new Point(x, y));

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                FloodFillSelectWall(x + 1, y, wallType, ref points, 1);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                FloodFillSelectWall(x, y + 1, wallType, ref points, 2);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                FloodFillSelectWall(x - 1, y, wallType, ref points, 3);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                FloodFillSelectWall(x, y - 1, wallType, ref points, 4);

        }

        private static void StartPaintFloodFill(int x, int y, int typeToPaint, int paintColorToReplace, int newPaintColor, bool applyToWalls = false) {

            if (paintColorToReplace == newPaintColor)
                return;

            if ((!applyToWalls && Main.tile[x, y].IsNotActive()) || (applyToWalls && (Main.tile[x, y].wall == 0 || (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type]))))
                return;

            _floodFillCounter = 0;

            _onScreenOnly = !Main.keyState.PressingAlt();

            _undo = new UndoStep($"Fill Paint ({CurrentTabName})");

            PaintFloodFill(x, y, typeToPaint, paintColorToReplace, newPaintColor, -1, applyToWalls);

            if (_undo.Count > 0) {

                _undo.ResetFrames(true);
                NeoDraw.UndoManager.UndoPush(_undo);
                _undo = null;

            }

        }

        private static void PaintFloodFill(int x, int y, int typeToPaint, int paintColorToReplace, int newPaintColor, int originDirection = -1, bool applyToWalls = false) {

            if (x < 0 || (_onScreenOnly && x < Main.screenPosition.X / 16f))
                return;

            if (y < 0 || (_onScreenOnly && y < Main.screenPosition.Y / 16f))
                return;

            if (x > Main.maxTilesX || (_onScreenOnly && x > (Main.screenPosition.X + Main.screenWidth) / 16f))
                return;

            if (y > Main.maxTilesY || (_onScreenOnly && y > (Main.screenPosition.Y + Main.screenHeight) / 16f))
                return;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (applyToWalls) {

                if (Main.tile[x, y].wall == 0 || (Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type]))
                    return;

                if (Main.tile[x, y].wall != typeToPaint)
                    return;

                if (Main.tile[x, y].wallColor() != paintColorToReplace)
                    return;

                _undo.Add(new ChangedTile(x, y));

                WorldGen.paintWall(x, y, (byte)newPaintColor, true);

            }
            else {

                if (Main.tile[x, y].IsNotActive())
                    return;

                if (Main.tile[x, y].type != typeToPaint)
                    return;

                if (Main.tile[x, y].color() != paintColorToReplace)
                    return;

                _undo.Add(new ChangedTile(x, y));

                WorldGen.paintTile(x, y, (byte)newPaintColor, true);

            }

            _floodFillCounter++;

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 3)
                PaintFloodFill(x + 1, y, typeToPaint, paintColorToReplace, newPaintColor, 1, applyToWalls);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 4)
                PaintFloodFill(x, y + 1, typeToPaint, paintColorToReplace, newPaintColor, 2, applyToWalls);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 1)
                PaintFloodFill(x - 1, y, typeToPaint, paintColorToReplace, newPaintColor, 3, applyToWalls);

            if (_floodFillCounter > MaxFloodFill)
                return;

            if (originDirection != 2)
                PaintFloodFill(x, y - 1, typeToPaint, paintColorToReplace, newPaintColor, 4, applyToWalls);

        }

        #endregion Fill Functions

        #region Helper Functions

        public static void AddChangedTile(int x, int y) {

            if (_undo == null)
                _undo = new UndoStep();

            _undo.Add(new ChangedTile(x, y));

        }

        public static void AddClickDelay(int time) {

            DateTime newClickDelay = DateTime.Now.AddMilliseconds(time);

            if (newClickDelay > _clickDelay)
                _clickDelay = newClickDelay;

        }

        public static void AddKeyPressDelay(int time = KeyDelay) {
            KeyPressDelay = DateTime.Now.AddMilliseconds(time);
        }

        private static IEnumerable<Point> BresenhamLine(int x0, int y0, int x1, int y1) {

            List<Point> result = new List<Point>();

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep) {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1) {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;

            if (y0 < y1) {
                ystep = 1;
            } else {
                ystep = -1;
            }

            for (int x = x0; x <= x1; x++) {

                result.Add(steep ? new Point(y, x) : new Point(x, y));
                error += deltay;

                if (2 * error >= deltax) {
                    y += ystep;
                    error -= deltax;
                }

            }

            return result;

        }

        private static IEnumerable<Point> BresenhamLineSuperCover(int x0, int y0, int x1, int y1) {

            List<Point> result = new List<Point>();

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep) {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1) {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int errorPrev = 0;
            int ystep;
            int y = y0;

            if (y0 < y1) {
                ystep = 1;
            } else {
                ystep = -1;
            }

            for (int x = x0; x <= x1; x++) {

                result.Add(steep ? new Point(y, x) : new Point(x, y));
                error += deltay;

                if (2 * error >= deltax) {

                    y += ystep;
                    error -= deltax;

                    if (error + errorPrev < deltax)
                        result.Add(steep ? new Point(y, x) : new Point(x, y));

                }

                errorPrev = error;

            }

            return result;

        }

        private static void CheckPressedKeys() {

            if ((RightClicked && !Main.keyState.PressingAlt()) || (Keys.Escape.Pressed() && DateTime.Now > KeyPressDelay)) {

                if (CurrentPaintMode == PaintMode.Select) {

                    if (StartPoint == default) {

                        if (SelectedPoints.Count > 0) {

                            SelectedPoints.Clear();
                            SelectedPointsHash.Clear();

                        }
                        else {

                            ClearSelectedListItem();

                        }

                    }
                    else {

                        StartPoint = default;

                    }

                }
                else if (CurrentPaintMode == PaintMode.MagicWand) {

                    if (SelectedPoints.Count > 0) {

                        SelectedPoints.Clear();
                        SelectedPointsHash.Clear();

                    }
                    else {

                        ClearSelectedListItem();

                    }

                }
                else if(CurrentBrushShape == BrushShape.Line) {

                    if (StartPoint == default) {

                        ClearSelectedListItem();

                    }
                    else {

                        StartPoint = default;

                    }

                }
                else { // Right Clicking Unselects current tile/special/other

                    ClearSelectedListItem();

                    StartPoint = default;

                }

                if (Main.keyState.GetPressedKeys().Contains(Keys.Escape))
                    KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelay);

            }

            if (DateTime.Now <= KeyPressDelay)
                return;

            //if (Keys.OemPlus.Pressed()) {
            //
            //    BrushSize += 1;
            //    KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelayQuick);
            //
            //}
            //else if (Keys.OemMinus.Pressed()) {
            //
            //    BrushSize -= 1;
            //    KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelayQuick);
            //
            //}

            if (SelectedPoints != null && SelectedPoints.Count > 0 && Main.keyState.GetPressedKeys().Contains(Keys.Delete)) {

                EraseSelection();
                KeyPressDelay = DateTime.Now.AddMilliseconds(KeyDelay);

            }

        }

        private static void ClearSelectedListItem() {

            switch (CurrentTab) {

                case Tabs.Tiles: {

                        NeoDraw.TileToCreate = null;
                        _currentSubTile = -1;
                        SubTileNames.Clear();
                        break;

                    }
                case Tabs.Walls: {

                        NeoDraw.WallToCreate = null;
                        break;

                    }
                case Tabs.Structures: {

                        NeoDraw.StructureToCreate = null;
                        _currentSubStructure = -1;
                        SubStructureNames.Clear();
                        break;

                    }
                case Tabs.Other: {

                        NeoDraw.OtherToCreate = null;
                        _currentSubOther = -1;
                        SubOtherNames.Clear();
                        break;

                    }

            }

            Pasting = false;

        }

        public static void CopySelection(bool clearSelection = false) {

            CurrentSelection = new UndoStep();

            if (SelectedPoints == null || SelectedPoints.Count == 0)
                return;

            int minX = SelectedPoints[0].X;
            int maxX = SelectedPoints[0].X;
            int minY = SelectedPoints[0].Y;
            int maxY = SelectedPoints[0].Y;

            for (int i = 1; i < SelectedPoints.Count; i++) {

                if (SelectedPoints[i].X < minX)
                    minX = SelectedPoints[i].X;

                if (SelectedPoints[i].X > maxX)
                    maxX = SelectedPoints[i].X;

                if (SelectedPoints[i].Y < minY)
                    minY = SelectedPoints[i].Y;

                if (SelectedPoints[i].Y > maxY)
                    maxY = SelectedPoints[i].Y;

            }

            for (int i = 0; i < SelectedPoints.Count; i++)
                CurrentSelection.Add(new ChangedTile(SelectedPoints[i].X - minX, SelectedPoints[i].Y - minY, new Tile(Main.tile[SelectedPoints[i].X, SelectedPoints[i].Y])));

            if (clearSelection)
                EraseSelection();

            SetStatusBarTempMessage(clearSelection ? "Cut" : "Copy", 50);

        }

        private static void CreateButtons() {

            TopButtons.Clear();

            int buttonCount = 0;
            int xCount = 0;
            int yCount = 0;

            #region Paint
            TopButtons.Add(new TopButton(

                name: "Paint",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_1071"),
                textureSize: new Vector2(26, 26),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentPaintMode == PaintMode.Paint,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentPaintMode = PaintMode.Paint;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Paint"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Erase
            TopButtons.Add(new TopButton(

                name: "Erase",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_166"),
                textureSize: new Vector2(22, 30),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentPaintMode == PaintMode.Erase,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    if (CurrentTab == Tabs.Structures) // Erase has no use while in the Structures Tab
                        return;

                    if (CurrentTab == Tabs.Other && (OtherToCreateName == OtherNames.Converter || OtherToCreateName == OtherNames.ResetFrames || OtherToCreateName == OtherNames.RoomCheck || OtherToCreateName == OtherNames.Spawn)) // Erase has no use while doing Room Check
                        return;
                    
                    if (CurrentTab != Tabs.Structures)
                        CurrentPaintMode = PaintMode.Erase;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Erase"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Select
            TopButtons.Add(new TopButton(

                name: "Select",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(40, 40),
                textureFrame: new Vector2(42 * 8, 0),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentPaintMode == PaintMode.Select,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentPaintMode = PaintMode.Select;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Select",
                frameCount: 5,
                frameDelay: 8

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Magic Wand
            TopButtons.Add(new TopButton(

                name: "Magic Wand",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_113"),
                textureSize: new Vector2(26, 26),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentPaintMode == PaintMode.MagicWand,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentPaintMode = PaintMode.MagicWand;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Magic Wand"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Eye Dropper
            TopButtons.Add(new TopButton(

                name: "Eye Dropper",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_3186"),
                textureSize: new Vector2(32, 32),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentPaintMode == PaintMode.Eyedropper,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentPaintMode = PaintMode.Eyedropper;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Eye Dropper"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Wire
            TopButtons.Add(new TopButton(

                name: "Wire",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_3625"),
                textureSize: new Vector2(40, 40),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => Main.LocalPlayer.builderAccStatus[8] == 0,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    // -1: Off, 0: Full Bright, 1: Dim, 2: Dimmer
                    // [2] - Auto Actuator
                    // [3] - Auto Paint
                    // [4] - Red
                    // [5] - Blue
                    // [6] - Green
                    // [7] - Yellow
                    // [8] - View Wire (0: On, 1: Off)
                    // [9] - Actuator?
                    // [11] - Biome Torches

                    WireStatus++;

                    Main.LocalPlayer.builderAccStatus[4] = WireStatus;
                    Main.LocalPlayer.builderAccStatus[5] = WireStatus;
                    Main.LocalPlayer.builderAccStatus[6] = WireStatus;
                    Main.LocalPlayer.builderAccStatus[7] = WireStatus;
                    Main.LocalPlayer.builderAccStatus[8] = 0;
                    Main.LocalPlayer.builderAccStatus[9] = WireStatus;

                    if (WireStatus > 2) {
                        Main.LocalPlayer.builderAccStatus[8] = 1;
                        WireStatus = -1;
                    }

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = (WireStatus == -1 ? "Show" : (WireStatus == 0 ? "Dim" : (WireStatus == 1 ? "Dimmer" : "Hide"))) + " Wire"

            ));
            #endregion

            buttonCount++;
            xCount = 0;
            yCount++;

            #region Square Brush
            TopButtons.Add(new TopButton(

                name: "Square Brush",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(28, 28),
                textureFrame: new Vector2(6, 6),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentBrushShape == BrushShape.Square,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentBrushShape = BrushShape.Square;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Square Brush"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Circle Brush
            TopButtons.Add(new TopButton(

                name: "Circle Brush",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(28, 28),
                textureFrame: new Vector2(48, 6),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentBrushShape == BrushShape.Circle,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentBrushShape = BrushShape.Circle;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Circle Brush"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Line
            TopButtons.Add(new TopButton(

                name: "Line",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(36, 16),
                textureFrame: new Vector2(254, 13),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentBrushShape == BrushShape.Line,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentBrushShape = BrushShape.Line;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Line"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Fill
            TopButtons.Add(new TopButton(

                name: "Fill",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_206"),
                textureSize: new Vector2(24, 22),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => CurrentBrushShape == BrushShape.Fill,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CurrentBrushShape = BrushShape.Fill;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Fill"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Bright
            TopButtons.Add(new TopButton(

                name: "Bright",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_538"),
                textureSize: new Vector2(20, 20),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => NeoDraw.BrightMode > 0,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    NeoDraw.BrightMode++;

                    if (NeoDraw.BrightMode > 2)
                        NeoDraw.BrightMode = 0;

                    if (NeoDraw.BrightMode > 0) {
                        t.Effects = SpriteEffects.None;
                    }
                    else {
                        t.Effects = SpriteEffects.FlipVertically;
                    }

                },
                getAlpha: () => BoxAlpha,
                onHover: t => {

                    _hoverText = "Bright" + (NeoDraw.BrightMode > 0 ? " Off" : " On");

                    float scrollBy = PlayerInput.ScrollWheelDelta / 120;

                    Brightness += (scrollBy / 100);

                    Brightness = MathHelper.Clamp(Brightness, 0.2f, 1f);

                },
                spriteEffects: SpriteEffects.FlipVertically

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Day / Night
            TopButtons.Add(new TopButton(

                name: "Day / Night",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(40, 40),
                textureFrame: new Vector2(42 * 17, 0),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => DayNightOption != 0,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    if (DayNightOption == 0) {
                        Main.time = 27000;
                        Main.dayTime = true;
                        DayNightOption++;
                    }
                    else if (DayNightOption == 1) {
                        Main.time = 16200;
                        Main.dayTime = false;
                        DayNightOption++;
                    }
                    else {
                        Main.time = NeoDraw.OldTime;
                        Main.dayTime = NeoDraw.OldDayTime;
                        DayNightOption = 0;
                    }

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = DayNightOption == 1 ? "Make Night Time" : DayNightOption == 2 ? "Reset Time" : "Make Day Time"

            ));
            #endregion

            buttonCount++;
            xCount = 0;
            yCount++;

            #region Cut
            TopButtons.Add(new TopButton(

                name: "Cut",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(34, 29),
                textureFrame: new Vector2(171, 5),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CopySelection(true);

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Cut"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Copy
            TopButtons.Add(new TopButton(

                name: "Copy",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(30, 30),
                textureFrame: new Vector2(215, 5),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    CopySelection();

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Copy"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Paste
            TopButtons.Add(new TopButton(

                name: "Paste",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(30, 30),
                textureFrame: new Vector2(425, 5),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    PasteSelection();

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Paste"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Clear
            TopButtons.Add(new TopButton(

                name: "Clear",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: UIButtonTextures,
                textureSize: new Vector2(40, 40),
                textureFrame: new Vector2(42 * 11, 0),
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    EraseSelection();

                    SetStatusBarTempMessage("Clear", 50);

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Clear"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region GPS
            TopButtons.Add(new TopButton(

                name: "GPS",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_395"),
                textureSize: new Vector2(30, 28),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => _showGPS,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    _showGPS = !_showGPS;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = (_showGPS ? "Hide" : "Show") + " GPS"

            ));
            #endregion

            buttonCount++;
            xCount++;

            #region Grid
            TopButtons.Add(new TopButton(

                name: "Grid",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(PaddingSide + (PaddingSide + ButtonWidth) * xCount, BoxPadding + (BoxPadding + ButtonHeight) * yCount),
                texture: GetTexture("Terraria/Item_486"),
                textureSize: new Vector2(12, 28),
                textureFrame: default,
                bgActiveTexture: ButtonActive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => GridStyle > 0,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    GridStyle++;

                    if (GridStyle > 3)
                        GridStyle = 0;

                },
                getAlpha: () => BoxAlpha,
                onHover: t => _hoverText = "Toggle Grid Style"

            ));
            #endregion

            buttonCount++;

            #region Undo
            TopButtons.Add(new TopButton(

                name: "Undo",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(ListWidth + PaddingSide, Main.screenHeight - ButtonHeight - BoxPadding - (ShowStatusBar ? StatusBarHeight : 0)),
                texture: UIButtonTextures,
                textureSize: new Vector2(40, 40),
                textureFrame: new Vector2(42 * buttonCount, 0),
                bgActiveTexture: ButtonInactive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    if (NeoDraw.UndoManager.HistoryCount > 0)
                        NeoDraw.UndoManager.Undo();

                },
                getAlpha: () => NeoDraw.UndoManager.HistoryCount > 0 ? ActiveAlpha : InactiveAlpha,
                onHover: t => { Main.LocalPlayer.mouseInterface = true; if (NeoDraw.UndoManager.HistoryCount > 0) _hoverText = "Undo"; },
                frameCount: 1,
                frameDelay: 4,
                preDrawBackground: (t, tex) => {

                    if (NeoDraw.UndoManager.HistoryCount < 1)
                        return t.BGInactiveTexture;

                    return tex;

                }

            ));
            #endregion

            buttonCount++;

            #region Redo
            TopButtons.Add(new TopButton(

                name: "Redo",
                size: new Vector2(ButtonWidth, ButtonHeight),
                position: new Vector2(ListWidth + PaddingSide * 2 + ButtonWidth, Main.screenHeight - ButtonHeight - BoxPadding - (ShowStatusBar ? StatusBarHeight : 0)),
                texture: UIButtonTextures,
                textureSize: new Vector2(40, 40),
                textureFrame: new Vector2(42 * buttonCount, 0),
                bgActiveTexture: ButtonInactive,
                bgInactiveTexture: ButtonInactive,
                bgHoveredTexture: ButtonHover,
                isActive: () => false,
                onClick: t => {

                    if (_buttonClicked)
                        return;

                    _buttonClicked = true;

                    if (NeoDraw.UndoManager.RedoCount > 0)
                        NeoDraw.UndoManager.Redo();

                },
                getAlpha: () => NeoDraw.UndoManager.RedoCount > 0 ? ActiveAlpha : InactiveAlpha,
                onHover: t => { Main.LocalPlayer.mouseInterface = true; if (NeoDraw.UndoManager.RedoCount > 0) _hoverText = "Redo"; },
                frameCount: 1,
                frameDelay: 4,
                preDrawBackground: (t, tex) => {

                    if (NeoDraw.UndoManager.RedoCount < 1)
                        return t.BGInactiveTexture;

                    return tex;

                }

            ));
            #endregion

        }

        private static void CreateSearchButton() {

            _search = new FancyButton(

                clicked: (b, mb) => {

                    if (SearchBoxText == null && SearchString != null) {

                        Searching = false;

                        SearchString = null;

                        Refresh(true);

                    }
                    else {

                        Main.GetInputText("");

                        if (SearchBoxText == null) {

                            Searching = true;

                            SearchBoxText = "";

                        }
                        else {

                            Searching = false;

                            SearchString = SearchBoxText;

                            if (SearchString == "")
                                SearchString = null;

                            SearchBoxText = null;

                        }

                    }

                },

                drawCode: (b, sb, mb) => {

                    Texture2D tex = (SearchBoxText == null && SearchString != null) ? Main.cdTexture : Main.confuseTexture;
                    float tscale = 1f;

                    if (tex.Width * tscale > b.Size.X - 4)
                        tscale = (b.Size.X - 4) / (tex.Width * tscale);

                    if (tex.Height * tscale > b.Size.Y - 4)
                        tscale = (b.Size.Y - 4) / (tex.Height * tscale);

                    sb.Draw(tex, b.Position + b.Offset + b.Size / 2, null, Color.White * BoxAlpha, 0f, tex.Size() / 2, tscale, SpriteEffects.None, 0f);

                }

            ) {

                Position = new Vector2(ListWidth - 32, SearchTop),
                Size = new Vector2(32, SearchBarHeight)

            };

            _searchBar = new FancyButton(

                clicked: (b, mb) => {

                    Main.GetInputText("");

                    if (SearchBoxText == null) {

                        Searching = true;

                        SearchBoxText = "";

                    }
                    else {

                        Searching = false;

                        SearchString = SearchBoxText;

                        if (SearchString == "")
                            SearchString = null;

                        SearchBoxText = null;

                    }

                },

                drawCode: (b, sb, mb) => {

                    if (SearchBoxText == null && SearchString == null) {
                        Drawing.StringShadowed(sb, Main.fontMouseText, "Search...", new Vector2(b.Position.X + b.Offset.X + 8, b.Position.Y + b.Offset.Y + 6), (Color.White * .5f) * BoxAlpha);
                    }
                    else {
                        Drawing.StringShadowed(sb, Main.fontMouseText, SearchBoxText == null ? SearchString : SearchBoxText + _searchAppend, new Vector2(b.Position.X + b.Offset.X + 8, b.Position.Y + b.Offset.Y + 6), Color.White * BoxAlpha);
                    }

                }

            ) {

                Position = new Vector2(0, SearchTop),
                Size = new Vector2(ListWidth - 32, SearchBarHeight)

            };

        }

        private static void CreateSliders() {

            _middleVerticalSlider = new VerticalSlider(

                new Vector2(0f, 0f),
                new Vector2(ListWidth - 20, MiddleListTop + 2)
                )
                .SetSize(20f, MiddleListLength - 4)
                .With(delegate (VerticalSlider w) {

                    w.Update = delegate {

                        if (AtLeftEdgeOfWorld) {
                            w.offset = new Vector2(ListWidth - 20 + ToolbarXoffset, MiddleListTop + 2 + ToolbarYoffset);
                        }
                        else {
                            w.offset = new Vector2(ListWidth - 20, MiddleListTop + 2 + ToolbarYoffset);
                        }

                        w.canMouseOver = !_subScrolling;

                        w.multislide = MiddleListItemsCount + 1;

                        switch (CurrentTab) {

                            case Tabs.Tiles:
                                w.slider = TileScroll;
                                w.segments = TileNames.TileCount + 1;
                                break;

                            case Tabs.Walls:
                                w.slider = WallScroll;
                                w.segments = WallID.Count + 1;
                                break;

                            case Tabs.Structures:
                                w.slider = StructureScroll;
                                w.segments = StructuresList.Count + 1;
                                break;

                            case Tabs.Other:
                                w.slider = OtherScroll;
                                w.segments = OthersList.Count + 1;
                                break;

                        }

                        if ((w.segments < (int)Math.Floor(MiddleListLength / (float)ListItemHeight) || !MouseXoverToolbar) && w.framesHeld == 0) {

                            w.disabled = true;

                            return;

                        }

                        w.disabled = false;

                    };

                    w.Click = delegate {

                        int hover = w.slider_hover;

                        if (hover == -1 && Math.Abs(Main.mouseX - w.position.X) < 100) {

                            float num = 0f;
                            float num2 = w.size.Y - 8f;
                            float num3 = num2 / w.segments;

                            for (int i = 0; i < w.segments; i++) {

                                if (Main.mouseY >= num + w.position.Y + 4f && Main.mouseY < num + w.position.Y + 4f + num3) {
                                    hover = i;
                                    break;
                                }

                                num += num3;

                            }

                            if (hover == -1 && Main.mouseY < w.position.Y + 4) {
                                hover = 0;
                            }
                            else if (hover == -1 && Main.mouseY > num + w.position.Y + 4) {
                                hover = w.segments - 1;
                            }

                        }

                        if (hover == -1)
                            return;

                        switch (CurrentTab) {

                            case Tabs.Tiles:
                                TileScroll = (int)MathHelper.Clamp(hover, 0, TileScrollMax);
                                break;

                            case Tabs.Walls:
                                WallScroll = (int)MathHelper.Clamp(hover, 0, WallScrollMax);
                                break;

                            case Tabs.Structures:
                                StructureScroll = (int)MathHelper.Clamp(hover, 0, StructureScrollMax);
                                break;

                            case Tabs.Other:
                                OtherScroll = (int)MathHelper.Clamp(hover, 0, OtherScrollMax);
                                break;

                        }

                    };

                    w.ClickHold = () => w.Click();

                });

            _bottomVerticalSlider = new VerticalSlider(

                anchor: new Vector2(0f, 0f),
                offset: new Vector2(ListWidth - 20, MiddleListBottom + 2)
                )
                .SetSize(20f, SubListLength - 4)
                .With(delegate (VerticalSlider w) {

                    w.Update = delegate {

                        if (AtLeftEdgeOfWorld) {
                            w.offset = new Vector2(ListWidth - 20 + ToolbarXoffset, MiddleListBottom + 2);
                        }
                        else {
                            w.offset = new Vector2(ListWidth - 20, MiddleListBottom + 2);
                        }

                        w.canMouseOver = !_scrolling;

                        w.multislide = SubListItemsLength + 1;

                        w.slider = SubScroll;

                        if (CurrentTab == Tabs.Tiles) {
                            w.segments = SubTileNames.Count + 1;
                        }
                        else if (CurrentTab == Tabs.Structures) {
                            w.segments = SubStructureNames.Count + 1;
                        }
                        else if (CurrentTab == Tabs.Other) {
                            w.segments = SubOtherNames.Count + 1;
                        }
                        else {
                            w.segments = 0;
                        }

                        if ((w.segments < (int)Math.Floor(SubListLength / (float)ListItemHeight) || !MouseXoverToolbar) && w.framesHeld == 0) {

                            w.disabled = true;

                            return;

                        }

                        w.disabled = false;

                    };

                    w.Click = delegate {

                        int hover = w.slider_hover;

                        if (hover == -1 && Math.Abs(Main.mouseX - w.position.X) < 100) {

                            float num = 0f;
                            float num2 = w.size.Y - 8f;
                            float num3 = num2 / w.segments;

                            for (int i = 0; i < w.segments; i++) {

                                if (Main.mouseY >= num + w.position.Y + 4f && Main.mouseY < num + w.position.Y + 4f + num3) {
                                    hover = i;
                                    break;
                                }

                                num += num3;

                            }

                            if (hover == -1 && Main.mouseY < w.position.Y + 4) {
                                hover = 0;
                            }
                            else if (hover == -1 && Main.mouseY > num + w.position.Y + 4) {
                                hover = w.segments - 1;
                            }

                        }

                        if (hover == -1)
                            return;

                        SubScroll = (int)MathHelper.Clamp(hover, 0, SubScrollMax);

                    };

                    w.ClickHold = () => w.Click();

                });

        }

        private static void CreateStructuresList() {

            int index = 0;

            StructuresList = new List<ListItem> {

                new ListItem(index++, StructureNames.DeadMansChest), // Booby Trap Chest
                new ListItem(index++, StructureNames.CampSite),
                new ListItem(index++, StructureNames.CaveOpenater),
                new ListItem(index++, StructureNames.Caverer),
                new ListItem(index++, StructureNames.Cavinator),
                new ListItem(index++, StructureNames.CloudIsland),
                new ListItem(index++, StructureNames.CloudIslandHouse),
                new ListItem(index++, StructureNames.CloudLake),
                //new ListItem(index++, StructureNames.CorruptionPit),
                new ListItem(index++, StructureNames.CorruptionStart),
                //new ListItem(index++, StructureNames.CrimsonEntrance),
                new ListItem(index++, StructureNames.CrimsonStart),
                new ListItem(index++, StructureNames.Desert),
                new ListItem(index++, StructureNames.Dunes),
                new ListItem(index++, StructureNames.Dungeon),
                new ListItem(index++, StructureNames.EnchantedSword),
                new ListItem(index++, StructureNames.EpicTree),
                //new ListItem(index++, StructureNames.FloatingIsland),
                new ListItem(index++, StructureNames.GemCave),
                new ListItem(index++, StructureNames.GraniteCave),
                new ListItem(index++, StructureNames.GraniteCavern),
                new ListItem(index++, StructureNames.HellFort),
                new ListItem(index++, StructureNames.Hive),
                new ListItem(index++, StructureNames.HoneyPatch),
                new ListItem(index++, StructureNames.JungleShrine),
                new ListItem(index++, StructureNames.Lakinater),
                new ListItem(index++, StructureNames.LavaTrap),
                new ListItem(index++, StructureNames.LihzahrdTemple),
                new ListItem(index++, StructureNames.LivingMahoganyTree),
                new ListItem(index++, StructureNames.LivingTree),
                new ListItem(index++, StructureNames.MahoganyTree),
                new ListItem(index++, StructureNames.MakeHole),
                new ListItem(index++, StructureNames.MarbleCave),
                new ListItem(index++, StructureNames.MarbleCavern),
                new ListItem(index++, StructureNames.Meteor),
                new ListItem(index++, StructureNames.MiningExplosives),
                new ListItem(index++, StructureNames.MossCave),
                new ListItem(index++, StructureNames.Mountinater),
                new ListItem(index++, StructureNames.Oasis),
                new ListItem(index++, StructureNames.OceanCave),
                new ListItem(index++, StructureNames.Pyramid),
                new ListItem(index++, StructureNames.SandTrap),
                new ListItem(index++, StructureNames.ShroomPatch),
                new ListItem(index++, StructureNames.SpiderCave),
                new ListItem(index++, StructureNames.StonePatch),
                new ListItem(index++, StructureNames.UndergroundHouse),
                new ListItem(index++, StructureNames.WateryIceThing)

            };

        }

        private static void DrawCactusOutline(SpriteBatch sb) {

            int height = 8;
            int startX = (int)Neo.TileTarget_Vector.X;
            int startY = (int)Neo.TileTarget_Vector.Y;

            while (startY < Main.maxTilesY && (!Main.tile[startX, startY].active() || (Main.tile[startX, startY].active() && Main.tileCut[Main.tile[startX, startY].type])))
                startY++;

            Vector2 topLeft = new Vector2((startX - 1) * 16 - Main.screenPosition.X, (startY - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 topRight = new Vector2((startX + 2) * 16 - Main.screenPosition.X, (startY - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 botomLeft = new Vector2((startX - 1) * 16 - Main.screenPosition.X, (startY) * 16 - Main.screenPosition.Y);
            Vector2 botomRight = new Vector2((startX + 2) * 16 - Main.screenPosition.X, (startY) * 16 - Main.screenPosition.Y);

            bool canPlace = true;
            string mouseText = "";

            if (!Main.tile[startX, startY].nactive()) {
                canPlace = false;
                mouseText = "Start Tile is nActive.";
            }
            else if ((Main.tile[startX, startY - 1].active() && !Main.tileCut[Main.tile[startX, startY - 1].type]) ||
                     (Main.tile[startX - 1, startY - 1].active() && !Main.tileCut[Main.tile[startX - 1, startY - 1].type]) ||
                     (Main.tile[startX + 1, startY - 1].active() && !Main.tileCut[Main.tile[startX + 1, startY - 1].type])) {
                canPlace = false;
                mouseText = "Ground is not wide enough.";
            }
            else if (Main.tile[startX, startY].halfBrick() ||
                     Main.tile[startX, startY].slope() != 0) {
                canPlace = false;
                mouseText = "Ground is not level.";
            }
            else if (Main.tile[startX, startY].type != TileID.Sand &&
                     Main.tile[startX, startY].type != TileID.Ebonsand &&
                     Main.tile[startX, startY].type != TileID.Pearlsand &&
                     Main.tile[startX, startY].type != TileID.Crimsand &&
                     !TileLoader.CanGrowModPalmTree(Main.tile[startX, startY].type)) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Cactus.";
            }
            else if (!EmptyTileCheck(startX - 1, startX + 1, startY - 21, startY - 1, 80, true)) {
                canPlace = false;
                mouseText = "Empty Tile Check Failed.";
            }

            Color boxColor = canPlace ? Color.Green : Color.Red;

            sb.DrawLine(topLeft, topRight, boxColor, 2);
            sb.DrawLine(topLeft, botomLeft, boxColor, 2);
            sb.DrawLine(botomLeft, botomRight, boxColor, 2);
            sb.DrawLine(topRight, botomRight, boxColor, 2);

            if (mouseText != "")
                MouseText(sb, mouseText);

        }

        private static void DrawEpicTreeOutline(SpriteBatch sb) {

            int width = 4;
            int height = 30;

            Point origin = new Point(Neo.TileTargetX, Neo.TileTargetY);

            while (origin.Y < Main.maxTilesY && (!Main.tile[origin.X, origin.Y].active() || (Main.tile[origin.X, origin.Y].active() && Main.tileCut[Main.tile[origin.X, origin.Y].type])))
                origin.Y++;

            Vector2 topLeft = new Vector2((origin.X - (width / 2)) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 topRight = new Vector2((origin.X + (width / 2) + 1) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 botomLeft = new Vector2((origin.X - (width / 2)) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);
            Vector2 botomRight = new Vector2((origin.X + (width / 2) + 1) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);

            bool canPlace = true;
            string mouseText = "";

            if (!Main.tile[origin.X, origin.Y].nactive()) {
                canPlace = false;
                mouseText = "Start Tile is nActive.";
            }
            else if (!Main.tile[origin.X - 1, origin.Y].active() &&
                     !Main.tile[origin.X + 1, origin.Y].active()) {
                canPlace = false;
                mouseText = "Ground is not wide enough.";
            }
            else if (Main.tile[origin.X, origin.Y].halfBrick() ||
                     Main.tile[origin.X, origin.Y].slope() != 0) {
                canPlace = false;
                mouseText = "Ground is not level.";
            }
            else if (Main.tile[origin.X, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X, origin.Y].type)) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (
                     (Main.tile[origin.X - 1, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X - 1, origin.Y].type))
                     &&
                     (Main.tile[origin.X + 1, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X + 1, origin.Y].type))) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (!EmptyTileCheck(origin.X - 2, origin.X + 2, origin.Y - height, origin.Y - 1, 20, true)) {
                canPlace = false;
                mouseText = "Empty Tile Check Failed.";
            }

            Color boxColor = canPlace ? Color.Green : Color.Red;

            sb.DrawLine(topLeft, topRight, boxColor, 2);
            sb.DrawLine(topLeft, botomLeft, boxColor, 2);
            sb.DrawLine(botomLeft, botomRight, boxColor, 2);
            sb.DrawLine(topRight, botomRight, boxColor, 2);

            if (mouseText != "")
                MouseText(sb, mouseText);

        }

        private static void DrawLaserRuler(SpriteBatch sb) {

            PlayerInput.SetZoom_MouseInWorld();

            float num = Vector2.Distance(Main.player[Main.myPlayer].position, Main.player[Main.myPlayer].shadowPos[2]);

            float num2 = 6f;

            Texture2D value = GetTexture("Terraria/Extra_68");

            float scale = MathHelper.Lerp(0.2f, 0.7f, MathHelper.Clamp(1f - num / num2, 0f, 1f));
            Vector2 vec = Main.screenPosition;

            vec += new Vector2(-50f);
            vec = vec.ToTileCoordinates().ToVector2() * 16f;

            int num3 = (Main.screenWidth + 100) / 16;
            int num4 = (Main.screenHeight + 100) / 16;

            Point point = Main.MouseWorld.ToTileCoordinates();

            point.X -= (int)vec.X / 16;
            point.Y -= (int)vec.Y / 16;

            Color color = new Color(0.24f, 0.8f, 0.9f, 0.5f) * 0.4f * scale;
            Color color2 = new Color(1f, 0.8f, 0.9f, 0.5f) * 0.5f * scale;
            Rectangle value2 = new Rectangle(0, 18, 18, 18);

            vec -= Vector2.One;

            if (GridStyle < 3) {

                for (int i = 0; i < num3; i++) {

                    for (int j = 0; j < num4; j++) {

                        Vector2 zero = Vector2.Zero;

                        if ((i != point.X && j != point.Y) || GridStyle == 2 || MouseOffScreen) {

                            if (i != point.X + 1 || GridStyle == 2 || MouseOffScreen) {
                                value2.X = 0;
                                value2.Width = 16;
                            }
                            else {
                                value2.X = 2;
                                value2.Width = 14;
                                zero.X = 2f;
                            }

                            if (j != point.Y + 1 || GridStyle == 2 || MouseOffScreen) {
                                value2.Y = 18;
                                value2.Height = 16;
                            }
                            else {
                                value2.Y = 2;
                                value2.Height = 14;
                                zero.Y = 2f;
                            }

                            // Draws the Grid
                            sb.Draw(value, Main.ReverseGravitySupport(new Vector2(i, j) * 16f - Main.screenPosition + vec + zero, 16f), value2, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

                        }

                    }

                }

            }

            if (GridStyle == 2 || MouseOffScreen) {
                PlayerInput.SetZoom_UI();
                return;
            }

            value2 = new Rectangle(0, 0, 16, 18);

            for (int k = 0; k < num3; k++) {

                if (k == point.X) {
                    // Center Square
                    sb.Draw(value, Main.ReverseGravitySupport(new Vector2(k, point.Y) * 16f - Main.screenPosition + vec, 16f), new Rectangle(0, 0, 16, 16), color2, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }
                else {
                    // Horizontal Center Line
                    sb.Draw(value, Main.ReverseGravitySupport(new Vector2(k, point.Y) * 16f - Main.screenPosition + vec, 16f), value2, color2, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }

            }

            value2 = new Rectangle(0, 0, 18, 16);

            // Vertical Center Line
            for (int l = 0; l < num4; l++)
                if (l != point.Y)
                    sb.Draw(value, Main.ReverseGravitySupport(new Vector2(point.X, l) * 16f - Main.screenPosition + vec, 16f), value2, color2, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            PlayerInput.SetZoom_UI();

        }

        private static bool DrawMapResetDialog(SpriteBatch sb, string message, string confirm = "Yes", string cancel = "Cancel") {

            if (!ShowMapResetDialog && !ShowDebugRegenDialog)
                return false;

            if (Keys.Escape.Pressed() || (Main.mouseRight && Main.mouseRightRelease)) {
                ShowDebugRegenDialog = false;
                ShowMapResetDialog = false;
                return false;
            }

            Vector2 messageSize = Main.fontMouseText.MeasureString(message);
            Vector2 confirmSize = Main.fontMouseText.MeasureString(confirm);
            Vector2 cancelSize = Main.fontMouseText.MeasureString(cancel);

            int sideMargin = 10;
            int topMargin = 10;
            int bottomMargin = 10;
            int buttonHeight = 50;
            int buttonTopMargin = 20;

            float boxLeft = Main.screenWidth / 2f - messageSize.X / 2 - sideMargin;
            float boxTop = Main.screenHeight / 2f - messageSize.Y / 2 - topMargin;

            Rectangle background = new Rectangle((int)boxLeft, (int)boxTop, (int)(messageSize.X + sideMargin * 2), (int)(messageSize.Y + topMargin + bottomMargin + buttonHeight));
            Rectangle confirmButton = new Rectangle((int)(boxLeft + sideMargin), (int)(boxTop + messageSize.Y + buttonTopMargin), (int)(confirmSize.X + sideMargin * 2), (int)(confirmSize.Y + sideMargin));
            Rectangle cancelButton = new Rectangle((int)(boxLeft + background.Width - (cancelSize.X + sideMargin * 2) - sideMargin), (int)(boxTop + messageSize.Y + buttonTopMargin), (int)(cancelSize.X + sideMargin * 2), (int)(cancelSize.Y + sideMargin));

            if (Main.mouseLeft && Main.mouseLeftRelease && !background.Contains(new Point(Main.mouseX, Main.mouseY))) {
                ShowMapResetDialog = false;
                ShowDebugRegenDialog = false;
                AddClickDelay(200);
                return false;
            }

            NeoDraw.ForceCursorToShow = true;

            Drawing.DrawBox(sb, background.X, background.Y, background.Width, background.Height, Color.CornflowerBlue);
            sb.DrawString(Main.fontMouseText, message, new Vector2(boxLeft + sideMargin, boxTop + topMargin), Color.White);

            bool buttonHovered = confirmButton.Contains(new Point(Main.mouseX, Main.mouseY));

            Color buttonColor = buttonHovered ? Color.Yellow : Color.CadetBlue;

            Drawing.DrawBox(sb, confirmButton.X, confirmButton.Y, confirmButton.Width, confirmButton.Height, buttonColor);
            sb.DrawString(Main.fontMouseText, confirm, new Vector2(confirmButton.X + sideMargin, confirmButton.Y + topMargin), Color.White);

            if (buttonHovered && Main.mouseLeft && Main.mouseLeftRelease) {
                ShowMapResetDialog = false;
                ShowDebugRegenDialog = false;
                AddClickDelay(200);
                return true;
            }

            buttonHovered = cancelButton.Contains(new Point(Main.mouseX, Main.mouseY));

            buttonColor = buttonHovered ? Color.Yellow : Color.CadetBlue;

            Drawing.DrawBox(sb, cancelButton.X, cancelButton.Y, cancelButton.Width, cancelButton.Height, buttonColor);
            sb.DrawString(Main.fontMouseText, cancel, new Vector2(cancelButton.X + sideMargin, cancelButton.Y + topMargin), Color.White);

            if (buttonHovered && Main.mouseLeft && Main.mouseLeftRelease) {
                ShowDebugRegenDialog = false;
                ShowMapResetDialog = false;
                AddClickDelay(200);
                return false;
            }

            return false;

        }

        private void DrawMinimap(SpriteBatch sb) {

            if (!Main.mapEnabled || !Main.mapReady)
                return;

            for (int i = 0; i < Main.instance.mapTarget.GetLength(0); i++) {

                for (int j = 0; j < Main.instance.mapTarget.GetLength(1); j++) {

                    if (Main.instance.mapTarget[i, j] != null) {

                        if (Main.instance.mapTarget[i, j].IsContentLost && !Main.mapWasContentLost[i, j]) {

                            Main.mapWasContentLost[i, j] = true;
                            Main.refreshMap = true;
                            Main.clearMap = true;

                        }
                        else if (!Main.instance.mapTarget[i, j].IsContentLost && Main.mapWasContentLost[i, j]) {
                            Main.mapWasContentLost[i, j] = false;
                        }

                    }
                    else {

                        //Main.refreshMap = true;
                        //return;

                        try {

                            int width = Main.textureMaxWidth;
                            int height = Main.textureMaxHeight;

                            if (i == Main.mapTargetX - 1)
                                width = 400;

                            if (j == Main.mapTargetY - 1)
                                height = 600;

                            Main.instance.mapTarget[i, j] = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, mipMap: false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                        }
                        catch {

                            Main.mapEnabled = false;

                            for (int k = 0; k < Main.mapTargetX; k++) {
                                for (int l = 0; l < Main.mapTargetY; l++) {

                                    try {
                                        Main.instance.mapTarget[k, l].Dispose();
                                    }
                                    catch { }

                                }

                            }

                            return;

                        }

                    }

                }

            }

            float num5 = Main.mapMinimapScale;

            bool flag = false;

            Matrix transformMatrix = Main.UIScaleMatrix;

            if (num5 > 1f) {

                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
                flag = true;

            }

            Main.miniMapWidth = 240;
            Main.miniMapHeight = 240;
            Main.miniMapX = AtLeftEdgeOfWorld ? 52 : Main.screenWidth - Main.miniMapWidth - 52;
            Main.miniMapY = ToolbarYoffset + 52;

            if (Main.mapMinimapScale < 0.2)
                Main.mapMinimapScale = 0.2f;

            if (Main.mapMinimapScale > 3f)
                Main.mapMinimapScale = 3f;

            if (Main.mapMinimapAlpha < 0.01)
                Main.mapMinimapAlpha = 0.01f;

            if (Main.mapMinimapAlpha > 1f)
                Main.mapMinimapAlpha = 1f;

            num5 = Main.mapMinimapScale;

            byte b = (byte)(255f * Main.mapMinimapAlpha);

            float num = Main.miniMapX;
            float num2 = Main.miniMapY;
            float num3 = num;
            float num4 = num2;

            float num34 = (Main.screenPosition.X + (PlayerInput.RealScreenWidth / 2)) / 16f;
            float num35 = (Main.screenPosition.Y + (PlayerInput.RealScreenHeight / 2)) / 16f;

            float num11 = (0f - (num34 - (int)((Main.screenPosition.X + (PlayerInput.RealScreenWidth / 2)) / 16f))) * num5;
            float num12 = (0f - (num35 - (int)((Main.screenPosition.Y + (PlayerInput.RealScreenHeight / 2)) / 16f))) * num5;

            float num15 = Main.miniMapWidth / num5;
            float num16 = Main.miniMapHeight / num5;

            float num13 = (int)num34 - num15 / 2f;
            float num14 = (int)num35 - num16 / 2f;

            float x = num3 - 6f;
            float y = num4 - 6f;

            sb.Draw(Main.miniMapFrame2Texture, new Vector2(x, y), new Rectangle(0, 0, Main.miniMapFrame2Texture.Width, Main.miniMapFrame2Texture.Height), new Color(b, b, b, b), 0f, default, 1f, SpriteEffects.None, 0f);

            float num7 = 10f;
            float num8 = 10f;
            float num9 = Main.maxTilesX - 10;
            float num10 = Main.maxTilesY - 10;

            if (num13 < num7)
                num -= (num13 - num7) * num5;

            if (num14 < num8)
                num2 -= (num14 - num8) * num5;

            num15 = num13 + num15;
            num16 = num14 + num16;

            if (num13 > num7)
                num7 = num13;

            if (num14 > num8)
                num8 = num14;

            if (num15 < num9)
                num9 = num15;

            if (num16 < num10)
                num10 = num16;

            int num6 = Main.maxTilesY / Main.textureMaxHeight;

            float num41 = Main.textureMaxHeight * num5;
            float num42 = num;
            float num43 = 0f;

            for (int k = 0; k <= Main.mapTargetX - 1; k++) {

                if (!(((k + 1) * Main.textureMaxWidth) > num7) || !((k * Main.textureMaxWidth) < num7 + num9))
                    continue;

                for (int l = 0; l <= num6; l++) {

                    if (((l + 1) * Main.textureMaxHeight) > num8 && (l * Main.textureMaxHeight) < num8 + num10) {

                        float num44 = num42;
                        float num45 = num2 + (int)(l * num41);
                        float num46 = k * Main.textureMaxWidth;
                        float num47 = l * Main.textureMaxHeight;
                        float num48 = 0f;
                        float num49 = 0f;

                        if (num46 < num7)
                            num48 = num7 - num46;

                        if (num47 < num8) {
                            num49 = num8 - num47;
                            num45 = num2;
                        }
                        else {
                            num45 -= num8 * num5;
                        }

                        float num50 = Main.textureMaxWidth;
                        float num51 = Main.textureMaxHeight;
                        float num52 = (k + 1) * Main.textureMaxWidth;
                        float num53 = (l + 1) * Main.textureMaxHeight;

                        if (num52 >= num9)
                            num50 -= num52 - num9;

                        if (num53 >= num10)
                            num51 -= num53 - num10;

                        num44 += num11;
                        num45 += num12;

                        if (num50 > num48)
                            sb.Draw(Main.instance.mapTarget[k, l], new Vector2(num44, num45), new Rectangle((int)num48, (int)num49, (int)num50 - (int)num48, (int)num51 - (int)num49), new Color(b, b, b, b), 0f, default, num5, SpriteEffects.None, 0f);

                        num43 = ((int)num50 - (int)num48) * num5;

                    }

                    if (l == num6)
                        num42 += num43;

                }

            }

            if (flag) {

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);

            }

            float num64 = num3 - 6f;
            float num65 = num4 - 6f;

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
            sb.Draw(Main.miniMapFrameTexture, new Vector2(num64, num65), new Rectangle(0, 0, Main.miniMapFrameTexture.Width, Main.miniMapFrameTexture.Height), Color.White, 0f, default, 1f, SpriteEffects.None, 0f);

            #region Minimap Buttons
            for (int num94 = 0; num94 < 3; num94++) {

                float num95 = num64 + 148f + (num94 * 26);
                float num96 = num65 + 234f;

                if (!(Main.mouseX > num95) || !(Main.mouseX < num95 + 22f) || !(Main.mouseY > num96) || !(Main.mouseY < num96 + 22f))
                    continue;

                sb.Draw(Main.miniMapButtonTexture[num94], new Vector2(num95, num96), new Rectangle(0, 0, Main.miniMapButtonTexture[num94].Width, Main.miniMapButtonTexture[num94].Height), Color.White, 0f, default, 1f, SpriteEffects.None, 0f);

                if (PlayerInput.IgnoreMouseInterface)
                    continue;

                Main.player[Main.myPlayer].mouseInterface = true;

                if (Main.mouseLeft) {

                    if (Main.mouseLeftRelease)
                        Main.PlaySound(12);

                    switch (num94) {

                        case 0: Main.mapMinimapScale = 1.25f; break;
                        case 1: Main.mapMinimapScale *= 0.975f; break;
                        case 2: Main.mapMinimapScale *= 1.025f; break;

                    }

                }

            }
            #endregion

            sb.End();
            sb.Begin();

            PlayerInput.SetZoom_Unscaled();
            TimeLogger.DetailedDrawTime(9);

        }

        private static void DrawMushroomTreeOutline(SpriteBatch sb) {

            int height = 11;
            int startX = (int)Neo.TileTarget_Vector.X;
            int startY = (int)Neo.TileTarget_Vector.Y;

            while (startY < Main.maxTilesY && (!Main.tile[startX, startY].active() || (Main.tile[startX, startY].active() && Main.tileCut[Main.tile[startX, startY].type])))
                startY++;

            Vector2 topLeft = new Vector2((startX - 1) * 16 - Main.screenPosition.X, (startY - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 topRight = new Vector2((startX + 1 + 1) * 16 - Main.screenPosition.X, (startY - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 botomLeft = new Vector2((startX - 1) * 16 - Main.screenPosition.X, (startY) * 16 - Main.screenPosition.Y);
            Vector2 botomRight = new Vector2((startX + 1 + 1) * 16 - Main.screenPosition.X, (startY) * 16 - Main.screenPosition.Y);

            bool canPlace = true;
            string mouseText = "";

            if (!Main.tile[startX, startY].nactive()) {
                canPlace = false;
                mouseText = "Start Tile is nActive.";
            }
            else if (!Main.tile[startX - 1, startY].active() ||
                     !Main.tile[startX + 1, startY].active()) {
                canPlace = false;
                mouseText = "Ground is not wide enough.";
            }
            else if (Main.tile[startX, startY].halfBrick() ||
                     Main.tile[startX, startY].slope() != 0) {
                canPlace = false;
                mouseText = "Ground is not level.";
            }
            else if (Main.tile[startX, startY].type != TileID.MushroomGrass) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (Main.tile[startX - 1, startY].type != TileID.MushroomGrass) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (Main.tile[startX + 1, startY].type != TileID.MushroomGrass) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (!EmptyTileCheck(startX - 2, startX + 2, startY - 13, startY - 1, 71, true)) {
                mouseText = "Empty Tile Check Failed.";
                canPlace = false;
            }
            else if (Main.tile[startX - 1, startY - 1].lava() || Main.tile[startX - 1, startY - 1].lava() || Main.tile[startX + 1, startY - 1].lava()) {
                canPlace = false;
                mouseText = "Cannot be placed in lava.";
            }

            Color boxColor = canPlace ? Color.Green : Color.Red;

            sb.DrawLine(topLeft, topRight, boxColor, 2);
            sb.DrawLine(topLeft, botomLeft, boxColor, 2);
            sb.DrawLine(botomLeft, botomRight, boxColor, 2);
            sb.DrawLine(topRight, botomRight, boxColor, 2);

            if (mouseText != "")
                MouseText(sb, mouseText);

        }

        private static void DrawPalmTreeOutline(SpriteBatch sb) {

            int width = 1;
            int height = 31;

            Point origin = new Point(Neo.TileTargetX, Neo.TileTargetY);

            while (origin.Y < Main.maxTilesY && (!Main.tile[origin.X, origin.Y].active() || (Main.tile[origin.X, origin.Y].active() && Main.tileCut[Main.tile[origin.X, origin.Y].type])))
                origin.Y++;

            Vector2 topLeft = new Vector2((origin.X - width) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 topRight = new Vector2((origin.X + width + 1) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 botomLeft = new Vector2((origin.X - width) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);
            Vector2 botomRight = new Vector2((origin.X + width + 1) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);

            bool canPlace = true;
            string mouseText = "";

            if (!Main.tile[origin.X, origin.Y].active()) {
                canPlace = false;
                mouseText = "Ground Tile is not Active.";
            }
            else if (Main.tile[origin.X, origin.Y].halfBrick() ||
                     Main.tile[origin.X, origin.Y].slope() != 0) {
                canPlace = false;
                mouseText = "Ground is not level.";
            }
            else if (Main.tile[origin.X, origin.Y].type != 53 &&
                     Main.tile[origin.X, origin.Y].type != 234 &&
                     Main.tile[origin.X, origin.Y].type != 116 &&
                     Main.tile[origin.X, origin.Y].type != 112 &&
                     !TileLoader.CanGrowModPalmTree(Main.tile[origin.X, origin.Y].type)) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Palm Tree.";
            }
            else if (!EmptyTileCheck(origin.X - width, origin.X + width, origin.Y - (height - 1), origin.Y - 1, 20, true)) {
                canPlace = false;
                mouseText = "Empty Tile Check Failed.";
            }

            Color boxColor = canPlace ? Color.Green : Color.Red;

            sb.DrawLine(topLeft, topRight, boxColor, 2);
            sb.DrawLine(topLeft, botomLeft, boxColor, 2);
            sb.DrawLine(botomLeft, botomRight, boxColor, 2);
            sb.DrawLine(topRight, botomRight, boxColor, 2);

            if (mouseText != "")
                MouseText(sb, mouseText);

        }

        public static void DrawPlacingTile(SpriteBatch sb) {

            if (CurrentTab != Tabs.Tiles)
                return;

            if (Main.gameMenu || Main.gameInactive || Main.mapFullscreen)
                return;

            int x = Neo.TileTargetX;
            int y = Neo.TileTargetY;

            int tileToCreate = NeoDraw.TileToCreate ?? -1;

            if (tileToCreate == -1)
                return;

            if (tileToCreate == TileID.Trees) {

                tileToCreate = TileID.Saplings;
                DrawTreeOutline(sb);

            }
            else if (tileToCreate == TileID.PalmTree) {

                tileToCreate = TileID.Saplings;
                DrawPalmTreeOutline(sb);

            }
            else if (tileToCreate == TileID.Cactus) {

                tileToCreate = TileID.Saplings;
                DrawCactusOutline(sb);

            }
            else if (tileToCreate == TileID.MushroomTrees) {

                tileToCreate = TileID.Saplings;
                DrawMushroomTreeOutline(sb);

            }

            TOD tod = GetTileData(tileToCreate);

            if (Main.tileTexture[tileToCreate] == null)
                Main.instance.LoadTiles(tileToCreate);

            if (Main.tileTexture[tileToCreate] == null)
                return;

            Texture2D tileTexture = Main.tileTexture[tileToCreate];

            float xToDrawAt = (x * 16f - Main.screenPosition.X) - (tod.Origin.X * 16);
            float yToDrawAt = (y * 16f - Main.screenPosition.Y) - (tod.Origin.Y * 16) + tod.DrawYOffset;

            int placeStyleX = _placeStyle;
            int placeStyleY = 0;

            if (styleDown.Contains(tileToCreate)) {
                placeStyleX = 0;
                placeStyleY = _placeStyle;
            }

            switch (tileToCreate) {

                case TileID.Torches: {

                        placeStyleY = _placeStyle;

                        Tile tileAbove = Main.tile[x, y - 1];
                        Tile tileBelow = Main.tile[x, y + 1];
                        Tile tileLeft = Main.tile[x - 1, y];
                        Tile tileRight = Main.tile[x + 1, y];

                        xToDrawAt -= 2;

                        if (tileBelow.active() && Main.tileSolid[tileBelow.type]) {
                            placeStyleX = 0;
                            if (tileAbove.active() && Main.tileSolid[tileAbove.type])
                                yToDrawAt += 4;
                        }
                        else if (tileLeft.active() && Main.tileSolid[tileLeft.type]) {
                            placeStyleX = 1;
                            if (tileAbove.active() && Main.tileSolid[tileAbove.type])
                                yToDrawAt += 4;
                        }
                        else if (tileRight.active() && Main.tileSolid[tileRight.type]) {
                            placeStyleX = 2;
                            if (tileAbove.active() && Main.tileSolid[tileAbove.type])
                                yToDrawAt += 4;
                        }

                        if (Main.keyState.PressingAlt())
                            placeStyleX += 3;

                        break;

                    }
                case TileID.ProjectilePressurePad: {

                        if (Main.tile[x, y + 1] == null)
                            Main.tile[x, y + 1] = new Tile();

                        if (Main.tile[x, y - 1] == null)
                            Main.tile[x, y - 1] = new Tile();

                        if (Main.tile[x - 1, y] == null)
                            Main.tile[x - 1, y] = new Tile();

                        if (Main.tile[x + 1, y] == null)
                            Main.tile[x + 1, y] = new Tile();

                        if (Main.tile[x, y + 1].active() && WorldGen.SolidTile(x, y + 1)) {
                            xToDrawAt -= 2;
                            placeStyleX = 0;

                        }
                        else if (Main.tile[x, y - 1].active() && WorldGen.SolidTile(x, y - 1)) {

                            placeStyleX = 1;

                        }
                        else if (Main.tile[x - 1, y].active() && WorldGen.SolidTile(x - 1, y)) {

                            placeStyleX = 2;

                        }
                        else if (Main.tile[x + 1, y].active() && WorldGen.SolidTile(x + 1, y)) {

                            placeStyleX = 3;

                        }

                        break;

                    }
                case TileID.ScorpionCage:
                case TileID.BlackScorpionCage:
                case TileID.GrasshopperCage: {

                        yToDrawAt -= 2;

                        break;

                    }
                case TileID.GeyserTrap: {

                        if ((!WorldGen.SolidTile2(x, y + 1) || !WorldGen.SolidTile2(x + 1, y + 1)) && (WorldGen.SolidTile2(x, y - 1) && WorldGen.SolidTile2(x + 1, y - 1)))
                            placeStyleX += 2;

                        yToDrawAt -= 2;
                        break;

                    }
                case TileID.Signs:
                case TileID.AnnouncementBox: {

                        Tile tileAbove1 = Main.tile[x, y - 2];
                        Tile tileAbove2 = Main.tile[x + 1, y - 2];
                        Tile tileBelow1 = Main.tile[x, y + 1];
                        Tile tileBelow2 = Main.tile[x + 1, y + 1];
                        Tile tileLeft1 = Main.tile[x - 1, y];
                        Tile tileLeft2 = Main.tile[x - 1, y - 1];
                        Tile tileRight1 = Main.tile[x + 2, y];
                        Tile tileRight2 = Main.tile[x + 2, y - 1];

                        if (tileBelow1.active() && Main.tileSolid[tileBelow1.type] && !tileBelow1.topSlope() &&
                            tileBelow2.active() && Main.tileSolid[tileBelow2.type] && !tileBelow2.topSlope()) {
                            placeStyleX = 0;
                        }
                        else if (tileAbove1.active() && Main.tileSolid[tileAbove1.type] && !tileAbove1.bottomSlope() &&
                                 tileAbove2.active() && Main.tileSolid[tileAbove2.type] && !tileAbove2.bottomSlope()) {
                            placeStyleX = 1;
                            yToDrawAt += 16;
                        }
                        else if (tileLeft1.active() && Main.tileSolid[tileLeft1.type] && !tileLeft1.rightSlope() &&
                                 tileLeft2.active() && Main.tileSolid[tileLeft2.type] && !tileLeft2.rightSlope()) {
                            placeStyleX = 2;
                            yToDrawAt += 16;
                        }
                        else if (tileRight1.active() && Main.tileSolid[tileRight1.type] && !tileRight1.leftSlope() &&
                                 tileRight2.active() && Main.tileSolid[tileRight2.type] && !tileRight2.leftSlope()) {
                            placeStyleX = 3;
                            xToDrawAt -= 16;
                            yToDrawAt += 16;
                        }
                        else if (tileAbove1.wall + tileAbove2.wall + tileBelow1.wall + tileBelow2.wall + tileLeft1.wall + tileLeft2.wall + tileRight1.wall + tileRight2.wall >= 8) {
                            placeStyleX = 4;
                            yToDrawAt += 16;
                        }

                        break;

                    }
                case TileID.Coral: {

                        xToDrawAt -= 4;
                        break;

                    }
                case TileID.BeachPiles: {

                        xToDrawAt -= 2;

                        if (_placeStyle > 5) {
                            placeStyleX = 0;
                            placeStyleY = _placeStyle - 4;
                        }
                        else if (_placeStyle > 2) {
                            placeStyleX -= 3;
                            placeStyleY = 1;
                        }

                        break;

                    }
                case TileID.HoneyDrip:
                case TileID.LavaDrip:
                case TileID.SandDrip:
                case TileID.WaterDrip: {

                        xToDrawAt += 8;
                        yToDrawAt -= 24;

                        break;

                    }
                case TileID.Statues: {

                        if (_placeStyle > 54) {
                            placeStyleX -= 55;
                            placeStyleY++;
                        }

                        //if (Main.keyState.PressingAlt()) // TODO: Re-add this for v1.4
                        //    placeStyleY += 3;

                        xToDrawAt += 16;
                        break;

                    }
                case TileID.WaterFountain:
                case TileID.SeaweedPlanter:
                case TileID.AlphabetStatues:
                case TileID.Sundial:
                case TileID.FishingCrate:
                case TileID.TargetDummy:
                case TileID.PartyBundleOfBalloonTile: {

                        xToDrawAt += 16;
                        break;

                    }
                case TileID.FakeContainers:
                case TileID.FakeContainers2: {

                        xToDrawAt -= 16;
                        break;

                    }
                case TileID.Pots: {

                        xToDrawAt += 16;
                        yToDrawAt += 2;
                        placeStyleY = _placeStyle * 3 + 1;
                        break;

                    }
                case TileID.LunarMonolith: {

                        xToDrawAt += 16;
                        //yToDrawAt += 2;
                        break;

                    }
                case TileID.PlatinumCandelabra:
                case TileID.FishBowl: case TileID.GoldButterflyCage: case TileID.WeightedPressurePlate: {

                        yToDrawAt += 2;
                        break;

                    }
                case TileID.Lever: {

                        //yToDrawAt += 1;

                        Tile topLeft = Main.tile[x - 1, y - 1];
                        Tile topRight = Main.tile[x, y - 1];
                        Tile bottomLeft = Main.tile[x - 1, y];
                        Tile bottomRight = Main.tile[x, y];

                        if (topLeft == null)
                            topLeft = new Tile();

                        if (topRight == null)
                            topRight = new Tile();

                        if (bottomLeft == null)
                            bottomLeft = new Tile();

                        if (bottomRight == null)
                            bottomRight = new Tile();

                        bool flag = true;

                        if (!Main.tile[x - 1, y + 1].nactive() || (!WorldGen.SolidTile2(x - 1, y + 1) && !Main.tileTable[Main.tile[x - 1, y + 1].type]))
                            flag = false;

                        if (!Main.tile[x, y + 1].nactive() || (!WorldGen.SolidTile2(x, y + 1) && !Main.tileTable[Main.tile[x, y + 1].type]))
                            flag = false;

                        if (!flag && (topLeft.wall > 0 && topRight.wall > 0 && bottomLeft.wall > 0 && bottomRight.wall > 0)) {
                            placeStyleX = 2;
                        }

                        break;

                    }
                case TileID.Switches: {

                        Tile tileBelow = Main.tile[x, y + 1];
                        Tile tileLeft1 = Main.tile[x - 1, y];
                        Tile tileLeft2 = Main.tile[x - 1, y - 1];
                        Tile tileLeft3 = Main.tile[x - 1, y + 1];
                        Tile tileRight1 = Main.tile[x + 2, y];
                        Tile tileRight2 = Main.tile[x + 2, y - 1];
                        Tile tileRight3 = Main.tile[x + 2, y + 1];

                        if (

                               tileLeft1.nactive() &&
                               !tileLeft1.halfBrick() &&
                               !TileID.Sets.NotReallySolid[tileLeft1.type] &&
                               tileLeft1.slope() == 0 &&
                               (
                                   WorldGen.SolidTile(x - 1, y) ||
                                   tileLeft1.type == 124 ||
                                   (
                                       tileLeft1.type == 5 &&
                                       tileLeft2.type == 5 &&
                                       tileLeft3.type == 5
                                   )
                               )

                           ) {

                            placeStyleX = 1;

                        }
                        else if (

                               tileRight1.nactive() &&
                               !tileRight1.halfBrick() &&
                               !TileID.Sets.NotReallySolid[tileRight1.type] &&
                               tileRight1.slope() == 0 &&
                               (
                                   WorldGen.SolidTile(x + 1, y) ||
                                   tileRight1.type == 124 ||
                                   (
                                       tileRight1.type == 5 &&
                                       tileRight2.type == 5 &&
                                       tileRight3.type == 5
                                   )
                               )

                           ) {

                            placeStyleX = 2;

                        }
                        else if (

                               tileBelow.nactive() &&
                               !tileBelow.halfBrick() &&
                               WorldGen.SolidTile(x, y + 1) &&
                               tileBelow.slope() == 0
                           ) {

                            placeStyleX = 0;

                        }
                        else if (Main.tile[x, y].wall > 0) {

                            placeStyleX = 3;

                        }

                        break;

                    }
                case TileID.ExposedGems: {

                        Tile tileAbove = Main.tile[x, y - 1];
                        Tile tileBelow = Main.tile[x, y + 1];
                        Tile tileLeft = Main.tile[x - 1, y];
                        Tile tileRight = Main.tile[x + 2, y];

                        if (tileBelow.active() && Main.tileSolid[tileBelow.type]) {
                            placeStyleY = 0;
                        }
                        else if (tileLeft.active() && Main.tileSolid[tileLeft.type]) {
                            placeStyleY = 2 * 3;
                        }
                        else if (tileRight.active() && Main.tileSolid[tileRight.type]) {
                            placeStyleY = 3 * 3;
                        }
                        else if (tileAbove.active() && Main.tileSolid[tileAbove.type]) {
                            placeStyleY = 1 * 3;
                        }

                        break;

                    }
                case TileID.SmallPiles: {

                        yToDrawAt += 2;

                        SpriteEffects se = SpriteEffects.None;

                        if (x % 2 != 0)
                            se = SpriteEffects.FlipHorizontally;

                        if (placeStyleX > 71) {

                            se = SpriteEffects.None;

                            placeStyleX -= 72;
                            placeStyleY = 1;

                            if (placeStyleX > 52) {

                                placeStyleX -= 53;
                                placeStyleY = 3;

                            }

                            for (int i = 0; i < 2; i++) {

                                sb.Draw(
                                        tileTexture,

                                        new Vector2(
                                                        xToDrawAt + tod.CoordinateWidth * i,
                                                        yToDrawAt
                                                    ),

                                        new Rectangle(
                                                          tod.CoordinateFullWidth * 2 * placeStyleX + (tod.CoordinateWidth + tod.CoordinatePadding) * i,
                                                          tod.CoordinateFullHeight * placeStyleY,
                                                          tod.CoordinateWidth,
                                                          tod.CoordinateHeights[0]
                                                      ),

                                        Color.White * Main.cursorAlpha * 0.5f,
                                        rotation: 0f,
                                        origin: Vector2.Zero,
                                        scale: 1f,
                                        se,
                                        layerDepth: 1
                                    );

                            }

                        }
                        else {

                            sb.Draw(
                                        tileTexture,

                                        new Vector2(
                                                        xToDrawAt,
                                                        yToDrawAt
                                                    ),

                                        new Rectangle(
                                                          tod.CoordinateFullWidth * placeStyleX,
                                                          tod.CoordinateFullHeight * placeStyleY,
                                                          tod.CoordinateWidth,
                                                          tod.CoordinateHeights[0]
                                                      ),

                                        Color.White * Main.cursorAlpha * 0.5f,
                                        rotation: 0f,
                                        origin: Vector2.Zero,
                                        scale: 1f,
                                        se,
                                        layerDepth: 1
                                    );

                        }

                        return;

                    }
                case TileID.PlantDetritus: {

                        xToDrawAt -= 16;
                        yToDrawAt += 2;

                        if (_placeStyle > 8) {

                            placeStyleX -= 9;
                            placeStyleY = 2;

                            for (int i = 0; i < 2; i++) {

                                for (int j = 0; j < 2; j++) {

                                    sb.Draw(
                                            tileTexture,

                                            new Vector2(
                                                            xToDrawAt + tod.CoordinateWidth * i,
                                                            yToDrawAt + tod.CoordinateHeights[0] * j
                                                        ),

                                            new Rectangle(
                                                              tod.CoordinateFullWidth * 2 * placeStyleX + (tod.CoordinateWidth + tod.CoordinatePadding) * i,
                                                              tod.CoordinateFullHeight * placeStyleY + (tod.CoordinateHeights[0] + tod.CoordinatePadding) * j,
                                                              tod.CoordinateWidth,
                                                              tod.CoordinateHeights[0]
                                                          ),

                                            Color.White * Main.cursorAlpha * 0.5f,
                                            rotation: 0f,
                                            origin: Vector2.Zero,
                                            scale: 1f,
                                            SpriteEffects.None,
                                            layerDepth: 1
                                        );

                                }

                            }

                        }
                        else {

                            for (int i = 0; i < 3; i++) {

                                for (int j = 0; j < 2; j++) {

                                    sb.Draw(
                                            tileTexture,

                                            new Vector2(
                                                            xToDrawAt + tod.CoordinateWidth * i,
                                                            yToDrawAt + tod.CoordinateHeights[0] * j
                                                        ),

                                            new Rectangle(
                                                              tod.CoordinateFullWidth * 3 * placeStyleX + (tod.CoordinateWidth + tod.CoordinatePadding) * i,
                                                              tod.CoordinateFullHeight * placeStyleY + (tod.CoordinateHeights[0] + tod.CoordinatePadding) * j,
                                                              tod.CoordinateWidth,
                                                              tod.CoordinateHeights[0]
                                                          ),

                                            Color.White * Main.cursorAlpha * 0.5f,
                                            rotation: 0f,
                                            origin: Vector2.Zero,
                                            scale: 1f,
                                            SpriteEffects.None,
                                            layerDepth: 1
                                        );

                                }

                            }

                        }

                        return;

                    }
                case TileID.Painting3X3: {

                        while (placeStyleX > 35) {
                            placeStyleX -= 36;
                            placeStyleY++;
                        }

                        break;

                    }
                case TileID.MinecartTrack: {

                        switch (_placeStyle) {

                            case 0: {
                                    placeStyleX = 0;
                                    placeStyleY = 0;
                                    break;
                                }
                            case 1: {
                                    placeStyleX = 0;
                                    placeStyleY = 4;
                                    break;
                                }
                            case 2: {
                                    placeStyleX = 2;
                                    placeStyleY = 3;
                                    break;
                                }

                        }

                        break;

                    }
                case TileID.Stalactite: {

                        placeStyleX = placeStyleY = -1;

                        int height = _placeStyle == 0 ? 2 : 1;

                        bool hive = false;
                        bool spider = false;

                        if (Main.tile[x, y] == null)
                            Main.tile[x, y] = new Tile();

                        if (Main.tile[x, y - 1] == null)
                            Main.tile[x, y - 1] = new Tile();

                        if (Main.tile[x, y + 1] == null)
                            Main.tile[x, y + 1] = new Tile();

                        Tile tile = Main.tile[x, y];
                        Tile tileAbove = Main.tile[x, y - 1];
                        Tile tileBelow = Main.tile[x, y + 1];

                        if (WorldGen.SolidTile(x, y - 1) && !tile.active() && !tileBelow.active()) { // Place on ceiling

                            if (tileAbove.type == TileID.SnowBlock || tileAbove.type == TileID.IceBlock || tileAbove.type == TileID.CorruptIce || tileAbove.type == TileID.HallowedIce || tileAbove.type == TileID.FleshIce) {

                                placeStyleX = 0;

                            }
                            else if (tileAbove.type == TileID.Stone || Main.tileMoss[tileAbove.type] || tileAbove.type == TileID.Pearlstone || tileAbove.type == TileID.Ebonstone || tileAbove.type == TileID.Crimstone) {

                                placeStyleX = 3;

                            }
                            else if (tileAbove.type == TileID.Sandstone || tileAbove.type == TileID.HardenedSand) {

                                placeStyleX = 21;

                            }
                            else if (tileAbove.type == TileID.Hive) {

                                hive = true;
                                placeStyleX = 9;

                            }
                            else if (tileAbove.type == TileID.Granite) {

                                placeStyleX = 24;

                            }
                            else if (tileAbove.type == TileID.Marble) {

                                placeStyleX = 27;

                            }

                            if (_placeStyle == 0) { // Tall

                                placeStyleY = 0;

                                if (hive)
                                    placeStyleX = placeStyleY = -1;

                            }
                            else if (_placeStyle == 1) { // Short

                                placeStyleY = 4;
                                placeStyleX++;

                                if (spider)
                                    placeStyleX = placeStyleY = -1;

                            }

                        }
                        else if (WorldGen.SolidTile(x, y + 1) && !tile.active() && !tileAbove.active()) { // Place on floor

                            if (Main.tile[x, y + 1].type == TileID.Stone || Main.tileMoss[Main.tile[x, y + 1].type] || Main.tile[x, y - 1].type == TileID.Pearlstone || Main.tile[x, y - 1].type == TileID.Ebonstone || Main.tile[x, y - 1].type == TileID.Crimstone) {

                                placeStyleX = 3;

                            }
                            else if (Main.tile[x, y + 1].type == TileID.Sandstone || Main.tile[x, y + 1].type == TileID.HardenedSand) {

                                placeStyleX = 21;

                            }
                            else if (Main.tile[x, y + 1].type == TileID.Hive) {

                                hive = true;
                                placeStyleX = 9;

                            }
                            else if (Main.tile[x, y + 1].type == TileID.Granite) {

                                placeStyleX = 24;

                            }
                            else if (Main.tile[x, y + 1].type == TileID.Marble) {

                                placeStyleX = 27;

                            }

                            if (_placeStyle == 0) { // Tall

                                placeStyleY = 2;

                                yToDrawAt -= 16;

                                if (hive)
                                    placeStyleX = placeStyleY = -1;

                                if (spider)
                                    placeStyleX = placeStyleY = -1;


                            }
                            else if (_placeStyle == 1) { // Short

                                placeStyleY = 5;
                                placeStyleX++;

                                if (spider)
                                    placeStyleX = placeStyleY = -1;

                            }

                        }

                        if (placeStyleX >= 0 && placeStyleY >= 0) {

                            SpriteEffects se = SpriteEffects.None;

                            for (int i = 0; i < 1; i++) {

                                for (int j = 0; j < height; j++) {

                                    sb.Draw(
                                            tileTexture,

                                            new Vector2(
                                                            xToDrawAt + 16 * i,
                                                            yToDrawAt + 16 * j
                                                        ),

                                            new Rectangle(
                                                              18 * placeStyleX + 18 * i,
                                                              18 * placeStyleY + 18 * j,
                                                              16,
                                                              16
                                                          ),

                                            Color.White * Main.cursorAlpha * 0.5f,
                                            rotation: 0f,
                                            origin: Vector2.Zero,
                                            scale: 1f,
                                            se,
                                            layerDepth: 1
                                        );

                                }

                            }

                        }
                        else {

                            sb.Draw(
                                    tileTexture,

                                    new Vector2(
                                                    xToDrawAt,
                                                    yToDrawAt
                                                ),

                                    new Rectangle(
                                                      18 * 4,
                                                      18 * 4,
                                                      16,
                                                      16
                                                  ),

                                    Color.White * Main.cursorAlpha * 0.5f,
                                    rotation: 0f,
                                    origin: Vector2.Zero,
                                    scale: 1f,
                                    SpriteEffects.None,
                                    layerDepth: 1
                                );

                            tileTexture = GetTexture("Terraria/CoolDown");

                            sb.Draw(
                                    tileTexture,

                                    new Vector2(
                                                    xToDrawAt - 0,
                                                    yToDrawAt - 8
                                                ),

                                    new Rectangle(
                                                      0,
                                                      0,
                                                      tileTexture.Width,
                                                      tileTexture.Height
                                                  ),

                                    Color.White * Main.cursorAlpha * 0.5f,
                                    rotation: 0f,
                                    origin: Vector2.Zero,
                                    scale: 0.5f,
                                    SpriteEffects.None,
                                    layerDepth: 1
                                );

                        }

                        return;

                    }
                case TileID.Plants2:
                case TileID.JunglePlants2:
                case TileID.HallowedPlants2: {

                        yToDrawAt += 4;
                        break;

                    }
                case TileID.Chairs: {

                        //yToDrawAt += 1;
                        break;

                    }
                case TileID.DyePlants: {

                        xToDrawAt -= 8;
                        break;

                    }
                case TileID.MushroomStatue: {

                        xToDrawAt += 16;
                        yToDrawAt -= 2;
                        break;

                    }
                case TileID.Beds:
                case TileID.Bathtubs: {

                        if (Main.keyState.PressingAlt())
                            placeStyleX = 1;

                        break;

                    }
                case TileID.OpenDoor: {

                        if (_placeStyle > 35) {
                            placeStyleX += 2;
                            placeStyleY -= 36;
                        }

                        if (Main.keyState.PressingAlt()) {
                            placeStyleX++;
                            xToDrawAt -= 16;
                        }

                        break;

                    }
                case TileID.ClosedDoor: {

                        if (_placeStyle > 35) {
                            placeStyleX += 3;
                            placeStyleY -= 36;
                        }

                        break;

                    }
                case TileID.Mannequin:
                case TileID.Womannequin: {

                        if (Main.keyState.PressingAlt())
                            placeStyleX++;

                        break;

                    }
                case TileID.Platforms: case TileID.TeamBlockBluePlatform: case TileID.TeamBlockGreenPlatform: case TileID.TeamBlockPinkPlatform: case TileID.TeamBlockRedPlatform: case TileID.TeamBlockWhitePlatform: case TileID.TeamBlockYellowPlatform: {

                        placeStyleX = 5;

                        break;

                    }
                case TileID.PlanterBox: {

                        if (Main.tile[x - 1, y] == null)
                            Main.tile[x - 1, y] = new Tile();

                        if (Main.tile[x + 1, y] == null)
                            Main.tile[x + 1, y] = new Tile();

                        Tile tileLeft = Main.tile[x - 1, y];
                        Tile tileRight = Main.tile[x + 1, y];

                        if (tileLeft.type == TileID.PlanterBox && tileRight.type == TileID.PlanterBox) {
                            placeStyleX += 1;
                        }
                        else if (tileLeft.type == TileID.PlanterBox) {
                            placeStyleX += 2;
                        }
                        else if (tileRight.type == TileID.PlanterBox) {

                        }
                        else {
                            placeStyleX += 3;
                        }

                        break;

                    }
                case TileID.SillyBalloonTile: {

                        placeStyleX = _placeStyle * 2;

                        if (Main.keyState.PressingAlt())
                            placeStyleX++;

                        break;

                    }
                case TileID.GemLocks: {

                        if (_placeStyle > 6) {

                            placeStyleX -= 7;
                            placeStyleY++;

                        }

                        break;

                    }
                case TileID.TrapdoorOpen: {

                        if (Main.keyState.PressingAlt()) {
                            placeStyleX++;
                            yToDrawAt += 16;
                        }

                        break;

                    }

            }

            SpriteEffects spriteEffects = SpriteEffects.None;

            if ((tileToCreate == TileID.Plants || tileToCreate == TileID.CorruptPlants || tileToCreate == TileID.HallowedPlants || tileToCreate == TileID.JunglePlants || tileToCreate == TileID.Bottles ||
                tileToCreate == TileID.MushroomPlants || tileToCreate == TileID.FleshWeeds || tileToCreate == TileID.Coral || tileToCreate == TileID.DyePlants || tileToCreate == TileID.LightningBuginaBottle ||
                tileToCreate == TileID.BloomingHerbs || tileToCreate == TileID.ImmatureHerbs || tileToCreate == TileID.MatureHerbs || tileToCreate == TileID.Banners || tileToCreate == TileID.Lampposts || tileToCreate == TileID.Lamps ||
                tileToCreate == TileID.Plants2 || tileToCreate == TileID.JunglePlants2 || tileToCreate == TileID.HallowedPlants2 || tileToCreate == TileID.PeaceCandle || tileToCreate == TileID.Books || tileToCreate == TileID.FireflyinaBottle)
                && x % 2 != 0)
                spriteEffects = SpriteEffects.FlipHorizontally;

            if (tileToCreate == TileID.HoneyDrip || tileToCreate == TileID.LavaDrip || tileToCreate == TileID.SandDrip || tileToCreate == TileID.WaterDrip) {

                if (tileToCreate == TileID.HoneyDrip) {

                    if (Main.itemTexture[ItemID.MagicHoneyDropper] != null)
                        tileTexture = Main.itemTexture[ItemID.MagicHoneyDropper];

                }
                else if (tileToCreate == TileID.LavaDrip) {

                    if (Main.itemTexture[ItemID.MagicLavaDropper] != null)
                        tileTexture = Main.itemTexture[ItemID.MagicLavaDropper];

                }
                else if (tileToCreate == TileID.SandDrip) {

                    if (Main.itemTexture[ItemID.MagicSandDropper] != null)
                        tileTexture = Main.itemTexture[ItemID.MagicSandDropper];

                }
                else if (tileToCreate == TileID.WaterDrip) {

                    if (Main.itemTexture[ItemID.MagicWaterDropper] != null)
                        tileTexture = Main.itemTexture[ItemID.MagicWaterDropper];

                }

                sb.Draw(

                        tileTexture,

                        new Vector2(xToDrawAt - 1, yToDrawAt + 1),

                        new Rectangle(0, 0, tileTexture.Width, tileTexture.Height),

                        Color.White * Main.cursorAlpha * 0.5f

                    );

            }
            else if (tod.Width >= 1) {

                if (tod.Width == 1 && tod.Height == 1 && (Main.tileTexture[tileToCreate].Width == 288 || Main.tileTexture[tileToCreate].Width == 234 || Main.tileTexture[tileToCreate].Width == 324)) {

                    sb.Draw(

                            tileTexture,

                            new Vector2(xToDrawAt - 1, yToDrawAt + 1),

                            new Rectangle(18 * 9, 18 * 3, 16, 16),

                            Color.White * Main.cursorAlpha * 0.5f

                        );

                }
                else {

                    for (int i = 0; i < tod.Width; i++) {

                        for (int j = 0; j < tod.Height; j++) {

                            sb.Draw(
                                    texture: tileTexture,

                                    position: new Vector2(
                                                    xToDrawAt + tod.CoordinateWidth * i,
                                                    yToDrawAt + (j == 0 ? 0 : tod.CoordinateHeights[j - 1]) * j
                                                ),

                                    sourceRectangle: new Rectangle(
                                                      tod.CoordinateFullWidth * placeStyleX + (tod.CoordinateWidth + tod.CoordinatePadding) * i,
                                                      tod.CoordinateFullHeight * placeStyleY + ((j == 0 ? 0 : tod.CoordinateHeights[j - 1]) + tod.CoordinatePadding) * j,
                                                      tod.CoordinateWidth,
                                                      tod.CoordinateHeights[j]
                                                  ),

                                    color: Color.White * Main.cursorAlpha * 0.5f,
                                    rotation: 0f,
                                    origin: Vector2.Zero,
                                    scale: 1f,
                                    effects: spriteEffects,
                                    layerDepth: 1
                                );

                        }

                    }

                }

            }

        }

        private static void DrawSelectedPoints(SpriteBatch sb) {

            for (int i = 0; i < SelectedPoints.Count; i++) {

                if (SelectedPoints[i].X * 16 < Main.screenPosition.X - 16)
                    continue;

                if (SelectedPoints[i].X * 16 > Main.screenPosition.X + Main.screenWidth + 16)
                    continue;

                if (SelectedPoints[i].Y * 16 < Main.screenPosition.Y - 16)
                    continue;

                if (SelectedPoints[i].Y * 16 > Main.screenPosition.Y + Main.screenHeight + 16)
                    continue;

                sb.Draw(Square, new Rectangle((int)(SelectedPoints[i].X * 16 - 2 - Main.screenPosition.X), (int)(SelectedPoints[i].Y * 16 - 2 - Main.screenPosition.Y), 18, 18), Color.LightBlue * 0.5f);

                if (!SelectedPointsHash.Contains(GetHash(SelectedPoints[i].X, SelectedPoints[i].Y - 1))) {

                    sb.Draw(SelectionDash, new Rectangle((int)(SelectedPoints[i].X * 16 - 2 - Main.screenPosition.X), (int)(SelectedPoints[i].Y * 16 - 2 - Main.screenPosition.Y), 16, 2),
                        new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White);

                }

                if (!SelectedPointsHash.Contains(GetHash(SelectedPoints[i].X, SelectedPoints[i].Y + 1))) {

                    sb.Draw(SelectionDash, new Rectangle((int)(SelectedPoints[i].X * 16 - 2 - Main.screenPosition.X), (int)(SelectedPoints[i].Y * 16 + 16 - Main.screenPosition.Y), 16, 2),
                        new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, 0f, default, SpriteEffects.FlipHorizontally, 0);

                }

                if (!SelectedPointsHash.Contains(GetHash(SelectedPoints[i].X - 1, SelectedPoints[i].Y))) {

                    sb.Draw(SelectionDash, new Rectangle((int)(SelectedPoints[i].X * 16 - Main.screenPosition.X), (int)(SelectedPoints[i].Y * 16 - Main.screenPosition.Y), 16, 2),
                        new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.FlipHorizontally, 0f);

                }

                if (!SelectedPointsHash.Contains(GetHash(SelectedPoints[i].X + 1, SelectedPoints[i].Y))) {

                    sb.Draw(SelectionDash, new Rectangle((int)(SelectedPoints[i].X * 16 + 16 - Main.screenPosition.X), (int)(SelectedPoints[i].Y * 16 - Main.screenPosition.Y), 16, 2),
                        new Rectangle(0, SelectionFrameCurrent * 2, 24, 2), Color.White, MathHelper.ToRadians(90), default, SpriteEffects.None, 0f);

                }

            }

        }

        private static void DrawSelection() {

            _undo = new UndoStep();

            IList<Point> points = CurrentSelection.GetChangedTilesList();

            for (int i = 0; i < CurrentSelection.Count; i++) {

                _undo.Add(new ChangedTile(points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY));

                if (CurrentTab == Tabs.Tiles || CurrentTab == Tabs.Structures) {
                    ushort tempWall = Main.tile[points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY].wall;
                    Main.tile[points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY] = new Tile(CurrentSelection.GetTileAtIndex(i)) {
                        wall = tempWall
                    };
                }

                if (CurrentTab == Tabs.Walls || CurrentTab == Tabs.Structures)
                    Main.tile[points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY].wall = CurrentSelection.GetTileAtIndex(i).wall;

            }

            for (int i = 0; i < CurrentSelection.Count; i++) {

                if (CurrentTab == Tabs.Tiles || CurrentTab == Tabs.Structures)
                    WorldGen.SquareTileFrame(points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY);

                if (CurrentTab == Tabs.Walls || CurrentTab == Tabs.Structures)
                    WorldGen.SquareWallFrame(points[i].X + Neo.TileTargetX, points[i].Y + Neo.TileTargetY);

            }

            Pasting = false;

        }

        private static bool DrawToolbar(SpriteBatch sb) {

            string oldTyping = SearchBoxText;

            UpdateSearchBar();

            DrawSearchBar(sb, false, true);

            if (oldTyping != SearchBoxText)
                Refresh(true);

            DrawSearchBar(sb, true, false);

            bool checkHoverTop = false;

            if (MouseXoverToolbar && Main.mouseY < SearchTop - TabHeight + ToolbarYoffset) {

                HoverLeftTopBox();
                checkHoverTop = true;

            }

            DrawBox(sb, 0 + ToolbarXoffset, 0 + ToolbarYoffset, ListWidth, TopBoxHeight, BoxAlpha);
            DrawButtonsTop(sb, checkHoverTop);

            bool checkHoverMiddle = false;

            if (MouseXoverToolbar && Main.mouseY >= SearchTop && Main.mouseY < MiddleListBottom) {

                HoverLeftMiddleBox();
                checkHoverMiddle = true;

            }

            DrawBox(sb, 0 + ToolbarXoffset, MiddleListTop + ToolbarYoffset, ListWidth, MiddleListLength + 4, BoxAlpha);
            DrawListMiddle(sb, checkHoverMiddle);

            bool checkHoverTabs = false;

            if (MouseXoverToolbar && Main.mouseY >= SearchTop - TabHeight && Main.mouseY < SearchTop) {

                HoverTabs();
                checkHoverTabs = true;

            }

            DrawTabs(sb, checkHoverTabs);

            if (CurrentTab == Tabs.Tiles) {

                if (NeoDraw.TileToCreate.HasValue && NeoDraw.TileToCreate != _currentSubTile) {

                    _currentSubTile = NeoDraw.TileToCreate.GetValueOrDefault();
                    SubTileNames.Clear();

                    if (TileNames.SubTileNames.ContainsKey(NeoDraw.TileToCreate.GetValueOrDefault()))
                        SubTileNames.AddRange(TileNames.SubTileNames[NeoDraw.TileToCreate.GetValueOrDefault()]);

                }

            }
            else if (CurrentTab == Tabs.Structures) {

                if (NeoDraw.StructureToCreate.HasValue && NeoDraw.StructureToCreate != _currentSubStructure) {

                    _currentSubStructure = NeoDraw.StructureToCreate.GetValueOrDefault();
                    SubStructureNames.Clear();

                    if (StructureSubList.ContainsKey(StructureToCreateName))
                        SubStructureNames.AddRange(StructureSubList[StructureToCreateName]);

                }

            }
            else if (CurrentTab == Tabs.Walls) {

                // TODO: Add paint selection here.

            }
            else if (CurrentTab == Tabs.Other) {

                if (NeoDraw.OtherToCreate.HasValue && NeoDraw.OtherToCreate != _currentSubOther) {

                    _currentSubOther = NeoDraw.OtherToCreate.GetValueOrDefault();
                    SubOtherNames.Clear();

                    if (OtherSubList.ContainsKey(OthersList[NeoDraw.OtherToCreate.GetValueOrDefault()].Name))
                        SubOtherNames.AddRange(OtherSubList[OthersList[NeoDraw.OtherToCreate.GetValueOrDefault()].Name]);

                }

            }

            DrawMiddleVerticalSlider(sb);

            bool checkHoverBottom = false;

            if (MouseXoverToolbar && Main.mouseY >= MiddleListBottom) {

                HoverLeftBottomBox();
                checkHoverBottom = true;

            }

            DrawBox(sb, 0 + ToolbarXoffset, MiddleListBottom + ToolbarYoffset, ListWidth, SubListLength, BoxAlpha);
            DrawListBottom(sb, checkHoverBottom);

            DrawBottomVerticalSlider(sb);

            return checkHoverTop || checkHoverTabs || checkHoverMiddle || checkHoverBottom;

        }

        private static void DrawTreeOutline(SpriteBatch sb) {

            int width = 4;
            int height = 17 + 4;

            Point origin = new Point(Neo.TileTargetX, Neo.TileTargetY);

            while (origin.Y < Main.maxTilesY && (!Main.tile[origin.X, origin.Y].active() || (Main.tile[origin.X, origin.Y].active() && Main.tileCut[Main.tile[origin.X, origin.Y].type])))
                origin.Y++;

            if (Main.tile[origin.X, origin.Y].type == TileID.JungleGrass)
                height += 5;

            Vector2 topLeft = new Vector2((origin.X - (width / 2)) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 topRight = new Vector2((origin.X + (width / 2) + 1) * 16 - Main.screenPosition.X, (origin.Y - (height - 1)) * 16 - Main.screenPosition.Y);
            Vector2 botomLeft = new Vector2((origin.X - (width / 2)) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);
            Vector2 botomRight = new Vector2((origin.X + (width / 2) + 1) * 16 - Main.screenPosition.X, (origin.Y) * 16 - Main.screenPosition.Y);

            bool canPlace = true;
            string mouseText = "";

            if (!Main.tile[origin.X, origin.Y].nactive()) {
                canPlace = false;
                mouseText = "Start Tile is nActive.";
            }
            else if (!Main.tile[origin.X - 1, origin.Y].active() &&
                     !Main.tile[origin.X + 1, origin.Y].active()) {
                canPlace = false;
                mouseText = "Ground is not wide enough.";
            }
            else if (Main.tile[origin.X, origin.Y].halfBrick() ||
                     Main.tile[origin.X, origin.Y].slope() != 0) {
                canPlace = false;
                mouseText = "Ground is not level.";
            }
            else if (Main.tile[origin.X, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X, origin.Y].type)) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (
                     (Main.tile[origin.X - 1, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X - 1, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X - 1, origin.Y].type))
                     &&
                     (Main.tile[origin.X + 1, origin.Y].type != TileID.Grass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.CorruptGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.JungleGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.MushroomGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.HallowedGrass &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.SnowBlock &&
                     Main.tile[origin.X + 1, origin.Y].type != TileID.FleshGrass &&
                     !TileLoader.CanGrowModTree(Main.tile[origin.X + 1, origin.Y].type))) {
                canPlace = false;
                mouseText = "Ground tile cannot grow a Tree.";
            }
            else if (!EmptyTileCheck(origin.X - 2, origin.X + 2, origin.Y - height, origin.Y - 1, 20, true)) {
                canPlace = false;
                mouseText = "Empty Tile Check Failed.";
            }

            Color boxColor = canPlace ? Color.Green : Color.Red;

            sb.DrawLine(topLeft, topRight, boxColor, 2);
            sb.DrawLine(topLeft, botomLeft, boxColor, 2);
            sb.DrawLine(botomLeft, botomRight, boxColor, 2);
            sb.DrawLine(topRight, botomRight, boxColor, 2);

            if (mouseText != "")
                MouseText(sb, mouseText);

        }

        private static int GetHash(int a, int b) {

            a = a >= 0 ? 2 * a : -2 * a - 1;
            b = b >= 0 ? 2 * b : -2 * b - 1;

            return a >= b ? a * a + a + b : a + b * b;

        }

        private static int GetHash(Point point) {

            return GetHash(point.X, point.Y);

        }

        private static List<Point> GetLinePoints(bool supercover = false) {

            Point target = new Point(Neo.TileTargetX, Neo.TileTargetY);

            float angle = MathHelper.ToDegrees((float)Math.Atan2(target.Y - StartPoint.Y, target.X - StartPoint.X));

            if (Main.keyState.PressingCtrl()) {

                if (angle > -22.5 && angle < 22.5) {

                    angle = 0;
                    target.Y = (int)StartPoint.Y;

                }
                else if (angle <= -22.5 && angle >= -67.5) {

                    angle = -45;

                    int deltaX = Math.Abs(target.X - (int)StartPoint.X);
                    int deltaY = Math.Abs(target.Y - (int)StartPoint.Y);

                    if (deltaX < deltaY) {
                        target.X += (deltaY - deltaX);
                    }
                    else if (deltaY < deltaX) {
                        target.Y -= (deltaX - deltaY);
                    }

                }
                else if (angle >= 22.5 && angle <= 67.5) {

                    angle = 45;

                    int deltaX = Math.Abs(target.X - (int)StartPoint.X);
                    int deltaY = Math.Abs(target.Y - (int)StartPoint.Y);

                    if (deltaX < deltaY) {
                        target.X += (deltaY - deltaX);
                    }
                    else if (deltaY < deltaX) {
                        target.Y += (deltaX - deltaY);
                    }

                }
                else if (angle > 67.5 && angle < 112.5) {

                    angle = 90;
                    target.X = (int)StartPoint.X;

                }
                else if (angle < -67.5 && angle > -112.5) {

                    angle = -90;
                    target.X = (int)StartPoint.X;

                }
                else if (angle >= 112.5 && angle <= 157.5) {

                    angle = 135;

                    int deltaX = Math.Abs(target.X - (int)StartPoint.X);
                    int deltaY = Math.Abs(target.Y - (int)StartPoint.Y);

                    if (deltaX < deltaY) {
                        target.X -= (deltaY - deltaX);
                    }
                    else if (deltaY < deltaX) {
                        target.Y += (deltaX - deltaY);
                    }

                }
                else if (angle <= -112.5 && angle >= -157.5) {

                    angle = -135;

                    int deltaX = Math.Abs(target.X - (int)StartPoint.X);
                    int deltaY = Math.Abs(target.Y - (int)StartPoint.Y);

                    if (deltaX < deltaY) {
                        target.X -= (deltaY - deltaX);
                    }
                    else if (deltaY < deltaX) {
                        target.Y -= (deltaX - deltaY);
                    }

                }
                else if (angle > 157.5 || angle < -157.5) {

                    angle = -180;
                    target.Y = (int)StartPoint.Y;

                }

            }

            List<Point> points = new List<Point>();

            for (int j = 0; j < Math.Ceiling((BrushSize + 1) / 2f); j++) {

                if (angle >= -45 && angle < 45) {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X, (int)StartPoint.Y + j, target.X, target.Y + j));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X, (int)StartPoint.Y + j, target.X, target.Y + j));
                    }
                    
                }
                else if (angle <= -135 || angle >= 135) {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X, (int)StartPoint.Y + j, target.X, target.Y + j));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X, (int)StartPoint.Y + j, target.X, target.Y + j));
                    }
                    
                }
                else {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X + j, (int)StartPoint.Y, target.X + j, target.Y));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X + j, (int)StartPoint.Y, target.X + j, target.Y));
                    }
                    
                }

            }

            for (int k = 0; k < Math.Floor((BrushSize + 1) / 2f); k++) {

                if (k == 0)
                    continue;

                if (angle >= -45 && angle < 45) {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X, (int)StartPoint.Y - k, target.X, target.Y - k));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X, (int)StartPoint.Y - k, target.X, target.Y - k));
                    }
                    
                }
                else if (angle <= -135 || angle >= 135) {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X, (int)StartPoint.Y - k, target.X, target.Y - k));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X, (int)StartPoint.Y - k, target.X, target.Y - k));
                    }
                    
                }
                else {

                    if (supercover) {
                        points.AddRange(BresenhamLineSuperCover((int)StartPoint.X - k, (int)StartPoint.Y, target.X - k, target.Y));
                    }
                    else {
                        points.AddRange(BresenhamLine((int)StartPoint.X - k, (int)StartPoint.Y, target.X - k, target.Y));
                    }
                    
                }

            }

            return points;

        }

        private static void GetSearchText() {

            if (Keys.Enter.Pressed() && DateTime.Now > KeyPressDelay) {

                Main.GetInputText("");

                if (SearchBoxText == null) {

                    SearchBoxText = "";

                }
                else {

                    SearchString = SearchBoxText;

                    if (SearchString == "")
                        SearchString = null;

                    SearchBoxText = null;

                }

                Searching = false;

            }
            else if (SearchBoxText != null) {

                PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                SearchBoxText = Main.GetInputText(SearchBoxText);

            }

        }

        private static Tuple<List<Point>, Vector4> GetSelectedPoints() {

            if (StartPoint == default)
                return new Tuple<List<Point>, Vector4>(new List<Point>(), default);

            int minX = Math.Min(Neo.TileTargetX, (int)StartPoint.X);
            int maxX = Math.Max(Neo.TileTargetX, (int)StartPoint.X);
            int minY = Math.Min(Neo.TileTargetY, (int)StartPoint.Y);
            int maxY = Math.Max(Neo.TileTargetY, (int)StartPoint.Y);

            List<Point> points = new List<Point>();

            for (int l = minY; l <= maxY; l++)
                for (int m = minX; m <= maxX; m++)
                    points.Add(new Point(m, l));

            return new Tuple<List<Point>, Vector4>(points, new Vector4(minX, maxX, minY, maxY));

        }

        private static TOD GetTileData(int tileToCreate) {

            TOD tod = TOD.GetTileData(tileToCreate, _placeStyle);

            if (tod == null) {

                switch (tileToCreate) {

                    case TileID.Heart:
                    case TileID.Pots:
                    case TileID.ShadowOrbs:
                    case TileID.PlanteraBulb:
                    case TileID.LifeFruit: {
                            tod = TOD.Style2x2;
                            break;
                        }
                    case TileID.Plants2:
                    case TileID.JunglePlants2:
                    case TileID.HallowedPlants2: {
                            tod = TOD.Style1x2;
                            break;
                        }
                    default: {
                            tod = TOD.Style1x1;
                            break;
                        }

                }

            }

            return tod;

        }

        private static void HandleClickLogic() {

            if (!Main.hasFocus)
                return;

            if (Main.mouseLeft) {

                if (LeftClickHeld) {
                    if (LeftClickHoldTime < 250)
                        LeftClickHoldTime++;
                }
                else {
                    LeftClickHeld = true;
                    LeftClickHoldTime = 0;
                }

            }
            else {

                LeftClickHeld = false;

            }

            if (Main.mouseRight) {

                if (RightClickHeld) {
                    if (RightClickHoldTime < 250)
                        RightClickHoldTime++;
                }
                else {
                    RightClickHeld = true;
                    RightClickHoldTime = 0;
                }

            }
            else {

                RightClickHeld = false;

            }

            LeftClicked = !Main.mouseLeft && !Main.mouseLeftRelease && LeftClickHoldTime < DragDelay;

            LeftDragging = Main.mouseLeft && !Main.mouseLeftRelease && LeftClickHoldTime >= DragDelay;

            LeftLongClick = !Main.mouseLeft && !Main.mouseLeftRelease && LeftClickHoldTime >= DragDelay;

            RightClicked = !Main.mouseRight && !Main.mouseRightRelease && RightClickHoldTime < DragDelay;

            RightDragging = Main.mouseRight && !Main.mouseRightRelease && RightClickHoldTime >= DragDelay;

            RightLongClick = !Main.mouseRight && !Main.mouseRightRelease && RightClickHoldTime >= DragDelay;

        }

        private static void HandleTimeChange() {

            if (DayNightOption == DayNight.MakeDay) {
                Main.dayTime = true;
                Main.time = 27000;
            }
            else if (DayNightOption == DayNight.MakeNight) {
                Main.dayTime = false;
                Main.time = 16200;
            }
            else {
                Main.dayTime = NeoDraw.OldDayTime;
                Main.time = NeoDraw.OldTime;
            }

        }

        private static void HighlightInvalidTiles(SpriteBatch sb) {

            if (InvalidTiles == null || InvalidTiles.Count < 1)
                return;

            List<Tuple<Point, int>> tempList = new List<Tuple<Point, int>>();

            foreach (Tuple<Point, int> invalidTile in InvalidTiles) {

                Point tileLocation = invalidTile.Item1;
                int showCount = invalidTile.Item2;

                Drawing.DrawBox(sb, tileLocation.X * 16f - Main.screenPosition.X - 16, tileLocation.Y * 16f - Main.screenPosition.Y - 16, 48, 48, Color.Red * 0.25f);
                Drawing.DrawBox(sb, tileLocation.X * 16f - Main.screenPosition.X, tileLocation.Y * 16f - Main.screenPosition.Y, 16, 16, Color.Red * 0.85f);

                showCount--;

                if (showCount > 0)
                    tempList.Add(new Tuple<Point, int>(tileLocation, showCount));

            }

            InvalidTiles = tempList;

        }

        private static void LivingTreeOutline(SpriteBatch sb) {

        }

        public static void MouseText(SpriteBatch sb, string message, Color baseColor = default) {

            if (baseColor == default)
                baseColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);

            MouseText(sb, new[] { new TextSnippet(message, baseColor) });

        }

        public static void MouseText(SpriteBatch sb, TextSnippet[] snippets) {

            if (Main.MouseScreen.X < 0 || Main.MouseScreen.Y < 0 || Main.MouseScreen.X > Main.screenWidth || Main.MouseScreen.Y > Main.screenHeight)
                return;

            bool quickSwapped = false;

            if (SwitchedToGameZoom) {

                SwitchToUIZoom();
                quickSwapped = true;

            }

            Vector2 stringSize = ChatManager.GetStringSize(Main.fontMouseText, snippets, Vector2.One);

            float offsetX = 0;
            float offsetY = 0;

            float screenRightSide = Main.screenPosition.X + Main.screenWidth;
            float screenBottom = Main.screenPosition.Y + Main.screenHeight;

            if (Main.MouseWorld.X + TextBoxMouseOffsetX + TextBoxSidePadding * 2 + (int)stringSize.X + 5 > screenRightSide)
                offsetX = screenRightSide - (Main.MouseWorld.X + TextBoxMouseOffsetX + TextBoxSidePadding * 2 + (int)stringSize.X + 5);

            if (Main.MouseWorld.Y + TextBoxMouseOffsetY + TextBoxTopPadding + (int)stringSize.Y + 5 > screenBottom)
                offsetY = screenBottom - (Main.MouseWorld.Y + TextBoxMouseOffsetY + TextBoxTopPadding + (int)stringSize.Y + 5);

            float boxPosX = Main.mouseX + TextBoxMouseOffsetX + offsetX;
            float boxPosY = Main.mouseY + TextBoxMouseOffsetY + offsetY;

            Drawing.DrawBox(sb, boxPosX, boxPosY, (int)stringSize.X + TextBoxSidePadding * 2, (int)stringSize.Y + TextBoxTopPadding);

            ChatManager.DrawColorCodedStringWithShadow(sb, Main.fontMouseText, snippets, new Vector2(boxPosX + TextBoxSidePadding, boxPosY + TextBoxTopPadding), 0f, Vector2.Zero, Vector2.One, out int _);

            if (quickSwapped)
                SwitchToGameZoom();

        }

        public static void PasteSelection() {

            if (CurrentSelection == null || CurrentSelection.Count < 1)
                return;

            Pasting = true;

            SetStatusBarTempMessage("Paste", 50);

        }

        private static void Refresh(bool resetScroll) {

            if (resetScroll) {

                switch (CurrentTab) {

                    case Tabs.Tiles: {

                            _tileScroll = 0;
                            break;

                        }
                    case Tabs.Walls: {

                            _wallScroll = 0;
                            break;

                        }
                    case Tabs.Structures: {

                            _structureScroll = 0;
                            break;

                        }
                    case Tabs.Other: {

                            _otherScroll = 0;
                            break;

                        }

                }

            }

            RunFilters();

        }

        public static void Reset() {

            //DayNightOption = 0;
            TileFrameY = 0;

            Pasting = false;
            Searching = false;
            SwitchedToEyedropper = false;
            ShowDebugRegenDialog = false;
            ShowMapResetDialog = false;
            TileFramingTheWorld = false;

            SearchBoxText = null;
            SearchString = null;

            for (int i = 0; i < CurrentSorter.Length; i++)
                CurrentSorter[i] = 0;

        }

        private static void RunFilters() {

            if (ListFiltered == null) {
                ListFiltered = new List<ListItem>();
            }
            else {
                ListFiltered.Clear();
            }

            List<ListItem> listSource;

            switch (CurrentTab) {

                case Tabs.Tiles:
                    listSource = NeoDraw.TilesList;
                    break;

                case Tabs.Walls:
                    listSource = NeoDraw.WallsList;
                    break;

                case Tabs.Structures:
                    listSource = StructuresList;
                    break;

                case Tabs.Other:
                    listSource = OthersList;
                    break;

                default:
                    return;

            }

            foreach (ListItem listItem in listSource) {

                try {

                    if ((SearchBoxText != null || SearchString != null) && listItem.Name.ToLower().IndexOf((SearchBoxText ?? SearchString).ToLower()) == -1)
                        continue;

                    if (!ListSorters[CurrentSorter[CurrentTab]].Allow(listItem))
                        continue;

                    ListFiltered.Add(listItem);

                }
                catch (Exception) { }

            }

            ListFiltered.Sort(ListSorters[CurrentSorter[CurrentTab]]);

            if (ReverseSortList)
                ListFiltered.Reverse();

        }

        private static void SetSpawnPlayer(int x, int y) {

            if (MouseOffScreen)
                return;

            if (Player.CheckSpawn(x, y)) {

                Main.LocalPlayer.RemoveSpawn();
                Main.LocalPlayer.ChangeSpawn(x, y);

            }

        }

        private static bool SetSpawnWorld(int x, int y) {

            if (MouseOffScreen)
                return false;

            Vector2 vector = new Vector2(x, y) * 16f + new Vector2(-Main.LocalPlayer.width / 2f + 8f, -Main.LocalPlayer.height);

            int xPos = x;
            int yPos = y;

            if (Collision.SolidCollision(vector, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("Not enough room to spawn here.");
                return false;

            }

            int i = 0;

            while (i < 20) {

                if (Main.tile[xPos, yPos + i] == null)
                    Main.tile[xPos, yPos + i] = new Tile();

                vector = new Vector2(xPos, (yPos + i)) * 16f + new Vector2(-Main.LocalPlayer.width / 2f + 8f, -Main.LocalPlayer.height);

                Collision.SlopeCollision(vector, Main.LocalPlayer.velocity, Main.LocalPlayer.width, Main.LocalPlayer.height, Main.LocalPlayer.gravDir);

                bool flag2 = !Collision.SolidCollision(vector, Main.LocalPlayer.width, Main.LocalPlayer.height);

                if (!flag2)
                    if (Main.tile[xPos, yPos + i].active() && !Main.tile[xPos, yPos + i].inActive() && Main.tileSolid[Main.tile[xPos, yPos + i].type])
                        break;

                i++;

            }

            if (i >= 20) {

                SetStatusBarTempMessage("No solid ground to spawn on.");
                return false;

            }

            if (Collision.LavaCollision(vector, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("Cannot set spawn in lava.");
                return false;

            }

            if (Collision.HurtTiles(vector, Main.LocalPlayer.velocity, Main.LocalPlayer.width, Main.LocalPlayer.height).Y > 0f) { // TODO: Fix so you can set on Sand

                SetStatusBarTempMessage("Cannot set spawn above dangerous terrain.");
                return false;

            }

            Collision.SlopeCollision(vector, Main.LocalPlayer.velocity, Main.LocalPlayer.width, Main.LocalPlayer.height, Main.LocalPlayer.gravDir);

            if (!Collision.SolidCollision(vector, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("No solid ground to spawn on.");
                return false;

            }

            yPos += i;

            Main.spawnTileX = x;
            Main.spawnTileY = yPos - 1;
            SetStatusBarTempMessage("World spawn point set!");

            return true;

        }

        public static void SetStatusBarTempMessage(string message, int timer = 150) {

            statusBarTempMessage = message;
            statusBarTempMessageTimer = timer;

        }

        public static void SetUndo(UndoStep undo) {
            _undo = undo;
        }

        private static void ShowPasteHighlight(SpriteBatch sb) {

            if (CurrentSelection == null || CurrentSelection.Count < 1) {
                Pasting = false;
                return;
            }

            IList<Point> points = CurrentSelection.GetChangedTilesList();

            for (int i = 0; i < CurrentSelection.Count; i++)
                sb.Draw(Square, new Vector2((points[i].X + Neo.TileTargetX) * 16 - Main.screenPosition.X, (points[i].Y + Neo.TileTargetY) * 16 - Main.screenPosition.Y), Color.Yellow * 0.5f);

        }

        private static void ShowRuler(SpriteBatch sb) {

            if (Main.mouseX > Main.screenWidth || Main.mouseX < 0 || Main.mouseY < 0 || Main.mouseY > Main.screenHeight)
                return;

            PlayerInput.SetZoom_MouseInWorld();
            
            int startTileX = (int)RulerStartPoint.X;
            int startTileY = (int)RulerStartPoint.Y;

            Vector2 targetTile = Main.MouseWorld;

            targetTile /= 16f;

            int width = (int)targetTile.X - startTileX;
            int height = (int)targetTile.Y - startTileY;

            int widthCount = Math.Abs(width) + 1;
            int heightCount = Math.Abs(height) + 1;

            Texture2D texture = GetTexture("Terraria/Extra_2");

            Rectangle sourceRectangle = new Rectangle(0, 0, 16, 16);

            int currentTileX = startTileX;
            int currentTileY = startTileY;

            float r = 0.24f;
            float g = 0.8f;
            float b = 0.9f;
            float a = 1f;
            float scale2 = 0.8f;

            Color color = new Color(r, g, b, a) * scale2;

            Vector2 screenPosition = new Vector2(currentTileX, currentTileY) * 16f - Main.screenPosition;

            sb.Draw(texture, Main.ReverseGravitySupport(screenPosition, 16f), sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            if (width != 0) {

                int num13 = Math.Sign(width);
                sourceRectangle.Y = ((num13 == 1) ? 16 : 32);

                while (width != 0) {

                    width -= num13;
                    currentTileX += num13;

                    if (width == 0)
                        sourceRectangle.Y = 0;

                    color = new Color(r, g, b, a) * scale2;

                    sb.Draw(texture, Main.ReverseGravitySupport(new Vector2(currentTileX, currentTileY) * 16f - Main.screenPosition, 16f), sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                }

            }

            if (height != 0) {

                int num14 = Math.Sign(height);
                sourceRectangle.Y = ((num14 == 1) ? 48 : 64);

                while (height != 0) {

                    height -= num14;
                    currentTileY += num14;

                    if (height == 0)
                        sourceRectangle.Y = 0;

                    color = new Color(r, g, b, a) * scale2;

                    sb.Draw(texture, Main.ReverseGravitySupport(new Vector2(currentTileX, currentTileY) * 16f - Main.screenPosition, 16f), sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                }

            }

            PlayerInput.SetZoom_UI();

            bool switched = false;

            if (SwitchedToGameZoom) {
                SwitchToUIZoom();
                switched = true;
            }

            Utils.DrawBorderString(sb, widthCount + "x" + heightCount, new Vector2(Main.mouseX + 16, Main.mouseY), new Color(r, g, b, a), 1f, 0f, 0.8f);

            if (switched)
                SwitchToGameZoom();

        }

        private static void ShowSpinningRing(SpriteBatch sb) {

            Texture2D value = GetTexture("Terraria/Extra_199");

            int frameY = (int)(Main.GlobalTime * 10f) % 4;

            Rectangle rectangle = new Rectangle(value.Width / 1 * 0, value.Height / 4 * frameY, value.Width / 1 + 0, value.Height / 4 - 2);

            Vector2 origin = rectangle.Size() / 2f;

            Color color = Color.White * 0.7f;

            color.A /= 2;

            sb.Draw(value, Main.MouseScreen, rectangle, color, 0f, origin, 1f, SpriteEffects.None, 0f);

        }

        public static void StartSearchBarTyping() {

            if (Searching)
                return;

            AddKeyPressDelay();
            _searchBar.Clicked(_searchBar, 0);

        }

        private static void Swap<T>(ref T a, ref T b) {
            T c = a;
            a = b;
            b = c;
        }

        private static void SwitchToGameZoom() {

            SwitchedToGameZoom = true;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

        }

        public static void SwitchToMouseInUIZoom() {

            SwitchedToMouseInWorldZoom = false;
            PlayerInput.SetZoom_UI();

        }

        public static void SwitchToMouseInWorldZoom() {

            SwitchedToMouseInWorldZoom = true;
            PlayerInput.SetZoom_MouseInWorld();

        }

        private static void SwitchToUIZoom() {

            SwitchedToGameZoom = false;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);

        }

        private static void Teleport(Vector2 newPos, int style = 0) {

            try {

                Main.LocalPlayer.grappling[0] = -1;
                Main.LocalPlayer.grapCount = 0;

                for (int i = 0; i < 1000; i++)
                    if (Main.projectile[i].active && Main.projectile[i].owner == Main.LocalPlayer.whoAmI && Main.projectile[i].aiStyle == 7)
                        Main.projectile[i].Kill();

                Main.LocalPlayer.position = newPos;
                Main.LocalPlayer.fallStart = (int)(Main.LocalPlayer.position.Y / 16f);

                if (Main.LocalPlayer.whoAmI == Main.myPlayer) {

                    if (Main.mapTime < 5)
                        Main.mapTime = 5;

                    Main.quickBG = 10;
                    Main.maxQ = true;
                    Main.renderNow = true;

                }
                
            }
            catch { }

        }

        private static void TileFrameTheWorld() {

            if (!TileFramingTheWorld)
                return;

            if (TileFrameY >= Main.maxTilesY) {
                TileFramingTheWorld = false;
                return;
            }

            float progress = TileFrameY / Main.maxTilesY * 100;

            SetStatusBarTempMessage($"Progress: {progress:P1}");

            for (int x = 0; x < Main.maxTilesX; x++)
                WorldGen.TileFrame(x, TileFrameY);

            TileFrameY++;

        }

        public static bool TryTeleport() {

            if (MouseOffScreen)
                return false;

            Vector2 targetPosition = Neo.TileTarget_Vector * 16f + new Vector2(-Main.LocalPlayer.width / 2f + 8f, -Main.LocalPlayer.height);

            int xPos = Neo.TileTargetX;
            int yPos = Neo.TileTargetY;

            if (Collision.SolidCollision(targetPosition, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("Not enough room to teleport to here.");
                return false;

            }

            int i = 0;

            while (i < 20) {

                if (Main.tile[xPos, yPos + i] == null)
                    Main.tile[xPos, yPos + i] = new Tile();

                targetPosition = new Vector2(xPos, yPos + i) * 16f + new Vector2(-Main.LocalPlayer.width / 2f + 8f, -Main.LocalPlayer.height);

                bool flag2 = !Collision.SolidCollision(targetPosition, Main.LocalPlayer.width, Main.LocalPlayer.height);

                if (!flag2)
                    if (Main.tile[xPos, yPos + i].active() && !Main.tile[xPos, yPos + i].inActive() && Main.tileSolid[Main.tile[xPos, yPos + i].type])
                        break;

                i++;

            }

            if (i >= 20) {

                SetStatusBarTempMessage("No solid ground to land on.");
                return false;

            }

            if (Collision.LavaCollision(targetPosition, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("Cannot teleport into lava.");
                return false;

            }

            if (HurtTiles(targetPosition, Main.LocalPlayer.velocity, Main.LocalPlayer.width, Main.LocalPlayer.height).Y > 0f) {

                SetStatusBarTempMessage("Cannot teleport onto dangerous terrain.");
                return false;

            }

            if (!Collision.SolidCollision(targetPosition, Main.LocalPlayer.width, Main.LocalPlayer.height)) {

                SetStatusBarTempMessage("No solid ground to land on.");
                return false;

            }

            Teleport(targetPosition, 5);
            Main.LocalPlayer.velocity = Vector2.Zero;

            if (Main.netMode == NetmodeID.Server) {

                RemoteClient.CheckSection(Main.LocalPlayer.whoAmI, Main.LocalPlayer.position);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.LocalPlayer.whoAmI, targetPosition.X, targetPosition.Y, 3);
            }

            return true;

        }

        private static void UpdatePositions() {

            CreateSliders();
            CreateButtons();

        }

        private static void UpdateSearchBar() {

            if (Searching) {

                _searchAppendCounter++;

                if (_searchAppendCounter == 32) {
                    _searchAppend = "";
                }
                else if (_searchAppendCounter == 64) {
                    _searchAppend = "|";
                    _searchAppendCounter = 0;
                }

                GetSearchText();

                if ((Main.mouseLeft || Main.mouseRight) && !(new Rectangle((int)_searchBar.Position.X + ToolbarXoffset, (int)_searchBar.Position.Y + ToolbarYoffset, (int)_searchBar.Size.X, (int)_searchBar.Size.Y).Contains(Main.MouseScreen.ToPoint()))) {

                    Searching = false;

                    if (!string.IsNullOrWhiteSpace(SearchBoxText))
                        SearchString = SearchBoxText;

                    SearchBoxText = null;

                }

            }
            else {
                CheckPressedKeys();
            }

        }

        private static void UpdateSelectionFrame() {

            if (FrameCounterUpdated)
                return;

            FrameCounterUpdated = true;

            SelectionFrameCounter--;

            if (SelectionFrameCounter <= 0) {

                SelectionFrameCounter = SelectionFrameCounterMax;

                SelectionFrameCurrent++;

                if (SelectionFrameCurrent >= SelectionFrameCount)
                    SelectionFrameCurrent = 0;

            }

        }

        #endregion

    }

    struct DayNight {

        public const byte Default   = 0;
        public const byte MakeDay   = 1;
        public const byte MakeNight = 2;

    }

    struct Mech {

        public const byte Actuator = 0;
        public const byte BlueWire = 1;
        public const byte GreenWire = 2;
        public const byte RedWire = 3;
        public const byte YellowWire = 4;
        public const byte Wire = 3;
        public const byte Wire2 = 1;
        public const byte Wire3 = 2;
        public const byte Wire4 = 4;

    }

}

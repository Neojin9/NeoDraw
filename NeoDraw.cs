using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Terraria.Map;

namespace NeoDraw {

	public class NeoDraw : Mod {

        #region Variables

        internal DrawInterface DrawInterface; // <--- UIState

        private const byte DoubleClickDelay = 25;

        #region Private Variables

        private static byte oldSelectedItem;

        private static int oldDayRate;

        private static bool oldAutoPause;
        private static bool oldEditSign;
        private static bool oldMapEnabled;
        private static bool oldSmartCursorEnabled;

        private static byte DoubleClickCounter;

        private static float oldGameZoom;

        private static int[] oldBuilderAccStatus;

        private static UserInterface drawInterface; // <--- UserInterface

        public static WorldMap oldWorldMap;

        #endregion Private Variables

        #region Public Variables

        public static bool AtLeftEdgeOfWorld;
        public static bool AtRightEdgeOfWorld;
        public static bool DrawMode;
        public static bool ForceCursorToShow;
        public static bool GridView;
        public static bool OldDayTime;

        public static byte BrightMode;
        public static byte MaxUndoCount = 10;

        public static double OldTime;

        public static int CurrentTab;

        public static int? OtherToCreate;
        public static int? StructureToCreate;
        public static int? TileToCreate;
        public static int? WallToCreate;

        public static List<ListItem> TilesList;
        public static List<ListItem> WallsList;
        public static List<string> TileNames;
        public static List<string> WallNames;

        public static ModHotKey CameraUp;
        public static ModHotKey CameraDown;
        public static ModHotKey CameraLeft;
        public static ModHotKey CameraRight;
        public static ModHotKey Eyedropper;
        public static ModHotKey FindPlayer;
        public static ModHotKey PauseGame;
        public static ModHotKey TeleportPlayer;
        public static ModHotKey ToggleDrawMode;
        public static ModHotKey UndoRedo;

        public static UndoManager UndoManager;

        public static bool MouseXoverToolbar => (!AtLeftEdgeOfWorld && Main.mouseX <= DrawInterface.ListWidth) || (AtLeftEdgeOfWorld && Main.mouseX >= Main.screenWidth - DrawInterface.ListWidth);

        #endregion

        #endregion

        #region Override Functions

        public override void Close() {

            if (DrawMode)
                ExitDrawMode();

        }

        public override void Load() {

            SetupLists();
            WldGen.WldGen.SetupHellChest();

            UndoManager = new UndoManager(MaxUndoCount);

            if (Main.netMode == NetmodeID.Server || Main.dedServ)
                return;

            ToggleDrawMode = RegisterHotKey("Draw Mode", "F5");
            CameraUp       = RegisterHotKey("Camera Up", "W");
            CameraDown     = RegisterHotKey("Camera Down", "S");
            CameraLeft     = RegisterHotKey("Camera Left", "A");
            CameraRight    = RegisterHotKey("Camera Right", "D");
            UndoRedo       = RegisterHotKey("Undo/Redo", "Z");
            Eyedropper     = RegisterHotKey("Quick Eyedropper", "LeftShift");
            FindPlayer     = RegisterHotKey("Find Player", "Home");
            PauseGame      = RegisterHotKey("Pause Game", "Pause");
            TeleportPlayer = RegisterHotKey("Teleport", "Insert");

            DrawInterface = new DrawInterface();
            DrawInterface.Activate();

            drawInterface = new UserInterface();
            drawInterface.SetState(DrawInterface);

        }
        
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {

            if (!DrawMode || Main.playerInventory)
                return;

            AtLeftEdgeOfWorld  = Main.screenPosition.X < 900;
            AtRightEdgeOfWorld = Main.screenPosition.X > Main.rightWorld - Main.screenWidth - 900;

            foreach (GameInterfaceLayer layer in layers) {

                bool undoHovered = false;
                bool redoHovered = false;

                if (Main.mouseY >= Main.screenHeight - 40 - 10 - (DrawInterface.ShowStatusBar ? 35 : 0) && Main.mouseY <= Main.screenHeight - 10 - (DrawInterface.ShowStatusBar ? 35 : 0)) {

                    if (Main.mouseX >= DrawInterface.ListWidth + 8.5 && Main.mouseX <= DrawInterface.ListWidth + 8.5 + 40)
                        undoHovered = true;

                    if (Main.mouseX >= DrawInterface.ListWidth + 8.5 * 2 + 40 && Main.mouseX <= DrawInterface.ListWidth + 8.5 * 2 + 40 * 2)
                        redoHovered = true;

                }

                if (layer.Name.Equals("Vanilla: Cursor")) {
                    
                    bool show = true;

                    if (!MouseXoverToolbar && !DrawInterface.MouseYoverStatusbar) {

                        switch (CurrentTab) {

                            case Tabs.Tiles: {
                                    if (TileToCreate.HasValue)
                                        show = false;
                                    break;
                                }
                            case Tabs.Walls: {
                                    if (WallToCreate.HasValue)
                                        show = false;
                                    break;
                                }
                            case Tabs.Structures: {
                                    if (StructureToCreate.HasValue)
                                        show = false;
                                    break;
                                }
                            case Tabs.Other: {
                                    if (OtherToCreate.HasValue)
                                        show = false;
                                    break;
                                }

                        }
                        
                        // Don't show if using Eyedropper
                        if (DrawInterface.CurrentPaintMode == PaintMode.Eyedropper || DrawInterface.SwitchedToEyedropper)
                            show = false;

                        if (DrawInterface.CurrentPaintMode != 0)
                            show = false;

                        if (undoHovered || redoHovered)
                            show = true;

                    }

                    if (show || ForceCursorToShow)
                        continue;

                }

                if (layer.Name.Equals("Vanilla: Tile Grid Option") && !MouseXoverToolbar && !DrawInterface.MouseYoverStatusbar && !undoHovered && !redoHovered)
                    continue;

                if (layer.Name.Equals("Vanilla: Player Chat") || 
                    layer.Name.Equals("Vanilla: Mouse Text") ||
                    layer.Name.Equals("Vanilla: Mouse Over"))
                    continue;

                layer.Active = false;

            }

            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1) {

                layers.Insert(mouseTextIndex + 1, new LegacyGameInterfaceLayer(

                    "NeoDraw: DrawInterface",

                    delegate {
                        
                        if (drawInterface?.CurrentState != null)
                            drawInterface.Draw(Main.spriteBatch, new GameTime());

                        return true;

                    },

                    InterfaceScaleType.UI)

                );

            }

        }

        public override void PostDrawFullscreenMap(ref string mouseText) {

            if (!DrawMode)
                return;

            if (Main.mapFullscreen) {

                DrawInterface.DrawStatusBar(Main.spriteBatch);

                if (DoubleClickCounter > 0)
                    DoubleClickCounter--;

                if (Main.mouseLeft && Main.mouseLeftRelease) {

                    if (DoubleClickCounter > 0) {
                        GrabMapTile();
                        DoubleClickCounter = 0;
                    }
                    else {
                        DoubleClickCounter = DoubleClickDelay;
                    }

                }

            }

        }

        public override void PreSaveAndQuit() {

            if (DrawMode)
                ExitDrawMode();

        }
        
        public override void PreUpdateEntities() {

            if (!DrawMode)
                return;

            Main.mapEnabled = oldMapEnabled;

        }

        public override void Unload() {

            ToggleDrawMode = null;
            CameraUp       = null;
            CameraDown     = null;
            CameraLeft     = null;
            CameraRight    = null;
            UndoRedo       = null;
            Eyedropper     = null;
            FindPlayer     = null;
            PauseGame      = null;
            TeleportPlayer = null;

            UndoManager = null;

        }

        public override void UpdateUI(GameTime gameTime) {

            if (!DrawMode)
                return;

            Main.mapEnabled = false;

        }

        #endregion

        #region Public Functions

        public static void ExitDrawMode() {

            if (DrawInterface.TileFramingTheWorld)
                return;

            DrawInterface.Reset();

            Liquid.quickSettle = true;

            Main.GameZoomTarget = oldGameZoom;
            Main.inFancyUI = oldEditSign;
            Main.dayRate   = oldDayRate;

            for (int x = 0; x < Main.maxTilesX; x++) {
                for (int y = 0; y < Main.maxTilesY; y++) {
                    MapTile newMapTile = MapTile.Create(oldWorldMap[x, y].Type, oldWorldMap[x, y].Light, oldWorldMap[x, y].Color);
                    Main.Map.SetTile(x, y, ref newMapTile);
                }
            }

            Main.clearMap   = true;
            Main.refreshMap = true;
            Main.updateMap  = true;

            Main.mapEnabled = oldMapEnabled;

            UndoManager.ClearHistory();
            UndoManager.ClearRedo();

            Main.time    = OldTime;
            Main.dayTime = OldDayTime;

            Main.autoPause = oldAutoPause;

            Main.LocalPlayer.selectedItem     = oldSelectedItem;
            Main.LocalPlayer.builderAccStatus = oldBuilderAccStatus;

            Main.SmartCursorEnabled = oldSmartCursorEnabled;

            Main.MouseShowBuildingGrid = false;
            
            ForceCursorToShow = false;

            NeoPlayer.ResetCameraPositionVector();

            DrawMode = false;

            Main.NewText("Draw Mode: Off");
            
        }

        public static void GrabMapTile() {

            if (!Main.mapEnabled || !Main.mapReady)
                return;

            float mapScale = Main.mapFullscreenScale;

            float minTileX = 10f;
            float minTileY = 10f;
            float maxTileX = Main.maxTilesX - 10;
            float maxTileY = Main.maxTilesY - 10;

            float minFullscreenScale = Main.screenWidth / (float)Main.maxTilesX * 0.8f;

            if (Main.mapFullscreenScale < minFullscreenScale)
                Main.mapFullscreenScale = minFullscreenScale;

            if (Main.mapFullscreenScale > 16f)
                Main.mapFullscreenScale = 16f;

            if (Main.mapFullscreenPos.X < minTileX)
                Main.mapFullscreenPos.X = minTileX;

            if (Main.mapFullscreenPos.X > maxTileX)
                Main.mapFullscreenPos.X = maxTileX;

            if (Main.mapFullscreenPos.Y < minTileY)
                Main.mapFullscreenPos.Y = minTileY;

            if (Main.mapFullscreenPos.Y > maxTileY)
                Main.mapFullscreenPos.Y = maxTileY;

            float mapScreenPosX = Main.mapFullscreenPos.X * mapScale;
            float mapScreenPosY = Main.mapFullscreenPos.Y * mapScale;

            float offsetX = 0f - mapScreenPosX + Main.screenWidth / 2;
            float offsetY = 0f - mapScreenPosY + Main.screenHeight / 2;

            offsetX += minTileX * mapScale;
            offsetY += minTileY * mapScale;

            int mapX = (int)((0f - offsetX + Main.mouseX) / mapScale + minTileX);
            int mapY = (int)((0f - offsetY + Main.mouseY) / mapScale + minTileY);

            if (mapX < minTileX || mapX >= maxTileX || mapY < minTileY || mapY >= maxTileY)
                return;

            Vector2 targetPosition = new Vector2(mapX * 16f, mapY * 16f);
            Vector2 centerScreen = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            Vector2 newScreenPos = targetPosition - centerScreen;

            if (newScreenPos.X < Main.leftWorld + 640 + 16)
                newScreenPos.X = Main.leftWorld + 640 + 16;

            if (newScreenPos.Y < Main.topWorld + 640 + 16)
                newScreenPos.Y = Main.topWorld + 640 + 16;

            if (newScreenPos.X + Main.screenWidth > Main.rightWorld - 640 - 32)
                newScreenPos.X = Main.rightWorld - Main.screenWidth - 640 - 32;

            if (newScreenPos.Y + Main.screenHeight > Main.bottomWorld - 640 - 32)
                newScreenPos.Y = Main.bottomWorld - Main.screenHeight - 640 - 32;

            NeoPlayer.SetLastCameraPosition(newScreenPos);

            Main.mapFullscreen = false;

            DrawInterface.AddClickDelay(500);

        }

        public static void OpenDrawMode() {

            Main.playerInventory = false;

            Liquid.quickSettle = true;

            oldGameZoom = Main.GameZoomTarget;
            Main.GameZoomTarget = 1f;

            oldEditSign = Main.inFancyUI;
            Main.inFancyUI = true;

            oldWorldMap = new WorldMap(Main.maxTilesX, Main.maxTilesY);
            oldMapEnabled = Main.mapEnabled;

            for (int x = 0; x < Main.maxTilesX; x++) {

                for (int y = 0; y < Main.maxTilesY; y++) {

                    MapTile oldMapTile = MapTile.Create(Main.Map[x, y].Type, Main.Map[x, y].Light, Main.Map[x, y].Color);
                    oldWorldMap.SetTile(x, y, ref oldMapTile);

                    Main.Map.UpdateLighting(x, y, byte.MaxValue);

                }

            }

            Main.updateMap  = true;
            Main.refreshMap = true;

            oldDayRate = Main.dayRate;
            Main.dayRate = 0;

            oldSelectedItem = (byte)Main.LocalPlayer.selectedItem;
            Main.LocalPlayer.selectedItem = 58;

            oldSmartCursorEnabled = Main.SmartCursorEnabled;
            Main.SmartCursorEnabled = false;

            oldBuilderAccStatus = Main.LocalPlayer.builderAccStatus;

            oldAutoPause = Main.autoPause;

            DrawInterface.WireStatus = -1;

            for (int index = 0; index < Main.LocalPlayer.builderAccStatus.Length; index++)
                Main.LocalPlayer.builderAccStatus[index] = index == 8 ? 1 : 0;

            OldTime    = Main.time;
            OldDayTime = Main.dayTime;

            Main.MouseShowBuildingGrid = true;

            NeoPlayer.ResetCameraPositionVector();
            NeoPlayer.SetLastCameraPosition(Main.screenPosition);

            DrawMode = true;

            DrawInterface.SetStatusBarTempMessage("Draw Mode: On");

        }

        public static void Reset() {

            CurrentTab = BrightMode = 0;
            DrawMode = GridView = false;
            OtherToCreate = StructureToCreate = TileToCreate = WallToCreate = null;

            UndoManager = new UndoManager(MaxUndoCount);

        }

        #endregion

        #region Private Functions

        private static void SetupLists() {

            Core.TileNames.Setup();
            Core.TileNames.SetupSubTiles();
            Core.WallNames.Setup();

            TilesList = new List<ListItem>();
            WallsList = new List<ListItem>();

            for (int i = 0; i < Core.TileNames.DisplayNames.Length; i++)
                TilesList.Add(new ListItem(i, Core.TileNames.DisplayNames[i]));

            for (int i = 0; i < Core.WallNames.DisplayNames.Length; i++)
                WallsList.Add(new ListItem(i, Core.WallNames.DisplayNames[i]));

        }

        #endregion

    }

}
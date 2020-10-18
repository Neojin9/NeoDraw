using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using TOD = Terraria.ObjectData.TileObjectData;

namespace NeoDraw.Core {

    [Flags]
    public enum TileDataType {

        Tile = 0x1,
        TilePaint = 0x2,
        Wall = 0x4,
        WallPaint = 0x8,
        Liquid = 0x10,
        Wiring = 0x20,
        Actuator = 0x40,
        Slope = 0x80,
        All = 0xFF

    }

    public class Neo {

        public static int TileTargetX => Utils.Clamp((int)((Main.mouseX + Main.screenPosition.X) / 16f), 0, Main.maxTilesX);

        public static int TileTargetY => Utils.Clamp(Main.LocalPlayer.gravDir == -1f ? (int)((Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16f) : (int)((Main.mouseY + Main.screenPosition.Y) / 16f), 0, Main.maxTilesY);

        public static int UnderworldLayer => Main.maxTilesY - 200;

        public static Tile TileTarget_Tile => Main.tile[TileTargetX, TileTargetY];

        public static Vector2 TileTarget_Vector => new Vector2(TileTargetX, TileTargetY);

        private static Color ColorBorder(int x, int y, int width, int height, int borderThickness, int borderRadius, int borderShadow, Color initialColor, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity) {

            Rectangle internalRectangle = new Rectangle((borderThickness + borderRadius), (borderThickness + borderRadius), width - 2 * (borderThickness + borderRadius), height - 2 * (borderThickness + borderRadius));

            if (internalRectangle.Contains(x, y))
                return initialColor;

            Vector2 origin = Vector2.Zero;
            Vector2 point  = new Vector2(x, y);

            if (x < borderThickness + borderRadius) {

                if (y < borderRadius + borderThickness) {
                    origin = new Vector2(borderRadius + borderThickness, borderRadius + borderThickness);
                }
                else if (y > height - (borderRadius + borderThickness)) {
                    origin = new Vector2(borderRadius + borderThickness, height - (borderRadius + borderThickness));
                }
                else {
                    origin = new Vector2(borderRadius + borderThickness, y);
                }

            }
            else if (x > width - (borderRadius + borderThickness)) {

                if (y < borderRadius + borderThickness) {
                    origin = new Vector2(width - (borderRadius + borderThickness), borderRadius + borderThickness);
                }
                else if (y > height - (borderRadius + borderThickness)) {
                    origin = new Vector2(width - (borderRadius + borderThickness), height - (borderRadius + borderThickness));
                }
                else {
                    origin = new Vector2(width - (borderRadius + borderThickness), y);
                }

            }
            else {

                if (y < borderRadius + borderThickness) {
                    origin = new Vector2(x, borderRadius + borderThickness);
                }
                else if (y > height - (borderRadius + borderThickness)) {
                    origin = new Vector2(x, height - (borderRadius + borderThickness));
                }

            }

            if (!origin.Equals(Vector2.Zero)) {

                float distance = Vector2.Distance(point, origin);

                if (distance > borderRadius + borderThickness + 1)
                    return Color.Transparent;

                if (distance > borderRadius + 1) {

                    if (borderColors.Count > 2) {

                        float modNum = distance - borderRadius;

                        if (modNum < borderThickness / 2)
                            return Color.Lerp(borderColors[2], borderColors[1], (float)((modNum) / (borderThickness / 2.0)));

                        return Color.Lerp(borderColors[1], borderColors[0], (float)((modNum - (borderThickness / 2.0)) / (borderThickness / 2.0)));

                    }


                    if (borderColors.Count > 0)
                        return borderColors[0];

                }
                else if (distance > borderRadius - borderShadow + 1) {

                    float mod = (distance - (borderRadius - borderShadow)) / borderShadow;
                    float shadowDiff = initialShadowIntensity - finalShadowIntensity;

                    return DarkenColor(initialColor, ((shadowDiff * mod) + finalShadowIntensity));

                }

            }

            return initialColor;

        }

        public static void ConvertTile(int x, int y, int conversionType, ref UndoStep undo) {

            if (!WorldGen.InWorld(x, y))
                return;

            Tile tile = Main.tile[x, y];

            if (tile == null)
                return;

            ushort type = tile.type;
            ushort wall = tile.wall;

            if (conversionType == 5) { // Jungle

                if (WallID.Sets.Conversion.Grass[wall] || WallID.Sets.Conversion.Stone[wall] || wall == WallID.MushroomUnsafe) {

                    if (y < Main.worldSurface + 3.0 - WorldGen.genRand.Next(3)) {

                        undo.Add(new ChangedTile(x, y));
                        tile.wall = Main.rand.Next(10) == 0 ? WallID.FlowerUnsafe : WallID.GrassUnsafe;
                        
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 1);

                    }
                    else if (y > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3)) {

                        undo.Add(new ChangedTile(x, y));
                        tile.wall = WallID.MudUnsafe;
                        
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 3);

                    }
                    else {

                        undo.Add(new ChangedTile(x, y));
                        tile.wall = WallID.JungleUnsafe;
                        
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 3);

                    }

                }

                if (type == TileID.Dirt) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Mud;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Stone[type] && type != TileID.Stone) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Stone;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if ((TileID.Sets.Conversion.Grass[type] || type == TileID.MushroomGrass) && type != TileID.JungleGrass) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.JungleGrass;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == TileID.Plants || type == TileID.CorruptPlants || type == TileID.HallowedPlants || type == TileID.FleshWeeds) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.JunglePlants;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == TileID.Vines || type == TileID.HallowedVines || type == TileID.CrimsonVines) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.JungleVines;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sand[type] && type != TileID.Sand) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Sand;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Thorn[type] && type != TileID.JungleThorns) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.JungleThorns;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 73 || type == 113) { // Tall Plants

                    undo.Add(new ChangedTile(x, y));
                    if (tile.frameX > (16 - 1) * 16)
                        tile.frameX = (short)(tile.frameX - 80);

                    tile.type = 74;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Ice[type] && type != TileID.IceBlock) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.IceBlock;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

            }
            else if (conversionType == 4) { // Crimson

                if (WallID.Sets.Conversion.Grass[wall] && wall != 81) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 81;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Stone[wall] && wall != 83) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 83;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 218) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 218;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 221) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 221;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if (TileID.Sets.Conversion.Grass[type] && type != 199) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 199;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if ((Main.tileMoss[type] || TileID.Sets.Conversion.Stone[type]) && type != 203) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 203;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Ice[type] && type != 200) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 200;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sand[type] && type != 234) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 234;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.HardenedSand[type] && type != 399) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 399;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sandstone[type] && type != 401) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 401;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Thorn[type] && type != 352) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 352;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if (type == 59 && (Main.tile[x - 1, y].type == 199 || Main.tile[x + 1, y].type == 199 || Main.tile[x, y - 1].type == 199 || Main.tile[x, y + 1].type == 199)) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 0;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

            }
            else if (conversionType == 2) { // Hallow

                if (WallID.Sets.Conversion.Grass[wall] && wall != 70) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 70;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Stone[wall] && wall != 28) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 28;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 219) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 219;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 222) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 222;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if ((Main.tileMoss[type] || TileID.Sets.Conversion.Stone[type]) && type != 117) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 117;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Grass[type] && type != 109) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 109;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sand[type] && type != TileID.Pearlsand) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Pearlsand;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Ice[type] && type != TileID.HallowedIce) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.HallowedIce;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.HardenedSand[type] && type != 402) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 402;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sandstone[type] && type != TileID.HallowSandstone) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.HallowSandstone;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Thorn[type]) {

                    undo.Add(new ChangedTile(x, y));
                    WorldGen.KillTile(x, y);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y);

                }

                if (type == 59 && (Main.tile[x - 1, y].type == 109 || Main.tile[x + 1, y].type == 109 || Main.tile[x, y - 1].type == 109 || Main.tile[x, y + 1].type == 109)) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 0;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

            }
            else if (conversionType == 1) { // Corruption

                if (WallID.Sets.Conversion.Grass[wall] && wall != 69) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 69;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Stone[wall] && wall != 3) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 3;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 217) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 217;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 220) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 220;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if (TileID.Sets.Conversion.Grass[type] && type != TileID.CorruptGrass) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.CorruptGrass;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if ((Main.tileMoss[type] || TileID.Sets.Conversion.Stone[type]) && type != TileID.Ebonstone) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Ebonstone;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sand[type] && type != TileID.Ebonsand) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Ebonsand;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Ice[type] && type != TileID.CorruptIce) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.CorruptIce;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.HardenedSand[type] && type != 398) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 398;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Sandstone[type] && type != 400) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 400;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (TileID.Sets.Conversion.Thorn[type] && type != 32) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 32;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if (type == 59 && (Main.tile[x - 1, y].type == 23 || Main.tile[x + 1, y].type == 23 || Main.tile[x, y - 1].type == 23 || Main.tile[x, y + 1].type == 23)) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 0;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

            }
            else if (conversionType == 3) {

                if (wall == 64 || wall == 15) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 80;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 3);

                }

                if (type == 60) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 70;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 3);

                }
                else if (TileID.Sets.Conversion.Thorn[type]) {

                    undo.Add(new ChangedTile(x, y));
                    WorldGen.KillTile(x, y);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y);

                }

            }
            else { // Purity

                if (wall == WallID.CorruptGrassUnsafe || wall == WallID.HallowedGrassUnsafe || wall == WallID.CrimsonGrassUnsafe) {

                    undo.Add(new ChangedTile(x, y));
                    if (y < Main.worldSurface) {
                        tile.wall = Main.rand.Next(10) == 0 ? WallID.FlowerUnsafe : WallID.GrassUnsafe;
                    }
                    else {
                        tile.wall = 64;
                    }

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (wall == 3 || wall == 28 || wall == 83) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 1;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (wall == 80) {

                    undo.Add(new ChangedTile(x, y));
                    if (y < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || y > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3)) {
                        tile.wall = 15;
                        
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 3);
                    }
                    else {
                        tile.wall = 64;
                        
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 3);
                    }

                }
                else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 216) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 216;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 187) {

                    undo.Add(new ChangedTile(x, y));
                    tile.wall = 187;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }

                if (type == TileID.CorruptGrass || type == TileID.JungleGrass || type == TileID.MushroomGrass || type == TileID.HallowedGrass || type == TileID.FleshGrass) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Grass;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 117 || type == 25 || type == 203) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Stone;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 112 || type == 116 || type == 234) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Sand;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 398 || type == 402 || type == 399) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 397;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 400 || type == 403 || type == 401) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 396;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 164 || type == 163 || type == 200) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = 161;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == TileID.Mud) {

                    undo.Add(new ChangedTile(x, y));
                    tile.type = TileID.Dirt;
                    
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, x, y, 1);

                }
                else if (type == 32 || type == 352) {

                    undo.Add(new ChangedTile(x, y));
                    WorldGen.KillTile(x, y);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, x, y);

                }

            }

        }

        public Texture2D CreateRoundedRectangleTexture(int width, int height, int borderThickness, int borderRadius, int borderShadow, List<Color> backgroundColors, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity) {

            #region ArgumentExceptions

            if (backgroundColors == null || backgroundColors.Count == 0)
                throw new ArgumentException("Must define at least one background color (up to four).");
            if (borderColors == null || borderColors.Count == 0)
                throw new ArgumentException("Must define at least one border color (up to three).");
            if (borderRadius < 1)
                throw new ArgumentException("Must define a border radius (rounds off edges).");
            if (borderThickness < 1)
                throw new ArgumentException("Must define border thikness.");
            if (borderThickness + borderRadius > height / 2 || borderThickness + borderRadius > width / 2)
                throw new ArgumentException("Border will be too thick and/or rounded to fit on the texture.");
            if (borderShadow > borderRadius)
                throw new ArgumentException("Border shadow must be lesser in magnitude than the border radius (suggeted: shadow <= 0.25 * radius).");

            #endregion ArgumentExceptions
            
            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] color = new Color[width * height];

            for (int x = 0; x < texture.Width; x++) {

                for (int y = 0; y < texture.Height; y++) {

                    switch (backgroundColors.Count) {

                        case 4:
                            Color leftColor0 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor0 = Color.Lerp(backgroundColors[2], backgroundColors[3], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor0, rightColor0, ((float)x / (width - 1)));
                            break;

                        case 3:
                            Color leftColor1 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor1 = Color.Lerp(backgroundColors[1], backgroundColors[2], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor1, rightColor1, ((float)x / (width - 1)));
                            break;

                        case 2:
                            color[x + width * y] = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)x / (width - 1)));
                            break;

                        default:
                            color[x + width * y] = backgroundColors[0];
                            break;

                    }

                    color[x + width * y] = ColorBorder(x, y, width, height, borderThickness, borderRadius, borderShadow, color[x + width * y], borderColors, initialShadowIntensity, finalShadowIntensity);

                }

            }

            texture.SetData(color);

            return texture;

        }

        public static Color DarkenColor(Color color, float shadowIntensity) => Color.Lerp(color, Color.Black, shadowIntensity);

        public static Point FindTopLeft(int x, int y) {

            if (!WorldGen.InWorld(x, y))
                return default;

            if (Main.tile[x, y] == null)
                return default;

            int tileType = Main.tile[x, y].type;

            if (!Main.tileFrameImportant[tileType])
                return new Point(x, y);

            TOD tileData = TOD.GetTileData(Main.tile[x, y]);

            if (tileData == null)
                return default;

            int fullWidth  = tileData.CoordinateFullWidth;
            int fullHeight = tileData.CoordinateFullHeight;

            int safetyCounter = 0;

            while (Main.tile[x, y].frameX % fullWidth != 0) {
                x--;
                safetyCounter++;
                if (safetyCounter > 10)
                    return default;
            }

            safetyCounter = 0;

            while (Main.tile[x, y].frameY % fullHeight != 0) {
                y--;
                safetyCounter++;
                if (safetyCounter > 10)
                    return default;
            }

            return new Point(x, y);

        }

        public static bool IsTopLeft(Point location) => IsTopLeft(location.X, location.Y);

        public static bool IsTopLeft(float x, float y) => IsTopLeft((int)x, (int)y);

        public static bool IsTopLeft(int x, int y) {

            if (!WorldGen.InWorld(x, y))
                return false;

            Tile tile = Main.tile[x, y];

            if (tile == null)
                return false;

            int tileType = tile.type;

            if (!Main.tileFrameImportant[tileType])
                return true;

            TOD tileData = TOD.GetTileData(tile);

            if (tileData != null) {

                int fullWidth  = tileData.CoordinateFullWidth;
                int fullHeight = tileData.CoordinateFullHeight;

                bool isTop  = tile.frameY % fullHeight == 0;
                bool isLeft = tile.frameX % fullWidth == 0;

                return isTop && isLeft;

            }

            return false;

        }

        public static void SetActive(int x, int y, bool active) {
            Main.tile[x, y].active(active: active);
        }

        public static void SetActive(int x, int y, bool active, ref UndoStep undo) {

            undo.Add(new ChangedTile(x, y));

            Main.tile[x, y].active(active: active);

        }

        public static void SetLiquid(int x, int y, byte volume, bool? lava = null, bool? honey = null) {

            Main.tile[x, y].liquid = volume;

            if (lava != null)
                Main.tile[x, y].lava(lava: (bool)lava);

            if (honey != null)
                Main.tile[x, y].honey(honey: (bool)honey);

        }

        public static void SetLiquid(int x, int y, byte volume, ref UndoStep undo, bool? lava = null, bool? honey = null) {

            undo.Add(new ChangedTile(x, y));

            Main.tile[x, y].liquid = volume;

            if (lava != null)
                Main.tile[x, y].lava(lava: (bool)lava);

            if (honey != null)
                Main.tile[x, y].honey(honey: (bool)honey);

        }

        public static void SetTile(int x, int y, ushort type, bool? active = true) {

            Main.tile[x, y].Clear(TileDataType.Slope);

            if (active != null)
                Main.tile[x, y].active((bool)active);

            Main.tile[x, y].type = type;

        }

        public static void SetTile(int x, int y, ushort type, ref UndoStep undo, bool? active = true) {

            undo.Add(new ChangedTile(x, y));

            Main.tile[x, y].Clear(TileDataType.Slope);

            if (active != null)
                Main.tile[x, y].active((bool)active);

            Main.tile[x, y].type = type;

        }

        public static void SetTileWall(int x, int y, ushort tileType, ushort wallType, bool active = true) {

            Main.tile[x, y].Clear(TileDataType.Slope);
            Main.tile[x, y].active(active);
            Main.tile[x, y].type = tileType;
            Main.tile[x, y].wall = wallType;

        }

        public static void SetTileWall(int x, int y, ushort tileType, ushort wallType, ref UndoStep undo, bool active = true) {

            undo.Add(new ChangedTile(x, y));

            Main.tile[x, y].Clear(TileDataType.Slope);
            Main.tile[x, y].active(active);
            Main.tile[x, y].type = tileType;
            Main.tile[x, y].wall = wallType;

        }

        public static void SetWall(int x, int y, ushort type, bool? active = null) {

            Main.tile[x, y].wall = type;

            if (active != null)
                Main.tile[x, y].active(active: (bool)active);

        }

        public static void SetWall(int x, int y, ushort type, ref UndoStep undo, bool? active = null) {

            undo.Add(new ChangedTile(x, y));

            Main.tile[x, y].wall = type;

            if (active != null)
                Main.tile[x, y].active(active: (bool)active);
            
        }

        public static bool TileCut(int x, int y) => TileCut(new[] { new Point(x, y) });

        public static bool TileCut(Point[] tiles) {

            foreach (Point tile in tiles)
                if (!WorldGen.InWorld(tile.X, tile.Y))
                    return false;

            foreach (Point tile in tiles)
                if (Main.tile[tile.X, tile.Y] == null)
                    Main.tile[tile.X, tile.Y] = new Tile();

            foreach (Point tile in tiles)
                if (Main.tile[tile.X, tile.Y].active() && !Main.tileCut[Main.tile[tile.X, tile.Y].type] && Main.tile[tile.X, tile.Y].type != TileID.Stalactite && Main.tile[tile.X, tile.Y].type != TileID.SmallPiles)
                    return false;

            foreach (Point tile in tiles) {

                if (Main.tile[tile.X, tile.Y].active() && (Main.tileCut[Main.tile[tile.X, tile.Y].type] || Main.tile[tile.X, tile.Y].type == TileID.Stalactite || Main.tile[tile.X, tile.Y].type == TileID.SmallPiles)) {

                    if (Main.tileCut[Main.tile[tile.X, tile.Y].type] || Main.tile[tile.X, tile.Y].type == TileID.SmallPiles) {
                        DrawInterface.ErasePot(tile.X, tile.Y);
                    }
                    else {
                        WorldGen.KillTile(tile.X, tile.Y);
                    }
                    
                }

            }

            return true;

        }

        public static int TileType(int x, int y) => !Main.tile[x, y].active() ? -1 : Main.tile[x, y].type;

        public static void WorldRestrain(ref int minX, ref int maxX, ref int minY, ref int maxY, int additionalXrestrain = 0, int additionalYrestrain = 0) {

            if (minX < 0 + additionalXrestrain)
                minX = 0 + additionalXrestrain;

            if (maxX > Main.maxTilesX - additionalXrestrain)
                maxX = Main.maxTilesX - additionalXrestrain;

            if (minY < 0 + additionalYrestrain)
                minY = 0 + additionalYrestrain;

            if (maxY > Main.maxTilesY - additionalYrestrain)
                maxY = Main.maxTilesY - additionalYrestrain;

        }

    }

    public struct BrushShape {

        public const byte Square = 0;
        public const byte Circle = 1;
        public const byte Line   = 2;
        public const byte Fill   = 3;

    }

    public struct ListSorters {

        public const byte ID   = 0;
        public const byte Name = 1;

    }

    public struct PaintMode {

        public const byte Paint      = 0;
        public const byte Erase      = 1;
        public const byte Eyedropper = 2;
        public const byte Select     = 3;
        public const byte MagicWand  = 4;

    }

    public struct Tabs {

        public const byte Tiles      = 0;
        public const byte Walls      = 1;
        public const byte Structures = 2;
        public const byte Other      = 3;

    }

    public class ListItem {

        public int ID { get; }
        
        public string Name { get; }

        public ListItem(int id, string name) {

            ID   = id;
            Name = name;

        }

    }

    public readonly struct IntRange {

        public readonly int Minimum;
        public readonly int Maximum;

        public IntRange(int minimum, int maximum) {
            Minimum = minimum;
            Maximum = maximum;
        }

        public static IntRange operator *(IntRange range, float scale) {
            return new IntRange((int)(range.Minimum * scale), (int)(range.Maximum * scale));
        }

        public static IntRange operator *(float scale, IntRange range) {
            return range * scale;
        }

        public static IntRange operator /(IntRange range, float scale) {
            return new IntRange((int)(range.Minimum / scale), (int)(range.Maximum / scale));
        }

        public static IntRange operator /(float scale, IntRange range) {
            return range / scale;
        }

    }

    public struct StructureNames {

        public const string CampSite           = "Camp Site";
        public const string CaveOpenater       = "Cave Openater";
        public const string Caverer            = "Caverer";
        public const string Cavinator          = "Cavinator";
        public const string CloudIsland        = "Cloud Island";
        public const string CloudIslandHouse   = "Cloud Island House";
        public const string CloudLake          = "Cloud Lake";
        public const string DeadMansChest      = "Booby Trap Chest";
        public const string CorruptionStart    = "Corruption Start";
        public const string CrimsonEntrance    = "Crimson Entrance";
        public const string CrimsonStart       = "Crimson Start";
        public const string Desert             = "Desert";
        public const string Dunes              = "Dunes";
        public const string Dungeon            = "Dungeon";
        public const string EnchantedSword     = "Enchanted Sword Biome";
        public const string EpicTree           = "Epic Tree";
        public const string GemCave            = "Gem Cave";
        public const string GraniteCave        = "Granite Cave";
        public const string GraniteCavern      = "Granite Cavern";
        public const string HellFort           = "Hell Fort";
        public const string Hive               = "Hive";
        public const string HoneyPatch         = "Honey Patch";
        public const string JungleShrine       = "Jungle Shrine";
        public const string Lakinater          = "Lake";
        public const string LavaTrap           = "Lava Trap";
        public const string LihzahrdTemple     = "Lihzahrd Temple";
        public const string LivingMahoganyTree = "Living Mahogany Tree";
        public const string LivingTree         = "Living Tree";
        public const string MahoganyTree       = "Mahogany Tree";
        public const string MakeHole           = "Make Hole";
        public const string MarbleCave         = "Marble Cave";
        public const string MarbleCavern       = "Marble Cavern";
        public const string Meteor             = "Meteor";
        public const string MiningExplosives   = "Mining Explosives";
        public const string MossCave           = "Moss Cave";
        public const string Mountinater        = "Mountinater";
        public const string Oasis              = "Oasis";
        public const string OceanCave          = "Ocean Cave";
        public const string Pyramid            = "Pyramid";
        public const string SandTrap           = "Sand Trap";
        public const string ShroomPatch        = "Shroom Patch";
        public const string SpiderCave         = "Spider Cave";
        public const string StonePatch         = "Stone Patch";
        public const string UndergroundHouse   = "Underground House";
        public const string WateryIceThing     = "Watery Ice Thing";

    }

    public struct OtherNames {

        public const string Converter   = "Convert Biome";
        public const string Liquid      = "Liquid";
        public const string Mech        = "Mech";
        public const string PaintTile   = "Paint Tile";
        public const string PaintWall   = "Paint Wall";
        public const string ResetFrames = "Reset Frames";
        public const string RoomCheck   = "Room Check";
        public const string Slope       = "Slope";
        public const string Smooth      = "Smoothing";
        public const string Spawn       = "Spawn Point";

    }

}

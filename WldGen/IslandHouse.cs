using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 7/13/2020

		public static int IslandHouseWidth      = -1;
		public static int IslandHouseHeight     = -1;
		public static int IslandHouseRandomSide = -1;

		public static void GetIslandHouseSize() {

			IslandHouseWidth      = WorldGen.genRand.Next(7, 12);
			IslandHouseHeight     = WorldGen.genRand.Next(5, 7);
			IslandHouseRandomSide = WorldGen.genRand.Next(2) == 0 ? -1 : 1;

		}

		public static void IslandHouse(int i, int j, ref UndoStep undo, ushort type = 202, byte wall = 82, byte doorStyle = 9, byte tableStyle = 7, byte chairStyle = 10, byte[] bannerStyle = null, byte chestStyle = 13, int chestItem = -1) {

			if (IslandHouseWidth == -1)
				GetIslandHouseSize();

			if (bannerStyle == null || bannerStyle.Length == 0)
				bannerStyle = new byte[] { 7, 8, 9 };

			int randomSide = IslandHouseRandomSide;
			int widthHalf  = IslandHouseWidth;
			int height     = IslandHouseHeight;

			Vector2 position = new Vector2(i, j);

			position.X = i + (widthHalf + 2) * randomSide;

			for (int k = j; k < j + 30; k++) {

				if (Main.tile[(int)position.X, k].active()) {
					position.Y = k - 1;
					break;
				}

			}

			position.X = i;

			int startX = (int)(position.X - widthHalf - 1f);
			int endX   = (int)(position.X + widthHalf + 1f);
			int startY = (int)(position.Y - height - 1f);
			int endY   = (int)(position.Y + 2f);

			Neo.WorldRestrain(ref startX, ref endX, ref startY, ref endY);

			for (int l = startX; l <= endX; l++) {

				for (int m = startY - 1; m < endY + 1; m++) {

					if (m != startY - 1 || (l != startX && l != endX)) {

						Neo.SetTileWall(l, m, type, 0, ref undo);
						Neo.SetLiquid(l, m, 0);

					}

				}

			}

			startX = (int)(position.X - widthHalf);
			endX   = (int)(position.X + widthHalf);
			startY = (int)(position.Y - height);
			endY   = (int)(position.Y + 1f);

			Neo.WorldRestrain(ref startX, ref endX, ref startY, ref endY);

			for (int curX = startX; curX <= endX; curX++)
				for (int curY = startY; curY < endY; curY++)
					if ((curY != startY || (curX != startX && curX != endX)) && Main.tile[curX, curY].wall == 0)
						Neo.SetWall(curX, curY, wall, ref undo, false);

			int xPos  = i + (widthHalf + 1) * randomSide;

            for (int num11 = xPos - 2; num11 <= xPos + 2; num11++) {

				Neo.SetActive(num11, (int)position.Y, false, ref undo);
				Neo.SetActive(num11, (int)position.Y - 1, false, ref undo);
				Neo.SetActive(num11, (int)position.Y - 2, false, ref undo);

			}

            PlaceTile(xPos, (int)position.Y, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, doorStyle);
			
			xPos = i + (widthHalf + 1) * -randomSide - randomSide;
			
			for (int yPos = startY; yPos <= endY + 1; yPos++) {

				Neo.SetTileWall(xPos, yPos, type, 0, ref undo);
				Neo.SetLiquid(xPos, yPos, 0);

			}

			if (chestItem < 0 || chestItem >= ItemLoader.ItemCount) {

				switch (WorldGen.genRand.Next(3)) {

					case 0:
						chestItem = ItemID.ShinyRedBalloon;
						break;

					case 1:
						chestItem = ItemID.Starfury;
						break;

					case 2:
						chestItem = ItemID.LuckyHorseshoe;
						break;

				}

			}

			if (GetGoodWorldGen)
				chestStyle = 2;

            AddBuriedChest(i, (int)position.Y - 3, ref undo, chestItem, notNearOtherChests: false, chestStyle, false, 0);
			
			xPos = i - widthHalf / 2 + 1;
			int num15 = i + widthHalf / 2 - 1;
			int windowBuffer = 1;

			if (widthHalf > 10)
				windowBuffer = 2;
			
			int middleHeight = (startY + endY) / 2 - 1;

			// Place window    
			for (int currentX = xPos - windowBuffer; currentX <= xPos + windowBuffer; currentX++)
				for (int currentY = middleHeight - 1; currentY <= middleHeight + 1; currentY++)
					Neo.SetWall(currentX, currentY, WallID.Glass, ref undo);

			// Place window    
			for (int currentX = num15 - windowBuffer; currentX <= num15 + windowBuffer; currentX++)
				for (int currentY = middleHeight - 1; currentY <= middleHeight + 1; currentY++)
					Neo.SetWall(currentX, currentY, WallID.Glass, ref undo);

			xPos = i + (widthHalf / 2 + 1) * -randomSide;

			// Place Table
			PlaceTile(xPos,     endY - 1, TileID.Tables, ref undo, mute: true, forced: false, -1,  tableStyle);

			// Place Chair
			PlaceTile(xPos - 2, endY - 1, TileID.Chairs, ref undo, mute: true, forced: false,  0, chairStyle);

			// Flip Chair
			Main.tile[xPos - 2, endY - 1].frameX += 18;
			Main.tile[xPos - 2, endY - 2].frameX += 18;

			// Place Chair
			PlaceTile(xPos + 2, endY - 1, TileID.Chairs, ref undo, mute: true, forced: false, 0, chairStyle);
			
			xPos = startX + 1;

			// Place banner
			PlaceTile(xPos, startY, TileID.Banners, ref undo, mute: true, forced: false, -1, bannerStyle[Main.rand.Next(bannerStyle.Length)]);
			
			xPos = endX - 1;

			// Place banner
			PlaceTile(xPos, startY, TileID.Banners, ref undo, mute: true, forced: false, -1, bannerStyle[Main.rand.Next(bannerStyle.Length)]);
			
			xPos = randomSide > 0 ? startX : endX;

			// Place banner
			PlaceTile(xPos, startY + 1, TileID.Banners, ref undo, mute: true, forced: false, -1, bannerStyle[Main.rand.Next(bannerStyle.Length)]);

		}

	}

}

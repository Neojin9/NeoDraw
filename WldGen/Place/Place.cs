using NeoDraw.Undo;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using TOD = Terraria.ObjectData.TileObjectData;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/3/2020

		public static bool Place(TileObject toBePlaced, ref UndoStep undo) {

			TOD tileData = TOD.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);

			if (tileData == null)
				return false;

			ushort tileType = (ushort)toBePlaced.type;

			int style = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
			int row = 0;

			if (tileData.StyleWrapLimit > 0) {
				row = style / tileData.StyleWrapLimit * tileData.StyleLineSkip;
				style %= tileData.StyleWrapLimit;
			}

			int frameX;
			int frameY;

			if (tileData.StyleHorizontal) {
				frameX = tileData.CoordinateFullWidth * style;
				frameY = tileData.CoordinateFullHeight * row;
			}
			else {
				frameX = tileData.CoordinateFullWidth * row;
				frameY = tileData.CoordinateFullHeight * style;
			}

			int xPos = toBePlaced.xCoord;
			int yPos = toBePlaced.yCoord;

			for (int x = 0; x < tileData.Width; x++) {

				for (int y = 0; y < tileData.Height; y++) {

					Tile tileSafely = Framing.GetTileSafely(xPos + x, yPos + y);

					if (tileSafely.active() && Main.tileCut[tileSafely.type])
						WorldGen.KillTile(xPos + x, yPos + y);
					
				}

			}

			for (int tileX = 0; tileX < tileData.Width; tileX++) {

				int curFrameX = frameX + tileX * (tileData.CoordinateWidth + tileData.CoordinatePadding);
				int curFrameY = frameY;

				for (int tileY = 0; tileY < tileData.Height; tileY++) {

					Tile tileSafely2 = Framing.GetTileSafely(xPos + tileX, yPos + tileY);

					if (!tileSafely2.active()) {

						undo.Add(new ChangedTile(xPos + tileX, yPos + tileY));

						tileSafely2.active(true);
						tileSafely2.frameX = (short)curFrameX;
						tileSafely2.frameY = (short)curFrameY;
						tileSafely2.type = tileType;

					}

					curFrameY += tileData.CoordinateHeights[tileY] + tileData.CoordinatePadding;

				}

			}

			if (tileData.FlattenAnchors) {

				AnchorData anchorBottom = tileData.AnchorBottom;

				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile) {

					int num10 = toBePlaced.xCoord + anchorBottom.checkStart;
					int j2 = toBePlaced.yCoord + tileData.Height;

					for (int m = 0; m < anchorBottom.tileCount; m++) {

						Tile tileSafely3 = Framing.GetTileSafely(num10 + m, j2);

						if (Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely3.blockType() != 0) {
							
							undo.Add(new ChangedTile(num10 + m, j2));
							
							WorldGen.SlopeTile(num10 + m, j2);

						}


					}

				}

				anchorBottom = tileData.AnchorTop;

				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile) {

					int num11 = toBePlaced.xCoord + anchorBottom.checkStart;
					int j3 = toBePlaced.yCoord - 1;

					for (int n = 0; n < anchorBottom.tileCount; n++) {

						Tile tileSafely4 = Framing.GetTileSafely(num11 + n, j3);

						if (Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && tileSafely4.blockType() != 0) {
							
							undo.Add(new ChangedTile(num11 + n, j3));
							
							WorldGen.SlopeTile(num11 + n, j3);

						}
					}

				}

				anchorBottom = tileData.AnchorRight;

				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile) {

					int i2 = toBePlaced.xCoord + tileData.Width;
					int num12 = toBePlaced.yCoord + anchorBottom.checkStart;

					for (int num13 = 0; num13 < anchorBottom.tileCount; num13++) {

						Tile tileSafely5 = Framing.GetTileSafely(i2, num12 + num13);

						if (Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type] && tileSafely5.blockType() != 0) {
							
							undo.Add(new ChangedTile(i2, num12 + num13));
							
							WorldGen.SlopeTile(i2, num12 + num13);

						}
					}

				}

				anchorBottom = tileData.AnchorLeft;

				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile) {

					int i3 = toBePlaced.xCoord - 1;
					int num14 = toBePlaced.yCoord + anchorBottom.checkStart;

					for (int num15 = 0; num15 < anchorBottom.tileCount; num15++) {

						Tile tileSafely6 = Framing.GetTileSafely(i3, num14 + num15);

						if (Main.tileSolid[tileSafely6.type] && !Main.tileSolidTop[tileSafely6.type] && tileSafely6.blockType() != 0) {
							
							undo.Add(new ChangedTile(i3, num14 + num15));
							
							WorldGen.SlopeTile(i3, num14 + num15);

						}

					}

				}

			}

			return true;

		}

	}

}

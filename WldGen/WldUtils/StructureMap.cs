﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/23/2020

	public class StructureMap {

		private List<Rectangle> _structures = new List<Rectangle>(2048);
		private List<Rectangle> _protectedStructures = new List<Rectangle>(2048);

		private readonly object _lock = new object();

		public bool CanPlace(Rectangle area, int padding = 0) {
			return CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
		}

		public bool CanPlace(Rectangle area, bool[] validTiles, int padding = 0) {

			lock (_lock) {

				if (area.X < 0 || area.Y < 0 || area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1)
					return false;

				if (Main.keyState.PressingAlt())
					return true;

				Rectangle rectangle = new Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);

				for (int i = 0; i < _protectedStructures.Count; i++)
					if (rectangle.Intersects(_protectedStructures[i]))
						return false;

				for (int j = rectangle.X; j < rectangle.X + rectangle.Width; j++) {

					for (int k = rectangle.Y; k < rectangle.Y + rectangle.Height; k++) {

						if (Main.tile[j, k].active()) {

							if (Main.tile[j, k].type >= TileNames.OriginalTileCount) {
								DrawInterface.AddInvalidTile(j, k);
								DrawInterface.SetStatusBarTempMessage("Warning! Area contains modded tiles. Hold ALT while clicking to force placement.");
								return false;
							}

							if (!validTiles[Main.tile[j, k].type]) {
								DrawInterface.AddInvalidTile(j, k);
								DrawInterface.SetStatusBarTempMessage("Warning! Area contains invalid tiles. Hold ALT while clicking to force placement.");
								return false;
							}

						}

					}

				}

			}

			return true;

		}

		public Rectangle GetBoundingBox() {

			lock (_lock) {

				if (_structures.Count == 0)
					return Rectangle.Empty;
				
				Point point  = new Point(_structures.Min((Rectangle rect) => rect.Left),  _structures.Min((Rectangle rect) => rect.Top));
				Point point2 = new Point(_structures.Max((Rectangle rect) => rect.Right), _structures.Max((Rectangle rect) => rect.Bottom));
				
				return new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);

			}

		}

		public void AddStructure(Rectangle area, int padding = 0) {

			lock (_lock) {

				area.Inflate(padding, padding);
				_structures.Add(area);

			}

		}

		public void AddProtectedStructure(Rectangle area, int padding = 0) {

			lock (_lock) {

				area.Inflate(padding, padding);
				_structures.Add(area);
				_protectedStructures.Add(area);

			}

		}

		public void Reset() {

			lock (_lock) {
				_protectedStructures.Clear();
			}

		}

	}

}

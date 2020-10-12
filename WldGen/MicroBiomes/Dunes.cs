using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/25/2020

	public class Dunes : MicroBiome {

		private class DunesDescription {

			public bool IsValid { get; private set; }

			public SurfaceMap Surface { get; private set; }

			public Rectangle Area { get; private set; }

			public WindDirection WindDirection { get; private set; }

			private DunesDescription() { }

			public static DunesDescription CreateFromPlacement(Point origin, int width, int height) {

				Rectangle area = new Rectangle(origin.X - width / 2, origin.Y - height / 2, width, height);
				
				return new DunesDescription {

					Area = area,
					IsValid = true,
					Surface = SurfaceMap.FromArea(area.Left - 20, area.Width + 40),
					WindDirection = ((WorldGen.genRand.Next(2) != 0) ? WindDirection.Right : WindDirection.Left)

				};

			}

		}

		private enum WindDirection {
			Left,
			Right
		}

		private WorldGenRange _singleDunesWidth = new WorldGenRange(150, 250); // WorldGenRange.Empty;

		private float _heightScale = 1f;

		public int MaximumWidth => _singleDunesWidth.ScaledMaximum * 2;

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			int height  = (int)(_random.Next(60, 100) * _heightScale);
			int height2 = (int)(_random.Next(60, 100) * _heightScale);

			int random  = _singleDunesWidth.GetRandom(_random);
			int random2 = _singleDunesWidth.GetRandom(_random);

			DunesDescription description  = DunesDescription.CreateFromPlacement(new Point(origin.X - random / 2 + 30, origin.Y), random, height);
			DunesDescription description2 = DunesDescription.CreateFromPlacement(new Point(origin.X + random2 / 2 - 30, origin.Y), random2, height2);

			PlaceSingle(description, structures);
			PlaceSingle(description2, structures);

			int minX = Math.Min(description.Area.X, description2.Area.X);
			int minY = Math.Min(description.Area.Y, description2.Area.Y);
			int maxX = Math.Max(description.Area.X + description.Area.Width, description2.Area.X + description2.Area.Width);
			int maxY = Math.Max(description.Area.Y + description.Area.Height, description2.Area.Y + description2.Area.Height);

			WldUtils.WldUtils.Gen(new Point(description.Area.X, description.Area.Y), new Shapes.Rectangle(description.Area.Width, description.Area.Height), new Actions.SetFrames(true));
			WldUtils.WldUtils.Gen(new Point(description2.Area.X, description2.Area.Y), new Shapes.Rectangle(description2.Area.Width, description2.Area.Height), new Actions.SetFrames(true));

			return true;

		}

		private void PlaceSingle(DunesDescription description, StructureMap structures) {

			int num = _random.Next(3) + 8;

			for (int i = 0; i < num - 1; i++) {

				int num2 = (int)(2f / num * description.Area.Width);
				int num3 = (int)(i / (float)num * description.Area.Width + description.Area.Left) + num2 * 2 / 5;
				
				num3 += _random.Next(-5, 6);
				
				float num4 = i / (float)(num - 2);
				float num5 = 1f - Math.Abs(num4 - 0.5f) * 2f;
				
				PlaceHill(num3 - num2 / 2, num3 + num2 / 2, (num5 * 0.3f + 0.2f) * _heightScale, description);

			}

			int num6 = _random.Next(2) + 1;

			for (int j = 0; j < num6; j++) {

				int num7 = description.Area.Width / 2;
				int x = description.Area.Center.X;
				
				x += _random.Next(-10, 11);
				
				PlaceHill(x - num7 / 2, x + num7 / 2, 0.8f * _heightScale, description);

			}

		}

		private static void PlaceHill(int startX, int endX, float scale, DunesDescription description) {

			Point startPoint = new Point(startX, description.Surface[startX]);
			Point endPoint = new Point(endX, description.Surface[endX]);
			Point point = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2 - (int)(35f * scale));
			
			int num = (endPoint.X - point.X) / 4;
			int minValue = (endPoint.X - point.X) / 16;
			
			if (description.WindDirection == WindDirection.Left) {
				point.X -= WorldGen.genRand.Next(minValue, num + 1);
			}
			else {
				point.X += WorldGen.genRand.Next(minValue, num + 1);
			}
			
			Point point2 = new Point(0, (int)(scale * 12f));
			Point point3 = new Point(point2.X / -2, point2.Y / -2);
			
			PlaceCurvedLine(startPoint, point, (description.WindDirection != 0) ? point3 : point2, description);
			PlaceCurvedLine(point, endPoint, (description.WindDirection == WindDirection.Left) ? point3 : point2, description);

		}

		private static void PlaceCurvedLine(Point startPoint, Point endPoint, Point anchorOffset, DunesDescription description) {

			Point p = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
			p.X += anchorOffset.X;
			p.Y += anchorOffset.Y;
			
			Vector2 value = startPoint.ToVector2();
			Vector2 value2 = endPoint.ToVector2();
			Vector2 vector = p.ToVector2();
			
			float num = 0.5f / (value2.X - value.X);
			
			Point b = new Point(-1, -1);
			
			for (float num2 = 0f; num2 <= 1f; num2 += num) {

				Vector2 value3 = Vector2.Lerp(value, vector, num2);
				Vector2 value4 = Vector2.Lerp(vector, value2, num2);
				Point point = Vector2.Lerp(value3, value4, num2).ToPoint();
				
				if (point == b)
					continue;
				
				b = point;

				int num3 = description.Area.Width / 2 - Math.Abs(point.X - description.Area.Center.X);
				int num4 = description.Surface[point.X] + (int)(Math.Sqrt(num3) * 3.0);
				
				for (int i = point.Y - 10; i < point.Y; i++) {

					if (_tiles[point.X, i].active() && _tiles[point.X, i].type != 53) {

						DrawInterface.AddChangedTile(point.X, i);
                        _tiles[point.X, i].ClearEverything();

					}

				}

				for (int j = point.Y; j < num4; j++) {

					DrawInterface.AddChangedTile(point.X, j);
                    _tiles[point.X, j].ResetToType(53);
					SmoothSlope(point.X, j, ref DrawInterface.Undo);

				}

			}

		}

	}

}

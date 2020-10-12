using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/25/2020

	public static class Shapes {

		public class Branch : GenShape {

			private Point _offset;

			private List<Point> _endPoints;

			public Branch() {
				_offset = new Point(10, -5);
			}

			public Branch(Point offset) {
				_offset = offset;
			}

			public Branch(double angle, double distance) {
				_offset = new Point((int)(Math.Cos(angle) * distance), (int)(Math.Sin(angle) * distance));
			}

			private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size) {

				size = Math.Max(1, size);
				
				for (int i = -(size >> 1); i < size - (size >> 1); i++)
					for (int j = -(size >> 1); j < size - (size >> 1); j++)
						if (!Utils.PlotLine(new Point(start.X + i, start.Y + j), end, (int tileX, int tileY) => UnitApply(action, origin, tileX, tileY) || !_quitOnFail, jump: false))
							return false;
						
				return true;

			}

			public override bool Perform(Point origin, GenAction action) {

				float num = new Vector2(_offset.X, _offset.Y).Length();
				int num2 = (int)(num / 6f);
				
				if (_endPoints != null)
					_endPoints.Add(new Point(origin.X + _offset.X, origin.Y + _offset.Y));
				
				if (!PerformSegment(origin, action, origin, new Point(origin.X + _offset.X, origin.Y + _offset.Y), num2))
					return false;
				
				int num3 = (int)(num / 8f);

				for (int i = 0; i < num3; i++) {

					float num4 = (i + 1f) / (num3 + 1f);
					
					Point point = new Point((int)(num4 * _offset.X), (int)(num4 * _offset.Y));
					
					Vector2 spinningpoint = new Vector2(_offset.X - point.X, _offset.Y - point.Y);
					
					spinningpoint = spinningpoint.RotatedBy(((float)_random.NextDouble() * 0.5f + 1f) * ((_random.Next(2) != 0) ? 1 : (-1))) * 0.75f;
					
					Point point2 = new Point((int)spinningpoint.X + point.X, (int)spinningpoint.Y + point.Y);
					
					if (_endPoints != null)
						_endPoints.Add(new Point(point2.X + origin.X, point2.Y + origin.Y));
					
					if (!PerformSegment(origin, action, new Point(point.X + origin.X, point.Y + origin.Y), new Point(point2.X + origin.X, point2.Y + origin.Y), num2 - 1))
						return false;
					
				}

				return true;

			}

			public Branch OutputEndpoints(List<Point> endpoints) {

				_endPoints = endpoints;

				return this;

			}

		}

		public class Circle : GenShape {

			private int _verticalRadius;
			private int _horizontalRadius;

			public Circle(int radius) {
				_verticalRadius = radius;
				_horizontalRadius = radius;
			}

			public Circle(int horizontalRadius, int verticalRadius) {
				_horizontalRadius = horizontalRadius;
				_verticalRadius = verticalRadius;
			}

			public void SetRadius(int radius) {
				_verticalRadius = radius;
				_horizontalRadius = radius;
			}

			public override bool Perform(Point origin, GenAction action) {

				int num = (_horizontalRadius + 1) * (_horizontalRadius + 1);

				for (int i = origin.Y - _verticalRadius; i <= origin.Y + _verticalRadius; i++) {

					float num2 = _horizontalRadius / (float)_verticalRadius * (i - origin.Y);
					int num3 = Math.Min(_horizontalRadius, (int)Math.Sqrt(num - num2 * num2));

					for (int j = origin.X - num3; j <= origin.X + num3; j++)
						if (!UnitApply(action, origin, j, i) && _quitOnFail)
							return false;
						
				}

				return true;

			}

		}

		public class HalfCircle : GenShape {

			private int _radius;

			public HalfCircle(int radius) {
				_radius = radius;
			}

			public override bool Perform(Point origin, GenAction action) {

				int num = (_radius + 1) * (_radius + 1);

				for (int i = origin.Y - _radius; i <= origin.Y; i++) {

					int num2 = Math.Min(_radius, (int)Math.Sqrt(num - (i - origin.Y) * (i - origin.Y)));

					for (int j = origin.X - num2; j <= origin.X + num2; j++)
						if (!UnitApply(action, origin, j, i) && _quitOnFail)
							return false;

				}

				return true;

			}

		}

		public class Mound : GenShape {

			private int _halfWidth;
			private int _height;

			public Mound(int halfWidth, int height) {
				_halfWidth = halfWidth;
				_height = height;
			}

			public override bool Perform(Point origin, GenAction action) {

				float num = _halfWidth;

				for (int i = -_halfWidth; i <= _halfWidth; i++) {

					int num2 = Math.Min(_height, (int)((0f - (_height + 1) / (num * num)) * (i + num) * (i - num)));

					for (int j = 0; j < num2; j++)
						if (!UnitApply(action, origin, i + origin.X, origin.Y - j) && _quitOnFail)
							return false;

				}

				return true;

			}

		}

		public class Rectangle : GenShape {

			private int _width;
			private int _height;

			public Rectangle(int width, int height) {
				_width = width;
				_height = height;
			}

			public override bool Perform(Point origin, GenAction action) {

				for (int i = origin.X; i < origin.X + _width; i++)
					for (int j = origin.Y; j < origin.Y + _height; j++)
						if (!UnitApply(action, origin, i, j) && _quitOnFail)
							return false;

				return true;

			}

		}

		public class Rectangle2 : GenShape {

			private Microsoft.Xna.Framework.Rectangle _area;

			public Rectangle2(Microsoft.Xna.Framework.Rectangle area) {
				_area = area;
			}

			public Rectangle2(int width, int height) {
				_area = new Microsoft.Xna.Framework.Rectangle(0, 0, width, height);
			}

			public void SetArea(Microsoft.Xna.Framework.Rectangle area) {
				_area = area;
			}

			public override bool Perform(Point origin, GenAction action) {

				for (int i = origin.X + _area.Left; i < origin.X + _area.Right; i++)
					for (int j = origin.Y + _area.Top; j < origin.Y + _area.Bottom; j++)
						if (!UnitApply(action, origin, i, j) && _quitOnFail)
							return false;
						
				return true;

			}

		}

		public class Root : GenShape {

			private float _angle;
			private float _startingSize;
			private float _endingSize;
			private float _distance;

			public Root(float angle, float distance = 10f, float startingSize = 4f, float endingSize = 1f) {

				_angle = angle;
				_distance = distance;
				_startingSize = startingSize;
				_endingSize = endingSize;

			}

			private bool DoRoot(Point origin, GenAction action, float angle, float distance, float startingSize) {

				float num = origin.X;
				float num2 = origin.Y;
				
				for (float num3 = 0f; num3 < distance * 0.85f; num3 += 1f) {

					float num4 = num3 / distance;
					float num5 = MathHelper.Lerp(startingSize, _endingSize, num4);
					
					num += (float)Math.Cos(angle);
					num2 += (float)Math.Sin(angle);
					angle += _random.NextFloat() - 0.5f + _random.NextFloat() * (_angle - (float)Math.PI / 2f) * 0.1f * (1f - num4);
					angle = angle * 0.4f + 0.45f * MathHelper.Clamp(angle, _angle - 2f * (1f - 0.5f * num4), _angle + 2f * (1f - 0.5f * num4)) + MathHelper.Lerp(_angle, (float)Math.PI / 2f, num4) * 0.15f;
					
					for (int i = 0; i < (int)num5; i++)
						for (int j = 0; j < (int)num5; j++)
							if (!UnitApply(action, origin, (int)num + i, (int)num2 + j) && _quitOnFail)
								return false;
							
				}

				return true;

			}

			public override bool Perform(Point origin, GenAction action) {
				return DoRoot(origin, action, _angle, _distance, _startingSize);
			}

		}

		public class Runner : GenShape {

			private float _startStrength;

			private int _steps;

			private Vector2 _startVelocity;

			public Runner(float strength, int steps, Vector2 velocity) {
				_startStrength = strength;
				_steps = steps;
				_startVelocity = velocity;
			}

			public override bool Perform(Point origin, GenAction action) {

				float  num  = _steps;
				float  num2 = _steps;
				double num3 = _startStrength;
				
				Vector2 vector  = new Vector2(origin.X, origin.Y);
				Vector2 vector2 = (_startVelocity == Vector2.Zero) ? Utils.RandomVector2(_random, -1f, 1f) : _startVelocity;

				while (num > 0f && num3 > 0.0) {

					num3 = _startStrength * (num / num2);
					num -= 1f;

					int num4 = Math.Max(1, (int)(vector.X - num3 * 0.5));
					int num5 = Math.Max(1, (int)(vector.Y - num3 * 0.5));
					int num6 = Math.Min(_worldWidth, (int)(vector.X + num3 * 0.5));
					int num7 = Math.Min(_worldHeight, (int)(vector.Y + num3 * 0.5));

					for (int i = num4; i < num6; i++)
						for (int j = num5; j < num7; j++)
							if (!(Math.Abs(i - vector.X) + Math.Abs(j - vector.Y) >= num3 * 0.5 * (1.0 + _random.Next(-10, 11) * 0.015)))
								UnitApply(action, origin, i, j);
							
					int num8 = (int)(num3 / 50.0) + 1;
					num -= num8;
					vector += vector2;

					for (int k = 0; k < num8; k++) {
						vector += vector2;
						vector2 += Utils.RandomVector2(_random, -0.5f, 0.5f);
					}

					vector2 += Utils.RandomVector2(_random, -0.5f, 0.5f);
					vector2 = Vector2.Clamp(vector2, -Vector2.One, Vector2.One);

				}

				return true;

			}

		}

		public class Slime : GenShape {

			private int _radius;
			private float _xScale;
			private float _yScale;

			public Slime(int radius) {
				_radius = radius;
				_xScale = 1f;
				_yScale = 1f;
			}

			public Slime(int radius, float xScale, float yScale) {
				_radius = radius;
				_xScale = xScale;
				_yScale = yScale;
			}

			public override bool Perform(Point origin, GenAction action) {

				float num = _radius;
				int num2 = (_radius + 1) * (_radius + 1);

				for (int i = origin.Y - (int)(num * _yScale); i <= origin.Y; i++) {

					float num3 = (i - origin.Y) / _yScale;
					int num4 = (int)Math.Min(_radius * _xScale, _xScale * (float)Math.Sqrt(num2 - num3 * num3));

					for (int j = origin.X - num4; j <= origin.X + num4; j++)
						if (!UnitApply(action, origin, j, i) && _quitOnFail)
							return false;

				}

				for (int k = origin.Y + 1; k <= origin.Y + (int)(num * _yScale * 0.5f) - 1; k++) {

					float num5 = (k - origin.Y) * (2f / _yScale);
					int num6 = (int)Math.Min(_radius * _xScale, _xScale * (float)Math.Sqrt(num2 - num5 * num5));

					for (int l = origin.X - num6; l <= origin.X + num6; l++)
						if (!UnitApply(action, origin, l, k) && _quitOnFail)
							return false;

				}

				return true;

			}

		}

		public class Tail : GenShape {

			private float _width;

			private Vector2 _endOffset;

			public Tail(float width, Vector2 endOffset) {
				_width = width * 16f;
				_endOffset = endOffset * 16f;
			}

			public override bool Perform(Point origin, GenAction action) {
				Vector2 vector = new Vector2(origin.X << 4, origin.Y << 4);
				return Utils.PlotTileTale(vector, vector + _endOffset, _width, (int x, int y) => UnitApply(action, origin, x, y) || !_quitOnFail);
			}

		}

	}

}

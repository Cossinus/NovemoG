using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Novemo.Controllers
{
	public class TileController : MonoBehaviour
	{
		private static Tilemap tilemap;

		private void Awake()
		{
			tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
		}

		public static List<Node> Search(Node start, Node target)
		{
			Node current = null;
			var path = new List<Node>();
			var openList = new List<Node>();
			var closedList = new List<Node>();

			openList.Add(start);

			while (openList.Count > 0)
			{
				var lowest = openList.Min(n => n.F);
				current = openList.First(n => Math.Abs(n.F - lowest) < 0.01f);
				closedList.Add(current);
				openList.Remove(current);

				if (closedList.FirstOrDefault(n => n.X == target.X && n.Y == target.Y) != null) break;

				var adjacentNodes = GetAdjacentTiles(current.X, current.Y);
				
				var g = current.G;

				foreach (var adjacentNode in adjacentNodes)
				{
					if (closedList.FirstOrDefault(n => n.X == adjacentNode.X && n.Y == adjacentNode.Y) != null)
						continue;

					if (openList.FirstOrDefault(n => n.X == adjacentNode.X && n.Y == adjacentNode.Y) == null)
					{
						adjacentNode.G = adjacentNode.IsDiagonal ? adjacentNode.G = g + 1.414f : adjacentNode.G = g + 1;
						
						adjacentNode.H = ComputeHValue(adjacentNode.X, adjacentNode.Y, target.X, target.Y);
						adjacentNode.ParentNode = current;
						
						openList.Insert(0, adjacentNode);
					}
					else
					{
						if (!(g + adjacentNode.H < adjacentNode.F)) continue;
						
						adjacentNode.G = adjacentNode.IsDiagonal ? adjacentNode.G = g + 1.414f : adjacentNode.G = g + 1;
						
						adjacentNode.ParentNode = current;
					}
				}
			}

			while (current != null)
			{
				path.Add(current);
				current = current.ParentNode;
			}

			path.Reverse();

			return path;
		}

		private static IEnumerable<Node> GetAdjacentTiles(int x, int y)
		{
			var proposedLocations = new List<Node>()
			{
				new Node { X = x - 1, Y = y + 1, IsDiagonal = true }, //North-West
				new Node { X = x,     Y = y + 1 },                    //North
				new Node { X = x + 1, Y = y + 1, IsDiagonal = true }, //North-East
				new Node { X = x + 1, Y = y },                        //East
				new Node { X = x + 1, Y = y - 1, IsDiagonal = true }, //South-East
				new Node { X = x,     Y = y - 1 },                    //South
				new Node { X = x - 1, Y = y - 1, IsDiagonal = true }, //South-West
				new Node { X = x - 1, Y = y }                         //West
			};
			
			return proposedLocations.Where(n => GetTile(n) != null).ToList();
		}

		private static Node GetTile(Node node)
		{
			return tilemap.HasTile(tilemap.WorldToCell(new Vector3(node.X, node.Y))) ? node : null;
		}

		private static float ComputeHValue(int x, int y, int targetX, int targetY)
		{
			return Vector2.Distance(new Vector2(x, y), new Vector2(targetX, targetY));
		}

		public class Node
		{
			public int X { get; set; }
			public int Y { get; set; }
			public float F => G + H;
			public float G { get; set; }
			public float H { get; set; }
			public bool IsDiagonal { get; set; }
			public Node ParentNode { get; set; }
		}
	}
}
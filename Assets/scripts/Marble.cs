using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public enum MarbleType {
		None,
		Blue,
		Green,
		Orange,
		Red,
		Count,
	};

	public struct Position {
		public int x;
		public int y;

		public Position (int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	public class Marble {
		
		public MarbleType type;
		public GameObject marbleObject;
		public Position position;

		public Marble (MarbleType type, GameObject gameObject, Position position) {
			this.type = type;
			marbleObject = gameObject;
			this.position = position;
		}
	}
}


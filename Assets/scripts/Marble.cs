using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public enum MarbleType {
		Blue,
		Green,
		Orange,
		Red,
		Count,
	};

	public class Marble {
		
		public MarbleType type;
		public GameObject marbleObject;

		public Marble (MarbleType type, GameObject gameObject) {
			this.type = type;
			marbleObject = gameObject;
		}
	}
}


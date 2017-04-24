using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class BoardManager : MonoBehaviour {

	public int xBoardSize;
	public int yBoardSize;

	public GameObject bgTile;
	private GameObject[,] bgTiles;

	public GameObject blueMarble;
	public GameObject greenMarble;
	public GameObject orangeMarble;
	public GameObject redMarble;

	private Marble[,] marbles;

	private Vector2 originOffset = new Vector2 (0, 0);
	private Vector2[,] tilePositions;

	// Use this for initialization
	void Start () {
		CalculateOriginOffset ();
		CalculateTilePositions ();
		SetupBoard ();
		SetupMarbles ();

		// debug
		ApplyGravity (GravityDirection.Down);
	}

	private void CalculateOriginOffset () {
		Vector2 tileSize = bgTile.GetComponent<SpriteRenderer>().bounds.size;
		Vector2 gapSize = new Vector2 (tileSize.x, tileSize.y) * 0.1f;
		float startX = transform.position.x - (tileSize.x * xBoardSize + gapSize.x * (xBoardSize - 1)) / 2.0f;
		float startY = transform.position.y - (tileSize.y * yBoardSize + gapSize.y * (yBoardSize - 1)) / 2.0f;
		originOffset = new Vector2 (startX, startY);
	}

	private void CalculateTilePositions () {
		tilePositions = new Vector2 [xBoardSize, yBoardSize];
		for (int y = 0; y < yBoardSize; y++) {
			for (int x = 0; x < xBoardSize; x++) {
				tilePositions [x, y] = PositionForTile (x, y);
			}
		}
	}

	private Vector2 PositionForTile (int xIndex, int yIndex) {
		Vector2 tileSize = bgTile.GetComponent<SpriteRenderer>().bounds.size;
		Vector2 gapSize = new Vector2 (tileSize.x, tileSize.y) * 0.1f;

		float xPos = originOffset.x + ((tileSize.x + gapSize.x) * (float)xIndex) + tileSize.x / 2.0f;
		float yPos = originOffset.y + ((tileSize.y + gapSize.y) * (float)yIndex) + tileSize.y / 2.0f;
		return new Vector2 (xPos, yPos);
	}

	private void SetupBoard () {
		bgTiles = new GameObject [xBoardSize, yBoardSize];

		for (int y = 0; y < yBoardSize; y++) {
			for (int x = 0; x < xBoardSize; x++) {
				Vector2 pos = tilePositions [x, y];
				GameObject newTile = Instantiate(bgTile, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
				bgTiles[x, y] = newTile;
			}
		}
	}


	// ############ Gameplay logic #############

	private void SetupMarbles () {
		marbles = new Marble[xBoardSize, yBoardSize];
		for (int i = 0; i < 5; i++) {
			AddMarble ();
		}}

	// Adds a marble to some random place
	private bool AddMarble () {
		MarbleType type = (MarbleType)(Mathf.RoundToInt (Random.value * 100) % (int)MarbleType.Count);
		return AddMarble (type);
	}

	private bool AddMarble (MarbleType type) {
		int xIndex, yIndex;
		int tries = 0;
		do {
			if (tries > xBoardSize * yBoardSize * 3) { // TODO: get random tile from empty tiles only
				return false;
			}
			tries++;
			xIndex = Mathf.RoundToInt (Random.value * 100) % xBoardSize;
			yIndex = Mathf.RoundToInt (Random.value * 100) % yBoardSize;
		} while (!AddMarble (xIndex, yIndex, type));
		return true;
	}

	private bool AddMarble (int xIndex, int yIndex, MarbleType type) {
		if (marbles [xIndex, yIndex] != null) {
			return false;
		}

		Vector2 position = tilePositions [xIndex, yIndex];
		GameObject newMarbleObject = Instantiate(ObjectForMarbleType (type), new Vector3(position.x, position.y, 0), Quaternion.identity);
		marbles [xIndex, yIndex] = new Marble (type, newMarbleObject);
		return true;
	}

	private GameObject ObjectForMarbleType(MarbleType type) {
		switch (type) {
		case MarbleType.Blue:
			return blueMarble;
		case MarbleType.Green:
			return greenMarble;
		case MarbleType.Orange:
			return orangeMarble;
		case MarbleType.Red:
			return redMarble;
		default:
			return null;
		}
	}

	void UpdateMarblePositions () {
		for (int y = 0; y < yBoardSize; y++) {
			for (int x = 0; x < xBoardSize; x++) {
				if (marbles [x, y] != null) {
					GameObject marbleObject = marbles [x, y].marbleObject;
					Vector2 position = tilePositions [x, y];
					marbleObject.transform.position = new Vector3 (position.x, position.y, 0);
				}
			}
		}
	}

	// ##### Gravity ######

	void ApplyGravity (GravityDirection direction) {

		Marble[,] newMarbles = new Marble [xBoardSize, yBoardSize];

//		int startDim1Index = 0, startDim2Index = 0;
//		int endDim1Index = xBoardSize, endDim2Index = yBoardSize;
//		int dim1StepDelta = 1, dim2StepDelta = 1;
//
//		for (int dim1 = startDim1Index; x < xBoardSize; x++) { // TODO: dim1 < endDim1Index might require ">"
//			int lastSetPlace = 0;
//			for (int dim2 = 0; y < yBoardSize; y++) {
//				if (marbles [x, y] != null) {
//					newMarbles [x, lastSetPlace] = marbles [x, y];
//					lastSetPlace++;
//				}
//			}
//		}

		switch (direction) {

		case GravityDirection.Down:
			for (int x = 0; x < xBoardSize; x++) {
				int lastSetPlace = 0;
				for (int y = 0; y < yBoardSize; y++) {
					if (marbles [x, y] != null) {
						newMarbles [x, lastSetPlace] = marbles [x, y];
						lastSetPlace++;
					}
				}
			}
			break;

		case GravityDirection.Up:
			for (int x = xBoardSize - 1; x >= 0; x--) {
				int lastSetPlace = yBoardSize - 1;
				for (int y = yBoardSize - 1; y >= 0; y--) {
					if (marbles [x, y] != null) {
						newMarbles [x, lastSetPlace] = marbles [x, y];
						lastSetPlace--;
					}
				}
			}
			break;

		case GravityDirection.Left:
			for (int y = 0; y < yBoardSize; y++) {
				int lastSetPlace = 0;
				for (int x = 0; x < yBoardSize; x++) {
					if (marbles [x, y] != null) {
						newMarbles [lastSetPlace, y] = marbles [x, y];
						lastSetPlace++;
					}
				}
			}
			break;

		case GravityDirection.Right:
			for (int y = yBoardSize - 1; y >= 0; y--) {
				int lastSetPlace = xBoardSize - 1;
				for (int x = xBoardSize - 1; x >= 0; x--) {
					if (marbles [x, y] != null) {
						newMarbles [lastSetPlace, y] = marbles [x, y];
						lastSetPlace--;
					}
				}
			}
			break;

		}

		marbles = newMarbles;
		UpdateMarblePositions ();
	}

	void GravityChanged(GravityDirection newDirection) {
//		MarbleType type;
//		switch (newDirection) {
//		case GravityDirection.Left:
//			type = MarbleType.Blue;
//			break;
//		case GravityDirection.Up:
//			type = MarbleType.Green;
//			break;
//		case GravityDirection.Right:
//			type = MarbleType.Red;
//			break;
//		case GravityDirection.Down:
//			type = MarbleType.Orange;
//			break;
//		default:
//			type = MarbleType.Red;
//			break;
//		}
//
		ApplyGravity (newDirection);
		AddMarble ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	// ##### Controls #####

	void OnEnable () {
		ControlManager.Swipe += GravityChanged;
	}

	void OnDisable () {
		ControlManager.Swipe -= GravityChanged;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Match3
{   
	public class MatchManager : MonoBehaviour
	{
		public const float TIME_TO_EXPLODE = 0.3f;
		public const float TIME_TO_SWAP_DRAG_MODE = 0.2f;
		public const float TIME_TO_SWAP = 0.3f;

		public float timeToSwap
		{
			get
			{
				return dragMode ? TIME_TO_SWAP_DRAG_MODE : TIME_TO_SWAP;
			}
		}

		public static MatchManager instance;

		public bool dragMode;      
		public bool swapBack;
		public bool diagonalMatches;
		public float dragThreshold = 1.2f;

		public int rows, columns;
		[SerializeField]
		public List<MatchPieceType> pieceTypes = new List<MatchPieceType>();

		public bool canMove { get; set; }
		public bool needCheckMatches { get; set; }
		public bool gameIsOver { get; set; }

		private MatchPiece[][] board;
	    private GameObject matchPieceObject;
        private MatchPiece currentPiece;
		private SwapDirection currentDirection;

		public void Start()
		{
			instance = this;

		    matchPieceObject = (GameObject) Instantiate(Resources.Load("Prefabs/Piece"));
            Vector2 offset = matchPieceObject.GetComponent<SpriteRenderer>().bounds.size;
			CreateBoard(offset.x, offset.y);

			canMove = true;
			gameIsOver = false;
		}

		private void CreateBoard(float xOffset, float yOffset)
		{
			float startX = transform.position.x;
			float startY = transform.position.y;

			MatchPieceType[] previousLeft = new MatchPieceType[columns];
			MatchPieceType previousBelow = null;

			board = new MatchPiece[rows][];
			for (int x = 0; x < rows; x++)
			{
				board[x] = new MatchPiece[columns];
				for (int y = 0; y < columns; y++)
				{
					var tile = Instantiate(
						matchPieceObject,
						new Vector3(startX + (xOffset * x),
									startY + (yOffset * y),
									2),
						matchPieceObject.transform.rotation).AddComponent<MatchPiece>();

					List<MatchPieceType> possibletypes = new List<MatchPieceType>();
					possibletypes.AddRange(pieceTypes);

					possibletypes.Remove(previousLeft[y]);
					possibletypes.Remove(previousBelow);

					MatchPieceType type = possibletypes[Random.Range(0, possibletypes.Count)];

					tile.SetupPiece(y, x, type, TIME_TO_EXPLODE);

					previousLeft[y] = type;
					previousBelow = type;

					board[x][y] = tile;
				}
			}
		}


	}
}

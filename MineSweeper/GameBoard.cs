using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MineSweeper
{
	public class GameBoard
	{
//		int GameWidth;
//		int GameHeight;
		Game game;
		Random r = new Random();
		int state;
		int mines; //value for total mines in game

		public int GameState // 0 = play, 1 = win, 2 = lose
		{
			get {
				return state;
			}
			set {
				state = value;
			}
		}

		public GameSquare[,] CreateGameBoard (int GameHeight, int GameWidth, int dif)
		{
			GameSquare[,] Cells = new GameSquare[GameWidth, GameHeight];
			for (int i = 0; i < GameWidth; i++) {
				for (int j = 0; j < GameHeight; j++) {
					int xPosition = j * 20;
					int yPosition = i * 20;

					Cells [i, j] = new GameSquare (xPosition, yPosition, false, false, 0, false);  //Makes a board array with x,y,mine bool, clicked bool
				}																			
			}
			mines = (GameHeight * GameWidth) / dif; // Calculates # of mines based off 
			for (int i = 0; i < mines;) {               // total cells and desired difficulty
				int x = r.Next (0, GameWidth);
				int y = r.Next (0, GameHeight);
				if (Cells [x,y].isMine == false) {
					Cells[x,y].isMine = true;           // Adds mines to the board Array
					i++;
				}
			}
			for (int i = 0; i < GameWidth; i++) {  //Giving each square a "Connected Mines" value
				for (int j = 0; j < GameHeight; j++) {
					if (Cells [i, j].isMine == false) {
						int MineCount = 0; //variable to hold count of connected mined
						for (int a = i - 1; a <= i + 1; a++) { //looping through neighbor cells
							for (int b = j - 1; b <= j + 1; b++) {
								if ((a >= 0 && a < Cells.GetLength (0)) && (b >= 0 && b < Cells.GetLength (1))) {
									if (Cells[a,b].isMine == true) {
										MineCount++;
									}
								}
							}
						}
						Cells [i, j].connectedMines = MineCount;
					}
				}
			}

			return Cells;
		}

		// This function will set the Boolean for "clicked" to true on the cell that was clicked
		public void ClickedCell (GameSquare[,] Cells, int x, int y)
		{
			for (int i = 0; i < Cells.GetLength (0); i++) {
				for (int j = 0; j < Cells.GetLength (1); j++) {
					if(Cells[i,j].Contains(x,y)) {  //Calls to the GameSquare.Contains function.  
						Cells [i, j].isClicked = true; 
						if (Cells [i, j].isMine == false) {
							Cells [i, j].isClicked = true;
							ClearNeighbor(Cells, i, j);
						} else {
							GameState = 2;  //TODO: REPLACE WITH GAME OVER FUNCTION 
						}
					}
				}
			}
		}
		public void ClearNeighbor(GameSquare[,] Cells, int x, int y)
		{
			int mines = 0; //mine counter
			for (int i = x - 1; i <= x + 1; i++) {
				for (int j = y - 1; j <= y + 1; j++) {
					if ((i >= 0 && i < Cells.GetLength(0)) && (j >= 0 && j < Cells.GetLength(1))) {
						if (Cells [i, j].isMine == true) {
							mines++; //if there;s a mine within 1 block count it
						}
					}
				}
			}
			if (mines == 0) { //if there are no mines in 1 block, destroy all 8, and check their neighbors
				for (int i = x - 1; i <= x + 1; i++) {
					for (int j = y - 1; j <= y + 1; j++) {
						if ((i >= 0 && i < Cells.GetLength (0)) && (j >= 0 && j < Cells.GetLength (1))) {
							if (Cells [i, j].isClicked == false) { //if it hasn't been clicked, check it's neighbors too! 
								Cells [i, j].isClicked = true;    // important to do this now, to avoid passing it back and 
								ClearNeighbor (Cells, i, j);      // making an infinite loop							
							}
						}
					}
				}
			}
		}
		public void FlagCell (GameSquare[,] Cells, int x, int y) {
			for (int i = 0; i < Cells.GetLength (0); i++) {
				for (int j = 0; j < Cells.GetLength (1); j++) {
					if (Cells [i, j].Contains (x, y) && Cells[i,j].isClicked == false) {  //Calls to the GameSquare.Contains function.  
						Cells [i, j].isFlagged = !Cells [i, j].isFlagged; 
					}
				}
			}
		}
		public void IsWinner (GameSquare[,] Cells) {
			int NumOfMines = 0;
			int NumOfFlags = 0;
			for (int i = 0; i < Cells.GetLength (0); i++) {
				for (int j = 0; j < Cells.GetLength (1); j++) {
					if (Cells[i, j].isMine ==true && Cells[i, j].isFlagged == true) {
						NumOfMines++;
					}
					if (Cells [i, j].isFlagged == true) {
						NumOfFlags++;
					}
				}
			}
			if (NumOfFlags == NumOfMines && NumOfMines == mines) { //win condition
				GameState = 1;
			}
		}
	}
}


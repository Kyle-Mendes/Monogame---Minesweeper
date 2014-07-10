using System;
using Microsoft.Xna.Framework.Input;

namespace MineSweeper
{
	public class ComputerAI
	{
		Random r = new Random ();
		bool firstClick = false;

		public ComputerAI ()
		{
			
		}
		public void PlayGame(GameSquare[,] cells, GameBoard game) 
		{
//			int x = 0;
//			int y = 0;
			Console.WriteLine (firstClick);
			if (firstClick == false) {
				int randX = r.Next (0, cells.GetLength (1)) * 20 - 5;
				int randY = r.Next (0, cells.GetLength (0)) * 20 - 5;
				Mouse.SetPosition (randX, randY);
				game.ClickedCell (cells, randX, randY);
				firstClick = true;
			} else {
				for (int i = 0; i < cells.GetLength (0); i++) {
					for (int j = 0; j < cells.GetLength (1); j++) { //looping through game board
						if (cells [i, j].isClicked == true && cells [i, j].connectedMines == 1) { //if we find a square with only one connected mine...
							SingleConnectedMine (cells, game, i, j);
						}
					}
				}
			}
			

			//game.ClickedCell (cells, x, y); 
		}
		public void SingleConnectedMine(GameSquare[,] cells, GameBoard game, int x, int y)
		{
//			if (cells [x, y - 1].isClicked == true && cells [x - 1, y].isClicked == true && cells [x + 1, y].isClicked == true ) { //is it a corner?
//				for (int i = x - 1; i <= x + 1; i++) {
//					for (int j = y - 1; j <= y + 1; j++) {
//						if ((i >= 0 && i < cells.GetLength (0)) && (j >= 0 && j < cells.GetLength (1))) {
//							if (cells [i, j].isClicked == false) {
//								int xPos = cells [i, j].xPosition + 5;
//								int yPos = cells [i, j].yPosition - 5;
//								Mouse.SetPosition (xPos, yPos);
//								game.FlagCell (cells, xPos, yPos);
//							}
//						}
//					}
//				}
//			}
		}
	}
}


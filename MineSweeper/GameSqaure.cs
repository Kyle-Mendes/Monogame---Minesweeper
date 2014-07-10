using System;


namespace MineSweeper
{
	public class GameSquare
	{
		private int xPos;
		private int yPos;
		Boolean clicked;
		Boolean mine;
		private int mines;
		Boolean flag;

		public int xPosition {
			get {
				return xPos;
			}
			set {
				xPos = value;
			}
		}
		public int yPosition {
			get {
				return yPos;
			}
			set {
				yPos = value;
			}
		}
		public Boolean isClicked {
			get {
				return clicked;
			}
			set {
				clicked = value;
			}
		}
		public Boolean isMine {
			get {
				return mine;
			}
			set {
				mine = value;
			}
		}
		public int connectedMines {
			get {
				return mines;
			}
			set {
				mines = value;
			}
		}
		public Boolean isFlagged {
			get {
				return flag;
			}
			set {
				flag = value;
			}
		}

		public GameSquare (int x, int y, Boolean clicked, Boolean mine, int m, Boolean f)
		{
			this.xPos = x;
			this.yPos = y;
			this.clicked = clicked;
			this.mine = mine;
			this.mines = m;
			this.flag = f;
		}

		public Boolean Contains (int x, int y) { // This will check to see if the mouse position intersects with a cell
			int xWidth = this.xPosition + 20;    // And then assign the value of that cell to "clicked"
			int yWidth = this.yPosition + 20;
			if ((this.xPosition <= x && x <= xWidth) && (this.yPosition <= y && y <= yWidth)) {
				return true;
			} else {
				return false;
			}
		}

	}
}


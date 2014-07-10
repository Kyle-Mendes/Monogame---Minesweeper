#region File Description
//-----------------------------------------------------------------------------
// MineSweeperGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

#endregion

namespace MineSweeper
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Game1 : Game
	{

		#region Fields

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D mineTexture;
		GameBoard game = new GameBoard();
		GameSquare[,] GameCells;
		int DIFFICULTY;
		Boolean debug = false;
		private KeyboardState _currentKeyboardState;
		private KeyboardState _previousKeyboardState;
		int width = 30;
		int height = 20; //TODO: make a way to change this

		ComputerAI AI = new ComputerAI();

		#endregion

		#region Initialization

		public Game1 ()
		{
			//TODO: Add a menu system for settings (difficulty, width, etc)
			//TODO: Add a win condition (using flags)
			//TODO: add a reset function (gamestate = 0, new board)
			//TODO: Make mines happen after first click?




			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = width*20;  // set this value to the desired width of your window
			graphics.PreferredBackBufferHeight = height*20;   // set this value to the desired height of your window
			graphics.ApplyChanges();
			
			Content.RootDirectory = "Assets";

			graphics.IsFullScreen = false;
			this.IsMouseVisible = true;
			DIFFICULTY = 8; //Smaller value = harder
			GameCells = game.CreateGameBoard (width, height, DIFFICULTY);
		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
		}


		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);

			mineTexture = Content.Load<Texture2D> ("Mines");


		}

		#endregion

		#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
            		
			KeyboardState KBS = Keyboard.GetState ();
			MouseState mouseState = Mouse.GetState();

			int state = game.GameState;
			// Before handling input
			_currentKeyboardState = Keyboard.GetState();

			if(_currentKeyboardState.IsKeyDown(Keys.K) &&
				_previousKeyboardState.IsKeyUp(Keys.K))
			{
				debug = !debug;			
			}
			if(_currentKeyboardState.IsKeyDown(Keys.R) &&
				_previousKeyboardState.IsKeyUp(Keys.R))
			{
				GameCells = game.CreateGameBoard (width, height, DIFFICULTY);	
				game.GameState = 0;
			}
			if(_currentKeyboardState.IsKeyDown(Keys.Down) &&
				_previousKeyboardState.IsKeyUp(Keys.Down))
			{
				DIFFICULTY += 1;
				GameCells = game.CreateGameBoard (width, height, DIFFICULTY);	
				game.GameState = 0;
			}
			if(_currentKeyboardState.IsKeyDown(Keys.Up) &&
				_previousKeyboardState.IsKeyUp(Keys.Up))
			{
				DIFFICULTY -= 1;
				GameCells = game.CreateGameBoard (width, height, DIFFICULTY);	
				game.GameState = 0;
			}

			if (_currentKeyboardState.IsKeyDown (Keys.Space) &&
			   _previousKeyboardState.IsKeyUp (Keys.Space)) {  //Play the AI when you hit space
				AI.PlayGame (GameCells, game);
			}
				
			_previousKeyboardState = _currentKeyboardState;

			if (KBS.IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			if(mouseState.LeftButton == ButtonState.Pressed && state == 0) // GameState = 0 to make 
			{															   // so you can only click if not win/lose
				int x = mouseState.X;
				int y = mouseState.Y;
				game.ClickedCell (GameCells, x, y);  
			}
			if (mouseState.RightButton == ButtonState.Pressed && state == 0) {
				int x = mouseState.X;
				int y = mouseState.Y;
				game.FlagCell (GameCells, x, y);
				game.IsWinner (GameCells);
			}
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// 

		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			Rectangle clickableSquare = new Rectangle (0, 0, 20, 20);
			Rectangle mineSquare = new Rectangle (20, 0, 20, 20);
			Rectangle winMineSquare = new Rectangle (240, 0, 20, 20);
			Rectangle flaggedSquare = new Rectangle (40, 0, 20, 20);
			Rectangle clickedSquare = new Rectangle (60, 0, 20, 20);
			Rectangle winClickedSquare = new Rectangle (260, 0, 20, 20);
			Rectangle DrawSquare;

			Rectangle oneMines = new Rectangle (80, 0, 20, 20);
			Rectangle twoMines = new Rectangle (100, 0, 20, 20);
			Rectangle threeMines = new Rectangle (120, 0, 20, 20);
			Rectangle fourMines = new Rectangle (140, 0, 20, 20);
			Rectangle fiveMines = new Rectangle (160, 0, 20, 20);
			Rectangle sixMines = new Rectangle (180, 0, 20, 20);
			Rectangle sevenMines = new Rectangle (200, 0, 20, 20);
			Rectangle eightMines = new Rectangle (220, 0, 20, 20);



			spriteBatch.Begin ();
			for (int i = 0; i < GameCells.GetLength(0); i++) {
				for (int j = 0; j < GameCells.GetLength (1); j++) {
					Rectangle destinationRectangle = new Rectangle (GameCells [i, j].xPosition, GameCells [i, j].yPosition, 20, 20);
					if (GameCells[i,j].isFlagged == true) {
						if (game.GameState == 1) {
							DrawSquare = winMineSquare;
						} else {
							DrawSquare = flaggedSquare;
						}
					} else if (GameCells [i, j].isClicked == false && GameCells [i, j].isMine == false) {
						DrawSquare = clickableSquare;
					} else if (GameCells [i, j].isMine) {
						if (game.GameState == 2 || debug == true) {
							DrawSquare = mineSquare;
						} else if (game.GameState == 1) {
							DrawSquare = winMineSquare;
						} else {
							DrawSquare = clickableSquare;
						}
					}
					else {
						if (game.GameState == 1) {
							DrawSquare = winClickedSquare;
						} else if (GameCells [i, j].connectedMines == 1) {
							DrawSquare = oneMines;
						} else if (GameCells [i, j].connectedMines == 2) {
							DrawSquare = twoMines;
						} else if (GameCells [i, j].connectedMines == 3) {
							DrawSquare = threeMines;
						} else if (GameCells [i, j].connectedMines == 4) {
							DrawSquare = fourMines;
						} else if (GameCells [i, j].connectedMines == 5) {
							DrawSquare = fiveMines;
						} else if (GameCells [i, j].connectedMines == 6) {
							DrawSquare = sixMines;
						} else if (GameCells [i, j].connectedMines == 7) {
							DrawSquare = sevenMines;
						} else if (GameCells [i, j].connectedMines == 8) {
							DrawSquare = eightMines;
						} else {
							DrawSquare = clickedSquare;
						}
					}
					spriteBatch.Draw (mineTexture, destinationRectangle, DrawSquare, Color.White);
				}
			}
			spriteBatch.End ();
			base.Draw (gameTime);
		}

		#endregion
	}
}

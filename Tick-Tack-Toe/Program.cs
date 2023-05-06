﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Tick_Tack_Toe
{
	/// <summary>
	/// Custom console that suports colored messages ane number Input
	/// </summary>
	public static class MyConsole {

		/// <summary>
		/// Prints message in desired color
		/// </summary>
		/// <param name="message">Text of the message</param>
		/// <param name="color">color of the message</param>
		public static void Color(string message, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		/// <summary>
		/// Prints message in desired color without starting a new line.
		/// </summary>
		/// <param name="message">Text of the message</param>
		/// <param name="color">color of the message</param>
		public static void ColorSameLine(string message, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(message);
			Console.ResetColor();
		}

		/// <summary>
		/// Gets a integer number from the user
		/// </summary>
		/// <param name="min">Minimum number</param>
		/// <param name="max">Maximum number</param>
		/// <param name="prompt">Promt to display to the user</param>
		/// <returns>returns a number</returns>
		public static int GetNumber(int min, int max, string prompt = "Enter a number")
		{
			MyConsole.Color($"{prompt} ({min}..{max})", ConsoleColor.Green);
			for (; ; )
			{
				int number;

				for (; ; ) {
					string input = Console.ReadLine();
					bool valid = Int32.TryParse(input, out number);
					if (valid)
					{
						break;
					}
					else
					{
						MyConsole.Color("Your Input is not an integer number.", ConsoleColor.Red);
					}
				}

				if (number < min)
				{
					MyConsole.Color($"The number should be at least {min}.", ConsoleColor.Red);
				}	
				else if (number > max)
				{
					MyConsole.Color($"The number should be at most {max}.", ConsoleColor.Red);
				}
				else
				{
					return number;
				}
			}
		}
	}


	/// <summary>
	/// Board object holding the curent state of the game
	/// </summary>
	public class Board
	{
		/// <summary>
		/// number of rows
		/// </summary>
		public int Height { get; }

		/// <summary>
		/// number of coloms
		/// </summary>
		public int Width { get; }

		/// <summary>
		/// the individual cells of the board (0 - empty, 1 - player one, 2 - Platyer two)
		/// </summary>
		public int[,] Data { get; }

		/// <summary>
		/// creates a new board
		/// </summary>
		/// <param name="width">number of coloms</param>
		/// <param name="height">number of rows</param>
		public Board(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.Data = new int[height, width];
		}

		/// <summary>
		/// Is a point within the board range
		/// </summary>
		/// <param name="point">point to test</param>
		/// <returns>True if the point is valid</returns>
		public bool IsValid(Point point)
		{
			return 
				0 <= point.Row && point.Row < Height &&
				0 <= point.Col && point.Col < Width;
		}

		/// <summary>
		/// Is a point within the board range
		/// </summary>
		/// <param name="row">row</param>
		/// <param name="col">Column</param>
		/// <returns>True if the point is valid</returns>
		public bool IsValid(int row, int col)
		{
			return IsValid(new Point(row, col));
		}

		//will make this a option to play with
		//Console.WriteLine("  | 1 | 2 | 3 |");
		//Console.WriteLine("──┼───┼───┼───┼──");
		//Console.WriteLine("1 | X │ O │ X │");
		//Console.WriteLine("──┼───┼───┼───┼──");
		//Console.WriteLine("2 | X │ O │ O │");
		//Console.WriteLine("──┼───┼───┼───┼──");
		//Console.WriteLine("3 | O │ X │ X │");
		//Console.WriteLine("──┼───┼───┼───┼──");
		//Console.WriteLine("  |   |   |   |");

		/// <summary>
		/// Prints the board in the console
		/// </summary>
		public void Print()
		{
			for (int r = 0; r < Height; r++)
			{
				for (int c = 0; c < Width; c++)
				{
					MyConsole.ColorSameLine("|", ConsoleColor.White);
					if (Data[r, c] == 1)
					{
						MyConsole.ColorSameLine($"{Data[r, c]}", ConsoleColor.Blue);
					}
					else if (Data[r, c] == 2)
					{
						MyConsole.ColorSameLine($"{Data[r, c]}", ConsoleColor.DarkYellow);
					}
					else if (Data[r, c] == 0)
					{
						MyConsole.ColorSameLine($"{Data[r, c]}", ConsoleColor.White);
					}
				}
				Console.WriteLine("|");
			}
		}

		/// <summary>
		/// Is a point on the board empty
		/// </summary>
		/// <param name="place">point to test</param>
		/// <returns>True if the point is empty</returns>
		public bool IsFree(Point place)
		{
			return Data[place.Row, place.Col] == 0;
		}

		/// <summary>
		/// Places the piece on the game board
		/// </summary>
		/// <param name="place">point to place the piece at</param>
		/// <param name="player">player that the piece belongs to</param>
		public void PlacePiece(Point place, int player)
		{
			Data[place.Row, place.Col] = player + 1;
		}
	}

	/// <summary>
	/// point (row and column) on the board
	/// </summary>
	public class Point
	{
		/// <summary>
		/// zero-based row index
		/// </summary>
		public int Row { get; }

		/// <summary>
		/// zero-based column index
		/// </summary>
		public int Col { get; }

		/// <summary>
		///	Creates a new point
		/// </summary>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		public Point(int row, int col)
		{
			this.Row = row;
			this.Col = col;
		}
	}

	/// <summary>
	/// The main program
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// number of pieces next to each other requierd to win
		/// </summary>
		static int PiecesToWin = 3;

		/// <summary>
		/// Checks if a player has a winning row at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if the row is winning</returns>
		private static bool CheckIfPlayerWonRow(Board board, int player, int row, int col)
		{
			if (col + PiecesToWin > board.Width) return false;

			for (int i = 0; i < PiecesToWin; i++)
			{
				if (board.Data[row, col + i] != player + 1) return false;
			}

			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning column at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if the column is winning</returns>
		private static bool CheckIfPlayerWonCol(Board board, int player, int row, int col)
		{
			if (row + PiecesToWin > board.Height) return false;

			for (int i = 0; i < PiecesToWin; i++)
			{
				if (board.Data[row + i, col] != player + 1) return false;
			}

			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning up-right (/) diagonal at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if /-diagonal is winning</returns>
		private static bool CheckIfPlayerWonDiagUp(Board board, int player, int row, int col)
		{
			if (!board.IsValid(row - PiecesToWin + 1, col + PiecesToWin - 1)) return false;
 
			for (int i = 0; i < PiecesToWin; i++)
			{
				if (board.Data[row - i, col + i] != player + 1) return false;
			}
			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning down-right (\) diagonal at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if \-diagonal is winning</returns>
		private static bool CheckIfPlayerWonDiagDown(Board board, int player, int row, int col)
		{
			if (!board.IsValid(row + PiecesToWin - 1, col + PiecesToWin - 1)) return false; 

			for (int i = 0; i < PiecesToWin; i++)
			{
			if (board.Data[row + i, col + i] != player + 1) return false; 
			}

			return true;
		}

		/// <summary>
		/// Checks if a player won. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <returns>true if the player won</returns>
		static bool CheckIfPlayerWon(Board board, int player)
		{
			for (int row = 0; row < board.Height; row++)
			{
				for (int col = 0; col < board.Width; col++)
				{
					if (
						CheckIfPlayerWonRow(board, player, row, col) ||
						CheckIfPlayerWonCol(board, player, row, col) ||
						CheckIfPlayerWonDiagUp(board, player, row, col) ||
						CheckIfPlayerWonDiagDown(board, player, row, col)
					)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Ensures that a condition is true.
		/// </summary>
		/// <param name="test">condition to check</param>
		/// <param name="msg">exception message to use if the condition fails</param>
		/// <exception cref="Exception">an exception thrown if the condition fails</exception>
		private static void check(bool test, string msg)
		{
			if (!test) throw new Exception(msg);
		}

		/// <summary>
		/// Asks the user for a point (two numbers).
		/// </summary>
		/// <param name="minx">minimum required for the first number (x, column)</param>
		/// <param name="miny">minimum required for the second number (y, row)</param>
		/// <param name="maxx">maximum required for the first number (x, column)</param>
		/// <param name="maxy">maximum required for the second number (y, row)</param>
		/// <param name="prompt">prompt to display to the user</param>
		/// <param name="color">color to use for messages</param>
		/// <returns></returns>
		///todo: when a invalid Input is enterd the Input loses its color
		static Point Input(int minx, int miny, int maxx, int maxy, String prompt="Please enter two numbers separated with a comma.", ConsoleColor color = ConsoleColor.White)
		{
			MyConsole.Color(prompt, ConsoleColor.Green);
			Console.ForegroundColor = color;
			for (; ; ) {
				string input = "";
				input = Console.ReadLine();
				Console.ResetColor();
				string[] words = input.Split(',', '.', '/');
				int x, y;
				try
				{
					check(words.Length >= 2, "At least two numbers required");
					check(words[0] != "" && words[1] != "", "Input must cointain something");
					bool validX = Int32.TryParse(words[0], out x);
					bool validY = Int32.TryParse(words[1], out y);
					check(validX, "First item is not a number");
					check(validX, "Second item is not a number");
					check(minx <= x && x <= maxx, $"The first number must be in [{minx}, {maxx}]");
					check(miny <= y && y <= maxy, $"The second number must be in [{miny}, {maxy}]");
				
					return new Point(x, y);
				}
				catch(Exception e)
				{
					MyConsole.Color($"Invalid Input, {e.Message}", ConsoleColor.Red);
				}
			}
		}

		/// <summary>
		///  Asks a player to place a piece on the board
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <returns>A list containg two numbers showing the position the piece was placed at</returns>
		static List<int> PlacePiece(Board board, int player)
		{
			int typeOfPlayer = player % 2; //Type Of player that should be playing
			List<int> lastPlayed = new List<int>();	// todo replace with point object
			for (; ; )
			{
				Point place = Input(1, 1, board.Width, board.Height, color: typeOfPlayer == 0 ? ConsoleColor.Blue : ConsoleColor.DarkYellow);
				place = new Point(row: place.Row - 1, col: place.Col - 1);

				lastPlayed.Add(place.Row);
				lastPlayed.Add(place.Col);

				if (board.IsFree(place))
				{
					if (typeOfPlayer == 0)
					{
						MyConsole.Color($"row:{place.Row + 1} col:{place.Col + 1}", ConsoleColor.Blue);
					}
					else if (typeOfPlayer == 1)
					{
						MyConsole.Color($"row:{place.Row + 1} col:{place.Col + 1}", ConsoleColor.DarkYellow);
					}
					
					board.PlacePiece(place, player);
					return lastPlayed;
				}
				else
				{
					MyConsole.Color("place already taken", ConsoleColor.Red);
				}
			}
		}
		/// <summary>
		/// The first type of the ai's turn (random strategy)
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		static void AiPlayRandom(Board board, int player)
		{
			Random rnd = new Random();
			for (; ; )
			{
				int aiOptionY = rnd.Next(0, board.Height);
				int aiOptionX = rnd.Next(0, board.Width);

				if (board.Data[aiOptionX, aiOptionY] == 0)
				{
					MyConsole.Color($"row:{aiOptionX + 1} col:{aiOptionY + 1}", ConsoleColor.DarkYellow);
					board.Data[aiOptionX, aiOptionY] = player + 1;
					return;
				}
				else
				{
					MyConsole.Color("Ai failed. place already taken", ConsoleColor.Red);
				}
			}
		}

		/// <summary>
		/// The second type of the ai's turn (Playing around the other player last piece)
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="player">player index (0 or 1)</param>
		/// <param name="lastPiecePlayed">oponents last played position</param>
		static void AiPlayLocal(Board board, int player, List<int> lastPiecePlayed)
		{
			int lpy = lastPiecePlayed[0] + 1;
			int plx = lastPiecePlayed[1] + 1;
			Random rnd = new Random();
			for (; ; )
			{
				int variance = rnd.Next(0, 3);
				int aiOptionY = rnd.Next(lpy - variance, lpy + variance);
				int aiOptionX = rnd.Next(plx - variance, plx + variance);
				if (0 <= aiOptionY && aiOptionY < board.Height && 0 <= aiOptionX && aiOptionX < board.Width)
				{
					if (board.Data[aiOptionY, aiOptionX] == 0)
					{
						board.Data[aiOptionY, aiOptionX] = 2;
						MyConsole.Color($"row:{aiOptionY + 1} col:{aiOptionX + 1}", ConsoleColor.DarkYellow);
						break;
					}
				}
			}
		}

		//this would be a new bot time (either a AI or a score system)
		static void AiPlaySmart(Board board, int player)
		{
			//not implemented yet
		}


		/// <summary>
		/// AI strategy to use
		/// </summary>
		enum AiType
		{
			None,
			Random,
			Smart,
			Local
		}
		/// <summary>
		/// Ask player the player what about the ai strategy
		/// </summary>
		/// <returns>returns the ai strategy</returns>
		private static AiType GetAiType()
		{
			MyConsole.Color("Do you want an AI to play against you?", ConsoleColor.Green);
			MyConsole.Color("Y/N", ConsoleColor.Green);
			string input = Console.ReadLine();

			if (input.ToLower() == "n")
			{
				return AiType.None;
			}
			MyConsole.Color("Please enter the type of the AI (1/2/3)", ConsoleColor.Green);
			input = Console.ReadLine();
			if (input == "1")
			{
				return AiType.Random;
			}
			
			return AiType.Local;
		}

		/// <summary>
		/// Resets the game
		/// </summary>
		/// <param name="board">The game board</param>
		/// <param name="maxPlayes">How many turn must happen until a draw is delcared</param>
		static void RestartGame()
		{
			Console.Clear();
		}

		/// <summary>
		/// Main game loop
		/// </summary>
		/// <param name="args">Command line arguments</param>
		static void Main(string[] args)
		{
			//a bool for if the program should end or not
			bool exit = false; //default
			for (;exit == false;) //main game loop
			{

				//gets the board size
				Point boardSize = Input(3, 3, 50, 50, "Please enter two number for the size of the board (width, height)");

				//sets the number of X/O you need to win
				int maxPiecesToWin = Math.Max(boardSize.Row, boardSize.Col);
				if (3 < maxPiecesToWin)
				{
					PiecesToWin = MyConsole.GetNumber(3, maxPiecesToWin, "Please enter how many X's next to each other are needed to win");
				}
				else
				{
					PiecesToWin = 3;
				}
				//gets the Ai type (random, near player)
				AiType aiType = GetAiType();
				int maxPlays = boardSize.Col * boardSize.Row - 1; //sets the number of posible turn until a draw is declared
				Board board = new Board(boardSize.Col, boardSize.Row); //creates the board
				List<int> lastPiecePlayed = new List<int>(); //the ai (near player type) can get the last piece played from the player to play.
				Console.Clear();

				//the game loop start here
				for (int i = 0; ; i++) // inf loop
				{
					int player = i % 2; //sets the player that should play this round
					board.Print(); //prints the curent board
					if (aiType == AiType.None) //checks if a player or a ai should play
					{
						PlacePiece(board, player);
					}
					else {
						if (player % 2 == 0) // player playing
						{
							lastPiecePlayed = PlacePiece(board, player);
						}
						else if (player % 2 == 1) // Ai playing
						{ //gets the type of the ai selected
							Thread.Sleep(1000); // so the program seems to think about the playing
							switch (aiType)
							{
								case AiType.Random:
									AiPlayRandom(board, player);
									break;
								case AiType.Local:
									AiPlayLocal(board, player, lastPiecePlayed);
									break;
								case AiType.Smart:
									AiPlaySmart(board, player);
									break;
							}
						}
					}
					//checks if the current player won.
					if (CheckIfPlayerWon(board, player)) //win
					{
						board.Print();
						MyConsole.Color($"player{player + 1} Won", ConsoleColor.Magenta);
						break;
					}
					if (maxPlays == i) //draw
					{
						board.Print();
						MyConsole.Color("Draw", ConsoleColor.Magenta);
						break;
					}
				}

				MyConsole.Color("Do you want to exit? (Y/N)", ConsoleColor.Green); // give a player a option to exit the program
				string shouldYouExit = Console.ReadLine().ToUpper(); // gets the Input and put's it in a string
				if (shouldYouExit == "Y") // checks what the answer is
				{
					exit = true; // exits
				}else
				{
					exit = false; // continues
					RestartGame(); // restarts the game
				}
			}

			// exit the aplication
			Console.Clear();
			Console.WriteLine("Exiting application...");
			Thread.Sleep(500); //waits .5 seconds
		}
	}
}

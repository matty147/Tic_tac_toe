using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Tick_Tack_Toe
{
	/// <summary>
	/// Custom console that suports colored messages ane number input
	/// </summary>
	public static class MyConsole {

		/// <summary>
		/// Prints message in desired color
		/// </summary>
		/// <param name="Message">Text of the message</param>
		/// <param name="Color">Color of the message</param>
		public static void Color(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.WriteLine(Message);
			Console.ResetColor();
		}

		/// <summary>
		/// Prints message in desired color without starting a new line.
		/// </summary>
		/// <param name="Message">Text of the message</param>
		/// <param name="Color">Color of the message</param>
		public static void ColorSameLine(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.Write(Message);
			Console.ResetColor();
		}

		/// <summary>
		/// Gets a integer number from the user
		/// </summary>
		/// <param name="min">Minimum number</param>
		/// <param name="max">Maximum number</param>
		/// <param name="prompt">Promt to display to the user</param>
		/// <returns>returns a number</returns>
		public static int getNumber(int min, int max, string prompt = "Enter a number")
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
						MyConsole.Color("Your input is not an integer number.", ConsoleColor.Red);
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
		/// Number of rows
		/// </summary>
		public int height { get; }

		/// <summary>
		/// Number of coloms
		/// </summary>
		public int width { get; }

		/// <summary>
		/// the individual cells of the board (0 - empty, 1 - Player one, 2 - Platyer two)
		/// </summary>
		public int[,] data { get; }

		/// <summary>
		/// creates a new board
		/// </summary>
		/// <param name="width">Number of coloms</param>
		/// <param name="height">Number of rows</param>
		public Board(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.data = new int[height, width];
		}

		/// <summary>
		/// Is a point within the board range
		/// </summary>
		/// <param name="point">Point to test</param>
		/// <returns>True if the point is valid</returns>
		public bool isValid(Point point)
		{
			return 
				0 <= point.row && point.row < height &&
				0 <= point.col && point.col < width;
		}

		/// <summary>
		/// Is a point within the board range
		/// </summary>
		/// <param name="row">Row</param>
		/// <param name="col">Column</param>
		/// <returns>True if the point is valid</returns>
		public bool isValid(int row, int col)
		{
			return isValid(new Point(row, col));
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
		public void print()
		{
			for (int r = 0; r < height; r++)
			{
				for (int c = 0; c < width; c++)
				{
					MyConsole.ColorSameLine("|", ConsoleColor.White);
					if (data[r, c] == 1)
					{
						MyConsole.ColorSameLine($"{data[r, c]}", ConsoleColor.Blue);
					}
					else if (data[r, c] == 2)
					{
						MyConsole.ColorSameLine($"{data[r, c]}", ConsoleColor.DarkYellow);
					}
					else if (data[r, c] == 0)
					{
						MyConsole.ColorSameLine($"{data[r, c]}", ConsoleColor.White);
					}
				}
				Console.WriteLine("|");
			}
		}

		/// <summary>
		/// Is a point on the board empty
		/// </summary>
		/// <param name="place">Point to test</param>
		/// <returns>True if the point is empty</returns>
		public bool isFree(Point place)
		{
			return data[place.row, place.col] == 0;
		}

		/// <summary>
		/// Places the piece on the game board
		/// </summary>
		/// <param name="place">Point to place the piece at</param>
		/// <param name="player">Player that the piece belongs to</param>
		public void placePiece(Point place, int player)
		{
			data[place.row, place.col] = player + 1;
		}
	}

	/// <summary>
	/// Point (row and column) on the board
	/// </summary>
	public class Point
	{
		/// <summary>
		/// zero-based row index
		/// </summary>
		public int row { get; }

		/// <summary>
		/// zero-based column index
		/// </summary>
		public int col { get; }

		/// <summary>
		///	Creates a new point
		/// </summary>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		public Point(int row, int col)
		{
			this.row = row;
			this.col = col;
		}
	}

	/// <summary>
	/// The main program
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// Number of pieces next to each other requierd to win
		/// </summary>
		static int d = 3;

		/// <summary>
		/// Checks if a player has a winning row at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if the row is winning</returns>
		static bool CheckIfPlayerWonRow(Board board, int Player, int row, int col)
		{
			if (col + d > board.width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board.data[row, col + i] != Player + 1) return false;
			}

			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning column at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if the column is winning</returns>
		static bool CheckIfPlayerWonCol(Board board, int Player, int row, int col)
		{
			if (row + d > board.height) return false;

			for (int i = 0; i < d; i++)
			{
				if (board.data[row + i, col] != Player + 1) return false;
			}

			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning up-right (/) diagonal at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if /-diagonal is winning</returns>
		static bool CheckIfPlayerWonDiagUp(Board board, int Player, int row, int col)
		{
			if (!board.isValid(row - d + 1, col + d - 1)) return false;
 
			for (int i = 0; i < d; i++)
			{
				if (board.data[row - i, col + i] != Player + 1) return false;
			}
			return true;
		}

		/// <summary>
		/// todo Checks if a player has a winning down-right (\) diagonal at a particular place on the board. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <param name="row">zero-based row index</param>
		/// <param name="col">zero-based column index</param>
		/// <returns>true if \-diagonal is winning</returns>
		static bool CheckIfPlayerWonDiagDown(Board board, int Player, int row, int col)
		{
			if (!board.isValid(row + d - 1, col + d - 1)) return false; 

			for (int i = 0; i < d; i++)
			{
			if (board.data[row + i, col + i] != Player + 1) return false; 
			}

			return true;
		}

		/// <summary>
		/// Checks if a player won. 
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <returns>true if the player won</returns>
		static bool CheckIfPlayerWon(Board board, int Player)
		{
			for (int row = 0; row < board.height; row++)
			{
				for (int col = 0; col < board.width; col++)
				{
					if (
						CheckIfPlayerWonRow(board, Player, row, col) ||
						CheckIfPlayerWonCol(board, Player, row, col) ||
						CheckIfPlayerWonDiagUp(board, Player, row, col) ||
						CheckIfPlayerWonDiagDown(board, Player, row, col)
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
		static void check(bool test, string msg)
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
		/// <param name="prompt">Prompt to display to the user</param>
		/// <param name="color">Color to use for messages</param>
		/// <returns></returns>
		///todo: when a invalid input is enterd the input loses its color
		static Point input(int minx, int miny, int maxx, int maxy, String prompt="Please enter two numbers separated with a comma.", ConsoleColor color = ConsoleColor.White)
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
					bool ValidX = Int32.TryParse(words[0], out x);
					bool ValidY = Int32.TryParse(words[1], out y);
					check(ValidX, "First item is not a number");
					check(ValidX, "Second item is not a number");
					check(minx <= x && x <= maxx, $"The fist number must be in [{minx}, {maxx}]");
					check(miny <= y && y <= maxy, $"The fist number must be in [{miny}, {maxy}]");
				
					return new Point(x, y);
				}
				catch(Exception e)
				{
					MyConsole.Color($"Invalid input, {e.Message}", ConsoleColor.Red);
				}
			}
		}

		/// <summary>
		///  Asks a player to place a piece on the board
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <returns>A list containg two numbers showing the position the piece was placed at</returns>
		static List<int> PlacePiece(Board board, int Player)
		{
			int TypeOfPlayer = Player % 2; //Type Of Player that should be playing
			List<int> LastPlayed = new List<int>();	// todo replace with Point object
			for (; ; )
			{
				Point place = input(1, 1, board.width, board.height, color: TypeOfPlayer == 0 ? ConsoleColor.Blue : ConsoleColor.DarkYellow);
				place = new Point(row: place.row - 1, col: place.col - 1);

				LastPlayed.Add(place.row);
				LastPlayed.Add(place.col);

				if (board.isFree(place))
				{
					if (TypeOfPlayer == 0)
					{
						MyConsole.Color($"Row:{place.row + 1} Col:{place.col + 1}", ConsoleColor.Blue);
					}
					else if (TypeOfPlayer == 1)
					{
						MyConsole.Color($"Row:{place.row + 1} Col:{place.col + 1}", ConsoleColor.DarkYellow);
					}
					
					board.placePiece(place, Player);
					return LastPlayed;
				}
				else
				{
					MyConsole.Color("Place already taken", ConsoleColor.Red);
				}
			}
		}
		/// <summary>
		/// The first type of the ai's turn (random strategy)
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		static void AiPlayT1(Board board, int Player)
		{
			Random rnd = new Random();
			for (; ; )
			{
				int AiOptionY = rnd.Next(0, board.height);
				int AiOptionX = rnd.Next(0, board.width);

				if (board.data[AiOptionX, AiOptionY] == 0)
				{
					MyConsole.Color($"Row:{AiOptionX + 1} Col:{AiOptionY + 1}", ConsoleColor.DarkYellow);
					board.data[AiOptionX, AiOptionY] = Player + 1;
					return;
				}
				else
				{
					MyConsole.Color("Ai failed. Place already taken", ConsoleColor.Red);
				}
			}
		}

		/// <summary>
		/// The second type of the ai's turn (Playing around the other player last piece)
		/// </summary>
		/// <param name="board">board object</param>
		/// <param name="Player">player index (0 or 1)</param>
		/// <param name="LastPiecePlayed">oponents last played position</param>
		static void AiPlayT2(Board board, int Player, List<int> LastPiecePlayed)
		{
			int LPY = LastPiecePlayed[0] + 1;
			int LPX = LastPiecePlayed[1] + 1;
			Random rnd = new Random();
			for (; ; )
			{
				int Variance = rnd.Next(0, 3);
				int AiOptionY = rnd.Next(LPY - Variance, LPY + Variance);
				int AiOptionX = rnd.Next(LPX - Variance, LPX + Variance);
				if (0 <= AiOptionY && AiOptionY < board.height && 0 <= AiOptionX && AiOptionX < board.width)
				{
					if (board.data[AiOptionY, AiOptionX] == 0)
					{
						board.data[AiOptionY, AiOptionX] = 2;
						MyConsole.Color($"Row:{AiOptionY + 1} Col:{AiOptionX + 1}", ConsoleColor.DarkYellow);
						break;
					}
				}
			}
		}

		//not implemented yet
		//this would be a new bot time (either a AI or a score system)
		static void AiPlayT3(Board board, int Player)
		{
		}

		/// <summary>
		/// todo
		/// </summary>
		/// <param name="Words"></param>
		/// <param name="Boardx"></param>
		/// <param name="Boardy"></param>
		/// <param name="BMax"></param>
		/// <param name="BMin"></param>
		/// <returns></returns>
		static bool IsBoardSizeValid(string[] Words,int Boardx,int Boardy,int BMax,int BMin)
		{
			if (Words.Length >= 2)
			{
				if (Words[0] != "" && Words[1] != "")
				{
					bool ValidX = Int32.TryParse(Words[0], out Boardx);
					bool ValidY = Int32.TryParse(Words[1], out Boardy);
					if (ValidX && ValidY)
					{
						if (Boardy >= BMin && Boardy <= BMax && Boardx >= BMin && Boardx <= BMax) //make a method for this crap
						{
							return true;
						}
						else MyConsole.Color($"Invalid input. Please enter values from {BMin} to {BMax}.", ConsoleColor.Red);
					}
					else MyConsole.Color("Invalid input. X and Y coordinates must be numeric values.", ConsoleColor.Red);
				}
				else MyConsole.Color("Invalid input. X and Y coordinates cannot be empty.", ConsoleColor.Red);
			}
			else MyConsole.Color("Invalid input. Please enter two values separated by a space, dot or a backslash.", ConsoleColor.Red);
			return false;
		}

		/// <summary>
		/// Ask Player for the size of the board they want
		/// </summary>
		/// <returns>size of the board</returns>
		private static Point GetBoardSize()
		{
			// todo replace with input(...)

			MyConsole.Color("Please enter two number for the size of the table (width, height)", ConsoleColor.Green);
			int Boardx = 0, Boardy = 0;
			int BMax = 50, BMin = 3;
			string input = Console.ReadLine();
			string PLastPlace = input;
			string[] Words = input.Split(',', '.', '/');

			if (IsBoardSizeValid(Words, Boardx, Boardy, BMax, BMin)) // if number are 0 and then changed it think it is good but the board will not exist :/
			{
				Boardx = Int32.Parse(Words[1]);
				Boardy = Int32.Parse(Words[0]);
				return new Point(Boardy, Boardx);
			}
			else
			{
				return GetBoardSize();
			}
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
			MyConsole.Color("Do you want a ai?", ConsoleColor.Green);
			MyConsole.Color("Y/N", ConsoleColor.Green);
			string input = Console.ReadLine();

			if (input.ToLower() == "n")
			{
				return AiType.None;
			}
			MyConsole.Color("Please enter the type of the bot (1/2/3)", ConsoleColor.Green);
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
		/// <param name="MaxPlayes">How many turn must happen until a draw is delcared</param>
		static void RestartGame(Board board, int MaxPlayes)
		{
			//Array.Clear(board.data, 0,MaxPlayes -1); //this is usles
			Console.Clear();
		}

		/// <summary>
		/// Main game loop
		/// </summary>
		/// <param name="args">Command line arguments</param>
		static void Main(string[] args)
		{
			//a bool for if the program should end or not
			bool Exit = false; //default
			for (;Exit == false;) //main game loop
			{

				//gets the board size
				Point boardSize = input(3, 3, 50, 50, "Please enter two number for the size of the board (width, height)"
				);

				//sets the number of X/O you need to win
				int maxD = Math.Max(boardSize.row, boardSize.col);
				if (3 < maxD)
				{
					d = MyConsole.getNumber(3, maxD, "Please enter how many X's next to each other are needed to win");
				}
				else
				{
					d = 3;
				}
				//gets the Ai type (random, near player)
				AiType aiType = GetAiType();
				int MaxPlays = boardSize.col * boardSize.row - 1; //sets the number of posible turn until a draw is declared
				Board board = new Board(boardSize.col, boardSize.row); //creates the board
			List<int> LastPiecePlayed = new List<int>(); //the ai (near player type) can get the last piece played from the player to play.
			Console.Clear();

				//the game loop start here
				for (int i = 0; ; i++) // inf loop
				{
					int Player = i % 2; //sets the player that should play this round
					board.print(); //prints the curent board
					if (aiType == AiType.None) //checks if a player or a ai should play
					{
						PlacePiece(board, Player);
					}
					else {
						if (Player % 2 == 0) //Player
						{
							LastPiecePlayed = PlacePiece(board, Player);
						}
						else if (Player % 2 == 1) //Ai
						{ //gets the type of the ai selected
							switch (aiType)
							{
								case AiType.Random:
									AiPlayT1(board, Player);
									break;
								case AiType.Local:
									AiPlayT2(board, Player, LastPiecePlayed);
									break;
								case AiType.Smart:
									AiPlayT3(board, Player);
									break;
							}
							Thread.Sleep(1000);
						}
					}
					//checks if a somebody hase won.
					if (CheckIfPlayerWon(board, Player)) //win
					{
						board.print();
						MyConsole.Color($"Player{Player + 1} Won", ConsoleColor.Magenta);
						break;
					}
					if (MaxPlays == i) //draw
					{
						board.print();
						MyConsole.Color("Draw", ConsoleColor.Magenta);
						break;
					}
				}
				MyConsole.Color("Do you want to exit? (Y/N)", ConsoleColor.Green); //give a player a option to exit the program
				string ShouldYouExit = Console.ReadLine().ToUpper(); //gets the input and put's it in a string
				if (ShouldYouExit == "Y") //checks what the answer is
				{
					Exit = true; //Exits
				}else
				{
					Exit = false; //continues
					RestartGame(board , MaxPlays); //restarts the game
				}
			}
			//exist the aplication
			Console.Clear();
			Console.WriteLine("Exiting aplication...");
			Thread.Sleep(500); //waits .5 seconds
		}
	}
}

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
	public static class MyConsole {
		public static void Color(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.WriteLine(Message);
			Console.ResetColor();
		}
		public static void ColorNL(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.Write(Message);
			Console.ResetColor();
		}

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



	public class Board
	{
		public int height { get; }
		public int width { get; }
		public int[,] data { get; }


		public Board(int width, int height)
		{
			this.width = width;
			this.height = height;
			this.data = new int[height, width];
		}

		public bool isValid(Point point)
		{
			return 
				0 <= point.row && point.row < height &&
				0 <= point.col && point.col < width;
		}

		public bool isValid(int row, int col)
		{
			return isValid(new Point(row, col));
		}
		public void print()
		{
			for (int r = 0; r < height; r++)
			{
				for (int c = 0; c < width; c++)
				{
					MyConsole.ColorNL("|", ConsoleColor.White);
					if (data[r, c] == 1)
					{
						MyConsole.ColorNL($"{data[r, c]}", ConsoleColor.Blue);
					}
					else if (data[r, c] == 2)
					{
						MyConsole.ColorNL($"{data[r, c]}", ConsoleColor.DarkYellow);
					}
					else if (data[r, c] == 0)
					{
						MyConsole.ColorNL($"{data[r, c]}", ConsoleColor.White);
					}
				}
				Console.WriteLine("|");
			}
		}

		public bool isFree(Point place)
		{
			return data[place.row, place.col] == 0;
		}

		public void placePiece(Point place, int player)
		{
			data[place.row, place.col] = player + 1;
		}

		public bool isFull() { return true; }	// todo
		
	}

	public class Point
	{
		public int row { get; }
		public int col { get; }

		public Point(int row, int col)
		{
			this.row = row;
			this.col = col;
		}
	}

	internal class Program
	{
		static int d = 3;


		static bool CheckIfPlayerWonRow(Board board, int Player, int row, int col)
		{
			if (col + d > board.width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board.data[row, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonCol(Board board, int Player, int row, int col)
		{
			if (row + d > board.height) return false;

			for (int i = 0; i < d; i++)
			{
				if (board.data[row + i, col] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagUp(Board board, int Player, int row, int col)
		{
			if (!board.isValid(row - d + 1, col + d - 1)) return false;
 
			for (int i = 0; i < d; i++)
			{
				if (board.data[row - i, col + i] != Player + 1) return false;
			}
			return true;
		}

		static bool CheckIfPlayerWonDiagDown(Board board, int Player, int row, int col)
		{
			if (!board.isValid(row + d - 1, col + d - 1)) return false; 

			for (int i = 0; i < d; i++)
			{
			if (board.data[row + i, col + i] != Player + 1) return false; 
			}

			return true;
		}

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

		static void check(bool test, string msg)
		{
			if (!test) throw new Exception(msg);
		}

		static Point input(int minx, int miny, int maxx, int maxy, ConsoleColor color = ConsoleColor.White)
		{
			MyConsole.Color("Please enter two numbers separated with a comma.", ConsoleColor.Green);
			//I could write the player who should be playing but idk about that
			for (; ; ) {
				string input = "";
				Console.ForegroundColor = color;
				input = Console.ReadLine();//for some reason the input take 2.2 as 3.3
				Console.ResetColor();
				string[] words = input.Split(',', '.', '/');
				int x, y;
				try
				{
					check(words.Length >= 2, "At least two numbers required"); // if (words == "") {//input is for some reason 1(2)}
					check(words[0] != "" && words[1] != "", "Input must cointain something");
					bool ValidX = Int32.TryParse(words[0], out x);
					bool ValidY = Int32.TryParse(words[1], out y);
					check(ValidX, "First item is not a number");
					check(ValidX, "Second item is not a number");
					check(minx <= x && x < maxx, $"The fist number must be in [{minx + 1}, {maxx})");
					check(miny <= y && y < maxy, $"The fist number must be in [{miny + 1}, {maxy})");
					return new Point(y, x);
				}
				catch(Exception e)
				{
					MyConsole.Color($"Invalid input, {e.Message}", ConsoleColor.Red);
				}
			}
		}

		static List<int> PlacePiece(Board board, int Player)
		{
			int TypeOfPlayer = Player % 2; //Type Of Player that should be playing
			List<int> LastPlayed = new List<int>();
			for (; ; )
			{
				Point place = input(0, 0, board.width, board.height, TypeOfPlayer == 0 ? ConsoleColor.Blue : ConsoleColor.DarkYellow);

				LastPlayed.Add(place.row);
				LastPlayed.Add(place.col);

				if (board.isFree(place))
				{
					if (TypeOfPlayer == 0)
					{
						MyConsole.Color($"Row:{place.row + 1} Col:{place.col + 1}", ConsoleColor.Blue);
					}else if (TypeOfPlayer == 1)
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

		static void AiPlayT3(Board board, int Player)
		{

		}

		static bool CanPlay(string[] Words,int Boardx,int Boardy,int BMax,int BMin)
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
		private static Point GetBoardSize()
		{
			MyConsole.Color("Please enter two number for the size of the table (width, height)", ConsoleColor.Green);
				int Boardx = 0, Boardy = 0;
				int BMax = 50, BMin = 3;
				string input = Console.ReadLine();
				string PLastPlace = input;
				string[] Words = input.Split(',', '.', '/');
			if (CanPlay(Words, Boardx, Boardy, BMax, BMin)) // if number are 0 and then changed it think it is good but the board will not exist :/
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

		enum AiType
		{
			None,
			Random,
			Smart,
			Local
		}

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
			MyConsole.Color("(invalid input will result in the game going singleplayer)", ConsoleColor.DarkYellow);
			input = Console.ReadLine();

			if (input == "1")
			{
				return AiType.Random;
			}
			
			return AiType.Local;
		}
		static void RestartGame(Board board, int MaxPlayes)
		{
			Array.Clear(board.data, 0,MaxPlayes -1);
			Console.Clear();
		}

		static int FitInto(int value, int min, int max)
		{
			int tmp = Math.Max(value, min);
			return Math.Min(tmp, max);
		}

		static void Main(string[] args)
		{
			bool Exit = false;
			for (;Exit == false;)
			{
				Point boardSize = GetBoardSize();

				int maxD = Math.Max(boardSize.row, boardSize.col);
				if (3 < maxD)
				{
					d = MyConsole.getNumber(3, maxD, "Please enter how many X's next to each other are needed to win");
				}
				else
				{
					d = 3;
				}
				AiType aiType = GetAiType();
				int MaxPlays = boardSize.col * boardSize.row - 1;
				//check if a number is a decimal
				//if (!Decimal.TryParse(words[0], out <output>))
				//number will alwayes be positive
				//Math.Abs();
				Board board = new Board(boardSize.col, boardSize.row);
			List<int> LastPiecePlayed = new List<int>();
			Console.Clear();
				for (int i = 0; ; i++) // inf loop // strange setup of player selection // have to change
				{
					int Player = i % 2;
					board.print();
					if (aiType == AiType.None)
					{
						PlacePiece(board, Player);
					}
					else {
						if (Player % 2 == 0) //Player
						{
							LastPiecePlayed = PlacePiece(board, Player);
						}
						else if (Player % 2 == 1) //Ai
						{
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

					if (CheckIfPlayerWon(board, Player))
					{
						board.print();
						MyConsole.Color($"Player{Player + 1} Won", ConsoleColor.Magenta);
						break;
					}
					if (MaxPlays == i)
					{
						board.print();
						MyConsole.Color("Draw", ConsoleColor.Magenta);
						break;
					}
				}
				MyConsole.Color("Do you want to exit? (Y/N)", ConsoleColor.Green);
				string ShouldYouExit = Console.ReadLine().ToUpper();
				if (ShouldYouExit == "Y")
				{
					Exit = true;
				}else
				{
					Exit = false;
					RestartGame(board , MaxPlays);
				}
			}
			Console.Clear();
			Console.WriteLine("Exiting aplication...");
			Thread.Sleep(500); // .5 seconds
			//Console.ReadKey(); //can make it wait a second or two insted of waiting for the player input
		}

	}
}

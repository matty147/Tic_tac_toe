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
				//ColorNL($"{r + 1}: ", ConsoleColor.Red); look strange when with biger number than 9
				for (int c = 0; c < width; c++)
				{
					MyConsole.ColorNL("|", ConsoleColor.White);
					if (data[r, c] == 1)
					{
						MyConsole.ColorNL($"{data[r, c]}", ConsoleColor.Blue);
						//Console.Write($"|{board[r, c]}");
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
			//Console.WriteLine("d");
			return true;
		}

		static bool CheckIfPlayerWonDiagDown(Board board, int Player, int row, int col)
		{
			if (!board.isValid(row + d - 1, col + d - 1)) return false; 

			for (int i = 0; i < d; i++)
			{
				//Console.WriteLine($"i:{i}");
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
						MyConsole.Color("I was here4", ConsoleColor.Red);
						return true;
					}
				}
			}
			return false;
		}


		static Point input(int Boardx, int Boardy)
		{
			MyConsole.Color("Please enter two numbers separated with a comma.", ConsoleColor.Green);
			for (; ; ) {
				Console.ForegroundColor = ConsoleColor.Blue;
				string input = Console.ReadLine();
				Console.ResetColor();
				string[] words = input.Split(',', '.', '/');
				if (words.Length >= 2) // Check if input contains at least two words
				{
					int row = Int32.Parse(words[0]) - 1;
					int col = Int32.Parse(words[1]) - 1;

					if (0 <= row && row < Boardy && 0 <= col && col < Boardx)
					{
						return new Point(row, col);
					}

				}
				MyConsole.Color("Invalid input", ConsoleColor.Red);
			}
		}

		static List<int> PlacePiece(Board board, int Player)
		{
			List<int> LastPlayed = new List<int>();
			for (; ; )
			{
				Point place = input(board.width, board.height);

				LastPlayed.Add(place.row);
				LastPlayed.Add(place.col);

				if (board.isFree(place))
				{
					MyConsole.Color($"Row:{place.row + 1} Col:{place.col + 1}", ConsoleColor.Blue);
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
					//Console.WriteLine($"Y:{AiOptionY}");
					//Console.WriteLine($"X:{AiOptionX}");
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
			//Color($"LPY: {LPY - 1} LPX: {LPX - 1}", ConsoleColor.Blue);
			//Color($"", ConsoleColor.White);
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
						//Color($"X: {AiOptionX} Y: {AiOptionY}", ConsoleColor.DarkYellow);
						board.data[AiOptionY, AiOptionX] = 2;
						MyConsole.Color($"Row:{AiOptionY + 1} Col:{AiOptionX + 1}", ConsoleColor.DarkYellow);
						break;
					}
					//else Color("Try again", ConsoleColor.Red);
				}
				/*else
				{
					Color("Try again", ConsoleColor.Red);
				}*/
			}
		}

		static void AiPlayT3(Board board, int Player)
		{

		}


		private static Point GetBoardSize()
		{
			MyConsole.Color("Please enter two number for the size of the table (width, height)", ConsoleColor.Green);
			string input = Console.ReadLine();
			string PLastPlace = input;
			string[] words = input.Split(',', '.', '/');
			int Boardx = Int32.Parse(words[1]);
			int Boardy = Int32.Parse(words[0]);
			int BMax = 50, BMin = 3;
			if (Boardx > BMax)
			{
				Boardx = BMax;

				MyConsole.Color("X is too big.", ConsoleColor.Red);
				MyConsole.Color($"Seting it to {BMax}", ConsoleColor.Red);
			}
			if (Boardy > BMax)
			{
				Boardy = BMax;
				MyConsole.Color("Y is too big.", ConsoleColor.Red);
				MyConsole.Color($"Seting it to {BMax}", ConsoleColor.Red);
			}
			if (Boardx < BMin)
			{
				Boardx = BMin;
				MyConsole.Color("X is too small.", ConsoleColor.Red);
				MyConsole.Color($"Seting it to {BMin}", ConsoleColor.Red);
			}
			if (Boardy < BMin)
			{
				Boardy = BMin;
				MyConsole.Color("Y is too small.", ConsoleColor.Red);
				MyConsole.Color($"Seting it to {BMin}", ConsoleColor.Red);
			}
			Console.WriteLine("X + Y " + Boardx * Boardy);
			return new Point(Boardy, Boardx);
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

			//{
			//	MyConsole.Color("Wrong input", ConsoleColor.Red);
			//	Console.ReadKey();
			//	break;
			//}

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
				AiType aiType = GetAiType();

				int maxD = Math.Max(boardSize.row, boardSize.col);
				MyConsole.Color($"MaxD: {maxD}", ConsoleColor.Red);
				if (3 < maxD)
				{
					d = MyConsole.getNumber(3, maxD, "Please enter how many X's next to each other are needed to win");
				}
				else
				{
					d = 3;
				}

				//Int32.TryParse(input, out SizeOfD);
				int MaxPlays = boardSize.col * boardSize.row - 1;
				/*if (Ai != "n"|| Ai != "N"|| Ai != "Y" || Ai != "y")
				{
					Color("Wrong input", ConsoleColor.Red);
					Color("Seting the Ai to off", ConsoleColor.Red);
					Ai = "N";
				}*/
				//check if a number is a decimal
				//if (!Decimal.TryParse(words[0], out <output>))
				//number will alwayes be positive
				//Math.Abs();

				Board board = new Board(boardSize.col, boardSize.row);
			// Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			List<int> LastPiecePlayed = new List<int>();
			Console.Clear();
				for (int i = 0; ; i++) // inf loop // strange setup of player selection // have to chiainge
				{
					int Player = i % 2;
					//PlacePiece(Boardx, Boardy, Player, board);
					//Console.WriteLine(Player);
					board.print();
					//string comfirm = "";
					if (aiType == AiType.None)
					{
						PlacePiece(board, Player);
					}
					else {
						if (Player % 2 == 0) //Player
						{
							LastPiecePlayed = PlacePiece(board, Player);
							//Console.WriteLine(Player);
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
						MyConsole.Color($"Player:{Player + 1} Won", ConsoleColor.Magenta);
						//Console.ReadLine();
						break;
					}
					if (MaxPlays == i) // remove +1
					{ // should check if ther is a draw
						board.print();
						MyConsole.Color("Draw", ConsoleColor.Magenta);
						//Console.ReadLine();
						break;
					}
					//Color($"i:{i} MaxPlays: {MaxPlays}", ConsoleColor.Gray);
					//Table(board, Boardx, Boardy);
					//Console.Clear(); //looks like a good place
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
			Console.ReadKey(); //can make it wait a second or two insted of waiting for the player input
		}

	}
}

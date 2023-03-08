using System;
using System.Collections.Generic;
using System.Linq;
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

		public void placePiece(int row, int col)
		{
			// todo
		}

		public bool isFull() { return true; }	// todo
		
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
			if (row - d < 0) return false;
			if (col + d > board.width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board.data[row - i, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagDown(Board board, int Player, int row, int col)
		{
			if (row + d > board.height) return false;
			if (col + d > board.width) return false;

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


		static (int, int) input(int Boardx, int Boardy)
		{
			MyConsole.Color("Please enter two numbers separated with a comma.", ConsoleColor.Green);
			for (; ; ) {
				Console.ForegroundColor = ConsoleColor.Blue;
				string input = Console.ReadLine();
				Console.ResetColor();
				string[] words = input.Split(',', '.','/');
				if (words.Length >= 2) // Check if input contains at least two words
				{
					int row = Int32.Parse(words[0]) - 1;
					int col = Int32.Parse(words[1]) - 1;

					if (0 <= row && row < Boardy && 0 <= col && col < Boardx)
					{
						return (row, col);
					}

				}
				MyConsole.Color("Invalid input", ConsoleColor.Red);
			}
		}

		static  List<int> PlacePiece(Board board, int Player)
		{
			List<int> LastPlayed = new List<int>();
			for (; ; )
			{
				(int, int) place = input(board.width, board.height);
				int row = place.Item1;
				int col = place.Item2;

				LastPlayed.Add(row);
				LastPlayed.Add(col);

				if (board.data[row, col] == 0)
				{
					MyConsole.Color($"Row:{row + 1} Col:{col + 1}", ConsoleColor.Blue);
					board.data[row, col] = Player + 1;
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
					board.data[AiOptionX, AiOptionY] = Player +1;
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
		static void AiPlayT2(Board board, int Player,List<int> LastPiecePlayed)
		{
			int LPY = LastPiecePlayed[0] +1;
			int LPX = LastPiecePlayed[1] +1;
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
						MyConsole.Color($"Row:{AiOptionY+1} Col:{AiOptionX+1}", ConsoleColor.DarkYellow);
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
		static void Main(string[] args)
		{
			MyConsole.Color("Please enter two number for the size of the table (width, height)", ConsoleColor.Green);
			string input = Console.ReadLine();
			string PLastPlace = input;
			string[] words = input.Split(',', '.', '/');
			int Boardx = Int32.Parse(words[1]);
			int	Boardy = Int32.Parse(words[0]);
			string AiType = "";
			
			int BMax = 25, BMin = 3;
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
			d = (Boardx + Boardy) / 2; //or i will ask
			if (d < 2)
			{
				d = 3;
			}
			if (d > 5)
			{
				d = 5;
			}
			int MaxPlays = Boardx * Boardy;
			MyConsole.Color("Do you want a ai?", ConsoleColor.Green);
			MyConsole.Color("Y/N", ConsoleColor.Green);
			string Ai = " ";
			Ai = Console.ReadLine();
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
			MyConsole.Color("Please enter the type of the bot (1/2/3)", ConsoleColor.Green);
			MyConsole.Color("(invalid input will result in the game going singleplayer)", ConsoleColor.DarkYellow);
			AiType = Console.ReadLine();
			Board board = new Board(Boardy, Boardx);
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
				if (Ai == "Y" || Ai == "y") //(Ai.ToLower = comfirm.ToLower)
				{
					if (Player % 2 == 0) //Player
					{
						LastPiecePlayed = PlacePiece(board, Player);
						//Console.WriteLine(Player);
					}
					else if (Player % 2 == 1) //Ai
					{
						if (AiType == "1")
						{
							AiPlayT1(board, Player);
						}
						else if (AiType == "2")
						{
							AiPlayT2(board, Player, LastPiecePlayed);
						}
						Thread.Sleep(1000);
					}
				}
				else if (Ai == "N" || Ai == "n")
				{
					PlacePiece(board, Player);
					//Console.WriteLine(Player);
				}else
				{
					MyConsole.Color("Wrong input", ConsoleColor.Red);
					Console.ReadKey();
					break;
				}
				if (CheckIfPlayerWon(board, Player))
				{
					board.print();
					MyConsole.Color($"Player:{Player + 1} Won", ConsoleColor.Magenta);
					Console.ReadLine();
					break;
				}
				if (MaxPlays == i)
				{
					board.print();
					MyConsole.Color("Draw", ConsoleColor.Magenta);
					Console.ReadLine();
					break;
				}
				//Color($"i:{i} MaxPlays: {MaxPlays}", ConsoleColor.Gray);
				//Table(board, Boardx, Boardy);
				//Console.Clear(); //looks like a good place
			}
			Console.Clear();
			Console.WriteLine("Achievement get: How did we get here?");
			Console.ReadKey();
		}	
	}
}

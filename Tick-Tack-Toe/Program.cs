using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Tick_Tack_Toe
{
	/*
	class Board
	{
		int height;
		int width;
		int[,] data = new int[3, 3];


		public void printBoard()
		{

		}

		public void placePiece(int row, int col)
		{

		}

		public bool isFull() { return true; }
		public bool isWinning() { return true; }
	}
	*/
	internal class Program
	{
		static int d = 3;

		static void Color(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.WriteLine(Message);
			Console.ResetColor();
		}
		static void ColorNL(string Message, ConsoleColor Color)
		{
			Console.ForegroundColor = Color;
			Console.Write(Message);
			Console.ResetColor();
		}
		static bool CheckIfPlayerWonRow(int[,] board, int Boardx, int Boardy, int Player, int row, int col)
		{
			if (col + d > Boardx) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonCol(int[,] board, int Boardx, int Boardy, int Player, int row, int col)
		{
			if (row + d > Boardy) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row + i, col] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagUp(int[,] board, int Boardx, int Boardy, int Player, int row, int col)
		{
			if (row - d < 0) return false;
			if (col + d > Boardx) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row - i, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagDown(int[,] board, int Boardx, int Boardy, int Player, int row, int col)
		{
			if (row + d > Boardy) return false;
			if (col + d > Boardx) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row + i, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWon(int[,] board, int Boardx, int Boardy, int Player)
		{
			for (int row = 0; row < Boardy; row++)
			{
				for (int col = 0; col < Boardx; col++)
				{
					if (
						CheckIfPlayerWonRow(board, Boardx, Boardy, Player, row, col) ||
						CheckIfPlayerWonCol(board, Boardx, Boardy, Player, row, col) ||
						CheckIfPlayerWonDiagUp(board, Boardx, Boardy, Player, row, col) ||
						CheckIfPlayerWonDiagDown(board, Boardx, Boardy, Player, row, col)
					)
					{
						return true;
					}
				}
			}
			return false;
		}
		static void Table(int[,] board, int Boardx, int Boardy)
		{
			for (int r = 0; r < Boardy; r++)
			{
				//ColorNL($"{r + 1}: ", ConsoleColor.Red); look strange when with biger number than 9
				for (int c = 0; c < Boardx; c++)
				{
					ColorNL("|", ConsoleColor.White);
					if (board[r, c] == 1)
					{
						ColorNL($"{board[r, c]}", ConsoleColor.DarkYellow);
						//Console.Write($"|{board[r, c]}");
					}
					else if (board[r,c] == 2)
					{
						ColorNL($"{board[r, c]}", ConsoleColor.Blue);
					}
					else if (board[r,c] == 0)
					{
						ColorNL($"{board[r, c]}",ConsoleColor.White);
					}
				}
				Console.WriteLine("|");
			}
		}

		static (int, int) input(int Boardx, int Boardy)
		{
			Console.WriteLine("Please enter two numbers separated with a comma.");
			for (; ; ) {
				string input = Console.ReadLine();
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
				Console.WriteLine("Invalid input");
			}
		}


		static void PlacePiece(int Boardx, int Boardy, int Player, int[,] board)
		{
			for (; ; )
			{
				(int, int) place = input(Boardx, Boardy);
				int row = place.Item1;
				int col = place.Item2;

				if (board[row, col] == 0)
				{
					Console.WriteLine($"Row:{row + 1} Col:{col + 1}");
					board[row, col] = Player + 1;
					return;
				}
				else
				{
					Color("Place already taken", ConsoleColor.Red);
				}
			}
		}

		static void AiPlay(int[,] board, int Boardx, int Boardy, int Player)
		{
			Random rnd = new Random();
			for (; ; )
			{
				int AiOptionY = rnd.Next(0, Boardx);
				int AiOptionX = rnd.Next(0, Boardy);

				if (board[AiOptionX, AiOptionY] == 0)
				{
					Console.WriteLine($"Row:{AiOptionX + 1} Col:{AiOptionY + 1}");
					board[AiOptionX, AiOptionY] = Player +1; //can get player 3+
					Console.WriteLine($"Y:{AiOptionY}");
					Console.WriteLine($"X:{AiOptionX}");
					return;
				}
				else
				{
					Color("Ai failed. Place already taken", ConsoleColor.Red);
				}
			}
		}
		static void Main(string[] args)
		{
			Color("Please enter two number for the size of the table (width, height)", ConsoleColor.Green);
			string input = Console.ReadLine();
			string[] words = input.Split(',', '.', '/');
			int Boardx = Int32.Parse(words[1]);
			int Boardy = Int32.Parse(words[0]);
			int BMax = 25, BMin = 3;
			if (Boardx > BMax)
			{
				Boardx = BMax;

				Color("X is too big.", ConsoleColor.Red);
				Color($"Seting it to {BMax}", ConsoleColor.Red);
			}
			if (Boardy > BMax)
			{
				Boardy = BMax;
				Color("Y is too big.", ConsoleColor.Red);
				Color($"Seting it to {BMax}", ConsoleColor.Red);
			}
			if (Boardx < BMin)
			{
				Boardx = BMin;
				Color("X is too small.", ConsoleColor.Red);
				Color($"Seting it to {BMin}", ConsoleColor.Red);
			}
			if (Boardy < BMin)
			{
				Boardy = BMin;
				Color("Y is too small.", ConsoleColor.Red);
				Color($"Seting it to {BMin}", ConsoleColor.Red);
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
			Color("Do you want a ai?", ConsoleColor.Green);
			Color("Y/N", ConsoleColor.Green);
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
			int[,] board = new int[Boardy, Boardx];
			// Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			for (int i = 0; ; i++) // inf loop // strange setup of player selection // have to chiainge
			{
				int Player = i % 2;
					//PlacePiece(Boardx, Boardy, Player, board);
					//Console.WriteLine(Player);
				Table(board, Boardx, Boardy);
				//string comfirm = "";
				if (Ai == "Y" || Ai == "y") //(Ai.ToLower = comfirm.ToLower)
				{
					if (Player % 2 == 0) //Player
					{
						PlacePiece(Boardx, Boardy, Player, board);
						Console.WriteLine(Player);
					}
					else if (Player % 2 == 1) //Ai
					{
						AiPlay(board, Boardx, Boardy, Player);
						Thread.Sleep(1000);
					}
				}
				else if (Ai == "N" || Ai == "n")
				{
					PlacePiece(Boardx, Boardy, Player, board);
					Console.WriteLine(Player);
				}
				if (CheckIfPlayerWon(board, Boardx, Boardy, Player))
				{
					Table(board, Boardx, Boardy);
					Color($"Player:{Player + 1} Won", ConsoleColor.Magenta);
					Console.ReadLine();
					break;
				}
				if (MaxPlays == i)
				{
					Table(board, Boardx, Boardy);
					Color("Draw", ConsoleColor.Magenta);
					Console.ReadLine();
					break;
				}
				Color($"i:{i} MaxPlays: {MaxPlays}", ConsoleColor.Gray);
				//Table(board, Boardx, Boardy);
			}
			Console.Clear();
			Console.WriteLine("Achievement get: How did we get here?");
			Console.ReadKey();
		}	
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			for (int row=0; row < Boardy; row++)
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
				for (int c = 0; c < Boardx; c++)
				{
					Console.Write($"|{board[r,c]}");
				}
				Console.WriteLine("|");
			}
		}

		static (int, int) input(int Boardx, int Boardy)
		{
			Console.WriteLine("Please enter two numbers separated with a comma.");
			for(;;) { 
				string input = Console.ReadLine();
				string[] words = input.Split(',', '.');
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
					Console.WriteLine("Place already taken");
				}
			}
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Please enter two number for the size of the table (width, height)");
			string input = Console.ReadLine();
			string[] words = input.Split(',', '.');
			//check if a number is a decimal
			//if (!Decimal.TryParse(words[0], out <output>))
			//number will alwayes be positive
			//Math.Abs();
			int Boardx = Int32.Parse(words[1]);
			int Boardy = Int32.Parse(words[0]);

			int[,] board = new int[Boardy, Boardx]; // Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			for (int i = 0; ; i++) // inf loop
			{
				int Player = i % 2;
				PlacePiece(Boardx, Boardy, Player, board);
				Console.WriteLine(Player);
				Table(board, Boardx, Boardy);

				if (CheckIfPlayerWon(board, Boardx, Boardy, Player))
				{
					Console.WriteLine($"Player {Player+1} Won");
					//break;
				}
			}
			Console.WriteLine("Achievement get: How did we get here?");
			Console.ReadKey();
		}	
	}
}

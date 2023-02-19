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

		static bool CheckIfPlayerWonRow(int[,] board, int height, int width, int Player, int row, int col)
		{
			if (col + d > width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonCol(int[,] board, int height, int width, int Player, int row, int col)
		{
			if (row + d > height) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row + i, col] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagUp(int[,] board, int height, int width, int Player, int row, int col)
		{
			if (row - d < 0) return false;
			if (col + d > width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row - i, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWonDiagDown(int[,] board, int height, int width, int Player, int row, int col)
		{
			if (row + d > height) return false;
			if (col + d > width) return false;

			for (int i = 0; i < d; i++)
			{
				if (board[row + i, col + i] != Player + 1) return false;
			}

			return true;
		}

		static bool CheckIfPlayerWon(int[,] board, int height, int width,int Player)
		{
			for (int row=0; row < height; row++)
			{
				for (int col = 0; col < width; col++)
				{
					if (
						CheckIfPlayerWonRow(board, height, width, Player, row, col) ||
						CheckIfPlayerWonCol(board, height, width, Player, row, col) ||
						CheckIfPlayerWonDiagUp(board, height, width, Player, row, col) ||
						CheckIfPlayerWonDiagDown(board, height, width, Player, row, col)
					)
					{
						return true;
					}
				}
			}
			return false;
		}
	static void Table(int[,] board, int row, int col)
		{
			int i = 0;
			foreach (int a in board)
			{
				if (i == row)
				{
					Console.WriteLine("|");
					i = 1;
				}
				else i++;

				Console.Write($"|{a}");
				//Console.WriteLine($"Row: {row} i: {i}");
			}
			Console.WriteLine("|");
		}
		static (int, int) input(int width, int height)
		{
			Console.WriteLine("Please enter two numbers separated with a comma.");
			for(;;) { 
				string input = Console.ReadLine();
				string[] words = input.Split(',', '.');
				if (words.Length >= 2) // Check if input contains at least two words
				{
					int row = Int32.Parse(words[0]) - 1;
					int col = Int32.Parse(words[1]) - 1;

					if (0 <= row && row < height && 0 <= col && col < width)
					{
						return (row, col);
					}

				}
				Console.WriteLine("Invalid input");
			}
		}


		static void PlacePiece(int Boardx, int Boardy,int Player, int[,] board)
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
			int row, col;
			Console.WriteLine("Please enter two number for the size of the table");
			string input = Console.ReadLine();
			string[] words = input.Split(',', '.');
			//check if a number is a decimal
			//if (!Decimal.TryParse(words[0], out <output>))
			//number will alwayes be positive
			//Math.Abs();
			row = Int32.Parse(words[0]);
			col = Int32.Parse(words[1]);
			int[,] board = new int[row, col]; // Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			for (int i = 0; ; i++) // inf loop
			{
				int Player = i % 2;
				PlacePiece(row, col, Player, board);
				Console.WriteLine(Player);
				Table(board, row, col);
				/*
				//Console.WriteLine("  A B C ");
				Console.WriteLine($"|{board[0, 0]}|{board[0, 1]}|{board[0, 2]}|"); // can write 1-3 infront of the console.writeline to make it more readable
				Console.WriteLine($"|{board[1, 0]}|{board[1, 1]}|{board[1, 2]}|");
				Console.WriteLine($"|{board[2, 0]}|{board[2, 1]}|{board[2, 2]}|");
				*/
				if (CheckIfPlayerWon(board,row,col,Player))
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

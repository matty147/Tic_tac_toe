using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tick_Tack_Toe
{
	internal class Program
	{
		static void PlacePiece(int Player, int[,] board)
		{
			Console.WriteLine("Please enter two numbers sepereted with a comma.");
			string input = Console.ReadLine();
			string[] words = input.Split(',','.');
			if (words.Length >= 2) // Check if input contains at least two words
			{
				int row = Int32.Parse(words[0]) - 1;
				int col = Int32.Parse(words[1]) - 1;
				if (board[row, col] == 0)
				{
					Console.WriteLine($"Row:{row + 1} Col:{col + 1}");
					if (Player == 0)
					{
						board[row, col] = 1;

					}
					else if (Player == 1)
					{
						board[row, col] = 2;
					}
				}
				else
				{
					Console.WriteLine("Place allredy taken");
					PlacePiece(Player, board);
				}
			}
			else
			{
				Console.WriteLine("Invalid input");
				PlacePiece(Player, board);
			}
		}
		static void Main(string[] args)
		{
			int[,] board = new int[3, 3]; // Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			int Player = 0;
			for (int i = 0; i != i+1; i++) // inf loop
			{
				PlacePiece(Player, board);
				Player++;
				if (Player >= 2)
				{
					Player = 0;
				}
				Console.WriteLine(Player);
				Console.WriteLine($"|{board[0, 0]}|{board[0, 1]}|{board[0, 2]}|");
				Console.WriteLine($"|{board[1, 0]}|{board[1, 1]}|{board[1, 2]}|");
				Console.WriteLine($"|{board[2, 0]}|{board[2, 1]}|{board[2, 2]}|");
			}
			Console.WriteLine("Achivment get: How did we get here?");
			Console.ReadKey();
		}
	}
}

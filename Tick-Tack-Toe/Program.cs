using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tick_Tack_Toe
{
	internal class Program
	{
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


		static void PlacePiece(int Player, int[,] board)
		{
			for (; ; )
			{
				(int, int) place = input(3, 3);
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
			int[,] board = new int[3, 3]; // Define and initialize board array
			//char x = 'B';
			//Console.WriteLine((int)x);
			for (int i = 0; ; i++) // inf loop
			{
				int Player = i % 2;
				PlacePiece(Player, board);
				Console.WriteLine(Player);
				//Console.WriteLine("  A B C ");
				Console.WriteLine($"|{board[0, 0]}|{board[0, 1]}|{board[0, 2]}|"); // can write 1-3 infront of the console.writeline to make it more readable
				Console.WriteLine($"|{board[1, 0]}|{board[1, 1]}|{board[1, 2]}|");
				Console.WriteLine($"|{board[2, 0]}|{board[2, 1]}|{board[2, 2]}|");
			}
			Console.WriteLine("Achievement get: How did we get here?");
			Console.ReadKey();
		}	
	}
}

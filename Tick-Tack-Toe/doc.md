		public static class MyConsole - Custom console that suports colored messages ane number Input
				public static void Color(string message, ConsoleColor color) - Prints message in desired color
				public static void ColorSameLine(string message, ConsoleColor color) - Prints message in desired color without starting a new line.
		public static void PrintPlace(Point place, ConsoleColor color)
				public static int GetNumber(int min, int max, string prompt) - Gets a integer number from the user
		public class Board - Board object holding the curent state of the game
				public int Height - number of rows
				public int Width - number of coloms
				public int[,] Data - the individual cells of the board (0 - empty, 1 - player one, 2 - player two)
				public Board(int width, int height) - creates a new board
				public bool IsValid(Point point) - Is a point within the board range
				public bool IsValid(int row, int col) - Is a point within the board range
				public void Print() - Prints the board in the console
				public bool IsFree(Point place) - Is a point on the board empty
				public void PlacePiece(Point place, int player) - Places the piece on the game board
				private bool CheckIfPlayerWonRow(int player, int piecesToWin, int row, int col) - Checks if a player has a winning row at a particular place on the board. 
				private bool CheckIfPlayerWonCol(int player, int piecesToWin, int row, int col) - Checks if a player has a winning column at a particular place on the board. 
				private bool CheckIfPlayerWonDiagUp(int player, int piecesToWin, int row, int col) - Checks if a player has a winning up-right (/) diagonal at a particular place on the board. 
				private bool CheckIfPlayerWonDiagDown(int player, int piecesToWin, int row, int col) - Checks if a player has a winning down-right (\) diagonal at a particular place on the board. 
				public bool CheckIfPlayerWon(int player, int piecesToWin) - Checks if a player won. 
		public class Point - point (row and column) on the board
				public int Row - zero-based row index
				public int Col - zero-based column index
				public Point(int row, int col) - Creates a new point
		internal class Program - The main program
				private static void check(bool test, string msg) - Ensures that a condition is true.
				static Point Input(int minx, int miny, int maxx, int maxy, String prompt, ConsoleColor color) - Asks the user for a point (two numbers).
				static Point PlacePiece(Board board, int player) -  Asks a player to place a piece on the board
				static void AiPlayRandom(Board board, int player) - The first type of the ai's turn (random strategy)
				static void AiPlayLocal(Board board, int player, Point lastPiecePlayed) - The second type of the ai's turn (Playing around the other player last piece)
				static void AiPlaySmart(Board board, int player)
				enum AiType - AI strategy to use
				private static AiType GetAiType() - Ask player the player what about the ai strategy - 
				static void RestartGame() - Resets the game
				static void Main(string[] args) - Main game loop

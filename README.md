# Tic-tac-toe
## by Matyas Hana

### The goal of the Project:

- Make a  working version of Tic-tac-toe.
- Make the board scalable to any size.
- Make a simple ai to play against you.

### Selected technologies, expected difficulty

- The c# language.
- The project will be made in visual studio with the .net console.
- It will take about 5-10h.


# zz

Tic-tac-toe


Todo:
Make documentacion (about 1000 words)
presentacion

Asigment: https://github.com/jVsetecka/PVA_1
Project: https://github.com/matty147/Tic_tac_toe
Itch.io: https://inkk-ing.itch.io/tick-tack-toe-in-the-console

# User Instruction

This is a school project I am still working on.

In this game, you'll be able to choose the size of the board, from a compact 3x3 to a sprawling 50x50. The game's objective is to get 3 in a row.

To play, input the x and y coordinates of the space you wish to occupy, and aim to outmaneuver your opponent. With the ability to choose the winning condition, you can make the game as challenging or straightforward as you like. Have fun!

## Install Instructions

* Download the .zip file containing Tick-Tack-Toe to your computer.
* Right-click on the .zip file and select "Extract All" or a similar option to extract the file's contents.
* Once the extraction is complete, navigate to the folder where the files were extracted.
* Find the "Tick-Tack-Toe.exe" file and double-click on it to run the game.
* Enjoy playing Tick-Tack-Toe on your computer!
* If you encounter any issues during the installation process, please let me know and I'll be happy to assist.

# Program Design

# sample game
(the game is not case sensitive) :thumbsup:
**Please enter two number for the size of the board (width, height)**
> Here i select what board size i want to play on. 
 `5.5`

**Please enter how many X's next to each other are needed to win (3..5)**
> Here i will select how many X i need next to each other to win. 
 `3`

**Do you want an AI to play against you?**
> Here you can select if a Bot should play against you or not
 `y`

**Please enter the type of the AI (1/2/3)**
> Here you can select the type of the bot by entering a number from 1 to 2. 
 `2`

```
|0|0|0|0|0|
|0|0|0|0|0|
|0|0|0|0|0|
|0|0|0|0|0|
|0|0|0|0|0|
```
**Please enter two numbers separated with a comma.**
> Here you will select the coordinates of the place on the board you would like to play on
`3.3`

```
Row: 3 Col:3
|0|0|0|0|0|
|0|0|0|0|0|
|0|0|1|0|0|
|0|0|0|0|0|
|0|0|0|0|0|
```
**this is where the ai plays**
```
Row: 3 Col:4
|0|0|0|0|0|
|0|0|0|0|0|
|0|0|1|2|0|
|0|0|0|0|0|
|0|0|0|0|0|
```
> After that continue the steps fromt before until somebody wins
```
Row: 4 Col:4
|0|0|0|0|0|
|2|1|0|0|0|
|0|0|1|2|0|
|0|0|0|1|0|
|0|0|0|0|0|
```
**player1 Won**
> In this case the first player (you) won.

**Do you want to exit? (Y/N)**
> Here you can exit the program by answering Yes or No
- case `n`
> the program restarts and you can play again
- case: `y`
**Exiting the aplication...**
> the program exits

# Program Blocks

grep -P "private|public|static|class|///" Program.cs | grep -vP "///.*(summary|param name|returns|exception)" > doc.txt


`class MyConsole` - Custom console that suports colored messages ane number Input

- `static void Color(string message, ConsoleColor color)` - Prints message in desired color
- `static void ColorSameLine(string message, ConsoleColor color)` - Prints message in desired color without starting a new line.
- `static void PrintPlace(Point place, ConsoleColor color)
- `static int GetNumber(int min, int max, string prompt)` - Gets a integer number from the user
		
`class Board` - Board object holding the curent state of the game

- `int Height` - number of rows
- `int Width` - number of coloms
- `int[,] Data` - the individual cells of the board (0 - empty, 1 - player one, 2 - player two)
- `Board(int width, int height)` - creates a new board
- `bool IsValid(Point point)` - Is a point within the board range
- `bool IsValid(int row, int col)` - Is a point within the board range
- `void Print()` - Prints the board in the console
- `bool IsFree(Point place)` - Is a point on the board empty
- `void PlacePiece(Point place, int player)` - Places the piece on the game board
- `bool CheckIfPlayerWonRow(int player, int piecesToWin, int row, int col)` - Checks if a player has a winning row at a particular place on the board. 
- `bool CheckIfPlayerWonCol(int player, int piecesToWin, int row, int col)` - Checks if a player has a winning column at a particular place on the board. 
- `bool CheckIfPlayerWonDiagUp(int player, int piecesToWin, int row, int col)` - Checks if a player has a winning up-right (/) diagonal at a particular place on the board. 
- `bool CheckIfPlayerWonDiagDown(int player, int piecesToWin, int row, int col)` - Checks if a player has a winning down-right (\) diagonal at a particular place on the board. 
- `bool CheckIfPlayerWon(int player, int piecesToWin)` - Checks if a player won. 
		
`class Point` - point (row and column) on the board

- `int Row` - zero-based row index
- `int Col` - zero-based column index
- `Point(int row, int col)` - Creates a new point

`class Program` - The main program

- `static void check(bool test, string msg)` - Ensures that a condition is true.
- `static Point Input(int minx, int miny, int maxx, int maxy, String prompt, ConsoleColor color)` - Asks the user for a point (two numbers).
- `static Point PlacePiece(Board board, int player)` -  Asks a player to place a piece on the board
- `static void AiPlayRandom(Board board, int player)` - The first type of the ai's turn (random strategy)
- `static void AiPlayLocal(Board board, int player, Point lastPiecePlayed)` - The second type of the ai's turn (Playing around the other player last piece)
- `static void AiPlaySmart(Board board, int player)
- `enum AiType` - AI strategy to use
- `static AiType GetAiType()` - Ask player the player what about the ai strategy 
- `static void RestartGame()` - Resets the game
- `static void Main(string[] args)` - Main game loop



using System;

class Program
{
    static char[,] board = new char[3, 3]; // Tic-tac-toe board

    static void Main(string[] args)
    {
        InitializeBoard();
        PrintBoard();

        while (!IsGameOver(board))
        {
            if (IsPlayerTurn())
            {
                Console.WriteLine("Player's turn.");
                Console.WriteLine("Enter row (0-2): ");
                int row = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter column (0-2): ");
                int col = int.Parse(Console.ReadLine());

                if (IsValidMove(row, col))
                {
                    MakeMove(row, col, 'X');
                    PrintBoard();
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
            else
            {
                Console.WriteLine("AI's turn.");
                var bestMove = FindBestMove(board);
                MakeMove(bestMove.Row, bestMove.Col, 'O');
                PrintBoard();
            }
        }

        char winner = GetWinner(board);
        if (winner == 'X')
        {
            Console.WriteLine("Player wins!");
        }
        else if (winner == 'O')
        {
            Console.WriteLine("AI wins!");
        }
        else
        {
            Console.WriteLine("It's a draw!");
        }
    }

    static void InitializeBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = '-';
            }
        }
    }

    static void PrintBoard()
    {
        Console.WriteLine("Board:");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static bool IsPlayerTurn()
    {
        int countX = 0;
        int countO = 0;

        foreach (var cell in board)
        {
            if (cell == 'X')
                countX++;
            else if (cell == 'O')
                countO++;
        }

        return countX == countO;
    }

    static bool IsValidMove(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3 && board[row, col] == '-';
    }

    static void MakeMove(int row, int col, char player)
    {
        board[row, col] = player;
    }

    static bool IsGameOver(char[,] board)
    {
        return IsBoardFull(board) || GetWinner(board) != '-';
    }

    static bool IsBoardFull(char[,] board)
    {
        foreach (var cell in board)
        {
            if (cell == '-')
                return false;
        }
        return true;
    }

    static char GetWinner(char[,] board)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != '-')
                return board[i, 0];
        }

        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != '-')
                return board[0, j];
        }

        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != '-')
            return board[0, 0];

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != '-')
            return board[0, 2];

        return '-';
    }

    static int EvaluateBoard(char[,] board)
    {
        char winner = GetWinner(board);
        if (winner == 'O')
            return 10;
        else if (winner == 'X')
            return -10;
        else
            return 0;
    }

    static int MiniMax(char[,] board, int depth, bool isMaximizing)
    {
        int score = EvaluateBoard(board);

        if (score == 10) // AI wins
            return score;

        if (score == -10) // Player wins
            return score;

        if (!IsBoardFull(board)) // Draw
            return 0;

        if (isMaximizing)
        {
            int best = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        board[i, j] = 'O';
                        best = Math.Max(best, MiniMax(board, depth + 1, !isMaximizing));
                        board[i, j] = '-';
                    }
                }
            }

            return best;
        }
        else
        {
            int best = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '-')
                    {
                        board[i, j] = 'X';
                        best = Math.Min(best, MiniMax(board, depth + 1, !isMaximizing));
                        board[i, j] = '-';
                    }
                }
            }

            return best;
        }
    }

    static (int Row, int Col) FindBestMove(char[,] board)
    {
        int bestVal = int.MinValue;
        var bestMove = (-1, -1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '-')
                {
                    board[i, j] = 'O';
                    int moveVal = MiniMax(board, 0, false);
                    board[i, j] = '-';

                    if (moveVal > bestVal)
                    {
                        bestMove = (i, j);
                        bestVal = moveVal;
                    }
                }
            }
        }

        return bestMove;
    }
}
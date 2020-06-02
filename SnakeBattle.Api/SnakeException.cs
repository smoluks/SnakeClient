using System;
using System.IO;

namespace SnakeBattle.Api
{
    public class SnakeException: Exception
    {
        public SnakeException(string message): base (message)
        {
#if DEBUG
            File.AppendAllText(Path.Combine(@"C:\Users\Администратор\Desktop", "error.log"), "Error: "+message);
#endif
        }

        public SnakeException(string message, GameBoard gameBoard) : this(message)
        {
#if DEBUG
            File.AppendAllText(Path.Combine(@"C:\Users\Администратор\Desktop", "error.log"), "Error: " + message + "\n");
            Graphical.WriteToLog(gameBoard);
#endif
        }
    }
}

﻿using SnakeBattle.Api;
using System;
using System.Text;

namespace Tests
{
    public class Graphical
    {
        public static void WriteField(GameBoard gameBoard)
        {           
            StringBuilder s = new StringBuilder();

            s.Append("   ");
            for (int i = 0; i < gameBoard.Size; i++)
            {
                s.Append(i % 10);
            }

            s.AppendLine();
            s.AppendLine();

            for (int i = 0; i < gameBoard.Size; i++)
            {
                s.Append(i.ToString().PadLeft(2) + " ");
                for (int j = 0; j < gameBoard.Size; j++)
                {
                    s.Append((char)gameBoard.Board[j, i]);
                }
                s.AppendLine();
            }
            
            Console.Write(s.ToString());
        }
    }
}

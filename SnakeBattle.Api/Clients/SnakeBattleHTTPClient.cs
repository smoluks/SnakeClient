using System;
using System.Diagnostics;
using System.IO;
using WebSocketCore;

namespace SnakeBattle.Api.Clients
{
    public class SnakeBattleHTTPClient
    {
        private const string RESPONSE_PREFIX = "board=";
        private WebSocket _socket;

        private readonly Func<GameBoard, SnakeAction?> _callback;
        private readonly Action<long> _timeOutCallback;

        private readonly string server;
        private bool exit;
        public bool save;
        static readonly Stopwatch stopwatch = new Stopwatch();

        public SnakeBattleHTTPClient(string url, Func<GameBoard, SnakeAction?> callback, Action<long> timeOutCallback)
        {
            _callback = callback;
            _timeOutCallback = timeOutCallback;

            server = url.Replace("http", "ws").Replace("board/player/", "ws?user=").Replace("?code=", "&code=");
            this._socket = new WebSocket(server);

            _socket.OnMessage += Socket_OnMessage;
            _socket.OnClose += Socket_OnClose;
            _socket.OnError += Socket_OnError;
        }

        public bool Connect()
        {
            try
            {
                _socket.Connect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            exit = true;
            _socket.Close();
        }

        private void Socket_OnError(object sender, WebSocketCore.ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($"Disconnected");
            if (!exit && server != null)
            {
                this._socket = new WebSocket(server);

                _socket.OnMessage += Socket_OnMessage;
                _socket.OnClose += Socket_OnClose;
                _socket.OnError += Socket_OnError;

                _socket.Connect();
            }
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            stopwatch.Restart();

            var response = e.Data;

            if (!response.StartsWith(RESPONSE_PREFIX))
            {
                Console.WriteLine($"Bad data: {response}");
                return;
            }

            var boardString = response.Substring(RESPONSE_PREFIX.Length);
            if (save)
            {
                File.WriteAllText(Path.Combine(@"C:\Users\Администратор\Desktop", $"board_{DateTime.Now:hhMMss}"), boardString);
                save = false;
            }

            var board = new GameBoard(boardString);
            var action = _callback(board);

            var answer = action.HasValue ? MakeAnswer(action.Value) : "STOP";

            try
            {
                ((WebSocket)sender).Send(answer);
                stopwatch.Stop();
                Console.WriteLine($"Take {stopwatch.ElapsedMilliseconds} ms");
                if (stopwatch.ElapsedMilliseconds >= 1000)
                    _timeOutCallback(stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string MakeAnswer(SnakeAction snakeAction)
        {
            switch (snakeAction)
            {
                case SnakeAction.Down:
                    return "DOWN";
                case SnakeAction.Left:
                    return "LEFT";
                case SnakeAction.Right:
                    return "RIGHT";
                case SnakeAction.Up:
                    return "UP";
                case SnakeAction.ActDown:
                    return "ACT,DOWN";
                case SnakeAction.ActLeft:
                    return "ACT,LEFT";
                case SnakeAction.ActRight:
                    return "ACT,RIGHT";
                case SnakeAction.ActUp:
                    return "ACT,UP";
                default:
                    break;
            }

            return "";
        }

    }
}

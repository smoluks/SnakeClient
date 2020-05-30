using System;
using System.Threading.Tasks;

namespace SnakeBattle.Api.Clients
{
    public class SnakeBattleMockClient
    {
        private Func<GameBoard, SnakeAction> callback;

        public SnakeBattleMockClient(string url, Func<GameBoard, SnakeAction> callback)
        {
            this.callback = callback;
        }

        public void Connect()
        {
            callback(new GameBoard(@"
☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼
☼☼            ☼
☼☼     $      ☼
☼☼           @☼
☼#           ▲☼
☼☼           ║☼
☼☼   ○       ║☼
☼#           ║☼
☼☼         ╘═╝☼
☼☼      %     ☼
☼☼   ×—>      ☼
☼☼            ☼
☼☼       ●    ☼
☼☼            ☼
☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼
"));
        }

        public void Disconnect()
        {
        }
    }
}

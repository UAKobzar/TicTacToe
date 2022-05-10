using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class TicTacToeAgent
    {
        private readonly bool _debugEnabled = false;

        private IPlayer _player1;
        private IPlayer _player2;

        private int _board;

        private bool _firstPlayerMove;

        public TicTacToeAgent(IPlayer player1, IPlayer player2,bool debugEnabled = false)
        {
            _player1 = player1;
            _player2 = player2;

            _board = 0;
            _firstPlayerMove = true;

            _debugEnabled = debugEnabled;
        }

        public BoardState NextMove()
        {
            var player = _firstPlayerMove ? _player1 : _player2;

            var move = player.GetNextStep(_board, _firstPlayerMove);

            _board = Board.MakeMove(_board, move.x, move.y, _firstPlayerMove);
            _firstPlayerMove = !_firstPlayerMove;

            var state = Board.GetState(_board);

            return state;
        }

        public void Swap()
        {
            var tmp = _player1;
            _player1 = _player2;
            _player2 = tmp;
        }

        public BoardState Play()
        {
            _board = 0;
            _firstPlayerMove = true;

            BoardState state = BoardState.Playing;

            while (state == BoardState.Playing)
            {
                if (_debugEnabled)
                {
                    Console.WriteLine("============================================");
                    Console.WriteLine(Board.DrawBoard(_board));
                }
                state = NextMove();
            }

            return state;
        }

    }

    enum BoardState
    {
        Playing = 999,
        FirstPlayerWin = 1,
        SecondPlayerWin = -1,
        Draw = 0
    }

    static class Board
    {
        private static int _max = 0b_11_11_11_11_11_11_11_11_11;

        private static BoardState[] _states = new BoardState[_max];

        private static int[,] movesFirstPlayer =
        {
            {0b_00_00_00_00_00_00_00_00_10, 0b_00_00_00_00_00_00_00_10_00, 0b_00_00_00_00_00_00_10_00_00 },
            {0b_00_00_00_00_00_10_00_00_00, 0b_00_00_00_00_10_00_00_00_00, 0b_00_00_00_10_00_00_00_00_00 },
            {0b_00_00_10_00_00_00_00_00_00, 0b_00_10_00_00_00_00_00_00_00, 0b_10_00_00_00_00_00_00_00_00 },
        };

        private static int[,] movesSecondPlayer =
        {
            {0b_00_00_00_00_00_00_00_00_11, 0b_00_00_00_00_00_00_00_11_00, 0b_00_00_00_00_00_00_11_00_00 },
            {0b_00_00_00_00_00_11_00_00_00, 0b_00_00_00_00_11_00_00_00_00, 0b_00_00_00_11_00_00_00_00_00 },
            {0b_00_00_11_00_00_00_00_00_00, 0b_00_11_00_00_00_00_00_00_00, 0b_11_00_00_00_00_00_00_00_00 },
        };

        private static int[,] drawMasks =
{
            {0b_00_00_00_00_00_00_00_00_01, 0b_00_00_00_00_00_00_00_01_00, 0b_00_00_00_00_00_00_01_00_00 },
            {0b_00_00_00_00_00_01_00_00_00, 0b_00_00_00_00_01_00_00_00_00, 0b_00_00_00_01_00_00_00_00_00 },
            {0b_00_00_01_00_00_00_00_00_00, 0b_00_01_00_00_00_00_00_00_00, 0b_01_00_00_00_00_00_00_00_00 },
        };

        private static int[] firstPlayerWins =
        {
            0b_00_00_00_00_00_00_10_10_10,
            0b_00_00_00_10_10_10_00_00_00,
            0b_10_10_10_00_00_00_00_00_00,

            0b_00_00_10_00_00_10_00_00_10,
            0b_00_10_00_00_10_00_00_10_00,
            0b_10_00_00_10_00_00_10_00_00,

            0b_10_00_00_00_10_00_00_00_10,
            0b_00_00_10_00_10_00_10_00_00,
        };

        private static int[] secondPlayerWins =
{
            0b_00_00_00_00_00_00_11_11_11,
            0b_00_00_00_11_11_11_00_00_00,
            0b_11_11_11_00_00_00_00_00_00,

            0b_00_00_11_00_00_11_00_00_11,
            0b_00_11_00_00_11_00_00_11_00,
            0b_11_00_00_11_00_00_11_00_00,

            0b_11_00_00_00_11_00_00_00_11,
            0b_00_00_11_00_11_00_11_00_00,
        };

        private static int drawMask = 0b_10_10_10_10_10_10_10_10_10;

        public static int MakeMove(int bitBoard, int x, int y, bool firstPlayer)
        {
            if (firstPlayer)
            {
                return bitBoard | movesFirstPlayer[x, y];
            }
            else
            {
                return bitBoard | movesSecondPlayer[x, y];
            }
        }

        public static IEnumerable<Tuple<int, int>> GetPosiibleMoves(int bitBoard, BoardState? state = null)
        {
            state = state == null ? GetState(bitBoard) : state;
            if (state == BoardState.Playing)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if ((bitBoard & movesSecondPlayer[x, y]) == 0)
                        {
                            yield return new Tuple<int, int>(x, y);
                        }
                    }
                }
            }
        }

        public static string DrawBoard(int bitBoard)
        {
            string result = "";

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var maskedBoard = bitBoard & movesSecondPlayer[x, y];
                    var sign = ". ";
                    if (maskedBoard == movesFirstPlayer[x, y])
                        sign = "X ";
                    else if (maskedBoard == movesSecondPlayer[x, y])
                        sign = "O ";
                    result += sign;
                }
                result += "\n";
            }

            return result;
        }

        public static BoardState GetState(int bitBoard)
        {
            return _states[bitBoard];
        }

        public static BoardState CalculateState(int bitBoard)
        {
            if (((bitBoard & drawMask) | (bitBoard << 1 & drawMask)) == drawMask)
                return BoardState.Draw;

            for (int i = 0; i < 8; i++)
            {
                var maskedBoard = bitBoard & secondPlayerWins[i];

                if (maskedBoard == firstPlayerWins[i])
                    return BoardState.FirstPlayerWin;
                else if (maskedBoard == secondPlayerWins[i])
                    return BoardState.SecondPlayerWin;
            }

            return BoardState.Playing;
        }

        public static void Calculate()
        {
            for (int field = 0; field < 0b_11_11_11_11_11_11_11_11_11; field++)
            {
                var state = Board.CalculateState(field);

                _states[field] = state;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Bot
    {
        enum MoveType
        {
            Down,
            Left,
            Right,
            TurnLeft,
            TurnRight
        }

        private string _botName;
        private int _height;
        private int _width;
        private int _round;
        private bool _debug = false;
        private string _block;
        private string _nextBlock;
        private int _blockX;
        private int _skips = 0;
        private const int ValidPosOffset =5;
        Matrix _gamestate;

        public void Run()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.Equals("debug"))
                {
                    DoDebugStuff();
                }
                var lineArray = line.Split(' ');
                if (lineArray != null && lineArray.Count() != 0)
                {
                    ParseLine(lineArray);
                }
            }
        }
        private double time = 10000;
        private void DoDebugStuff()
        {
            #region debug zut
            _debug = true;

            //ParseLine("settings timebank 10000".Split(' '));
            //ParseLine("settings time_per_move 500".Split(' '));
            //ParseLine("settings player_names player1,player2".Split(' '));
            //ParseLine("settings your_bot player2".Split(' '));
            //ParseLine("settings field_width 10".Split(' '));
            //ParseLine("settings field_height 20".Split(' '));
            //ParseLine("update game round 72".Split(' '));
            //ParseLine("update game this_piece_type T".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player2 row_points 20".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("update player2 skips 0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,2,2,0,0,0,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,0,0,2,2,2;2,2,0,2,2,2,2,2,2,2;2,2,2,2,2,0,0,2,2,2;2,2,2,2,0,2,2,2,2,2;0,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,2,0;2,2,0,2,2,2,2,2,2,2;2,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            //ParseLine("update player1 row_points 28".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 skips 1".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //Console.ReadLine();
            ParseLine("settings timebank 10000".Split(' '));
            ParseLine("settings time_per_move 500".Split(' '));
            ParseLine("settings player_names player1,player2".Split(' '));
            ParseLine("settings your_bot player1".Split(' '));
            ParseLine("settings field_width 10".Split(' '));
            ParseLine("settings field_height 20".Split(' '));
            ParseLine("update game round 1".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 2".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 3".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,2,2,2,2,2,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 4".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,2,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,0,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 5".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,2,2,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 6".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,2,0;0,0,0,0,0,2,2,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 7".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,2;2,2,2,2,0,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,2,0;2,2,2,2,0,2,2,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 8".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,0,0,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,2,2,0;0,0,0,0,2,2,2,2,2,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 9".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,0,0,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 10".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,0,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,0,2,0,0,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 11".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 12".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,0,0,0,2;2,2,2,2,2,2,0,0,0,2;2,2,2,2,2,2,2,0,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,2,2,0,0,0,2,2;2,2,2,2,2,0,0,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 13".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,0,0,0,2;2,2,2,2,2,2,2,2,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,0,2,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 14".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,2;2,2,2,2,2,2,2,2,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,2,2,2,0,2,2,2,2".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 15".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,2;2,2,2,2,2,2,2,2,0,2".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,2,0,0,0,0,0;2,2,0,0,2,2,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 16".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,2;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;2,2,0,2,2,2,0,0,0,0;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 17".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,0,2,2,2,0,0,0,0;2,2,2,2,0,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 18".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,2,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,2,2,0,0,0,2,2;2,2,0,2,2,2,0,0,2,2;2,2,2,2,0,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 19".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;2,2,0,0,2,0,0,0,0,2;2,2,2,2,2,2,2,0,0,2;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,0,0,0,2,2;2,2,0,2,2,2,0,0,2,2;2,2,2,2,0,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 20".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,2,2,0,0,2;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,2,2,0,0,2,2,2;2,2,0,2,2,2,0,0,2,2;2,2,2,2,0,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 21".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,2;2,2,2,2,2,0,0,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,0,0,0,0,2,0,0,0,0;2,2,0,2,2,2,0,0,2,2;2,2,2,2,0,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 22".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,0,0,0,2,2,0,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,0,0,0,0,2,2,0,0,0;2,2,0,2,2,2,2,2,2,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 23".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;0,0,2,2,2,2,2,2,0,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;2,0,0,2,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 24".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,2,2,2,0;0,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;2,0,2,2,2,2,2,0,0,0;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 25".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,2,2,0,0,2,2,2,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,2,0,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,0,2,2,2,2,2,0,0,0;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 26".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;2,0,0,0,0,0,0,0,0,2;2,0,2,2,0,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,2,0,0,0,0,0;2,0,2,2,2,2,2,0,0,0;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 27".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;2,2,2,2,0,0,0,0,0,2;2,2,2,2,0,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,0,0,0,2,0,0,0,0,2;2,0,2,2,2,2,2,2,2,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 28".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,0,0,0,0,2;2,2,2,2,2,0,0,0,0,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 29".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,0,0,2,0,2;2,2,2,2,2,0,0,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,2,0,0,0;2,2,2,2,2,2,2,2,0,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 30".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,0,0,2,2,2;0,0,0,0,2,0,0,2,2,2;2,2,2,2,2,0,0,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,2,2,2,2;2,2,2,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 31".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,2,0;0,0,0,2,2,0,0,2,2,2;0,0,0,2,2,0,0,2,2,2;2,2,2,2,2,0,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,0,0,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 32".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,0,0,0,2,2;0,0,0,2,2,0,0,2,2,2;0,0,0,2,2,0,0,2,2,2;2,2,2,2,2,0,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,2,2;2,0,0,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 33".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,0,0,0,2,2;0,0,0,2,2,2,0,2,2,2;0,0,0,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 34".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,0,0,0,2,2;0,2,2,2,2,2,0,2,2,2;0,2,2,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,0,2,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 35".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,2,2,0,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 36".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,2,2,2,2;0,0,0,0,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 37".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,2,2,2,2,2,2,2;0,0,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 38".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,2,0,2,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 39".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,2;0,0,2,0,0,0,0,0,0,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;0,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 40".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 3".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,2,0,0,0,0,2;0,0,2,2,2,2,0,0,0,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,2,2,0,0,0,2,2,2;0,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 41".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,2,0,2,0,0,0,0,2;2,0,2,2,2,2,0,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,2,2;0,0,2,2,0,0,0,2,2,2;0,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 42".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;2,0,2,2,2,0,0,0,0,2;2,0,2,2,2,2,0,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,2,0,0,2,2;0,0,2,2,2,2,2,2,2,2;0,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 43".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,2,0,0,0;2,0,2,2,2,2,2,0,0,2;2,0,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;2,0,0,0,0,2,0,0,2,2;2,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 44".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,2,2,2,2,2,0,0,0;2,0,2,2,2,2,2,0,0,2;2,0,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,2,0,0,0,0,0,0,2;2,2,2,0,0,2,0,0,2,2;2,2,2,2,2,2,0,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 45".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,2,0,0,0;0,0,2,2,2,2,2,0,0,0;0,0,2,2,2,2,2,0,0,0;2,0,2,2,2,2,2,0,0,2;2,0,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,2,0,0,0,2,0,0,2;2,2,2,0,0,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 46".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,2,2,2,2,2,0,0,0;0,0,2,2,2,2,2,0,0,0;2,0,2,2,2,2,2,0,0,2;2,0,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,2,0,0,0,2;0,0,2,0,2,2,2,0,0,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 47".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,0,0,2;2,2,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,2;0,0,0,2,0,2,0,0,0,2;0,0,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 48".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,0;0,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,0,0,0,0,0,2;2,0,0,2,0,2,0,0,0,2;2,2,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 49".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,0,0,0,0,0,2;2,0,0,2,0,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 50".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;2,0,0,2,2,2,0,0,0,2;2,0,0,2,2,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 51".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,0,2,2,2,2,0,0,0;2,2,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;2,0,0,2,2,2,2,2,2,2;2,0,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 52".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,0,2,0,0;2,2,0,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,2,0,0,0,0,0;2,2,0,2,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 53".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,2,2,0,0,0,0;2,2,2,2,2,2,0,2,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,0,0,2,2,2,2,0,0;2,2,0,2,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 54".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,2,2,0,0,0,2;2,2,2,2,2,2,0,2,0,2;2,0,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 55".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,2,0,0,0,0;2,2,0,0,2,2,0,0,0,2;2,2,2,2,2,2,0,2,0,2;2,0,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 56".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;2,2,0,0,0,2,2,0,0,0;2,2,0,0,2,2,2,0,0,2;2,2,2,2,2,2,2,2,0,2;2,0,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,2,2,2,0,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 57".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 6".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,2,0,0,0;2,2,2,0,0,2,2,0,0,0;2,2,2,2,2,2,2,0,0,2;2,2,2,2,2,2,2,2,0,2;2,0,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,0,0,0;2,2,2,2,2,2,0,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 58".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,2,0,0,0;2,2,2,0,0,2,2,2,0,0;2,0,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,2,2,0;0,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 59".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,2,0,0,2;2,2,2,0,0,2,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,2,0,2,2,2,2,2,0;0,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 60".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,2,2,0,0,2,0,0,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,0,2,0,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 61".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,0,0,0,0,0,0;2,2,2,2,0,0,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,2,2,0,2,0,0,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 62".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;2,2,2,2,0,0,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,0,2,2,0;0,0,2,2,0,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 63".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,2,2,2,2,2,0;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 64".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,0,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,2,2,2;0,0,0,0,2,2,2,2,2,2;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 65".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,2;0,0,0,0,2,2,2,2,2,2;0,0,0,0,2,2,2,2,2,2;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 66".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,2,0,0;0,0,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,2;0,2,0,0,2,2,2,2,2,2;0,2,2,0,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 3".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 67".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,2,2,0,0;0,0,2,2,2,2,2,2,0,0;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 68".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 69".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,2;0,0,0,0,0,0,0,2,2,2;0,0,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 70".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,2,2;0,2,0,0,0,0,0,2,2,2;2,2,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 71".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,2,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,2,2;0,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 72".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,2,0,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,2,0,0,0,0,2,2,2,2;0,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 73".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,2,2,0;2,2,0,0,0,0,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 74".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,0,0,0,2,2,2,2,0;2,2,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 75".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,2,0,0,2,2;2,2,0,0,0,2,2,2,2,2;2,2,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 76".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,0,0,0,2,2,2,2,2;2,2,0,0,0,2,2,2,2,2;2,2,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 77".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 6".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 78".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,0,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,0,0,0,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 9".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 79".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,2,0,2,0;2,2,2,0,2,2,2,2,2,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,0,0,0,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 2".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 80".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,2,0,0;2,2,2,2,2,0,2,2,0,0;2,2,2,2,2,2,2,2,2,0;2,2,2,0,2,2,2,2,2,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,2,0,0,0,0,2,2,2;0,2,0,0,0,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 81".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,2,0,2;2,2,2,2,2,0,2,2,0,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 82".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 9".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,0,0,0,2,2;2,2,2,2,2,0,0,2,2,2;2,2,2,2,2,0,2,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,2,2,2,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 83".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,0,0,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,2,2,2,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 84".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,2,0,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 85".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,2,0,0,0,2,2;2,2,2,2,2,2,0,0,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 86".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,2,2,2,0,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 87".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,2,2,0,0;0,0,0,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,2,0,0,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 88".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,0,0,2,2,0,2,2,2,2;0,0,0,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 89".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,2,2,2,0;0,0,0,2,2,2,2,2,2,2;0,0,0,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,2,0,0,0,0;2,2,0,0,2,2,2,2,2,2;2,2,0,0,2,2,2,2,2,2;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 13".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 90".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,2,0,0,0,0;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 16".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 91".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,2,0,0,0,2,2,0,0,0;2,2,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 16".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 92".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,2,0,0,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,2,0,0,2,2,0,0,0;0,2,2,2,0,2,2,0,0,0;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 16".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 93".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;2,0,0,2,2,0,0,0,2,2;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,2,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,0,0;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 16".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 94".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,2;2,0,2,2,2,0,0,0,2,2;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,0,0;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 16".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 95".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,0,0,0,0,0;0,2,2,2,2,0,0,0,0,2;2,2,2,2,2,0,0,0,2,2;0,2,2,2,2,2,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 96".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,0,0,0,0,0;0,2,2,2,2,0,0,0,0,2;0,2,2,2,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 97".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,0,0,2,0,0;0,2,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 98".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 12".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,2,2,0,0,0,0,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,2,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 99".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,2;2,2,2,0,0,0,0,2,2,2;2,2,2,0,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,2,0,0,0,0;2,2,2,2,2,2,0,0,0,0;2,2,2,2,2,2,2,2,0,0;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 100".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,2;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,2,0,0,0,2,2,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,0;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 101".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;2,0,0,0,0,2,2,0,0,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,2,0,0,0,2,2,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 102".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,2,0,0,0;2,0,0,2,2,2,2,0,0,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,2,0;0,2,0,0,0,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 103".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,0,0,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,0,0,0,2,2;0,0,0,0,0,2,2,0,2,2;0,2,0,0,0,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 104".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,0,0,0,2,2;0,0,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 105".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,2,0;0,0,0,2,2,2,0,0,2,2;0,0,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 106".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,0,2,2,2,2;0,0,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,2,0;0,0,0,2,2,2,0,0,2,2;0,0,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 107".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,2,2,0,0;0,2,2,0,0,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,0,2,0;0,0,0,2,2,2,2,2,2,2;0,0,0,2,2,2,2,2,2,2;0,2,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 108".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,2,2,0,0,2,2,0,0;0,2,2,2,2,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,2,0;0,0,0,2,2,2,2,2,2,2;0,0,0,2,2,2,2,2,2,2;0,2,0,2,2,2,2,0,2,2;0,2,0,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 109".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,0,0,2,2,0;0,0,2,2,0,0,2,2,2,2;0,2,2,2,2,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,2,0;0,0,0,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,0,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 19".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 110".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,2,2;0,0,0,0,0,0,0,2,2,2;0,0,2,2,0,0,2,2,2,2;0,2,2,2,2,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,2,0;2,0,0,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 111".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,2,2;0,2,2,2,0,0,0,2,2,2;0,2,2,2,0,0,2,2,2,2;0,2,2,2,2,0,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,2,0,2,2,2,2,0,0,0;0,2,0,2,2,2,2,0,2,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 112".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,2,2;0,2,2,2,0,2,2,2,2,2;0,2,2,2,0,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,2,2,2,2,2,2,0,0,0;0,2,2,2,2,2,2,0,2,0;2,2,2,2,2,2,2,0,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 113".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,0,0,0,2;0,0,0,0,2,2,0,0,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 114".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;0,0,0,0,0,0,2,2,0,2;0,0,0,0,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,2,0,0;0,0,2,2,2,2,2,2,0,0;0,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 115".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,2;2,0,0,0,0,0,2,2,0,2;2,2,0,0,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,2,2,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 116".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,0,0,0,0,2;2,0,2,0,0,0,2,2,0,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,0,0,2,2,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 117".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,0,0,2,0,0,0,2;2,0,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 118".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,0,0,2,0,0,0,2;2,2,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 22".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 119".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,2,0,0;2,2,2,0,0,2,0,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,2,2,2,2,0,0,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,0;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 120".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,0,0,0,2,0,0,0;2,0,0,0,0,0,2,2,0,0;2,2,2,0,0,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,2;0,2,2,2,2,0,0,0,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 1".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 121".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,0,0,0,2,0,0,0;2,0,0,0,2,2,2,2,0,0;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,0,0,2,2,2;0,2,2,2,2,0,2,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 122".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,0,0,0,2,2,2,0;2,0,0,0,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 123".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type Z".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,2,0,0,2,2,2,0;0,2,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,2,2,2,2,2,2,2,2;0,2,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 124".Split(' '));
            ParseLine("update game this_piece_type Z".Split(' '));
            ParseLine("update game next_piece_type O".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,2,0,0,0;2,2,0,2,0,0,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,0,0,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 125".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,2,0,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,2;2,0,0,2,2,2,2,0,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 126".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;2,0,0,2,2,0,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,2,0,0,0,2;2,2,2,2,2,2,2,0,2,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 127".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;2,2,2,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;2,0,0,2,2,0,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,0,2,0,0;0,2,2,0,0,2,0,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 128".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;2,2,2,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,2,2,0,0;2,0,0,2,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,2,0,0;0,2,2,0,0,0,0,2,0,0;0,2,2,0,0,2,0,2,0,2;0,2,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 129".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type J".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;2,2,2,2,0,0,0,0,0,0;0,0,0,2,2,0,2,2,2,2;0,0,0,2,2,2,2,2,0,0;2,0,0,2,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,2,0,0;2,2,2,0,0,0,0,2,0,0;2,2,2,0,0,2,0,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 130".Split(' '));
            ParseLine("update game this_piece_type J".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,2,0,0;2,2,2,2,0,0,0,2,2,2;0,0,0,2,2,0,2,2,2,2;0,0,0,2,2,2,2,2,0,0;2,0,0,2,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,2,0,0;2,2,2,0,2,2,2,2,0,0;2,2,2,0,0,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 131".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,2,0,0,2,0,0;0,0,0,2,2,0,2,2,2,2;0,0,0,2,2,2,2,2,0,0;2,0,0,2,2,2,2,0,0,0;2,2,0,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,2;2,2,0,0,0,0,0,2,0,2;2,2,2,0,2,2,2,2,2,2;2,2,2,0,0,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 132".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,2,0,0,2,0,0;0,0,0,2,2,0,2,2,2,2;0,2,2,2,2,2,2,2,0,0;2,0,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,2;2,2,2,2,0,0,0,2,0,2;2,2,2,2,0,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 133".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 15".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 skips 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,2,2,2,0,0,2,0,0;0,2,2,2,2,0,2,2,2,2;0,2,2,2,2,2,2,2,0,0;2,0,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,2,2,2,2,2;2,2,2,2,2,2,2,0,2,2;2,2,2,2,2,0,2,2,2,0;2,2,2,2,2,2,2,2,2,0;0,2,2,2,2,0,2,2,2,2;2,0,2,2,2,2,2,2,2,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,2,2,0,2;2,2,2,2,0,2,2,2,0,2;2,2,2,2,0,2,2,2,0,2;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3;3,3,3,3,3,3,3,3,3,3".Split(' '));
            ParseLine("update player2 row_points 25".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));


            #endregion
        }

        private void ParseLine(string[] lineArray)
        {
            switch (lineArray[0])
            {
                case "action":
                    DoMoves();
                    break;
                case "settings":
                    ParseSettings(lineArray.Skip(1).ToArray());
                    break;
                case "update":
                    ParseUpdate(lineArray.Skip(1).ToArray());
                    break;
                default:
                    //error
                    break;
            }
        }

        private void ParseSettings(string[] lineArray)
        {
            switch (lineArray[0])
            {
                case "your_bot":
                    _botName = lineArray[1];
                    break;
                case "field_height":
                    _height = int.Parse(lineArray[1]);
                    break;
                case "field_width":
                    _width = int.Parse(lineArray[1]);
                    break;
            }
        }

        private void ParseUpdate(string[] lineArray)
        {
            if (lineArray[0].Equals(_botName))
            {
                ParsePlayerUpdate(lineArray.Skip(1).ToArray());
            }
            else if (lineArray[0].Equals("game"))
            {
                ParseGame(lineArray.Skip(1).ToArray());
            }
        }

        private void ParseGame(string[] lineArray)
        {
            switch (lineArray[0])
            {
                case "round":
                    _round = int.Parse(lineArray[1]);
                    break;
                case "this_piece_type":
                    _block = lineArray[1];
                    break;
                case "next_piece_type":
                    _nextBlock = lineArray[1];
                    break;
                case "this_piece_position":
                    _blockX = int.Parse(lineArray[1].Split(',')[0]);
                    break;
            }
        }

        private void ParsePlayerUpdate(string[] lineArray)
        {
            switch (lineArray[0])
            {
                case "field":
                    if (!_debug || _gamestate == null)
                    {
                        ParseField(lineArray[1]);
                    }
                    break;
                case "skips":
                    _skips = int.Parse(lineArray[1]);
                    break;

            }
        }

        private void ParseField(string field)
        {
            if (!_debug || _gamestate == null)
            {
                _gamestate = new Matrix(field);
            }
        }

        private int _startY = 0;

        private void DoMoves()
        {
            var blocks = GetBlockMatrix(_block);
            if (blocks == null || blocks.Count == 0)
            {
                Console.WriteLine("no_moves");
            }
            _startY = findStartY();
            double bestScore = double.MinValue;
            string bestRoute = "";
            Matrix bestMatrix = null;
            if (_skips > 0)
            {
                bestMatrix = new Matrix(_gamestate, new Matrix(0, 0), 0, _gamestate.Height);
                bestScore = bestMatrix.Score;
                bestRoute = "skip";
            }
            string [,,] validPositions = new string[20+ValidPosOffset,20 + ValidPosOffset, 20 + ValidPosOffset];
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int x = (0 - block.Width) + 2; x < _gamestate.Width - 1; x++)
                {
                    for (int y = _startY; y < _height; y++)
                    {

                        var matrix = new Matrix(_gamestate, block, x, y);
                        string route = GetValidRoute(x, y, i, validPositions);
                        bool endpos = matrix.IsValid && !_gamestate.CanAdd(block, x, y + 1);
                        if (!string.IsNullOrEmpty(route) && matrix.IsValid)
                        {
                            validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] = route;
                            if (endpos)
                            {

                                double thisScore = GetScore(matrix);

                                if (thisScore > bestScore)
                                {
                                    //Console.Error.WriteLine("Score: " + thisScore);
                                    //Console.Error.WriteLine(matrix);
                                    bestScore = thisScore;
                                    bestMatrix = matrix;
                                    bestRoute = route;
                                }
                            }
                        }
                    }
                }

                for (int x = _gamestate.Width - 2; x > (0 - block.Width) + 3; x--)
                {
                    for (int y = _height - 1; y >= _startY; y--)
                    {

                        var matrix = new Matrix(_gamestate, block, x, y);
                        bool endpos = matrix.IsValid && !_gamestate.CanAdd(block, x, y + 1);
                        if (!string.IsNullOrEmpty(validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset]))
                        {
                            continue;
                        }
                        string route = GetValidRoute(x, y, i, validPositions);
                        if (!string.IsNullOrEmpty(route) && matrix.IsValid)
                        {
                            validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] = route;
                            if (endpos)
                            {
                                double thisScore = GetScore(matrix);
                                if (thisScore > bestScore)
                                {
                                    //Console.Error.WriteLine("Score: " + thisScore);
                                    //Console.Error.WriteLine(matrix);
                                    bestScore = thisScore;
                                    bestMatrix = matrix;
                                    bestRoute = route;
                                }
                            }
                        }
                    }
                }
            }
            if (_debug)
            {
                if(bestMatrix == null)
                {
                    Console.Error.WriteLine(_gamestate);
                    Console.Error.WriteLine(_startY);
                    Console.WriteLine("fuk");
                }
                _gamestate = bestMatrix;
                if (_gamestate != null)
                {
                    _gamestate.RemoveFullLines();
                }
            }

            Console.Error.WriteLine(bestMatrix);
            Console.Error.WriteLine("Full lines: " + bestMatrix.RemovedLines + " Solid Lines: " + bestMatrix.SolidLines);
            Console.WriteLine(bestRoute);
        }

        private int findStartY()
        {
            int highestPoint = 0;
            for (int y = 0; y > _height; y++)
            {
                for (int x = 0; x < _gamestate.Width; x++)
                {
                    if (_gamestate[x, y] != 0)
                    {
                        return Math.Max(y,0);
                    }
                }
            }
            return 0;
        }

        private string GetValidRoute(int x, int y, int i, string[,,] validPositions)
        {
            string route = "";
            if (y <= _startY)
            {
                for (int j = 0; j < _startY; j++)
                {
                    route += "down,";
                }
                int steps = x - _blockX;
                for (int j = 0; j < i; j++)
                {
                    route += "turnleft,";
                }
                for (int j = 0; j < Math.Abs(steps); j++)
                {
                    if (steps < 0)
                    {
                        route += "left,";
                    }
                    if (steps > 0)
                    {
                        route += "right,";
                    }
                }
                route += "down,";
                route = route.Trim(',');
            }
            //else there is already a route near this one. get the route from that pos and add the step we need to get here.
            if (!string.IsNullOrEmpty(validPositions[x - 1 + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset]))
            {
                return validPositions[x - 1 + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] + "," + "right";
            }
            if (!string.IsNullOrEmpty(validPositions[x + 1 + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset]))
            {
                return validPositions[x + 1 + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] + "," + "left";
            }
            if (!string.IsNullOrEmpty(validPositions[x + ValidPosOffset, y - 1 + ValidPosOffset, i + ValidPosOffset]))
            {
                return validPositions[x + ValidPosOffset, y - 1 + ValidPosOffset, i + ValidPosOffset] + "," + "down";
            }
            if (!string.IsNullOrEmpty(validPositions[x + ValidPosOffset, y + ValidPosOffset, i - 1 + ValidPosOffset]))
            {
                return validPositions[x + ValidPosOffset, y + ValidPosOffset, i - 1 + ValidPosOffset] + "," + "turnleft";
            }
            if (!string.IsNullOrEmpty(validPositions[x + ValidPosOffset, y + ValidPosOffset, i + 1 + ValidPosOffset]))
            {
                return validPositions[x + ValidPosOffset, y + ValidPosOffset, i + 1 + ValidPosOffset] + "," + "turnright";
            }
            return route;
        }

        private void MoveStuff()
        {
            List<Triple> triedMoves = new List<Triple>();
            string bestRoute = "";
            int bestScore = int.MaxValue;

        }

        private double GetScore(Matrix matrix, bool calculateNextBlock = true)
        {
            if (calculateNextBlock)
            {
                return GetSecondBlockScore(matrix) + matrix.Score;
            }
            else
            {
                return matrix.Score;
            }
        }

        private double GetSecondBlockScore(Matrix matrix)
        {
            Matrix bestOne = null;
            var start = findStartY();
            var blocks = GetBlockMatrix(_nextBlock);
            if (blocks == null || blocks.Count == 0)
            {
                return 10000;
            }
            double bestScore = double.MinValue;
            bool[,,] validPositions = new bool[20 + ValidPosOffset, 20 + ValidPosOffset, 20 + ValidPosOffset];
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int x = 0 ; x < _gamestate.Width; x++)
                {
                    for (int y = (start); y < _height; y++)
                    {

                        var newmatrix = new Matrix(matrix, block, x, y);
                        bool canBeReached = PosCanBeReached(validPositions, i, x, y);
                        bool endpos = newmatrix.IsValid && canBeReached && !matrix.CanAdd(block, x, y + 1);
                        if (newmatrix.IsValid)
                        {
                            validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] = canBeReached;
                            if (endpos)
                            {
                                double thisScore = GetScore(newmatrix, false);
                                if (thisScore > bestScore)
                                {
                                    bestScore = thisScore;
                                    bestOne = newmatrix;
                                }
                            }
                        }
                    }
                }

                for (int x = _gamestate.Width - 1; x > 0; x--)
                {
                    for (int y = _height - 1; y >= start; y--)
                    {

                        var newmatrix = new Matrix(matrix, block, x, y);
                        bool canBeReached = PosCanBeReached(validPositions, i, x, y);
                        bool endpos = newmatrix.IsValid && canBeReached && !matrix.CanAdd(block, x, y + 1);
                        if (validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset])
                        {
                            continue;
                        }
                        if (newmatrix.IsValid)
                        {
                            validPositions[x + ValidPosOffset, y + ValidPosOffset, i + ValidPosOffset] = canBeReached;
                            if (endpos)
                            {
                                double thisScore = GetScore(newmatrix, false);
                                if (thisScore > bestScore)
                                {
                                    bestScore = thisScore;
                                    bestOne = newmatrix;
                                }
                            }
                        }
                    }
                }
            }
 
            return bestScore;
        }

        private bool PosCanBeReached(bool[,,] vp, int i, int x, int y)
        {
            bool top = y < 1;
            i += ValidPosOffset;
            x += ValidPosOffset;
            y += ValidPosOffset;
            return top || vp[x, y, i - 1] || vp[x, y, i + 1] || vp[x - 1, y, i] || vp[x + 1, y, i] || vp[x, y - 1, i];
        }

        private List<Matrix> GetBlockMatrix(string block)
        {
            switch (block)
            {
                case "O":
                    return new List<Matrix> { new Matrix("2,2;2,2") };
                case "I":
                    return new List<Matrix> { new Matrix("0,0,0,0;2,2,2,2;0,0,0,0;0,0,0,0"), new Matrix("0,2,0,0;0,2,0,0;0,2,0,0;0,2,0,0"), new Matrix("0,0,0,0;0,0,0,0;2,2,2,2;0,0,0,0"), new Matrix("0,0,2,0;0,0,2,0;0,0,2,0;0,0,2,0") };
                case "J":
                    return new List<Matrix> { new Matrix("2,0,0;2,2,2;0,0,0"), new Matrix("0,2,0;0,2,0;2,2,0"), new Matrix("0,0,0;2,2,2;0,0,2"), new Matrix("0,2,2;0,2,0;0,2,0") };
                case "L":
                    return new List<Matrix> { new Matrix("0,0,2;2,2,2;0,0,0"), new Matrix("2,2,0;0,2,0;0,2,0"), new Matrix("0,0,0;2,2,2;2,0,0"), new Matrix("0,2,0;0,2,0;0,2,2") };
                case "Z":
                    return new List<Matrix> { new Matrix("2,2,0;0,2,2;0,0,0"), new Matrix("0,2,0;2,2,0;2,0,0"), new Matrix("0,0,0;2,2,0;0,2,2"), new Matrix("0,0,2;0,2,2;0,2,0") };
                case "T":
                    return new List<Matrix> { new Matrix("0,2,0;2,2,2;0,0,0"), new Matrix("0,2,0;2,2,0;0,2,0"), new Matrix("0,0,0;2,2,2;0,2,0"), new Matrix("0,2,0;0,2,2;0,2,0") };
                case "S":
                    return new List<Matrix> { new Matrix("0,2,2;2,2,0;0,0,0"), new Matrix("2,0,0;2,2,0;0,2,0"), new Matrix("0,0,0;0,2,2;2,2,0"), new Matrix("0,2,0;0,2,2;0,0,2") };
            }
            return null;
        }
    }
    public struct Triple
    {
        public Triple(int x, int y, int r)
        {
            X = x;
            Y = y;
            R = r;
        }
        public int X;
        public int Y;
        public int R;
    }
}

﻿using System;
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

        private void DoDebugStuff()
        {
            #region debug zut
            _debug = true;
            ParseLine("settings timebank 10000".Split(' '));
            ParseLine("settings time_per_move 500".Split(' '));
            ParseLine("settings player_names player1,player2".Split(' '));
            ParseLine("settings your_bot player1".Split(' '));
            ParseLine("settings field_width 10".Split(' '));
            ParseLine("settings field_height 20".Split(' '));
            ParseLine("update game round 1".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type L".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 2".Split(' '));
            ParseLine("update game this_piece_type L".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 3".Split(' '));
            //ParseLine("update game this_piece_type J".Split(' '));
            //ParseLine("update game next_piece_type T".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 0".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 4".Split(' '));
            //ParseLine("update game this_piece_type T".Split(' '));
            //ParseLine("update game next_piece_type O".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 1".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,2,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 5".Split(' '));
            //ParseLine("update game this_piece_type O".Split(' '));
            //ParseLine("update game next_piece_type L".Split(' '));
            //ParseLine("update game this_piece_position 4,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,2,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 6".Split(' '));
            //ParseLine("update game this_piece_type L".Split(' '));
            //ParseLine("update game next_piece_type O".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;2,2,2,2,0,0,0,2,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 7".Split(' '));
            //ParseLine("update game this_piece_type O".Split(' '));
            //ParseLine("update game next_piece_type J".Split(' '));
            //ParseLine("update game this_piece_position 4,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,0,0,0,0,0,0;2,2,2,2,0,0,2,0,0,0;2,2,2,2,2,2,2,2,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 8".Split(' '));
            //ParseLine("update game this_piece_type J".Split(' '));
            //ParseLine("update game next_piece_type O".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,2,2,0,0,0,0;2,2,2,2,2,2,2,0,0,0;2,2,2,2,2,2,2,2,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,0,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //Console.ReadLine();
            //ParseLine("update game round 9".Split(' '));
            //ParseLine("update game this_piece_type O".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 4,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,0,2,2,0,0,2,2;2,2,2,2,2,2,2,0,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,0,0,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 10".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,0,0,0,0,0,0,0;2,2,2,0,2,2,0,0,2,2;2,2,2,2,2,2,2,0,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 11".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type T".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,2,2,0,0,0,0,2,0,0;2,2,2,0,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 12".Split(' '));
            //ParseLine("update game this_piece_type T".Split(' '));
            //ParseLine("update game next_piece_type T".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,0,0,0,2,0,0;0,2,2,2,0,0,0,2,0,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,2,2,0,0,0,0,0,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 13".Split(' '));
            //ParseLine("update game this_piece_type T".Split(' '));
            //ParseLine("update game next_piece_type L".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,0,0,2,0,0,0,0,0,0;2,2,0,2,0,0,0,2,0,0;2,2,2,2,0,0,0,2,0,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,0,0;2,2,2,2,0,0,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 14".Split(' '));
            //ParseLine("update game this_piece_type L".Split(' '));
            //ParseLine("update game next_piece_type Z".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;2,0,0,2,0,0,2,2,0,0;2,2,0,2,0,0,2,2,0,0;2,2,2,2,0,0,0,2,0,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,0,2,2,0,0;2,2,2,2,0,0,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 15".Split(' '));
            //ParseLine("update game this_piece_type Z".Split(' '));
            //ParseLine("update game next_piece_type Z".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;2,0,0,2,2,0,2,2,0,0;2,2,0,2,2,0,2,2,0,0;2,2,2,2,2,2,0,2,0,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,2,0,0,0;2,2,2,0,0,0,2,2,0,0;2,0,0,0,0,0,2,2,0,0;2,2,2,2,0,0,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 16".Split(' '));
            //ParseLine("update game this_piece_type Z".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,2,0,0,0;2,2,2,2,2,0,2,2,0,0;2,2,0,2,2,0,2,2,0,0;2,2,2,2,2,2,0,2,0,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,2,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;2,2,2,0,0,0,2,2,0,0;2,0,0,0,0,0,2,2,0,0;2,2,2,2,0,0,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 17".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type J".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,2,0,0,0;2,2,2,2,2,0,2,2,0,2;2,2,0,2,2,0,2,2,2,2;2,2,2,2,2,2,0,2,2,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,2,2,2,0;0,0,0,0,0,0,2,2,2,0;0,0,0,0,0,0,2,2,0,0;2,2,2,0,0,0,2,2,0,0;2,0,0,0,0,0,2,2,0,0;2,2,2,2,0,0,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 18".Split(' '));
            //ParseLine("update game this_piece_type J".Split(' '));
            //ParseLine("update game next_piece_type J".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,0,0,0,0;2,2,0,0,0,2,2,0,0,0;2,2,2,2,2,2,2,2,0,2;2,2,0,2,2,2,2,2,2,2;2,2,2,2,2,2,0,2,2,0;2,2,2,2,2,2,0,2,2,2;2,2,2,2,2,2,2,2,2,0;2,2,2,2,2,2,2,2,2,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,2,0,0;0,0,0,0,0,0,2,2,2,0;0,0,0,0,0,0,2,2,2,0;0,0,0,0,0,2,2,2,0,0;2,2,2,0,0,2,2,2,0,0;2,0,0,0,0,2,2,2,0,0;2,2,2,2,0,2,2,2,2,0;0,2,2,2,2,2,2,0,0,0;0,0,0,2,2,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,2,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,2,0,2,0,0,0,0,0;0,2,2,0,2,0,0,2,2,2;0,0,2,2,2,2,0,0,2,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));


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
            }
        }

        private void ParseField(string field)
        {
            if (!_debug || _gamestate==null)
            {
                _gamestate = new Matrix(field);
            }
        }

        private void DoMoves()
        {
            var blocks = GetBlockMatrix(_block);
            if (blocks == null || blocks.Count == 0)
            {
                Console.WriteLine("no_moves");
            }
            int bestScore = int.MaxValue;
            string bestRoute = "";
            Matrix bestMatrix = null;
            Dictionary<Triple,string> validPositions = new Dictionary<Triple,string>();
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int x = (0 - block.Width) + 2; x < _gamestate.Width - 1; x++)
                {
                    for (int y = (0); y < _height; y++)
                    {

                        var matrix = new Matrix(_gamestate, block, x, y);
                        string route = GetValidRoute(x,y,i, validPositions);
                        if (!string.IsNullOrEmpty(route) && matrix.IsValid)
                        {
                            validPositions.Add(new Triple(x, y,i),route);

                            int thisScore = GetScore(matrix);
                            if (thisScore < bestScore)
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

                for (int x = _gamestate.Width - 2 ; x > (0 - block.Width) + 3; x--)
                {
                    for (int y = _height-1; y >= 0 ; y--)
                    {

                        var matrix = new Matrix(_gamestate, block, x, y);
                        if(validPositions.ContainsKey(new Triple(x, y, i)))
                        {
                            continue;
                        }
                        string route = GetValidRoute(x, y, i, validPositions);
                        if (!string.IsNullOrEmpty(route) && matrix.IsValid)
                        {
                            validPositions.Add(new Triple(x, y, i), route);

                            int thisScore = GetScore(matrix);
                            if (thisScore < bestScore)
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
            if (_debug)
            {
                _gamestate = bestMatrix;
                if (_gamestate != null)
                {
                    _gamestate.RemoveFullLines();
                }
                Console.Error.WriteLine(bestMatrix);
            }
            //
            //string command = "";
            //int steps = bestX - _blockX;
            //for(int j =0; j < besti; j++)
            //{
            //    command += "turnleft,";
            //}
            //for (int i = 0; i < Math.Abs(steps); i++)
            //{
            //    if (steps < 0)
            //    {
            //        command += "left,";
            //    }
            //    if(steps > 0)
            //    {
            //        command += "right,";
            //    }
            //}
           
            //command += "drop";
            Console.WriteLine(bestRoute);
        }

        private string GetValidRoute(int x, int y, int i, Dictionary<Triple, string> validPositions)
        {
            string route = "";
            if (y == 0)
            {
                
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
                route += "down";
                route = route.Trim(',');
            }
            //else there is already a route near this one. get the route from that pos and add the step we need to get here.
            if (validPositions.ContainsKey(new Triple(x - 1, y, i)))
            {
                return validPositions[new Triple(x - 1, y, i)] + "," + "right";
            }
            if (validPositions.ContainsKey(new Triple(x + 1, y, i)))
            {
                return validPositions[new Triple(x + 1, y, i)] + "," + "left";
            }
            if (validPositions.ContainsKey(new Triple(x, y-1, i)))
            {
                return validPositions[new Triple(x, y-1, i)] + "," + "down";
            }
            if (validPositions.ContainsKey(new Triple(x, y, i-1)))
            {
                return validPositions[new Triple(x, y, i-1)] + "," + "turnleft";
            }
            if (validPositions.ContainsKey(new Triple(x, y, i + 1)))
            {
                return validPositions[new Triple(x, y, i + 1)] + "," + "turnright";
            }
            return route;
        }

        private void MoveStuff()
        {
            List<Triple> triedMoves = new List<Triple>();
            string bestRoute = "";
            int bestScore = int.MaxValue;

        }

        private int GetScore(Matrix matrix, bool calculateNextBlock = true)
        {
            int score = 0;
            if (calculateNextBlock)
            {
               // score = GetSecondBlockScore(matrix);
            }
            int highestPoint = 0;
            bool highestPointSet = false;
            for(int y = matrix.Height-1; y >= 0; y--)
            {
                bool lineFull = true;

                for (int x=0; x < matrix.Width; x++)
                {
                    //lager is beter
                    if(matrix[x,y] != 0)
                    {
                        if (!highestPointSet)
                        {
                            score += (matrix.Height - y);
                            highestPoint = y;
                        }
                    }
                    else
                    {
                        lineFull = false;
                    }
                    //gaatjes zijn slechter
                    if(y>0 && matrix[x,y]==0)
                    {
                        
                        var suby = y - 1;
                        var foundFilledIn = false;
                        var penalty = 0;
                        while (suby > 0)
                        {
                            penalty += suby;
                            if (matrix[x, suby] != 0)
                            {
                                foundFilledIn = true;
                                break;
                            }
                            suby--;
                        }
                        if (foundFilledIn)
                        {
                            score += 5;
                        }
                    }
                    //lijnen weghalen is goed
                    if (lineFull)
                    {
                        score --;
                    }
                }
            }
            //lager in totaal is beter
            return score + ((_gamestate.Height - highestPoint ) );
        }

        private int GetSecondBlockScore(Matrix matrix)
        {
            var blocks = GetBlockMatrix(_nextBlock);
            if (blocks == null || blocks.Count == 0)
            {
                return 10000;
            }
            int bestScore = int.MaxValue;
            List<Triple> validPositions = new List<Triple>();
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int x = (0 - block.Width) + 2; x < matrix.Width - 1; x++)
                {
                    for (int y = (0 - block.Height) + 2; y < matrix.Height; y++)
                    {

                        var newMatrix = new Matrix(_gamestate, block, x, y);
                        if ((y == 0 || validPositions.Contains(new Triple(x, y - 1,i))) && newMatrix.IsValid)
                        {
                            validPositions.Add(new Triple(x, y,i));
                            int thisScore = GetScore(matrix, false);
                            if (thisScore < bestScore)
                            {
                                //Console.WriteLine("Score: " + thisScore);
                                //Console.WriteLine(matrix);
                                bestScore = thisScore;
                            }
                        }
                    }
                }
            }
            return bestScore;
        }

        private List<Matrix> GetBlockMatrix(string block)
        {
            switch (block)
            {
                case "O":
                    return new List<Matrix> { new Matrix("2,2;2,2") };
                case "I":
                    return new List<Matrix> { new Matrix("0,0,0,0;2,2,2,2;0,0,0,0;0,0,0,0"), new Matrix("0,2,0,0;0,2,0,0;0,2,0,0;0,2,0,0"), new Matrix("0,0,0,0;0,0,0,0;2,2,2,2;0,0,0,0"), new Matrix("0,0,2,0;0,0,2,0;0,0,2,0;0,0,2,0")};
                case "J":
                    return new List<Matrix> { new Matrix("2,0,0;2,2,2;0,0,0") , new Matrix("0,2,0;0,2,0;2,2,0"), new Matrix("0,0,0;2,2,2;0,0,2"), new Matrix("0,2,2;0,2,0;0,2,0")};
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

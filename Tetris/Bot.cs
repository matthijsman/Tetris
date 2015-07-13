using System;
using System.Collections.Generic;
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

        private string _block;
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
            //_botName = "player1";
            ////0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,2,2,2,2,0,0,0,0;0,0,0,0,2,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,2,0,0,0,0,0,0,0;0,0,2,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0;0,0,0,2,2,0,0,0,0,0
            ////0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0
            //var field = "update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0";

            //ParseLine(field.Split(' '));
            //ParseLine("update game this_piece_type J".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //Console.WriteLine(_gamestate);

            //ParseLine("settings timebank 10000".Split(' '));
            //ParseLine("settings time_per_move 500".Split(' '));
            //ParseLine("settings player_names player1,player2".Split(' '));
            //ParseLine("settings your_bot player1".Split(' '));
            //ParseLine("settings field_width 10".Split(' '));
            //ParseLine("settings field_height 20".Split(' '));
            //ParseLine("update game round 1".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type T".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 0".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 2".Split(' '));
            //ParseLine("update game this_piece_type T".Split(' '));
            //ParseLine("update game next_piece_type J".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 0".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,2,2,2,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 3".Split(' '));
            //ParseLine("update game this_piece_type J".Split(' '));
            //ParseLine("update game next_piece_type O".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 0".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,2,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));

            ParseLine("settings timebank 10000".Split(' '));
            ParseLine("settings time_per_move 500".Split(' '));
            ParseLine("settings player_names player1,player2".Split(' '));
            ParseLine("settings your_bot player1".Split(' '));
            ParseLine("settings field_width 10".Split(' '));
            ParseLine("settings field_height 20".Split(' '));
            ParseLine("update game round 1".Split(' '));
            ParseLine("update game this_piece_type O".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 4,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 2".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type I".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 3".Split(' '));
            ParseLine("update game this_piece_type I".Split(' '));
            ParseLine("update game next_piece_type T".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 0".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0;2,2,2,2,2,2,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 4".Split(' '));
            ParseLine("update game this_piece_type T".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 1".Split(' '));
            ParseLine("update player1 combo 1".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;2,2,0,0,0,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            ParseLine("update game round 5".Split(' '));
            ParseLine("update game this_piece_type S".Split(' '));
            ParseLine("update game next_piece_type S".Split(' '));
            ParseLine("update game this_piece_position 3,-1".Split(' '));
            ParseLine("update player1 row_points 1".Split(' '));
            ParseLine("update player1 combo 0".Split(' '));
            ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,2,0,0,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            ParseLine("update player2 row_points 0".Split(' '));
            ParseLine("update player2 combo 0".Split(' '));
            ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 6".Split(' '));
            //ParseLine("update game this_piece_type S".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 7".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type I".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 8".Split(' '));
            //ParseLine("update game this_piece_type I".Split(' '));
            //ParseLine("update game next_piece_type Z".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,1,1,1,1,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
            //ParseLine("update game round 9".Split(' '));
            //ParseLine("update game this_piece_type Z".Split(' '));
            //ParseLine("update game next_piece_type S".Split(' '));
            //ParseLine("update game this_piece_position 3,-1".Split(' '));
            //ParseLine("update player1 row_points 1".Split(' '));
            //ParseLine("update player1 combo 0".Split(' '));
            //ParseLine("update player1 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 field 0,0,0,0,1,1,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,0,0,0,0,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,2,2,2,2,0;0,0,0,0,0,0,0,2,2,0;0,0,0,0,0,0,2,2,0,0;0,0,0,0,0,2,2,0,0,0;0,0,0,2,2,2,0,0,0,0;2,2,2,2,2,0,0,0,0,0".Split(' '));
            //ParseLine("update player2 row_points 0".Split(' '));
            //ParseLine("update player2 combo 0".Split(' '));
            //ParseLine("action moves 10000".Split(' '));
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
                    ParseField(lineArray[1]);
                    break;
            }
        }

        private void ParseField(string field)
        {
            _gamestate = new Matrix(field);
        }

        private void DoMoves()
        {
            var block = GetBlockMatrix();
            if (block == null)
            {
                Console.WriteLine("no_moves");
            }
            int bestScore = int.MaxValue;
            int bestX = 0;
            for (int x = (0 - block.Width) + 2; x < _gamestate.Width - 1; x++)
            {
                for (int y = (0 - block.Height) + 2; y < _gamestate.Height; y++)
                {

                    var matrix = new Matrix(_gamestate, block, x, y);
                    if (matrix.IsValid)
                    {
                        int thisScore = GetScore(matrix);
                        if(thisScore < bestScore)
                        {
                            //Console.WriteLine("Score: " + thisScore);
                            //Console.WriteLine(matrix);
                            bestX = x;
                            bestScore = thisScore;
                        }
                    }
                }
            }
            string command = "";
            int steps = bestX - _blockX;

            for (int i = 0; i < Math.Abs(steps); i++)
            {
                if (steps < 0)
                {
                    command += "left,";
                }
                if(steps > 0)
                {
                    command += "right,";
                }
            }
           
            command += "drop";
            Console.WriteLine(command);
        }

        private int GetScore(Matrix matrix)
        {
            int score = 0;
            for(int y = matrix.Height-1; y >= 0; y--)
            {
                for (int x=0; x < matrix.Width; x++)
                {
                    if(matrix[x,y] != 0)
                    {
                        score += (matrix.Height -y);
                    }
                }
            }
            return score;
        }

        private Matrix GetBlockMatrix()
        {
            switch (_block)
            {
                case "O":
                    return new Matrix("2,2;2,2");
                case "I":
                    return new Matrix("0,0,0,0;2,2,2,2;0,0,0,0;0,0,0,0");
                case "J":
                    return new Matrix("2,0,0;2,2,2;0,0,0");
                case "L":
                    return new Matrix("0,0,2;2,2,2;0,0,0");
                case "Z":
                    return new Matrix("2,2,0;0,2,2;0,0,0");
                case "T":
                    return new Matrix("0,2,0;2,2,2;0,0,0");
                case "S":
                    return new Matrix("0,2,2;2,2,0;0,0,0");
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;

namespace BackEnd
{
    class MainAlgorithm
    {
        public static double RakXDistanceTip = 0.5;
        public static double RakYDistanceTip = 0.5;
        public static int RakCapacity = 96;
        public static int RakRow = 12;
        public static int RakColumn = 8;
        public static double Velocity = 5;
        public static double Z_source = 3;
        public static double Z_target = 3;
        public static double Z_tool = 3;
        public static double Z_tool_input = 0.1;
        public static double Z_exit = 3;
        public static double Z_exit_output = 0.1;
        public string CreateGCode(List<InputData> _sources, List<InputData> _targets, List<InputData> _tools, Position _exit_tool, bool _is_change_tip,
         int _method, double _push, double _release, out string _error)
        {
            List<Position> sources = CreatePositions(_sources);
            List<Position> targets = CreatePositions(_targets);
            List<Position> tools = CreatePositions(_tools);
            switch (_method)
            {
                case 1:
                    return PeerToPeer(sources, targets, tools, _exit_tool, _is_change_tip, _push, _release, out _error);
                case 2:
                    return OneToSeveral(out _error);
                case 3:
                    return SeveralToOne(out _error);
            }
            _error = "ERROR: Create GCode has problem!";
            return " ";
        }
        private List<Position> CreatePositions(List<InputData> _raks)
        {
            List<Position> outs = new List<Position>();
            foreach (InputData rak in _raks)
            {
                bool flag = false;
                int counter = 0;
                Position start_pt = GetPosition(rak.CalibratedPoint, rak.StartPoint);
                Position last_pt = new Position(start_pt.X, start_pt.Y, start_pt.Z);
                int _start_i = rak.StartPoint.IRow, _start_j = rak.StartPoint.IColumn;
                bool is_first = true;
                for (int i = _start_i; i < RakRow; i++)
                {
                    double _x, _y, _z = start_pt.Z;
                    int j = 0;
                    if (is_first)
                    {
                        j = _start_j;
                        is_first = false;
                    }
                    for (; j < RakColumn; j++)
                    {
                        _x = last_pt.X;
                        _y = last_pt.Y;
                        outs.Add(new Position(_x, _y, _z));
                        counter++;
                        if (counter == rak.NumberOfUnit)
                        {
                            flag = true;
                            break;
                        }
                        last_pt.X += RakXDistanceTip;
                    }
                    if (flag)
                        break;
                    last_pt.X = start_pt.X;
                    last_pt.Y += RakYDistanceTip;
                }
            }
            return outs;
        }
        private Position GetPosition(Position _start, StartPointRak _spr)
        {
            return new Position
            (
                _start.X + (_spr.IColumn * RakXDistanceTip),
                _start.Y + (_spr.IRow * RakYDistanceTip),
                _start.Z
            );
        }
        private string PeerToPeer(List<Position> _sources, List<Position> _targets, List<Position> _tools,
        Position _exit_tool, bool is_change_tip, double _push, double _release, out string _error)
        {
            string _out = " ";
            if (_sources.Count != _targets.Count)
            {
                _error = "ERROR: The number of source and target are not equal!";
                return _out;
            }
            if (is_change_tip && _sources.Count != _tools.Count)
            {
                _error = "ERROR: The number of source, target, and tips are not equal!";
                return _out;
            }
            if (!is_change_tip && _tools.Count == 0)
            {
                _error = "ERROR: Number of tips must be more than 0!";
                return _out;
            }
            _error = "OK";
            bool is_first = true;
            if (_sources.Count > 0)
            {
                _out = "G01 F" + Velocity.ToString() + "\n";
            }
            for (int i = 0; i < _sources.Count; i++)
            {
                if (is_change_tip)
                {
                    _out += AddLineToGCode(_tools[i], "tool", 0);
                    _out += AddLineToGCode(_sources[i], "source", _release);
                    _out += AddLineToGCode(_targets[i], "target", _push);
                    _out += AddLineToGCode(_exit_tool, "exit", 0);
                }
                else
                {
                    if (is_first)
                    {
                        _out += AddLineToGCode(_tools[0], "tool", 0);
                        is_first = false;
                    }
                    _out += AddLineToGCode(_sources[i], "source", _release);
                    _out += AddLineToGCode(_targets[i], "target", _push);
                }
            }
            return _out;
        }
        private string OneToSeveral(out string _error)
        {
            _error = "ERROR: This method has not implemented yet!";
            string _out = " ";
            return _out;
        }
        private string SeveralToOne(out string _error)
        {
            _error = "ERROR: This method has not implemented yet!";
            string _out = " ";
            return _out;
        }
        private string AddLineToGCode(Position pt, string type, double _motor4)
        {
            string _out = "";
            switch (type)
            {
                case "tool":
                    _out += "G01 X" + pt.X.ToString() + " Y" + pt.Y.ToString() + "\n";
                    _out += "G01 Z" + pt.Z.ToString() + "\n";
                    _out += "G01 Z" + (Z_tool_input + pt.Z).ToString() + "\n";
                    _out += "G01 Z" + (-Z_tool).ToString() + "\n";
                    return _out;
                case "source":
                    _out += "G01 X" + pt.X.ToString() + " Y" + pt.Y.ToString() + "\n";
                    _out += "G01 Z" + pt.Z.ToString() + "\n";
                    _out += "G01 E" + _motor4.ToString() + "\n";//motor4
                    _out += "G01 Z" + (-Z_source).ToString() + "\n";
                    return _out;
                case "target":
                    _out += "G01 X" + pt.X.ToString() + " Y" + pt.Y.ToString() + "\n";
                    _out += "G01 Z" + pt.Z.ToString() + "\n";
                    _out += "G01 E" + _motor4.ToString() + "\n";//motor4
                    _out += "G01 Z" + (-Z_target).ToString() + "\n";
                    return _out;
                case "exit":
                    _out += "G01 Z" + pt.Z.ToString() + "\n";
                    _out += "G01 X" + pt.X.ToString() + " Y" + pt.Y.ToString() + "\n";
                    _out += "G01 Z" + (pt.Z - Z_exit_output).ToString() + "\n";
                    _out += "G01 Z" + (-Z_tool).ToString() + "\n";
                    return _out;
            }
            return "";
        }
    }
}

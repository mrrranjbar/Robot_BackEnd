using System;

namespace BackEnd
{
    class StartPointRak
    {
        public int IRow, IColumn; // IRow: 0 - 11 , IColumn: 0 - 8
        public StartPointRak(int _number, string _label)
        {
            IRow = _number - 1; // number is 1 - 12 
            switch (_label)
            {
                case "A":
                    IColumn = 0;
                    break;
                case "B":
                    IColumn = 1;
                    break;
                case "C":
                    IColumn = 2;
                    break;
                case "D":
                    IColumn = 3;
                    break;
                case "E":
                    IColumn = 4;
                    break;
                case "F":
                    IColumn = 5;
                    break;
                case "G":
                    IColumn = 6;
                    break;
                case "H":
                    IColumn = 7;
                    break;
            }
        }

    }
}

using System;
using System.Collections.Generic;


namespace BackEnd
{
    class InputData
    {
       public bool IsSource; //type
       public StartPointRak StartPoint;
       public int Index;
       public int NumberOfUnit;
       public Position CalibratedPoint;
       public InputData(Position _calibratedPosition, int _start_point_rak_number, string _start_point_rak_label ,bool _is_source, int _index, int _number_of_unit)
       {
           StartPoint = new StartPointRak(_start_point_rak_number,_start_point_rak_label);
           IsSource = _is_source;
           Index = _index;
           NumberOfUnit = _number_of_unit;
           CalibratedPoint = new Position(_calibratedPosition.X,_calibratedPosition.Y,_calibratedPosition.Z);
       }
    }
}

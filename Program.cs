using System;
using System.Collections.Generic;

namespace BackEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            //source
            List<InputData> sources = new List<InputData>();
            Position calibratedPositionSource = new Position(10, 60, 15);
            InputData source = new InputData(calibratedPositionSource, 6, "F", true, 1, 9);
            InputData source1 = new InputData(calibratedPositionSource, 3, "B", true, 1, 1);
            InputData source2 = new InputData(calibratedPositionSource, 2, "A", true, 1, 10);
            sources.Add(source);
            sources.Add(source1);
            sources.Add(source2);

            //target
            List<InputData> targets = new List<InputData>();
            Position calibratedPositionTarget = new Position(90, 60, 15);
            InputData target = new InputData(calibratedPositionTarget, 1, "A", false, 2, 4);
            InputData target1 = new InputData(calibratedPositionTarget, 1, "A", false, 2, 14);
            InputData target2 = new InputData(calibratedPositionTarget, 1, "A", false, 2, 2);
            targets.Add(target);
            targets.Add(target1);
            targets.Add(target2);

            //tool
            List<InputData> tools = new List<InputData>();
            Position calibratedPositionTool = new Position(10, 15, 20);
            InputData tool = new InputData(calibratedPositionTool, 6, "D", false, 1, 5);
            InputData tool1 = new InputData(calibratedPositionTool, 3, "F", false, 1, 12);
            InputData tool2 = new InputData(calibratedPositionTool, 12, "B", false, 1, 3);
            tools.Add(tool);
            tools.Add(tool1);
            tools.Add(tool2);

            //exit tool position
            Position exit_tool_pos = new Position(160, 90, 18);


            //push value
            double push = 10;

            //release value
            double release = 5;

            //error message
            string error_message;


            //run main algorithm
            MainAlgorithm ma = new MainAlgorithm();
            string res = ma.CreateGCode(sources, targets, tools, exit_tool_pos, true, 1, push, release, out error_message);
            Console.WriteLine("Start Program:");
            Console.Write(res);
            Console.WriteLine("Error message: " + error_message);
            System.IO.File.WriteAllText(@"D:\gcode.txt", res);
        }
    }
}

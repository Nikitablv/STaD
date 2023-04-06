using System;
using System.Collections.Generic;
using System.Globalization;

namespace LW1
{
    public class Program
    {
        const string unknownError = "неизвестная ошибка";
        public static void Main(string[] args)
        {
            List<double> sides = new List<double>();
            if (args.Length == 3)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    bool isNumber = double.TryParse(args[i], out double side);
                    if (isNumber)
                        sides.Add(side);
                    else
                    {
                        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                        isNumber = double.TryParse(args[i], NumberStyles.AllowDecimalPoint, formatter, out side);
                        if (isNumber)
                            sides.Add(side);
                        else
                        {
                            Console.WriteLine(unknownError);
                            Environment.Exit(0);
                        }
                    }
                }
                var triangle = new Triangle(sides[0], sides[1], sides[2]);
                Console.WriteLine(triangle.GetShape());
            }
            else
                Console.WriteLine(unknownError);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace sudoku_game
{
    class Program
    {
        private string a = "string a";
        private string b = "string b";
        private string c = "string c";
        static void Main(string[] args)
        {
            Program[,] programs = new Program[,] { { new Program() }, { new Program() } };
            foreach (Program program in programs)
            {
                program.a = "new string a";
                program.b += " new";
            }
            for (int i = 0; i < programs.GetLength(0); i++)
            {
                for (int s = 0; s < programs.GetLength(1); s++)
                {
                    Console.WriteLine(programs[i, s].a);
                }
            }
            //Array.ForEach<Program[]>(programs, x =>
            //{
            //    Array.ForEach<Program>(x, v => Console.WriteLine($"{v.a}\n{v.b}\n{v.c}"));
            //});


            //Map map = new Map();
            //map.PrintMap();
        }
    }
}


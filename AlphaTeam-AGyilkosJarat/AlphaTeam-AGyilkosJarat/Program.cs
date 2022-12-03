using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaTeam_AGyilkosJarat
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(80, 25);

            int wagon1_row = 7;
            int wagon1_col = 7;
            int[,] wagon1 = new int[wagon1_row, wagon1_col];

            for (int i = 0; i < wagon1.GetLength(0); i++)
            {
                for (int j = 0; j < wagon1.GetLength(1); j++)
                {
                    //middle path
                    if (i == (wagon1_row - 1) / 2 && j > wagon1_col-wagon1_col)
                    {
                        Console.Write("-");
                        Console.Write("\t");
                        
                    }
                    //chairs
                    else if ((i > wagon1_row-wagon1_row && i < wagon1_row-1) && (j < wagon1_col-1 && j > wagon1_col-wagon1_col))
                    {
                        Console.Write("L");
                        Console.Write("\t");
                    }
                    //around
                    else
                    {
                        //adding more '#' at exit side
                        if ((i == ((wagon1_row - 1) / 2) - 1 || i == ((wagon1_row - 1) / 2) + 1) && j == wagon1_col-1)
                        {
                            Console.Write("#######");
                        }
                        else
                        {
                            Console.Write($"#");
                            Console.Write("\t");
                        }
                    }
                    
                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlphaTeam_AGyilkosJarat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();


            //set window size
            Console.SetWindowSize(80, 25);


            //wagon[NxN] field, mainly (will be) odd numbers to get nice field
            int wagon1_row = 7;
            int wagon1_col = 7;
            int[,] wagon1 = new int[wagon1_row, wagon1_col];

            int player_pos_y = (wagon1_row - 1) / 2;
            int player_pos_x = 1;

            bool playing = true;

            //hp
            char heart_emoji = '\u2665';
            byte player_hp = 5;

            //generate enemies
            //even number for size, every oddth value will represent 'y' and every eventh value will represent 'x'
            int[] enemies_coordinates = new int[6]; //even number / 2 = enemies number
            //enemies will only on chairs coordinates
            for (int i = 0; i < enemies_coordinates.Length/2; i++)
            {
                //upper or lower side(0 = upper, 1 = lower)
                int upper_or_lower_side = random.Next(0, 2);

                int enemy_pos_y;
                int enemy_pos_x;

                if (upper_or_lower_side == 0)
                {
                    enemy_pos_y = random.Next(wagon1_row-wagon1_row+1, (wagon1_row-1)/2);
                    enemies_coordinates[i] = enemy_pos_y;

                    enemy_pos_x = random.Next(wagon1_col - wagon1_col+1, wagon1_col - 1);
                    enemies_coordinates[i + 1] = enemy_pos_x;
                }
                else
                {
                    enemy_pos_y = random.Next(((wagon1_row - 1) / 2) + 1, wagon1_row-1);
                    enemies_coordinates[i] = enemy_pos_y;

                    enemy_pos_x = random.Next(wagon1_col - wagon1_col + 1, wagon1_col - 1);
                    enemies_coordinates[i + 1] = enemy_pos_x;
                }

                Console.WriteLine($"enemy -> y: {enemy_pos_y} | x: {enemy_pos_x}");
            }

            while (playing)
            {
                for (int i = 0; i < wagon1.GetLength(0); i++)
                {
                    for (int j = 0; j < wagon1.GetLength(1); j++)
                    {
                        //print player
                        if (i == player_pos_y && j == player_pos_x)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("@");
                            Console.ResetColor();
                            Console.Write("\t");
                        }
                        else
                        {
                            //middle path
                            if (i == (wagon1_row - 1) / 2 && j > wagon1_col - wagon1_col)
                            {
                                Console.Write("-");
                                Console.Write("\t");

                            }
                            //chairs
                            else if ((i > wagon1_row - wagon1_row && i < wagon1_row - 1) && (j < wagon1_col - 1 && j > wagon1_col - wagon1_col))
                            {
                                Console.Write("L");
                                Console.Write("\t");
                            }
                            //around
                            else
                            {
                                //adding more '#' at exit side
                                if ((i == ((wagon1_row - 1) / 2) - 1 || i == ((wagon1_row - 1) / 2) + 1) && j == wagon1_col - 1)
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

                    }
                    Console.WriteLine();
                    Console.WriteLine();

                }

                //        -red hearts for hp
                Console.Write($"Élet: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{string.Concat(Enumerable.Repeat($"{heart_emoji}", player_hp))}");
                Console.ResetColor();



                Console.WriteLine("\n\n[1]-Fel\n[2]-Le\n[3]-Jobbra\n[4]-Balra\n\n[8]-Kilépés\n");
                Console.Write("Válasz: ");
                
                int player_move = int.Parse(Console.ReadLine());

                //player moving (up, down, left, right)
                //player can't go through the "walls"
                if (player_move == 1 && player_pos_x != wagon1_col - 1) //up
                {
                    //won't allow to go up if the player pos y and x in the middle path at the exit pos
                    if (player_pos_y != wagon1_row-wagon1_row+1)
                    {
                        player_pos_y--;
                    }
                }
                else if (player_move == 2) //down
                {
                    //won't allow to go down if the player pos y and x in the middle path at the exit pos
                    if (player_pos_y != wagon1_row-2 && player_pos_x != wagon1_col - 1)
                    {
                        player_pos_y++;
                    }
                }
                else if (player_move == 3) //right
                {
                    //allowing the exit position (last '_' char on the playing field)
                    if (player_pos_y == (wagon1_row - 1) / 2 && player_pos_x != wagon1_col - 1)
                    {
                        player_pos_x++;
                    }
                    else if (player_pos_y != (wagon1_row - 1) / 2 && player_pos_x != wagon1_col - 2)
                    {
                        player_pos_x++;
                    }

                }
                else if (player_move == 4) //left
                {
                    if (player_pos_x != wagon1_col - wagon1_col + 1)
                    {
                        player_pos_x--;
                    }


                }
                else if (player_move == 8) //exit
                {
                    playing = false;
                }

                //clearing the old stuff
                Console.Clear();
            }
            

        }
    }
}

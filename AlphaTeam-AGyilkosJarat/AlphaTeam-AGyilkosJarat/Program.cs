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

            //coin
            byte spawnable_coin_amount = 5;
            int player_coin = 10;
            //generate coin pos
            int coin_position_x = random.Next(1, wagon1_col-1);
            int coin_position_y = random.Next(1, wagon1_row-1);


            while (playing)
            {
                for (int i = 0; i < wagon1.GetLength(0); i++)
                {
                    for (int j = 0; j < wagon1.GetLength(1); j++)
                    {
                        if ((coin_position_y == i && coin_position_x == j) && spawnable_coin_amount > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write('$');
                            Console.ResetColor();
                            Console.Write('\t');
                        }
                        else
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


                                if (player_pos_y == coin_position_y && player_pos_x == coin_position_x)
                                {
                                    Console.Beep();
                                    player_coin += 10;
                                    spawnable_coin_amount--;
                                    //the coin won't spawn in the same pos as the player
                                    coin_position_x = random.Next(1, wagon1_col - 1);
                                    coin_position_y = random.Next(1, wagon1_row - 1);
                                    while (coin_position_x == player_pos_x && coin_position_y == player_pos_y)
                                    {
                                        coin_position_x = random.Next(1, wagon1_col - 1);
                                        coin_position_y = random.Next(1, wagon1_row - 1);
                                    }
                                }

                            }
                        }

                    }
                    Console.WriteLine();
                    Console.WriteLine();

                }
                //display -remaining coins
                Console.WriteLine($"Hátralévő érmék száma: {spawnable_coin_amount}");

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

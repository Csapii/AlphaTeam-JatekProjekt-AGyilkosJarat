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
        static Random random = new Random();


        //Random string generator 
        static string RandomString(string difficulty = "normal")
        {

            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789<>#&@?%-.";
            string randomString = "";
            int amount = 0;
            if (difficulty == "normal")
            {
                amount = 10;
            }
            else if (difficulty == "easy")
            {
                amount = 5;
            }
            else if (difficulty == "hard")
            {
                amount = 20;
            }

            for (int i = 1; i <= amount; i++)
            {

                randomString = randomString.Insert(random.Next(randomString.Length),characters[random.Next(characters.Length - 1)].ToString());
            }



            return randomString;
        }

        static void Main(string[] args)
        {
            

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
            int remaining_enemies = enemies_coordinates.Length/2;
            int[] enemies_hp = new int[3];
            int cnt_for_enemy_coordinates = 0;
            int cnt_for_enemy_hp = 0;
            //enemies will only on chairs coordinates
            for (int i = 0; i < enemies_coordinates.Length-1; i++)
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

                if (i<3)
                {
                    int random_hp_for_enemy = random.Next(1, 4);
                    enemies_hp[i] = random_hp_for_enemy;
                }

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
                                
                                if (enemies_coordinates[cnt_for_enemy_coordinates] == i && enemies_coordinates[cnt_for_enemy_coordinates+1] == j)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("!");
                                    Console.ResetColor();
                                    Console.Write("\t");
                                }
                                else
                                {

                                    Console.Write("L");
                                    Console.Write("\t");
                                }

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

                Console.WriteLine($"\tHátralévő ellenfelek: {remaining_enemies}");



                if (player_pos_y == enemies_coordinates[cnt_for_enemy_coordinates] && player_pos_x == enemies_coordinates[cnt_for_enemy_coordinates+1])
                {
                    Console.WriteLine("\n\n[1]-Fel\t\t[7]-Harc\n[2]-Le\n[3]-Jobbra\n[4]-Balra\n\n[8]-Kilépés\n");
                    Console.Write("Válasz: ");
                }
                else
                {
                    Console.WriteLine("\n\n[1]-Fel\n[2]-Le\n[3]-Jobbra\n[4]-Balra\n\n[8]-Kilépés\n");
                    Console.Write("Válasz: ");

                }

                int player_move = int.Parse(Console.ReadLine());

                //fight with enemies
                if (player_move == 7 && (player_pos_y == enemies_coordinates[cnt_for_enemy_coordinates] && player_pos_x == enemies_coordinates[cnt_for_enemy_coordinates+1]))
                {
                    Console.Clear();
                    bool fighting_last_prompt = true;
                    while (fighting_last_prompt)
                    {
                        Console.WriteLine($"Üdvözöllek a harcrendszer bemutatásában !");
                        Console.WriteLine($"Jelenleg nincsen semmilyen fegyvered amit használni tudnál, szóval csak verekedni tudsz.");
                        Console.WriteLine($"Ha jól írod be a megadott szöveget, akkor sikeresen támadtál, ellenkező esetben életet fogsz veszíteni.");

                        Console.WriteLine($"Biztosan harcolni szeretnél?\n[1]-Igen\t[2]-Nem");
                        Console.Write("Válasz: ");
                        int fight_or_not = int.Parse(Console.ReadLine());

                        if (fight_or_not == 1)
                        {
                            Console.Clear();
                            bool active_fight = true;
                            byte enemy_hp = (byte)enemies_hp[cnt_for_enemy_hp];
                            while (active_fight)
                            {
                                string random_string_for_attack = RandomString(difficulty:"easy").Trim();
                                Console.WriteLine($"Beírandó szöveg: {random_string_for_attack}");
                                Console.Write($"Életerőd: ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{string.Concat(Enumerable.Repeat($"{heart_emoji}", player_hp))}\t");
                                Console.ResetColor();
                                Console.Write($"Ellenfél életerő: ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{string.Concat(Enumerable.Repeat($"{heart_emoji}", enemy_hp))}\n");
                                Console.ResetColor();

                                Console.Write("Támadás: ");
                                string attack_string_user_input = Console.ReadLine().Trim(); //removing extra spaces

                                if (attack_string_user_input == random_string_for_attack)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("SIKERES TÁMADÁS!");
                                    Console.ResetColor();
                                    enemy_hp--;
                                    if (enemy_hp == 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Sikeresen legyőzted az ellenfeledet !");
                                        Console.ResetColor();
                                        Console.WriteLine("Jutalmak: "); //TODO
                                        Console.WriteLine("Nyomj meg egy gombot a továbblépéshez!");
                                        Console.ReadKey();
                                        active_fight = false;
                                        fighting_last_prompt = false;
                                        remaining_enemies--;
                                        if (remaining_enemies != 0)
                                        {
                                            cnt_for_enemy_coordinates += 2;
                                        }
                                        else
                                        {
                                            cnt_for_enemy_coordinates = 0; //need to remove coordinates for this line
                                        }
                                        
                                        cnt_for_enemy_hp++;
                                        //TODOs
                                        //Remove died enemies from array
                                        //temp 'removing' (not proper)
                                        enemies_coordinates[0] = 0;
                                        enemies_coordinates[1] = 0;


                                        //Rewards
                                        //Code comments
                                        //Print out the next enemy
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("SIKERTELEN TÁMADÁS!");
                                    Console.ResetColor();
                                    player_hp--;
                                    if (player_hp == 0)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor= ConsoleColor.Red;
                                        Console.WriteLine("SAJNOS EZ A HARC NEM SIKERÜLT, MEGHALTÁL!");
                                        Console.ResetColor();
                                        Console.WriteLine("Vége a játéknak...");
                                        Console.Write("Nyomj meg egy gombot a kilépéshez.");
                                        Console.ReadKey();
                                        fighting_last_prompt=false;
                                        active_fight=false;
                                        playing = false;
                                    }
                                    //TODOs
                                    //Player died
                                    //restart the whole game or quit(?)
                                }


                            }
                        }
                        else
                        { 
                            fighting_last_prompt = false;
                        }
                    }
                }


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

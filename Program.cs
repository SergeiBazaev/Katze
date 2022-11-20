using System;
using SFML.Learning;
using SFML.Graphics;
using SFML.Window;


namespace ConsoleApp1
{
    internal class Program : Game
    {
        static string beckgroundTexture = LoadTexture("background.png");
        static string playerTexture = LoadTexture("player.png");
        static string foodTexture = LoadTexture("food.png");

        static string meowSaund = LoadSound("meow_sound.wav");
        static string crashSound = LoadSound("cat_crash_sound.wav");
        static string bgMusic = LoadMusic("bg_music.wav");

        static float playerX = 300;
        static float playerY = 200;

        static float playerSpeed = 100;
         static int playerDirection = 1;
        static int playerSize = 56;

        static int playerScore = 0;

        static float foodX;
        static float foodY;
        static int foodSize = 32;

        static int record;

        static void PlayerMove()
        {
            if (GetKey(Keyboard.Key.W) == true) playerDirection = 0;
            if (GetKey(Keyboard.Key.D) == true) playerDirection = 1;
            if (GetKey(Keyboard.Key.S) == true) playerDirection = 2;
            if (GetKey(Keyboard.Key.A) == true) playerDirection = 3;

            if (playerDirection == 0)  playerY -= playerSpeed * DeltaTime;
            if (playerDirection == 1)  playerX += playerSpeed * DeltaTime;
            if (playerDirection == 2)  playerY += playerSpeed * DeltaTime;
            if (playerDirection == 3)  playerX -= playerSpeed * DeltaTime;
        }

        static void DrowPlayer()
        {
            if (playerDirection == 0) DrawSprite(playerTexture,playerX,playerY,64,64,playerSize,playerSize);
            if (playerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
            if (playerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
            if (playerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
        }


        static void Main(string[] args)
        {
            InitWindow(800, 600,"Katze");

            SetFont("comic.ttf");

            Random random = new Random();
            foodX = random.Next(0, 800 - foodSize);
            foodY = random.Next(200, 600 - foodSize);

            bool isLose = false;

            PlayMusic(bgMusic,10);

            while (true)
            {
                // 1. Расчет
                DispatchEvents();

                if(isLose == false)
                {
                    PlayerMove();

                    if (playerX + playerSize > foodX && playerX < foodX + foodSize
                    && playerY + playerSize > foodY && playerY < foodY + foodSize)
                    {
                        foodX = random.Next(0, 800 - foodSize);
                        foodY = random.Next(200, 600 - foodSize);
                        playerScore += 1;
                        playerSpeed += 10;
                        PlaySound(meowSaund);
                    }

                    if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 150)
                    {
                        isLose = true;
                        PlaySound(crashSound);
                        if(playerScore > record)
                        {
                            record = playerScore;   
                        }
                    }
                }

                if (isLose == true)
                {
                    if (GetKeyDown(Keyboard.Key.R))
                    {
                        isLose = false;
                         playerX = 300;
                         playerY = 200;

                         playerSpeed = 100;
                         playerDirection = 1;
                        playerScore = 0;

                    }
                }  

                
                // Игровая логика
                
                // 2. Очистка буфера и окна

                ClearWindow(Color.Magenta);

                // 3. Отрисовка буфера на окне
                DrawSprite(beckgroundTexture, 0, 0);

                // Вызов методов отрисовки объектов
                if(isLose == true)
                {
                    SetFillColor(50, 50, 50);
                    DrawText(200, 300, "Ну и чего ты носишься по кухне?!", 24);

                    SetFillColor(70, 70, 70);
                    DrawText(200, 350, "Для перезапуска игры нажмите букву \"R\" !", 18);
                }

                DrowPlayer();
                DrawSprite(foodTexture,foodX,foodY);

                SetFillColor(70, 70, 70);
                DrawText(20, 4, "Съедено корма: " + playerScore.ToString() , 18);

                SetFillColor(70, 70, 70);
                DrawText(20, 22, "Рекорд: " + record.ToString(), 18);

                DisplayWindow();

                // 4. Ожидание

                Delay(1);


            }
            Console.ReadLine();
        }
    }
}

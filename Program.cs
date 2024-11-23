using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp9
{
    internal class Program
    {
        // Определяем размеры игрового поля
        const int Width = 50;
        const int Height = 20;

        // Устанавливаем размеры консоли с учетом поля и дополнительного пространства
        const int ConsoleWidth = Width + 2; // +2 для границ
        const int ConsoleHeight = Height + 3; // +3 для границ и текста

        enum Direction { Up, Down, Left, Right }
        enum SpecialFoodType { None, RedX, BlueX }

        struct Position
        {
            public int X;
            public int Y;

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        static Position redSpecialFood = new Position(-1, -1);
        static Position blueSpecialFood = new Position(-1, -1);
        static bool isBlueSpecialFoodActive = false;
        static int nextBlueSpecialFoodScore = 30;
        static int lastRedSpecialFoodScore = 0;

        static void Main(string[] args)
        {
            SetConsoleWindowSize(ConsoleWidth, ConsoleHeight);

            Position snakeHead = new Position(Width / 2, Height / 2);
            List<Position> snakeBody = new List<Position>();
            Position food = GenerateFoodPosition(snakeBody); // Передаем snakeBody

            Direction direction;
            int speed = 150;
            bool isGameOver = false;

            int score = 0;

            Console.CursorVisible = false;

            DrawStartMessage();
            direction = WaitForStartInput();
            Console.Clear();

            while (!isGameOver)
            {
                GenerateSpecialFood(score);

                DrawField(snakeHead, snakeBody, food);
                DrawScore(score);

                direction = HandleInput(direction, ref speed);

                snakeBody.Insert(0, new Position(snakeHead.X, snakeHead.Y));
                snakeHead = MoveSnakeHead(snakeHead, direction);

                isGameOver = CheckCollisions(snakeHead, snakeBody);

                if (snakeHead.X == food.X && snakeHead.Y == food.Y)
                {
                    food = GenerateFoodPosition(snakeBody); // Передаем snakeBody
                    score++;
                }
                else if (snakeHead.X == redSpecialFood.X && snakeHead.Y == redSpecialFood.Y)
                {
                    redSpecialFood = new Position(-1, -1); // Удаляем красный X после сбора
                    score += 2; // Добавляем 2 очка
                }
                else if (snakeHead.X == blueSpecialFood.X && snakeHead.Y == blueSpecialFood.Y)
                {
                    blueSpecialFood = new Position(-1, -1); // Удаляем синий X после сбора
                    isBlueSpecialFoodActive = false; // Разрешаем появление следующего красного X
                    score += 4; // Добавляем 4 очка
                }
                else
                {
                    snakeBody.RemoveAt(snakeBody.Count - 1);
                }
                // Увеличение скорости каждые 60 очков
                if (score > 0 && score % 60 == 0)
                {
                    speed = Math.Max(50, speed - 20); // Увеличиваем скорость, уменьшая интервал задержки
                }


                Thread.Sleep(speed);
            }

            Console.SetCursorPosition(0, Height + 2);
            Console.WriteLine("Game Over! Score: " + score);
            Console.ReadKey();
        }

        static void SetConsoleWindowSize(int columns, int lines)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-Command \"$host.UI.RawUI.BufferSize = New-Object Management.Automation.Host.Size({columns}, {lines}); $host.UI.RawUI.WindowSize = New-Object Management.Automation.Host.Size({columns}, {lines})\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
        }

        static void DrawStartMessage()
        {
            string message = "Press any arrow key to start...";
            int boxWidth = message.Length + 4;
            int boxHeight = 3;

            int startX = (Width / 2) - (boxWidth / 2);
            int startY = (Height / 2) - (boxHeight / 2);

            Console.SetCursorPosition(startX, startY);
            Console.Write("╔" + new string('═', boxWidth - 2) + "╗");
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("║ " + message + " ║");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("╚" + new string('═', boxWidth - 2) + "╝");
        }

        static Direction WaitForStartInput()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            return Direction.Up;
                        case ConsoleKey.DownArrow:
                            return Direction.Down;
                        case ConsoleKey.LeftArrow:
                            return Direction.Left;
                        case ConsoleKey.RightArrow:
                            return Direction.Right;
                    }
                }
            }
        }

        static Position GenerateFoodPosition(List<Position> snakeBody)
        {
            Random random = new Random();
            Position food;

            do
            {
                // Генерируем случайную позицию еды, не выходящую за границы поля
                food = new Position(random.Next(1, Width - 1), random.Next(1, Height - 1));
            }
            // Повторяем генерацию, если еда появляется на теле змейки
            while (snakeBody.Exists(pos => pos.X == food.X && pos.Y == food.Y));

            return food;
        }


        static void GenerateSpecialFood(int score)
        {
            // Проверка появления синего X
            if (score >= nextBlueSpecialFoodScore)
            {
                Random random = new Random();
                blueSpecialFood = new Position(random.Next(Width), random.Next(Height));
                isBlueSpecialFoodActive = true;
                nextBlueSpecialFoodScore += 30; // Устанавливаем порог для следующего синего X

                // Убираем красный X, пока активен синий X
                redSpecialFood = new Position(-1, -1);
            }
            else if (!isBlueSpecialFoodActive && score >= lastRedSpecialFoodScore + 10)
            {
                Random random = new Random();
                redSpecialFood = new Position(random.Next(Width), random.Next(Height));
                lastRedSpecialFoodScore += 10; // Устанавливаем порог для следующего красного X
            }
        }

        static void DrawField(Position snakeHead, List<Position> snakeBody, Position food)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔" + new string('═', Width) + "╗");
            for (int y = 0; y < Height; y++)
            {
                Console.Write("║");
                for (int x = 0; x < Width; x++)
                {
                    if (x == snakeHead.X && y == snakeHead.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("O");
                        Console.ResetColor();
                    }
                    else if (snakeBody.Exists(pos => pos.X == x && pos.Y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("o");
                        Console.ResetColor();
                    }
                    else if (x == food.X && y == food.Y)
                    {
                        Console.Write("X");
                    }
                    else if (x == redSpecialFood.X && y == redSpecialFood.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                    }
                    else if (x == blueSpecialFood.X && y == blueSpecialFood.Y)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("X");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("║");
            }
            Console.WriteLine("╚" + new string('═', Width) + "╝");
        }

        static void DrawScore(int score)
        {
            Console.SetCursorPosition(0, Height + 1);
            Console.WriteLine($"Score: {score}");
        }

        static Direction HandleInput(Direction currentDirection, ref int speed)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentDirection != Direction.Down)
                            return Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentDirection != Direction.Up)
                            return Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentDirection != Direction.Right)
                            return Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentDirection != Direction.Left)
                            return Direction.Right;
                        break;
                    case ConsoleKey.Q:
                        speed -= 20;
                        if (speed < 0)
                            speed = 0;
                        break;
                }
            }
            return currentDirection;
        }
        static Position MoveSnakeHead(Position head, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Position(head.X, head.Y - 1);
                case Direction.Down:
                    return new Position(head.X, head.Y + 1);
                case Direction.Left:
                    return new Position(head.X - 1, head.Y);
                case Direction.Right:
                    return new Position(head.X + 1, head.Y);
                default:
                    return head;
            }
        }

        static bool CheckCollisions(Position head, List<Position> body)
        {
            // Проверка столкновения с границами поля
            if (head.X < 0 || head.X >= Width || head.Y < 0 || head.Y >= Height)
                return true;

            // Проверка столкновения с самой собой
            foreach (var segment in body)
            {
                if (head.X == segment.X && head.Y == segment.Y)
                    return true;
            }

            return false;
        }
    }
}
// Изменения для iss58
// Изменения для iss58

using Raylib_cs;
using System.Numerics;

float timer = 0;
float bestScore = 0;
int powerupState = 1; // 0 = inte spawnad, 1 = spawnad, 2 = upplockad
int pointstate = 1;

Raylib.InitWindow(800, 600, "Mitt fina fönster");

Texture2D sprite = Raylib.LoadTexture("hero2.png");
Texture2D powerupTexture = Raylib.LoadTexture("Speed.boost.png");
Texture2D point = Raylib.LoadTexture("Point.png");
Raylib.SetTargetFPS(60);


Random generator = new Random();
int powerupX = generator.Next(700);
int powerupY = generator.Next(500);
int pointX = generator.Next(700);
int pointY = generator.Next(500);


Vector2 movement = new Vector2(10, 10);

Vector2 enemyMovement = new Vector2(0, 0);

Vector2 enemyPosition = new Vector2(0, 0);

string scene = "start";
string gamemode = "";

int lastIncrease = 0;

int points = 0;

float enemySpeed = 4;
float poweruptimer = 10;

Rectangle powerupHitbox = new Rectangle(powerupX, powerupY, powerupTexture.width, powerupTexture.height);

Rectangle pointHitbox = new Rectangle(0, 0, point.width, point.height);

Rectangle hitBox = new Rectangle(200, 200, sprite.width, sprite.height);

while (Raylib.WindowShouldClose() == false)
{
    if (scene == "game")
    {
        if (gamemode == "pointgrab")
        {
            timer -= Raylib.GetFrameTime();
        }
        else if (gamemode == "survival")
        {
            timer += Raylib.GetFrameTime();
        }

        poweruptimer -= Raylib.GetFrameTime();

        if (poweruptimer < 0 && powerupState == 0)
        {
            powerupState = 1;
        }

        movement = Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
        {
            movement.Y = 10;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
        {
            movement.Y = -10;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            movement.X = -10;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            movement.X = 10;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
        {
            movement.Y = 10;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
        {
            movement.Y = -10;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            movement.X = -10;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            movement.X = 10;
        }

        if (powerupState == 2)
        {
            movement *= 3f;
        }


        int seconds = (int)timer;
        if (seconds % 10 == 0 && lastIncrease != seconds)
        {
            lastIncrease = seconds;

            if (gamemode == "survival")
            {
                enemySpeed += 2;
            }
            else if (gamemode == "pointgrab")
            {
                enemySpeed += 1;
            }
        }


        Vector2 pVector = new Vector2(hitBox.x, hitBox.y);
        Vector2 diff = pVector - enemyPosition;
        enemyMovement = Vector2.Normalize(diff) * enemySpeed;

        hitBox.x += movement.X;
        hitBox.y += movement.Y;

        enemyPosition += enemyMovement;

        if (hitBox.x < 0)
        {
            hitBox.x = 0;
        }
        if (hitBox.x > Raylib.GetScreenWidth() - hitBox.width)
        {
            hitBox.x = Raylib.GetScreenWidth() - hitBox.width;
        }
        if (hitBox.y < 0)
        {
            hitBox.y = 0;
        }
        if (hitBox.y > Raylib.GetScreenHeight() - hitBox.height)
        {
            hitBox.y = Raylib.GetScreenHeight() - hitBox.height;
        }

        if (Raylib.CheckCollisionRecs(hitBox, powerupHitbox) && powerupState == 1 && gamemode == "survival")
        {
            powerupState = 2;
        }
        if (Raylib.CheckCollisionRecs(hitBox, pointHitbox))
        {
            pointHitbox.x = generator.Next(700);
            pointHitbox.y = generator.Next(500);
            points++;
        }


        if (Raylib.CheckCollisionCircleRec(enemyPosition, 30, hitBox))
        {
            scene = "gameover";
            if (timer > bestScore)
            {
                bestScore = timer;
            }
        }
    }
    else if (scene == "start")
    {

        if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
        {
            scene = "game";
            hitBox.x = 200;
            hitBox.y = 200;
            enemyPosition = new Vector2(0, 0);
            gamemode = "survival";

        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            enemyPosition = new Vector2(0, 0);
            pointstate = 0;
            pointHitbox.x = generator.Next(700);
            pointHitbox.y = generator.Next(500);
            lastIncrease = 0;
            powerupState = 0;
            poweruptimer = 0;
            gamemode = "pointgrab";
            timer = 60;
        }

    }
    else if (scene == "gameover")
    {

        if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
        {
            scene = "game";
            hitBox.x = 200;
            hitBox.y = 200;
            timer = 0;
            poweruptimer = 0;
            enemyPosition = new Vector2(0, 0);
            enemySpeed = 4;
            powerupHitbox.x = generator.Next(700);
            powerupHitbox.y = generator.Next(500);
            powerupState = 0;
            lastIncrease = 0;
            gamemode = "survival";
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            hitBox.x = 200;
            hitBox.y = 200;
            timer = 60;
            enemyPosition = new Vector2(0, 0);
            enemySpeed = 4;
            pointstate = 0;
            powerupState = 0;
            points = 0;
            poweruptimer = 0;
            pointHitbox.x = generator.Next(700);
            pointHitbox.y = generator.Next(500);
            lastIncrease = 0;
            gamemode = "pointgrab";

        }

    }





    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.WHITE);

    if (scene == "game")
    {

        Raylib.DrawCircleV(enemyPosition, 30, Color.BLACK);

        Raylib.DrawTexture(sprite, (int)hitBox.x, (int)hitBox.y, Color.WHITE);

        Raylib.DrawText(timer.ToString("0"), 10, 10, 32, Color.BLACK);



        if (gamemode == "survival" && powerupState == 1)
        {
            Raylib.DrawTexture(powerupTexture, (int)powerupHitbox.x, (int)powerupHitbox.y, Color.WHITE);
        }

        if (gamemode == "pointgrab")
        {
            Raylib.DrawTexture(point, (int)pointHitbox.x, (int)pointHitbox.y, Color.WHITE);
            Raylib.DrawText("Score:" + points, 725, 10, 16, Color.BLACK);
        }


    }
    else if (scene == "start")
    {

        Raylib.DrawText("PRESS ENTER TO PLAY (Survive)", 150, 300, 32, Color.BLACK);
        Raylib.DrawText("(How To Play)", 20, 20, 25, Color.BLACK);
        Raylib.DrawText("Run Away From The Ball!", 20, 50, 20, Color.BLACK);
        Raylib.DrawText("PRESS SPACE TO PLAY (Point Grab)", 150, 350, 32, Color.BLACK);
    }
    else if (scene == "gameover")
    {
        Raylib.DrawText("GAME OVER", 20, 20, 32, Color.BLACK);

        if (gamemode == "survival")
        {
            Raylib.DrawText("Press Enter To Play Again", 20, 60, 32, Color.BLACK);
            Raylib.DrawText(timer.ToString("0"), 400, 300, 32, Color.BLACK);
            Raylib.DrawText(bestScore.ToString("0"), 400, 250, 32, Color.BLACK);
            Raylib.DrawText("PRESS SPACE TO PLAY (Point Grab)", 420, 20, 20, Color.RED);
        }
        else if (gamemode == "pointgrab")
        {
            Raylib.DrawText("GAME OVER", 20, 20, 32, Color.BLACK);
            Raylib.DrawText("Press Space To Play Again", 20, 60, 32, Color.BLACK);
            Raylib.DrawText("PRESS ENTER TO PLAY (Survival)", 420, 20, 20, Color.RED);
            Raylib.DrawText(points.ToString("0"), 400, 300, 32, Color.BLACK);
        }



    }


    Raylib.EndDrawing();
}






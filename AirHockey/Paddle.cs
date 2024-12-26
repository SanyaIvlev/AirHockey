using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Paddle
{
    public RectangleShape Shape; 
    
    private float _direction;
    
    private Keyboard.Key _upMovementButton; 
    private Keyboard.Key _downMovementButton; 
    
    public Paddle(Keyboard.Key upMovement, Keyboard.Key downMovement, bool isRight)
    {
        _upMovementButton = upMovement;
        _downMovementButton = downMovement;

        Shape = new(new Vector2f(30, 100));
        Shape.FillColor = Color.Red;

        int x;
        
        if (isRight)
            x = Game.WIDTH - 200; 
        else
            x = 200;
        
        Shape.Position = new Vector2f(x, Game.HEIGHT / 2f);
    }
    
    public void ProcessInput()
    {
        _direction = 0;
        
        if (Keyboard.IsKeyPressed(_upMovementButton))
        {
            _direction = -1;
        }
        else if (Keyboard.IsKeyPressed(_downMovementButton))
        {
            _direction = 1;
        }
    }

    public void DoLogic()
    {
        Vector2f movementDistance = new Vector2f(0, _direction);

        float halfOfPaddle = Shape.Size.Y / 2;
        float nextCenterPositionY = Shape.Position.Y + movementDistance.Y + halfOfPaddle;

        if (nextCenterPositionY + halfOfPaddle >= Game.HEIGHT || nextCenterPositionY - halfOfPaddle <= 0)
            return;
        
        Shape.Position += movementDistance;
    }
}
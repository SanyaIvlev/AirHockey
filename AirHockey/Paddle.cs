using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Paddle
{
    public RectangleShape Shape; 
    
    private float _direction;
    
    private Keyboard.Key _leftMovementButton; 
    private Keyboard.Key _rightMovementButton; 
    
    public Paddle(Keyboard.Key leftMovement, Keyboard.Key rightMovement, bool isOnTop)
    {
        _leftMovementButton = leftMovement;
        _rightMovementButton = rightMovement;

        Shape = new(new Vector2f(100, 30));
        Shape.FillColor = Color.Red;

        int y;
        
        if (isOnTop)
            y = 200; 
        else
            y = Game.HEIGHT - 200;
        
        Shape.Position = new Vector2f(Game.WIDTH / 2, y);
    }
    
    public void ProcessInput()
    {
        _direction = 0;
        
        if (Keyboard.IsKeyPressed(_leftMovementButton))
        {
            _direction = -1;
        }
        else if (Keyboard.IsKeyPressed(_rightMovementButton))
        {
            _direction = 1;
        }
    }

    public void DoLogic()
    {
        Vector2f movementDistance = new Vector2f(_direction, 0);

        float halfOfPaddle = Shape.Size.X / 2;
        float nextCenterPositionX = Shape.Position.X + movementDistance.X + halfOfPaddle;

        if (nextCenterPositionX + halfOfPaddle >= Game.WIDTH || nextCenterPositionX - halfOfPaddle <= 0)
            return;
        
        Shape.Position += movementDistance;
    }
}
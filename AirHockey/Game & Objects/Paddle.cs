using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Paddle
{
    public RectangleShape Shape { get; private set; }

    private float _direction;
    
    private Keyboard.Key _upMovementButton; 
    private Keyboard.Key _downMovementButton;

    private Vector2u _windowSize;
    
    public Paddle(Keyboard.Key upMovement, Keyboard.Key downMovement, Color fillColor, bool isRight, Vector2u windowSize)
    {
        _upMovementButton = upMovement;
        _downMovementButton = downMovement;
        
        _windowSize = windowSize;

        Shape = new(new Vector2f(5, 100));
        Shape.FillColor = fillColor;

        int x;
        
        if (isRight)
            x = (int)_windowSize.X - 200; 
        else
            x = 200;
        
        Shape.Position = new Vector2f(x, (int)_windowSize.Y / 2f);
    }

    public void Reset()
    {
        Shape.Position = new Vector2f(Shape.Position.X, (int)_windowSize.Y / 2f);
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

        float halfOfPaddleY = Shape.Size.Y / 2;
        float nextCenterPositionY = Shape.Position.Y + movementDistance.Y + halfOfPaddleY;

        float DownPosition = nextCenterPositionY + halfOfPaddleY;
        float UpPosition = nextCenterPositionY - halfOfPaddleY;

        if (DownPosition >= _windowSize.Y || UpPosition <= 0)
            return;
        
        Shape.Position += movementDistance;
    }

    public FloatRect GetGlobalBounds()
        => Shape.GetGlobalBounds();
        
}
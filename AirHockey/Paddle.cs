using System.Diagnostics;
using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Paddle
{
    public Vector2f DownRightPosition { get; private set; }
    public Vector2f UpLeftPosition { get; private set; }

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
        
        float halfOfPaddleX = Shape.Size.X / 2;
        float centerPositionX = Shape.Position.X + halfOfPaddleX;
        
        DownRightPosition = new Vector2f(centerPositionX + halfOfPaddleX, nextCenterPositionY + halfOfPaddleY);
        UpLeftPosition = new Vector2f(centerPositionX - halfOfPaddleX, nextCenterPositionY - halfOfPaddleY);

        if (DownRightPosition.Y >= _windowSize.Y || UpLeftPosition.Y <= 0)
            return;
        
        Shape.Position += movementDistance;

    }
}
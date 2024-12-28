using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Player
{
    public uint Score { get; private set; }

    public RectangleShape Paddle;
    
    private PaddleController _controller;
    
    private Vector2f _defaultPosition;

    public Player(Keyboard.Key upMovement, Keyboard.Key downMovement, Color fillColor, bool isRight, Vector2u windowSize)
    {
        int x;
        
        if (isRight)
            x = (int)windowSize.X - 200; 
        else
            x = 200;
        
        Paddle = new (new Vector2f(5, 100))
        {
            FillColor = fillColor,
            Position = new Vector2f(x, (int)windowSize.Y / 2f),
        };
        
        _defaultPosition = Paddle.Position;
        
        _controller = new(upMovement, downMovement, windowSize, _defaultPosition, Paddle.Size);
    }
    
    public void Reset()
    {
        Paddle.Position = _defaultPosition;
        _controller.Reset();
    }

    public void ProcessInput()
    {
        _controller.ProcessInput();
    }

    public void DoLogic()
    {
        _controller.DoLogic();
        Paddle.Position = _controller.Position;
    }
    
    public FloatRect GetGlobalBounds()
        => Paddle.GetGlobalBounds();

    public void UpdateScore()
    {
        Score++;
    }
    
    

}
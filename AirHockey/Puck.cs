using SFML.Graphics;
using SFML.System;

namespace Aerohockey;

public class Puck
{
    public float LeftPosition => Ball.Position.X;
    public float RightPosition => Ball.Position.X + Ball.Radius * 2;
    
    public CircleShape Ball;

    private Paddle _rightPaddle;
    
    private Paddle _leftPaddle;

    private float _speedBoost;
    
    private Vector2f _direction;

    private Clock _clock;

    private Vector2u _windowSize;

    public Puck(Paddle rightPaddle, Paddle leftPaddle, Vector2u windowSize)
    {
        Ball = new CircleShape(10);
        _clock = new(); 
        
        _windowSize = windowSize;
        
        Ball.Position = new Vector2f(_windowSize.X / 2f, _windowSize.Y / 2f);
        
        _rightPaddle = rightPaddle;
        _leftPaddle = leftPaddle;
        
        _speedBoost = 500f;
        
        _clock.Restart();
        
        _direction = GetRandomDirection();
    }

    public void DoLogic()
    {
        TryBounce();
        
        float deltaTime = _clock.ElapsedTime.AsSeconds();
        
        Vector2f currentDirection = _direction * _speedBoost * deltaTime;
        
        Ball.Position += currentDirection;
        _clock.Restart();
    }

    private void TryBounce()
    {
        float radius = Ball.Radius;
        float shapeCenterY = Ball.Position.Y + radius;

        float upperPuckBorder = shapeCenterY - radius;
        float lowerPuckBorder = shapeCenterY + radius;
        
        if (upperPuckBorder <= 0 || lowerPuckBorder >= _windowSize.Y)
        {
            _direction = new Vector2f(_direction.X, -_direction.Y);
            _speedBoost += 1f;
        }
        
        float shapeCenterX = Ball.Position.X + radius;
        
        float leftPuckBorder = shapeCenterX - radius;
        float rightPuckBorder = shapeCenterX + radius;

        if ((upperPuckBorder >= _rightPaddle.UpLeftPosition.Y && lowerPuckBorder <= _rightPaddle.DownRightPosition.Y && rightPuckBorder >= _rightPaddle.UpLeftPosition.X)
            || (upperPuckBorder >= _leftPaddle.UpLeftPosition.Y && lowerPuckBorder <= _leftPaddle.DownRightPosition.Y && leftPuckBorder <= _leftPaddle.DownRightPosition.X))
        {
            _direction = new Vector2f(-_direction.X, _direction.Y);
            _speedBoost += 1f;
        }
    }

    private Vector2f GetRandomDirection()
    {
        var random = new Random();
        
        float x = (float)random.NextDouble();
        float y = (float)random.NextDouble();
        
        x = GetRandomState() ? x : -x;
        y = GetRandomState() ? y : -y;
        
        return new Vector2f(x, y);
    }

    private bool GetRandomState()
    {
        var random = new Random();
        
        return random.Next(0, 2) == 0;
    }
}
using SFML.Graphics;
using SFML.System;

namespace Aerohockey;

public class Puck
{
    public float LeftPosition => Figure.Position.X;
    public float RightPosition => Figure.Position.X + Figure.Radius * 2;
    
    public CircleShape Figure;

    private Paddle _rightPaddle;
    private Paddle _leftPaddle;

    private float _speedBoost;
    private float _boostPerOneBounce;
    
    private Vector2f _direction;

    private Vector2u _windowSize;
    
    private Clock _clock;
    private Random _random;


    public Puck(Paddle rightPaddle, Paddle leftPaddle, Vector2u windowSize)
    {
        Figure = new (10);
        _clock = new(); 
        
        _windowSize = windowSize;
        
        Figure.Position = new Vector2f(_windowSize.X / 2f, _windowSize.Y / 2f);
        
        _rightPaddle = rightPaddle;
        _leftPaddle = leftPaddle;
        
        _speedBoost = 500f;
        _boostPerOneBounce = 10.0f;
        
        _clock.Restart();

        _random = new();
        
        _direction = GetRandomDirection();
    }
    
    public void Reset()
    {
        Figure.Position = new Vector2f(_windowSize.X / 2f, _windowSize.Y / 2f);
        _direction = GetRandomDirection();
        _speedBoost = 500f;
    }

    public void DoLogic()
    {
        TryBounce();
        
        float deltaTime = _clock.ElapsedTime.AsSeconds();
        
        Vector2f currentDirection = _direction * _speedBoost * deltaTime;
        
        Figure.Position += currentDirection;
        _clock.Restart();
    }

    private void TryBounce()
    {
        float radius = Figure.Radius;
        float shapeCenterY = Figure.Position.Y + radius;

        float upperPuckBorder = shapeCenterY - radius;
        float lowerPuckBorder = shapeCenterY + radius;
        
        if ((upperPuckBorder <= 0 && _direction.Y < 0)
            || (lowerPuckBorder >= _windowSize.Y && _direction.Y > 0))
        {
            _direction = new Vector2f(_direction.X, -_direction.Y);
            _speedBoost += _boostPerOneBounce;
        }
        
        FloatRect boundsOfPuck = Figure.GetGlobalBounds();
        FloatRect boundsOfRightPaddle = _rightPaddle.GetGlobalBounds();
        FloatRect boundsOfLeftPaddle = _leftPaddle.GetGlobalBounds();
        
        
        if ((boundsOfPuck.Intersects(boundsOfRightPaddle) && _direction.X > 0)
            || (boundsOfPuck.Intersects(boundsOfLeftPaddle) && _direction.X < 0))
        {
            _direction = new Vector2f(-_direction.X, _direction.Y);
            _speedBoost += _boostPerOneBounce;
        }


    }

    private Vector2f GetRandomDirection()
    {
        float x = (float)_random.NextDouble();
        float y = (float)_random.NextDouble();
        
        x = GetRandomState() ? x : -x;
        y = GetRandomState() ? y : -y;
        
        return new Vector2f(x, y);
    }

    private bool GetRandomState()
        => _random.Next(0, 2) == 0;
}
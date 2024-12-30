using AirHockey.Extensions;
using SFML.Graphics;
using SFML.System;

namespace Aerohockey;

public class Puck
{
    
    public float Radius => Figure.Radius;
    public float LeftBorderDistanceX => Figure.Position.X + Radius;
    public float RightBorderDistanceX => _windowSize.X - (Figure.Position.X + 2 * Radius);
    
    public CircleShape Figure;

    private Player _rightPaddle;
    private Player _leftPaddle;

    private float _defaultSpeedBoost;
    private float _currentSpeedBoost;
    private float _boostPerOneBounce;
    
    private Vector2f _direction;

    private Vector2u _windowSize;
    
    private Clock _clock;
    private Random _random;


    public Puck(Player rightPaddle, Player leftPaddle, Vector2u windowSize)
    {
        Figure = new (10);
        _clock = new(); 
        
        _windowSize = windowSize;
        
        Figure.Position = new Vector2f(_windowSize.X / 2f, _windowSize.Y / 2f);
        
        _rightPaddle = rightPaddle;
        _leftPaddle = leftPaddle;

        _defaultSpeedBoost = 500f;
        _currentSpeedBoost = _defaultSpeedBoost;
        _boostPerOneBounce = 10.0f;
        
        _clock.Restart();

        _random = new();
        
        _direction = GetRandomDirection();
    }
    
    public void Reset()
    {
        Figure.Position = new Vector2f(_windowSize.X / 2f, _windowSize.Y / 2f);
        _direction = GetRandomDirection();
        _currentSpeedBoost = _defaultSpeedBoost;
    }

    public void DoLogic()
    {
        TryBounce();
        
        float deltaTime = _clock.ElapsedTime.AsSeconds();
        
        Vector2f currentDirection = _direction * _currentSpeedBoost * deltaTime;
        
        Figure.Position += currentDirection;
        _clock.Restart();
    }

    private void TryBounce()
    {
        float radius = Figure.Radius;
        float centerY = Figure.Position.Y + radius;
        
        float UpBorderDistanceY = centerY;
        float DownBorderDistanceY = _windowSize.Y - centerY;
        
        if ((radius >= UpBorderDistanceY && _direction.Y < 0)
            || (radius >= DownBorderDistanceY && _direction.Y > 0))
        {
            _direction = new Vector2f(_direction.X, -_direction.Y);
            _currentSpeedBoost += _boostPerOneBounce;
        }
        
        FloatRect boundsOfPuck = Figure.GetGlobalBounds();
        FloatRect boundsOfRightPaddle = _rightPaddle.GetGlobalBounds();
        FloatRect boundsOfLeftPaddle = _leftPaddle.GetGlobalBounds();
        
        
        if ((boundsOfPuck.Intersects(boundsOfRightPaddle) && _direction.X > 0)
            || (boundsOfPuck.Intersects(boundsOfLeftPaddle) && _direction.X < 0))
        {
            _direction = new Vector2f(-_direction.X, _direction.Y);
            _currentSpeedBoost += _boostPerOneBounce;
        }
    }

    private Vector2f GetRandomDirection()
    {
        float x = (float)_random.NextDouble();
        float y = (float)_random.NextDouble();
        
        x = GetRandomState() ? x : -x;
        y = GetRandomState() ? y : -y;
        
        Vector2f direction = new(x, y);
        var normalizedDirection = direction.Normalize();
        
        return normalizedDirection;
    }

    private bool GetRandomState()
        => _random.Next(0, 2) == 0;
}
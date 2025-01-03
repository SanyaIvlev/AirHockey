using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class PaddleController
{
    public Vector2f Position { get; private set; }
    private Vector2f _defaultPosition;

    private Vector2f _size;
    
    private float _direction;
    
    private Keyboard.Key _upMovementButton; 
    private Keyboard.Key _downMovementButton;

    private Vector2u _windowSize;

    private Clock _clock;
    
    public PaddleController(Keyboard.Key upMovement, Keyboard.Key downMovement, Vector2u windowSize, Vector2f defaultPosition, Vector2f paddleSize)
    {
        _defaultPosition = defaultPosition;
        Position = _defaultPosition;
        
        _upMovementButton = upMovement;
        _downMovementButton = downMovement;
        
        _size = paddleSize;
        
        _windowSize = windowSize;
        
        _clock = new Clock();
    }

    public void Reset()
    {
        Position = _defaultPosition;
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

    public void TryMove()
    {
        float deltaTime = _clock.ElapsedTime.AsMilliseconds();
        
        Vector2f movementDistance = new Vector2f(0, _direction * deltaTime);

        float halfOfPaddleY = _size.Y / 2;
        float nextCenterPositionY = Position.Y + movementDistance.Y + halfOfPaddleY;

        float DownPosition = nextCenterPositionY + halfOfPaddleY;
        float UpPosition = nextCenterPositionY - halfOfPaddleY;

        if (DownPosition >= _windowSize.Y || UpPosition <= 0)
            return;
        
        Position += movementDistance;
        
        _clock.Restart();
    }
        
}
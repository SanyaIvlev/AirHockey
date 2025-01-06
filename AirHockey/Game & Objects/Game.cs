using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Game
{
    private const int TARGET_FPS = 60;
    private const int SECOND_TO_A_MICROSECOND = 1000000;
    private const long TIME_BEFORE_NEXT_FRAME = SECOND_TO_A_MICROSECOND / TARGET_FPS;
    
    private const int WIDTH = 1600;
    private const int HEIGHT = 900;
    
    private RenderWindow _window;
    
    private Player _leftPaddle;
    private Player _rightPaddle;
    
    private Puck _puck;
    
    private Text _victoryText;
    
    private List<Drawable> _drawables;

    public void Start()
    {
        SetWindow();
        
        _drawables = new();

        SetText();

        CreateGameObjects();

        Run();
    }

    private void SetWindow()
    {
        _window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Aerohockey");
        _window.Closed += WindowClosed;
    }

    private void SetText()
    {
        string fontName = "Obelix Pro.ttf";
        Font font = new (GetFontLocation(fontName));
        
        _victoryText = new("", font)
        {
            CharacterSize = 25,
            FillColor = Color.Yellow,
            Position = new Vector2f(0,0),
        };
        
        _victoryText.DisplayedString = 0 + " : " + 0;
        _victoryText.Position = new Vector2f(WIDTH / 2f - _victoryText.Scale.X / 2f, 0 + _victoryText.Scale.Y);
        
        AddDrawables(_victoryText);
    }
    
    private string GetFontLocation(string fontName)
    {
        return Path.GetFullPath("..\\..\\..\\..\\Resources\\Fonts\\" + fontName);
    }

    private void CreateGameObjects()
    {
        _leftPaddle = new(Keyboard.Key.W, Keyboard.Key.S, Color.Blue,  false, _window.Size);
        _rightPaddle = new(Keyboard.Key.Up, Keyboard.Key.Down, Color.Red, true, _window.Size);
        
        _puck = new Puck(_rightPaddle, _leftPaddle, _window.Size);
        
        AddDrawables(_leftPaddle.Paddle, _rightPaddle.Paddle, _puck.Figure);
    }

    private void AddDrawables(params Drawable[] newDrawables)
    {
        foreach (var drawable in newDrawables)
        {
            if (_drawables.Contains(drawable))
                return;
            
            _drawables.Add(drawable);
        }
    }

    private void Run()
    {
        long totalTimeBeforeUpdate = 0;
        long previousTimeElapsed = 0;
        long deltaTime = 0;
        long totalTimeElapsed = 0;
        
        Clock clock = new Clock();
        
        while (DoesGameContinue())
        {
            ProcessInput();

            totalTimeElapsed = clock.ElapsedTime.AsMicroseconds();
            deltaTime = totalTimeElapsed - previousTimeElapsed;
            previousTimeElapsed = totalTimeElapsed;
            
            totalTimeBeforeUpdate += deltaTime;
            

            if (totalTimeBeforeUpdate >= TIME_BEFORE_NEXT_FRAME)
            {
                clock.Restart();
                
                Update();
                Render();
            }
        }
    }

    private bool DoesGameContinue()
        => _window.IsOpen;
    
    private void ProcessInput()
    {
        _window.DispatchEvents();
        
        PaddleController leftPaddleController = _leftPaddle.Controller;
        PaddleController rightPaddleController = _rightPaddle.Controller;
        
        leftPaddleController.ProcessInput();
        rightPaddleController.ProcessInput();
    }
    
    private void Update()
    {
        _rightPaddle.DoLogic();
        _leftPaddle.DoLogic();
        
        _puck.DoLogic();
        
        CheckWinner();
    }

    private void CheckWinner()
    {
        if (_puck.Radius >= _puck.LeftBorderDistanceX)
        {
            UpdateScore(_rightPaddle);
            ResetGameObjects();
        }
        else if (_puck.Radius >= _puck.RightBorderDistanceX)
        {
            UpdateScore(_leftPaddle);
            ResetGameObjects();
        }
    }

    private void UpdateScore(Player player)
    {
        player.UpdateScore();
        
        _victoryText.DisplayedString = _leftPaddle.Score + " : " + _rightPaddle.Score;
        _victoryText.Position = new Vector2f(WIDTH / 2f - _victoryText.Scale.X / 2f, 0 + _victoryText.Scale.Y);
    }

    private void ResetGameObjects()
    {
        _leftPaddle.Reset();
        _rightPaddle.Reset();
        _puck.Reset();
    }
    
    private void Render()
    {
        _window.Clear(Color.Black);

        foreach (var drawable in _drawables)
        {
            _window.Draw(drawable);
        }
        
        _window.Display();
    }
    
    private void WindowClosed(object? sender, EventArgs e)
    {
        RenderWindow window = (RenderWindow)sender;
        window.Close();
    }

    ~Game()
    {
        _window.Closed -= WindowClosed;
    }

}
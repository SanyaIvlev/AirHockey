using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aerohockey;

public class Game
{
    private const int WIDTH = 1600;
    private const int HEIGHT = 900;
    
    private RenderWindow _window;
    
    private Text _victoryText;

    private Player _leftPaddle;
    private Player _rightPaddle;
    
    private Puck _puck;

    public void Start()
    {
        SetWindow();

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
    }

    private void Run()
    {
        while (DoesGameContinue())
        {
            ProcessInput();
            Update();
            Render();
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
            _rightPaddle.UpdateScore();
            ResetGameObjects();
        }
        else if (_puck.Radius >= _puck.RightBorderDistanceX)
        {
            _leftPaddle.UpdateScore();
            ResetGameObjects();
        }

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
        
        _window.Draw(_victoryText);

        Shape firstPaddle = _leftPaddle.Paddle;
        Shape secondPaddle = _rightPaddle.Paddle;
        Shape puck = _puck.Figure;
        
        _window.Draw(firstPaddle);
        _window.Draw(secondPaddle);
        _window.Draw(puck);
        
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
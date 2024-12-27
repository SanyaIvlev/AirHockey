using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace Aerohockey;

public class Game
{
    public const int WIDTH = 1600;
    public const int HEIGHT = 900;
    
    private RenderWindow _window;

    private Paddle _paddle1;
    private Paddle _paddle2;
    
    private Puck _puck;
    
    
    public void Start()
    {
        _window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Aerohockey");
        _window.Closed += WindowClosed;
        
        _paddle1 = new(Keyboard.Key.Up, Keyboard.Key.Down, Color.Blue,  false);
        _paddle2 = new(Keyboard.Key.W, Keyboard.Key.S, Color.Red, true);
        
        _puck = new Puck(_paddle2, _paddle1);
        
        Run();
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
        
        _paddle1.ProcessInput();
        _paddle2.ProcessInput();
    }
    
    private void Update()
    {
        _paddle1.DoLogic();
        _paddle2.DoLogic();
        
        _puck.DoLogic();
    }

    private void Render()
    {
        _window.Clear(Color.Black);

        Shape firstPaddle = _paddle1.Shape;
        Shape secondPaddle = _paddle2.Shape;
        Shape puck = _puck.Ball;
        
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
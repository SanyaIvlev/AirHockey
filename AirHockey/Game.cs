using SFML.Graphics;
using SFML.Window;

namespace Aerohockey;

public class Game
{
    public const int WIDTH = 900;
    public const int HEIGHT = 1600;
    
    private RenderWindow _window;

    private Paddle _paddle1;
    private Paddle _paddle2;
    
    
    public void Start()
    {
        _window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Aerohockey");
        _window.Closed += WindowClosed;
        
        _paddle1 = new(Keyboard.Key.A, Keyboard.Key.D, false);
        _paddle2 = new(Keyboard.Key.W, Keyboard.Key.S, true);
        
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
    }

    private void Render()
    {
        _window.Clear(Color.Black);

        Shape firstPaddle = _paddle1.Shape;
        Shape secondPaddle = _paddle2.Shape;
        
        _window.Draw(firstPaddle);
        _window.Draw(secondPaddle);
        
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
using SFML.Graphics;
using SFML.Window;

namespace Aerohockey;

public class Game
{
    private const int WIDTH = 1600;
    private const int HEIGHT = 900;
    private RenderWindow _window;
    
    public void Start()
    {
        _window = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Aerohockey");
        _window.Closed += WindowClosed;
        
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
    }
    
    private void Update()
    {
        
    }

    private void Render()
    {
        _window.Clear(Color.Black);
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
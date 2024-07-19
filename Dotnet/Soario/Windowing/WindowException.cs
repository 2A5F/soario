namespace Soario.Windowing;

public class WindowException : Exception
{
    public WindowException() { }
    public WindowException(string message) : base(message) { }
    public WindowException(string message, Exception inner) : base(message, inner) { }
}

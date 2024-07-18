namespace Soario.Windowing;

public class WindowException : Exception
{
    public WindowException(string msg) : base(msg) { }
    public WindowException(string msg, Exception? innerException) : base(msg, innerException) { }
}

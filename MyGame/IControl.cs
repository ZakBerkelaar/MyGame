namespace MyGame
{
    public interface IControl
    {
        IDString IDString { get; }
        bool IsDown { get; }
        bool IsDownFrame { get; }
        bool IsUpFrame { get; }
    }
}
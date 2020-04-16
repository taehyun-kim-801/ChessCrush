namespace ChessCrush.Game
{
    public interface ITransmittable
    {
        void Write(OutputMemoryStream outputMemoryStream);
        void Read(InputMemoryStream inputMemoryStream);
    }
}

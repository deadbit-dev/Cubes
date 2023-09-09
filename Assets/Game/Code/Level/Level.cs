namespace JoyTeam.Game
{
    public class Level
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int[] Data;

        public Level(int width, int height, int[] data)
        {
            Width = width;
            Height = height;
            Data = data;
        }
    }
}
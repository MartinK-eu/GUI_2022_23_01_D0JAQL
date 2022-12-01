namespace RayCaster.Models
{
    public interface IGameModel
    {
        int[,] MapMatrix { get; set; }
        Character Player { get; set; }
        bool InMapMode { get; set; }

    }
}
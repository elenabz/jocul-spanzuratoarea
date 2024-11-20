using JoculSpanzuratoarea.Models;

namespace JoculSpanzuratoarea.Interfaces
{
    public interface IGameLogic
    {
        void StartGame();
        GameState ComputeGameState(string playLetter, int maxGuesses);
    }
}

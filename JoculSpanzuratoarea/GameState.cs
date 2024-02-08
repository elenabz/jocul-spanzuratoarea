namespace JoculSpanzuratoarea.Pages
{
    public class GameState
    {
        public bool GuessedLetter { get; set; } = false;
        public int GuessedFullWord { get; set; } = 0;
        public string MaskedWordToGuess { get; set; } = "";
        public int FailCount { get; set; } = 0;

    }
}
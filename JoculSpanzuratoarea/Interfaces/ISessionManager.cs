namespace JoculSpanzuratoarea.Interfaces
{
    public interface ISessionManager
    {
        void DeleteSessionItems();
        void SetWord(string word);
        void SetWordDefinition(string wordDefinition);
        void SetMaskedWord(string maskedWord);
        void IncrementFailCount();
        void SetGuessedFullWord();
        int GetGuessedFullWord();
        string GetWord();
        string GetWordDefinition();
        string GetMaskedWord();
        int GetFailCount();
    }
}

using JoculSpanzuratoarea.Interfaces;

namespace JoculSpanzuratoarea.Services
{
    public class SessionManager: ISessionManager
    {
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void DeleteSessionItems()
        {
            _session.Clear();
        }

        public void SetWord(string word)
        {
            _session.SetString(SessionKeyEnum.SessionKeyWord.ToString(), word);
            return;
        }

        public void SetWordDefinition(string wordDefinition)
        {
            _session.SetString(SessionKeyEnum.SessionKeyWordDefinition.ToString(), wordDefinition);
            return;
        }

        public void SetMaskedWord(string maskedWord)
        {
            _session.SetString(SessionKeyEnum.SessionKeyMaskedWord.ToString(), maskedWord);
            return;
        }

        public void IncrementFailCount()
        {
            int sessionFailCount = (_session.GetInt32(SessionKeyEnum.SessionKeyFailCount.ToString()) ?? 0) + 1;
            _session.SetInt32(SessionKeyEnum.SessionKeyFailCount.ToString(), sessionFailCount);
            return;
        }

        public void SetGuessedFullWord()
        {
            _session.SetInt32(SessionKeyEnum.SessionKeyGuessedFullWord.ToString(), 1);
            return;
        }

        public int GetGuessedFullWord()
        {
            int sessionGuessedFullWord = _session.GetInt32(SessionKeyEnum.SessionKeyGuessedFullWord.ToString()) ?? 0;
            return sessionGuessedFullWord;
        }

        public string GetWord()
        {
            string sessionWord = _session.GetString(SessionKeyEnum.SessionKeyWord.ToString()) ?? "";
            return sessionWord;
        }

        public string GetWordDefinition()
        {
            string sessionMaskedWord = _session.GetString(SessionKeyEnum.SessionKeyWordDefinition.ToString()) ?? "";
            return sessionMaskedWord;
        }

        public string GetMaskedWord()
        {
            string sessionMaskedWord = _session.GetString(SessionKeyEnum.SessionKeyMaskedWord.ToString()) ?? "";
            return sessionMaskedWord;
        }

        public int GetFailCount()
        {
            int sessionFailCount = _session.GetInt32(SessionKeyEnum.SessionKeyFailCount.ToString()) ?? 0;
            return sessionFailCount;
        }

    }
}

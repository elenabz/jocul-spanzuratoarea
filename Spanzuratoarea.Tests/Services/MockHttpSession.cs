using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Spanzuratoarea.Tests.Services
{
    public class MockHttpSession : ISession
    {
        readonly Dictionary<string, object> _sessionStorage = [];
        public bool IsAvailable => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = Encoding.UTF8.GetString(value);
        }

        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
            if (_sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(_sessionStorage[key].ToString());
                return true;
            }
            value = null;
            return false;
        }

        //public static void SetInt32(this ISession session, string key, int value)
        //{
        //    var bytes = new byte[]
        //    {
        //        (byte)(value >> 24),
        //        (byte)(0xFF & (value >> 16)),
        //        (byte)(0xFF & (value >> 8)),
        //        (byte)(0xFF & value)
        //    };
        //    session.Set(key, bytes);
        //}

        //public static int? GetInt32(this ISession session, string key)
        //{
        //    var data = session.Get(key);
        //    if (data == null || data.Length < 4)
        //    {
        //        return null;
        //    }
        //    return data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3];
        //}
    }
}

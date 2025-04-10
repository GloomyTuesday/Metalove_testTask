using System.Threading.Tasks;

namespace Scripts.BaseSystems.InternetTools
{
    public interface IInternetTools
    {
        public Task<string> TryToGetRequestAnswerTcp(string url, int port);
    }
}

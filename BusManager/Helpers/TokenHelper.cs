using System.Threading;

namespace BusManager.Helpers
{
    public static class TokenHelper
    {
        public static CancellationToken GetToken(int ttl = 30)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(ttl);
            return cts.Token;
        }
    }
}

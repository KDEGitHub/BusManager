using System;
using System.Threading;

namespace BusManager.Helpers
{
    public static class TokenHelper
    {
        /// <summary>
        ///  получение токена отмееы
        /// </summary>
        /// <param name="ttl">время жизни логера</param>
        /// <returns></returns>
        public static CancellationToken GetToken(int ttl = 30)
        {
            CancellationTokenSource cts = new CancellationTokenSource(ttl*1000);
            return cts.Token;
        }
        
        public static CancellationTokenSource GetTokenSource(int ttl = 30)
        {
            return new CancellationTokenSource(ttl * 1000);
        }
      
        /// <summary>
        /// получение токена отмены для сообщения из шины
        /// </summary>
        /// <param name="created">время создания</param>
        /// <param name="ttlSec">время жизни токена</param>
        /// <returns></returns>
        public static CancellationToken GetTokenFromBus(DateTime created, int ttlSec)
        {
            var transTime = DateTime.UtcNow - created;
            double ttlMils = ttlSec * 1000;
            ttlMils = ttlMils - transTime.TotalMilliseconds;
            ttlMils = Math.Round(ttlMils, 0, MidpointRounding.AwayFromZero);
            return new CancellationTokenSource((int)ttlMils).Token;
        }

        public static CancellationTokenSource GetTokenSourceFromBus(DateTime created, int ttlSec)
        {
            var transTime = DateTime.UtcNow - created;
            double ttlMils = ttlSec * 1000;
            ttlMils = ttlMils - transTime.TotalMilliseconds;
            ttlMils = Math.Round(ttlMils, 0, MidpointRounding.AwayFromZero);
            return new CancellationTokenSource((int)ttlMils);
        }

    }
}

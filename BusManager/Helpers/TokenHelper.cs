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
        /// <param name="itemCreated">время создания токена</param>
        /// <param name="ttlSec">время жизни токена (сек)</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static CancellationToken GetToken(DateTime itemCreated, int ttl, DateTime? now = null)
        {
            if (!now.HasValue) now = DateTime.UtcNow;
            var transTime = (DateTime)now - itemCreated;
            double ttlMils = ttl * 1000;
            ttlMils = ttlMils - transTime.TotalMilliseconds;
            ttlMils = Math.Round(ttlMils, 0, MidpointRounding.AwayFromZero);
            return new CancellationTokenSource((int)ttlMils).Token;
        }

        public static CancellationTokenSource GetTokenSource(DateTime itemCreated, int ttl, DateTime? now = null)
        {
            if (!now.HasValue) now = DateTime.UtcNow;
            var transTime = (DateTime)now - itemCreated;
            double ttlMils = ttl * 1000;
            ttlMils = ttlMils - transTime.TotalMilliseconds;
            ttlMils = Math.Round(ttlMils, 0, MidpointRounding.AwayFromZero);
            return new CancellationTokenSource((int)ttlMils);
        }

    }
}

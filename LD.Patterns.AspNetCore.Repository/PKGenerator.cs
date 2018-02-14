using System;
using System.Threading;

namespace LD.Patterns.AspNetCore.Repository
{
    public class PKGenerator<TValue>
    {
        private long _current;

        public TValue Next()
            => (TValue)Convert.ChangeType(Interlocked.Increment(ref _current), typeof(TValue));
    }
}

﻿namespace Dixin.Linq.Parallel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Dixin.Common;

    public static class ParallelEnumerableX
    {
        public static void ForceParallel<TSource>(
            this IEnumerable<TSource> source, Action<TSource> action, int forcedDegreeOfParallelism)
        {
            source.NotNull(nameof(source));
            action.NotNull(nameof(action));

            ConcurrentQueue<TSource> queue = new ConcurrentQueue<TSource>(source);
            Thread[] threads = Enumerable
                .Range(0, Math.Min(forcedDegreeOfParallelism, queue.Count))
                .Select(_ => new Thread(() =>
                    {
                        TSource value;
                        while (queue.TryDequeue(out value))
                        {
                            action(value);
                        }
                    }))
                .ToArray();
            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());
        }
    }
}

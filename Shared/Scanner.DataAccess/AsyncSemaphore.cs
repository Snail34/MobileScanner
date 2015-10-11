using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.DataAccess
{
    public class AsyncSemaphore
    {
        private readonly static Task completed = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();

        private int currentCount;

        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0) throw new ArgumentOutOfRangeException("initialCount");

            this.currentCount = initialCount;
        }

        public Task WaitAsync()
        {
            lock(this.waiters)
            {
                if (this.currentCount > 0)
                {
                    --this.currentCount;
                    return completed;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();

                    this.waiters.Enqueue(waiter);
                    
                    return waiter.Task;
                }
            }
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock(this.waiters)
            {
                if (this.waiters.Count > 0)
                {
                    toRelease = this.waiters.Dequeue();
                }
                else
                {
                    ++this.currentCount;
                }
            }
            if (toRelease != null)
            {
                toRelease.SetResult(true);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner.DataAccess
{
    public class AsyncLock
    {
        private readonly AsyncSemaphore semaphore;

        private readonly Task<Releaser> releaser; 

        public AsyncLock()
        {
            this.semaphore = new AsyncSemaphore(1);
            this.releaser = Task.FromResult(new Releaser(this)); 
        }

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;

            internal Releaser(AsyncLock toRelease) { m_toRelease = toRelease; }

            public void Dispose()
            { 
                if (m_toRelease != null)
                    m_toRelease.semaphore.Release();
            } 
        }

        public Task<Releaser> LockAsync()
        {
            var wait = this.semaphore.WaitAsync();

            return wait.IsCompleted ?
                this.releaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }
}

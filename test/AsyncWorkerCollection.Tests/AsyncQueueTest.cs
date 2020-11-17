﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using dotnetCampus.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace AsyncWorkerCollection.Tests
{
    [TestClass]
    public class AsyncQueueTest
    {
        [ContractTestCase]
        public void DisposeTest()
        {
            "调用 AsyncQueue 销毁方法，有一个线程在等待出队，将会释放当前在等待出队的任务".Test(async () =>
            {
                // Arrange
                var asyncQueue = new AsyncQueue<int>();
                var task1 = asyncQueue.DequeueAsync();

                // Action
                asyncQueue.Dispose();
                // 等待一下 DequeueAsync 逻辑的完成
                await Task.Delay(TimeSpan.FromSeconds(1));

                // Assert
                Assert.AreEqual(true, task1.IsCompleted);
            });

            "在 AsyncQueue 进行销毁之前，存在元素没有出队，调用销毁时可以成功销毁".Test(() =>
            {
                // Arrange
                var asyncQueue = new AsyncQueue<int>();
                asyncQueue.Enqueue(0);
                asyncQueue.Enqueue(0);

                // Action
                asyncQueue.Dispose();

                // Assert
                Assert.AreEqual(0, asyncQueue.Count);
            });
        }
    }
}

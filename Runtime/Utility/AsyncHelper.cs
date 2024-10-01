using System;
using System.Collections;

namespace Hardgore
{
    public static class AsyncHelper
    {

        public static void WaitForFrame(Action action)
        {
            CoroutineRunner.StartCoroutine(IEWaitForFrame(action));
        }

        private static IEnumerator IEWaitForFrame(Action action)
        {
            yield return null;
            action();
        }
    }
}
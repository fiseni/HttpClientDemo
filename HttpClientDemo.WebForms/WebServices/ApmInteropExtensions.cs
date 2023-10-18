using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.WebForms.WebServices
{
    public static class ApmInteropExtensions
    {
        public static IAsyncResult AsApm(this Task task, AsyncCallback callback, object state)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var tcs = new TaskCompletionSource<object>(state);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(null);
                }

                if (callback != null)
                {
                    callback(tcs.Task);
                }
            }, TaskScheduler.Default);

            return tcs.Task;
        }

        public static IAsyncResult AsApm<T>(this Task<T> task, AsyncCallback callback, object state)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var tcs = new TaskCompletionSource<T>(state);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(t.Result);
                }

                if (callback != null)
                {
                    callback(tcs.Task);
                }
            }, TaskScheduler.Default);

            return tcs.Task;
        }

        public static void Unwrap(this IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }

            if (asyncResult is Task task)
            {
                task.GetAwaiter().GetResult();
            }
            else
            {
                throw new ArgumentException("Invalid asyncResult", nameof(asyncResult));
            }
        }

        public static T Unwrap<T>(this IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }

            if (asyncResult is Task<T> task)
            {
                return task.GetAwaiter().GetResult();
            }
            else
            {
                throw new ArgumentException("Invalid asyncResult", nameof(asyncResult));
            }
        }
    }
}

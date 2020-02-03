using System;
using System.Threading.Tasks;

namespace CarWorkshop.WPF.Infra
{
    public static class TaskHelpers
    {
        public static void ContinueOnUIThread<T>(this Task<T> task, Action<T> onSuccess, Action<Exception> onError = null)
        {
            task.ConfigureAwait(continueOnCapturedContext: true)
              .GetAwaiter()
              .OnCompleted(() =>
              {
                  if (task.IsCompletedSuccessfully)
                  {
                      onSuccess(task.Result);
                  }
                  else
                  {
                      var exception = task.Exception ?? new Exception("Async task failed");
                      if (onError!= null)
                      {
                          onError(exception);
                      }
                      else
                      {
                          throw exception;
                      }
                  }
              });
        }

        public static void ContinueOnUIThread(this Task task, Action onSuccess, Action<Exception> onError = null)
        {
            task.ConfigureAwait(continueOnCapturedContext: true)
              .GetAwaiter()
              .OnCompleted(() =>
              {
                  if (task.IsCompletedSuccessfully)
                  {
                      onSuccess();
                  }
                  else
                  {
                      var exception = task.Exception ?? new Exception("Async task failed");
                      if (onError != null)
                      {
                          onError(exception);
                      }
                      else
                      {
                          throw exception;
                      }
                  }
              });
        }
    }
}

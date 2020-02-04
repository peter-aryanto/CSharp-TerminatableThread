using System;
using System.Threading.Tasks;
using System.Threading;

namespace TerminatableThread
{
    public class TerminatableThread
    {
        public enum TerminatableType
        {
            InfiniteLoop,
            AMinuteDelay
        };

        private CancellationTokenSource cancellationSignaller;
        private CancellationToken cancellationSignal;
        private Task internalTask;

        public TerminatableType type { get; private set; }

        public void Start(TerminatableType type)
        {
            this.cancellationSignaller = new CancellationTokenSource();
            this.cancellationSignal = cancellationSignaller.Token;
            this.type = type;

            internalTask = Task.Run(async () =>
                {
                    Console.WriteLine("Terminatable thread"
                        + " containing " + this.type
                        + " is running ...");

                    switch (type)
                    {
                        case TerminatableType.InfiniteLoop:
                            // Below, cancellation will result in status: RanToCompletion.
                            while (!cancellationSignaller.IsCancellationRequested)
                            {
                                // Do nothing; infinite loop.
                            }
                            break;
                        case TerminatableType.AMinuteDelay:
                            // Below, cancellation will result in status: Cancelled.
                            // But, handling exception will result in status: RanToCompletion.
                            // try
                            // {
                                await Task.Delay(60 * 1000, cancellationSignal);
                            // }
                            // catch (Exception e)
                            // {
                            //     if (e is TaskCanceledException)
                            //     {
                            //         Console.WriteLine("Sssh..., "
                            //             + "exception is thrown from cancelling Task.Delay");
                            //     }
                            //     else
                            //     {
                            //         throw;
                            //     }
                            // }
                            break;
                        default:
                            throw new Exception("Invalid terminatable thread type #"
                                + ((int)type).ToString());
                    }
                }
            );
        }

        public TaskStatus GetStatus()
        {
            return internalTask.Status;
        }

        public void Stop()
        {
            cancellationSignaller.Cancel();
        }
    }
}
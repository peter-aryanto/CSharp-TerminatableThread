using System;
using System.Threading.Tasks;

namespace TerminatableThread
{
    class Program
    {
        static void Main(string[] args)
        {
            TerminatableThread terminatableInfiniteLoop = new TerminatableThread();
            TerminatableThread terminatableAMinuteDelay = new TerminatableThread();

            Console.WriteLine("Starting wrapper thread");
            Task wrapperThread = Task.Run(async () =>
            {
                Console.WriteLine("Starting terminatable thread of infinite loop");
                terminatableInfiniteLoop.Start(TerminatableThread.TerminatableType.InfiniteLoop);

                Console.WriteLine("Starting terminatable thread of 1 minute delay");
                terminatableAMinuteDelay.Start(TerminatableThread.TerminatableType.AMinuteDelay);

                Console.WriteLine("3 seconds to wrapper thread completion");
                await Task.Delay(3 * 1000);

                Console.WriteLine("Attempting to stop terminatable threads ...");
                terminatableInfiniteLoop.Stop();
                terminatableAMinuteDelay.Stop();
            });
            wrapperThread.Wait();
            Console.WriteLine("Wrapper thread status: " + wrapperThread.Status);

            Console.WriteLine("Terminatable thread status: " 
                + terminatableInfiniteLoop.GetStatus());
            Console.WriteLine("Terminatable thread status: " 
                + terminatableAMinuteDelay.GetStatus());
        }
    }
}
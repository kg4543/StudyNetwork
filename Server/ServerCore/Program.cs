using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("Thread Start!");

            while (!_stop)
            {

            }

            Console.WriteLine("Thread Stop!");
        }

        static void Main(string[] args)
        {
            
        }
    }
}

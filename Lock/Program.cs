using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lock
{
    class SpinLock
    {
        //가시성 보장(volatile)
        volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                /*int origin = Interlocked.Exchange(ref _locked, 1);

                if (origin == 0) // 잠금이 풀려 대기에서 벗어남
                    break;*/

                //CAS = Compare And Swap

                int expected = 0; // 기대값
                int desire = 1; // 원하는 값
                //_locked가 잠금이 안 잠겨있으면 (_locdked == expected) _locked를 잠근다. (_locked = desire)
                if(Interlocked.CompareExchange(ref _locked, desire, expected) == expected)
                    break;

                // 쉬다오기
                //Thread.Sleep(10); // 무조건 휴식
                //Thread.Sleep(0); // 조건부 양보 => 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인(대기)
                Thread.Yield(); // 관대한 양보 => 지금 실행이 가능한 쓰레드가 있으면 실행 / 없으면 다시 본인(대기)
            }
        }

        public void Release()
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int num = 0;
        static SpinLock spinLock = new SpinLock();

        static void Thread1()
        {
            for (int i = 0; i < 100000; i++)
            {
                spinLock.Acquire();
                num++;
                spinLock.Release();
            }
        }

        static void Thread2()
        {
            for (int i = 0; i < 100000; i++)
            {
                spinLock.Acquire();
                num--;
                spinLock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);
            Console.WriteLine(num);
        }
    }
}

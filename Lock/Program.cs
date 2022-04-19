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

    class Lock
    {
        // 커널단에서 조작 (시스템 부하가 큼)
        //Auto는 수동으로 문을 닫아서 Reset이 필요없다
        AutoResetEvent _available = new AutoResetEvent(true); //true : 아무나 들어올 수 있음
        //ManualResetEvent _available = new ManualResetEvent(true);

        public void Acquire()
        {
            _available.WaitOne(); // 입장 시도 flag = false
            //_available.Reset();
        }

        public void Release()
        {
            _available.Set(); //flag = true
        }
    }

    class Program
    {
        static int num = 0;
        //static SpinLock locked = new SpinLock();
        static Lock _lock = new Lock(); 
        //static Mutex _lock = new Mutex();

        static void Thread1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                //_lock.WaitOne();
                num++;
                _lock.Release();
                //_lock.ReleaseMutex();
            }
        }

        static void Thread2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                //_lock.WaitOne();
                num--;
                _lock.Release();
                //_lock.ReleaseMutex();
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

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Practice
{
    // 메모리 배리어
    // A) 코드 재배치 억제
    // B) 가시성

    // Full Memory Barrier : Store / Load 둘 다 막는다.
    // Store Memory Barrier
    // Load Memory Barrier 


    class Program
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread1()
        {
            y = 1; // Store y

            // -----------------------------Push
            Thread.MemoryBarrier(); //하드웨어가 함부로 메모리풀을 건들이지 못하게 한다.
            // -----------------------------Pull
            r1 = x; // Load x
        }

        static void Thread2()
        {
            x = 1; // Store x

            Thread.MemoryBarrier();

            r2 = y; // Load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread1);
                Task t2 = new Task(Thread2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                {
                    break;
                }
            }

            Console.WriteLine($"{count} 번");
        }
    }
}

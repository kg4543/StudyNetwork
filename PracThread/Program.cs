using System;
using System.Threading;
using System.Threading.Tasks;

namespace PracThread
{
    class FastLock
    {
        int id;
    }

    class SessionManager
    {
        static object _lock = new object();

        public static void TestSession()
        {
            lock(_lock)
            {

            }
        }

        public static void Test()
        {
            lock(_lock)
            {
                UserManager.TestUser();
            }
        }
    }

    class UserManager
    {
        static object _lock = new object();

        public static void TestUser()
        {
            lock (_lock)
            {

            }
        }

        public static void Test()
        {
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }
    }

    // 경합조건
    class Program
    {
        static int num;
        static object _obj = new object();
        

        static void Thread1()
        {
            for (int i = 0; i < 100000; i++)
            {
                //num++;

                //-> 어셈블리형으로 쪼개면...
                /*int temp = num;
                temp += 1;
                num = temp;*/

                // 원자성(Atomic)
                // 각 동작이
                // 한번에 이루어져야한다.
                // 유저끼리 아이템 교환 등 (트랜잭션 / 커밋)

                // All or Nothing
                // 원자성이 보장하나 성능이 다운
                //Interlocked.Increment(ref num);

                //상호배제
                //먼저 점유할 경우 다음 차례는 대기


                /*try
                {
                    Monitor.Enter(_obj); //잠금
                    num++;

                    return;
                    //중간에 return을 할 경우 잠금이 풀리지 않아 Thread2가 무한대기상황(DeadLock)이 될 수 있다 
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Monitor.Exit(_obj); //잠금해제
                }*/

                /*lock(_obj)
                {
                    num++;
                }*/

                SessionManager.Test();
            }
        }

        static void Thread2()
        {
            for (int i = 0; i < 100000; i++)
            {
                //num--;
                //원자성
                //Interlocked.Decrement(ref num);

                /*Monitor.Enter(_obj);
                num--;
                Monitor.Exit(_obj);*/

                /*lock (_obj)
                {
                    num--;
                }*/

                UserManager.Test();
            }
        }

        static void Main(string[] args)
        {
            Task task1 = new Task(Thread1);
            Task task2 = new Task(Thread2);
            task1.Start();
            task2.Start();

            Task.WaitAll(task1, task2);
            Console.WriteLine(num);
        }
    }
}

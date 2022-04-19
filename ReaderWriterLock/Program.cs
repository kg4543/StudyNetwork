using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{

    internal class Program
    {
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        //static Mutex _mutex = new Mutex(); //커널단에서 동작(무거워...)
        //RW Lock : RederWriterLock
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim();

        class Reward
        {

        }

        static Reward GetRewardId(int id)
        {
            _lock3.EnterReadLock();

            _lock3.ExitReadLock();

            return null;
        }

        static void AddReward(Reward reward)
        {
            _lock3.EnterWriteLock();

            _lock3.ExitWriteLock();
        }

        static void Main(string[] args)
        {
            lock(_lock)
            {

            }

            /*bool lockTaken = false;
            try
            {
                _lock2.Enter(ref lockTaken);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (lockTaken)
                {
                    _lock2.Exit();
                }
            }*/
        }
    }
}

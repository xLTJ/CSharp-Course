namespace RaceCondition;

class Program
{
    static int counter = 0;
    private static readonly Lock LockObject = new Lock();

    static void increment() {
        // uses lock to ensure that only one thread can access at a time
        lock (LockObject)
        {
            counter = counter + 1;
        }
    }

    static void thread_task() {
        for (int i = 0; i < 100_000; i++) {
            increment();
        }
    }

    static void race_condition_example() {
        
        counter = 0;

        Thread t1 = new Thread(thread_task);
        Thread t2 = new Thread(thread_task);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }

    static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            race_condition_example();

            Console.WriteLine($"Iteration {i}: Counter is {counter}");
        }
    }
}
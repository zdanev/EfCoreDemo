using System;

namespace EfCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            BlogDemo.Setup();
            BlogDemo.Execute();
            
            CustomerDemo.Execute();

            Console.WriteLine();
            Console.WriteLine("Done.");
        }
    }
}
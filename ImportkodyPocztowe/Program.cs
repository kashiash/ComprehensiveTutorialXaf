using Common.Module.Imports;

using System;

namespace ImportkodyPocztowe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Rozpoczeto import kodów pocztowych!");
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            string connectionString = @"Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\mssqllocaldb;Initial Catalog=ComprehensiveTutorialXaf";
            var MakeImporter = new KodyPocztoweImporter(connectionString);
            MakeImporter.Import(".\\UTF8KodyPocztowe\\spispna-cz1.txt");

                            watch.Stop();

            Console.WriteLine($"Complete Execution Time: {watch.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }
    }
}

using System;
using Hasidi.Infra.DataAccess;

namespace Hasidi.Infra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                var mem = new InMemoryKeyValueDataAccess<string, string>();
                mem.InsertData("11", "aa");
                mem.InsertData("22", "bb");
                mem.InsertData("33", "cc");

                var x = mem.GetAndUpdate("44", v =>
                {
                    v = "aaaaaa";
                    return v;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }

        }
    }
}

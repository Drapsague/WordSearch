using System;

namespace mots_glisses
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "0:0:10";
            double stop = TimeSpan.Parse(input).TotalMinutes;
            TimeSpan intStop = TimeSpan.FromMinutes(stop);
            DateTime debut = DateTime.Now;
            DateTime actuel = DateTime.Now;

            /// Durée écoulée
            TimeSpan interval = actuel - debut;
            while (interval < intStop)
            {
                Console.WriteLine(interval.ToString());
                actuel = DateTime.Now;
                interval = actuel - debut;
            }
            Console.WriteLine(interval);
        }



    }
 }


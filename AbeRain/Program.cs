using System;
using System.Collections.Generic;

namespace AbeRain
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            short fact_index;
            List<string> facts, topics;

            do
            {
                Console.Write("What would you like to know about? (quit to quit) ");
                input = Console.ReadLine().ToLower();

                if (input == "quit")
                    break;

                // Topics
                topics = NLP.GetKeywords(input);
                //topics = Encyclopedia.ParentTopics(input);
                if (topics.Count > 0)
                {
                    Console.Write("Parent topics: \n\t");
                    foreach (string topic in topics)
                        Console.Write("[" + topic + "] ");

                    Console.WriteLine();
                }

                // Facts
                facts = Encyclopedia.LookUpFacts(input);
                if (facts.Count > 0)
                {
                    fact_index = 1;
                    Console.WriteLine("Facts: ");
                    foreach (string fact in facts)
                        Console.WriteLine("\t" + fact_index++ + ": " + fact);
                }
                else
                {
                    Console.WriteLine("Sorry, dunno any facts about that, brah!");
                }

                Console.WriteLine();

            } while (input != "quit");
        }
    }
}

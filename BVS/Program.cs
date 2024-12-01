using System;
using System.Collections;
using System.Collections.Generic;

namespace Tree
{
    static class Program{
        static void Main(){
            BVS<int, int> n = new(0, 5);
            n.Insert(1, 9);
            n.Insert(-1, -5);

            n.DeleteByKey(-1);

            Console.WriteLine(n.Find(1)?.Value);
            Console.WriteLine(n.GetMinimum().Value);

            n.Show();
        }
    }
}
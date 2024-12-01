using System;
using System.Collections;
using System.Collections.Generic;

// Big O, bude stoupat rychleji než jakákoliv polonomiální funkce pokud P != NP, jak přesně to mám vyjádřit nevím

namespace Water{
    static class Program{
        static void Main(){
            _ = Capacities.capacity_one;

            (bool finished, uint length) = Solver.Solve();

            if (finished) Console.WriteLine("Lze, na " + length + " kroků");
            else Console.WriteLine("Nelze");
        }
    }

    static class Solver{
        public static (bool, uint) Solve(){
            HashSet<int> already_found_start = [];

            Queue<State> what_to_go_through = new();
            what_to_go_through.Enqueue(new State(0, 0, 0));

            while (what_to_go_through.Count != 0){
                State current_state = what_to_go_through.Dequeue();

                var new_states = current_state.GetStates();

                foreach (State s in new_states){
                    if (s.Done()){
                        return (true, s.Length);
                    }

                    if (!already_found_start.Contains(s.GetHashCode())){
                        already_found_start.Add(s.GetHashCode());
                            
                        what_to_go_through.Enqueue(s);
                    }
                }

            }

            return (false, 0);
        }
    }

    readonly struct State(ushort bottle_one, ushort bottle_two, uint length)
    {
        readonly ushort bottle_one = bottle_one;
        readonly ushort bottle_two = bottle_two;
        public readonly uint Length { get; } = length;

        public readonly State[] GetStates(){
            return [TransferToOne(), TransferToTwo(), EmptyOne(), EmptyTwo(), FillOne(), FillTwo()];
        }

        public override readonly int GetHashCode()
        {
            return (bottle_one << 16) + bottle_two;
        }

        private readonly State TransferToTwo()
        {
            if (bottle_one + bottle_two > Capacities.capacity_two){
                return new State((ushort)(bottle_one - Capacities.capacity_two + bottle_two), Capacities.capacity_two, Length+1);
            }

            return new State(0, (ushort)(bottle_one + bottle_two), Length+1);
        }

        private readonly State TransferToOne()
        {
            if (bottle_one + bottle_two > Capacities.capacity_one){
                return new State(Capacities.capacity_one, (ushort)(bottle_two - Capacities.capacity_one + bottle_one), Length+1);
            }

            return new State((ushort)(bottle_one + bottle_two), 0, Length+1);
        }

        private readonly State EmptyOne(){
            return new State(0, bottle_two, Length+1);
        }

        private readonly State EmptyTwo(){
            return new State(bottle_one, 0, Length+1);
        }

        private readonly State FillTwo(){
            return new State(bottle_one, Capacities.capacity_two, Length+1);
        }
        private readonly State FillOne(){
            return new State(Capacities.capacity_one, bottle_two, Length+1);
        }

        public readonly bool Done(){
            return Capacities.final_hash == GetHashCode();
        }
    }

    public static class Capacities
    {
        public static readonly ushort capacity_one;
        public static readonly ushort capacity_two;
        public static readonly int final_hash;

        static Capacities()
        {
            Console.Write("Capacity of first bottle: ");
            string? num_one = Console.ReadLine();
            Console.Write("Capacity of second bottle: ");
            string? num_two = Console.ReadLine();
            Console.Write("Final volume of first bottle: ");
            string? fin_one = Console.ReadLine();
            Console.Write("Final volume of second bottle: ");
            string? fin_two = Console.ReadLine();

            if (num_one is null || num_two is null || fin_one is null || fin_two is null) throw new Exception("Nuh uh");

            capacity_one = ushort.Parse(num_one);
            capacity_two = ushort.Parse(num_two);
            final_hash = (ushort.Parse(fin_one) << 16) + ushort.Parse(fin_two);
        }
    }
}

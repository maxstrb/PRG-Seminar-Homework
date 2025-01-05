using System;
using System.Collections.Generic;

namespace maturitnipisemka
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                using StreamReader sr = new("./vstupni_soubory_Zasnezene_hory/1.txt");
                string file = sr.ReadToEnd();

                (int n, int[,] avalanche_times, (int, int)[] moves) = Solver.ParseString(file);

                Solver s = new(n, avalanche_times, moves);
                (int, (int, int)[]) result = s.Solve();

                Console.WriteLine($"Výsledek: {result.Item1}\nCesta: {string.Join(" ", result.Item2)}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file doesn't exist");
            }
            catch (Exception e)
            {
                Console.WriteLine($"It looks like the file didn't contain correct information: {e}");
            }
        }
    }

    class Solver
    {
        readonly int n;
        readonly int[,] found_time;
        readonly int[,] avalanche_times;

        readonly (int, int)[] moves;

        readonly Queue<(int, int)> last_updated = new();

        public static (int, int[,], (int, int)[]) ParseString(string raw_input) //TODO return as struct for readibility
        {
            string[] input = raw_input.Split("\n");

            int n = int.Parse(input[0]);
            int m = int.Parse(input[1]);

            int[,] avalanche_times = new int[n, n];
            for (int row = 0; row < n; row++) for (int col = 0; col < n; col++) avalanche_times[row, col] = int.MaxValue;

            for (int i = 2; i < m+2; i++)
            {
                string[] splitted_line = input[i].Split(" ");

                int row = int.Parse(splitted_line[0]);
                int col = int.Parse(splitted_line[1]);
                int time = int.Parse(splitted_line[2]);

                avalanche_times[row, col] = time;
            }

            int k = int.Parse(input[m+2]);
            (int, int)[] moves = new (int, int)[k];

            for (int i = m+3; i < k + m + 3; i++)
            {
                string[] splitted_line = input[i].Split(" ");

                int row = int.Parse(splitted_line[0]);
                int col = int.Parse(splitted_line[1]);

                moves[i - m - 3] = (row, col);
            }

            return (n, avalanche_times, moves);
        }

        public Solver(int n, int[,] avalanche_times, (int, int)[] moves)
        {
            this.n = n;
            found_time = new int[n, n];
            for (int row = 0; row < n; row++) for (int col = 0; col < n; col++) found_time[row, col] = int.MaxValue;

            found_time[0, 0] = 0;
            last_updated.Enqueue((0, 0));

            this.avalanche_times = avalanche_times;
            this.moves = moves;
        }

        bool IsOutOfBounds(int row, int col)
        {
            return row >= n || col >= n || row < 0 || col < 0;
        }

        bool IsMoveValid((int, int) starting_point, (int, int) move, int time)
        {
            for (int i = 1; i < Math.Max(Math.Abs(move.Item1), Math.Abs(move.Item2))+1; i++)
            {
                (int, int) new_pos = (
                    starting_point.Item1 + i * Math.Sign(move.Item1),
                                      starting_point.Item2 + i * Math.Sign(move.Item2)
                );

                if (IsOutOfBounds(new_pos.Item1, new_pos.Item2)) return false;
                if (avalanche_times[new_pos.Item1, new_pos.Item2] <= time) return false;
            }

            return true;
        }

        (int, int)[] SolvePath()
        {
            int current_row = n - 1;
            int current_col = n - 1;

            (int, int)[] output = new (int, int)[found_time[current_row, current_col]+1];
            output[found_time[current_row, current_col]] = (current_row, current_col);

            for (int i = 0; i < moves.Length; i++)
            {
                moves[i] = (-moves[i].Item1, -moves[i].Item2);
            }

            while (current_row != 0 || current_col != 0)
            {
                foreach ((int move_row, int move_col) in moves)
                {
                    if (!IsMoveValid((current_row, current_col), (move_row, move_col), found_time[current_row, current_col] - 1)) continue; //Figure out time

                    int new_row = current_row + move_row;
                    int new_col = current_col + move_col;

                    if (found_time[new_row, new_col] == found_time[current_row, current_col] - 1)
                    {
                        current_row = new_row;
                        current_col = new_col;

                        output[found_time[current_row, current_col]] = (current_row, current_col);
                        break;
                    }
                }
            }

            return output;
        }

        public (int, (int, int)[]) Solve()
        {
            while (last_updated.Count > 0)
            {
                (int current_row, int current_col) = last_updated.Dequeue();

                foreach((int move_row, int move_col) in moves)
                {
                    if (!IsMoveValid((current_row, current_col), (move_row, move_col), found_time[current_row, current_col])) continue;

                    int new_row = current_row + move_row;
                    int new_col = current_col + move_col;

                    if (found_time[new_row, new_col] > found_time[current_row, current_col])
                    {
                        last_updated.Enqueue((new_row, new_col));
                        found_time[new_row, new_col] = found_time[current_row, current_col] + 1;
                    }
                }
            }

            if (found_time[n - 1, n - 1] == int.MaxValue) {
                return (-1, []);
            }

            return (found_time[n-1, n-1], SolvePath());
        }
    }
}

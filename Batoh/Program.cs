namespace Coins{
  static class Program{
    static void Main(){
      int[] costs = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
      int[] items = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
      int capacity = int.Parse(Console.ReadLine()!);

      bool[] items_used = new bool[items.Length];

      int[] solution = new int[0];
      int solution_value = 0;

      Console.WriteLine("");

      Step(items, costs, capacity, 0, items_used, new List<int>(), ref solution, ref solution_value);

      Console.WriteLine(solution_value);
      Console.WriteLine(string.Join(", ", solution.Select(x => x + 1)));
    }

    static void Step(int[] items, int[] costs, int remaining_capacity, int current_value, bool[] items_used, List<int> used_items, ref int[] solution, ref int solution_value){
      if (remaining_capacity < 0) return;
      if (remaining_capacity >= 0 && current_value > solution_value){
        solution = used_items.ToArray();
        solution_value = current_value;
      }

      for (int i = 0; i < items.Length; i++) {
        if (items_used[i]) continue;

        items_used[i] = true;
        used_items.Add(i);
          
        Step(items, costs, remaining_capacity - costs[i], current_value + items[i], items_used, used_items, ref solution, ref solution_value);

        items_used[i] = false;
        used_items.RemoveAt(used_items.Count - 1);
      } 
    }
  }
}

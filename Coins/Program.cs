namespace Coins{
  static class Program{
    static void Main(){
      int[] coins = Console.ReadLine()!.Split().Select(x => int.Parse(x)).ToArray();
      int target = int.Parse(Console.ReadLine()!);

      Console.WriteLine("");
      if (target <= 0){
        Console.WriteLine("Nelze");
        return;
      }

      bool found_something = false;

      Step(coins, target, new List<int>(), 0, 0, ref found_something);

      if (!found_something){
        Console.WriteLine("Nezle");
      }
    }

    static void Step(int[] coins, int target, List<int> so_far, int sum, int index, ref bool found_something){
      if (sum > target) return;
      if (sum == target){
        Console.WriteLine(string.Join(" ", so_far));
        found_something = true;
        return;
      }

      for (int i = index; i < coins.Length; i++)
      {
          so_far.Add(coins[i]);
          
          Step(coins, target, so_far, sum+coins[i], i, ref found_something);

          so_far.RemoveAt(so_far.Count - 1);
      } 
    }
  }
}

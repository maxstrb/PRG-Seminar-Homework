namespace kola{
  public static class Program{
    public static void Main(){
      int k = int.Parse(Console.ReadLine() ?? throw new Exception("Well fuck"));

      (float value, List<int> combination) output = Money(k, 100);

      Console.WriteLine(output.value * 100_000);
      Console.WriteLine(string.Join(", ", output.combination.ToArray().Reverse()));
    }

    static (float value, List<int> combination) Money(int round, int remaining){
      if (round == 1)
        return (1, [remaining]);

      float best = 0;
      List<int> best_combination = [];

      for (int i = 1; i < remaining; i++){
        (float value, List<int> combination) current_value = Money(round - 1, remaining - i);

        if (current_value.value + ((float)i/remaining) > best){
          best = current_value.value + ((float)i/remaining);
          best_combination = current_value.combination;
          best_combination.Add(i);
        }
      }

      return (best, best_combination);
    }
  }
}

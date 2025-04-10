namespace podmnozina{
  using System.Text;

  class Program{
    public static void Main(){
      using StreamReader sr = new("vstupy.txt");
      string input_file = sr.ReadToEnd();

      float[]?[] input = input_file.Split(Environment.NewLine).Where((x, i) => i % 2 == 0).Select(x => x == "" ? null : x.Split().Select(float.Parse).ToArray()).ToArray();

      StringBuilder sb = new();

      foreach (var i in input){
        float[] solution = Solve(i);
        sb.Append((solution.Length == 0 ? "prázdná posloupnost" : string.Join(", ", solution)) + "\n\n");
      }

      Console.WriteLine(sb.ToString().TrimEnd('\n'));
      
      /*using StreamWriter sw = new("vystupy.txt");
      sw.Write(sb.ToString().TrimEnd('\n'));*/
    }

    static float[] Solve(float[]? input){
      if (input == null || input.Length == 0){
        return [];
      }

      int l = input.Length;

      if (l == 1){
        return [input[0]];
      }

      int[] lengths = new int[l];
      int[] indexes = new int[l];

      lengths[^1] = 1;
      indexes[^1] = -1;

      for (int i = l - 2; i >= 0; i--){
        int current_best = 0;
        int current_best_index = -1;

        for (int j = i + 1; j < l; j++){
          if (lengths[j] > current_best && input[j] > input[i]){
            current_best = lengths[j];
            current_best_index = j;
          }
        }

        lengths[i] = current_best + 1;
        indexes[i] = current_best_index;
      }

      int best = lengths.Max();
      int current_index = Array.IndexOf(lengths, best);

      List<float> output = [];

      while (current_index != -1){
        output.Add(input[current_index]);
        current_index = indexes[current_index];
      }

      return output.ToArray();
    }
  }
}

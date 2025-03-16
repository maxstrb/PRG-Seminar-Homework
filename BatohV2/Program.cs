namespace BatohV2{
  class Program{
    static void Main(){
      int[] values = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
      int[] weights = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

      int capacity = int.Parse(Console.ReadLine()!);

      Console.WriteLine(Solve(weights, values, capacity));
    }

    static int Solve(int[] weights, int[] values, int capacity){
      int[,] matrix = new int[values.Length + 1, capacity + 1];
      
      for (int row = 1; row <= values.Length; row++){
        for (int col = 1; col <= capacity; col++){
          int current_item = row - 1;

          if (weights[current_item] > col) {
            matrix[row, col] = matrix[row - 1, col];
            continue;
          }

          matrix[row, col] = Math.Max(matrix[row - 1, col], values[current_item] + matrix[row - 1, col - weights[current_item]]);
        }
      }

      return matrix[values.Length, capacity];
    }
  }
}

using System.Text;

namespace Analyza_Textoveho_Souboru
{
    public static class Program
    {
        public static void Main()
        {
            string input;

            try
            {
                using StreamReader sr = new("./2_vstup.txt", Encoding.UTF8);
                input = sr.ReadToEnd();
            }

            catch (Exception err)
            {
                Console.WriteLine($"Ajaj, něco se nepovedlo, nemůžu otevřít vstupní soubor ({err.Message})");
                return;
            }

            string[] input_splitted = input.Split(new char[]{' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            try
            {
                using StreamWriter sw = new("outputik.txt", false, Encoding.UTF8);

                sw.WriteLine($"Počet slov: {WordCount(input_splitted)}");
                sw.WriteLine($"Počet nebílých znaků: {NonWhiteCharCount(input)}");
                sw.WriteLine($"Počet znaků: {CharCount(input)}");
                sw.WriteLine($"Slova s počtem:\r\n\t{
                    string.Join("\r\n\t", WordsWithCount(input_splitted).Select(val => $"{val.Key}: {val.Value}"))
                    }");
                sw.WriteLine($"Slova v pořadí:\r\n{TrimWhiteSpace(input)}");
            }

            catch
            {
                Console.WriteLine("Ajaj, něco se nepovedlo, nemůžu vytvořit výstupní soubor");
            }
        }
        static string TrimWhiteSpace(string input){
            StringBuilder sb = new();

            foreach (char ch in input){
                if (!char.IsWhiteSpace(ch)){
                    sb.Append(ch);
                }
                else if (ch == '\n'){
                    sb.Append("\r\n");
                }
                else if (sb.Length >= 1 && ch == ' ' && sb[^1] != ' ' && sb[^1] != '\n'){
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }
        static int WordCount(string[] input)
        {
            return input.Length;
        }
        static int CharCount(string input)
        {
            return input.Length;
        }
        static int NonWhiteCharCount(string input)
        {
            return input.Aggregate(0, (sum, currentChar) => sum + (Char.IsWhiteSpace(currentChar) ? 0 : 1));
        }
        static KeyValuePair<string, int>[] WordsWithCount(string[] input)
        {
            Dictionary<string, int> output = [];

            foreach (var word in input)
            {
                if (output.TryGetValue(word, out int value))
                    output[word] = value + 1;

                else output.Add(word, 1);
            }

            return [.. output];
        }
    }
}
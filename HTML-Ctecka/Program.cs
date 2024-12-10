using System.Text;

namespace HTMLCtecka
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://xkcd.com/39/");
                response.EnsureSuccessStatusCode();
                TextReader htmlContent = new StreamReader(response.Content.ReadAsStream());

                using Reader r = new(htmlContent, 1024);
                r.SkipUntil("<div id=\"comic\">");
                r.SkipUntil("title=\"");
                string? output = r.ReadUntil("\"");

                Console.WriteLine(output);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
    }

    class Reader : IDisposable {
        readonly TextReader tr;
        readonly char[] character_buffer;
        int number_of_correct_character_in_a_row = 0;
        int current_character_index = -1;
        int number_of_read_characters;

        public Reader(TextReader reader, int buffer_size) {
            tr = reader ?? throw new ArgumentNullException(nameof(reader));
            character_buffer = new char[buffer_size];

            number_of_read_characters = tr.ReadBlock(character_buffer, 0, buffer_size);
        }

        char? NextChar(){
            if (current_character_index + 1 >= number_of_read_characters){
                if (number_of_read_characters >= character_buffer.Length){
                    number_of_read_characters = tr.ReadBlock(character_buffer);
                    current_character_index = 0;

                    if (number_of_read_characters == 0){
                        return null;
                    }

                    return character_buffer[0];
                }
                
                return null;
            }

            current_character_index++;
            return character_buffer[current_character_index];
        }

        public void SkipUntil(string sequence){
            char? current_character;
            while ((current_character = NextChar()) != null){
                if (sequence[number_of_correct_character_in_a_row] == current_character){
                    number_of_correct_character_in_a_row++;
                    if (number_of_correct_character_in_a_row >= sequence.Length){
                        number_of_correct_character_in_a_row = 0;
                        return;
                    }

                    continue;
                }

                number_of_correct_character_in_a_row = 0;
            }
        }

        public string? ReadUntil(string sequence){
            char? current_character;
            StringBuilder sb = new();

            while ((current_character = NextChar()) != null){
                if (sequence[number_of_correct_character_in_a_row] == current_character){
                    number_of_correct_character_in_a_row++;
                    if (number_of_correct_character_in_a_row >= sequence.Length){
                        number_of_correct_character_in_a_row = 0;
                        return sb.ToString();
                    }

                    continue;
                }

                number_of_correct_character_in_a_row = 0;
                sb.Append(current_character);
            }

            return null;
        }

        public void Dispose() {
            tr?.Dispose();
        }
    }
}
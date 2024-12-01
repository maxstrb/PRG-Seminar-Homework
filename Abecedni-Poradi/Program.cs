using System;
using System.Collections.Generic;
using System.Text;

/*
    Co se bonusů týče tak můj kód rovnou vypisuje spolu s výsledkem

    1) Nemusí
    2) abd abc ac ad
*/

namespace Alphabet{
    static class Program{
        static void Main(){
            Console.WriteLine("Svůj vstup zadejte uspořádeně jako slova s mezerami mezi nimi: ");

            string? input = Console.ReadLine();
            if (input is null){
                Console.WriteLine("Nemůžu přečíst konzoli...");
                return;
            }
            Node[] nodes = InputHandler.FromWordsToList(input.Split(' '));

            try{
                (string output, bool moreChoices) = Solver.Solve(nodes);
                Console.WriteLine(string.Join(" -> ", output.ToArray()));
                Console.WriteLine(moreChoices?"Bylo více možností":"Byla pouze jedna možnost");
            }
            catch{
                Console.WriteLine("Tento vstup není možné vyřešit!");
            }
        }
    }

    static class InputHandler{
        public static Node[] FromWordsToList(string[] words){
            Node[] nodes = new Node[58];

            for (int i = 0; i < words.Length - 1; i++){
                for (int ch = 0; ch < Math.Min(words[i].Length, words[i+1].Length); ch++){
                    char earlier_word_current_char = words[i][ch];
                    char later_word_current_char = words[i+1][ch];

                    if (nodes[earlier_word_current_char-'A'] is null) nodes[earlier_word_current_char-'A'] = new Node(earlier_word_current_char);
                    if (nodes[later_word_current_char-'A'] is null) nodes[later_word_current_char-'A'] = new Node(later_word_current_char);

                    if (earlier_word_current_char == later_word_current_char) continue;
                    if (nodes[earlier_word_current_char - 'A'].ContainsChild(later_word_current_char)) break;

                    nodes[earlier_word_current_char - 'A'].AddChild(nodes[later_word_current_char-'A']);
                    nodes[later_word_current_char - 'A'].AddParent();
                    
                    break;
                }
            }

            return nodes;
        }
    }

    static class Solver{
        public static (string, bool) Solve(Node[] nodes){
            StringBuilder output = new();
            bool moreChoices = false;

            Queue<Node> sources = Findsources(nodes);

            while (sources.Count != 0) {
                moreChoices = moreChoices || (sources.Count > 1);

                Node current_node = sources.Dequeue();

                output.Append(current_node.Character);

                foreach (Node child in current_node.Children) if (child is not null && child.RemoveParent()) sources.Enqueue(child);
            }

            CheckIfFinished(nodes);

            return (output.ToString(), moreChoices);
        }

        private static Queue<Node> Findsources(Node[] nodes){
            Queue<Node> output = new();
            foreach(Node current_node in nodes) if (current_node is not null && current_node.NumberOfParents == 0) output.Enqueue(current_node);
            return output;
        }

        private static void CheckIfFinished(Node[] nodes){
            foreach (Node node in nodes) if (node is not null && (node.NumberOfParents != 0)) throw new ArgumentException("This input is not solvable!");
        }
    }

    class Node(char Character)
    {
        public readonly char Character = Character;

        public int NumberOfParents { get; private set; } = 0;
        public List<Node> Children { get; private set; } = [];
        long childrenCharsMask = 0;

        public bool ContainsChild(char c){
            return ((childrenCharsMask >> (c - 'A'))&1) == 1;
        }

        public void AddChild(Node n){
            Children.Add(n);
            childrenCharsMask += 1L<<(n.Character-'A');
        }

        public void AddParent(){
            NumberOfParents++;
        }

        public bool RemoveParent(){
            NumberOfParents--;
            return NumberOfParents == 0;
        }
    }
}
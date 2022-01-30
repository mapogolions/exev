using System;

namespace Exev.App;

internal static class Program
{
    internal static void Main()
    {
        var evaluator = new Evaluator();
        while (true)
        {
            Console.Write("> ");
            var source = Console.ReadLine();
            try
            {
                if (string.IsNullOrEmpty(source)) continue;
                var tree = new Parser(new Lexer(source)).Parse();
                var result = evaluator.Evaluate(tree);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

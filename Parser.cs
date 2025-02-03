using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleApp1;

public enum ParserState
{
    None,
    Loading,
    Parsing
}

public class Parser : IDisposable
{
    public static ParserState Loading = ParserState.None;
    public Dictionary<string, Type> Symbols;
    public Dictionary<string, ClassDescriptor> Classes;
    public Parser()
    {
        Symbols = new();
        Classes = new();

        Loading = ParserState.Loading;

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (type.Namespace == "ConsoleApp1.Symbol" && !type.IsAbstract)
            {
                var instance = Activator.CreateInstance(type, new object[]{});
                var method = type.GetMethod("GetCommand");
                if (method != null)
                {
                    var command = method.Invoke(instance, null) as string;
                    Symbols.Add(command, type);
                }
            }
        }
        
        Loading = ParserState.None;
    }
    
    public void Parse(string path)
    {
        Loading = ParserState.Parsing;
        
        var file = File.ReadAllText(path);
        var lines = file.Split("\n"); // Fuck lambdas
        var tmpLines = lines;
        for (var i = 0; i < lines.Length; i++)
            tmpLines[i] = lines[i].ClearShittyChars();

        ClassDescriptor currentObj = null;
        List<Symbol.Symbol> listSymbols = new List<Symbol.Symbol>();
        bool async = false;
        string name = "";

        for (var idLine = 0; idLine < lines.Length; idLine++)
        {
            var line = lines[idLine].Replace("\t", "");
            var parts = Regex.Split(line, @"\s+(?=(?:[^""]*""[^""]*"")*[^""]*$)").Select(p => p.Trim('"')).ToArray();
            if (parts[0] == "define")
            {
                if (parts[1] == "class")
                {
                    currentObj = new ClassDescriptor(parts[2]);
                }
                else if (parts[1] == "method")
                {
                    if (currentObj == null)
                        throw new ParsingException("Cannot define an function inside nothing! (no class defined.)", idLine, line);
                    listSymbols = new();
                    async = parts[2] == "async";
                    name = async ? parts[3] : parts[2];
                }
                else if (parts[1] == "method_end")
                {
                    if (currentObj == null)
                        throw new ParsingException("Cannot define an function inside nothing! (no class defined.)", idLine, line);
                    if (listSymbols == null)
                        throw new ParsingException("Cannot define an end of a function without defining a function!", idLine, line);
                    currentObj.AddFunction(new FunctionObject(name, async, listSymbols));
                    listSymbols = null;
                    async = false;
                    name = "";
                }
                else if (parts[1] == "class_end")
                {
                    if(listSymbols != null)
                        throw new ParsingException("Function not closed!", idLine, line);
                    if (currentObj == null)
                        throw new ParsingException("Cannot define an function inside nothing! (no class defined.)", idLine, line);
                    Classes.Add(currentObj.Name, currentObj);
                    currentObj = null;
                }
                else
                {
                    throw new ParsingException("Invalid define command!", idLine, parts[1]);
                }
            }
            else
            {
                if(String.IsNullOrWhiteSpace(parts[0]) || parts[0].StartsWith("#"))
                    continue;
                
                if (listSymbols == null)
                    throw new ParsingException($"Cannot define a symbol outside of a function! (LINE: {idLine} - '{line}')" , idLine, line);

                if (Symbols.ContainsKey(parts[0]))
                {
                    List<string> args = new();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        args.Add(parts[i]);
                    }
                    
                    var symbol = Activator.CreateInstance(Symbols[parts[0]], new object[]{args.ToArray()});

                    if (symbol is not Symbol.Symbol)
                        throw new Exception("Symbol must implement the Symbol interface!");

                    listSymbols.Add((Symbol.Symbol)symbol);
                }
                else
                {
                    throw new ParsingException("Invalid symbol!", idLine, parts[0]);
                }
            }
        }
        
        if(currentObj != null)
            throw new ParsingException("Class not closed!", lines.Length, "");
        
        if(listSymbols != null)
            throw new ParsingException("Function not closed!", lines.Length, "");
        
        Loading = ParserState.None;
    }
    
    public VM GenerateVM()
    {
        return new VM(Classes);
    }

    public void Dispose()
    {
        Symbols.Clear();
        Classes.Clear();
    }
}

public static class StringExtensions
{
    public static string ClearShittyChars(this string str)
    {
        return str.Replace("\r", "").Replace("\n", "");
    }
}

public class ParsingException : Exception
{
    public ParsingException(string message, int line, string part) : base($"Error during parsing at line {line} ('{part.Replace("\r", "").Replace("\n", "")}'): {message.Replace("\r", "").Replace("\n", "")}") {}
}
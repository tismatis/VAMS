namespace ConsoleApp1;

public class VM
{
    public Dictionary<string, ClassDescriptor> Classes;
    public List<ClassInstance> Instances;

    public VM(Dictionary<string, ClassDescriptor> classes)
    {
        Classes = Classes = classes.ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var @class in classes)
            @class.Value.Setup();
    }
    
    public void Execute(ulong instance, string @class, string function, params object[] args)
    {
        //Instances[instance].Execute(function, args);
        throw new NotImplementedException("Cannot call instance functions yet.");
    }
    
    public void Execute(string @class, string function, params object[] args)
    {
        Classes[@class].Execute(function, args);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class VM
    {
        public Dictionary<string, ClassDescriptor> Classes;
        public List<ClassInstance> Instances;
        
        public Queue<Action> MainThreadTasks = new Queue<Action>();

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
    
        public object Execute(string @class, string function, params object[] args)
        {
            return Classes[@class].Execute(this, function, args);
        }
        
        public void Update()
        {
            while (MainThreadTasks.Count > 0)
                MainThreadTasks.Dequeue().Invoke();
        }
    }
}
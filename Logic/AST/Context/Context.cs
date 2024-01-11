using System;
using System.Collections.Generic;

public class Context : IContext
{
    private readonly IContext parent;

    public Dictionary<string, (ExpType, Expression)> Variables { get; private set; } = new();

    public Dictionary<string, (List<string>, Expression, IContext)> Functions { get; private set; } = new();

    public Context()
    {
        Variables["PI"] = (ExpType.Number, new Number(MathF.PI.ToString()));
        Variables["E"] = (ExpType.Number, new Number(MathF.E.ToString()));
        AddFunction("sin", new Sin(), ("x", ExpType.Number));
        AddFunction("cos", new Cos(), ("x", ExpType.Number));
        AddFunction("log", new Log(), ("a", ExpType.Number), ("b", ExpType.Number));
        AddFunction("ln", new Ln(), ("x", ExpType.Number));
        AddFunction("intersect", new Intersect(), ("f1", ExpType.Undefined), ("f2", ExpType.Undefined));
        AddFunction("points", new Points(), ("f", ExpType.Undefined));
        AddFunction("samples", new Samples());
        AddFunction("randoms", new Randoms());

        void AddFunction(string identifier, FixedFunction function, params (string, ExpType)[] args){
            IContext innerContext = this.CreateChildContext();
            List<string> argNames = new();
            foreach (var arg in args){
                innerContext.DefineVariable(arg.Item1, arg.Item2);
                argNames.Add(arg.Item1);
            }
            Functions[identifier] = (argNames, function, innerContext);
        }
    }

    public Context(IContext parent) => this.parent = parent;

    public bool VariableIsDefined(string variable) 
    => Variables.ContainsKey(variable) || (parent != null && parent.VariableIsDefined(variable));

    public bool FunctionIsDefined(string function, int args) 
    => (Functions.TryGetValue(function, out (List<string>, Expression, IContext) func) && func.Item1.Count == args) 
        || (parent is not null && parent.FunctionIsDefined(function, args));

    public void DefineVariable(string variable, ExpType type = ExpType.NotSet, Expression value = null) 
    {
        if (!Variables.ContainsKey(variable)){
            if (value is not null && value.Type is ExpType.Number){
                value.Evaluate(this);
                Variables[variable] = (type, new Number(value.Value));
            }
            else
                Variables[variable] = (type, value);
        }
    }

    public void DefineFunction(string function, List<string> args, Expression body)
    {
        if (!Functions.ContainsKey(function))
            Functions[function] = (args, body, this.CreateChildContext());
    }

    public void SetVariableType(string variable, ExpType type)
    {
        if (Variables.TryGetValue(variable, out (ExpType, Expression) variableValue))
            Variables[variable] = (type, variableValue.Item2);
    }

    public void SetVariableValue(string variable, Expression value)
    {
        if (Variables.TryGetValue(variable, out (ExpType, Expression) variableValue))
            Variables[variable] = (variableValue.Item1, value);
    }

    public List<string> GetFunctionArgs(string function) 
    => Functions.TryGetValue(function, out (List<string>, Expression, IContext) func) ? func.Item1 : 
        parent?.GetFunctionArgs(function);

    public ExpType GetVariableType(string variable) 
    => Variables.TryGetValue(variable, out (ExpType, Expression) variableValue) ? variableValue.Item1 : 
        parent is not null ? parent.GetVariableType(variable) : ExpType.NotSet;

    public Expression GetVariableValue(string variable) 
    => Variables.TryGetValue(variable, out (ExpType, Expression) variableValue) ? variableValue.Item2 : 
        parent is not null ? parent.GetVariableValue(variable) : null;

    public Expression GetFunctionBody(string function, int args) 
    => Functions.TryGetValue(function, out (List<string>, Expression, IContext) functionBody) && functionBody.Item1.Count == args ? functionBody.Item2 : 
        parent is not null ? parent.GetFunctionBody(function, args) : null;

    public IContext GetInnerContext(string function, int args) 
    => Functions.TryGetValue(function, out (List<string>, Expression, IContext) functionBody) && functionBody.Item1.Count == args ? functionBody.Item3 : 
        parent is not null ? parent.GetInnerContext(function, args) : null;

    public IContext CreateChildContext() => new Context(this);

    public void Merge(IContext other)
    {
        foreach (var item in other.Variables)
            if (!Variables.ContainsKey(item.Key))
                Variables[item.Key] = item.Value;

        foreach (var item in other.Functions)
            if (!Functions.ContainsKey(item.Key))
                Functions[item.Key] = item.Value;
    }
}
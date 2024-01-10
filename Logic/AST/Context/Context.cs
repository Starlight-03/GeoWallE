using System;
using System.Collections.Generic;

public class Context : IContext
{
    private readonly IContext parent;

    public Dictionary<string, (ExpType, Expression)> Variables { get; private set; } = new();

    public Dictionary<(string, int), (ExpType, Expression)> Functions { get; private set; } = new();

    public Context()
    {
        Variables["PI"] = (ExpType.Number, new Number(MathF.PI.ToString()));
        Variables["E"] = (ExpType.Number, new Number(MathF.E.ToString()));
        Functions[("sin", 1)] = (ExpType.Number, new Sin());
        Functions[("cos", 1)] = (ExpType.Number, new Cos());
        Functions[("log", 2)] = (ExpType.Number, new Log());
        Functions[("ln", 1)] = (ExpType.Number, new Ln());
        Functions[("intersect", 2)] = (ExpType.Sequence, new Intersect());
        Functions[("points", 1)] = (ExpType.Sequence, new Points());
        Functions[("samples", 0)] = (ExpType.Sequence, new Samples());
        Functions[("randoms", 0)] = (ExpType.Sequence, new Randoms());
    }

    public Context(IContext parent) => this.parent = parent;

    public bool VariableIsDefined(string variable, out (ExpType, Expression) variableValue) 
    => Variables.TryGetValue(variable, out variableValue) || (parent != null && parent.VariableIsDefined(variable, out variableValue));

    public bool FunctionIsDefined(string function, int args, out (ExpType, Expression) functionBody) 
    => Functions.TryGetValue((function, args), out functionBody) 
        || parent != null && parent.FunctionIsDefined(function, args, out functionBody);

    public void DefineVariable(string variable, ExpType type, Expression value = null) 
    => Variables.Add(variable, (type, value));

    public void DefineFunction(string function, int args, ExpType type, Expression body)
    {
        if (!FunctionIsDefined(function, args, out (ExpType, Expression) functionBody))
            Functions[(function, args)] = (type, body);
    }

    public void SetFunctionType(string function, int args, ExpType type) 
    {
        if (FunctionIsDefined(function, args, out (ExpType, Expression) functionBody))
            functionBody.Item1 = type;
    }

    public void SetVariableValue(string variable, Expression value)
    {
        if (VariableIsDefined(variable, out (ExpType, Expression) variableValue))
            variableValue.Item2 = value;
    }

    public Expression GetVariableValue(string variable) 
    => VariableIsDefined(variable, out (ExpType, Expression) variableValue) ? variableValue.Item2 : null;

    public Expression GetFunctionBody(string function, int args) 
    => FunctionIsDefined(function, args, out (ExpType, Expression) functionBody) ? functionBody.Item2 : null;
    
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
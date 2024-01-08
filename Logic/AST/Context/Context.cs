using System;
using System.Collections.Generic;

public class Context : IContext
{
    private readonly IContext parent;

    private readonly Dictionary<string, (ExpType, Expression)> variables = new();

    private readonly Dictionary<(string, int), (ExpType, Expression)> functions = new();

    public Context()
    {
        variables["PI"] = (ExpType.Number, new Number(MathF.PI.ToString()));
        variables["E"] = (ExpType.Number, new Number(MathF.E.ToString()));
        functions[("sin", 1)] = (ExpType.Number, new Sin());
        functions[("cos", 1)] = (ExpType.Number, new Cos());
        functions[("log", 2)] = (ExpType.Number, new Log());
        functions[("ln", 1)] = (ExpType.Number, new Ln());
    }

    public Context(IContext parent) => this.parent = parent;

    public bool VariableIsDefined(string variable, out (ExpType, Expression) variableValue) 
    => variables.TryGetValue(variable, out variableValue) || (parent != null && parent.VariableIsDefined(variable, out variableValue));

    public bool FunctionIsDefined(string function, int args, out (ExpType, Expression) functionBody) 
    => functions.TryGetValue((function, args), out functionBody) 
        || parent != null && parent.FunctionIsDefined(function, args, out functionBody);

    public void DefineVariable(string variable, ExpType type, Expression value = null) 
    => variables.Add(variable, (type, value));

    public void DefineFunction(string function, int args, ExpType type, Expression body)
    {
        if (!FunctionIsDefined(function, args, out (ExpType, Expression) functionBody))
            functions[(function, args)] = (type, body);
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
}
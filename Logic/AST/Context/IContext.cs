using System.Collections.Generic;

public interface IContext
{
    public Dictionary<string, (ExpType, Expression)> Variables { get; }

    public Dictionary<string, (List<string>, Expression, IContext)> Functions { get; }

    bool VariableIsDefined(string variable);

    bool FunctionIsDefined(string function, int args);

    void DefineVariable(string variable, ExpType type = ExpType.NotSet, Expression value = null);

    void DefineFunction(string function, List<string> args, Expression body);

    void SetVariableType(string variable, ExpType type);

    void SetVariableValue(string variable, Expression value);

    List<string> GetFunctionArgs(string function);

    ExpType GetVariableType(string variable);

    Expression GetVariableValue(string variable);

    Expression GetFunctionBody(string function, int args);

    IContext GetInnerContext(string function, int args);

    IContext CreateChildContext();

    void Merge(IContext other);
}
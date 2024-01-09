public interface IContext
{
    bool VariableIsDefined(string variable, out (ExpType, Expression) variableValue);

    bool FunctionIsDefined(string function, int args, out (ExpType, Expression) functionBody);

    void DefineVariable(string variable, ExpType type, Expression value = null);

    void DefineFunction(string function, int args, ExpType type, Expression body);

    void SetFunctionType(string function, int args, ExpType type);

    void SetVariableValue(string variable, Expression value);

    Expression GetVariableValue(string variable);

    Expression GetFunctionBody(string function, int args);

    IContext CreateChildContext();
}
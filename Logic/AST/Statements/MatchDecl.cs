using System;
using System.Collections.Generic;

public class MatchDecl : Statement
{
    private readonly List<string> variables;

    private readonly Expression arg;

    private IContext context;

    public MatchDecl(List<string> variables, Expression arg, int line) : base(line)
    {
        this.variables = variables;
        this.arg = arg;
    }

    public override bool Validate(IContext context)
    {
        this.context = context;

        if (!arg.Validate(context))
            AddError("");

        foreach (string variable in variables){
            if (context.VariableIsDefined(variable, out (ExpType, Expression) variableValue))
                AddError("");
            else{
                if (variable is "_") continue;
                else context.DefineVariable(variable, arg.Type);
            }
        }
        
        return IsValid();
    }

    public override void Evaluate()
    {
        arg.Evaluate();
        if (arg is Sequence seq){
            int i = 0;
            for (; i < MathF.Min(variables.Count, seq.Count); i++){
                if (variables[i] is "_") continue;
                else context.SetVariableValue(variables[i], seq[i]);
            }
            while (i < variables.Count){
                if (variables[i] is "_") continue;
                else context.SetVariableValue(variables[i++], new Undefined());
            }
        }
        else if (arg is Undefined){
            foreach (string variable in variables){
                if (variable is "_") continue;
                else context.SetVariableValue(variable, new Undefined());
            }
        }
    }
}
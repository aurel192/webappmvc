using System;
using System.Collections.Generic;
using System.Text;
using Flee.CalcEngine.PublicTypes;
using Flee.PublicTypes;
using System.Linq.Expressions;
using static HelperClassLib.Helpers.HelperClass;

namespace HelperClassLib
{
    public static class MultipleConditions
    {
        public static bool Evaluate(Dictionary<string, bool> dict, string condition)
        {
            try
            {
                ExpressionContext context = new ExpressionContext();
                VariableCollection variables = context.Variables;
                foreach(var v in dict)
                    variables.Add(v.Key, v.Value);
                IGenericExpression<bool> e = context.CompileGeneric<bool>(condition);
                bool result = e.Evaluate();
                return result;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitAndMerge
{
    class NameVar
    {
        public string modbus;
        public string subsystem;
        public string name;

        public NameVar(string fullName)
        {
            if (!fullName.Contains(":"))
            {
                throw new ArgumentException("Format names subs[.modbus]:namevariable " + name);
            }
            if (fullName.Contains("."))
            {
                int p = fullName.IndexOf(".");
                subsystem = fullName.Substring(0,p);
                modbus= fullName.Substring(p+1, fullName.IndexOf(":"));
                name=fullName.Substring(name.IndexOf(":") + 1);
                return;
            }
            subsystem = fullName.Substring(0, fullName.IndexOf(":"));
            modbus = null;
            name = fullName.Substring(fullName.IndexOf(":") + 1);
        }
    }
    class InputFunction : ParserFunction
    {
        
        protected override Parser.Result Evaluate(string data, ref int from)
        {
            string varName = Utils.GetToken(data, ref from, Constants.END_ARG_ARRAY);
            if (from >= data.Length)
            {
                throw new ArgumentException("Couldn't set variable before end of line");
            }
            NameVar nv = new NameVar(varName);
            Parser.Result varValue = new Parser.Result();
            string value=ScDumper.GetValue(nv);
            varValue.Value = Double.Parse(value);
            // Check if the variable to be set has the form of x(0),
            // meaning that this is an array element.
            int arrayIndex = Utils.ExtractArrayElement(ref varName);
            if (arrayIndex >= 0)
            {
                bool exists = ParserFunction.FunctionExists(varName);
                Parser.Result currentValue = exists ?
                      ParserFunction.GetFunction(varName).GetValue(data, ref from) :
                      new Parser.Result();

                List<Parser.Result> tuple = currentValue.Tuple ?? new List<Parser.Result>();
                if (tuple.Count > arrayIndex)
                {
                    tuple[arrayIndex] = varValue;
                }
                else
                {
                    for (int i = tuple.Count; i < arrayIndex; i++)
                    {
                        tuple.Add(new Parser.Result(Double.NaN, string.Empty));
                    }
                    tuple.Add(varValue);
                }

                varValue = new Parser.Result(Double.NaN, null, tuple);
            }

            ParserFunction.AddFunction(varName, new GetVarFunction(varValue));

            return new Parser.Result(Double.NaN, varName);
        }
    }
    class UseFunction : ParserFunction
    {
        protected override Parser.Result Evaluate(string data, ref int from)
        {
            string useName = Utils.GetToken(data, ref from, Constants.END_ARG_ARRAY);
            ScDumper.AddDumper(useName);
            return new Parser.Result(Double.NaN, "");

        }
    }
    class CloseFunction : ParserFunction
    {
        protected override Parser.Result Evaluate(string data, ref int from)
        {
            string useName = Utils.GetToken(data, ref from, Constants.END_ARG_ARRAY);
            ScDumper.Close(useName);
            return new Parser.Result(Double.NaN, "");

        }
    }
    class OutputFunction : ParserFunction
    {
        protected override Parser.Result Evaluate(string data, ref int from)
        {
            string varName = Utils.GetToken(data, ref from, Constants.END_ARG_ARRAY);
            if (from >= data.Length)
            {
                throw new ArgumentException("Couldn't set variable before end of line");
            }
            NameVar nv = new NameVar(varName);
            Parser.Result varValue = new Parser.Result();
            ScDumper.SetValue(nv,varValue.Value.ToString());
            return varValue;
        }
    }

}

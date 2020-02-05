using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitAndMerge
{
    public class Constants
    {
        public const char START_ARG = '(';
        public const char END_ARG = ')';
        public const char END_LINE = '\n';
        public const char NEXT_ARG = ',';
        public const char QUOTE = '"';
        public const char START_GROUP = '{';
        public const char END_GROUP = '}';
        public const char END_STATEMENT = ';';
        public const char VAR_PREFIX = '$';

        public const string IF = "if";
        public const string ELSE = "else";
        public const string ELSE_IF = "elif";
        public const string WHILE = "while";
        public const string BREAK = "break";
        public const string CONTINUE = "continue";
        public const string COMMENT = "//";

        public const string ABS = "abs";
        public const string APPEND = "append";
        public const string CD = "cd";
        public const string CD__ = "cd..";
        public const string DIR = "dir";
        public const string ENV = "env";
        public const string EXP = "exp";
        public const string FINDFILES = "findfiles";
        public const string FINDSTR = "findstr";
        public const string INDEX_OF = "indexof";
        public const string KILL = "kill";
        public const string PI = "pi";
        public const string POW = "pow";
        public const string PRINT = "print";
        public const string PSINFO = "psinfo";
        public const string PSTIME = "pstime";
        public const string PWD = "pwd";
        public const string RUN = "run";
        public const string SETENV = "setenv";
        public const string SET = "set";
        public const string SIN = "sin";
        public const string SIZE = "size";
        public const string SQRT = "sqrt";
        public const string SUBSTR = "substr";
        public const string TOLOWER = "tolower";
        public const string TOUPPER = "toupper";
        public const string USE = "use";
        public const string INPUT = "in";
        public const string OUTPUT = "out";
        public const string CLOSE = "close";





        public static char[] NEXT_ARG_ARRAY = NEXT_ARG.ToString().ToCharArray();
        public static char[] END_ARG_ARRAY = END_ARG.ToString().ToCharArray();
        public static char[] END_LINE_ARRAY = END_LINE.ToString().ToCharArray();
        public static char[] QUOTE_ARRAY = QUOTE.ToString().ToCharArray();

        public static char[] COMPARE_ARRAY = "<>=)".ToCharArray();
        public static char[] IF_ARG_ARRAY = "&|)".ToCharArray();
        public static char[] END_PARSE_ARRAY = ";)}\n".ToCharArray();
        public static char[] NEXT_OR_END_ARRAY = { NEXT_ARG, END_ARG, END_STATEMENT };

        public static char[] TOKEN_SEPARATION = ("<>=+-*/&^|!\n\t " + START_ARG + END_ARG +
                             START_GROUP + NEXT_ARG + END_STATEMENT).ToCharArray();

    }
}

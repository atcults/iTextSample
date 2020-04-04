using System;
using org.mariuszgromada.math.mxparser;

namespace C2CPrint
{
    public class FormulaValidator
    {

        public static void ValidateContractFormula()
        {

            var expression = new Expression("P*SP");

            Console.WriteLine(expression.checkSyntax());

            expression.addArguments(new Argument("P", 0));

            Console.WriteLine(expression.checkSyntax());

            expression.addArguments(new Argument("SP", 0));

            Console.WriteLine(expression.checkSyntax());

            var e = new Expression();
            e.addFunctions(new Function());
            e.addArguments(new Argument("FL", 0));
            e.addArguments(new Argument("FSR", 0));
            e.addArguments(new Argument("FU", 0));
            e.addArguments(new Argument("FWNP", 0));
            e.addArguments(new Argument("FS", 0));
            e.addArguments(new Argument("FAA", 0));
            e.addArguments(new Argument("FM", 0));
            e.addArguments(new Argument("FGM", 0));
            e.addArguments(new Argument("FC", 0));
            e.addArguments(new Argument("FMR", 0));
            e.addArguments(new Argument("FMNCFHO", 0));
            e.addArguments(new Argument("CP", 0));
            e.addArguments(new Argument("IC", 0));
            e.addArguments(new Argument("SA", 0));
            e.addArguments(new Argument("MTBC", 0));
            e.addArguments(new Argument("M", 0));
            e.addArguments(new Argument("WA", 0));
            e.addArguments(new Argument("P", 0));

            e.setExpressionString("FL+10");

            Console.WriteLine(e.checkSyntax());

            Console.ReadLine();


        }

    }
}

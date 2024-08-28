

using System.Collections.Generic;
using System.Linq;

namespace Lox_CSharp.CraftingInterpreters
{

    internal abstract class Expr
    {
        internal class Binary : Expr
        {
            internal readonly Expr left;
            internal readonly Token oprator;
            internal readonly Expr right;

            internal Binary(Expr left, Token oprator, Expr right)
            {
                this.left = left;
                this.oprator = oprator;
                this.right = right;
            }
        }
    }

}

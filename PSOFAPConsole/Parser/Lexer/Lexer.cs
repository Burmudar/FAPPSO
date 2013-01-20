using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.Parser.Lexer
{
    class Lexer
    {
        public LinkedList<Token> Tokens { get; set; }

        public Scanner Scanner { get; set; }

        public Lexer(String path)
        {
            if(path.EndsWith(".ass"))
            {
                Scanner = new Scanner(path,Scanner.MODE.ASSIGNMENT);
            }
            else
            {
                Scanner = new Scanner(path,Scanner.MODE.SCENARIO);
            }
            Tokens = new LinkedList<Token>();
        }

        public void Lexify()
        {
            Token token = Scanner.NextToken();
            while (token.Tag != Tag.END)
            {
                Tokens.AddLast(token);
                token = Scanner.NextToken();
            }
            Scanner.Close();
        }

    }
}

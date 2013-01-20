using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.Parser.Lexer;

namespace PSOFAPConsole.FAP
{
    class FAPFileVerifier
    {
        private LinkedListNode<Token> TokenNode { get; set; }
        private const String ASSIGNMENT_FORMAT = "ASSIGNMENT";
        private const String SCENARIO_FORMAT = "SCENARIO";

        public FAPFileVerifier(LinkedListNode<Token> tokenNode)
        {
            TokenNode = tokenNode;
        }

        public bool IsScenarioFormat()
        {
            return ValidFormatType(SCENARIO_FORMAT);
        }

        public bool IsAssignmentFormat()
        {
            return ValidFormatType(ASSIGNMENT_FORMAT);
        }

        public bool ValidScenarioID(String scenarioID)
        {
            TokenNode = TokenNode.Next;
            while (TokenNode.Value.Tag != Tag.GENERAL_INFORMATION_END)
            {
                Token token = TokenNode.Value;
                switch (TokenNode.Value.Tag)
                {
                    case Tag.SCENARIO_ID:
                        {
                            if (((StringBlock)token).value.Equals(scenarioID))
                                return true;
                            else
                                return false;
                        }
                }
            }
            return false;
        }

        private bool ValidFormatType(String typeToCheckFor)
        {
            typeToCheckFor.ToUpper();
            while (TokenNode.Value.Tag != Tag.FORMAT_END)
            {
                Token token = TokenNode.Value;
                switch (TokenNode.Value.Tag)
                {
                    case Tag.FORMAT_TYPE:
                        {
                            if (((StringBlock)token).value.Equals(typeToCheckFor))
                                return true;
                            else
                                return false;
                        }
                    default: TokenNode = TokenNode.Next;
                        break;
                }
            }
            return false;
        }
    }
}

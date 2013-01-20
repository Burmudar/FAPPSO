using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.Parser.Lexer;
using PSOFAPConsole.FAP.Interfaces;
using PSOFAPConsole.FAP.Exceptions;

namespace PSOFAPConsole.FAP
{
    class FAPTestLoader
    {
        private LinkedListNode<Token> TokenNode { get; set; }
        private String ScenarioID { get; set; }
        public FAPTestLoader(LinkedListNode<Token> tokenNode,String problemScenarioID)
        {
            TokenNode = tokenNode;
            ScenarioID = problemScenarioID;
        }

        public void LoadDataIntoCells(ICell[] cells)
        {
            while (TokenNode != null)
            {
                switch (TokenNode.Value.Tag)
                {
                    case Tag.FORMAT_START:
                        {
                            FAPFileVerifier verifier = new FAPFileVerifier(TokenNode);
                            if (verifier.IsAssignmentFormat() == false)
                            {
                                throw new InvalidFAPFileException("The Format type of the given assignment file is not of an \"ASSIGNMENT\" type");
                            }
                        }
                        break;
                    case Tag.GENERATION_INFORMATION_START:
                        {
                            FAPFileVerifier verifier = new FAPFileVerifier(TokenNode);
                            if (verifier.ValidScenarioID(ScenarioID) == false)
                            {
                                throw new InvalidScenarioIDException("The given assignment file scenario id doesn't match the problem file scenario id");
                            }
                        }
                        break;
                    case Tag.CELLS_START: FillCells(cells);
                        break;
                }
                TokenNode = TokenNode.Next;
            }
        }

        private void FillCells(ICell[] cells)
        {
            TokenNode = TokenNode.Next;
            ICell cell = null;
            int index = 0;
            int freqIndex = 0;
            while (TokenNode.Value.Tag != Tag.CELLS_END)
            {
                Token token = TokenNode.Value;
                switch (TokenNode.Value.Tag)
                {
                    case Tag.CELL_ID:
                        {
                            cell = cells[index];
                            if (((Num)token).Value.ToString().Equals(cell.CellID) == false)
                            {
                                System.Console.WriteLine("Assignment incosistency: Must assign frequency to Cell with ID: {0} but found {1}", ((Num)token).Value.ToString(), cell.CellID);
                            }
                            freqIndex = 0;
                        }
                        break;
                    case Tag.CELL_ASSIGNMENT:
                        {
                            cell.FrequencyHandler[freqIndex] = GetFrequency(((StringBlock)token).value);
                            freqIndex++;
                        }
                        break;
                    case Tag.CELL_INNER_END:
                        {
                            index++;
                        }
                        break;
                }
                TokenNode = TokenNode.Next;
            }
        }

        private int GetFrequency(String assignment)
        {
            assignment = assignment.Substring(assignment.IndexOf('(') + 1, assignment.IndexOf(')') - 1).Trim();
            String[] asg = assignment.Split(',');
            return int.Parse(asg[0]);
        }
    }
}

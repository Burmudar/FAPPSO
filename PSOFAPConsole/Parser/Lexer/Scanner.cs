using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PSOFAP.Parser.Lexer
{
    public class Scanner
    {
        public enum Section
        {
            BASE, FORMAT, GENERAL_INFORMATION, CELLS, CELLS_INNER, CELL_RELATION, CELL_RELATION_INNER, END
        }

        public enum MODE
        {
            ASSIGNMENT,SCENARIO
        }

        public Section FileSection { get; set; }
        public StreamReader Reader { get; set; }
        private Token oldToken;
        private Token token;
        private MODE mode;

        public Scanner(String filename,MODE mode)
        {
            Reader = new StreamReader(filename);
            FileSection = Section.BASE;
            this.mode = mode;
        }

        public Token NextToken()
        {
            oldToken = token;
            switch(FileSection)
            {
                case Section.FORMAT : token = NextTokenInFormat();
                    break;
                case Section.GENERAL_INFORMATION: token = NextTokenInGeneralInformation();
                    break;
                case Section.CELLS: token = NextTokenInCells();
                    break;
                case Section.CELLS_INNER:
                    {
                        if (mode == MODE.SCENARIO)
                        {
                            token = NextTokenInInnerCells();
                        }
                        else
                        {
                            token = NextTokenInInnerAssignCells();
                        }
                    }
                    break;
                case Section.CELL_RELATION: token = NextTokenInCellRelation();
                    break;
                case Section.CELL_RELATION_INNER: token = NextTokenInInnerCellRelation();
                    break;
                case Section.BASE:
                    {
                        NextFileSection();
                    }
                    break;
                case Section.END: token = new Token(Tag.END);
                    break;
            }
            return token;
        }

        private void NextFileSection()
        {
            if(Reader.EndOfStream)
            {
                FileSection = Section.END;
                return;
            }
            String line = "";
            while (line.Length <= 0)
            {
                line = Reader.ReadLine().Trim();
            }
            String[] words = line.Split(' ');
            
            foreach(String word in words)
            {
                switch (word)
                {
                    case "FORMAT":
                        {
                            FileSection = Section.FORMAT;
                            this.token = new Token(Tag.FORMAT_START);
                        }
                        break;
                    case "GENERAL_INFORMATION":
                        {
                            FileSection = Section.GENERAL_INFORMATION;
                            this.token = new Token(Tag.GENERATION_INFORMATION_START);

                        }
                        break;
                    case "CELLS":
                        {
                            FileSection = Section.CELLS;
                            this.token = new Token(Tag.CELLS_START);

                        }
                        break;
                    case "CELL_RELATIONS":
                        {
                            FileSection = Section.CELL_RELATION;
                            this.token = new Token(Tag.CELL_RELATION_START);

                        }
                        break;
                }
            }
        }

        private Token NextTokenInFormat()
        {
            String word = ReadToken();
            switch (word)
            {
                case "TYPE": return new StringBlock(Tag.FORMAT_TYPE, ReadToken());
                case "VERSION": return new StringBlock(Tag.FORMAT_VERSION,ReadToken());
                case "}":
                    {
                        FileSection = Section.BASE;
                        return new Token(Tag.FORMAT_END);
                    }
            }
            return null;

        }

        private Token NextTokenInGeneralInformation()
        {
            String word = ReadToken();
            switch (word)
            {
                case "SCENARIO_ID": return new StringBlock(Tag.SCENARIO_ID, ReadToken());
                case "NAME": return new StringBlock(Tag.NAME,ReadLine('|'));
                case "ANNOTATION": return new StringBlock(Tag.ANNOTATION,ReadLine('|'));
                case "NETWORK_TYPE": return new StringBlock(Tag.NETWORK_TYPE, ReadToken());
                case "SPECTRUM": return new StringBlock(Tag.SPECTRUM, ReadToken());
                case "GLOBALLY_BLOCKED_CHANNELS": return new NumArray(Tag.GLOBALLY_BLOCKED_CHANNELS, ReadLine(),' ');
                case "CO_SITE_SEPARATION": return new Num(Tag.CO_SITE_SEPARATION, ReadToken());
                case "DEFAULT_CO_CELL_SEPARATION": return new Num(Tag.DEFAULT_CO_CELL_SEPARATION, ReadToken());
                case "HANDOVER_SEPARATION": return new NumArray(Tag.HANDOVER_SEPARATION, ReadLine(), ' ');
                case "MINIMAL_SIGNIFICANT_INTERFERENCE": return new Real(Tag.MINIMAL_SIGNIFICANT_INTERFERENCE, ReadToken());
                case "DEMAND_MODEL": return new StringBlock(Tag.DEMAND_MODEL, ReadToken());
                case "SITE_LOCATIONS": return new Num(Tag.SITE_LOCATIONS, ReadToken());
                case "}":
                    {
                        FileSection = Section.BASE;
                        return new Token(Tag.GENERAL_INFORMATION_END);
                    }
            }
            return null;
        }

        private Token NextTokenInCells()
        {
            String token = ReadToken();
            switch (token)
            {
                case "}":
                    {
                        FileSection = Section.BASE;
                        return new Token(Tag.CELLS_END);
                    }
                default:
                    {
                        Token num = new Num(Tag.CELL_ID, token);
                        if (ReadToken() == "{")
                        {
                            FileSection = Section.CELLS_INNER;
                        }
                        
                        return num;
                    }
            }
        }

        private Token NextTokenInInnerCells()
        {
            switch (oldToken.Tag)
            {
                case Tag.CELL_ID: return new StringBlock(Tag.CELL_SITENAME, ReadToken());
                case Tag.CELL_SITENAME: return new Num(Tag.CELL_SECTOR, ReadToken());
                case Tag.CELL_SECTOR: return new Num(Tag.CELL_TRAFFIC, ReadToken());
                default:
                    {
                        String word = ReadToken();
                        switch (word)
                        {
                            case "LOC": return new StringBlock(Tag.CELL_LOCATION, ReadToken());
                            case "LBC": return new NumArray(Tag.CELL_LOCALLY_BLOCKED, ReadLine(), ' ');
                            case "CCS": return new Num(Tag.CELL_CO_SITE_SEPARATION, ReadToken());
                            case "}":
                                {
                                    FileSection = Section.CELLS;
                                    return new Token(Tag.CELL_INNER_END);
                                }
                            default:
                                {
                                    return null;
                                }
                        }
                    }
            }
        }

        private Token NextTokenInInnerAssignCells()
        {
            String word = ReadToken();
            switch (word)
            {
                case "}":
                    {
                        FileSection = Section.CELLS;
                        return new Token(Tag.CELL_INNER_END);
                    }
                default: return new StringBlock(Tag.CELL_ASSIGNMENT, word);
            }
        }


        private Token NextTokenInCellRelation()
        {
            String token = ReadToken();
            switch (token)
            {
                case "}":
                    {
                        FileSection = Section.BASE;
                        return new Token(Tag.CELL_RELATION_END);
                    }
                default:
                    {
                        String line = token + " " + ReadUntil('{',false).Trim(); ;
                        Token num = new NumArray(Tag.CELL_RELATION_IDS, line,' ');
                        if (ReadToken() == "{")
                        {
                            FileSection = Section.CELL_RELATION_INNER;
                        }

                        return num;
                    }
            }
        }

        private Token NextTokenInInnerCellRelation()
        {
            String token = ReadToken();
            switch (token)
            {
                case "H": return new Num(Tag.CELLRELATION_HANDOVER, ReadToken());
                case "S": return new Num(Tag.CELLRELATION_SEPARATION, ReadToken());
                case "DA": return new RealArray(Tag.DA, ReadLine(), ' ');
                case "UA": return new RealArray(Tag.UA, ReadLine(), ' ');
                case "DT": return new RealArray(Tag.DT, ReadLine(), ' ');
                case "UT": return new RealArray(Tag.UT, ReadLine(), ' ');
                case "}":
                    {
                        FileSection = Section.CELL_RELATION;
                        return new Token(Tag.CELL_RELATION_INNER_END);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private String[] GetSplittedLine()
        {
            return Reader.ReadLine().Trim().Split(' ');
        }

        private Boolean AtEndOfSection()
        {
            return Reader.Peek().Equals('}');
        }

        public String ReadToken()
        {
            StringBuilder builder = new StringBuilder();

            char readChar = (char)Reader.Peek();
            Boolean proceed = false;
            while (!proceed && readChar != '}')
            {
                proceed = canRead();
            }

            while (!Reader.EndOfStream)
            {
                readChar = (char)Reader.Read();
                if (isBracket(readChar))
                {
                    builder.Append(readChar);
                    builder.Append(ReadUntil(getOppositeBracket(readChar),true));
                    continue;
                }
                if (readChar == ';' || Char.IsWhiteSpace(readChar))
                {
                    break;
                }
                else
                {
                    builder.Append(readChar);
                }
            }
            return builder.ToString();
        }

        private Boolean canRead()
        {
            char peekChar = (char)Reader.Peek();
            if (Char.IsWhiteSpace(peekChar) || peekChar == ';')
            {
                Reader.Read();
                return false;
            }
            return true;
        }

        public String ReadUntil(char endChar,Boolean inclusive)
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                char character = (char)Reader.Read();
                if (character != ' ' || character != ';')
                {
                    builder.Append(character);
                    char peekChar = (char)Reader.Peek();
                    if (peekChar == endChar)
                    {
                        if (inclusive)
                        {
                            builder.Append((char)Reader.Read());
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return builder.ToString();
        }

        public String ReadLine(char avoid)
        {
            String line = Reader.ReadLine().Replace(avoid,' ').Trim();
            line = line.Replace(';', ' ').TrimEnd();
            return line;
        }

        public String ReadLine()
        {
            String line = Reader.ReadLine().Replace(';',' ').Trim();
            return line;
        }

        private char getOppositeBracket(char bracket)
        {
            switch (bracket)
            {
                case '(' : return ')';
                case '[' : return ']';
                case '{' : return '}';
            }
            return bracket;
        }

        private Boolean isBracket(char bracket)
        {
            switch (bracket)
            {
                case '(': return true;
                case ')': return true;
                case '[': return true;
                case ']': return true;
            }
            return false;
        }

        public void Close()
        {
            Reader.Close();
        }
    }
}

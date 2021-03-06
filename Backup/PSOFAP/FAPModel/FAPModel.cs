﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSOFAP.Parser.Lexer;
using PSOFAP.FAPModel.Interfaces;

namespace PSOFAP.FAPModel
{
    public class FAPModel
    {
        public String ProblemPath { get; set; }
        public Format Format { get; set; }
        public GeneralInformation GeneralInformation { get; set; }
        public ICell[] Cells { get; set; }
        public LinkedList<ICellRelation> InterferenceMatrix { get; set; }

        public FAPModel(String path)
        {
            ProblemPath = path;
            this.GeneralInformation = new GeneralInformation();
            this.Format = new Format();
            InterferenceMatrix = new LinkedList<ICellRelation>();
        }

        public void createModel()
        {
            LinkedList<Token> tokens = GetTokens();
            LinkedListNode<Token> tokenNode = tokens.First;
            while (tokenNode != null)
            {
                switch (tokenNode.Value.Tag)
                {
                    case Tag.FORMAT_START: CreateFormat(ref tokenNode);
                        break;
                    case Tag.GENERATION_INFORMATION_START: CreateGeneralInformation(ref tokenNode);
                        break;
                    case Tag.CELLS_START: CreateCells(ref tokenNode);
                        break;
                    case Tag.CELL_RELATION_START: CreateCellRelations(ref tokenNode);
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        public void LoadTestData(String path)
        {
            Lexer lexer = new Lexer(path);
            lexer.Lexify();
            LinkedList<Token> tokens = lexer.Tokens;
            LinkedListNode<Token> tokenNode = tokens.First;
            while (tokenNode != null)
            {
                switch (tokenNode.Value.Tag)
                {
                    case Tag.CELLS_START: FillCells(ref tokenNode);
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        private void CreateFormat(ref LinkedListNode<Token> tokenNode)
        {
            tokenNode = tokenNode.Next;
            while (tokenNode.Value.Tag != Tag.FORMAT_END)
            {
                Token token = tokenNode.Value;
                switch (tokenNode.Value.Tag)
                {
                    case Tag.FORMAT_TYPE: this.Format.Type = ((StringBlock)token).value;
                        break;
                    case Tag.FORMAT_VERSION: this.Format.version = ((StringBlock)token).value;
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        private void CreateGeneralInformation(ref LinkedListNode<Token> tokenNode)
        {
            tokenNode = tokenNode.Next;
            while (tokenNode.Value.Tag != Tag.GENERAL_INFORMATION_END)
            {
                Token token = tokenNode.Value;
                switch (tokenNode.Value.Tag)
                {
                    case Tag.SCENARIO_ID: this.GeneralInformation.ScenarioID = ((StringBlock)token).value;
                        break;
                    case Tag.ANNOTATION: this.GeneralInformation.Annotation = ((StringBlock)token).value;
                        break;
                    case Tag.NETWORK_TYPE: this.GeneralInformation.NetworkType = ((StringBlock)token).value;
                        break;
                    case Tag.DEMAND_MODEL: this.GeneralInformation.DemandModel = ((StringBlock)token).value;
                        break;
                    case Tag.SPECTRUM: this.GeneralInformation.Spectrum = CreateSpectrum(((StringBlock)token).value);
                        break;
                    case Tag.GLOBALLY_BLOCKED_CHANNELS: this.GeneralInformation.GloballyBlockedChannels = ((NumArray)token).Value;
                        break;
                    case Tag.DEFAULT_CO_CELL_SEPARATION: this.GeneralInformation.DefaultCoCellSeperation = ((Num)token).Value;
                        break;
                    case Tag.CO_SITE_SEPARATION: this.GeneralInformation.CoSiteSeperation = ((Num)token).Value;
                        break;
                    case Tag.HANDOVER_SEPARATION: this.GeneralInformation.HandoverSeperation = ((NumArray)token).Value;
                        break;
                    case Tag.SITE_LOCATIONS: this.GeneralInformation.SiteLocations = ((Num)token).Value;
                        break;
                    case Tag.MINIMAL_SIGNIFICANT_INTERFERENCE: this.GeneralInformation.MinTolerableInterference = ((Real)token).Value;
                        break;
                    case Tag.MAXIMUM_SIGNIFICANT_INTERFERENCE: this.GeneralInformation.MaxTolerableInterference = ((Real)token).Value;
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        private void CreateCells(ref LinkedListNode<Token> tokenNode)
        {
            tokenNode = tokenNode.Next;
            LinkedList<ICell> Cells = new LinkedList<ICell>();
            Cell cell = null;
            while (tokenNode.Value.Tag != Tag.CELLS_END)
            {
                Token token = tokenNode.Value;
                switch (tokenNode.Value.Tag)
                {
                    case Tag.CELL_ID: cell = new Cell(((Num)token).Value.ToString());
                        break;
                    case Tag.CELL_SITENAME: cell.SiteName = ((StringBlock)token).value;
                        break;
                    case Tag.CELL_SECTOR: cell.Sector = ((Num)token).Value;
                        break;
                    case Tag.CELL_TRAFFIC: cell.Traffic = ((Num)token).Value;
                        break;
                    case Tag.CELL_LOCALLY_BLOCKED: cell.LocallyBlocked = ((NumArray)token).Value;
                        break;
                    case Tag.CELL_LOCATION: cell.Location = CreateLocation(((StringBlock)token).value);
                        break;
                    case Tag.CELL_INNER_END: Cells.AddLast(cell);
                        break;
                }
                tokenNode = tokenNode.Next;
            }
            this.Cells = new ICell[Cells.Count];
            Cells.CopyTo(this.Cells,0);
        }

        private void FillCells(ref LinkedListNode<Token> tokenNode)
        {
            tokenNode = tokenNode.Next;
            ICell cell = null;
            int index = 0;
            int freqIndex = 0;
            while (tokenNode.Value.Tag != Tag.CELLS_END)
            {
                Token token = tokenNode.Value;
                switch (tokenNode.Value.Tag)
                {
                    case Tag.CELL_ID:
                        {
                            cell = Cells[index];
                            if (((Num)token).Value.ToString().Equals(cell.CellID) == false)
                            {
                                System.Console.WriteLine("Assignment incosistency: Must assign frequency to Cell with ID: {0} but found {1}", ((Num)token).Value.ToString(), cell.CellID);
                            }
                            freqIndex = 0;
                        }
                        break;
                    case Tag.CELL_ASSIGNMENT:
                        {
                            cell.setFrequencyAt(freqIndex,GetFrequency(((StringBlock)token).value));
                            freqIndex++;
                        }
                        break;
                    case Tag.CELL_INNER_END:
                        {
                            index++;
                        }
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        private void CreateCellRelations(ref LinkedListNode<Token> tokenNode)
        {
            tokenNode = tokenNode.Next;
            CellRelation cellRelation = null;
            while (tokenNode.Value.Tag != Tag.CELL_RELATION_END)
            {
                Token token = tokenNode.Value;
                switch (tokenNode.Value.Tag)
                {
                    case Tag.CELL_RELATION_IDS: cellRelation = new CellRelation(GetCellIndecis(((NumArray)token).Value));
                        break;
                    case Tag.CELLRELATION_HANDOVER: cellRelation.Handover = ((Num)token).Value;
                        break;
                    case Tag.CELLRELATION_SEPARATION: cellRelation.Separation = ((Num)token).Value;
                        break;
                    case Tag.DA: cellRelation.DA = ((RealArray)token).Value;
                        break;
                    case Tag.UA: cellRelation.UA = ((RealArray)token).Value;
                        break;
                    case Tag.DT: cellRelation.DT = ((RealArray)token).Value;
                        break;
                    case Tag.UT: cellRelation.UT = ((RealArray)token).Value;
                        break;
                    case Tag.CELL_RELATION_INNER_END: InterferenceMatrix.AddLast(cellRelation);
                        break;
                }
                tokenNode = tokenNode.Next;
            }
        }

        private int[] CreateSpectrum(String spectrum)
        {
            spectrum = spectrum.Substring(spectrum.IndexOf('(')+1, spectrum.IndexOf(')')-1).Trim();
            String[] spec = spectrum.Split(',');
            return spec.Select(x => int.Parse(x.Trim())).ToArray();
        }

        private Coordinate CreateLocation(String location)
        {
            location = location.Substring(location.IndexOf('(') + 1, location.IndexOf(')') - 1).Trim();
            String[] loc = location.Split(',');
            float[] coords = loc.Select(x => float.Parse(x.Trim())).ToArray();
            return new Coordinate(coords[0], coords[1]);
        }

        private int GetFrequency(String assignment)
        {
            assignment = assignment.Substring(assignment.IndexOf('(') + 1, assignment.IndexOf(')') - 1).Trim();
            String[] asg = assignment.Split(',');
            return int.Parse(asg[0]);
        }

        private int[] GetCellIndecis(int[] id)
        {
            int[] index = {-1,-1};
            for(int i = 0; i < Cells.Length;i++)
            {
                if (Cells[i].CellID.Equals(id[0].ToString()))
                {
                    index[0] = i;
                }
                if(Cells[i].CellID.Equals(id[1].ToString()))
                {
                    index[1] = i;
                }
                if(index[0] != -1 && index[1] != -1)
                {
                    break;
                }
            }
            return index;
        }   

        private LinkedList<Token> GetTokens()
        {
            Lexer lexer = new Lexer(ProblemPath);
            lexer.Lexify();
            return lexer.Tokens;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using FuzzyLogicWebService.FISFiles.FISModel;

namespace FuzzyLogicWebService.FISFiles
{
    public class FISFileCreator
    {
        StringBuilder fisFileBuilder = new StringBuilder();

        public byte[] writeFisFileFromGivenModel(FISFileContent fisFileContent)
        {
            writeSystemParagraph(fisFileContent.SystemProperties);
            writeVariablesParagraphs(fisFileContent.InputVariables);
            writeVariablesParagraphs(fisFileContent.OutputVariables);
            writeRulesParagraph(fisFileContent.ListOfRules);
            return getBytes(fisFileBuilder.ToString());
        }

        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void writeSystemParagraph(FISSystem systemProperties)
        {
            fisFileBuilder.AppendLine("[System]");
            fisFileBuilder.AppendLine("Name='"+systemProperties.Name+"'");
            fisFileBuilder.AppendLine("Type='mamdani'");
            fisFileBuilder.AppendLine("NumInputs="+systemProperties.InputsNumber);
            fisFileBuilder.AppendLine("NumOutputs="+systemProperties.OutputsNumber);
            fisFileBuilder.AppendLine("NumRules="+systemProperties.RulesNumber);
            fisFileBuilder.AppendLine("AndMethod='"+systemProperties.AndMethod+"'");
            fisFileBuilder.AppendLine("OrMethod='" + systemProperties.OrMethod + "'");
            fisFileBuilder.AppendLine("ImpMethod='" + systemProperties.ImpMethod + "'");
            fisFileBuilder.AppendLine("AggMethod='" + systemProperties.AggMethod + "'");
            fisFileBuilder.AppendLine("DefuzzMethod='" + systemProperties.DefuzzMethod + "'");
            fisFileBuilder.AppendLine();
            
        }

        private void writeVariablesParagraphs(List<FISVariable> variables)
        {
            foreach(FISVariable input in variables){
                fisFileBuilder.AppendLine("["+input.Type+(input.Index)+"]");
                fisFileBuilder.AppendLine("Name='"+input.Name+"'");
                fisFileBuilder.AppendLine("Range=["+input.MinValue+" "+input.MaxValue+"]");
                fisFileBuilder.AppendLine("NumMFs="+input.ListOfMF.Count);
                foreach(FISMembershipFunction function in input.ListOfMF){
                    formatMembershipFunction(function, input.ListOfMF.IndexOf(function));
                }
                fisFileBuilder.AppendLine();

            }
        }

        private void writeRulesParagraph(List<string> listOfRules)
        {
            fisFileBuilder.AppendLine("[Rules]");
            foreach (string rule in listOfRules)
            {
                fisFileBuilder.AppendLine(rule);
            }
        }

        private string saveCreatedFile(StringBuilder fileBuilder)
        {
            string mydocpath = "~/App_Data/TempFile/";
            //Console.WriteLine("This is path "+mydocpath);
            StreamWriter fisFileWriter = new StreamWriter(mydocpath + "/file.fis");
            fisFileWriter.Write(fileBuilder.ToString());
            fisFileWriter.Close();
            return mydocpath + "/file.fis";
        }

        private void formatMembershipFunction(FISMembershipFunction membershipFunction, int sequenceNumber)
        {
            string indicator = "MF" + membershipFunction.Index +"=";
            string name = "'"+membershipFunction.Name+"'";
            string type = "'"+membershipFunction.Type+"'";
            string range = "[";
            foreach(int value in membershipFunction.ListOfCusps){
                range+=value+" ";
            }
            range.TrimEnd();
            range+="]";
            fisFileBuilder.AppendLine(indicator+name+":"+type+","+range);
        }


    }
}
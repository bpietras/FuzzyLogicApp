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

        public string writeFisFileFromGivenModel(FISFileContent fisFileContent)
        {
            writeSystemParagraph(fisFileContent.SystemProperties);
            /*writeVariablesParagraphs(fisFileContent.InputVariables);
            writeVariablesParagraphs(fisFileContent.OutputVariables);
            writeRulesParagraph(fisFileContent.ListOfRules);*/
            return saveCreatedFile(fisFileBuilder);
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
                fisFileBuilder.AppendLine("["+input.Type+(variables.IndexOf(input)+1)+"]");
                fisFileBuilder.AppendLine("Name='"+input.Name+"'");
                fisFileBuilder.AppendLine("Range=["+input.RangeOfValues.MinValue+" "+input.RangeOfValues.MaxValue+"]");
                fisFileBuilder.AppendLine("NumMFs="+input.ListOfMF.Count);
                foreach(MembershipFunction function in input.ListOfMF){
                    formatMembershipFunction(function, input.ListOfMF.IndexOf(function));
                }
                fisFileBuilder.AppendLine();

            }
        }

        private void writeRulesParagraph(List<Rule> listOfRules)
        {
            fisFileBuilder.AppendLine("[Rules]");
            foreach (Rule rule in listOfRules)
            {
                string oneRule = "";
                foreach(int inputValue in rule.inputs)
                {
                    oneRule += inputValue + " ";
                }

                oneRule = oneRule.TrimEnd(' ') + ", ";
                foreach (int outputValue in rule.outputs)
                {
                    oneRule += outputValue + " ";
                }
                oneRule = oneRule.TrimEnd(' ') + ", ";
                oneRule += rule.weight + " : ";

                oneRule += rule.connection;
                fisFileBuilder.AppendLine(oneRule);
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

        private void formatMembershipFunction(MembershipFunction membershipFunction, int sequenceNumber)
        {
            string indicator = "MF" + sequenceNumber+"=";
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

        /*private string saveCreatedFile(string content)
        {
            string mydocpath = HttpContext.Current.Server.MapPath("~/TempFile/");
            StreamWriter fisFileWriter = new StreamWriter(mydocpath + "/file.fis");
            fisFileWriter.Write(content);
            fisFileWriter.Close();
            return mydocpath + "/file.fis";
        }*/


    }
}
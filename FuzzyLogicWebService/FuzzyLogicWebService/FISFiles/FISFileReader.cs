using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FuzzyLogicWebService.FISFiles.FISModel;
using System.IO;

namespace FuzzyLogicWebService.FISFiles
{
    public class FISFileReader
    {
        public FISFileContent readModelFromGivenFisFile(String filePath) 
        {
            FISFileContent fisFileContent = new FISFileContent();
            if(isPathValid(filePath))
            {
                using(StreamReader fisFileReader = new StreamReader(filePath)){
                    fisFileContent.SystemProperties = readSystemParagraph();

                }
            }
            
            return fisFileContent;
        }

        private FISSystem readSystemParagraph()
        {
            FISSystem systemProperties = new FISSystem();

            return systemProperties;
        }

        private bool isPathValid(String filePath)
        {
            if (!filePath.Equals(null) && !filePath.Equals("")) return true;
            else return false;
        }
    }
}
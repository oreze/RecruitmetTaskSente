using System;
using System.Xml;
using RecruitmentTask.Services;

namespace RecruitmentTask
{
    class Program
    {
        private static string PYRAMID_PATH = @"C:\dev\csharp\RecruitmentTask\RecruitmentTask\Resources\piramida.xml";
        private static string TRANSFERS_PATH = @"C:\dev\csharp\RecruitmentTask\RecruitmentTask\Resources\przelewy.xml";
        
        static void Main(string[] args)
        {
            var pyramidStructure = XmlService.ReadXmlFrom(PYRAMID_PATH);
            var transfersStructure = XmlService.ReadXmlFrom(TRANSFERS_PATH);

            var pyramid = new Pyramid(pyramidStructure, transfersStructure);
            
        }
    }
}
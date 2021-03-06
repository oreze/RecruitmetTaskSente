using System;
using System.IO;
using System.Xml;
using RecruitmentTask.Models;
using RecruitmentTask.Services;

namespace RecruitmentTask
{
    class Program
    {
        private static string PYRAMID_PATH = @"Resources\piramida.xml";
        private static string TRANSFERS_PATH = @"Resources\przelewy.xml";
        
        static void Main(string[] args)
        {
            InitializePyramid().PrintUsersDataInOrder();
        }

        private static Pyramid InitializePyramid()
        {
            var pyramidPath = Path.Combine(GetProjectDirectory(), PYRAMID_PATH);
            var transfersPath = Path.Combine(GetProjectDirectory(), TRANSFERS_PATH);
            
            var pyramidStructure = XmlService.ReadXmlFrom(pyramidPath);
            var transfersStructure = XmlService.ReadXmlFrom(transfersPath);

            return new Pyramid(pyramidStructure, transfersStructure);
        }

        private static string GetProjectDirectory()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        }
    }
}
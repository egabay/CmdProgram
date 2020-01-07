using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CmdCommands
{
    static class Program
    {
        private static string _currentPosition = "c/";
        private const string _lsCommand = "ls";
        private const string _lsBackCommand = "cd..";
        private const string _cdCommand = "cd";
        private const string _makeFileCommand = "makefile";
        private const string _makeDirCommand = "makedir";
        private static Dictionary<string, string> FilesDictionary { get; } = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(_currentPosition.Replace('/', '>'));
                CheckCommand(Console.ReadLine());
            }
        }

        private static void CheckCommand(string command)
        {
            var allCommands = command.Split(' ').ToList();
            allCommands.RemoveAll(x => x.Equals(""));

            if (allCommands.Count == 1)
            {
                switch (allCommands[0].ToLower())
                {
                    case _lsCommand:
                        LsCommand();
                        return;
                    case _lsBackCommand:
                        CdBackCommand();
                        return;
                }
            }

            else if (allCommands.Count > 1)
            {
                allCommands.RemoveRange(2, allCommands.Count - 2);
                switch (allCommands[0].ToLower())
                {
                    case _makeFileCommand:
                        MakeCommand(allCommands[1]);
                        return;
                    case _makeDirCommand:
                        MakeCommand(allCommands[1] + "/");
                        return;
                    case _cdCommand:
                        CdCommand(allCommands[1]);
                        return;
                }
            }
        }

        private static void LsCommand()
        {
            var filesUnderDir = FilesDictionary.Where(x => x.Value == _currentPosition);
            foreach (var keyValuePair in filesUnderDir)
            {
                Console.WriteLine(keyValuePair.Key);
            }
        }

        private static void MakeCommand(string fileName)
        {
            try
            {
                FilesDictionary.Add(fileName, _currentPosition);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CdCommand(string fileName)
        {
            fileName += "/";
            var dirIsExists = FilesDictionary.ContainsValue(fileName);

            if (dirIsExists || FilesDictionary.ContainsKey(fileName))
            {
                _currentPosition = fileName;
            }
        }

        private static void CdBackCommand()
        {
            var backDirectory = FilesDictionary.FirstOrDefault(x => x.Key == _currentPosition).Value;
            if (backDirectory != null)
            {
                _currentPosition = backDirectory;
            }
        }
    }
}

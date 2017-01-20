using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeplinkUtil
{
    public class writer
    {
        printUtil _pr = new printUtil();
        public void writeScripts(string scripts, string baseDirectory)
        {
            string newPath = baseDirectory + getDirec(baseDirectory);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            newPath += getFilepath();
            if (!File.Exists(newPath))
            {
                File.CreateText(newPath).Close();                
            }
            using(StreamWriter write = new StreamWriter(newPath))
            {
                write.WriteLine(scripts);
                write.Close();
            }
        }
        public string getDirec(string baseDirectory)
        {
            _pr.write(_pr._br + "The base directory is " + baseDirectory + "." + _pr._br + "Please enter the directory you'd like to place the file in, it can be a new directory. formatted as such...\\documents\\files\\webDirectory\\", _pr._wht);
            string newDirec = _pr.rl(_pr._br + "New Directory: ", _pr._drkGray, _pr._mgnta).Trim();
            if(String.IsNullOrEmpty(newDirec))
            {
                return getDirec(baseDirectory);
            }
            else
            {
                char[] direcAsArray = newDirec.ToCharArray();
                if(direcAsArray[0] != '\\')
                {
                    newDirec = newDirec.Insert(0, "\\");
                }
                direcAsArray = newDirec.ToCharArray();
                if (direcAsArray.Last() != '\\')
                {
                    newDirec += "\\";
                }
                return newDirec;
            }
        }
        public string getFilepath()
        {
            _pr.write(_pr._br + "Enter a name for the file that will be created.", _pr._gray);
            string newFile = _pr.rl(_pr._br + "(If left blank, press enter and default filename will be deeplinks.js)" + _pr._br + "Name: ", _pr._drkGray, _pr._mgnta).Trim();
            if(String.IsNullOrEmpty(newFile))
            {
                newFile = "deeplinks.js";
            }
            string[] filenameTest = newFile.Split('.');
            
            if(filenameTest.Last() != "js")
            {
                newFile += ".js";
            }
            return newFile;
        }
    }
}

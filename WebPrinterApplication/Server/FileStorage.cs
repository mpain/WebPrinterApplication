using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace WebPrinterApplication.Server
{
    class FileStorage
    {
        private String path;
        private Hashtable map = new Hashtable();

        public String GetFile(String fileName)
        {
            return map[fileName] as String;
        }
        
        public FileStorage(String path, String[] fileNames)
        {
            this.path = path;
            foreach (String fileName in fileNames) {
                map.Add(fileName, LoadFile(fileName));
            }
        }

        private String LoadFile(String fileName)
        {

            String filePath = Path.Combine(path, fileName);
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                return streamReader.ReadToEnd();
            }
        }


    }
}

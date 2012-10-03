using System;
using System.Collections.Generic;

namespace FileManager.Models {
    public class FolderContent
    {

        public List<String> Files;
        public List<String> Folders;

        public FolderContent()
        {

            Files = new List<string>();
            Folders = new List<string>();

        }
    }
}
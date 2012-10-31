using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileManager.Models;

namespace FileManager.Controllers {
    public class HomeController : Controller {


        public ActionResult Index() {
            return View();
        }

        public ActionResult Module() {
            return View();
        }

        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(string dir, FilesTree node) {
            var directory = new DirectoryInfo(Request.MapPath(dir));

            if (node.children == null && directory.GetDirectories().Length > 0) {
                node.children = new List<FilesTree>();
            }
            // get the information of the directory
            
            // loop through each subdirectory
            foreach (var t in from d in directory.GetDirectories() let dirName = string.Format("{0}/{1}", dir, d.Name) select new FilesTree { attr = new FilesTreeAttribute { id = dirName }, data = d.Name, state = "closed"} into t where node.children != null select t)
            {
                node.children.Add(t); // add the node to the "master" node
            }

            // lastly, loop through each file in the directory, and add these as nodes
            //foreach (var f in directory.GetFiles()) {
            //    // create a new node
            //    var t = new FilesTree {attr = new FilesTreeAttribute {id = f.FullName}, data = f.Name};
            //    // add it to the "master"
            //    node.children.Add(t);
            //}
        }


        [HttpPost]
        public JsonResult GetTreeData()
        {
            const string rootPath = "/Portals2";
            var rootNode = new FilesTree { attr = new FilesTreeAttribute { id = rootPath }, data = "Portals" };
            rootNode.attr.id = rootPath;
            PopulateTree(rootPath, rootNode);
            return Json(rootNode);
        }

        

        [HttpPost]
        public JsonResult GetChildreenTree(string dir)
        {
            var rootNode = new FilesTree { attr = new FilesTreeAttribute { id = dir }, data = dir.Substring(1) };
            var rootPath = dir;
            rootNode.attr.id = rootPath;
            PopulateTree(rootPath, rootNode);
            
            return Json(rootNode.children);
        }


        [HttpPost]
        public ActionResult MoveData(string path, string destination)
        {

            path = Request.MapPath(path);
            destination = Request.MapPath(destination);
            // get the file attributes for file or directory
            var attPath = System.IO.File.GetAttributes(path);

            var attDestination = System.IO.File.GetAttributes(destination);

            var fi = new FileInfo(path);

            //detect whether its a directory or file
            if ((attPath & FileAttributes.Directory) == FileAttributes.Directory) {
                if ((attDestination & FileAttributes.Directory) == FileAttributes.Directory) {
                    MoveDirectory(path, destination);
                }
            }
            else {
                System.IO.File.Move(path, destination + "\\" + fi.Name);
            }
            return null;
        }

        [HttpPost]
        public ActionResult CreateFolder(string path, string newname)
        {
            CreateFolderInPath(path, newname);
            return null;
        }

        private static void CreateFolderInPath(string path, string newname)
        {
            Directory.CreateDirectory(path + "\\" + newname);
        }



        public void MoveDirectory(string source, string target) {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0) {
                var folders = stack.Pop();
                //Directory.CreateDirectory(folders.Target);

                // Create Directory
                var sourceFolderName =
                    folders.Source.Substring(folders.Source.LastIndexOf("\\", System.StringComparison.Ordinal) + 1);
                CreateFolderInPath(folders.Target,sourceFolderName );


                foreach (var file in Directory.GetFiles(folders.Source, "*.*")) {
                    var targetFile = Path.Combine(string.Format("{0}\\{1}", folders.Target, sourceFolderName), Path.GetFileName(file));
                    if (System.IO.File.Exists(targetFile)) System.IO.File.Delete(targetFile);
                    System.IO.File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source)) {
                    stack.Push(new Folders(folder, Path.Combine(string.Format("{0}\\{1}", folders.Target, sourceFolderName))));
                }
            }
            Directory.Delete(source, true);
        }

        private class Folders {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target) {
                Source = source;
                Target = target;
            }
        }

        public ActionResult ViewFilesFromFolder(string folder)
        {

            var directory = new DirectoryInfo(Request.MapPath(folder));
            var folderContent = new FolderContent();

            foreach (var f in directory.GetFiles()) {

                folderContent.Files.Add(new Files{ fullName = string.Format("{0}/{1}", folder, f.Name), name = f.Name, folder = folder}); 
            }

            foreach (var f in directory.GetDirectories()) {

                folderContent.Folders.Add(new Files { fullName = string.Format("{0}/{1}", folder, f.Name), name = f.Name, folder = folder }); 
            }


            return View("FilesView", folderContent);
        }

        public ActionResult UploadFile(HttpPostedFileBase fileData, string folderName)
        {

            if (fileData == null) {
                return new EmptyResult();
            }

            if (fileData.ContentLength == 0) {
                return Json("Nothing to upload");
            }

            var path = Request.MapPath(folderName);

            if (!Directory.Exists(path)) {
                return Json("wrong directory");
            }

            var fullName = string.Format(@"{0}\{1}", path, fileData.FileName);
            fileData.SaveAs(fullName);
            
            return Json("OK");
        }

        public JsonResult DeleteFile(string file, string folder )
        {
            try {
                var fullName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                System.IO.File.Delete(fullName);
                return Json("ok");
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = 500;
                return Json("");
            }
        }

        public JsonResult CreateNewFolder(string folder, string name)
        {
            try {

                CreateFolderInPath(Request.MapPath(folder), name);

                return Json("ok");
            }
            catch (Exception) {
                HttpContext.Response.StatusCode = 500;
                return Json("");
            }
        }

       public JsonResult RenameFile(string file, string folder, string name ) {
            try {
                if(file.LastIndexOf('.') > 0)
                {
                    var extension = file.Substring(file.LastIndexOf('.'));
                    name += extension;
                }
                var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(folder), name);
                System.IO.File.Move(fullOldName,fullNewName);

                return Json("ok");
            }
            catch (Exception) {
                HttpContext.Response.StatusCode = 500;
                return Json("");
            }
        }

       public JsonResult PasteFile(string file, string folder, string newFolder, bool isCopy) {
           try {
               
               var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
               var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(newFolder), file);
               System.IO.File.Copy(fullOldName, fullNewName);

               if(!isCopy)
               {
                   System.IO.File.Delete(fullOldName);  
               }
               //

               return Json("ok");
           }
           catch (Exception) {
               HttpContext.Response.StatusCode = 500;
               return Json("");
           }
       }
        

    }
}

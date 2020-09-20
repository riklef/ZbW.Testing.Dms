using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace ZbW.Testing.Dms.Client.Services
{
    public interface IFileService
    {
        void CopyFile(string filePath, string newFilename, bool isRemoveFileEnabled);
        string FileExtension(string filePath);
        List<string> getPaths();
    }
    public class FileService : IFileService
    {
        private readonly string _savePath;
        private readonly string _destPath;
        private readonly IFileSystem _fileSystem;

        public FileService(string savePath, string destPath, IFileSystem fileSystem)
        {
            _savePath = savePath;
            _destPath = destPath;
            _fileSystem = fileSystem;
        }

        public FileService(string savePath, string destPath)
        {
            _savePath = savePath;
            _destPath = destPath;
            _fileSystem = new FileSystem();
        }
        public void CopyFile(string filePath, string newFilename, bool isRemoveFileEnabled)
        {
            if(!CheckIfFolderExitst())
                CreateNewFolder();

            var newFilePath = _savePath + @"\" + newFilename;

            //wird nicht gelöscht, nur verschoben
            if(isRemoveFileEnabled)
                _fileSystem.File.Move(filePath, newFilePath);
            else
            {
                _fileSystem.File.Copy(filePath,newFilePath);
            }
        }

        public string FileExtension(string filePath)
        {
            return _fileSystem.Path.GetExtension(filePath);
        }

        public List<string> getPaths()
        {
            var paths = new List<string>();
            string[] directories = _fileSystem.Directory.GetDirectories(_destPath);
            foreach (var directory in directories)
            {
                foreach (var file in _fileSystem.Directory.GetFiles(directory, "*.xml"))
                {
                    paths.Add(file);
                }
            }

            return paths;
        }

        public void CreateNewFolder()
        {
            _fileSystem.Directory.CreateDirectory(_savePath);
        }

        private bool CheckIfFolderExitst()
        {
            return _fileSystem.Directory.Exists(_savePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using Prism.Mvvm;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.Client
{
    public class DocService : BindableBase
    {
        private readonly string _savePath;
        private readonly string _destPath;
        private readonly IFileService _fileService;
        private readonly IXMLService _xmlService;
        private List<MetadataItem> _metaDataItems;
        private readonly string _currentYear = DateTime.Now.Year.ToString();

        public List<MetadataItem> MetadataItems
        {
            get => _metaDataItems;
            set => SetProperty(ref _metaDataItems, value);
        }

        public DocService()
        {
            _destPath = ConfigurationManager.AppSettings["RepositoryDir"];
            _savePath = _destPath + @"\" + _currentYear;
            _fileService = new FileService(_savePath, _destPath);
            _xmlService = new XMLService();
        }

        public DocService(IFileService fileService, IXMLService xmlService)
        {
            _destPath = ConfigurationManager.AppSettings["RepositoryDir"];
            _savePath = _destPath + @"\" + _currentYear;
            _fileService = fileService;
            _xmlService = xmlService;
        }

        public void AddToFolder(MetadataItem metadataItem, string filepath, bool isRemoveFileEnabled, Guid guid)
        {
            //könnte in Methoden ausgelagert werden
            var newFilename = guid + "_Content" + _fileService.FileExtension(filepath);
            var xmlFilename = @"\" + guid + "_Metadata.xml";
            var xmlFilePath = _savePath + xmlFilename;

            metadataItem._filePath = _savePath + @"\" + newFilename;

            _fileService.CopyFile(filepath, newFilename, isRemoveFileEnabled);
            _xmlService.SaveXml(_xmlService.SerializeMetadataItem(metadataItem), xmlFilePath);
        }

        public List<MetadataItem> GetMetadataItems()
        {
            var metaDataItems = new List<MetadataItem>();
            var paths = _fileService.getPaths();

            foreach (var path in paths)
            {
                metaDataItems.Add(_xmlService.DeserializeMetadataItem(path));
            }

            return metaDataItems;
        }

        public List<MetadataItem> FilterMetadataItems(string search, string type)
        {
            var MetadataItems = this.MetadataItems;
            var filteredMetadataItems = new List<MetadataItem>();

            if (string.IsNullOrEmpty(search))
                search = "";

            foreach (var metadataItem in MetadataItems)
            {
                if (!string.IsNullOrEmpty(metadataItem._bezeichnung))
                {
                    //buggy
                    if ((metadataItem._bezeichnung.Contains(search) || metadataItem._selectedTypItem.Equals(type) && string.IsNullOrEmpty(type) || metadataItem._selectedTypItem.Equals(type)))
                    {
                        filteredMetadataItems.Add(metadataItem);
                    }
                }
            }

            return filteredMetadataItems;

        }

        public void OpenFile(string filePath)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process?view=netcore-3.1

            if (string.IsNullOrEmpty(filePath))
                MessageBox.Show("Keine Datei ausgewählt");
            else
            {
                Process.Start(filePath);
            }
        }
    }
}

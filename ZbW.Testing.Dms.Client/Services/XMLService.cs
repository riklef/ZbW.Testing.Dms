using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ZbW.Testing.Dms.Client.Model;

// File Stream --> https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream?view=netcore-3.1
// XML Serialization --> https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer?view=netcore-3.1

namespace ZbW.Testing.Dms.Client.Services
{
    public interface IXMLService
    {
        string SerializeMetadataItem(MetadataItem metadataItem);
        MetadataItem DeserializeMetadataItem(string path);
        void SaveXml(string serializeXML, string path);
    }

    public class XMLService : IXMLService
    {
        private readonly IFileSystem _fileSystem;

        public XMLService()
        {
            _fileSystem = new FileSystem();
        }

        public XMLService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        
        public MetadataItem DeserializeMetadataItem(string path)
        {
            using (Stream fs = (Stream) _fileSystem.FileStream.Create(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MetadataItem));
                StreamReader reader = new StreamReader(fs);

                var metadataItem = (MetadataItem) serializer.Deserialize(reader);
                reader.Close();
                return metadataItem;
            }
        }

        public void SaveXml(string serializeXML, string path)
        {
            using (Stream fs = (Stream) _fileSystem.FileStream.Create(path, FileMode.CreateNew))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(serializeXML);
                xmlDocument.Save(fs);
                fs.Flush();
                fs.Position = 0;
            }
        }

        public string SerializeMetadataItem(MetadataItem metadataItem)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MetadataItem));
            StringWriter stringWriter = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create(stringWriter);

            xmlSerializer.Serialize(xmlWriter, metadataItem);
            var serializeXML = stringWriter.ToString();
            xmlWriter.Close();
            return serializeXML;
        }
    }
}

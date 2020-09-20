using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;
using FakeItEasy;
using ZbW.Testing.Dms.Client;

namespace ZbW.Testing.Dms.UnitTests
{
    [TestFixture]
    public class DocServiceTests
    {
        private const string testPath = @"C:\temp\testpath.xml";

        [Test]
        public void AddToFolder_CopyFileCopy_IsCalled()
        {
            //arrange
            var metadataItem = A.Fake<MetadataItem>();
            var fileService = A.Fake<IFileService>();
            var xmlService = A.Fake<IXMLService>();
            var docService = new DocService(fileService, xmlService);
            Guid guid = Guid.NewGuid();
            var newFilename = guid + "_Content";
            //act
            docService.AddToFolder(metadataItem,testPath,false,guid);
            //assert
            A.CallTo(() => fileService.CopyFile(testPath, newFilename, false)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void MetaDataFilter_HasValue()
        {
            //arrange
            var docService = new DocService();
            docService.MetadataItems = A.CollectionOfDummy<MetadataItem>(5).ToList();
            docService.MetadataItems[0]._selectedTypItem = "";
            docService.MetadataItems[0]._bezeichnung = "TestBezeichnung";
            var selectedTypeItem = docService.MetadataItems[0]._selectedTypItem;
            var search = docService.MetadataItems[0]._bezeichnung;
            //act
            var result = docService.FilterMetadataItems(search, selectedTypeItem);
            //assert
            Assert.That(result, !Is.Empty);
        }

        [Test]
        public void GetMetaDataItems_ReturnsCorrectAmountOfItems()
        {
            //arrange
            var metadataItem = A.Fake<MetadataItem>();
            var fileService = A.Fake<IFileService>();
            var xmlService = A.Fake<IXMLService>();
            var docService = new DocService(fileService,xmlService);
            List<MetadataItem> metadataItems = A.CollectionOfDummy<MetadataItem>(5).ToList();
            var testPaths = new List<string>() {testPath, testPath};

            A.CallTo(() => fileService.getPaths()).Returns(testPaths);
            A.CallTo(() => xmlService.DeserializeMetadataItem(testPath)).Returns(metadataItem);
            //act
            var result = docService.GetMetadataItems();
            //assert
            Assert.That(result.Count, Is.EqualTo(testPaths.Count));
        }
    }
}

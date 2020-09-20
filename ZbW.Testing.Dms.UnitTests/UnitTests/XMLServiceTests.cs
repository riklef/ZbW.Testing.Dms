using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using ZbW.Testing.Dms.Client.Model;
using ZbW.Testing.Dms.Client.Services;

namespace ZbW.Testing.Dms.UnitTests
{
    [TestFixture]
    class XMLServiceTests
    {
        private const string testPath = @"C:\temp\DMS";
        private DateTime testDateTime = new DateTime(2020,09,15,12,12,12);

        [Test]
        public void SerializeMetadataItem_Serialize_CorrectResult()
        {
            //arrange
            var metadataItem = new MetadataItem("testbenutzer", "testbezeichnung", testDateTime, testPath, "testselectedTypeItem", "", testDateTime);
            var xmlService = new XMLService();

            string expectedResult =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><_benutzer>testbenutzer</_benutzer><_bezeichnung>testbezeichnung</_bezeichnung><_erfassungsdatum>2020-09-15T12:12:12</_erfassungsdatum><_filePath>C:\\temp\\DMS</_filePath><_selectedTypItem>testselectedTypeItem</_selectedTypItem><_stichwoerter /><_valutaDatum>2020-09-15T12:12:12</_valutaDatum></MetadataItem>";
            //act
            string result = xmlService.SerializeMetadataItem(metadataItem);
            //assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void SaveXml_Successful()
        {
            //arrange
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory(testPath);
            var xmlService = new XMLService(fileSystem);
            string serializeXML = "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><_benutzer>testbenutzer</_benutzer><_bezeichnung>testbezeichnung</_bezeichnung><_erfassungsdatum>2020-09-15T12:12:12</_erfassungsdatum><_filePath>C:\\temp\\DMS</_filePath><_selectedTypItem>testselectedTypeItem</_selectedTypItem><_stichwoerter /><_valutaDatum>2020-09-15T12:12:12</_valutaDatum></MetadataItem>";
            //act
            xmlService.SaveXml(serializeXML, testPath + @"\testfile.xml");
            //assert
            Assert.That(fileSystem.File.Exists(testPath + @"\testfile.xml"));
        }

        [Test]
        public void DeserializeMetadataItem_Deserialize_CorrectResult()
        {
            //arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    testPath + @"\test1.xml",
                    new MockFileData(
                        "<?xml version=\"1.0\" encoding=\"utf-16\"?><MetadataItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><_benutzer>testbenutzer</_benutzer><_bezeichnung>testbezeichnung</_bezeichnung><_erfassungsdatum>2020-09-15T12:12:12</_erfassungsdatum><_filePath>C:\\temp\\DMS</_filePath><_selectedTypItem>testselectedTypeItem</_selectedTypItem><_stichwoerter /><_valutaDatum>2020-09-15T12:12:12</_valutaDatum></MetadataItem>")
                }
            });
            var xmlService = new XMLService(fileSystem);
            var expectedResult = new MetadataItem("testbenutzer", "testbezeichnung", testDateTime, testPath, "testselectedTypeItem", "", testDateTime);
            //act
            var result = xmlService.DeserializeMetadataItem(testPath + @"\test1.xml");
            //assert
            Assert.That(result._benutzer.Equals(expectedResult._benutzer));
            Assert.That(result._bezeichnung.Equals(expectedResult._bezeichnung));
            Assert.That(result._erfassungsdatum.Equals(expectedResult._erfassungsdatum));
            Assert.That(result._filePath.Equals(expectedResult._filePath));
            Assert.That(result._selectedTypItem.Equals(expectedResult._selectedTypItem));
            Assert.That(result._stichwoerter.Equals(expectedResult._stichwoerter));
            Assert.That(result._valutaDatum.Equals(expectedResult._valutaDatum));
        }
    }
}

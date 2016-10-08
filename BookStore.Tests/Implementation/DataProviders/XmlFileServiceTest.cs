using System.IO;
using System.Text;
using BookStore.Implementation.DataProviders;
using BookStore.Implementation.Utils;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace BookStore.Tests.Implementation.DataProviders
{
    public class XmlFileServiceTest : UnitTestBase
    {
        private IPathUtility pathUtility;
        private IFileWrapper file;
        private IDirectoryWrapper directory;
        private XmlFileService xmlFileService;

        protected override void SetUp()
        {
            base.SetUp();

            pathUtility = NewMock<IPathUtility>();
            file = NewMock<IFileWrapper>();
            directory = NewMock<IDirectoryWrapper>();

            xmlFileService = new XmlFileService(pathUtility, file, directory);
        }

        [Test]
        public void LoadAllTestReturnsEmptyWhenDirectoryDoesNotExist()
        {
            const string uploadPath = @"C:\wwwroot\somepath\Upload";

            using (MocksRecord())
            {
                pathUtility.Expect(p => p.GetAbsolutePath("Upload")).Return(uploadPath);
                directory.Expect(d => d.Exists(uploadPath)).Return(false);
            }

            var expected = xmlFileService.LoadAll();
            expected.Should().BeEmpty();
        }

        [Test]
        public void LoadAllTestSkipsInvalidDocuments()
        {
            const string uploadPath = @"C:\wwwroot\somepath\Upload";
            const string file1 = @"C:\wwwroot\somepath\Upload\file1.xml";
            const string file2 = @"C:\wwwroot\somepath\file2.xml";
            const string invalidXmlData = @"<?xml version=""1.0""?><content><book><title>TITLE</book></title></content>";
            const string validXmlData = @"<?xml version=""1.0""?><content><book><title>TITLE</title></book></content>";

            using (MocksRecord())
            {
                pathUtility.Expect(p => p.GetAbsolutePath("Upload")).Return(uploadPath);
                directory.Expect(d => d.Exists(uploadPath)).Return(true);
                directory
                    .Expect(d => d.GetFiles(uploadPath, "*.xml", SearchOption.AllDirectories))
                    .Return(new[] {file1, file2});
                file.Expect(f => f.OpenRead(file1)).Return(new MemoryStream(Encoding.UTF8.GetBytes(invalidXmlData)));
                file.Expect(f => f.OpenRead(file2)).Return(new MemoryStream(Encoding.UTF8.GetBytes(validXmlData)));
            }

            var expected = xmlFileService.LoadAll();
            expected.Should().HaveCount(1);
            expected.Should().Contain(d => d.FilePath == file2);
        }

        [Test]
        public void LoadAllTestNormalBehavior()
        {
            const string uploadPath = @"C:\wwwroot\somepath\Upload";
            const string file1 = @"C:\wwwroot\somepath\Upload\file1.xml";
            const string file2 = @"C:\wwwroot\somepath\file2.xml";
            const string file1XmlData = @"<?xml version=""1.0""?><content><book><title>TITLE1</title></book></content>";
            const string file2XmlData = @"<?xml version=""1.0""?><content><book><title>TITLE2</title></book></content>";

            using (MocksRecord())
            {
                pathUtility.Expect(p => p.GetAbsolutePath("Upload")).Return(uploadPath);
                directory.Expect(d => d.Exists(uploadPath)).Return(true);
                directory
                    .Expect(d => d.GetFiles(uploadPath, "*.xml", SearchOption.AllDirectories))
                    .Return(new[] { file1, file2 });
                file.Expect(f => f.OpenRead(file1)).Return(new MemoryStream(Encoding.UTF8.GetBytes(file1XmlData)));
                file.Expect(f => f.OpenRead(file2)).Return(new MemoryStream(Encoding.UTF8.GetBytes(file2XmlData)));
            }

            var expected = xmlFileService.LoadAll();
            expected.Should().HaveCount(2);
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using ProductionCode.PrintPrime;
using Xunit;

namespace TestCode.PrintPrime
{
    public class PrintPrimesTests
    {
        [Fact]
        public void OutputShouldMatchGold()
        {
            using (var memoryStream = new MemoryStream())
            {
                var streamWriter = new StreamWriter(memoryStream);

                Console.SetOut(streamWriter);

                new PrintPrimes().Run();

                streamWriter.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;

                // **

                var underTestReader = new StreamReader(memoryStream);

                var goldFileReader = GetGoldFileReader();

                AssertStreamsMatch(goldFileReader, underTestReader);
            }
        }

        private StreamReader GetGoldFileReader()
        {
            var fileInfo = new FileInfo(GetGoldFilePath());
            return fileInfo.OpenText();
        }

        private static void AssertStreamsMatch(
            StreamReader expectedReader,
            StreamReader underTestReader)
        {
            var lineUnderTest = expectedReader.ReadLine();
            var expectedLine = underTestReader.ReadLine();

            while (expectedLine != null)
            {
                lineUnderTest.Should().Be(expectedLine);

                lineUnderTest = expectedReader.ReadLine();
                expectedLine = underTestReader.ReadLine();
            }
        }

        private string GetGoldFilePath()
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            folder = folder.Replace("file:\\", "");

            return folder + "\\PrintPrime\\gold.txt";
        }

        [Fact(Skip = "Should only run on the refactored code.")]
        // [Fact]
        public void CreateGoldFile()
        {
            using (var writer = new StreamWriter("c:\\temp\\gold.txt"))
            {
                Console.SetOut(writer);
                new PrintPrimes().Run();
                writer.Flush();
            }

        }
    }
}

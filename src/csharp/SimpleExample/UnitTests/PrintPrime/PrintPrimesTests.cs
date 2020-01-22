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

                var fileInfo = new FileInfo(GetGoldFilePath());
                var goldFileReader = fileInfo.OpenText();

                var lineUnderTest = goldFileReader.ReadLine();
                var expectedLine = underTestReader.ReadLine();

                while (expectedLine != null)
                {
                    lineUnderTest.Should().Be(expectedLine);

                    lineUnderTest = goldFileReader.ReadLine();
                    expectedLine = underTestReader.ReadLine();
                }
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

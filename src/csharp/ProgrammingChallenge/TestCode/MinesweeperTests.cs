using System.Text;
using Xunit;

namespace TestCode
{
    public class MinesweeperTests
    {
        [Fact]
        public void ShouldReturnGridGivenSingleEmptyCell()
        {
            AssertMatches(
                new [] { "0" },
                new [] { "." });
        }

        [Fact]
        public void ShouldReturnGridGivenSingleCellWithMine()
        {
            AssertMatches(
                new[] { "*" },
                new[] { "*" });
        }

        [Fact]
        public void ShouldReturnExpectedWith2Cells()
        {
            AssertMatches(
                new[] { "00" },

                new[] { ".." });

            AssertMatches(
                new[] { "*1" },

                new[] { "*." });

            AssertMatches(
                new[] { "1*" },

                new[] { ".*" });

            AssertMatches(
                new[] { "**" },

                new[] { "**" });
        }

        [Fact]
        public void ShouldWorkWith2x2()
        {
            AssertMatches(
                new[]
                {
                    "00",
                    "00"
                },
                new[]
                {
                    "..",
                    "..",
                });

            AssertMatches(
                new[]
                {
                    "1*",
                    "11"
                },
                new[]
                {
                    ".*",
                    "..",
                });

            AssertMatches(
                new[]
                {
                    "11",
                    "*1"
                },
                new[]
                {
                    "..",
                    "*.",
                });

            AssertMatches(
                new[]
                {
                    "2*",
                    "*2"
                },
                new[]
                {
                    ".*",
                    "*.",
                });
        }

        private static void AssertMatches(
            string[] expectedGrid,
            string[] gridUnderTest)
        {
            var ms = new Minesweeper();
            var solution = ms.Solve(gridUnderTest);
            Assert.Equal(expectedGrid, solution);
        }
    }

    public class Minesweeper
    {
        public string[] Solve(string[] grid)
        {
            // StringBuilder outputGrid = new StringBuilder();
            string[] outputGrid = new string[grid.Length];// {"*" };
            StringBuilder outputRow = new StringBuilder();

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    outputRow = new StringBuilder();
                    if (row == 0 && col == 0)
                    {
                        string currentRow = grid[row];
                        char cell = currentRow[col];

                        if (cell == '*')
                        {
                            outputRow.Append('*');
                        }
                        else if (cell == '.')
                        {
                            outputRow.Append('0');
                        }
                    }

                    


                }
            }

            outputGrid[0] = outputRow.ToString();
            return outputGrid;
        }
    }
}
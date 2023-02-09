namespace MarsClient.Tests.StepDefinitions
{
    [Binding]
    public class CellsInViewStepDefinitions
    {
        private readonly ScenarioContext context;

        public CellsInViewStepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [Given(@"the following map")]
        public void GivenTheFollowingMap(Table table)
        {
            var rowCount = table.RowCount;
            var cells = from row in Enumerable.Range(0, table.RowCount)
                        from col in Enumerable.Range(0, table.Rows[row].Count)
                        select new Cell(rowCount - row - 1, col, int.Parse(table.Rows[row][col]), isExplored: true);
            var map = Map.CreateFromCells(cells);
            context.Set(map);
        }

        [When(@"my ship is at \((.*),(.*)\) facing (.*)")]
        public void WhenMyShipIsAtFacingNorth(int row, int col, Orientation orientation)
        {
            context.Set((row, col));
            context.Set(orientation);
        }

        [Then(@"the visible cells are")]
        public void ThenTheVisibleCellsAre(Table table)
        {
            var map = context.Get<Map>();
            (int row, int col) location = context.Get<(int, int)>();

            Cell getCell(int tableRow, int tableCol)
            {
                var val = table.Rows[tableRow][tableCol];
                if (val == "null")
                    return null;
                return map.Cells.Single(kvp => kvp.Value.Difficulty == int.Parse(val)).Value;
            }

            var expectedCells = new ViewableCells
            {
                //Top Row
                LLUU = getCell(0, 0),
                LUU = getCell(0, 1),
                UU = getCell(0, 2),
                RUU = getCell(0, 3),
                RRUU = getCell(0, 4),

                //middle row
                LLU = getCell(1, 0),
                LU = getCell(1, 1),
                U = getCell(1, 2),
                RU = getCell(1, 3),
                RRU = getCell(1, 4),

                //bottom row
                LL = getCell(2, 0),
                L = getCell(2, 1),
                Me = getCell(2, 2),
                R = getCell(2, 3),
                RR = getCell(2, 4)
            };

            var orientation = context.Get<Orientation>();
            var actualCells = map.GetCellsInView(location, orientation);

            actualCells.Should().BeEquivalentTo(expectedCells);
        }
    }
}

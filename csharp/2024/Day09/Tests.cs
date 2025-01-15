// using Xunit;

// public class FileMapServiceTests
// {
//     private MapModel CreateMapFromString(string mapString)
//     {
//         var map = new MapModel();
//         foreach (var character in mapString)
//         {
//             map.Cells.Add(new CellModel(long.Parse(character.ToString() == "." ? "-1" : character.ToString())));
//         }
//         return map;
//     }

//     [Theory]
//     [InlineData("00..11..22..", 2, 2)]
//     [InlineData("00....1122..", 2, 4)]
//     public void FreeSpaceTest(string mapString, int freeSpaceIndex, int expectedfreeSpaceCount)
//     {
//         var map = CreateMapFromString(mapString);
//         IMapService mapService = new FileMapService();
//         var freeSpaceCount = mapService.CountFreeSpace(map, freeSpaceIndex);
//         Assert.Equal(freeSpaceCount, expectedfreeSpaceCount);
//     }
// }
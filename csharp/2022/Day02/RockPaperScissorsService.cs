namespace AdventOfCodeCSharp.Day02
{
    public static class RockPaperScissorsService
    {
        public static int GetScore(string player1, string player2)
        {
            if (player1 == "A" && player2 == "X")
            {
                return 3 + 1;
            } 
            else if (player1 == "A" && player2 == "Y")
            {
                return 6 + 2;
            }
            else if (player1 == "A" && player2 == "Z")
            {
                return 0 + 3;
            }
            else if (player1 == "B" && player2 == "X")
            {
                return 0 + 1;
            }
            else if (player1 == "B" && player2 == "Y")
            {
                return 3 + 2;
            }
            else if (player1 == "B" && player2 == "Z")
            {
                return 6 + 3;
            }
            else if (player1 == "C" && player2 == "X")
            {
                return 6 + 1;
            }
            else if (player1 == "C" && player2 == "Y")
            {
                return 0 + 2;
            }
            else if (player1 == "C" && player2 == "Z")
            {
                return 3 + 3;
            }
            throw new NotImplementedException();
        }

        public static string ChoosePlayer2Play(string player1Choice, string goal)
        {
            if (player1Choice == "A" && goal == "X")
            {
                return "Z";
            } 
            else if (player1Choice == "A" && goal == "Y")
            {
                return "X";
            }
            else if (player1Choice == "A" && goal == "Z")
            {
                return "Y";
            }
            else if (player1Choice == "B" && goal == "X")
            {
                return "X";
            }
            else if (player1Choice == "B" && goal == "Y")
            {
                return "Y";
            }
            else if (player1Choice == "B" && goal == "Z")
            {
                return "Z";
            }
            else if (player1Choice == "C" && goal == "X")
            {
                return "Y";
            }
            else if (player1Choice == "C" && goal == "Y")
            {
                return "Z";
            }
            else if (player1Choice == "C" && goal == "Z")
            {
                return "X";
            }
            throw new NotImplementedException();
        }
    }
}
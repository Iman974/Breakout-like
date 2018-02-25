using System.Text.RegularExpressions;

public static class RegexUtility {

    public static int GetNumberInString(string toParse, int rank = 1) {
        if (toParse != string.Empty) {
            rank--;
            Match digitMatch = Regex.Match(toParse, @"(\d+)");
            for (int i = 0; i < rank; i++) {
                digitMatch = digitMatch.NextMatch();
            }
            return int.Parse(digitMatch.Value);
        }
        throw new System.Exception("Empty string");
    }

    public static Match LevelSceneNameMatch(string path) {
        return Regex.Match(path, @"([0-9]+-[0-9]+)\.unity");
    }
}

using System.Text.RegularExpressions;

public static class RegexUtility {

    private static Regex sceneNameRegex, digitsRegex;

    static RegexUtility() {
        sceneNameRegex = new Regex(@"([0-9]+-[0-9]+)\.unity");
        digitsRegex = new Regex(@"\d+");
    }

    public static int GetNumberInString(string toParse, int rank = 1) {
        if (toParse == string.Empty) {
            throw new System.Exception("Empty string");
        } else if (rank <= 0) {
            throw new System.Exception("Unknown rank");
        }

        MatchCollection digitMatches = digitsRegex.Matches(toParse);

        if (rank > digitMatches.Count) {
            throw new System.ArgumentOutOfRangeException("rank", "Rank is greater than matches' length");
        }
        return int.Parse(digitMatches[rank - 1].Value);
    }

    public static Match LevelSceneNameMatch(string path) {
        return sceneNameRegex.Match(path);
    }
}

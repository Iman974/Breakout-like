using System.Text.RegularExpressions;

public static class RegexUtility {

    public static int GetNumberInString(string toParse) {
        if (toParse != string.Empty) {
            return int.Parse(Regex.Match(toParse, @"\d+").Value);
        }
        throw new System.Exception("Empty string");
    }

    public static Match LevelSceneNameMatch(string path) {
        return Regex.Match(path, @"([0-9]+-[0-9]+)\.unity");
    }
}

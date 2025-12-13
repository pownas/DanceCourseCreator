namespace DanceCourseCreator.Web.Helpers;

/// <summary>
/// Helper class for dance level names and translations
/// </summary>
public static class LevelHelper
{
    /// <summary>
    /// Dictionary mapping level enum values to Swedish display names
    /// </summary>
    public static readonly Dictionary<string, string> LevelDisplayNames = new()
    {
        { "Beginner", "Nybörjare" },
        { "Improver", "Fortsättning" },
        { "Intermediate", "Medel" },
        { "Advanced", "Avancerad" }
    };

    /// <summary>
    /// Get all level values for dropdowns
    /// </summary>
    public static IEnumerable<KeyValuePair<string, string>> GetAllLevels()
    {
        return LevelDisplayNames;
    }

    /// <summary>
    /// Get Swedish display name for a level
    /// </summary>
    public static string GetDisplayName(string level)
    {
        return LevelDisplayNames.TryGetValue(level, out var displayName) 
            ? displayName 
            : level;
    }

    /// <summary>
    /// Get level value from display name (reverse lookup)
    /// </summary>
    public static string? GetValueFromDisplayName(string displayName)
    {
        return LevelDisplayNames
            .FirstOrDefault(kvp => kvp.Value.Equals(displayName, StringComparison.OrdinalIgnoreCase))
            .Key;
    }
}

namespace DanceCourseCreator.Web.Helpers;

/// <summary>
/// Helper class for dance style names and translations
/// </summary>
public static class DanceStyleHelper
{
    /// <summary>
    /// Dictionary mapping dance style enum values to Swedish display names
    /// </summary>
    public static readonly Dictionary<string, string> DanceStyleDisplayNames = new()
    {
        { "WCS", "West Coast Swing" },
        { "Bugg", "Bugg" },
        { "Fox", "Fox" },
        //{ "Foxtrot", "Foxtrot" },
        //{ "Lindy", "Lindy Hop" },
        //{ "Balboa", "Balboa" },
        //{ "Blues", "Blues" },
        { "Other", "Annan" },
    };

    /// <summary>
    /// Get all dance style values for dropdowns
    /// </summary>
    public static IEnumerable<KeyValuePair<string, string>> GetAllDanceStyles()
    {
        return DanceStyleDisplayNames;
    }

    /// <summary>
    /// Get Swedish display name for a dance style
    /// </summary>
    public static string GetDisplayName(string danceStyle)
    {
        return DanceStyleDisplayNames.TryGetValue(danceStyle, out var displayName) 
            ? displayName 
            : danceStyle;
    }

    /// <summary>
    /// Get dance style value from display name (reverse lookup)
    /// </summary>
    public static string? GetValueFromDisplayName(string displayName)
    {
        return DanceStyleDisplayNames
            .FirstOrDefault(kvp => kvp.Value.Equals(displayName, StringComparison.OrdinalIgnoreCase))
            .Key;
    }
}

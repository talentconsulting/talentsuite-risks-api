namespace TalentConsulting.TalentSuite.RisksApi;

internal class OpenApiSchemaUtils
{
    private static Dictionary<string, string> _cache = new Dictionary<string, string>();

    internal static string RenameDtoStrategy(Type currentClass)
    {
        if (_cache.ContainsKey(currentClass.Name))
        {
            return _cache[currentClass.Name];
        }

        var truncate = (currentClass.FullName?.Contains(".dtos.", StringComparison.InvariantCultureIgnoreCase) ?? false) &&
            currentClass.Name.EndsWith("dto", StringComparison.InvariantCultureIgnoreCase) &&
            currentClass.Name.Length > 3;

        var displayName = truncate
            ? currentClass.Name[..^3]
            : currentClass.Name;

        _cache.Add(currentClass.Name, displayName);

        return displayName;
    }
}

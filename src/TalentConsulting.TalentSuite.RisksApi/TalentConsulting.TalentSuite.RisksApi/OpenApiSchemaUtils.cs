namespace TalentConsulting.TalentSuite.RisksApi;

internal class OpenApiSchemaUtils
{
    internal static string RenameDtoStrategy(Type currentClass)
    {
        return (currentClass.Name.EndsWith("dto", StringComparison.InvariantCultureIgnoreCase) && currentClass.Name.Length > 3)
            ? currentClass.Name[..^3]
            : currentClass.Name;
    }
}

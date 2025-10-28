using System.Text;

namespace HL7Forge.Core;

public static class Hl7Composer
{
    public const char FieldSep = '|';
    public const char CompSep = '^';
    public const char RepetitionSep = '~';
    public const char Escape = '\\';
    public const char SubcompSep = '&';

    public static string Encode(params string?[] fields)
        => string.Join(FieldSep, fields.Select(f => f ?? string.Empty));

    public static string NowTS() => DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    public static string EscapeText(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Replace($"{Escape}","\\E\\")
                    .Replace($"{FieldSep}","\\F\\")
                    .Replace($"{CompSep}","\\S\\")
                    .Replace($"{RepetitionSep}","\\R\\")
                    .Replace($"{SubcompSep}","\\T\\");
    }

    public static string JoinComponents(params string?[] comps)
        => string.Join(CompSep, comps.Select(c => EscapeText(c)));

    public static string JoinReps(params string?[] reps)
        => string.Join(RepetitionSep, reps.Select(r => r ?? string.Empty));
}

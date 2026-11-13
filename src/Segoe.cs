using Microsoft.UI.Xaml.Media;

namespace Syno.TableView;

/// <summary>
/// The icon font Windows provides, and each glyph the table draws from it, so a host does not
/// have to supply an icon font for the table to look right.
/// </summary>
/// <remarks>
/// Windows 11 has Segoe Fluent Icons. Windows 10 has only Segoe MDL2 Assets, which draws most of
/// the same code points in its older style; Microsoft offers Segoe Fluent Icons as a download for
/// design and development only, so a product cannot ship it. A glyph Windows 10 lacks therefore
/// names a second code point for it.
/// </remarks>
internal static class Segoe
{
    // Windows 11 is build 22000 and later, and every build of it has Segoe Fluent Icons.
    private static readonly bool Windows11 = OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000);

    private static FontFamily? _font;

    internal static FontFamily Font => _font ??= new(Windows11 ? "Segoe Fluent Icons" : "Segoe MDL2 Assets");

    internal const string ArrowLeft = "\uE72B";
    internal const string ArrowRight = "\uE72A";
    internal const string ChevronDown = "\uE70D";
    internal const string ChevronLeft = "\uE76B";
    internal const string ChevronRight = "\uE76C";
    internal const string ChevronUp = "\uE70E";
    internal const string Eraser = "\uE75C";
    internal const string Eye = "\uE890";
    internal static readonly string EyeOff = Windows11 ? "\uED1A" : "\uE711";
    internal const string Square = "\uE739";
    internal const string SquareCheck = "\uE73A";
}

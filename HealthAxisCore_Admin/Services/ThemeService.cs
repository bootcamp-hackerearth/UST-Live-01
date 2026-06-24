using Microsoft.JSInterop;

namespace HealthAxisCore_Admin.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetThemeAsync()
    {
        var theme = await _jsRuntime.InvokeAsync<string>(
            "healthAxisTheme.getTheme");

        return string.IsNullOrWhiteSpace(theme)
            ? "dark"
            : theme;
    }

    public async Task SetThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync(
            "healthAxisTheme.setTheme",
            theme);
    }

    public async Task<string> ToggleThemeAsync()
    {
        var currentTheme = await GetThemeAsync();

        var nextTheme = currentTheme == "dark"
            ? "light"
            : "dark";

        await SetThemeAsync(nextTheme);

        return nextTheme;
    }
}
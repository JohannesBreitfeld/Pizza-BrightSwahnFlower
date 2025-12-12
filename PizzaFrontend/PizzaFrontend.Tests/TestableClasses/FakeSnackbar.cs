using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics.CodeAnalysis;

namespace PizzaFrontend.Tests.TestableClasses
{
    public class FakeSnackbar : ISnackbar
    {
        // --- Storage for test verification ---
        public List<(string Message, Severity Severity)> Messages { get; } = new();

        // --- Required Interface Members ---
        public IEnumerable<Snackbar> ShownSnackbars => Array.Empty<Snackbar>();

        public SnackbarConfiguration Configuration { get; } = new SnackbarConfiguration();

        public event Action? OnSnackbarsUpdated;

        // --- Add(string) ---
        public Snackbar? Add(string message, Severity severity = Severity.Normal,
            Action<SnackbarOptions>? configure = null, string? key = null)
        {
            Messages.Add((message, severity));
            OnSnackbarsUpdated?.Invoke();
            return null;
        }

        // --- Add(MarkupString) ---
        public Snackbar? Add(MarkupString message, Severity severity = Severity.Normal,
            Action<SnackbarOptions>? configure = null, string? key = null)
        {
            Messages.Add((message.Value, severity));
            OnSnackbarsUpdated?.Invoke();
            return null;
        }

        // --- Add(RenderFragment) ---
        public Snackbar? Add(RenderFragment message, Severity severity = Severity.Normal,
            Action<SnackbarOptions>? configure = null, string? key = null)
        {
            // No easy way to extract text from RenderFragment, store a placeholder
            Messages.Add(("[RenderFragment]", severity));
            OnSnackbarsUpdated?.Invoke();
            return null;
        }

        // --- Add<TComponent> ---
        public Snackbar? Add<T>(Dictionary<string, object>? componentParameters = null,
            Severity severity = Severity.Normal, Action<SnackbarOptions>? configure = null, string? key = null)
            where T : IComponent
        {
            Messages.Add(($"[Component: {typeof(T).Name}]", severity));
            OnSnackbarsUpdated?.Invoke();
            return null;
        }

        // --- Clearing and Removal ---
        public void Clear()
        {
            Messages.Clear();
            OnSnackbarsUpdated?.Invoke();
        }

        public void Remove(Snackbar snackbar)
        {
            // No real Snackbar objects stored, so do nothing
        }

        public void RemoveByKey(string key)
        {
            // No keys used in testing
        }

        public void Dispose()
        {
        }
    }
}
using Security_App__Blazor.Data.Models;

namespace Security_App__Blazor.Data.Services;

public class ScriptResultService
{
    private Dictionary<string, List<ScriptModel>> _scriptGroups;

    public Dictionary<string, List<ScriptModel>> scriptGroups
    {
        get { return _scriptGroups; }
        set { _scriptGroups = value; }
    }
}

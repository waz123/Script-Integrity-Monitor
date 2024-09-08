using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Security_App__Blazor.Data.Models;

namespace Security_App__Blazor.Data;
public class SharedResultService
{
   
    private List<ScriptModel> _scripts = new List<ScriptModel>();

    public event Action OnChange;

    private Dictionary<string, List<ScriptModel>> _scriptGroups = new Dictionary<string, List<ScriptModel>>();

    public Dictionary<string, List<ScriptModel>> ScriptGroups
    {
        get { return _scriptGroups; }
        private set
        {
            _scriptGroups = value;
            NotifyStateChanged();
        }
    }

    public void UpdateScripts(List<ScriptModel> scripts)
    {
        _scripts = scripts;
        ProcessScripts(scripts);
    }

    private void ProcessScripts(List<ScriptModel> scripts)
    {
        ScriptGroups = new Dictionary<string, List<ScriptModel>>();
        if (scripts == null || !scripts.Any())
        {
            
            return;
        }
        ScriptGroups = scripts.GroupBy(script => script.Subdomain)
                              .ToDictionary(group => group.Key, group => group.ToList());
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}




using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;


public class UnityAnalytics : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
         
    }
    public void OnPlayerDead(int level)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "currentLevel", level }
        };
        Events.CustomData("playerDeadLevel", parameters);
        Events.Flush();
    }
    public void OnLevelStarted(int level)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "currentLevel", level }
        };
        Events.CustomData("levelStarted", parameters);
        Events.Flush();
    }
    public void OnLevelLifetime(int level, string time)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"currentLevel", level},
            {"levelLifetime", time}
        };
        Events.CustomData("levelLifetime", parameters);
        Events.Flush();
    }
}

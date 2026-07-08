using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public class BuildScript
{
    public static void BuildWindows()
    {
        Debug.Log("===== START BUILD =====");

        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found in Build Settings.");
            return;
        }

        foreach (var scene in scenes)
        {
            Debug.Log("Scene : " + scene);
        }

        BuildReport report = BuildPipeline.BuildPlayer(
            scenes,
            "Build/Game.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);

        Debug.Log(report.summary.result);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("BUILD SUCCESS");
        }
        else
        {
            Debug.LogError("BUILD FAILED");
        }
    }
}
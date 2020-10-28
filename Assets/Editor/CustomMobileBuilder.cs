using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Rendering;
using UnityEditor.Build.Reporting;
using System;

public class CustomMobileBuilder : EditorWindow
{

    enum CustomBuildTarget
    {
        None,
        Android,
        iOS
    }

    enum CustomAndroidGraphicsAPI
    {
        OpenGLES3,// = GraphicsDeviceType.OpenGLES3,
        Vulkan// = GraphicsDeviceType.Vulkan
    }

    enum CustomAndroidScriptingBackend
    {
        Mono,// = ScriptingImplementation.Mono2x,
        IL2CPP// = ScriptingImplementation.IL2CPP
    }

    enum CustomiOSStrippingLevel
    {
        Low,
        Medium,
        High
    }

    class BuildQueueItem
    {
        public int BuildTarget { get; set; }
        public dynamic BuildOptions { get; set; }
    }

    class AndroidBuildOptions
    {
        public CustomAndroidGraphicsAPI GraphicsAPI { get; set; }
        public CustomAndroidScriptingBackend ScriptingBackend { get; set; }
    }

    class IOSBuildOptions
    {
        public CustomiOSStrippingLevel StrippingLevel { get; set; }
    }

    List<BuildQueueItem> buildQueue = new List<BuildQueueItem> { new BuildQueueItem { BuildTarget = (int) CustomBuildTarget.None } };
    Vector2 scrollPos;
    bool isBuilding = false;

    [MenuItem("Window/Custom Mobile Builder")]
    public static void ShowWindow()
    {
        var Window = EditorWindow.GetWindow(typeof(CustomMobileBuilder));
        Window.minSize = new Vector2(500, 500);
    }

    void OnGUI()
    {
        
        GUILayout.Label("Builds In Queue - " + (buildQueue.Count - 1), EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < buildQueue.Count; i++)
        {
            // Only the last popup can be "Not selected"
            if ((buildQueue.Count - 1) != i && buildQueue[i].BuildTarget == (int)CustomBuildTarget.None)
            {
                buildQueue.RemoveAt(i);
            }
            // If last popup is selected, append one more
            if ((buildQueue.Count - 1) == i && buildQueue[i].BuildTarget != (int)CustomBuildTarget.None)
            {
                buildQueue.Add(new BuildQueueItem { BuildTarget = (int) CustomBuildTarget.None });
            }
        }

        for(int i = 0; i < buildQueue.Count; i++)
        {
            Rect r = (Rect) EditorGUILayout.BeginVertical("Box");
            // Show build id
            if (buildQueue[i].BuildTarget != (int)CustomBuildTarget.None) GUILayout.Label("ID:  " + i);
            // Show Build target popup
            buildQueue[i].BuildTarget = EditorGUILayout.Popup("Build target: ", buildQueue[i].BuildTarget, new string[] { "None", "Android", "iOS" });

            switch(buildQueue[i].BuildTarget)
            {
                case (int) CustomBuildTarget.Android:
                    // Set new AndroidBuildOptions instance if buildOptions is either null or another type
                    if (buildQueue[i].BuildOptions == null || buildQueue[i].BuildOptions.GetType() != typeof(AndroidBuildOptions))
                    {
                        buildQueue[i].BuildOptions = new AndroidBuildOptions();
                    }
                    // Get ref
                    var tmpBuildOptions = (buildQueue[i].BuildOptions as AndroidBuildOptions);
                    // Set Scripting backend
                    tmpBuildOptions.ScriptingBackend = (CustomAndroidScriptingBackend) EditorGUILayout.Popup("  Scripting Backend: ", (int) tmpBuildOptions.ScriptingBackend, System.Enum.GetNames(typeof(CustomAndroidScriptingBackend)));
                    // Set Graphics API
                    tmpBuildOptions.GraphicsAPI =(CustomAndroidGraphicsAPI) EditorGUILayout.Popup("  Graphics API: ", (int) tmpBuildOptions.GraphicsAPI, System.Enum.GetNames(typeof(CustomAndroidGraphicsAPI)));

                    break;
                case (int) CustomBuildTarget.iOS:
                    // Set new AndroidBuildOptions instance if buildOptions is either null or another type
                    if (buildQueue[i].BuildOptions == null || buildQueue[i].BuildOptions.GetType() != typeof(IOSBuildOptions))
                    {
                        buildQueue[i].BuildOptions = new IOSBuildOptions();
                    }
                    
                    var tmpIOSBuildOptions = (buildQueue[i].BuildOptions as IOSBuildOptions);
                    // Set Stripping level
                    tmpIOSBuildOptions.StrippingLevel = (CustomiOSStrippingLevel) EditorGUILayout.Popup("  Stripping level: ", (int) tmpIOSBuildOptions.StrippingLevel, System.Enum.GetNames(typeof(CustomiOSStrippingLevel)));
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        if (buildQueue.Count > 1)
        {
            if (BuildButton()) Build();
        } else
        {
            GUI.enabled = false;
            BuildButton();
            GUI.enabled = true;
        }

    }

    void Build()
    {
        try
        {
            if (isBuilding) return;
            else isBuilding = true;

            for (int i = 0; i < buildQueue.Count; i++)
            {
                try
                {
                    if (buildQueue[i].BuildTarget == (int)CustomBuildTarget.None) continue;

                    switch (buildQueue[i].BuildTarget)
                    {
                        case (int)CustomBuildTarget.Android:
                            BuildAndroid(buildQueue[i]);
                            break;
                        case (int)CustomBuildTarget.iOS:
                            BuildIOS(buildQueue[i]);
                            break;
                    }
                }
                catch (Exception e)
                {
                    var buildTargetName = ((CustomBuildTarget)buildQueue[i].BuildTarget).ToString();
                    throw new Exception(String.Format("Build failed for '{0}', build id: {1}\nReason: {2}", buildTargetName, i, e.Message));
                }
            }
            //
            isBuilding = false;
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
            isBuilding = false;
        }
        
        
    }

    void BuildAndroid(BuildQueueItem buildQueueItem)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        // Set target Android
        buildPlayerOptions.target = BuildTarget.Android;
        // Get all scenes paths that are added to build settings
        buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
        // Set Graphics API
        switch ((int)(buildQueueItem.BuildOptions as AndroidBuildOptions).GraphicsAPI)
        {
            case (int) CustomAndroidGraphicsAPI.OpenGLES3:
                PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
                break;
            case (int) CustomAndroidGraphicsAPI.Vulkan:
                PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.Vulkan });
                break;
            default:
                throw new Exception("Unknown Graphics API selected");
        }
        // Set Scripting backend
        switch ((int)(buildQueueItem.BuildOptions as AndroidBuildOptions).ScriptingBackend)
        {
            case (int)CustomAndroidScriptingBackend.Mono:
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                break;
            case (int)CustomAndroidScriptingBackend.IL2CPP:
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                break;
            default:
                throw new Exception("Unknown scripting backend selected");
        }

        // Set installation path
        buildPlayerOptions.locationPathName = "Builds/" +
            Application.productName + "_" +
            PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) + "_" +
            PlayerSettings.GetGraphicsAPIs(BuildTarget.Android)[0] + 
            ".apk";

        // Build player
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes\nBuild location: " + buildPlayerOptions.locationPathName);
        }

        if (summary.result == BuildResult.Failed)
        {
            throw new Exception("Build failed");
        }
    }

    void BuildIOS(BuildQueueItem buildQueueItem)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        // Set target iOS
        buildPlayerOptions.target = BuildTarget.iOS;
        // Get all scenes paths that are added to build settings
        buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

        // Set Stripping level
        switch ((int)(buildQueueItem.BuildOptions as IOSBuildOptions).StrippingLevel)
        {
            case (int)CustomiOSStrippingLevel.Low:
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
                break;
            case (int)CustomiOSStrippingLevel.Medium:
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Medium);
                break;
            case (int)CustomiOSStrippingLevel.High:
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.High);
                break;
            default:
                throw new Exception("Unknown Stripping level selected");
        }

        // Set installation path
        buildPlayerOptions.locationPathName = "Builds/" +
            Application.productName + "_" +
            PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.iOS);

        // Build player
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes\nBuild location: " + buildPlayerOptions.locationPathName);
        }

        if (summary.result == BuildResult.Failed)
        {
            throw new Exception("Build failed");
        }
    }

    bool BuildButton()
    {
        // Add space
        EditorGUILayout.Space();
        var style = new GUIStyle(GUI.skin.button)
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            padding = {bottom=5, top=5}
        };

        return GUILayout.Button(isBuilding ? "Building..." : "Build", style);
    }

}

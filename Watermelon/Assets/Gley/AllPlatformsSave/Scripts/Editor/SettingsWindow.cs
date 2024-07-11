namespace Gley.AllPlatformsSave.Editor
{
    using Gley.AllPlatformsSave.Internal;
    using Gley.Common;
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;


    public class SettingsWindow : EditorWindow
    {
        private static string rootFolder;

        private AllPlatformsSaveData saveSettings;
        private List<SupportedBuildTargetGroup> buildTargetGroup;
        private List<SupportedSaveMethods> selectedSaveMethod;


        // Get existing open window or if none, make a new one:
        [MenuItem(SettingsWindowProperties.menuItem, false, 10)]
        static void Init()
        {
            WindowLoader.LoadWindow<SettingsWindow>(new SettingsWindowProperties(), out rootFolder);
        }


        private void OnEnable()
        {
            if (rootFolder == null)
            {
                rootFolder = WindowLoader.GetRootFolder(new SettingsWindowProperties());
            }

            saveSettings = EditorUtilities.LoadOrCreateDataAsset<AllPlatformsSaveData>(rootFolder, Internal.Constants.RESOURCES_FOLDER, Internal.Constants.DATA_NAME_RUNTIME);

            selectedSaveMethod = saveSettings.saveMethod;
            buildTargetGroup = saveSettings.buildTargetGroup;
        }


        void OnGUI()
        {
            GUILayout.Label("Configure your save plugin from here: ", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < buildTargetGroup.Count; i++)
            {
                buildTargetGroup[i] = (SupportedBuildTargetGroup)EditorGUILayout.EnumPopup("Select your build target:", buildTargetGroup[i]);
                selectedSaveMethod[i] = (SupportedSaveMethods)EditorGUILayout.EnumPopup("Select save method:", selectedSaveMethod[i]);
                if (GUILayout.Button("Remove Build Target"))
                {
                    buildTargetGroup.RemoveAt(i);
                    selectedSaveMethod.RemoveAt(i);
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add Build Target"))
            {
                buildTargetGroup.Add(SupportedBuildTargetGroup.Android);
                selectedSaveMethod.Add(SupportedSaveMethods.JSONSerializationFileSave);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                for (int i = 0; i < buildTargetGroup.Count - 1; i++)
                {
                    for (int j = i + 1; j < buildTargetGroup.Count; j++)
                    {
                        if (buildTargetGroup[i] == buildTargetGroup[j])
                        {
                            Debug.LogError($"Platform {buildTargetGroup[i]} exists multiple times. Remove duplicate entries and save again.");
                            return;
                        }
                    }
                }

                var buildTargetGroups = Enum.GetValues(typeof(SupportedBuildTargetGroup));
                var saveMethods = Enum.GetValues(typeof(SupportedSaveMethods));
                for (int i = 0; i < buildTargetGroups.Length; i++)
                {
                    for (int j = 0; j < saveMethods.Length; j++)
                    {
                        try
                        {
                            PreprocessorDirective.AddToPlatform(saveMethods.GetValue(j).ToString(), true, (BuildTargetGroup)buildTargetGroups.GetValue(i));
                        }
                        catch { }
                    }
                }

                for (int i = 0; i < buildTargetGroup.Count; i++)
                {
                    PreprocessorDirective.AddToPlatform(selectedSaveMethod[i].ToString(), false, (BuildTargetGroup)buildTargetGroup[i]);
                }

                SaveSettings();
                Debug.Log("Settings applied.");
            }
          
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Example Scene"))
            {
                EditorSceneManager.OpenScene($"{rootFolder}/{SettingsWindowProperties.testScene}");
            }

            if (GUILayout.Button("Documentation"))
            {
                Application.OpenURL(SettingsWindowProperties.documentation);
            }
            EditorGUILayout.EndHorizontal();
        }


        private void SaveSettings()
        {
            saveSettings.saveMethod = selectedSaveMethod;
            saveSettings.buildTargetGroup = buildTargetGroup;
            EditorUtility.SetDirty(saveSettings);
        }
    }
}

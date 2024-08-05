// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/8/5 11:25:34
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Framework.Runtime;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(ResourceComponent))]
    public sealed class ResourceComponentInspector : FrameworkInspector
    {
        private static readonly string[] sResourceModeNames = new[] { "Package", "Updatable", "Updatable While Playing" };

        private SerializedProperty mResourceMode = null;
        private SerializedProperty mReadWritePathType = null;
        private SerializedProperty mMinUnloadUnusedAssetsInterval = null;
        private SerializedProperty mMaxUnloadUnusedAssetsInterval = null;
        private SerializedProperty mAssetAutoReleaseInterval = null;
        private SerializedProperty mAssetCapacity = null;
        private SerializedProperty mAssetPriority = null;
        private SerializedProperty mResourceAutoReleaseInterval = null;
        private SerializedProperty mResourceCapacity = null;
        private SerializedProperty mResourcePriority = null;
        private SerializedProperty mUpdatePrefixUri = null;
        private SerializedProperty mGenerateReadWriteVersionListLength = null;
        private SerializedProperty mUpdateRetryCount = null;
        private SerializedProperty mInstanceRoot = null;
        private SerializedProperty mLoadResourceAgentHelperCount = null;

        private FieldInfo mEditorResourceModeFieldInfo = null;
        private int mResourceModeIndex = 0;

        private HelperInfo<ResourceHelperBase> mResourceHelperInfo = new HelperInfo<ResourceHelperBase>("Resource");
        private HelperInfo<LoadResourceAgentHelperBase> mLoadResourceAgentHelperInfo = new HelperInfo<LoadResourceAgentHelperBase>("LoadResourceAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = target as ResourceComponent;

            var isEditorResourceMode = (bool)mEditorResourceModeFieldInfo.GetValue(target);
            if (isEditorResourceMode)
            {
                EditorGUILayout.HelpBox("Editor resource mode is enabled. Some options are disabled.", MessageType.Warning);
            }

            if (t != null)
            {
                EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
                {
                    if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                    {
                        EditorGUILayout.EnumPopup("Resource Mode", t.ResourceMode);
                    }
                    else
                    {
                        var selectedIndex = EditorGUILayout.Popup("Resource Mode", mResourceModeIndex, sResourceModeNames);
                        if (selectedIndex != mResourceModeIndex)
                        {
                            mResourceModeIndex = selectedIndex;
                            mResourceMode.enumValueIndex = selectedIndex + 1;
                        }
                    }

                    mReadWritePathType.enumValueIndex = (int)(ReadWritePathType)EditorGUILayout.EnumPopup("Read-Write Path Type", t.ReadWritePathType);
                }
                EditorGUI.EndDisabledGroup();

                var minUnloadUnusedAssetsInterval = EditorGUILayout.Slider("Min Unload Unused Assets Interval", mMinUnloadUnusedAssetsInterval.floatValue, 0f, 3600f);
                if (minUnloadUnusedAssetsInterval != mMinUnloadUnusedAssetsInterval.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.MinUnloadUnusedAssetsInterval = minUnloadUnusedAssetsInterval;
                    }
                    else
                    {
                        mMinUnloadUnusedAssetsInterval.floatValue = minUnloadUnusedAssetsInterval;
                    }
                }

                var maxUnloadUnusedAssetsInterval = EditorGUILayout.Slider("Max Unload Unused Assets Interval", mMaxUnloadUnusedAssetsInterval.floatValue, 0f, 3600f);
                if (maxUnloadUnusedAssetsInterval != mMaxUnloadUnusedAssetsInterval.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.MaxUnloadUnusedAssetsInterval = maxUnloadUnusedAssetsInterval;
                    }
                    else
                    {
                        mMaxUnloadUnusedAssetsInterval.floatValue = maxUnloadUnusedAssetsInterval;
                    }
                }

                EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying && isEditorResourceMode);
                {
                    var assetAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Asset Auto Release Interval", mAssetAutoReleaseInterval.floatValue);
                    if (assetAutoReleaseInterval != mAssetAutoReleaseInterval.floatValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.AssetAutoReleaseInterval = assetAutoReleaseInterval;
                        }
                        else
                        {
                            mAssetAutoReleaseInterval.floatValue = assetAutoReleaseInterval;
                        }
                    }

                    var assetCapacity = EditorGUILayout.DelayedIntField("Asset Capacity", mAssetCapacity.intValue);
                    if (assetCapacity != mAssetCapacity.intValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.AssetCapacity = assetCapacity;
                        }
                        else
                        {
                            mAssetCapacity.intValue = assetCapacity;
                        }
                    }

                    var assetPriority = EditorGUILayout.DelayedIntField("Asset Priority", mAssetPriority.intValue);
                    if (assetPriority != mAssetPriority.intValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.AssetPriority = assetPriority;
                        }
                        else
                        {
                            mAssetPriority.intValue = assetPriority;
                        }
                    }

                    var resourceAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Resource Auto Release Interval", mResourceAutoReleaseInterval.floatValue);
                    if (resourceAutoReleaseInterval != mResourceAutoReleaseInterval.floatValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.ResourceAutoReleaseInterval = resourceAutoReleaseInterval;
                        }
                        else
                        {
                            mResourceAutoReleaseInterval.floatValue = resourceAutoReleaseInterval;
                        }
                    }

                    var resourceCapacity = EditorGUILayout.DelayedIntField("Resource Capacity", mResourceCapacity.intValue);
                    if (resourceCapacity != mResourceCapacity.intValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.ResourceCapacity = resourceCapacity;
                        }
                        else
                        {
                            mResourceCapacity.intValue = resourceCapacity;
                        }
                    }

                    var resourcePriority = EditorGUILayout.DelayedIntField("Resource Priority", mResourcePriority.intValue);
                    if (resourcePriority != mResourcePriority.intValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.ResourcePriority = resourcePriority;
                        }
                        else
                        {
                            mResourcePriority.intValue = resourcePriority;
                        }
                    }

                    if (mResourceModeIndex > 0)
                    {
                        var updatePrefixUri = EditorGUILayout.DelayedTextField("Update Prefix Uri", mUpdatePrefixUri.stringValue);
                        if (updatePrefixUri != mUpdatePrefixUri.stringValue)
                        {
                            if (EditorApplication.isPlaying)
                            {
                                t.UpdatePrefixUri = updatePrefixUri;
                            }
                            else
                            {
                                mUpdatePrefixUri.stringValue = updatePrefixUri;
                            }
                        }

                        var generateReadWriteVersionListLength =
                            EditorGUILayout.DelayedIntField("Generate Read-Write Version List Length", mGenerateReadWriteVersionListLength.intValue);
                        if (generateReadWriteVersionListLength != mResourcePriority.intValue)
                        {
                            if (EditorApplication.isPlaying)
                            {
                                t.GenerateReadWriteVersionListLength = generateReadWriteVersionListLength;
                            }
                            else
                            {
                                mGenerateReadWriteVersionListLength.intValue = generateReadWriteVersionListLength;
                            }
                        }

                        var updateRetryCount = EditorGUILayout.DelayedIntField("Update Retry Count", mUpdateRetryCount.intValue);
                        if (updateRetryCount != mUpdateRetryCount.intValue)
                        {
                            if (EditorApplication.isPlaying)
                            {
                                t.UpdateRetryCount = updateRetryCount;
                            }
                            else
                            {
                                mUpdateRetryCount.intValue = updateRetryCount;
                            }
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
                {
                    EditorGUILayout.PropertyField(mInstanceRoot);

                    mResourceHelperInfo.Draw();
                    mLoadResourceAgentHelperInfo.Draw();

                    mLoadResourceAgentHelperCount.intValue = EditorGUILayout.IntSlider("Load Resource Agent Helper Count", mLoadResourceAgentHelperCount.intValue, 1, 128);
                }
                EditorGUI.EndDisabledGroup();

                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.LabelField("Unload Unused Assets", $"{t.LastUnloadUnusedAssetsOperationElapseSeconds:F2} / {t.MaxUnloadUnusedAssetsInterval}");
                    EditorGUILayout.LabelField("Read-Only Path", t.ReadOnlyPath);
                    EditorGUILayout.LabelField("Read-Write Path", t.ReadWritePath);
                    EditorGUILayout.LabelField("Current Variant", t.CurrentVariant ?? Constant.UnknownOptionName);
                    EditorGUILayout.LabelField("Applicable Version", isEditorResourceMode ? Constant.NAOptionName : t.ApplicableVersion ?? Constant.UnknownOptionName);
                    EditorGUILayout.LabelField("Internal Resource Version", isEditorResourceMode ? Constant.NAOptionName : t.InternalResourceVersion.ToString());
                    EditorGUILayout.LabelField("Asset Count", isEditorResourceMode ? Constant.NAOptionName : t.AssetCount.ToString());
                    EditorGUILayout.LabelField("Resource Count", isEditorResourceMode ? Constant.NAOptionName : t.ResourceCount.ToString());
                    EditorGUILayout.LabelField("Resource Group Count", isEditorResourceMode ? Constant.NAOptionName : t.ResourceGroupCount.ToString());
                    if (mResourceModeIndex > 0)
                    {
                        EditorGUILayout.LabelField("Applying Resource Pack Path", isEditorResourceMode ? Constant.NAOptionName : t.ApplyingResourcePackPath ?? Constant.UnknownOptionName);
                        EditorGUILayout.LabelField("Apply Waiting Count", isEditorResourceMode ? Constant.NAOptionName : t.ApplyingWaitingCount.ToString());
                        EditorGUILayout.LabelField("Updating Resource Group",
                            isEditorResourceMode ? Constant.NAOptionName : t.UpdatingResourceGroup != null ? t.UpdatingResourceGroup.Name : Constant.UnknownOptionName);
                        EditorGUILayout.LabelField("Update Waiting Count", isEditorResourceMode ? Constant.NAOptionName : t.UpdateWaitingCount.ToString());
                        EditorGUILayout.LabelField("Update Waiting While Playing Count", isEditorResourceMode ? Constant.NAOptionName : t.UpdateWaitingWhilePlayingCount.ToString());
                        EditorGUILayout.LabelField("Update Candidate Count", isEditorResourceMode ? Constant.NAOptionName : t.UpdateCandidateCount.ToString());
                    }

                    EditorGUILayout.LabelField("LoadTotalAgentCount", isEditorResourceMode ? Constant.NAOptionName : t.LoadTotalAgentCount.ToString());
                    EditorGUILayout.LabelField("LoadAvailableAgentCount", isEditorResourceMode ? Constant.NAOptionName : t.LoadAvailableAgentCount.ToString());
                    EditorGUILayout.LabelField("Load Working Agent Count", isEditorResourceMode ? Constant.NAOptionName : t.LoadWorkingAgentCount.ToString());
                    EditorGUILayout.LabelField("Load Waiting Task Count", isEditorResourceMode ? Constant.NAOptionName : t.LoadWaitingTaskCount.ToString());
                    if (!isEditorResourceMode)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            var loadAssetInfos = t.GetAllLoadAssetInfos();
                            if (loadAssetInfos.Length > 0)
                            {
                                foreach (var assetInfo in loadAssetInfos)
                                {
                                    EditorGUILayout.LabelField(assetInfo.Description, $"[SerialId]{assetInfo.SerialId} [Priority]{assetInfo.Priority} [Status]{assetInfo.Status}");
                                }

                                if (GUILayout.Button("Export CSV Data"))
                                {
                                    var exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, "Load Asset Task Data", "csv");
                                    if (!string.IsNullOrEmpty(exportFileName))
                                    {
                                        try
                                        {
                                            var index = 0;
                                            var data = new string[loadAssetInfos.Length + 1];
                                            data[index++] = "Load Asset Name,Serial Id,Priority,Status";
                                            foreach (var assetInfo in loadAssetInfos)
                                            {
                                                data[index++] = $"{index - 1},{assetInfo.SerialId},{assetInfo.Priority},{assetInfo.Status}";
                                            }

                                            File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                            Debug.Log($"Export load asset task CSV data to ({exportFileName}) success.");
                                        }
                                        catch (Exception e)
                                        {
                                            Debug.LogError($"Export load asset task csv data to ({exportFileName}) failure with exception is ({e})");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GUILayout.Label("Load Asset Task is Empty ...");
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void OnEnable()
        {
            mResourceMode = serializedObject.FindProperty("mResourceMode");
            mReadWritePathType = serializedObject.FindProperty("mReadWritePathType");
            mMinUnloadUnusedAssetsInterval = serializedObject.FindProperty("mMinUnloadUnusedAssetsInterval");
            mMaxUnloadUnusedAssetsInterval = serializedObject.FindProperty("mMaxUnloadUnusedAssetsInterval");
            mAssetAutoReleaseInterval = serializedObject.FindProperty("mAssetAutoReleaseInterval");
            mAssetCapacity = serializedObject.FindProperty("mAssetCapacity");
            mAssetPriority = serializedObject.FindProperty("mAssetPriority");
            mResourceAutoReleaseInterval = serializedObject.FindProperty("mResourceAutoReleaseInterval");
            mResourceCapacity = serializedObject.FindProperty("mResourceCapacity");
            mResourcePriority = serializedObject.FindProperty("mResourcePriority");
            mUpdatePrefixUri = serializedObject.FindProperty("mUpdatePrefixUri");
            mGenerateReadWriteVersionListLength = serializedObject.FindProperty("mGenerateReadWriteVersionListLength");
            mUpdateRetryCount = serializedObject.FindProperty("mUpdateRetryCount");
            mInstanceRoot = serializedObject.FindProperty("mInstanceRoot");
            mLoadResourceAgentHelperCount = serializedObject.FindProperty("mLoadResourceAgentHelperCount");

            mEditorResourceModeFieldInfo = target.GetType().GetField("mEditorResourceMode", BindingFlags.NonPublic | BindingFlags.Instance);
            mResourceModeIndex = mResourceMode.enumValueIndex > 0 ? mResourceMode.enumValueIndex - 1 : 0;

            mResourceHelperInfo.Init(serializedObject);
            mLoadResourceAgentHelperInfo.Init(serializedObject);

            OnRefreshTypeNames();
        }

        protected override void OnRefreshTypeNames()
        {
            base.OnRefreshTypeNames();

            mResourceHelperInfo.Refresh();
            mLoadResourceAgentHelperInfo.Refresh();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
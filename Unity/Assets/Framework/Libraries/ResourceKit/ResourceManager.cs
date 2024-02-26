using System;
using System.Collections.Generic;
namespace Framework
{
    public sealed class ResourceManager : FrameworkModule, IResourceManager
    {

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            
        }
        public override void Shutdown()
        {
            
        }
        public ResourceConstant ResourceConstant { get; }
        public int AssetCount { get; }
        public string Variant { get; }
        public ObjectPoolInfo AssetObjectPoolInfo { get; set; }
        public int ResourceCount { get; }
        public int ResourceGroupCount { get; }
        public ObjectPoolInfo ResourceObjectPoolInfo { get; set; }
        public event EventHandler<ResourceVerifyStartEventArgs> ResourceVerifyStart;
        public event EventHandler<ResourceVerifySuccessEventArgs> ResourceVerifySuccess;
        public event EventHandler<ResourceVerifyFailureEventArgs> ResourceVerifyFailure;
        public event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;
        public event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;
        public event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;
        public event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;
        public event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;
        public event EventHandler<ResourceApplyStartEventArgs> ResourceApplyStart;
        public event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;
        public event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
        {
            throw new NotImplementedException();
        }
        public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
        {
            throw new NotImplementedException();
        }
        public void UpdateVersionList(VersionListInfo versionListInfo, UpdateVersionListCallbacks updateVersionListCallbacks)
        {
            throw new NotImplementedException();
        }
        public void VerifyResource(int verifyResourceLengthPerFrame, VerifyResourceCompleteCallback verifyResourceCompleteCallback)
        {
            throw new NotImplementedException();
        }
        public void CheckResource(bool ignoreOtherVariant, CheckResourceCompleteCallback checkResourceCompleteCallback)
        {
            throw new NotImplementedException();
        }
        public void ApplyResource(string resourcePackPath, ApplyResourceCompleteCallback applyResourceCompleteCallback)
        {
            throw new NotImplementedException();
        }
        public void UpdateResource(UpdateResourceCompleteCallback updateResourceCompleteCallback)
        {
            throw new NotImplementedException();
        }
        public void StopUpdateResource()
        {
            throw new NotImplementedException();
        }
        public void VerifyResourcePack(string resourcePackPath)
        {
            throw new NotImplementedException();
        }
        public TaskInfo[] GetAllLoadAssetInfos()
        {
            throw new NotImplementedException();
        }
        public void GetAllLoadAssetInfos(List<TaskInfo> results)
        {
            throw new NotImplementedException();
        }
        public HasAssetResult HasAsset(string assetName)
        {
            throw new NotImplementedException();
        }
        public void LoadAsset(LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks)
        {
            throw new NotImplementedException();
        }
        public void UnloadAsset(object asset)
        {
            throw new NotImplementedException();
        }
        public void LoadScene(LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks)
        {
            throw new NotImplementedException();
        }
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null)
        {
            throw new NotImplementedException();
        }
        public string GetBinaryPath(string binaryAssetName)
        {
            throw new NotImplementedException();
        }
        public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName)
        {
            throw new NotImplementedException();
        }
        public int GetBinaryLength(string binaryAssetName)
        {
            throw new NotImplementedException();
        }
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData = null)
        {
            throw new NotImplementedException();
        }
        public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
        {
            throw new NotImplementedException();
        }
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex = 0)
        {
            throw new NotImplementedException();
        }
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
        {
            throw new NotImplementedException();
        }
        public bool HasResourceGroup(string resourceGroupName)
        {
            throw new NotImplementedException();
        }
        public IResourceGroup GetResourceGroup(string resourceGroupName = null)
        {
            throw new NotImplementedException();
        }
        public IResourceGroup[] GetResourceGroups()
        {
            throw new NotImplementedException();
        }
        public void GetResourceGroups(List<IResourceGroup> results)
        {
            throw new NotImplementedException();
        }
    }
}
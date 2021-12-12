using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



public class MainWindowView : AssetBundleViewBase
{
   [SerializeField] private Button _loadAssetsButton;
   
   [SerializeField] private AssetReference _loadPrefab;
  
   [SerializeField] private RectTransform _mountSpawnTransform;
   [SerializeField] private Button _spawnAssetsButton;
   
   private List<AsyncOperationHandle<GameObject>> _addressablePrefabs = new List<AsyncOperationHandle<GameObject>>();





   private void Start()
   {
      _loadAssetsButton.onClick.AddListener(LoadAssets);
      
      _spawnAssetsButton.onClick.AddListener(CreateAddressablesPrefab);

   }

   private void OnDestroy()
   {
       _loadAssetsButton.onClick.RemoveAllListeners();
       
       foreach (var addressablePrefab in _addressablePrefabs)
           Addressables.ReleaseInstance(addressablePrefab);
      
       _addressablePrefabs.Clear();
   }
   
   private void CreateAddressablesPrefab()
   {
       var addressablePrefab = Addressables.InstantiateAsync(_loadPrefab, _mountSpawnTransform);
       _addressablePrefabs.Add(addressablePrefab);
   }


   private void LoadAssets()
   {
       _loadAssetsButton.interactable = false;
       StartCoroutine(DownoloadAndSetAssetBundle());
   }
}

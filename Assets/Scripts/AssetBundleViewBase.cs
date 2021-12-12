using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AssetBundleViewBase : MonoBehaviour
{
    private const string UrlAssetBundleSprite = "https://drive.google.com/uc?export=download&id=1niwwnu6xGnRCojXnH2EtRrJOZT5qi4w1";
    private const string UrlAssetBundleAudio = "https://drive.google.com/uc?export=download&id=1_caBRzy8TOaFsZPtYqpXDHKWFVYwAsdL";


    [SerializeField] private DataSpriteBundle [] _dataSpriteBundle;
    [SerializeField] private DataAudioBundle [] _dataAudioBundle;

    private AssetBundle _spritesAssetBundle;
    private AssetBundle _audioAssetBundle;

    protected IEnumerator DownoloadAndSetAssetBundle()
    {
        yield return GetSpriteAssetBundle();
        yield return GetAudioAssetBundle();
        
        if (_spritesAssetBundle == null || _audioAssetBundle == null)
        {
            Debug.LogError("Error asset bundle");
            yield break;
        }
        
        SetDownloadAssets();
        yield break;
    }

    private void SetDownloadAssets()
    {
        foreach (var data in _dataSpriteBundle)
        {
            data.image.sprite = _spritesAssetBundle.LoadAsset<Sprite>(data.IdImage);
        }
        
        foreach (var data in _dataAudioBundle)
        {
            data.AudioSource.clip = _spritesAssetBundle.LoadAsset<AudioClip>(data.IdAudio);
            data.AudioSource.Play();
        }
    }
    private IEnumerator GetAudioAssetBundle()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleAudio);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;
        StateRequests(request, ref _audioAssetBundle);
    }

    private IEnumerator GetSpriteAssetBundle()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleSprite);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;
        StateRequests(request, ref _spritesAssetBundle);
    }

    private void StateRequests(UnityWebRequest request,ref AssetBundle assetBundle)
    {
        if (request.error == null)
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.Log("Complete");
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}

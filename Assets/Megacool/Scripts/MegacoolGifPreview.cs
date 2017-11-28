using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

[AddComponentMenu("Megacool/Gif Preview")]
[RequireComponent(typeof(RawImage))]
public class MegacoolGifPreview : MonoBehaviour {

    private RawImage _rawImage;

    private Coroutine _playGifIEnumerator;

    Texture2D previewTexture;

    private void Awake() {
        if (_rawImage == null) {
            _rawImage = GetComponent<RawImage>();
            _rawImage.enabled = false;
        }
    }

    public void StartPreview(string recordingIdentifier = default(string)) {
        StopPreview();
        var previewInfoJson = Megacool.Instance.GetPreviewInfoForRecording(recordingIdentifier);

        if (string.IsNullOrEmpty(previewInfoJson)) {
            return;
        }

        var dict = MegacoolThirdParty_MiniJSON.Json.Deserialize(previewInfoJson) as Dictionary<string, object>;
        var previewInfo = new MegacoolPreviewInfo(dict);

        if (previewInfo.FramePaths == null || previewInfo.FramePaths.Length == 0) {
            return;
        }

        _playGifIEnumerator = StartCoroutine(
            PreviewMegacoolGif(previewInfo.FramePaths, previewInfo.PlaybackFrameRate, previewInfo.LastFrameDelay)
        );
    }

    public void StopPreview() {
        if (_playGifIEnumerator != null) {
            StopCoroutine(_playGifIEnumerator);
           
            _rawImage.enabled = false;
            _rawImage.texture = null;

            // Only destroy the preview texture if it has been created, might not be the case if there were no frames
            // in the preview or it was stopped before any frames were loaded.
            if (previewTexture) {
                Destroy(previewTexture);
            }

            _playGifIEnumerator = null;
        }
    }

    private IEnumerator PreviewMegacoolGif(string[] framePaths, float playbackFrameRate, int lastFrameDelay) {
        float updateinterval = 1.0f / playbackFrameRate;
        float _lastFrameDelay = (float)lastFrameDelay / 1000f;

        framePaths = ValidatedFrameList(framePaths);

        byte[] fileData;

        previewTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        _rawImage.texture = previewTexture;

        _rawImage.enabled = true;

        bool isPlaying = true;
        float _cachedTime = 0.0f;

        int totalFrames = framePaths.Length;
        while (isPlaying) {
            for (int i = 0; i < totalFrames; i++) {
                _cachedTime = Time.realtimeSinceStartup;

                try {
                    fileData = File.ReadAllBytes(framePaths[i]);
                    if (previewTexture.LoadImage(fileData)) {
                        _rawImage.texture = previewTexture;
                    }
                } catch (System.Exception e) {
                    // Can happen for missing files, files that failed to write completely due to full disk, etc.
                    Debug.LogException(e);
                    continue;
                }

                _cachedTime = Mathf.Clamp(Time.realtimeSinceStartup - _cachedTime, 0, Mathf.Infinity);

                _cachedTime = updateinterval - _cachedTime;
                if (_cachedTime > 0.0f || i >= totalFrames - 1) {
#if UNITY_5_4_OR_NEWER
    WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(_cachedTime);
#else
    WaitForSeconds waitTime = new WaitForSeconds(_cachedTime);
#endif
                    yield return waitTime;
                }
            }
#if UNITY_5_4_OR_NEWER
    WaitForSecondsRealtime waitLastFrame = new WaitForSecondsRealtime(_lastFrameDelay);
#else
    WaitForSeconds waitLastFrame = new WaitForSeconds(_lastFrameDelay);
#endif
            yield return waitLastFrame;
        }
    }

    public string[] ValidatedFrameList(string[] framePaths) {
        var _validFramPaths = new List<string>();

        for (int i=0; i<framePaths.Length; i++) {
            if(!string.IsNullOrEmpty(framePaths[i])) {
                _validFramPaths.Add(framePaths[i]);
            }
        }
        return _validFramPaths.ToArray();
    }
}
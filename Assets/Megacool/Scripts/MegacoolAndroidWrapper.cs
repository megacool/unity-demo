using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MegacoolAndroidWrapper {

    private AndroidJavaObject CurrentActivity {
        get {
            AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return jclass.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    public MegacoolAndroidWrapper() {
        Android.Call("setActivity", CurrentActivity);
    }

    private AndroidJavaObject android;

    private AndroidJavaObject Android {
        get {
            if (android == null) {
                android = new AndroidJavaObject("co.megacool.megacool.MegacoolUnity");

                if (Megacool.Debug) {
                    // Ensure debugging is enabled as early as possible
                    Debug = true;
                }
            }
            return android;
        }
    }

    public string SharingText {
        get {
            return Android.Call<string>("getSharingText");
        }
        set {
            Android.Call("setSharingText", value);
        }
    }

    public float FrameRate {
        get {
            return Android.Call<float>("getFrameRate");
        }
        set {
            Android.Call("setFrameRate", value);
        }
    }

    public float PlaybackFrameRate {
        get {
            return Android.Call<float>("getPlaybackFrameRate");
        }
        set {
            Android.Call("setPlaybackFrameRate", value);
        }
    }

    public int MaxFrames {
        get {
            return Android.Call<int>("getMaxFrames");
        }
        set {
            Android.Call("setMaxFrames", value);
        }
    }

    public double PeakLocation {
        get {
            return Android.Call<double>("getPeakLocation");
        }
        set {
            Android.Call("setPeakLocation", value);
        }
    }

    public int LastFrameDelay {
        get {
            return Android.Call<int>("getLastFrameDelay");
        }
        set {
            Android.Call("setLastFrameDelay", value);
        }
    }

    public bool KeepCompletedRecordings {
        set {
            Android.Call("setKeepCompletedRecordings", value);
        }
    }

    public bool Debug {
        set {
            Android.Call("setDebug", value);
        }
    }

    public void StartWithConfig(string config) {
        Android.Call("startWithConfig", config);
    }

    public void Start(string config, Action<MegacoolEvent> eventHandler) {
        Android.Call("start", config, new OnEventsReceivedListener(eventHandler));
    }

    public void StartRecording() {
        Android.Call("startRecording");
    }

    public void StartRecording(MegacoolRecordingConfig config) {
        AndroidJavaObject jConfig = new AndroidJavaObject("co.megacool.megacool.RecordingConfig");

        // We have to use the generic version of Call here since the Java methods are not void, even
        // though we discard the return value
        jConfig.Call<AndroidJavaObject>("id", config.RecordingId);
        jConfig.Call<AndroidJavaObject>("maxFrames", config.MaxFrames);
        jConfig.Call<AndroidJavaObject>("peakLocation", config.PeakLocation);
        jConfig.Call<AndroidJavaObject>("frameRate", config.FrameRate);
        jConfig.Call<AndroidJavaObject>("playbackFrameRate", config.PlaybackFrameRate);
        jConfig.Call<AndroidJavaObject>("lastFrameDelay", config.LastFrameDelay);
        jConfig.Call<AndroidJavaObject>("overflowStrategy", config.OverflowStrategy.ToString());

//        AndroidJavaObject jCrop = new AndroidJavaObject("android.graphics.Rect",
//            (int)config.Crop.xMin, (int)config.Crop.yMin, (int)config.Crop.xMax, (int)config.Crop.yMax);
//        jConfig.Call<AndroidJavaObject>("cropRect", jCrop);

        Android.Call("startRecording", jConfig);
    }

    public void RegisterScoreChange(int scoreDelta) {
        Android.Call("registerScoreChange", scoreDelta);
    }

    public void CaptureFrame() {
        Android.Call("captureFrame");
    }

    public void CaptureFrame(MegacoolFrameCaptureConfig config) {
        AndroidJavaObject jConfig = new AndroidJavaObject("co.megacool.megacool.RecordingConfig");

        // We have to use the generic version of Call here since the Java methods are not void, even
        // though we discard the return value
        jConfig.Call<AndroidJavaObject>("id", config.RecordingId);
        jConfig.Call<AndroidJavaObject>("maxFrames", config.MaxFrames);
        jConfig.Call<AndroidJavaObject>("peakLocation", config.PeakLocation);
        jConfig.Call<AndroidJavaObject>("frameRate", config.FrameRate);
        jConfig.Call<AndroidJavaObject>("playbackFrameRate", config.PlaybackFrameRate);
        jConfig.Call<AndroidJavaObject>("lastFrameDelay", config.LastFrameDelay);
        jConfig.Call<AndroidJavaObject>("overflowStrategy", config.OverflowStrategy.ToString());

//        AndroidJavaObject jCrop = new AndroidJavaObject("android.graphics.Rect",
//            (int)config.Crop.xMin, (int)config.Crop.yMin, (int)config.Crop.xMax, (int)config.Crop.yMax);
//        jConfig.Call<AndroidJavaObject>("cropRect", jCrop);

        Android.Call("captureFrame", jConfig);
    }

    public void PauseRecording() {
        Android.Call("pauseRecording");
    }

    public void StopRecording() {
        Android.Call("stopRecording");
    }

    public void DeleteRecording(string recordingId) {
        Android.Call("deleteRecording", recordingId);
    }

    public string GetPreviewInfoForRecording(string recordingId) {
        return Android.Call<string>("getPreviewInfoForRecording", recordingId);
    }

    public void Share() {
        Android.Call("share");
    }

    public void Share(MegacoolShareConfig config) {
        Android.Call("share", config.RecordingId, config.FallbackImage, config.FallbackImage, config.LastFrameOverlay, config.Share.Url.ToString(), MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data));
    }

    public void ShareToMessenger() {
        Android.Call("shareToMessenger");
    }

    public void ShareToMessenger(MegacoolShareConfig config) {
        Android.Call("shareToMessenger", config.RecordingId, config.FallbackImage, config.FallbackImage, config.LastFrameOverlay, config.Share.Url.ToString(), MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data));
    }

    public void ShareToTwitter() {
        Android.Call("shareToTwitter");
    }

    public void ShareToTwitter(MegacoolShareConfig config) {
        Android.Call("shareToTwitter", config.RecordingId, config.FallbackImage, config.FallbackImage, config.LastFrameOverlay, config.Share.Url.ToString(), MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data));
    }

    public void ShareToMessages() {
        Android.Call("shareToMessages");
    }

    public void ShareToMessages(MegacoolShareConfig config) {
        Android.Call("shareToMessages", config.RecordingId, config.FallbackImage, config.FallbackImage, config.LastFrameOverlay, config.Share.Url.ToString(), MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data));
    }

    public void ShareToMail() {
        Android.Call("shareToMail");
    }

    public void ShareToMail(MegacoolShareConfig config) {
        Android.Call("shareToMail", config.RecordingId, config.FallbackImage, config.FallbackImage, config.LastFrameOverlay, config.Share.Url.ToString(), MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data));
    }

    public void GetShares(Action<List<MegacoolShare>> shares, Func<MegacoolShare, bool> filter = null) {
        Android.Call<AndroidJavaObject>("getShares", new ShareCallback(shares), filter != null ? new ShareFilter(filter) : null);
    }

    public void DeleteShares(Func<MegacoolShare, bool> filter) {
        Android.Call("deleteShares", new ShareFilter(filter));
    }

    public void SubmitDebugData(string message) {
        Android.Call("submitDebugData", message);
    }

    public void ResetIdentity() {
        Android.Call("resetIdentity");
    }
}

class OnEventsReceivedListener : AndroidJavaProxy {
    private Action<MegacoolEvent> eventHandler;

    public OnEventsReceivedListener(Action<MegacoolEvent> eventHandler) : base("co.megacool.megacool.Megacool$OnEventsReceivedListener") {
        this.eventHandler = eventHandler;
    }

    void onEventsReceived(AndroidJavaObject jEvents) {
        int size = jEvents.Call<int>("size");
        for (int i = 0; i < size; i++) {
            AndroidJavaObject jEvent = jEvents.Call<AndroidJavaObject>("get", i);
            eventHandler(new MegacoolEvent(jEvent));
        }
    }
}

class ShareCallback : AndroidJavaProxy {
    private Action<List<MegacoolShare>> shareHandler;

    public ShareCallback(Action<List<MegacoolShare>> shareHandler) : base("co.megacool.megacool.Megacool$ShareCallback") {
        this.shareHandler = shareHandler;
    }

    void shares(AndroidJavaObject jShares) {
        int size = jShares.Call<int>("size");
        List<MegacoolShare> result = new List<MegacoolShare>(size);
        for (int i = 0; i < size; i++) {
            AndroidJavaObject jShare = jShares.Call<AndroidJavaObject>("get", i);
            result.Add(new MegacoolShare(jShare));
        }
        shareHandler(result);
    }
}

class ShareFilter : AndroidJavaProxy {
    private Func<MegacoolShare, bool> filter;

    public ShareFilter(Func<MegacoolShare, bool> filter) : base("co.megacool.megacool.Megacool$ShareFilter") {
        this.filter = filter;
    }

    bool accept(AndroidJavaObject jShare) {
        return filter(new MegacoolShare(jShare));
    }
}

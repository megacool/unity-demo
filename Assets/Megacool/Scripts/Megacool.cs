using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using AOT;
using System.Collections.Generic;


public enum MegacoolCaptureMethod {
    BLIT,
    SCREEN,
    RENDER,
}

public sealed class Megacool {

    [StructLayout(LayoutKind.Sequential)]
    public struct MegacoolLinkClickedEvent {
        public int isFirstSession;
        public string userId;
        public string shareId;
        public string url;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MegacoolReceivedShareOpenedEvent {
        public string userId;
        public string shareId;
        public int state;
        public double createdAt;
        public double updatedAt;
        public IntPtr dataBytes;
        public int dataLength;
        public string url;
        public int isFirstSession;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MegacoolSentShareOpenedEvent {
        public string userId;
        public string shareId;
        public int state;
        public double createdAt;
        public double updatedAt;
        public string receiverUserId;
        public string url;
        public int isFirstSession;
        public IntPtr eventDataBytes;
        public int eventDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MegacoolShareData {
        public string userId;
        public string shareId;
        public int state;
        public double createdAt;
        public double updatedAt;
        public IntPtr dataBytes;
        public int dataLength;
    }

    public enum GifColorTableType {
        GifColorTableFixed,
        GifColorTableAnalyzeFirst,
    }

#pragma warning disable 0414
    [StructLayout(LayoutKind.Sequential)]
    private struct Crop {
        float x;
        float y;
        float width;
        float height;

        public Crop(Rect rect) {
            this.x = rect.x;
            this.y = rect.y;
            this.width = rect.width;
            this.height = rect.height;
        }
    }
#pragma warning restore 0414

#region iOS SDK linkage

#if UNITY_ANDROID && !UNITY_EDITOR
    [DllImport("megacool")]
    private static extern void mcl_init_capture(int width, int height, string graphicsDeviceType);

    [DllImport("megacool")]
    private static extern void mcl_set_capture_texture(IntPtr texturePointer);

    [DllImport("megacool")]
    private static extern IntPtr mcl_get_unity_render_event_pointer();
#endif


#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void mcl_init_capture(int width, int height, string graphicsDeviceType);

    [DllImport("__Internal")]
    private static extern void mcl_set_capture_texture(IntPtr texturePointer);

    [DllImport("__Internal")]
    private static extern IntPtr mcl_get_unity_render_event_pointer();

    [DllImport("__Internal")]
    private static extern void startWithAppConfig(string config);

    [DllImport("__Internal")]
    private static extern void startRecording();

    [DllImport("__Internal")]
    private static extern void startRecordingWithConfig(string recordingId, Crop crop, int maxFrames, int frameRate, double peakLocation, string overflowStrategy);

    [DllImport("__Internal")]
    private static extern void registerScoreChange(int scoreDelta);

    [DllImport("__Internal")]
    private static extern void captureFrame();

    [DllImport("__Internal")]
    private static extern void captureFrameWithConfig(string recordingId, string overflowStrategy, Crop crop, bool forceAdd, int maxFrames, int frameRate);

    [DllImport("__Internal")]
    private static extern void pauseRecording();

    [DllImport("__Internal")]
    private static extern void stopRecording();

    [DllImport("__Internal")]
    private static extern void deleteRecording(string recordingId);

    [DllImport("__Internal")]
    private static extern void deleteShares(IntPtr filter);

    [MonoPInvokeCallback(typeof(Func<MegacoolShare, bool>))]
    private static bool DeleteSharesFilter(MegacoolShareData shareData) {
        return deleteSharesFilter(new MegacoolShare(shareData));
    }

    [DllImport("__Internal")]
    private static extern string getPreviewInfoForRecording(string recordingId);

    [DllImport("__Internal")]
    private static extern void getShares();

    [DllImport("__Internal")]
    private static extern void presentShare();

    [DllImport("__Internal")]
    private static extern void presentShareWithConfig(string recordingId, string lastFrameOverlay, string fallbackImage, string url, string data);

    [DllImport("__Internal")]
    private static extern void presentShareToMessenger();

    [DllImport("__Internal")]
    private static extern void presentShareToMessengerWithConfig(string recordingId, string lastFrameOverlay, string fallbackImage, string url, string data);

    [DllImport("__Internal")]
    private static extern void presentShareToTwitter();

    [DllImport("__Internal")]
    private static extern void presentShareToTwitterWithConfig(string recordingId, string lastFrameOverlay, string fallbackImage, string url, string data);

    [DllImport("__Internal")]
    private static extern void presentShareToMessages();

    [DllImport("__Internal")]
    private static extern void presentShareToMessagesWithConfig(string recordingId, string lastFrameOverlay, string fallbackImage, string url, string data);

    [DllImport("__Internal")]
    private static extern void presentShareToMail();

    [DllImport("__Internal")]
    private static extern void presentShareToMailWithConfig(string recordingId, string lastFrameOverlay, string fallbackImage, string url, string data);

    [DllImport("__Internal")]
    private static extern void setSharingText(string text);

    [DllImport("__Internal")]
    private static extern string getSharingText();

    [DllImport("__Internal")]
    private static extern void setFrameRate(float frameRate);

    [DllImport("__Internal")]
    private static extern float getFrameRate();

    [DllImport("__Internal")]
    private static extern void setPlaybackFrameRate(float frameRate);

    [DllImport("__Internal")]
    private static extern float getPlaybackFrameRate();

    [DllImport("__Internal")]
    private static extern void setMaxFrames(int maxFrames);

    [DllImport("__Internal")]
    private static extern int getMaxFrames();

    [DllImport("__Internal")]
    private static extern void setPeakLocation(double peakLocation);

    [DllImport("__Internal")]
    private static extern double getPeakLocation();

    [DllImport("__Internal")]
    private static extern void setLastFrameDelay(int delay);

    [DllImport("__Internal")]
    private static extern int getLastFrameDelay();

    [DllImport("__Internal")]
    private static extern void setLastFrameOverlay(string lastFrameOverlay);

    [DllImport("__Internal")]
    private static extern void setDebugMode(bool debugMode);

    [DllImport("__Internal")]
    private static extern bool getDebugMode();

    [DllImport("__Internal")]
    private static extern float getScreenScale();

    [DllImport("__Internal")]
    private static extern void setKeepCompletedRecordings(bool keep);

    [DllImport("__Internal")]
    private static extern void submitDebugDataWithMessage(string message);

    [DllImport("__Internal")]
    private static extern void resetIdentity();

    [DllImport("__Internal")]
    private static extern void setGIFColorTable(GifColorTableType gifColorTable);

    [DllImport("__Internal")]
    private static extern void setMegacoolDidCompleteShareDelegate(IntPtr f);

    [DllImport("__Internal")]
    private static extern void setMegacoolDidDismissShareDelegate(IntPtr f);

    [DllImport("__Internal")]
    private static extern void manualApplicationDidBecomeActive();

    [DllImport("__Internal")]
    private static extern void setOnLinkClickedEventDelegate(IntPtr f);

    private delegate void OnLinkClickedEventDelegate(MegacoolLinkClickedEvent e);

    [MonoPInvokeCallback(typeof(OnLinkClickedEventDelegate))]
    private static void OnLinkClickedEvent(MegacoolLinkClickedEvent e) {
        if (Megacool.Instance.EventHandler == null) {
            return;
        }
        Megacool.Instance.EventHandler(new MegacoolEvent(e));
    }

    [DllImport("__Internal")]
    private static extern void setOnReceivedShareOpenedEventDelegate(IntPtr f);

    private delegate void OnReceivedShareOpenedEventDelegate(MegacoolReceivedShareOpenedEvent e);

    [MonoPInvokeCallback(typeof(OnReceivedShareOpenedEventDelegate))]
    private static void OnReceivedShareOpenedEvent(MegacoolReceivedShareOpenedEvent e) {
        if (Megacool.Instance.EventHandler == null) {
            return;
        }
        Megacool.instance.EventHandler(new MegacoolEvent(e));
    }

    [DllImport("__Internal")]
    private static extern void setOnSentShareOpenedEventDelegate(IntPtr f);

    private delegate void OnSentShareOpenedEventDelegate(MegacoolSentShareOpenedEvent e);

    [MonoPInvokeCallback(typeof(OnSentShareOpenedEventDelegate))]
    private static void OnSentShareOpenedEvent(MegacoolSentShareOpenedEvent e) {
        if (Megacool.Instance.EventHandler == null) {
            return;
        }
        Megacool.instance.EventHandler(new MegacoolEvent(e));
    }

    [DllImport("__Internal")]
    private static extern void setOnRetrievedSharesDelegate(IntPtr f);

    private delegate void OnRetrievedSharesDelegate(/*MegacoolShareData[]*/ IntPtr shares, int size);

    [MonoPInvokeCallback(typeof(OnRetrievedSharesDelegate))]
    private static void OnRetrievedShares(IntPtr shares, int size) {
        long longPtr = shares.ToInt64();

        var shs = new List<MegacoolShare>(size);

        for (int i = 0; i < size; i++) {
            IntPtr structPtr = new IntPtr(longPtr);
            MegacoolShareData shareData = (MegacoolShareData)Marshal.PtrToStructure(structPtr, typeof(MegacoolShareData));
            longPtr += Marshal.SizeOf(typeof(MegacoolShareData));
            shs.Add(new MegacoolShare(shareData));
        }

        Megacool.instance.OnSharesRetrieved(shs);
    }
#endif

#endregion

#region Android Instance

    private MegacoolAndroidWrapper androidWrapper;

    private MegacoolAndroidWrapper AndroidWrapper {
        get {
            if (androidWrapper == null) {
                androidWrapper = new MegacoolAndroidWrapper();
            }
            return androidWrapper;
        }
    }

#endregion

#region Instance

    private static readonly Megacool instance = new Megacool();

    private Megacool() {
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static Megacool Instance {
        get {
            return instance;
        }
    }

#endregion

#region Delegates

    private delegate void MegacoolDidCompleteShareDelegate();

    private delegate void MegacoolDidDismissShareDelegate();

    private delegate void EventHandlerDelegate(IntPtr jsonData, int length);

    public static Action<List<MegacoolEvent>> OnMegacoolEvents = delegate {};

    public static Action<MegacoolEvent> OnReceivedShareOpened = delegate {};

    public static Action<MegacoolEvent> OnLinkClicked = delegate {};

    public static Action<MegacoolEvent> OnSentShareOpened = delegate {};

    /// <summary>
    /// Callback when a user has completed a share
    /// </summary>
    /// <example>
    /// Megacool.Instance.CompletedSharing += () => {
    ///     Debug.Log("User completed sharing");
    /// }
    /// </example>
    public Action CompletedSharing = delegate {
    };

    /// <summary>
    /// Callback when a user has aborted (dismissed) a share
    /// </summary>
    /// <example>
    /// Megacool.Instance.DismissedSharing += () => {
    ///     Debug.Log("User dismissed sharing");
    /// }
    /// </example>
    public Action DismissedSharing = delegate {
    };

    public Action<List<MegacoolShare>> OnSharesRetrieved = delegate {
    };

    [MonoPInvokeCallback(typeof(MegacoolDidCompleteShareDelegate))]
    static void DidCompleteShare() {
        Megacool.Instance.CompletedSharing();
    }

    [MonoPInvokeCallback(typeof(MegacoolDidDismissShareDelegate))]
    static void DidDismissShare() {
        Megacool.Instance.DismissedSharing();
    }

#endregion

#region Properties

    private const int MCRS = 0x6d637273;
    private IntPtr nativePluginCallbackPointer;

    /// <summary>
    /// Set the text to be shared of different channels.
    /// </summary>
    /// <remarks>
    /// The text should be set before Share() is called.
    /// </remarks>
    /// <value>The sharing text.</value>
    public string SharingText {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setSharingText(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.SharingText = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getSharingText();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.SharingText;
#else
            return "Share text is unavailable outside iOS/Android";
#endif
        }
    }

    /// <summary>
    /// Set number of frames per second to record.
    /// </summary>
    /// <remarks>
    /// Default is 10 frames / second. The GIF will be recorded with this frame rate.
    /// </remarks>
    /// <value>The frame rate.</value>
    public float FrameRate {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setFrameRate(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.FrameRate = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getFrameRate();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.FrameRate;
#else
            return MegacoolConfiguration.Instance.recordingFrameRate;
#endif
        }
    }

    /// <summary>
    /// Set number of frames per second to play.
    /// </summary>
    /// <remarks>
    /// Default is 10 frames / second. The GIF will be exported with this frame rate.
    /// </remarks>
    /// <value>The playback frame rate.</value>
    public float PlaybackFrameRate {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setPlaybackFrameRate(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.PlaybackFrameRate = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getPlaybackFrameRate();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.PlaybackFrameRate;
#else
            return MegacoolConfiguration.Instance.playbackFrameRate;
#endif
        }
    }

    /// <summary>
    /// Max number of frames on the buffer.
    /// </summary>
    /// <remarks>
    /// Default is 50 frames.
    /// </remarks>
    /// <value>Max frames.</value>
    public int MaxFrames {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setMaxFrames(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.MaxFrames = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getMaxFrames();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.MaxFrames;
#else
            return MegacoolConfiguration.Instance.maxFrames;
#endif
        }
    }

    /// <summary>
    /// Location in recording where max number of points should occur
    /// </summary>
    /// <remarks>
    /// Default is 0.7 (70% of the way through the recording)
    /// </remarks>
    /// <value>Peak location.</value>
    public double PeakLocation {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setPeakLocation(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.PeakLocation = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getPeakLocation();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.PeakLocation;
#else
            return MegacoolConfiguration.Instance.peakLocation;
#endif
        }
    }

    /// <summary>
    /// Set a delay (in seconds) on the last frame in the animation.
    /// </summary>
    /// <remarks>
    /// Default is 1 second
    /// </remarks>
    /// <value>The last frame delay.</value>
    public int LastFrameDelay {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setLastFrameDelay(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.LastFrameDelay = value;
#endif
        }
        get {
#if UNITY_IOS && !UNITY_EDITOR
            return getLastFrameDelay();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return AndroidWrapper.LastFrameDelay;
#else
            return MegacoolConfiguration.Instance.lastFrameDelay;
#endif
        }
    }

    /// <summary>
    /// Overlay an image over the last frame of the GIF.
    /// </summary>
    /// <remarks>
    /// Default is none. The path should be relative to the StreamingAssets directory.
    ///
    /// To show the overlay on previews as well you need to set includeLastFrameOverlay=true
    /// on the PreviewConfig.
    /// </remarks>
    /// <value>The path to the last frame overlay</value>
    public string LastFrameOverlay {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setLastFrameOverlay(value);
#elif UNITY_ANDROID && !UNITY_EDITOR

#endif
        }
    }

    /// <summary>
    /// Set the type of GIF color table to use. Default is fixed 256 colors.
    /// </summary>
    /// <remarks>
    /// It's recommended to test all to see which gives the best result. It depends on the color usage in the app.
    /// </remarks>
    /// <value>The gif color table type</value>
    public GifColorTableType GifColorTable {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setGIFColorTable(value);
#elif UNITY_ANDROID && !UNITY_EDITOR

#endif
        }
    }


    /// <summary>
    /// Turn on / off debug mode. In debug mode calls to the SDK are stored and can be submitted to the core developers using SubmitDebugData later.
    /// </summary>
    /// <value><c>true</c> if debug mode; otherwise, <c>false</c>.</value>
    public static bool Debug {
        set {
            MegacoolConfiguration.Instance.debugMode = value;
#if UNITY_IOS && !UNITY_EDITOR
            setDebugMode(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            Instance.AndroidWrapper.Debug = value;
#endif
        }
        get {
            return MegacoolConfiguration.Instance.debugMode;
        }
    }

    /// <summary>
    /// Whether to keep completed recordings around.
    /// </summary>
    /// <description>
    /// The default is false, which means that all completed recordings will be deleted
    /// whenever a new recording is started with either <c>captureFrame</c> or <c>startRecording</c>.
    /// Setting this to <c>true</c> means we will never delete a completed recording, which is what you want if you want to
    /// enable player to browse previous GIFs they've created. A completed recording will still be
    /// overwritten if a new recording is started with the same <c>recordingId</c>
    /// </description>
    /// <value><c>true</c> to keep completed recordings; otherwise, <c>false</c>.</value>
    public bool KeepCompletedRecordings {
        set {
#if UNITY_IOS && !UNITY_EDITOR
            setKeepCompletedRecordings(value);
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidWrapper.KeepCompletedRecordings = value;
#endif
        }
    }

    public Action<MegacoolEvent> EventHandler { get; private set; }

    private float ScaleFactor = getDefaultScaleFactor();

    private static float getDefaultScaleFactor() {
        // Default to half of the screen size to strike a balance between quality and memory
        // usage/performance, while ensuring at least 200x200 for compatibility with Facebook
        int shortestEdge = Math.Min(Screen.width, Screen.height);
        int targetShortestEdge = Math.Max(shortestEdge / 2, 200);

        // Screen sizes bigger than 1500px (like iPad mini 3) will be divided by 4
        if (shortestEdge > 1500) {
            targetShortestEdge = shortestEdge / 4;
        }
        return (float)shortestEdge / targetShortestEdge;
    }

    private RenderTexture renderTexture;

    public RenderTexture RenderTexture {
        get {
            if (!renderTexture) {
                int width = (int)(Screen.width / ScaleFactor);
                int height = (int)(Screen.height / ScaleFactor);

                renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
                renderTexture.filterMode = FilterMode.Point;
            }
            if (!renderTexture.IsCreated()) {
                // The texture can become lost on level reloads, ensure it's recreated
                renderTexture.Create();

                if (CaptureMethod != MegacoolCaptureMethod.SCREEN) {
                    SignalRenderTexture(renderTexture);
                }
            }

            return renderTexture;
        }
    }

    private MegacoolCaptureMethod captureMethod = MegacoolCaptureMethod.SCREEN;

    /// <summary>
    /// Set how frames should be captured.
    /// </summary>
    /// <value>The capture method.</value>
    public MegacoolCaptureMethod CaptureMethod {
        get {
            // SCREEN is only compatible with OpenGL ES 3 or newer, fall back to blitting if unsupported.
            if (captureMethod == MegacoolCaptureMethod.SCREEN &&
                    SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3) {
                return MegacoolCaptureMethod.BLIT;
            }
            return captureMethod;
        }
        set {
            captureMethod = value;
            if (!hasStarted) {
                // Only communicate changes if it's already set, otherwise it'll be initialized with the correct method.
                return;
            }

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            // Use the getter so that we use a valid value
            if (CaptureMethod == MegacoolCaptureMethod.SCREEN) {
                mcl_set_capture_texture(IntPtr.Zero);
            } else {
                SignalRenderTexture(renderTexture);
            }
#endif
        }
    }


    private bool _isRecording = false;
    public bool IsRecording {
        get {
            return _isRecording;
        }
    }

#endregion

#region Functionality

    private void InitializeSharingDelegate() {
#if UNITY_IOS && !UNITY_EDITOR
        MegacoolDidCompleteShareDelegate didCompleteShareDelegate = new MegacoolDidCompleteShareDelegate(DidCompleteShare);
        MegacoolDidDismissShareDelegate didDismissShareDelegate = new MegacoolDidDismissShareDelegate(DidDismissShare);

        setMegacoolDidCompleteShareDelegate(Marshal.GetFunctionPointerForDelegate(didCompleteShareDelegate));
        setMegacoolDidDismissShareDelegate(Marshal.GetFunctionPointerForDelegate(didDismissShareDelegate));

        OnLinkClickedEventDelegate onLinkClickedEventDelegate = new OnLinkClickedEventDelegate(OnLinkClickedEvent);
        OnReceivedShareOpenedEventDelegate onReceivedShareOpenedEventDelegate = new OnReceivedShareOpenedEventDelegate(OnReceivedShareOpenedEvent);
        OnSentShareOpenedEventDelegate onSentShareOpenedEventDelegate = new OnSentShareOpenedEventDelegate(OnSentShareOpenedEvent);

        setOnLinkClickedEventDelegate(Marshal.GetFunctionPointerForDelegate(onLinkClickedEventDelegate));
        setOnReceivedShareOpenedEventDelegate(Marshal.GetFunctionPointerForDelegate(onReceivedShareOpenedEventDelegate));
        setOnSentShareOpenedEventDelegate(Marshal.GetFunctionPointerForDelegate(onSentShareOpenedEventDelegate));

        setOnRetrievedSharesDelegate(Marshal.GetFunctionPointerForDelegate(new OnRetrievedSharesDelegate(OnRetrievedShares)));
#endif
    }

    private void SetupDefaultConfiguration() {
        SharingText = MegacoolConfiguration.Instance.sharingText;
        FrameRate = MegacoolConfiguration.Instance.recordingFrameRate;
        PlaybackFrameRate = MegacoolConfiguration.Instance.playbackFrameRate;
        MaxFrames = MegacoolConfiguration.Instance.maxFrames;
        PeakLocation = MegacoolConfiguration.Instance.peakLocation;
        LastFrameDelay = MegacoolConfiguration.Instance.lastFrameDelay;
        GifColorTable = MegacoolConfiguration.Instance.gifColorTable;
    }

    private void ManualApplicationDidBecomeActive() {
#if UNITY_IOS && !UNITY_EDITOR
        manualApplicationDidBecomeActive();
#endif
    }

    private void CreateMainThreadAction(MegacoolEvent megacoolEvent) {

        // Call the appropriate delegate
        switch (megacoolEvent.Type) {

        // This device has received a share to the app, including a share object
        case MegacoolEvent.MegacoolEventType.ReceivedShareOpened:
            OnReceivedShareOpened(megacoolEvent);
            break;

            // The app has been opened from a link click, send the user instantly to
            // the right scene if the URL path exists
        case MegacoolEvent.MegacoolEventType.LinkClicked:
            OnLinkClicked(megacoolEvent);
            break;

            // A Friend has received a share from your device
        case MegacoolEvent.MegacoolEventType.SentShareOpened:
            OnSentShareOpened(megacoolEvent);
            break;
        }

        // Call an umbrella delegate to handle all the events
        List<MegacoolEvent> allCurrentEvents = new List<MegacoolEvent> ();
        allCurrentEvents.Add(megacoolEvent);
        OnMegacoolEvents(allCurrentEvents);
        allCurrentEvents.Clear ();
    }

    private bool hasStarted = false;

    /// <summary>
    /// Deprecated initialization of SDK with an event handler
    /// </summary>
    [System.Obsolete("Use Start() and add callbacks to their respective delegates. See https://docs.megacool.co/customize")]
    public void Start(Action<MegacoolEvent> eventHandler) {
        _Start(eventHandler);
    }


    /// <summary>
    /// Initialize the SDK.
    /// </summary>
    public void Start() {
         // Create a main thread action for every asynchronous callback
        Action<MegacoolEvent> eventHandler = ((MegacoolEvent e) => Megacool.Instance.CreateMainThreadAction (e));
        _Start(eventHandler);
    }

    // Temporary extracted method to initialize SDK until deprecated Start is removed
    private void _Start(Action<MegacoolEvent> eventHandler){
        if (hasStarted) {
            // Allowing multiple initializations would make it hard to maintain both thread-safety and performance
            // of the underlying capture code, and doesn't have any good use case for allowing it, thus ignoring.
            UnityEngine.Debug.Log("Megacool: Skipping duplicate init");
            return;
        }
        hasStarted = true;

        // Set debugging first so that it can be enabled before initializing the native SDK
        Debug = MegacoolConfiguration.Instance.debugMode;

        EventHandler = eventHandler;

        // Delegates must be initialized before start() since start() might trigger the event callbacks.
        InitializeSharingDelegate();

#if UNITY_IOS && !UNITY_EDITOR
        startWithAppConfig(MegacoolConfiguration.Instance.appConfigIos);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.Start(MegacoolConfiguration.Instance.appConfigAndroid, eventHandler);
#endif

        SetupDefaultConfiguration();
        ManualApplicationDidBecomeActive();

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        int width = (int)(Screen.width / ScaleFactor);
        int height = (int)(Screen.height / ScaleFactor);
        mcl_init_capture(width, height, SystemInfo.graphicsDeviceType.ToString());
#endif

        IssuePluginEvent(MCRS);
        SignalRenderTexture(renderTexture);
    }

    private void SignalRenderTexture(RenderTexture texture) {
        if (!texture) {
            texture = RenderTexture;
            // this automatically does the signalling
            return;
        }
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        mcl_set_capture_texture(texture.GetNativeTexturePtr());
#endif
    }

    public void IssuePluginEvent(int eventId) {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        if (nativePluginCallbackPointer == IntPtr.Zero) {
            nativePluginCallbackPointer = mcl_get_unity_render_event_pointer();
        }
        GL.IssuePluginEvent(nativePluginCallbackPointer, eventId);
#endif
    }

    /// <summary>
    /// Start recording a GIF
    /// </summary>
    /// <remarks>
    /// This will keep a buffer of 50 frames (default). The frames are overwritten until <c>StopRecording</c> gets called.
    /// </remarks>
    public void StartRecording() {

        InitializeManager();

#if UNITY_IOS && !UNITY_EDITOR
        startRecording();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.StartRecording();
#endif
        _isRecording = true;
    }

    /// <summary>
    /// Start customized GIF recording.
    /// </summary>
    /// <remarks>
    /// This will keep a buffer of 50 frames (default). The frames are overwritten until <c>StopRecording</c> gets called.
    /// </remarks>
    /// <param name="config">Config to customize the recording.</param>
    public void StartRecording(MegacoolRecordingConfig config) {
        config.SetDefaults();

        InitializeManager();

#if UNITY_IOS && !UNITY_EDITOR
        startRecordingWithConfig(config.RecordingId, new Crop(new Rect(0,0,0,0)), config.MaxFrames, config.FrameRate, config.PeakLocation, config.OverflowStrategy.ToString());
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.StartRecording(config);
#endif
        _isRecording = true;
    }

    private void InitializeManager() {
        MegacoolManager manager = null;
        foreach (Camera cam in Camera.allCameras) {
            MegacoolManager foundManager = cam.GetComponent<MegacoolManager>();
            if (foundManager) {
                manager = foundManager;
                break;
            }
        }
        if (!manager) {
            Camera mainCamera = Camera.main;
            if (!mainCamera) {
                UnityEngine.Debug.Log("No MegacoolManager already in the scene and no main camera to attach to, " +
                    "either attach it manually to a camera or tag one of the cameras as MainCamera");
                return;
            }
            mainCamera.gameObject.AddComponent<MegacoolManager>();
            manager = mainCamera.GetComponent<MegacoolManager>();
        }
        // Doing an explicit initialize ensures that if the capture method was customized the changes
        // are respected even if the manager was explicitly added to a camera and thus awoke before the
        // capture method was set.
        manager.Initialize();
    }

    /// <summary>
    /// Note an event for highlight recording
    /// </summary>
    /// <remarks>
    /// For highlight recording use only. Calling this function when something interesting occurs means that the end
    /// recording will focus on the stretch of gameplay with the most interesting events logged.
    /// </remarks>
    public void RegisterScoreChange() {
        RegisterScoreChange(1);
    }

    /// <summary>
    /// Note a change in score for highlight recording
    /// </summary>
    /// <remarks>
    /// For highlight recording use only. Calling this function when the score changes means that the end recording
    /// will focus on the stretch of gameplay with the most frequent / highest value events logged.
    /// </remarks>
    public void RegisterScoreChange(int scoreDelta) {
#if UNITY_IOS && !UNITY_EDITOR
        registerScoreChange(scoreDelta);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.RegisterScoreChange(scoreDelta);
#endif
    }

    // Indicates whether this frame should be rendered. Used by the custom cameras to detect when CaptureFrame
    // has been called.
    public bool RenderThisFrame = false;

    /// <summary>
    /// Capture a single frame.
    /// </summary>
    /// <remarks>
    /// Capture a single frame to the buffer. The buffer size is 50 frames (default) and the oldest frames will be deleted if the method gets called more than 50 times.
    /// The total number of frames can be customized by setting the <c>MaxFrames</c> property.
    /// </remarks>
    public void CaptureFrame() {
        InitializeManager();
        RenderThisFrame = true;

#if UNITY_IOS && !UNITY_EDITOR
        captureFrame();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.CaptureFrame();
#endif
    }

    public void CaptureFrame(MegacoolFrameCaptureConfig config) {
        config.SetDefaults();

        InitializeManager();
        RenderThisFrame = true;

#if UNITY_IOS && !UNITY_EDITOR
        captureFrameWithConfig(config.RecordingId, config.OverflowStrategy.ToString(), new Crop(new Rect(0,0,0,0)), config.ForceAdd, config.MaxFrames, config.FrameRate);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.CaptureFrame(config);
#endif
    }

    public void PauseRecording() {
#if UNITY_IOS && !UNITY_EDITOR
        pauseRecording();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.PauseRecording();
#endif
        _isRecording = false;
    }

    /// <summary>
    /// Stops the recording.
    /// </summary>
    public void StopRecording() {
#if UNITY_IOS && !UNITY_EDITOR
        stopRecording();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.StopRecording();
#endif
        _isRecording = false;
    }

    /// <summary>
    /// Delete a recording
    /// </summary>
    /// <description>
    /// Will remove any frames of the recording in memory and on disk. Both completed and incomplete
    /// recordings will take space on disk, thus particularly if you're using <c>KeepCompletedRecordings = true</c> you might want
    /// to provide an interface to your users for removing recordings they don't care about anymore to free up space for new recordings.
    /// </description>
    /// <param name="recordingId">Recording identifier.</param>
    public void DeleteRecording(string recordingId) {
#if UNITY_IOS && !UNITY_EDITOR
        deleteRecording(recordingId);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.DeleteRecording(recordingId);
#endif
    }

    public string GetPreviewInfoForRecording(string recordingId) {
#if UNITY_IOS && !UNITY_EDITOR
        return getPreviewInfoForRecording(recordingId);
#elif UNITY_ANDROID && !UNITY_EDITOR
        return AndroidWrapper.GetPreviewInfoForRecording(recordingId);
#else
        throw new NotImplementedException("GetPreviewInfoForRecording not supported in editor");
#endif
    }

    public void setDefaultLastFrameOverlay(string filename){

#if UNITY_IOS && !UNITY_EDITOR
        String path = Application.streamingAssetsPath + "/" + filename;
        setLastFrameOverlay(path);
#elif UNITY_ANDROID && !UNITY_EDITOR

#endif
    }

    public void GetShares(Action<List<MegacoolShare>> shares) {
        OnSharesRetrieved = shares;
#if UNITY_IOS && !UNITY_EDITOR
        getShares();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.GetShares(shares);
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    private static Func<MegacoolShare, bool> deleteSharesFilter = delegate(MegacoolShare arg) {
        return true;
    };
#endif

    public void DeleteShares(Func<MegacoolShare, bool> filter) {
#if UNITY_IOS && !UNITY_EDITOR
        deleteSharesFilter = filter;
        deleteShares(Marshal.GetFunctionPointerForDelegate(new Func<MegacoolShareData, bool>(DeleteSharesFilter)));
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.DeleteShares(filter);
#endif
    }

    /// <summary>
    /// Share this instance.
    /// </summary>
    public void Share() {
#if UNITY_IOS && !UNITY_EDITOR
        presentShare();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.Share();
#endif
    }

    public void Share(MegacoolShareConfig config) {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareWithConfig(config.RecordingId, config.LastFrameOverlay, config.FallbackImage, config.Share.Url.ToString(), config.Share.Data != null ? MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data) : null);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.Share(config);
#endif
    }

    public void ShareToMessenger() {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMessenger();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMessenger();
#endif
    }

    public void ShareToMessenger(MegacoolShareConfig config) {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMessengerWithConfig(config.RecordingId, config.LastFrameOverlay, config.FallbackImage, config.Share.Url.ToString(), config.Share.Data != null ? MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data) : null);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMessenger(config);
#endif
    }

    public void ShareToTwitter() {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToTwitter();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToTwitter();
#endif
    }

    public void ShareToTwitter(MegacoolShareConfig config) {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToTwitterWithConfig(config.RecordingId, config.LastFrameOverlay, config.FallbackImage, config.Share.Url.ToString(), config.Share.Data != null ? MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data) : null);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToTwitter(config);
#endif
    }

    public void ShareToMessages() {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMessages();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMessages();
#endif
    }

    public void ShareToMessages(MegacoolShareConfig config) {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMessagesWithConfig(config.RecordingId, config.LastFrameOverlay, config.FallbackImage, config.Share.Url.ToString(), config.Share.Data != null ? MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data) : null);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMessages(config);
#endif
    }

    public void ShareToMail() {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMail();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMail();
#endif
    }

    public void ShareToMail(MegacoolShareConfig config) {
#if UNITY_IOS && !UNITY_EDITOR
        presentShareToMailWithConfig(config.RecordingId, config.LastFrameOverlay, config.FallbackImage, config.Share.Url.ToString(), config.Share.Data != null ? MegacoolThirdParty_MiniJSON.Json.Serialize(config.Share.Data) : null);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ShareToMail(config);
#endif
    }

    public void SubmitDebugData(string message) {
#if UNITY_IOS && !UNITY_EDITOR
        submitDebugDataWithMessage(message);
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.SubmitDebugData(message);
#endif
    }

    public void ResetIdentity() {
#if UNITY_IOS && !UNITY_EDITOR
        resetIdentity();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidWrapper.ResetIdentity();
#endif
    }

#endregion
}

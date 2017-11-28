using System;
using UnityEngine;

public struct MegacoolShareConfig {

    private static string dataPath = Application.streamingAssetsPath + "/";

    public string RecordingId { get; set; }


    private String lastFrameOverlay;

    public string LastFrameOverlay  {
        get {
            return lastFrameOverlay;
        }
        set {
            lastFrameOverlay = dataPath + value;
        }
    }

    private String fallbackImage;

    public string FallbackImage {
        get {
            return fallbackImage;
        }
        set {
#if UNITY_IOS && !UNITY_EDITOR
            fallbackImage = dataPath + value;
#else
            fallbackImage = value;
#endif
        }
    }


    private MegacoolShare share;

    public MegacoolShare Share {
        get {
            return share = share ?? new MegacoolShare("/");
        }
        set {
            share = value;
        }
    }
}

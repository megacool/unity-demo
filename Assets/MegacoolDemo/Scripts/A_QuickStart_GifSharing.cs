using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_QuickStart_GifSharing : MonoBehaviour {

    public Button startRecordingButton;
    public Button stopRecordingButton;
    public Button captureFrameButton;
    public Button shareButton;
    public Button previewButton;
    public Button removePreviewButton;

    public MegacoolGifPreview megacoolGifPreview;

    void Start() {
        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);
        captureFrameButton.onClick.AddListener(CaptureFrame);
        previewButton.onClick.AddListener(Preview);
        removePreviewButton.onClick.AddListener(RemovePreview);
        shareButton.onClick.AddListener(Share);

        startRecordingButton.interactable = true;
        captureFrameButton.interactable = true;
    }

    void StartRecording() {
        Debug.Log("Start recording");
        startRecordingButton.interactable = false;
        captureFrameButton.interactable = false;
        stopRecordingButton.interactable = true;
        Megacool.Instance.StartRecording ();
    }

    void StopRecording() {
        Debug.Log("Stop recording");
        stopRecordingButton.interactable = false;   
        captureFrameButton.interactable = false;
        shareButton.interactable = true;
        previewButton.interactable = true;
        removePreviewButton.interactable = true;
        Megacool.Instance.StopRecording();
    }

    void CaptureFrame() {
        Debug.Log("Capture frame");
        startRecordingButton.interactable = false;
        stopRecordingButton.interactable = true;
        Megacool.Instance.CaptureFrame();
    }

    void Preview() {
        Debug.Log("Preview");
        megacoolGifPreview.StartPreview();
    }

    void RemovePreview() {
        Debug.Log ("Remove preview");
        megacoolGifPreview.StopPreview();
    }
        
    void Share() {
        Debug.Log ("Share");
        Megacool.Instance.Share();
    }
}

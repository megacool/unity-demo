using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_CustomizeRecording : MonoBehaviour {

    public Button startRecordingButton;
    public Button stopRecordingButton;

    public InputField recordingFrameRateInputField;
    public InputField maxFramesInputField;
    public InputField lastFrameDelayInputField;

    public Button previewButton;
    public Button removePreviewButton;

    public MegacoolGifPreview megacoolGifPreview;

    void Start() {
        Megacool.Instance.Start();

        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);

        recordingFrameRateInputField.onValueChanged.AddListener( (string newValue) => {
            int m_parsedInt = 0;
            Int32.TryParse(newValue, out m_parsedInt);
            Megacool.Instance.FrameRate = (float)m_parsedInt;
        });

        maxFramesInputField.onValueChanged.AddListener( (string newValue) => {
            int m_parsedInt = 0;
            Int32.TryParse(newValue, out m_parsedInt);
            Megacool.Instance.MaxFrames = m_parsedInt;
        });

        lastFrameDelayInputField.onValueChanged.AddListener( (string newValue) => {
            int m_parsedInt = 0;
            Int32.TryParse(newValue, out m_parsedInt);
            Megacool.Instance.LastFrameDelay = m_parsedInt;
        });

        previewButton.onClick.AddListener(Preview);
        removePreviewButton.onClick.AddListener(RemovePreview);
    }

    void SetInitialInputFieldValues() {
        recordingFrameRateInputField.text = Megacool.Instance.FrameRate.ToString();
        maxFramesInputField.text = Megacool.Instance.MaxFrames.ToString();
        lastFrameDelayInputField.text = Megacool.Instance.LastFrameDelay.ToString();
    }

    void StartRecording() {
        Debug.Log("Start recording. Lock inputfields");

        LockInputFields();

        Megacool.Instance.StartRecording();
    }

    void StopRecording() {
        Debug.Log("Stop Recording");
        previewButton.interactable = true;
        removePreviewButton.interactable = true;
        Megacool.Instance.StopRecording();
    }

    void LockInputFields() {
        recordingFrameRateInputField.interactable = false;
        maxFramesInputField.interactable = false;
        lastFrameDelayInputField.interactable = false;
    }

    void Preview() {
        Debug.Log ("Preview");
        megacoolGifPreview.StartPreview();
    }

    void RemovePreview() {
        Debug.Log ("Remove preview");
        megacoolGifPreview.StopPreview();
    }
}

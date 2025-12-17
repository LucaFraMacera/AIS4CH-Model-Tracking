// Copyright (c) Meta Platforms, Inc. and affiliates.

using Meta.XR.MRUtilityKit;
using Meta.XR.Samples;

using System.Linq;

using TMPro;

using UnityEngine;
public class MyQRCode : MonoBehaviour
{
    public string PayloadText => _text.text;

    [SerializeField]
    TMP_Text _text;

    [SerializeField]
    TMP_Text _trackingStateText;

    [SerializeField]
    TMP_Text _modelNameText;

    [SerializeField]
    TMP_Text _modelDescriptionText;

    [SerializeField]
    RectTransform _background;

    MRUKTrackable _trackable;

    bool loading = false;

    Model model;

    public void Initialize(MRUKTrackable trackable)
    {
        this.loading = true;
        if (trackable.MarkerPayloadString is { } str)
        {
            _text.text = $"\"{str}\"";
        }
        else if (trackable.MarkerPayloadBytes is { } bytes)
        {
            _text.text = $"Binary(data=[{string.Join(" ", bytes.Take(16).Select(b => $"{b:x02}"))}{(bytes.Length > 16 ? " ..." : "")}], length={bytes.Length})";
        }
        else
        {
            _text.text = "(no payload)";
        }

        this._modelNameText.text = "";
        this._modelDescriptionText.text = "";

        _trackable = trackable;
        SetTrackingStateText();

        if (!_background)
        {
            return;
        }

        _text.ForceMeshUpdate();

        var bounds = _text.textBounds;
        _background.position = _text.transform.TransformPoint(bounds.center);

        var size = bounds.size;
        size.x += 16f;
        size.y += 16f;
        _background.sizeDelta = size;
    }

    void Update(){
        if(this.model != null) {
            this._modelNameText.text = model.name;
            this._modelDescriptionText.text = model.ToString();
        } else if(loading) {
            this._modelNameText.text = "Caricando...";
        } else {
            this._modelNameText.text = "Modello non trovato...";
        }
        SetTrackingStateText();
    }

    void SetTrackingStateText() => _trackingStateText.text = _trackable
        ? _trackable.IsTracked ? "Tracked" : "Untracked"
        : "(none)";

    public void SetModel(Model model) {
        this.loading = false;
        this.model = model;
    }
}

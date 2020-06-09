﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEEP.Entities;
using DEEP.UI;

[RequireComponent(typeof(Mesh))]
public class LiquidSound : MonoBehaviour
{
    [Header("Ambient liquid settings")]
    [SerializeField] AudioClip ambientLiquidSound;
    [SerializeField] float extraHearingRange, decayTangent, ambientVolume, ambientSpatialBlend;

    [Header("Inside liquid settings")]
    [SerializeField] AudioClip insideLiquidSound;
    [SerializeField] float insideVolume;

    private MeshRenderer _mesh;
    private AudioSource _audio;

    void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _audio = Instantiate(new GameObject("_audio"), _mesh.bounds.center, Quaternion.identity, this.transform).AddComponent<AudioSource>();   
        float minHearingRange = _mesh.bounds.extents.magnitude;

        _audio.maxDistance = minHearingRange + extraHearingRange;
        _audio.volume = ambientVolume;
        _audio.spatialBlend = ambientSpatialBlend;
        _audio.loop = true;
        _audio.rolloffMode = AudioRolloffMode.Custom;

        AnimationCurve volumeCurve = new AnimationCurve();

        volumeCurve.AddKey(0f, 1f);
        volumeCurve.AddKey(new Keyframe(minHearingRange, 1f, 0f, decayTangent));
        volumeCurve.AddKey(new Keyframe(minHearingRange + extraHearingRange, 0f));
        
        _audio.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);

        AnimationCurve spatialBlendCurve = new AnimationCurve();

        spatialBlendCurve.AddKey(0f, 0f);
        spatialBlendCurve.AddKey(new Keyframe(minHearingRange, 0f, 0f, -decayTangent));
        spatialBlendCurve.AddKey(new Keyframe(minHearingRange + extraHearingRange, 1f));

        _audio.SetCustomCurve(AudioSourceCurveType.SpatialBlend, spatialBlendCurve);

        if(ambientLiquidSound != null)
        {
            _audio.clip = ambientLiquidSound;
            _audio.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player.Instance.HUD.StartScreenFeedback(HUDController.FeedbackType.Toxic);

            if(insideLiquidSound != null)
            {
                _audio.clip = insideLiquidSound;
                _audio.volume = insideVolume;
                _audio.spatialBlend = 0f; // No blend because you're right on top of it
                _audio.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player.Instance.HUD.StopConstantScreenFeedback();

            if(ambientLiquidSound != null)
            {
                _audio.clip = ambientLiquidSound;
                _audio.volume = ambientVolume;
                _audio.spatialBlend = ambientSpatialBlend;
                _audio.Play();
            }
        }
    }

}

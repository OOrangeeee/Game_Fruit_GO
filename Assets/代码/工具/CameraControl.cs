using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource cinemachineImpulseSource;
    public VoidObjectSO camaraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        camaraShakeEvent.onEventRaised += OnCamaraShake;
    }

    private void OnDisable()
    {
        camaraShakeEvent.onEventRaised -= OnCamaraShake;
    }

    private void OnCamaraShake()
    {
        cinemachineImpulseSource.GenerateImpulse();
    }

    private void Start()
    {
        GetNewCamaraBounds();
    }

    private void GetNewCamaraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("bounds");
        if(obj != null)
        {
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}

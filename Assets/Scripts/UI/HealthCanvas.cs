using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private Image progressBar;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void ChangeBar(float t)
    {
        progressBar.fillAmount = t;
    }

    private void Update()
    {
        canvas.transform.LookAt(transform.position + canvas.worldCamera.transform.rotation * Vector3.back, canvas.worldCamera.transform.rotation * Vector3.up);
    }
}

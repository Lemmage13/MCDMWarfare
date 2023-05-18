using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UItxtIndicator : MonoBehaviour
{
    private Camera mCam;
    private TMP_Text text;
    private void Awake()
    {
        mCam = Camera.main;
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        float newX = mCam.transform.position.x + 4 - (mCam.orthographicSize / 5) * 9;
        float newY = mCam.transform.position.y - mCam.orthographicSize + 0.5F;
        //float scaleFactor = mCam.orthographicSize / 5;
        transform.position = new Vector3(newX, newY, mCam.transform.position.z + 1);
        //transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
    public void UpdateText(string newtxt)
    {
        text.text = newtxt;
    }
}

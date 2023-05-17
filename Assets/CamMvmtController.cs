using UnityEngine;

public class CamMvmtController : MonoBehaviour
{
    private Camera mCam;
    private float targetZoom;
    private float zoomStrength = 2F;
    private float zoomLerpSpeed = 10F;
    private float minZoom = 10F;
    private float maxZoom = 3F;

    private Vector3 camCentre;
    private float mvmtLimit = 5;
    private float camSpeed = 0.1F;

    // Start is called before the first frame update
    void Start()
    {
        mCam = Camera.main;
        targetZoom = mCam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomStrength;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        mCam.orthographicSize = Mathf.Lerp(mCam.orthographicSize, targetZoom, Time.deltaTime*zoomLerpSpeed);

        float xAxisDelta = Input.GetAxis("Horizontal");
        float yAxisDelta = Input.GetAxis("Vertical");
        float newX = Mathf.Clamp(transform.position.x + xAxisDelta * camSpeed, camCentre.x - mvmtLimit, camCentre.x + mvmtLimit);
        float newY = Mathf.Clamp(transform.position.y + yAxisDelta * camSpeed, camCentre.y - mvmtLimit, camCentre.y + mvmtLimit);
        transform.position = new Vector3(newX, newY, -10);
    }
    public void SetCamCentre(Vector3 newCentre)
    {
        camCentre = newCentre;
        transform.position = camCentre;
    }
}

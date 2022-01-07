using System.Collections;
using System.Collections.Generic;
using easyar;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject Temp;
    public ARSession Session;
    public Camera Camera;
    public SurfaceTrackerFrameFilter tracker;


    private bool done = false;

    void Awake()
    {
        Camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            /*
            Touch touch = Input.GetTouch(0);

            var viewPoint = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height);
            if (tracker && tracker.Tracker != null && Session.FrameCameraParameters.OnSome)
            {
                var coord = EasyARController.Instance.Display.ImageCoordinatesFromScreenCoordinates(viewPoint, Session.FrameCameraParameters.Value, Session.Assembly.Camera);
                tracker.Tracker.alignTargetToCameraImagePoint(coord.ToEasyARVector());
            }
            */
        }
    }
}

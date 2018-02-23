using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GPSSystem : MonoBehaviour {
    public delegate void OnGPSReady();
    public OnGPSReady OnGPSReadyEvent;
    public static GPSSystem Instance;
    public float latitude, longtitude, altitude, horizontalAccuracy;
    public double timestamp;
    public bool dataReady = false, initial=false;
    [System.Serializable]
    public class Point {
        public double x;
        public double y;

        public Point(double x, double y) {
            this.x = x;
            this.y = y;
        }        
    }
    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        Debug.Log("GPS Start");
        startGPSService();

        Point[] kmutt = new Point[] { new Point(13.650635, 100.491120),
        new Point(13.654107, 100.494285),
        new Point(13.652971, 100.4946793),
        new Point(13.650929, 100.496504),
        new Point(13.649295, 100.494114),
        new Point(13.650635, 100.491120)};

        //Debug.Log(IsInPolygon(kmutt,new Point(13.648325, 100.493218)));
        /*Debug.Log((100.4910347 - 100.491120) * (13.654107 - 13.650635) - (13.6505575 - 13.650635) * (100.494285 - 100.491120));
        Debug.Log((100.4910347 - 100.491120) * (13.654107 - 13.650635) - (13.6505575 - 13.650635) * (100.494285 - 100.491120));
        /*Point[] pts = new Point[] { new Point ( 1, 1 ),
                                        new Point (1, 3 ),
                                        new Point ( 3, 3 ),
                                        new Point ( 3,0 ), new Point ( 1, 1 ),};        
        Debug.Log(IsInPolygon(pts, new Point(2, 0)));*/
        //Debug.Log(IsInPolygon(kmutt, new Point(13.652419d, 100.49368d)));
    }

    IEnumerator getGPSLocation() {
        initial = true;
        //Debug.Log("Start");
        yield return new WaitForSeconds(5);
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            //Popup.Instance.showPopup("GPS System" , "Location ไม่ได้ถูกเปิดใช้งาน");
            yield break;
        }
        //Debug.Log("Location Enable");
        // Start service before querying location
        Input.location.Start();
        Debug.Log(Input.location.status);
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            //Popup.Instance.showPopup("GPS System" , "ไม่สามารถเข้าถึง GPS ได้");
            Debug.Log("Unable to determine device location");
            yield break;
        } else {
            // Access granted and location value could be retrieved
            maxWait = 20;
            while (Input.location.status != LocationServiceStatus.Running && maxWait > 0) {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            // Service didn't running in 20 seconds
            if (maxWait < 1) {
                Debug.Log("Timed Out for wait Running");
                initial = false;
                yield break;
            }
            if(OnGPSReadyEvent!=null)
                OnGPSReadyEvent();
            dataReady = true;
            Debug.Log(Input.location.status);
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
        latitude = Input.location.lastData.latitude;
        longtitude = Input.location.lastData.longitude;        
        initial = false;
    }

    public void startGPSService() {
        //if not initial GPS Service
        if (!initial)
            StartCoroutine("getGPSLocation");
    }

    public void stopGPSService() {
        dataReady = false;
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    private void FixedUpdate() {
        if (dataReady) {
            latitude = Input.location.lastData.latitude;
            longtitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
            timestamp = Input.location.lastData.timestamp;
            fps.Instance.gpsText = "[GPS] Latitude : " + latitude +" Longitude : " + longtitude;
        }
    }

    public Point getCurrentLocationPoint() {
        return new Point(this.latitude, this.longtitude);
    }

    public static bool IsInPolygon(Point[] poly, Point point) { 

        var coef = poly.Skip(1).Select((p, i) =>
                                        (point.y - poly[i].y) * (p.x - poly[i].x)
                                      - (point.x - poly[i].x) * (p.y - poly[i].y))
                                .ToList();

        if (coef.Any(p => p == 0))
            return true;
        /*foreach(var val in coef) {
            Debug.Log(val);
        }*/
        for (int i = 1; i < coef.Count(); i++) {
            //Debug.Log(i);   
            if (coef[i] * coef[i - 1] < 0)
                return false;
        }
        return true;
    }

}
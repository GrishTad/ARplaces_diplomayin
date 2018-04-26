using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CompassController : MonoBehaviour {
    public static CompassController instance;
    // Use this for initialization
    public List<GameObject> places = new List<GameObject>();
    public float desiredAngle = 25;
    public GameObject origin;
    string closestObjectName=null;

    public Text nearintsObjectName;

    public Text CompassData;

    private void Awake()
    {
        instance = this;
    }
    void Start () {
        Input.location.Start();
        Input.compass.enabled = true;
        StartCoroutine(UpdateRotationOfOrigin());
    }
	
	// Update is called once per frame
	void Update () {

      


    }

    public IEnumerator UpdateRotationOfOrigin()
    {
        yield return new WaitForSeconds(0.5f);
        if (origin)
        {

            origin.transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
            CompassData.text = origin.transform.rotation.y.ToString()+ "   "+ origin.transform.eulerAngles.y;
            Debug.Log(origin.transform.rotation);
            

        }
        StartCoroutine(UpdateRotationOfOrigin());
    }


    public IEnumerator CheckNearistObject()
    {
        
        yield return new WaitForSeconds(5);

        try
        {
            if (closestObjectName != CompassController.instance.CheckObjects().name)
            {
                Debug.Log("<color=green>" + CompassController.instance.CheckObjects().name + "</color>");
                closestObjectName = CompassController.instance.CheckObjects().name;
                getDataFromWikipedia(closestObjectName);
            }
                     
        }
        catch
        {
            Debug.Log("<color=red>Dont Have a places</color>");
        }
        StartCoroutine(CheckNearistObject());

    }

    public void getDataFromWikipedia(string name)
    {
        nearintsObjectName.text = name;
        WikipediaSearch.instance.searchinfo(name);
    }
    public GameObject CheckObjects()
    {
        List<GameObject> InAngle = new List<GameObject>();

        for (int i = 0; i < places.Count; i++)
        {
            GameObject tested = places[i];

            Vector3 dir = tested.transform.position - origin.transform.position;
            float angle = Vector3.Angle(dir, origin.transform.forward);

            if (angle <= desiredAngle)
            {
                InAngle.Add(tested);
            }
        }

        if (InAngle.Count == 0) return null;

        GameObject closest = InAngle[0];
        float closesDistance = Vector3.Distance(closest.transform.position, origin.transform.position);

        for (int i = 1; i < InAngle.Count; i++)
        {
            float d = Vector3.Distance(InAngle[i].transform.position, origin.transform.position);

            if (d < closesDistance && d> 0.2f)
            {
                closesDistance = d;
                closest = InAngle[i];
            }
        }
        if (closesDistance <= 0.2f)
        {
            return null;
        }
        return closest;
    }




}

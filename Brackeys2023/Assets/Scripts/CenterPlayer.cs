using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlayer : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> objects = new List<GameObject>();
    public float offset = 0.01f;
    public float distance = 0.0f;
    public bool on = true;
    public Vector3 realpos = Vector3.zero;
    void Start()
    {
        foreach(GameObject gam in GameObject.FindGameObjectsWithTag("obj")){
            objects.Add(gam);
        }
    }
    public void AddThing(GameObject what)
    {
        objects.Add(what);
    }
    public void DestroyThing(GameObject what){
        objects.Remove(what);
        Destroy(what);
    }
    public void ForceMove(Vector3 neworigin){
        if(neworigin.magnitude > offset){
            realpos += neworigin;
            distance += neworigin.magnitude;
            foreach(GameObject obj in objects){
                obj.transform.position -= neworigin;
            }
        }
    }
    void LateUpdate()
    {
        if(!on){
            return;
        }
        Vector3 neworigin = player.transform.position;
        if(neworigin.magnitude > offset){
            realpos += neworigin;
            distance += neworigin.magnitude;
            foreach(GameObject obj in objects){
                obj.transform.position -= neworigin;
            }
        }
    }
}

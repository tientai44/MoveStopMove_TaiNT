using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : GOSingleton<CameraFollow>
{
    [SerializeField] Transform player;
    Vector3 intialOffset = new Vector3(0, 15, -30);
    Vector3 offset = new Vector3(0, 15, -30);
    [SerializeField]Vector3 zoomInOffset = new Vector3(0,5,-15);
    public Vector3 Offset { get => offset; set => offset = value; }
    private void Start()
    {
        GetInstance();
    }
    // Update is called once per frame
    void Update()
    {
        if(player != null)
            transform.position = Vector3.Lerp(transform.position, player.position+offset,0.5f);
    }
    public void SetTargetFollow(Transform target)
    {
        player = target;
    }
    public void ResetOffset()
    {
        offset = intialOffset;
    }
    public void ZoomOut()
    {
        GameController.GetInstance().currentPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
        offset = intialOffset;
    }
    public void ZoomIn()
    {
        GameController.GetInstance().currentPlayer.transform.rotation = new Quaternion(0, 180, 0, 0);
        offset = zoomInOffset;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum CameraState
{
    ZoomIn,ZoomOut
}
public class CameraFollow : GOSingleton<CameraFollow>
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] CharacterController player;
    Vector3 intialOffset = new Vector3(0, 15, -20);
    [SerializeField]Vector3 offset = new Vector3(0, 15, -20);
    [SerializeField]Vector3 zoomInOffset = new Vector3(0,5,-10);
    float speedCamera = 0.2f;
    private Vector3 velocity = Vector3.zero;

    public Vector3 Offset { get => offset; set => offset = value; }
    private void Start()
    {
        GetInstance();
        curve.Evaluate(0.5f);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)// Camera di theo player
        {
            //transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, speedCamera);
            //Dung Lerp cho smooth 
            transform.position = Vector3.Lerp(transform.position, player.TF.position + offset  , speedCamera);
        }
    }
    public void SetTargetFollow(CharacterController target)
    {
        player = target;
    }
    public void ResetOffset()// Tra offset ve gia tri ban dau
    {
        offset = intialOffset;
    }
    public void ZoomOut()
    {
        GameController.GetInstance().currentPlayer.transform.rotation = new Quaternion(0, 0, 0, 0);
        offset = intialOffset;
        speedCamera = 0.2f;
    }
    public void ZoomIn()
    {
        GameController.GetInstance().currentPlayer.transform.rotation = new Quaternion(0, 180, 0, 0);
        offset = zoomInOffset + new Vector3(0, 1, -1) *player.Point;
        speedCamera = 0.1f;
    }

}

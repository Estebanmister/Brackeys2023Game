using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;

    float viewx, viewy = 0;
    public float speed = 0.2f;
    public float sensitivity = 0.5f;
    public float responsiveness = 10f;
    GameObject cam;

    public AnimationCurve drag_forwards;
    public AnimationCurve drag_sideways;
    public AnimationCurve drag_vertical;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        cam = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        float forwards = playerInput.actions["forward"].ReadValue<float>();
        float sideways = playerInput.actions["sideways"].ReadValue<float>();
        float vertical = playerInput.actions["vertical"].ReadValue<float>();
        rb.AddForce(cam.transform.rotation * new Vector3(sideways, vertical, forwards) * Time.fixedDeltaTime * speed );

        Vector3 rotated_velocity = transform.InverseTransformVector(rb.velocity);
        Vector3 drags = new Vector3(drag_forwards.Evaluate(rotated_velocity.x),drag_sideways.Evaluate(rotated_velocity.y),drag_vertical.Evaluate(rotated_velocity.z));
        Vector3 dragCoeff = rb.velocity;
        dragCoeff.Scale(drags);
        Vector3 dragForce = dragCoeff.magnitude * rotated_velocity.sqrMagnitude * -rotated_velocity.normalized;
        rb.AddRelativeForce(dragForce);

        if(Cursor.lockState == CursorLockMode.Locked){
            if(playerInput.actions["escape"].IsPressed()){
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            Vector2 view = Camera.main.ScreenToViewportPoint(playerInput.actions["look"].ReadValue<Vector2>())*sensitivity;
            viewx += view.x;
            viewy += view.y;
            if(viewx > 359){
                viewx -= 359;
            }
            if(viewx < 0){
                viewx += 359;
            }
            if(viewy > 359){
                viewy -= 359;
            }
            if(viewy < 0){
                viewy += 359;
            }
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(new Vector3(-viewy,viewx)), 1.0f/responsiveness);
        }
    }
}

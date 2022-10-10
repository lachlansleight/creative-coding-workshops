using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ObjectPlacement : MonoBehaviour
{

    public Collider GroundCollider;
    public UnityEvent OnObjectPlaced;
    
    private GameObject _objectBeingPlaced;
    private bool _isPlacingObject;
    private Camera _mainCam;

    public void Awake()
    {
        //do this on awake because it's inefficient to do every frame
        _mainCam = Camera.main;
    }

    //This method is called by the UI button's OnClick event
    public void HandleObjectButtonClicked(GameObject prefab)
    {
        //If we're currently placing an object, that means that the player clicked the X button
        //So we destroy the temporary preview object and go back to the initial state
        if (_isPlacingObject) {
            Destroy(_objectBeingPlaced);
            _isPlacingObject = false;
            return;
        }
        
        //Spawn a new temporary object to indicate where the new object should go
        _objectBeingPlaced = Instantiate(prefab);
        
        //This will interfere with our raycasts trying to hit the ground!
        var colliders = _objectBeingPlaced.GetComponentsInChildren<Collider>();
        foreach(var c in colliders) c.enabled = false;
        
        //Random rotation for pretty variety :)
        _objectBeingPlaced.transform.rotation = Quaternion.Euler(0f, Random.value * 360f, 0f);
        
        _isPlacingObject = true;
    }

    public void Update()
    {
        if (!_isPlacingObject) return;
        
        //Get a ray indicating the mouse position
        var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        
        //If we didn't hit the ground, disable the preview object, otherwise move it to where the mouse cursor is
        var hasGroundCast = GroundCollider.Raycast(ray, out var groundHit, float.MaxValue);
        if (!hasGroundCast) {
            _objectBeingPlaced.SetActive(false);
            return;
        } else {
            _objectBeingPlaced.SetActive(true);
            _objectBeingPlaced.transform.position = groundHit.point;
        }

        //If we're mousing over the UI, we shouldn't do any more placement stuff
        var hasAllCast = Physics.Raycast(ray, out var firstHit, float.MaxValue);
        if (!hasAllCast) return;
        if (LayerMask.LayerToName(firstHit.collider.gameObject.layer) == "UI") return;
        
        //If the player clicks, place the object!
        if (Input.GetMouseButtonDown(0)) {
            //re-enable colliders
            var colliders = _objectBeingPlaced.GetComponentsInChildren<Collider>();
            foreach(var c in colliders) c.enabled = true;

            //start growing!
            _objectBeingPlaced.GetComponent<TreeGrowth>().IsGrowing = true;
            
            _objectBeingPlaced = null;
            _isPlacingObject = false;
            
            //This event is used by the UI to go back to the normal button state
            OnObjectPlaced.Invoke();
        }
    }
}

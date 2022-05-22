using System.Collections;
using UnityEngine;

public class RayShooter : FireAction
{
    private Camera camera;
    private int _damage = 10;
    private int _xHalfScreen;
    private int _yHalfScreen;

    protected override void Start()
    {
        base.Start();
        camera = GetComponentInChildren<Camera>();
        _xHalfScreen = Camera.main.pixelWidth / 2;
        _yHalfScreen = Camera.main.pixelHeight / 2;
    }
    

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }

        if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }    
    }
    

    protected override void Shooting()
    {
        base.Shooting();
        if (bullets.Count > 0) 
            StartCoroutine(Shoot());
    }
    
    private IEnumerator Shoot()
    {
        if (reloading)
        {
            yield break;
        }
        
        var point = new Vector3(_xHalfScreen, _yHalfScreen, 0);
        var ray = camera.ScreenPointToRay(point);
        CmdServerShoot(transform.position, ray.GetPoint(100f));
        yield return new WaitForSeconds(2.0f);
    }
}


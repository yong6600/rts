using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerHolder : SingletonMonobehaviour<ManagerHolder>
{

	DataManager dataManager;
	SoundManager soundManager;

	GridManager gridManager;

	InputManager inputManger;
	CameraManager cameraManager;


	UnityEngine.Component AddManager<T>()
    {
		GameObject newGameObject = new GameObject(typeof(T).ToString());
		newGameObject.transform.SetParent(transform);
		return newGameObject.AddComponent(typeof(T));
    }

	// Use this for initialization
	void Start ()
	{

		// 30프레임으로 고정
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 30;
		Application.runInBackground = true;

		this.dataManager = (DataManager)AddManager<DataManager>();
		this.soundManager = (SoundManager)AddManager<SoundManager>();

        this.gridManager = (GridManager)AddManager<GridManager>();
        this.gridManager.Init();

        this.inputManger = (InputManager)AddManager<InputManager>();
        this.cameraManager = (CameraManager)AddManager<CameraManager>();
        Debug.Log("Change Scene");
		SceneManager.LoadScene("InGame");
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (this.inputManger == null)
        {
            Debug.Log("input call");
            //this.inputManger.TouchUpdate();
        }
    }

    private void LateUpdate()
    {
        if (this.cameraManager != null)
        {
            //this.cameraManager.CameraUPdate();
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Button exitView;
    private bool paused;
    public GameObject pausePanel;

    private Transform oldCameraTran;

    private void Start()
    {
        pausePanel.SetActive(false);
        exitView.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void ExitView()
    {
        GameObject playerCharacters = GameObject.Find("PlayerCharacters");
        CinemachineVirtualCamera virtCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        CameraMove cameraMove = virtCam.GetComponent<CameraMove>();
        oldCameraTran = playerCharacters.GetComponent<InteractActions>().oldCameraTran;
        Transform[] oldCameras = null;
        //oldCameras.Append(oldCameraTran);

        if (cameraMove != null)
        {
            cameraMove.MoveCameraToRoom(oldCameras);

            playerCharacters.GetComponent<PlayerMovement>().enabled = true;

            exitView.gameObject.SetActive(false);
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}

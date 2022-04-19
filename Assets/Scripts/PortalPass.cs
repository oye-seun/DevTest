using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPass : MonoBehaviour
{
    public bool passed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !passed)
        {
            passed = true;
            FinishedGame();
        }
    }

    private Vector3 endcampos = new Vector3(-2.466505f, 9.754856f, 14.27727f);
    private Vector3 endcamrot = new Vector3(14.541f, 67.312f, 0f);

    private void FinishedGame()
    {
        CamState endcam = new CamState(63, endcampos, endcamrot);
        CameraControl.MoveCam(endcam, 2f, () => {
            GameManager.PlayPanel.SetActive(false);
            GameManager.GameoverPanel.SetActive(true);
            GameManager.ShowInfo("Thanks for playing...");
        });

    }
}

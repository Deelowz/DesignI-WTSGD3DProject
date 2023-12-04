using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDoor : MonoBehaviour
{
    // Start is called before the first frame update

    public InventoryManagerTwo inventoryManagerTwo;
    public ParticleSystem particles;

    public bool emerald = false;
    public bool ruby = false;
    public bool sapphire = false;

    public Camera mainCamera;
    public Camera cutSceneCamera;
    public Animator fadeScreen;

    private void OnMouseDown()
    {
        inventoryManagerTwo.GemDoorCheck();
    }

    public void RevealGem(string name)
    {
        if (name == "Emerald")
        {
            emerald = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (name == "Ruby")
        {
            ruby = true;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (name == "Sapphire")
        {
            sapphire = true;
            transform.GetChild(2).gameObject.SetActive(true);
        }

        if (emerald == true && ruby == true && sapphire == true)
        {
            fadeScreen.Play("ScreenFadeUnfade");
            Invoke("CameraFade", 2);
        }
    }

    public void CameraFade()
    {
        mainCamera.gameObject.SetActive(false);
        cutSceneCamera.gameObject.SetActive(true);
        cutSceneCamera.gameObject.GetComponent<Animator>().Play("Cutscene1Camera");
    }

}

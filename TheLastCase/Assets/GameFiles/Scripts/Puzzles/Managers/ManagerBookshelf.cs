using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBookshelf : MonoBehaviour
{
    public BookshelfHover BookshelfHover;

    public GameObject bookshelfCam;
    public CinemachineVirtualCamera VirtualCamera;

    void Start()
    {
        BookshelfHover.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bookshelfCam.transform.position == VirtualCamera.transform.position)
        {
            BookshelfHover.enabled = true;
        }
        else
        {
            BookshelfHover.enabled = false;
        }
    }
}

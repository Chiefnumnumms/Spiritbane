using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void DebugWarp()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            LoadScene(0);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            LoadScene(2);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            LoadScene(3);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            LoadScene(4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugWarp();
    }
}

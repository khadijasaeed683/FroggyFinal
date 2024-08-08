using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] scrollViewPrefabs;
    private Dictionary<string, GameObject> instantiatedScrollViews = new Dictionary<string, GameObject>();




    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    // Function to show panel by name
    public void ShowPanel(string panelName)
    {
        Debug.Log("ShowPanel called with name: " + panelName);

        // Deactivate all panels first
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                Debug.Log("Deactivating panel: " + panel.name);
                panel.SetActive(false);

                // Stop the video if there is a VideoPlayer component
                VideoPlayer videoPlayer = panel.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.Stop();
                    Debug.Log("Stopped video in panel: " + panel.name);
                }
            }
        }

        // Find the panel with the given name and activate it
        bool panelFound = false;
        foreach (GameObject panel in panels)
        {
            if (panel.name == panelName)
            {
                Debug.Log("Activating panel: " + panelName);
                panel.SetActive(true);
                panelFound = true;
                break;
            }
        }

        if (!panelFound)
        {
            Debug.LogError("Panel not found: " + panelName);
        }
    }

    // Function to instantiate and show a ScrollView prefab by name
    public void ShowScrollView(string scrollViewName)
    {
        Debug.Log("ShowScrollView called with name: " + scrollViewName);

        // Deactivate all instantiated ScrollViews first
        foreach (var scrollView in instantiatedScrollViews.Values)
        {
            scrollView.SetActive(false);
        }

        // Check if the ScrollView is already instantiated
        if (instantiatedScrollViews.ContainsKey(scrollViewName))
        {
            Debug.Log("Activating existing ScrollView: " + scrollViewName);
            instantiatedScrollViews[scrollViewName].SetActive(true);
        }
        else
        {
            // Find the ScrollView prefab with the given name and instantiate it
            GameObject scrollViewPrefab = null;
            foreach (GameObject prefab in scrollViewPrefabs)
            {
                if (prefab.name == scrollViewName)
                {
                    scrollViewPrefab = prefab;
                    break;
                }
            }

            if (scrollViewPrefab != null)
            {
                Debug.Log("Instantiating new ScrollView: " + scrollViewName);
                GameObject instantiatedScrollView = Instantiate(scrollViewPrefab, transform);
                instantiatedScrollViews[scrollViewName] = instantiatedScrollView;
            }
            else
            {
                Debug.LogError("ScrollView prefab not found: " + scrollViewName);
            }
        }



    }
    public void AddButtonClickSound()
    {
        // Call the AudioManager to play the button click sound
        AudioManager.Instance.PlayButtonClickSound();
        // Add any additional logic for the button click here
    }

    public void ShowCanvas(GameObject canvasName) {
       canvasName.SetActive(true);
    }
}

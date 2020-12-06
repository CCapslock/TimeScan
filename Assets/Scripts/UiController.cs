using UnityEngine;

public class UiController : MonoBehaviour
{
    public GameObject WinPanel;

    public void ActivateWinPanel(float DelatTime)
    {
        Invoke("SetPanelActive", DelatTime);
    }
    public void DisActivateWinPanel()
    {
        WinPanel.SetActive(false);
    }
    private void SetPanelActive()
    {
        WinPanel.SetActive(true);
    }
}

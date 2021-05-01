using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete]

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject HirePanel;
    [SerializeField] GameObject MyEmpPanel;
    [SerializeField] GameObject ButtonsPanel;


    [SerializeField] GameObject FixBugsPanel;
    [SerializeField] GameObject AddNewFeaturePanel;
    [SerializeField] GameObject PortToNewOSPanel;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void OpenPanel()
    {
        if (!gameObject.active){
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

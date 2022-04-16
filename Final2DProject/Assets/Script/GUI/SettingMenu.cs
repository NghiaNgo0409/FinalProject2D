using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] GameObject settingMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOff()
    {
        settingMenu.SetActive(false);
    }

    public void TurnOn()
    {
        settingMenu.SetActive(true);
    }
}

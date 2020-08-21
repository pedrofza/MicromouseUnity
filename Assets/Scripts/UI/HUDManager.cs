using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Zenject;

public class HUDManager : MonoBehaviour
{
    private RobotSerialBus bus;
    [SerializeField] private TMP_InputField comPortField;
    [SerializeField] private TMP_InputField baudRateField;
    [SerializeField] private UnityEngine.UI.Button connectButton;

    [Inject]
    void Construct(RobotSerialBus serialBus)
    {
        Debug.Log("Hud constructed");
        this.bus = serialBus;
    }

    public void HandleConnect()
    {
        if (!bus.IsConnected)
        {
            int br = Convert.ToInt32(baudRateField.text);
            bus.Configure(comPortField.text, br);
            bus.Connect();
            connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Disconnect";
        }
        else
        {
            bus.Disconnect();
            connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connect";
        }
    }
}

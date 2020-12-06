using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text text;

    private const int LOG_SIZE = 20;
    static private string[] log;
    static private int logIndex;
    static private bool modified = false;
    static private bool isUsable = false;

    #region Unity Lifecycles

    void Start()
    {
        log = new string[LOG_SIZE];
        for (var i = 0; i < LOG_SIZE; i++) {
            log[i] = "";
        }
        logIndex = 0;
        if (text != null) {
            text.text = "";
        }
    }

    void OnEnable()
    {
        isUsable = text != null;
    }

    void OnDisable()
    {
        isUsable = false;
    }

    void Update()
    {
        if (!isUsable || !modified) {
            return;
        }
        var curIndex = logIndex;
        var logText = "";
        for (var i = 0; i < LOG_SIZE; i++) {
            logText += log[curIndex];
            curIndex++;
            if (curIndex >= LOG_SIZE) {
                curIndex -= LOG_SIZE;
            }
        }
        text.text = logText;
        modified = false;
    }

    #endregion

    #region Public methods

    public static void AddLog(string message)
    {
        if (!isUsable) {
            return;
        }
        log[logIndex] = message + "\n";
        logIndex++;
        if (logIndex >= LOG_SIZE) {
            logIndex = 0;
        }
        modified = true;
    }

    #endregion
}

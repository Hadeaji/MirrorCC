using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayTextName = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;


    #region Server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    // server: can only run by the server callig it
    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    // server: called by user, works on server
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if (!string.IsNullOrEmpty(newDisplayName))
        {
            RpcLogNewName(newDisplayName);

            SetDisplayName(newDisplayName);
        }
    }

    #endregion

    #region Client
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        Debug.Log("Color Hook Working!!");
        displayTextName.text = newName;
    }

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        Debug.Log("Color Hook Working!!");
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    [ContextMenu("SetMyName")]
    private void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    // server calling a method on ALL clients 
    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}

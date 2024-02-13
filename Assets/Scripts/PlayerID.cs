using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerID : ScriptableObject
{
    public string playerName;
    public int playerType;
    public int playerCurrency;

    public PlayerEvents events;
}

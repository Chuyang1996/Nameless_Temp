using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioConfig 
{
    public static string moveStart = "SFX_MovingStart_01";
    public static string moveEnd = "SFX_MovingEnd_01";
    public static string battleStart = "SFX_CombatStart_01";
    public static string deathCharacter = "SFX_UnitDown_01";
    public static string buildEnd = "SFX_ConstructionEnd_01";
    public static string uiRemind = "SFX_UI_Click_01";
    public static string pageTurn = "SFX_Journal_01";

    public static void Init()
    {
        moveStart = "SFX_MovingStart_01";
        moveEnd = "SFX_MovingEnd_01";
        battleStart = "SFX_CombatStart_01";
        deathCharacter = "SFX_UnitDown_01";
        buildEnd = "SFX_ConstructionEnd_01";
        uiRemind = "SFX_UI_Click_01";
        pageTurn = "SFX_Journal_01";
    }
}

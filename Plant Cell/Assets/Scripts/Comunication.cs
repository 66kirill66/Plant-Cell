using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Collections;

public class Comunication : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void callTNITfunction();

    [DllImport("__Internal")]
    public static extern void onResetDone();

    [DllImport("__Internal")]
    public static extern void EntityClick(int id);

    [DllImport("__Internal")]
    public static extern void MeetMassege(int activatorId, int otherId, int dictId = -1);

    [DllImport("__Internal")]
    public static extern void SendEvent(string eventName);

    [DllImport("__Internal")]
    public static extern void WaterEntersToVacuole(int waterId, int vacuoleId, int booleanValue);

    [DllImport("__Internal")]
    public static extern void UpdateCytoplasmPressure(int cytoplasmId, string type);

    [DllImport("__Internal")]
    public static extern void UpdateMembranePressure(int membraneId, string type);
    [DllImport("__Internal")]
    public static extern void LightStatuUpdate(int lightId, int booleanValue);

    [DllImport("__Internal")]
    public static extern void CarbonDioxideEntersToCell(int carbonDId, int cellId);

    [DllImport("__Internal")]
    public static extern void DeleteEntity(int entityId);



    private VacuoleMain vacuoleMain;
    private Cytoplasm cytoplasm;
    private Membrane membrane;
    private AnimalCell animalCell;
    private TheCell theCell;


    static bool initFirstTime = true;
    private GuideEvents guideEvents;
    private string guidedEvent;

    


    private void Awake()
    {
        guideEvents = FindObjectOfType<GuideEvents>();
        vacuoleMain = FindObjectOfType<VacuoleMain>();
        cytoplasm = FindObjectOfType<Cytoplasm>();
        membrane = FindObjectOfType<Membrane>();
        animalCell = FindObjectOfType<AnimalCell>();
        theCell = FindObjectOfType<TheCell>();

        if (!Application.isEditor && initFirstTime == true)
        {
            callTNITfunction();
            initFirstTime = false;
        }
        else
        {
            if (!Application.isEditor)
            {
                onResetDone();
            }
        }
    }
    public void UpdateLightStatus(int lightId, bool value)
    {
        int valueToSend = value ? 1: 0;
        LightStatuUpdate(lightId, valueToSend);
    }

    public void SetSimulationStateInUnity(string state)
    {
        switch (state)
        {
            case "PAUSED":
                Time.timeScale = 0;
                break;
            case "RUNNING":
                Time.timeScale = 1;
                break;
        }
    }

    public void SendDeleteEntity(int id)
    {
        if (!Application.isEditor)
        {
            DeleteEntity(id);
        }
    }

    public void SendMeetMassege(int currentId, int otherId, int dictId = -1)
    {
        if (!Application.isEditor)
        {
            MeetMassege(currentId, otherId, dictId);
        }
    }
    public void CdEntersToCell(int carbonDId)
    {
        if (!Application.isEditor)
        {
            CarbonDioxideEntersToCell(carbonDId, theCell.CellId);
        }         
    }

    public void UpdateCytoplasmPressureType(string type)
    {
        if (!Application.isEditor)
        {
            UpdateCytoplasmPressure(cytoplasm.objectId, type);
        }
    }

    public void UpdatemMmbranePressureType(string objName, string type)
    {
        int id = objName == "Plant Membrane" ? membrane.objectId : animalCell.animalMembraneId;
        if (!Application.isEditor)
        {
            UpdateMembranePressure(id, type);
        }
    }

    public void ProcessWaterFlow(int waterId,  bool value)
    {
        int vacuoleId = vacuoleMain.objectId;
        int currentValue = value ? 1 : 0;
        if (!Application.isEditor)
        {
            WaterEntersToVacuole(waterId, vacuoleId, currentValue);
        }
    }


    public void ClickOnEntity(int id)
    {
        if (!Application.isEditor)
        {
            EntityClick(id);
        }
    }


    public void ResetSimulatiom()
    {
        SceneManager.LoadScene(0);
    }


    public void SendEventToPlethora(string eventName)
    {
        if (!Application.isEditor)
        {
            SendEvent(eventName);
            if (guidedEvent == eventName)
            {
                guideEvents.BackToStart();
                guideEvents.NoColor();
            }
        }
    }

    public void OnGuidedEventRequest(string eventName)
    {
        guideEvents.NoColor();
        guidedEvent = eventName;
        guideEvents.ShowColor(eventName);
    }
}

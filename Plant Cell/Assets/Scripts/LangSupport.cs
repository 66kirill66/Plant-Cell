using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using ArabicSupport;

public class LangSupport : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void SendLangCode();

    public static string langCode;
    private static bool currentIsRTL;

    private static string currentMembraneText;
    private static string currentCytoplasmText;
    private static string currentNucleusText;
    private static string currentMitochondrionText;
    private static string currentVacuoleText;
    private static string currentChloroplastText;
    private static string currentCellWallText;
    private static string currentGlucoseText;
    private static string currentCarbonDioxideText;

    [SerializeField] TextMeshProUGUI cellWallText;
    [SerializeField] TextMeshProUGUI cellWallTextLegend;

    [SerializeField] TextMeshProUGUI membraneText;
    [SerializeField] TextMeshProUGUI membraneTextLegend;

    [SerializeField] TextMeshProUGUI cytoplasmText;
    [SerializeField] TextMeshProUGUI cytoplasmTextLegend;

    [SerializeField] TextMeshProUGUI mitochondrionText;
    [SerializeField] TextMeshProUGUI mitochondrionTextLegend;

    [SerializeField] TextMeshProUGUI nucleusText;
    [SerializeField] TextMeshProUGUI nucleusTextLegend;

    [SerializeField] TextMeshProUGUI vacuoleText;
    [SerializeField] TextMeshProUGUI vacuoleTextLegend;

    [SerializeField] TextMeshProUGUI chloroplastText;
    [SerializeField] TextMeshProUGUI chloroplastTextLegend;

    [SerializeField] TextMeshProUGUI glucoseTextLegend;
    [SerializeField] TextMeshProUGUI carbonDioxideTextLegend;

    private void Start()
    {
        
    }
    public class LangData
    {
        public bool isRTL;

        public string cellWall;
        public string membrane;
        public string cytoplasm;
        public string mitochondrion;
        public string nucleus;
        public string vacuole;
        public string chloroplast;
        public string glucose;
        public string carbonDioxide;
    }
    private void Awake()
    {
        if (!Application.isEditor)
        {
            SetAllOptions();
        }
    }

    public void SetLanguage(string newlangCode)
    {
        if (langCode != newlangCode)
        {
            langCode = newlangCode;
            SendLangCode();
        }
    }
    public void SetChanges(string json)
    {
        LangData dataLang = JsonUtility.FromJson<LangData>(json);

        currentIsRTL = dataLang.isRTL;
        currentMembraneText = dataLang.membrane;
        currentNucleusText = dataLang.nucleus;
        currentMitochondrionText = dataLang.mitochondrion;
        currentCytoplasmText = dataLang.cytoplasm;
        currentVacuoleText = dataLang.vacuole;
        currentChloroplastText = dataLang.chloroplast;
        currentCellWallText = dataLang.cellWall;
        currentGlucoseText = dataLang.glucose;
        currentCarbonDioxideText = dataLang.carbonDioxide;

        SetAllOptions();
    }


    public void SetAllOptions()
    {
        SetOptions(cellWallText, currentIsRTL, currentCellWallText);
        SetOptions(membraneText, currentIsRTL, currentMembraneText);
        SetOptions(cytoplasmText, currentIsRTL, currentCytoplasmText);
        SetOptions(mitochondrionText, currentIsRTL, currentMitochondrionText);
        SetOptions(nucleusText, currentIsRTL, currentNucleusText);
        SetOptions(vacuoleText, currentIsRTL, currentVacuoleText);
        SetOptions(chloroplastText, currentIsRTL, currentChloroplastText);

        SetOptions(cellWallTextLegend, currentIsRTL, currentCellWallText, true);
        SetOptions(membraneTextLegend, currentIsRTL, currentMembraneText, true);
        SetOptions(cytoplasmTextLegend, currentIsRTL, currentCytoplasmText, true);
        SetOptions(mitochondrionTextLegend, currentIsRTL, currentMitochondrionText, true);
        SetOptions(nucleusTextLegend, currentIsRTL, currentNucleusText, true);
        SetOptions(vacuoleTextLegend, currentIsRTL, currentVacuoleText, true);
        SetOptions(chloroplastTextLegend, currentIsRTL, currentChloroplastText, true);
        SetOptions(glucoseTextLegend, currentIsRTL, currentGlucoseText, true);
        SetOptions(carbonDioxideTextLegend, currentIsRTL, currentCarbonDioxideText, true);
    }

    public void SetOptions(TextMeshProUGUI objText, bool isRTL, string text, bool changeTextSide = false)
    {
        TextAlignmentOptions option;
        if (changeTextSide == false)
        {
            
            option = TextAlignmentOptions.Center;
        }
        else
        {
            option = isRTL ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;

        }
        LanguageAssignment(objText, text, isRTL, option);
    }

    private void LanguageAssignment(TextMeshProUGUI objectText, string text, bool isRTL, TextAlignmentOptions side)
    {
        if (objectText != null)
        {
            objectText.enableWordWrapping = false;
            objectText.alignment = side;

            if (langCode != "ar")
            {
                objectText.text = text;
                objectText.isRightToLeftText = isRTL;
            }
            else
            {
                objectText.text = ArabicFixer.Fix(text, false, false);
                objectText.isRightToLeftText = false;
            }

        }
    }
}

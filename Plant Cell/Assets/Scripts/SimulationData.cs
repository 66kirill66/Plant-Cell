public class SimulationData
{
    public int id;
    public string viewType;
    public bool displayTooltip;
    public bool displayWaterCan;
    public bool veryLowPressureSupport;
    public string jsonTxt;
    public int listId;
    public float timer;
    public bool withToxic;
    public bool exists;
    public bool displayToxic;
    public int dictId;
    public int numOfGlucose;
    public float evaporationTimer;
}

public enum ObjectToCreate
{
    glucose,
    carbonDioxide
}

public static class EventNames
{
    public const string MembraneAdded = "Membrane Added";
    public const string CytoplasmAdded = "Cytoplasm Added";
    public const string CellNucleusAdded = "Cell Nucleus Added";
    public const string FiveMitochondriaAdded = "Five Mitochondria Added";
    public const string CellWallAdded = "Cell Wall Added";
    public const string ChloroplastAdded = "Chloroplast Added";
    public const string VacuoleAdded = "Vacuole Added";
    public const string WateringPlant = "Watering Plant";
    public const string AddLight = "Add Light";
    public const string AddCarbonDioxide = "Add CD";
    public const string WaterPlantCellTissue = "Water Plant Cell Tissue";
    public const string WaterAnimalCellTissue = "Water Animal Cell Tissue";
    public const string ToxicAdded = "Toxic Added";
}


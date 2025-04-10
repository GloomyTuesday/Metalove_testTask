namespace Scripts.BaseSystems.Localization
{
    public enum ExportLocalizationSourceId 
    {
        Non=0,

        //       Keep the sequence, for editor all items from enum are going to keep the incremental order 
        //  and value for next element will be increased by 1, ALWAYS!

        //      In case you need to remove a certain element, mark it as not unusable
        TextMeshProUGUI=1,
        TextMeshProUGUI_collection=2,
        TMP_InputField=3,
        TMP_InputField_collection=4,
        String=5
    }
}

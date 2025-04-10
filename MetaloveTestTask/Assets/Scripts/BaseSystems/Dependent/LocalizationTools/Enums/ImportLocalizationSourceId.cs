namespace Scripts.BaseSystems.Localization
{
    public enum ImportLocalizationSourceId 
    {
        Non=0,

        //       Keep the sequence, for editor all items from enum are going to keep the incremental order 
        //  and value for next element will be increased by 1, ALWAYS!

        //      In case you need to remove a certain element, mark it as not unusable
        Custom_ScriptableObj,
        FilteredByType_ITextLocalizator
    }
}

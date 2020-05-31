namespace CentralTrade.Models.Enums
{
    [System.Serializable]
    public enum ErrorCode
    {
        [System.ComponentModel.Description("None")]
        None = 1000,
        [System.ComponentModel.Description("Unspecified error")]
        Unspecified = 1001,
        [System.ComponentModel.Description("Input parameters are invalid")]        
        InvalidInput = 1002
    }
}

namespace SMSLiveResellerSite2026.Shared
{
    public class Settings
    {
        public string BaseUrl { get; set; } = "";
        public int SmsMaxParts { get; set; } = 6; // Max SMS part
        public int MaxCharacter { get => SmsMaxParts * 153; } // Max SMS character
    }
}

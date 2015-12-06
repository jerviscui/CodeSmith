namespace CodeSmith
{
    public class EnumSettings
    {
        public string FullName { get; set; }

        public string EnumType { get; set; }

        public EnumSettings()
        {

        }

        public EnumSettings(string fullName, string enumType)
        {
            FullName = fullName;
            EnumType = enumType;
        }
    }
}

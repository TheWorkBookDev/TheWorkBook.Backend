using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Utils.ParameterStore
{

    public sealed class Parameter : IParameter
    {
        public Parameter() { }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string DataType { get; set; }
        public string Description { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string FriendlyName
        {
            get
            {
                if (Name.IndexOf("/") > -1)
                    //return Name[(Name.LastIndexOf("/") + 1)..];
                    return Name.Substring(Name.LastIndexOf("/") + 1);
                return Name;
            }
        }
    }
}

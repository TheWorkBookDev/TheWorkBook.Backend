namespace TheWorkBook.Utils.Abstraction.ParameterStore
{
    public interface IParameter
    {
        string DataType { get; set; }
        string Description { get; set; }
        string FriendlyName { get; }
        DateTime LastModifiedDate { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}

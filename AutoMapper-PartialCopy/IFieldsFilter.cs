namespace AutoMapper_PartialCopy
{
    public interface IFieldsFilter
    {
        bool Matches(string path, bool ignoreCase = true);
    }
}

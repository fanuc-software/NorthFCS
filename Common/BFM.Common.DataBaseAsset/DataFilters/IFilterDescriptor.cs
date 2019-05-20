namespace BFM.Common.DataBaseAsset.DataFilters
{

    public interface IFilterDescriptor
    {
        void SetFilterItem(FilterAgent item, FilterLogic logic);
    }
}
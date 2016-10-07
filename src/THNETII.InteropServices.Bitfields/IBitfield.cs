namespace THNETII.InteropServices.Bitfields
{
    public interface IBitfield
    {
        int TypeWidth { get; }
        uint TypeMask { get; }
        int FieldWidth { get; }
        uint FieldMask { get; }
        uint InvertedMask { get; }
    }

    public interface IBitfield<T> : IBitfield
    {
        T Get(T storage);

        T Set(T field, T storage);

        void RefSet(T field, ref T storage);
    }
}
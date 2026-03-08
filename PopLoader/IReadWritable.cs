namespace PopLoader.BinaryHelper;

public interface IReadable<TOut>
{
    /// <summary>
    /// Read one <see cref="TOut"/> element from the reader and move the stream position the end of that element.
    /// </summary>
    public static abstract TOut Read(BinaryReader reader);
}

public interface IWritable
{
    public abstract void Write(BinaryWriter writer);
}
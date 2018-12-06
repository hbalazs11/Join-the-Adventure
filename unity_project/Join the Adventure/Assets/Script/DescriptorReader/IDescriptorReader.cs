
public interface IDescriptorReader
{
    GameDescriptor ReadDescriptor(string path);

    GameDescriptor[] ReadMultipleDescriptor(string[] paths);
}

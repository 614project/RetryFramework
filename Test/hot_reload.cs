[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(HotReloadManager))]
public static class HotReloadManager
{
    public static void ClearCache(Type[]? updatedTypes)
    {
        Console.WriteLine("ClearCache");
        if(updatedTypes is not null) Console.WriteLine(string.Join<Type>(',',updatedTypes));
    }

    public static void UpdateApplication(Type[]? updatedTypes)
    {
        Console.WriteLine("UpdateApplication");
        if (updatedTypes is not null)  Console.WriteLine(string.Join<Type>(',', updatedTypes));
    }
}
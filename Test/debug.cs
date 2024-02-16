using RetryFramework.SDL2;

namespace RetryFramework.Test;

public class Debug
{
    public static void VersionCheck(StreamWriter writer)
    {
        writer.WriteLine("{1,-24} : {0}", Framework.Version, "Retry Framework Version");
        SDL.SDL_GetVersion(out var sdlver);
        writer.WriteLine("{3,-24} : {0}.{1}.{2}", sdlver.major, sdlver.minor, sdlver.patch, "SDL2 Version");
    }

    public static void OSCheck(StreamWriter writer)
    {
        var osf = Environment.OSVersion.Platform;
        writer.WriteLine("{1,-24} : {0}", osf, "OS Platform");
        writer.WriteLine("{1,-24} : {0}", Environment.OSVersion.VersionString, "OS Version");
        writer.WriteLine("{0,-24} : {1}", "Service Pack", Environment.OSVersion.ServicePack);
        writer.WriteLine("{1,-24} : {0}", Environment.Is64BitOperatingSystem, "64Bit OS");
        writer.WriteLine("{0,-24} : {1}", "64Bit Processing", Environment.Is64BitProcess);
    }

    public static void ProgramCheck(StreamWriter writer)
    {
        writer.WriteLine("{1,-24} : {0}", Environment.ProcessPath, "Program Path");
        writer.WriteLine("{1,-24} : {0}", Environment.CurrentDirectory, "Current Directory");
        writer.WriteLine("{1,-24} : {0}", Environment.CommandLine, "Command Line");
    }

    public static void UserCheck(StreamWriter writer)
    {
        writer.WriteLine("{1,-24} : {0}", Environment.MachineName, "Computer Name");
        writer.WriteLine("{1,-24} : {0}", Environment.UserName, "User Name");
        writer.WriteLine("{1,-24} : {0}", Environment.UserDomainName, "User Domain Name");
    }

    public static void AllPrint(bool stderror = false)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        using (StreamWriter stream = new(stderror ?  Console.OpenStandardError() : Console.OpenStandardOutput()))
        {
            VersionCheck(stream);
            OSCheck(stream);
            ProgramCheck(stream);
            UserCheck(stream);
        }
    }
}

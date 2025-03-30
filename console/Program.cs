using System;
using System.Reflection;

using YourNamespace;

class Program
{
    static private LibraryEntryPoint entryPoint = new LibraryEntryPoint();
    static void Main(string[] args)
    {

        Assembly ass = typeof(LibraryEntryPoint).Assembly;
        Console.WriteLine($"The name of my plugin is {ass.GetCustomAttribute<AssemblyTitleAttribute>().Title}");

    }
}
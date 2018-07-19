using System;

namespace OwnerDependencyCheck
{
    // all Custom Exceptions..
    public class CircularDependencyDetected : Exception
    {
        public CircularDependencyDetected() : base("Circular Dependency Detected.") { }
    }
    public class DuplicateFilePathDetected : Exception
    {
        public DuplicateFilePathDetected() : base("There is duplicated path.") { }
    }
    public class FileNotFound : Exception
    {
        public FileNotFound(string msg ) : base("The File Path "+msg+" is not part of the Scanned Branch.") { }
    }
    public class DirNotFound : Exception
    {
        public DirNotFound(string msg) : base("Directory "+msg+" Not found.") { }
    }
}

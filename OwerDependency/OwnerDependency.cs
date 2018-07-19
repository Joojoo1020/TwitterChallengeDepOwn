using QuickGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OwnerDependencyCheck
{
    // This is a class Check handles Owners and Dependencies
    // It has a Graph, and Dictionary to speed up the process and lookup.
    // Dictionary to loopup the file and

    public class OwnerDependency
    {
        private AdjacencyGraph<Branch, Edge<Branch>> _DependencyGrath;
        private bool _AvoidCircularDependency;

        public OwnerDependency(String Path = null, bool AvoidCircularDependency = false)
        {
            _DependencyGrath = new AdjacencyGraph<Branch, Edge<Branch>>(false, -1, -1, new Branch());
            _AvoidCircularDependency = AvoidCircularDependency;
            if (Path != null)
            {
                AddPath(Path);
            }

        }
        public void AddPath(string Path)
        {
            ProcessFiles(new DirectoryInfo(Path).FullName);
        }
        private void addDependency(Branch Current, Branch Dependency)
        {
            addDependency(Current, new List<Branch>() { Dependency });
        }
        private void addDependency(Branch Current, List<Branch> Dependencies = null)

        {


            if (Dependencies != null)
            {
                // setting up all edges

                foreach (Branch dep in Dependencies)
                {
                    if (_AvoidCircularDependency && CircularDependencyCheck(dep, Current))
                    {
                        throw new CircularDependencyDetected();
                    }
                    Edge<Branch> DependenciesMap = new Edge<Branch>(dep, Current);
                    _DependencyGrath.AddVerticesAndEdge(DependenciesMap);
                }
            }


        }

        private bool CircularDependencyCheck(Branch From, Branch To)
        {

            if (string.Compare(From.Name, To.Name) == 0) return true;

            List<Edge<Branch>> DependenciesMap = _DependencyGrath.Edges.Where(e => string.Compare(e.Target.Name, From.Name) == 0).ToList<Edge<Branch>>();


            foreach (Edge<Branch> Dep in DependenciesMap)
            {
                if (CircularDependencyCheck(Dep.Source, To)) return true;

            }

            return false;
        }

        public bool Verify(string file, string Signed = null)
        {
            Branch Root = null;
            string OrgFile = file;
            do
            {
                DirectoryInfo Folder = new DirectoryInfo(file);
                Root = _DependencyGrath.Vertices.Where(V => string.Compare(V.Name, Folder.FullName) == 0).FirstOrDefault<Branch>();

                if (Directory.GetParent(file) == null)
                {
                    throw new FileNotFound(OrgFile);
                }
                file = Directory.GetParent(file).FullName;
            } while (Root == null);


            return VerifyDependencies(Root, Signed);


        }
        private bool VerifyDependencies(Branch root, string Signed)
        {
            IEnumerable<Edge<Branch>> DependenciesMap;
            if (root.Owers == null) return true;
            if (Signed == null && root.Owers != null) return false;
            if (root.Owers.Intersect(Signed.Split(',')).ToList<string>().Count==0) return false;
            if (_DependencyGrath.TryGetOutEdges(root, out DependenciesMap))
            {
                foreach (Edge<Branch> Dep in DependenciesMap)
                {
                    if (!VerifyDependencies(Dep.Target, Signed)) return false;
                }
            }
            return true;
        }
        private void ProcessFiles(string path)
        {

            string[] directories;
            if (path == null || !Directory.Exists(path))
            {
                throw new DirNotFound((path == null) ? "" : path);
            }
            List<string> mydep = new List<string>();
            Branch mybranch = GetBranch(path, out mydep);
            List<Branch> AllDep = new List<Branch>();
            if (mydep != null)
            {

                foreach (string Dep in mydep)
                {
                    List<string> depdep = new List<string>();
                    AllDep.Add(GetBranch(Dep, out depdep));
                }
            }
            addDependency(mybranch, AllDep);

            directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                ProcessFiles(directory);
            }
        }
        private List<string> ReadFileContent(string root, String path, bool IsPath)
        {
            if (!File.Exists(path)) return null;
            List<string> lines = new List<string>();
            foreach (string line in File.ReadAllLines(path))
            {
                string currentline = line.TrimEnd();
                if (currentline.Length == 0) continue;
                if (IsPath)
                {
                    lines.Add(Path.GetFullPath(Path.Combine(root, currentline)));
                }
                else
                {
                    lines.Add(currentline);
                }

            }
            return lines;
        }
        private Branch GetBranch(string path, out List<string> mydep)
        {
            if (!Directory.Exists(path))
            {
                throw new DirNotFound(path);
            }

            List<string> files = Directory.GetFiles(path).ToList();
            Branch ber = new Branch()
            {
                Name = new DirectoryInfo(path).FullName,
                Owers = ReadFileContent(null, files.Where(f => f.EndsWith(".own")).FirstOrDefault(), false),

            };
            mydep = ReadFileContent(Directory.GetParent(path).FullName, files.Where(f => f.EndsWith(".dep")).FirstOrDefault(), true);
            return ber;
        }
    }





}


using OwnerDependencyCheck;
using PowerArgs;
using System;
namespace User
{
    // Args
    public class ParseArgs
    {
        [ArgDescription("Prevent Circular Dependency"), DefaultValue(true)]
        public bool CircularDependency { get; set; }

        [ArgRequired, ArgDescription("Directory to scan")]
        public string Directory { get; set; }
        [ArgRequired, ArgDescription("Comma Separated Signatures"), DefaultValue("")]
        public string Approvers { get; set; }
        [ArgRequired, ArgDescription("Path to files to verify. Comma separated")]
        public string ChangedFiles { get; set; }

        [ArgDescription("Multiple Verification"), DefaultValue(false)]
        public bool Multiple { get; set; }

    }


    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // parsing the Args
                ParseArgs Input = Args.Parse<ParseArgs>(args);
                
                // Creating
                OwnerDependency depcheck = new OwnerDependency(Input.Directory, Input.CircularDependency);

                do
                {
                    try
                    {
                        // checking the signatures
                        foreach (string file in Input.ChangedFiles.Split(','))
                        {
                            if (depcheck.Verify(file, Input.Approvers))
                            {
                                Console.WriteLine(file + ": Approved.");
                            }
                            else
                            {
                                Console.WriteLine(file + ": Insufficient Approval.");
                            }
                        }




                    }
                    // error handling ..
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (!Input.Multiple) break;
                    Console.WriteLine("Other Files to Approve (comma separated): ");
                    Input.ChangedFiles = Console.ReadLine().Trim();
                    if (string.Compare(Input.ChangedFiles, "") == 0)
                    {
                        Input.Multiple = false;
                    }


                } while (Input.Multiple);

            }
            catch (Exception ex)
            {
                // error handling
                Console.WriteLine(ex.Message);
                if (ex.GetType().FullName.Contains("PowerArgs"))
                {
                    Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<ParseArgs>());
                }

            }
        }


    }
}

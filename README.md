# TwitterChallengeDepOwn
This is the question I got from Twitter and I only had 3 hr to finish it. 
It immediately offered me a onsite interview, (without any phone screening)


Here was the Challenge :

The Challenge
At Twitter, all changes to source code are required to be approved by other engineers responsible for the source files that are affected by the changes. As a simplified model, assume each directory may optionally contain two special files that contain information used to identify who must approve a change:

DEPENDENCIES files list the paths to other directories that its containing directory depends upon. Paths must be relative to the root directory of the source code repository. A directory is considered to be affected by a change if a file or directory in that directory or in one of its transitive dependencies is modified (including creation and deletion).

OWNERS files list the login names of all of the engineers who are able to approve changes affecting a directory. If there is no one listed in the OWNERS file or it does not exist then the parent directories OWNERS file should be applied.

For example, consider the following directory tree:
  x/
    DEPENDENCIES = "y\n"
    OWNERS = "A\nB\n"
  y/
    OWNERS = "B\nC\n"
    file

If a change modifies y/file, it affects both directories y (contains file) and x (depends on y). This change must at a minimum be approved by either B alone (owner of x and y) or both A (owner of x) and C (owner of y).

Build a command line tool that validates the correct people have approved the changes made to a set of files.

As an example, the following is expected to work on the example directory structure we have provided to you.

$ validate_approvals --approvers alovelace ghopper --changed-files src/com/twitter/follow/Follow.java src/com/twitter/user/User.java
Approved
$ validate_approvals --approvers alovelace --changed-files src/com/twitter/user/User.java
Insufficient approvals
Expectations:
		The challenge should take about three hours to complete.
		Choice of programming language is entirely up to you.
		Documentation for the project.
		Code is tested.

My Respond : 

The program is written in C#. 
There is a Documentation for the library developed, and all parameters. 
There is a test.ps1 file which tests the quality of the code.


Enjoy 
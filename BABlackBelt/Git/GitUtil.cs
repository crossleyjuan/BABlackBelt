using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BABlackBelt.Git
{
    public class GitUtil
    {
        string _folder;

        public GitUtil(string folder)
        {
            _folder = folder;
        }

        public List<GitChange> GetStatus()
        {
            string result = ExecuteGitCommand("status");

            List<GitChange> changes = parseStatus(result);

            return changes;
        }

        public string Add(string file)
        {
            string result = ExecuteGitCommand("add " + file);

            return result;
        }

        public string Remove(string file)
        {
            string result = ExecuteGitCommand("rm " + file);

            return result;
        }

        public string Reset()
        {
            return ExecuteGitCommand("reset --hard");
        }

        public void ShowDiff(string file)
        {
            ExecuteGitCommand("difftool -y " + file, false);
        }

        public string GetGlobal(string global)
        {
            string value = ExecuteGitCommand("config " + global);
            if (string.IsNullOrEmpty(value))
            {
                value = "";
            }
            value = value.Replace('\n', ' ');
            value = value.Replace('\r', ' ');
            return value.Trim();
        }

        public void setGlobal(string global, string value)
        {
            ExecuteGitCommand(string.Format("config {0} \"{1}\"", global, value));
        }

        public string Pull(string remote, string branch)
        {
            string command = string.Format("pull {0} {1}", remote, branch);

            return ExecuteGitCommand(command);
        }

        public string Push(string remote, string branch)
        {
            string command = string.Format("push {0} {1}", remote, branch);

            return ExecuteGitCommand(command);
        }

        private List<GitChange> parseStatus(string status)
        {
            string[] lines = status.Split('\n');

            List<GitChange> result = new List<GitChange>();
            bool untracked = false;
            foreach (string line in lines)
            {
                if (!untracked)
                {
                    if (line.StartsWith("#\tmodified:"))
                    {
                        GitChange change = new GitChange()
                        {
                            ChangeType = "modified"
                        };
                        string fileName = line.Substring("#	modified:   ".Length);
                        change.File = fileName;
                        result.Add(change);
                    }
                    else if (line.StartsWith("#\tdeleted:"))
                    {
                        GitChange change = new GitChange()
                        {
                            ChangeType = "deleted"
                        };
                        string fileName = line.Substring("#\tdeleted:    ".Length);
                        change.File = fileName;
                        result.Add(change);
                    }
                    else if (line.StartsWith("# Untracked files:"))
                    {
                        untracked = true;
                    }
                }
                else
                {
                    if (line.StartsWith("#\t"))
                    {
                        GitChange change = new GitChange()
                        {
                            ChangeType = "new"
                        };
                        string fileName = line.Substring("#\t".Length);
                        change.File = fileName;
                        result.Add(change);
                    }
                }
            }

            return result;
        }

        public string GitClone(string remoteFolder, string destFolder)
        {
            string command = string.Format("clone {0} {1}", remoteFolder, destFolder);

            return ExecuteGitCommand(command);

        }

        public string ExecuteGitCommand(string command)
        {
            return ExecuteGitCommand(command, true);
        }

        public string ExecuteGitCommand(string command, bool wait)
        {
            ProcessStartInfo gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            string gitFolder = @"C:\Program Files (x86)\Git";
            gitInfo.FileName = gitFolder + @"\bin\git.exe";

            Process gitProcess = new Process();
            gitInfo.Arguments = command; // such as "fetch orign"
            gitInfo.WorkingDirectory = _folder;
            gitInfo.UseShellExecute = false;

            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();

            if (wait)
            {
                string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT
                string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR

                gitProcess.WaitForExit();
                gitProcess.Close();

                return stdout_str;
            }
            else
            {
                return null;
            }
        }

        public string Commit(string comment)
        {
            string command = string.Format("commit -m \"{0}\"", comment);

            string result = ExecuteGitCommand(command);
            return result;
        }

        public static bool CheckValidGitFolder(string folder)
        {
            string gitFolder = Path.Combine(folder, ".git");

            if (Directory.Exists(gitFolder))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

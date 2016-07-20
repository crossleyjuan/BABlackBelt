using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

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
            string result = ExecuteGitCommand("status", "--untracked-files=all");

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

        private static Regex Deleted = new Regex("[#]*\tdeleted:[ ]*([a-zA-Z0-9_\\\\\\/]*\\.[a-zA-Z]{3})[\\r]$");
        private static Regex Modified = new Regex("[#]*\tmodified:[ ]*([a-zA-Z0-9_\\\\\\/]*\\.[a-zA-Z]{3})[\\r]$");
        private static Regex UntrackedStarts = new Regex("[#]*\t([a-zA-Z0-9_\\\\\\/]*\\.[a-zA-Z]{3})[\\r]$");

        private List<GitChange> parseStatus(string status)
        {
            string[] lines = status.Split('\n');

            List<GitChange> result = new List<GitChange>();
            bool untracked = false;
            foreach (string line in lines)
            {
                if (!untracked)
                {
                    if (Modified.IsMatch(line))
                    {
                        GitChange change = new GitChange()
                        {
                            ChangeType = "modified"
                        };
                        Group grp = Modified.Match(line).Groups[1];
                        string fileName = grp.Value;
                        change.File = fileName;
                        result.Add(change);
                    }
                    else if (Deleted.IsMatch(line))
                    {
                        GitChange change = new GitChange()
                        {
                            ChangeType = "deleted"
                        };
                        Group grp = Deleted.Match(line).Groups[1];
                        string fileName = grp.Value;
                        change.File = fileName;
                        result.Add(change);
                    }
                        
                    else if (line.IndexOf("Untracked files:") > -1)
                    {
                        untracked = true;
                    }
                }
                else
                {
                    if (UntrackedStarts.IsMatch(line))
                    {
                        Group grp = UntrackedStarts.Match(line).Groups[1];
                        string fileName = grp.Value;
                        GitChange change = new GitChange()
                        {
                            ChangeType = "new"
                        };
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

        public string ExecuteGitCommand(string command, string args="")
        {
            return ExecuteGitCommand(command, true, args);
        }

        public string ExecuteGitCommand(string command, bool wait, string args = "")
        {
            ProcessStartInfo gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            string gitFolder = @"C:\Program Files (x86)\Git";
            gitInfo.FileName = gitFolder + @"\bin\git.exe";

            Process gitProcess = new Process();
            gitInfo.Arguments = command + " " + args; // such as "fetch orign"
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            gitInfo.WorkingDirectory = _folder;
            gitInfo.UseShellExecute = false;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.RedirectStandardError = true;
            gitProcess.StartInfo = gitInfo;

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                gitProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                gitProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                gitProcess.Start();

                gitProcess.BeginOutputReadLine();
                gitProcess.BeginErrorReadLine();

                int timeout = 5000;
                if (wait && gitProcess.WaitForExit(timeout) &&
                        outputWaitHandle.WaitOne(timeout) &&
                        errorWaitHandle.WaitOne(timeout))
                {

                    string stdout_str = output.ToString(); // pick up STDOUT
                    string stderr_str = error.ToString();  // pick up STDERR

                    gitProcess.Close();
                    outputWaitHandle.Close();
                    errorWaitHandle.Close();
                    return stdout_str;
                }
                else
                {
                    return null;
                }
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

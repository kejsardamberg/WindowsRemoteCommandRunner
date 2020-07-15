using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace WindowsRemoteCommandRunner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindowsCommandController : ControllerBase
    {
        private static readonly string filePath = "cmd.txt";

        // GET: api/WindowsCommand
        [HttpGet]
        public string Get()
        {
            var cmd = "";
            if (Request.QueryString.HasValue)
                cmd = HttpUtility.ParseQueryString(Request.QueryString.Value).Get("cmd"); ;
            if (string.IsNullOrEmpty(cmd))
            {
                if (!System.IO.File.Exists(filePath))
                    return "Usage: " +
                        "Send command as 'cmd' query parameter (e.g. 'http://" + Environment.MachineName + ":5000/api/WindowsCommand?cmd=dir') or place " +
                        "a file with command(s) as '" + filePath + "' in execution directory '" + Environment.CurrentDirectory + "'." +
                        Environment.NewLine +
                        "Remember that spaces and other characters should be URL-encoded (space = '%20')";
                return "Execution result from local command file execution:" + Environment.NewLine + ExecuteFile();
            }

            var feedback = Execute(cmd);
            if (string.IsNullOrEmpty(feedback)) feedback = "<executed but nothing returned from command>";
            var pathString = "\\\\" + Environment.MachineName + "\\" + Environment.CurrentDirectory.Replace(":\\", "") + ">" + cmd;
            return "Execution result from query parameter:" + Environment.NewLine + pathString + Environment.NewLine + feedback;
        }

        private string ExecuteFile()
        {
            var returnString = new StringBuilder();
            int counter = 0;
            string line;
            if (!System.IO.File.Exists(filePath)) return "Cannot execute file '" + filePath + "' in execution directory '" + Environment.CurrentDirectory + "'. It cannot be found.";

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                var pathString = "\\\\" + Environment.MachineName + "\\" + Environment.CurrentDirectory.Replace(":\\", "") + ">" + line;
                returnString.AppendLine(pathString);
                var feedback = Execute(line);
                if (string.IsNullOrEmpty(feedback)) feedback = "<executed but nothing returned from command>";
                returnString.AppendLine(feedback);
                returnString.AppendLine();
                counter++;
            }

            file.Close();
            return returnString.ToString();
        }

        // POST: api/WindowsCommand
        [HttpPost]
        [Consumes("text/plain")]
        public string Post([FromBody] string value)
        {
            var returnString = new StringBuilder();
            if (string.IsNullOrEmpty(value)) return "Cannot execute empty POST body.";
            foreach (var myString in value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                var pathString = "\\\\" + Environment.MachineName + "\\" + Environment.CurrentDirectory.Replace(":\\", "") + ">" + myString;

                returnString.AppendLine(pathString);
                var feedback = Execute(myString);
                if (string.IsNullOrEmpty(feedback)) feedback = "<executed but nothing returned from command>";
                returnString.AppendLine(feedback);
                returnString.AppendLine();
            }
            return "Execution result from HTTP POST body command execution: " + Environment.NewLine + returnString.ToString();
        }

        private string Execute(string cmd)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {cmd}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.CurrentDirectory
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}

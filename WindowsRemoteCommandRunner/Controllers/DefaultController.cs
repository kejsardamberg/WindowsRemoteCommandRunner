using Microsoft.AspNetCore.Mvc;
using System;

namespace WindowsRemoteCommandRunner.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        // GET: api/Default
        [HttpGet]
        public ContentResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = webPage
            };
        }

        private static string webPage = "<!DOCTYPE html>" +
            "<html>" +
            "  <head>" +
            "    <title>Computer controller</title>" +
            "    <meta charset='UTF-8'>" +
            "    <style>" +
            "      body { background-color: skyblue; color: darkslategray; margin: 50px; font-family: Verdana,Geneva,sans-serif; }" +
            "      #result { background-color: black; color: white; font-family: Consolas,monaco,monospace; border: 2px solid gray;padding-top: 15px; padding-bottom: 15px; white-space: pre-line; }" +
            "      button { padding: 15px; }" +
            "      custom { width: 50%; }" +
            "      #help { padding: 25px; margin: 25px; display: none; position: fixed; left: 0px; top: 0px; z-index: 2; border: 1px solid black; background-color: white; color: darkslategray; }" +
            "    </style>" +
            "    <script>" +
            "      function lockScreen(){" +
            "        var xhttp = new XMLHttpRequest();" +
            "        xhttp.open('POST', 'api/WindowsCommand', true);" +
            "        xhttp.setRequestHeader('Content-Type','text/plain');" +
            "        xhttp.setRequestHeader('Accept','text/plain');" +
            "        xhttp.setRequestHeader('Media-Type','text/plain');" +
            "        xhttp.onreadystatechange = function() {" +
            "          if (this.readyState == 4 && this.status == 200) {" +
            "            document.getElementById('result').innerHTML = this.responseText;" +
            "          } else if (this.readyState == 4) {" +
            "            document.getElementById('result').innerHTML = this.status + ': ' + this.responseText" +
            "          }" +
            "        };" +
            "        xhttp.send('rundll32.exe user32.dll,LockWorkStation');" +
            "      }" +
            "" +
            "      function cancelShutdown(){" +
            "        var xhttp = new XMLHttpRequest();" +
            "        xhttp.open('POST', 'api/WindowsCommand', true);" +
            "        xhttp.setRequestHeader('Content-Type','text/plain');" +
            "        xhttp.setRequestHeader('Accept','text/plain');" +
            "        xhttp.setRequestHeader('Media-Type','text/plain');" +
            "        xhttp.onreadystatechange = function() {" +
            "          if (this.readyState == 4 && this.status == 200) {" +
            "            document.getElementById('result').innerHTML = this.responseText;" +
            "          } else if (this.readyState == 4) {" +
            "            document.getElementById('result').innerHTML = this.status + ': ' + this.responseText" +
            "          }" +
            "        };" +
            "        xhttp.send('shutdown.exe /a');" +
            "      }" +
            "" +
            "      function shutdown(){" +
            "        var xhttp = new XMLHttpRequest();" +
            "        xhttp.open('POST', 'api/WindowsCommand', true);" +
            "        xhttp.setRequestHeader('Content-Type','text/plain');" +
            "        xhttp.setRequestHeader('Accept','text/plain');" +
            "        xhttp.setRequestHeader('Media-Type','text/plain');" +
            "        xhttp.onreadystatechange = function() {" +
            "          if (this.readyState == 4 && this.status == 200) {" +
            "            document.getElementById('result').innerHTML = this.responseText;" +
            "          } else if (this.readyState == 4) {" +
            "            document.getElementById('result').innerHTML = this.status + ': ' + this.responseText" +
            "          }" +
            "        };" +
            "        xhttp.send('shutdown.exe /s /t ' + document.getElementById('shutdowntimeout').value );" +
            "      }" +
            "" +
            "      function custom(){" +
            "        var xhttp = new XMLHttpRequest();" +
            "        xhttp.open('POST', 'api/WindowsCommand', true);" +
            "        xhttp.setRequestHeader('Content-Type','text/plain');" +
            "        xhttp.setRequestHeader('Accept','text/plain');" +
            "        xhttp.setRequestHeader('Media-Type','text/plain');" +
            "        xhttp.onreadystatechange = function() {" +
            "          if (this.readyState == 4 && this.status == 200) {" +
            "            document.getElementById('result').innerHTML = this.responseText;" +
            "          } else if (this.readyState == 4) {" +
            "            document.getElementById('result').innerHTML = this.status + ': ' + this.responseText" +
            "          }" +
            "        };" +
            "        xhttp.send(document.getElementById('custom').value);" +
            "      }" +
            "" +
            "      function help(){" +
            "          var help = document.getElementById('help');" +
            "          if(help.style.display == 'none'){" +
            "             help.style.display = 'block';" +
            "          } else {" +
            "             help.style.display = 'none';" +
            "          }" +
            "      }" +
            "" +
            "      function init(){" +
            "        document.getElementById('computername').innerHTML = '" + Environment.MachineName + "';" +
            "        document.title = '" + Environment.MachineName + " remote control';" +
            "        document.getElementById('help').addEventListener('click', function() { help(); });" +
            "      }" +
            "    </script" +
            "  </head>" +
            "  <body onload='init()'>" +
            "    <h1>Remote computer control screen</h2>" +
            "    <div id='comp'><span>Computer name: </span><span id='computername'></span></div>" +
            "    <button onclick='lockScreen()'>Lock screen</button>" +
            "    <br /><hr />" +
            "    <button onclick='shutdown()'>Shut down computer</button>" +
            "    <label for='shutdowntimeout'>Shutdown timeout in seconds</label>" +
            "    <input type='number' id='shutdowntimeout' value='30'>sec." +
            "    <button onclick='cancelShutdown()'>Cancel shutdown</button>" +
            "    <br /><hr />" +
            "    <label for='custom'>Custom command:</label>" +
            "    <input type='text' id='custom' value=''>" +
            "    <button onclick='custom()'>Execute custom command</button>" +
            "    <br /><hr />" +
            "    <button onclick='help()'>Help</button>" +
            "    <br /><hr />" +
            "    <h2>Output from last command</h2>" +
            "    <div id='result'></div>" +
            "    <div id='help'>" +
            "      <h2>Help</h2>" +
            "      <p>There are a few ways of using this utility.</p>" +
            "      <p>This utility require a Windows login for the computer you attempt to control for basic integrity purposes.</p>" +
            "      <h3>Web page use</h3>" +
            "      <p>This web page works well for intended purposes. You may lock or shutdown the remote computer. You can also send custom Windows CLU commands.</p>" +
            "      <h3>Prepared command</h3>" +
            "      <p>A request to <code>http://hostname:port/api/WindowsCommand</code> without any extra parameter will try to find if there " +
            "      is a file called 'cmd.txt' in the execution folder and try to execute the command in that file line by line.</p><p>This " +
            "      approach is included to enable a browser shortcut/favorite for easy access.</p>" +
            "      <h3>REST services</h3>" +
            "      <p>This is more advanced usage. There are a few options here. For short and simple commands you may use the HTTP GET parameters " +
            "approach that is available from any browser but for more advanced stuff the HTTP POST one probably is easier.</p>" +
            "      <h4>HTTP GET with paramete</h4>" +
            "      <p>If you provide a 'cmd' parameter for the URL it will be treated as a command. Remeber to URL encode the string. E.g.:</p>" +
            "      <pre>" +
            " http://" + Environment.MachineName + ":5000/api/WindowsCommand?cmd=shutdown%20/n" +
            "      </pre>" +
            "      <p>The %20 is the URL encoded space character.</p>" +
            "      <p>The GET method is the default from any web browser. This makes this accessible from any web browser by just setting the URL of the browser to the service API and provide the command as a URL parameter.</p>" +
            "      <h4>HTTP POST</h4>" +
            "      <p>The body of anything you post to <code>http://" + Environment.MachineName + ":5000/api/WindowsCommand</code> will be treated as a command. " +
            "If there are several lines in the request body each line will be executed in sequence.</p><p>E.g. to lock computer screen:</p>" +
            "      <pre>" +
            " curl -d 'rundll32.exe user32.dll,LockWorkStation' -H 'Content-Type: text/plain' https://" + Environment.MachineName + ":5000/api/WindowsCommand" +
            "      </pre>" +
            "      <h4>Pre-set command</h4>" +
            "      <p>Any HTTP GET request to the default API endpoint without parameters will make the service look for a file named 'cmd.txt' in the same folder " +
            "it is executed from and if that file is found each line in that file will be executed in sequence.</p>" +
            "      <p><i><b>Click anywhere in this help text to close it.</b></i></p>" +
            "    </div>" +
            "  </body>" +
            "</html>";
    }
}

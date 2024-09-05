using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;
using UnityEditor;

namespace StinkySteak.WebGLEditorServer
{
    public class WebGLProfiler
    {
        private const string PROFILER_NODE_PATH = @"Data\PlaybackEngines\WebGLSupport\BuildTools\Emscripten\node\node.exe";
        private const string PROFILER_SCRIPT_PATH = @"Data\PlaybackEngines\WebGLSupport\BuildTools\websockify\websockify.js";

        private const int SOURCE_PORT = 54998;
        private const int TARGET_PORT = 64046;

        private Process _process;

        public void StartServer()
        {
            string apppath = Path.GetDirectoryName(EditorApplication.applicationPath);

            string nodeExePath = Path.Combine(apppath, PROFILER_NODE_PATH);
            string scriptPath = Path.Combine(apppath, PROFILER_SCRIPT_PATH);
            string args = $"\"{scriptPath}\" {SOURCE_PORT} localhost:{TARGET_PORT}";

            _process = new Process();
            _process.StartInfo.FileName = nodeExePath;
            _process.StartInfo.Arguments = args;
            _process.StartInfo.UseShellExecute = false;
            _process.Start();

            Debug.Log($"[{nameof(WebGLProfiler)}]: Starting profiler \n NodeExePath: {nodeExePath} \n Arguments: {args}");
        }

        public void Shutdown()
        {
            if (_process == null) return;

            try
            {
                _process.Kill();
            }
            catch (System.Exception e)
            {
                Debug.Log($"[{nameof(WebGLServerEditorWindow)}]: {e.Message}");
            }
        }
    }
}
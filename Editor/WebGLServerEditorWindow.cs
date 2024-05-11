using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace StinkySteak.WebGLEditorServer
{
    public class WebGLServerEditorWindow : EditorWindow
    {
        private Process _process;
        private const string DEFAULT_URL = "http://localhost:" + DEFAULT_PORT;
        private const string DEFAULT_PORT = "8080";
        private const string WEB_SERVER_PATH = @"Data\PlaybackEngines\WebGLSupport\BuildTools\SimpleWebServer.exe";

        private string _lastBuildPath;

        [MenuItem("Tools/WebGL Editor Server")]
        public static void SetGameBuild()
           => GetWindow<WebGLServerEditorWindow>("WebGL Editor Server");

        private void DrawField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Custom Last Build Path", GUILayout.Width(150));
            _lastBuildPath = GUILayout.TextField(_lastBuildPath, GUILayout.Height(30));
            GUILayout.EndHorizontal();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("WebGL Server Editor", GetHeadingStyle());
            GUILayout.Space(10);

            GUILayout.Label($"Last Build Path: {GetLastBuildPath()}", GUILayout.Height(30));
            DrawField();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Start Server"))
                StartServer();

            if (GUILayout.Button("Terminate Server"))
                TerminateServer();

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Launch Browser"))
                LaunchBrowser();
        }

        private GUIStyle GetHeadingStyle()
        {
            return new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                },

                fontSize = 18
            };
        }

        private string GetLastBuildPath()
        {
            if (!string.IsNullOrEmpty(_lastBuildPath))
                return _lastBuildPath;

            return EditorUserBuildSettings.GetBuildLocation(BuildTarget.WebGL);
        }

        private void StartServer()
        {
            string apppath = Path.GetDirectoryName(EditorApplication.applicationPath);
            string lastBuildPath = GetLastBuildPath();

            _process = new Process();
            _process.StartInfo.FileName = Path.Combine(apppath, WEB_SERVER_PATH);
            _process.StartInfo.Arguments = lastBuildPath + $" {DEFAULT_PORT}";
            _process.StartInfo.UseShellExecute = false;
            _process.Start();

            Debug.Log($"[{nameof(WebGLServerEditorWindow)}]: Starting server.... lastBuildPath: {lastBuildPath}");
        }

        private void TerminateServer()
        {
            try
            {
                _process.Kill();
            }
            catch (System.Exception e)
            {
                Debug.Log($"[{nameof(WebGLServerEditorWindow)}]: {e.Message}");
            }
        }

        private void LaunchBrowser()
        {
            try
            {
                if (_process.HasExited)
                {
                    StartServer();
                }
            }
            catch (System.Exception)
            {
                StartServer();
            }

            Process b = new Process();
            b.StartInfo.FileName = DEFAULT_URL;
            b.Start();
        }
    }
}
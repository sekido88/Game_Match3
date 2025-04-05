using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportDLL : EditorWindow
{
    private string outputFolder = "Assets/Plugins/";
    private string dllName = "MyLibrary.dll";
    private string scriptFolder = "Assets/Scripts"; // Thư mục chứa file .cs

    [MenuItem("Tools/Export Scripts to DLL")]
    public static void ShowWindow()
    {
        GetWindow<ExportDLL>("Export DLL");
    }

    private void OnGUI()
    {
        GUILayout.Label("Export Scripts to DLL", EditorStyles.boldLabel);
        scriptFolder = EditorGUILayout.TextField("Scripts Folder:", scriptFolder);
        outputFolder = EditorGUILayout.TextField("Output Folder:", outputFolder);
        dllName = EditorGUILayout.TextField("DLL Name:", dllName);

        if (GUILayout.Button("Build DLL"))
        {
            BuildDLL();
        }
    }

    private void BuildDLL()
    {
        if (!Directory.Exists(scriptFolder))
        {
            UnityEngine.Debug.LogError("Script folder does not exist: " + scriptFolder);
            return;
        }

        string outputPath = Path.Combine(outputFolder, dllName);
        string unityManaged = Path.Combine(EditorApplication.applicationContentsPath, "Managed");

        // Lấy danh sách tất cả file .cs trong thư mục
        string[] scriptPaths = Directory.GetFiles(scriptFolder, "*.cs", SearchOption.AllDirectories);
        if (scriptPaths.Length == 0)
        {
            UnityEngine.Debug.LogError("No .cs files found in: " + scriptFolder);
            return;
        }

        // Đường dẫn đến trình biên dịch C# (csc.exe)
        string cscPath = Path.Combine(EditorApplication.applicationContentsPath, "MonoBleedingEdge/bin/mcs");

        // Tạo lệnh biên dịch
        string compileCommand = $" -target:library -out:\"{outputPath}\" ";
        foreach (var script in scriptPaths)
        {
            compileCommand += $"\"{script}\" ";
        }

        // Thêm thư viện cần thiết
        compileCommand += $" -r:\"{unityManaged}/UnityEngine.dll\"";
        compileCommand += $" -r:\"{unityManaged}/System.dll\"";

        // Thực thi lệnh biên dịch
        ProcessStartInfo processInfo = new ProcessStartInfo(cscPath, compileCommand)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process { StartInfo = processInfo };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError("Error compiling DLL:\n" + error);
        }
        else
        {
            UnityEngine.Debug.Log("DLL successfully created: " + outputPath);
        }
    }
}

using GracesGames.SimpleFileBrowser.Scripts;
using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject FileBrowserPrefab;

    public InputField pathField;
    public Button loadButton;
    private IDescriptorReader descriporReader;
    private IDescriptorProcessor descriptorProcessor;

    private event EventHandler<EventArgs> OnLoadingProcessFinished;
    private event EventHandler<ExceptionEventArgs> OnLoadingProcessException;

    private bool isLoadFinished;

    public MainMenuController()
    {
        descriporReader = Injector.DescriptorReader;
        descriptorProcessor = Injector.DescriptorProcessor;
    }

    private void Start()
    {
        Injector.GameElementManager = new GameElementManager("emptyGEM");
        PersistanceHelper.PersistentDataPath = Application.persistentDataPath;
        OnLoadingProcessFinished += new EventHandler<EventArgs>(LoadGameMenuScene);
        OnLoadingProcessException += new EventHandler<ExceptionEventArgs>(HandleLoadingProcessException);
        isLoadFinished = false;
    }

    private void Update()
    {
        if(pathField.text == null || pathField.text.Equals(""))
        {
            loadButton.interactable = false;
        } else
        {
            loadButton.interactable = true;
        }
        if (isLoadFinished)
        {
            isLoadFinished = false;
#if UNITY_ANDROID //&& !UNITY_EDITOR
            SceneManager.LoadScene("GameMenu_Android");
#endif
#if UNITY_STANDALONE //|| UNITY_EDITOR
            SceneManager.LoadScene("GameMenu_Standalone");
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
             Application.Quit();
        }
#endif
    }

    private void LoadGameMenuScene(object sender, EventArgs e)
    {
        Debug.Log("Descriptor loading is finished!");
        isLoadFinished = true;
    }

    private void HandleLoadingProcessException(object sender, ExceptionEventArgs args)
    {
        Debug.LogError(args.Msg + "\\n" + args.Exc, this);
        //TODO error message on UI
    }


    public void LoadGame()
    {
        Thread loadingThread = new Thread( () => DescriptorLoaderUtility.LoadDescriptor(pathField.text, OnLoadingProcessFinished, OnLoadingProcessException));
        loadingThread.Start();
    }

    //Reading from Application.streamingAssetsPath.
    public void LoadTestGame()
    {
        StartCoroutine(DescriptorLoaderUtility.LoadTestDescriptor("AwesomeTestGame.zip", OnLoadingProcessFinished, OnLoadingProcessException));
    }
    

    public void BrowseFiles()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        // Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";
        // Set the mode to save or load
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
        
        fileBrowserScript.OpenFilePanel(new string[] { "zip"});
        // Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
        fileBrowserScript.OnFileSelect += LoadFileUsingPath;
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        var extensions = new[] {
            new ExtensionFilter("Zip Files", "zip"),
            new ExtensionFilter("All Files", "*" ),
        };
        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => { if (paths.Length != 0) { LoadFileUsingPath(paths[0]); } });
#endif

    }

    private void LoadFileUsingPath(string path)
    {
        if (path.Length != 0)
        {
            pathField.text = path;
        }
        else
        {
            Debug.Log("Invalid path given");
        }
    }


    private void RaiseLoadingProcessException(String msg, Exception e)
    {
        if(OnLoadingProcessException != null)
        {
            OnLoadingProcessException(null, new ExceptionEventArgs(msg, e));
        }
    }

    public class ExceptionEventArgs : EventArgs
    {
        private string msg;
        private Exception exception;

        public ExceptionEventArgs(string msg, Exception exception)
        {
            this.msg = msg;
            this.exception = exception;
        }

        public string Msg
        {
            get
            {
                return msg;
            }
        }

        public Exception Exc
        {
            get
            {
                return exception;
            }
        }
    }

    private event EventHandler<EventArgs> OnLoadingProcessFinishedForSerTest;
    public void LoadTestForTestSerrialization()
    {
        OnLoadingProcessFinishedForSerTest += new EventHandler<EventArgs>(TestSerrialization);
        StartCoroutine(DescriptorLoaderUtility.LoadTestDescriptor("AwesomeTestGame.zip", OnLoadingProcessFinishedForSerTest, OnLoadingProcessException));
    }

    private void TestSerrialization(object sender, EventArgs e)
    {
        Debug.Log("serialization starts");
        GameElementManager gem = Injector.GameElementManager;
        var stream = new MemoryStream();
        //var serializer = new DataContractSerializer(gem.GetType(), null,
        //    0x7FFF /*maxItemsInObjectGraph*/,
        //    false /*ignoreExtensionDataObject*/,
        //    true /*preserveObjectReferences : this is where the magic happens */,
        //    null /*dataContractSurrogate*/);
        //serializer.WriteObject(stream, gem);

        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        using (file = File.Open(Application.persistentDataPath + "/serializeTest2.asd", FileMode.OpenOrCreate))
        {
            bf.Serialize(file, gem);
        }
        Debug.Log("serialization has finished");

        GameElementManager asd = null;
        //asd = (GameElementManager) serializer.ReadObject(stream);
        FileStream file2 = null;
        if (File.Exists(Application.persistentDataPath + "/serializeTest2.asd"))
        {
            Debug.Log(Application.persistentDataPath);
            using (file2 = File.Open(Application.persistentDataPath + "/serializeTest2.asd", FileMode.Open))
            {
                file2.Position = 0;
                asd = (GameElementManager) bf.Deserialize(file2);
            }

            Debug.Log("deserialization has finished");
        }
        Debug.Log(asd);
    }
}

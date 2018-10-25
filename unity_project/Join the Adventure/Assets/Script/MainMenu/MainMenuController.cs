using GracesGames.SimpleFileBrowser.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            SceneManager.LoadScene("GameMenu");
        }
    }

    private void LoadGameMenuScene(object sender, EventArgs e)
    {
        Debug.Log("Descriptor loading is finished!");
        isLoadFinished = true;
    }

    private void HandleLoadingProcessException(object sender, ExceptionEventArgs args)
    {
        Debug.LogError(args.Msg + "\\n" + args.Exc.StackTrace, this);
        //TODO error message on UI
    }


    public void LoadGame()
    {
        //Debug.Log("LoadGame invoked!");
        //Thread loadingThread = new Thread(LoadDescriptor);
        //loadingThread.Start();

        StartCoroutine(DescriptorLoaderUtility.LoadDescriptor(pathField.text, OnLoadingProcessFinished, OnLoadingProcessException));
    }

    public void LoadTestGame()
    {
        //version 1
        //Thread loadingThread = new Thread(LoadTestDescriptor);
        //loadingThread.Start();

        //version2
        //var a = Resources.Load<TextAsset>("AwesomeTestGame");
        //var b = GameDescriptor.Deserialize(a.text);
        //descriptorProcessor.ProcessGameDescriptor(b);
        //OnLoadingProcessFinished(null, EventArgs.Empty);

        //version3
        StartCoroutine(DescriptorLoaderUtility.LoadDescriptor(Application.streamingAssetsPath + "/AwesomeTestGame.zip", OnLoadingProcessFinished, OnLoadingProcessException));
    }

    private void LoadTestDescriptor()
    {
        GameDescriptor descriptor = null;
        try
        {
            descriptor = descriporReader.ReadDescriptor("Assets/TestXML/AwesomeTestGame.xml");
        }
        catch (Exception e)
        {
            RaiseLoadingProcessException("Could not load descriptors! " + e.Message, e);
        }
        try
        {
            descriptorProcessor.ProcessGameDescriptor(descriptor);
        }
        catch (Exception e)
        {
            RaiseLoadingProcessException("Could not process descriptors! " + e.Message, e);
        }

        if (OnLoadingProcessFinished != null)
        {
            OnLoadingProcessFinished(null, EventArgs.Empty);
        }

    }

    public void BrowseFiles()
    {
        // Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";
        // Set the mode to save or load
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        
        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
        
        fileBrowserScript.OpenFilePanel(new string[] { "zip"});
        // Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
        fileBrowserScript.OnFileSelect += LoadFileUsingPath;
        
    }

    private void LoadFileUsingPath(string path)
    {
        if (path.Length != 0)
        {
            //BinaryFormatter bFormatter = new BinaryFormatter();
            //// Open the file using the path
            //FileStream file = File.OpenRead(path);
            //// Convert the file from a byte array into a string
            //string fileData = bFormatter.Deserialize(file) as string;
            //// We're done working with the file so we can close it
            //file.Close();
            //// Set the LoadedText with the value of the file
            //_loadedText.GetComponent<Text>().text = "Loaded data: \n" + fileData;
            pathField.text = path;
        }
        else
        {
            Debug.Log("Invalid path given");
        }
    }

    /**
        * Expensive process, must run on working thread.
        * Callbacks are handled in events: OnLoadingProcessFinished, OnLoadingProcessException
        */
    //private void LoadDescriptor()
    //{
    //    GameDescriptor[] descriptors = null;
    //    try
    //    {
    //        string path = pathField.text;
    //        if (path.EndsWith("\\") || path.EndsWith("/"))
    //        {
    //            string[] xmls = Directory.GetFiles(pathField.text);
    //            descriptors = descriporReader.ReadMultipleDescriptor(xmls);
    //        }
    //        else
    //        {
    //            descriptors = new GameDescriptor[] { descriporReader.ReadDescriptor(path) };
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        RaiseLoadingProcessException("Could not load descriptors! " + e.Message, e);
    //    }


    //    try
    //    {
    //        descriptorProcessor.ProcessMultipleGameDescriptor(descriptors);
    //    }
    //    catch (Exception e)
    //    {
    //        RaiseLoadingProcessException("Could not process descriptors! " + e.Message, e);
    //    }

    //    if (OnLoadingProcessFinished != null)
    //    {
    //        OnLoadingProcessFinished(null, EventArgs.Empty);
    //    }
    //}



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
}

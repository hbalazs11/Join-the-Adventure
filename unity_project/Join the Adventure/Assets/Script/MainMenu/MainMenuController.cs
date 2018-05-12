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
        Debug.Log("LoadGame invoked!");
        Thread loadingThread = new Thread(LoadDescriptor);
        loadingThread.Start();
    }

    public void LoadTestGame()
    {
        Thread loadingThread = new Thread(LoadTestDescriptor);
        loadingThread.Start();
    }

    private void LoadTestDescriptor()
    {
        GameDescriptor descriptor = null;
        try
        {
            descriptor = descriporReader.ReadDescriptor("Assets/TestXML/AwsomeTestGame.xml");
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

    /**
        * Expensive process, must run on working thread.
        * Callbacks are handled in events: OnLoadingProcessFinished, OnLoadingProcessException
        */
    private void LoadDescriptor()
    {
        GameDescriptor[] descriptors = null;
        try
        {
            string path = pathField.text;
            if (path.EndsWith("\\") || path.EndsWith("/"))
            {
                string[] xmls = Directory.GetFiles(pathField.text);
                descriptors = descriporReader.ReadMultipleDescriptor(xmls);
            }
            else
            {
                descriptors = new GameDescriptor[] { descriporReader.ReadDescriptor(path) };
            }
        }
        catch (Exception e)
        {
            RaiseLoadingProcessException("Could not load descriptors! " + e.Message, e);
        }


        try
        {
            descriptorProcessor.ProcessMultipleGameDescriptor(descriptors);
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
    
    

    private void RaiseLoadingProcessException(String msg, Exception e)
    {
        if(OnLoadingProcessException != null)
        {
            OnLoadingProcessException(null, new ExceptionEventArgs(msg, e));
        }
    }

    private class ExceptionEventArgs : EventArgs
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

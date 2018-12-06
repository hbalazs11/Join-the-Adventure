public class ObjectManager  {

    private static GameElementManager gameElementManager;
    private static ILogger logger;

    public static GameElementManager CurrentGEM
    {
        get
        {
            return gameElementManager;
        }
        set
        {
            gameElementManager = value;
        }
    }

    public static ILogger Logger
    {
        get
        {
            if (logger == null)
            {
                logger = new Logger();
            }
            return logger;
        }
    }
}

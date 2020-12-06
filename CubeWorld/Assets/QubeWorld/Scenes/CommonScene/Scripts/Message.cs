using System.Collections.Generic;

namespace CommonScene
{
    public static class Message
    {
        public static int Version { get; private set; }

        private const float TIMEOUT_MESSAGE = 30f;
        private static List<VolatileMessage> volatileMessages = new List<VolatileMessage>(100);
        private static CWConsoleListener listener = new CWConsoleListener();

        #region Public methods

        public static void Initialize()
        {
            volatileMessages.Clear();
            Version = 1;
        }

        public static void Update(float deltaTime)
        {
            var lastRemoved = -1;
            for (var i = 0; i < volatileMessages.Count; i++)
            {
                var newDeltaTime = volatileMessages[i].timeDelta + deltaTime;
                volatileMessages[i] = new VolatileMessage()
                {
                    timeDelta = newDeltaTime,
                    message = volatileMessages[i].message
                };
                if (newDeltaTime > TIMEOUT_MESSAGE)
                {
                    lastRemoved = i;
                }
            }
            if (lastRemoved >= 0)
            {
                volatileMessages.RemoveRange(0, lastRemoved + 1);
                updateVersion();
            }
        }

        static public void AddMessage(string _message)
        {
            volatileMessages.Add(new VolatileMessage() { timeDelta = 0f, message = _message });
            updateVersion();
        }

        static public string GetMessages()
        {
            var stringBuilder = new System.Text.StringBuilder(1024);
            for (var i = 0; i < volatileMessages.Count; i++)
            {
                stringBuilder.Append(volatileMessages[i].message);
                if (i != volatileMessages.Count - 1)
                {
                    stringBuilder.Append('\n');
                }
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region Private methods

        static private void updateVersion()
        {
            Version = Version + 1;
        }

        #endregion

        private struct VolatileMessage
        {
            public float timeDelta;
            public string message;
        }

        private class CWConsoleListener : CubeWorld.Console.ICWConsoleListener
        {
            public CWConsoleListener()
            {
                Message.Initialize();
                CubeWorld.Console.CWConsole.Singleton.listener = this;
                var log = CubeWorld.Console.CWConsole.Singleton.TextLog;
                if (!string.IsNullOrEmpty(log))
                {
                    Message.AddMessage(log);
                }
            }

            public void Log(CubeWorld.Console.CWConsole.LogLevel level, string message)
            {
                Message.AddMessage(string.Format("{0}: {1}", level, message));
            }
        }
    }
}

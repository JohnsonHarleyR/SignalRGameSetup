using System.Collections.Generic;

namespace SignalRGameSetup.Py
{
    public static class ScriptSources
    {
        // STORE ALL PYTHON SOURCES HERE - See "AddDefaultSources()" to add sources

        private static Dictionary<string, string> _classes;
        public static Dictionary<string, string> Classes
        {
            get
            {
                if (IsNullOrEmpty())
                {
                    AddDefaultSources();
                }
                return _classes;
            }
        }

        private static void AddDefaultSources()
        {
            if (_classes == null)
            {
                _classes = new Dictionary<string, string>();
            }

            // Add all the python sources
            _classes.Add("testCalculator", @"Py\Scripts\TestCalculator.py");

        }

        private static bool IsNullOrEmpty()
        {
            if (_classes == null || _classes.Count == 0)
            {
                return true;
            }
            return false;
        }

    }
}
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SignalRGameSetup.Py.Interfaces;
using System;
using System.IO;

namespace SignalRGameSetup.Py
{
    public class PythonEngine : IPythonEngine
    {
        private ScriptEngine engine;
        private ScriptSource source;
        private ScriptScope scope;

        public PythonEngine(string scriptSource)
        {
            // create instance of the engine
            engine = Python.CreateEngine();

            // read the code from the file
            source = engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptSource));
            scope = engine.CreateScope();

            // execute the script
            source.Execute(scope);
        }

        public void CreateInstance()
        {

        }


    }
}
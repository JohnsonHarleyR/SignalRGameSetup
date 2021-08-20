using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SignalRGameSetup.Py.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace SignalRGameSetup.Py
{
    public class PythonEngine : IPythonEngine
    {
        private ScriptEngine engine;
        private Dictionary<string, ScriptSource> sources;
        private ScriptScope scope;

        public PythonEngine()
        {
            sources = new Dictionary<string, ScriptSource>();

            // create instance of the engine
            engine = Python.CreateEngine();

            // create scope
            scope = engine.CreateScope();

            // Add names of classes in the default python scripts
            AddDefaultSources();

        }

        public dynamic CreateInstance(string className)
        {
            // Get the class
            var classReference = scope.GetVariable(className);

            // Initialize it
            var instance = engine.Operations.CreateInstance(classReference);

            // Return it
            return instance;
        }

        private void AddDefaultSources()
        {
            if (sources != null)
            {
                foreach (var item in ScriptSources.Classes)
                {

                    // read script from the file
                    var source = engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, item.Value));


                    // execute the script
                    source.Execute(scope);

                    // store in dictionary
                    sources.Add(item.Key, source);
                }
            }
        }

        public void AddSource(string className, string path)
        {
            // read script from the file
            ScriptSource source =
                engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));

            // execute the script
            source.Execute(scope);

            // store in dictionary
            sources.Add(className, source);
        }


    }
}
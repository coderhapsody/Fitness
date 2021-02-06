using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConfigurationSingletonProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class ConfigurationSingletonProvider
    {
        private static volatile ConfigurationSingletonProvider instance;
        private static object syncRoot = new Object();

        private ConfigurationSingletonProvider() { }

        public static ConfigurationSingletonProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConfigurationSingletonProvider();
                    }
                }

                return instance;
            }
        }

        public string GetValue(string key)
        {
            string result = String.Empty;
            using(FitnessDataContext ctx = new FitnessDataContext())
            {
                result = ctx.Configurations.SingleOrDefault(config => config.Key == key).Value;
            }
            return result;
        }

        public T GetValue<T>(string key)
        {
            string result = GetValue(key);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public void SetValue(string key, string value)
        {
            using(FitnessDataContext ctx = new FitnessDataContext())
            {
                var configuration = ctx.Configurations.SingleOrDefault(config => config.Key == key);
                configuration.Key = key;
                configuration.Value = value;
                ctx.SubmitChanges();    
            }
        }

        public void SetValue<T>(string key, T value)
        {
            SetValue(key, Convert.ToString(value));
        }

        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                SetValue(key, value);
            }
        }
    }
}
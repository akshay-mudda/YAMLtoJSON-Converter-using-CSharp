using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace YAMLtoJSON_Converter_using_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Read source and destination paths from the app.config file
                string sourceDirectory = ConfigurationManager.AppSettings["SourceDirectory"];
                string destinationDirectory = ConfigurationManager.AppSettings["DestinationDirectory"];

                // Ensure the destination directory exists
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Get all YAML files from the source directory
                string[] yamlFiles = Directory.GetFiles(sourceDirectory, "*.yaml");

                if (yamlFiles.Length == 0)
                {
                    Console.WriteLine("No YAML files found in the source directory.");
                    return;
                }

                foreach (string yamlFilePath in yamlFiles)
                {
                    try
                    {
                        // Get the file name without extension
                        string fileName = Path.GetFileNameWithoutExtension(yamlFilePath);

                        // Set the output JSON file path
                        string outputFilePath = Path.Combine(destinationDirectory, fileName + ".json");

                        // Convert YAML to JSON
                        var yamlData = File.ReadAllText(yamlFilePath);
                        var jsonData = ConvertYamlToJson(yamlData);

                        // Write JSON file
                        File.WriteAllText(outputFilePath, jsonData);

                        Console.WriteLine($"Successfully converted '{yamlFilePath}' to JSON.");

                        // Optionally, delete the source YAML file after conversion
                        File.Delete(yamlFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error converting file {yamlFilePath}: {ex.Message}");
                    }
                }

                Console.WriteLine("Conversion process completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Helper method to convert YAML string to JSON string
        public static string ConvertYamlToJson(string yamlData)
        {
            // Deserialize the YAML into an object
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize<object>(yamlData);

            // Serialize the object into JSON format using Newtonsoft.Json
            string json = JsonConvert.SerializeObject(yamlObject, Formatting.Indented);

            return json;
        }
    }
}
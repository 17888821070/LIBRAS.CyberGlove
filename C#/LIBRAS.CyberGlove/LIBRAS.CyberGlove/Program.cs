using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LIBRAS.CyberGlove
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    class Program
    {
        static int QUANTITY_SENSORS = 23;

        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        static string[,] ReadFile()
        {
            string[] lines;

            try
            {
                lines = File.ReadAllLines(@"C:\LIBRAS.CyberGlove\gesture.txt");
                string[,] gestures = new string[lines.Length, QUANTITY_SENSORS];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] sensors = lines[i].Split(' ');
                    for (int j = 0; j < QUANTITY_SENSORS; j++)
                    {
                        gestures[i, j] = sensors[j];
                    }
                }

                return gestures;
            }
            catch (Exception)
            {
                Console.WriteLine("Gesture File Not Found");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return null;
        }

        static async Task InvokeRequestResponseService()
        {
            Console.WriteLine("Detecting Gestures...\n");

            string[,] gestures = ReadFile();

            for (int i = 0; i < (gestures.Length / QUANTITY_SENSORS); i++)
            {
                using (var client = new HttpClient())
                {
                    var scoreRequest = new
                    {
                        Inputs = new Dictionary<string, StringTable>() { 
                            { 
                                "input1", 
                                new StringTable() 
                                {
                                    ColumnNames = new string[] {"I01", "I02", "I03", "I04", "I05", "I06", "I07", "I08", "I09", "I10", "I11", "I12", "I13", "I14", "I15", "I16", "I17", "I18", "I19", "I20", "I21", "I22", "I23", "Y"},
                                    Values = new string[,] { { gestures[i, 0],
                                                               gestures[i, 1],
                                                               gestures[i, 2],
                                                               gestures[i, 3],
                                                               gestures[i, 4],
                                                               gestures[i, 5],
                                                               gestures[i, 6],
                                                               gestures[i, 7],
                                                               gestures[i, 8],
                                                               gestures[i, 9],
                                                               gestures[i, 10],
                                                               gestures[i, 11],
                                                               gestures[i, 12],
                                                               gestures[i, 13],
                                                               gestures[i, 14],
                                                               gestures[i, 15],
                                                               gestures[i, 16],
                                                               gestures[i, 17],
                                                               gestures[i, 18],
                                                               gestures[i, 19],
                                                               gestures[i, 20],
                                                               gestures[i, 21],
                                                               gestures[i, 22],
                                                               ""}, }
                                }
                            },
                        },
                        GlobalParameters = new Dictionary<string, string>() { }
                    };

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JKh0CGa3HIRr6I/X86hgZScBGDeoznHKHwqW2Opi4d7xLq1A/jYyQ8g0PMOUuFDyX1fhJmOBmXy2LoT6NffRXw==");
                    client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/5e87c662fc5c4ed7880a99176ae465ae/services/3240df9bf131423993ccd01734f8e1e8/execute?api-version=2.0&details=true");

                    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                        foreach (var gesture in obj.Results.output1.value.Values)
                        {
                            Console.Write("Gesture {0}: ", i+1);
                            Console.WriteLine(gesture.Last);
                        }
                    }
                    else
                    {
                        Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                        Console.WriteLine(response.Headers.ToString());
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                    }
                }
            }
            Console.WriteLine("\nGesture Detection Complete");
            Console.ReadKey();
        }
    }
}
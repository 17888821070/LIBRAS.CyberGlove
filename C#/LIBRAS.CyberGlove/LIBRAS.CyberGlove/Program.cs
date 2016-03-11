using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LIBRAS.CyberGlove
{

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        static async Task InvokeRequestResponseService()
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
                                Values = new string[,] { {"-0.96624", "0.02918", "-0.44237", "-0.28112", "-1.48672", "-1.73621", "-0.01506", "0.0348208", "-1.98375", "-2.02048", "0.23122", "-0.0639592", "-1.49345", "-2.07408", "0.19969", "0.0951608", "-0.88139", "0.0546", "0.05928", "0.0474808", "-0.6279", "-0.06904", "-0.09408", ""}, }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JKh0CGa3HIRr6I/X86hgZScBGDeoznHKHwqW2Opi4d7xLq1A/jYyQ8g0PMOUuFDyX1fhJmOBmXy2LoT6NffRXw==");
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/5e87c662fc5c4ed7880a99176ae465ae/services/3240df9bf131423993ccd01734f8e1e8/execute?api-version=2.0&details=true");

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    Console.WriteLine(obj.Results.output1.value.Values[0].Last);
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
    }
}
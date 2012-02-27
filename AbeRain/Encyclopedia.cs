using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AbeRain
{
    class Encyclopedia
    {

        public static List<String> ParentTopics(string topic)
        {
            List<string> results = new List<string>();
            string res = POST("http://wdm.cs.waikato.ac.nz:8080/services/search?responseFormat=xml"
                + "&query=" + topic, "");

            // Check for disambiguation
            Match match = Regex.Match(res, "<Sense commonness=\"(.*)\" id=\"(.*)\" title=\"(.*)\">");
            if (match.Success)
            {
                string id;

                id = match.ToString();
                id = id.Substring(id.IndexOf("id=") + 4);
                id = id.Substring(0, id.IndexOf('"'));

                //Console.WriteLine("DEBUG: Found an ambiguous page; went with most relevant result.");

                // Re-request a non-ambiguous page to continue on
                res = POST("http://wdm.cs.waikato.ac.nz:8080/services/search?responseFormat=xml" 
                    + "&query=" + topic, "");
            }

            // <Category id="2475793" title="Junglefowls"/> 
            MatchCollection matches = Regex.Matches(res, "<Category id=\"(.*)\"");
            foreach (Match m in matches)
            {
                string t;
                t = Regex.Match(m.ToString(), "title=\"(.*)\"").ToString();
                t = t.Substring(t.IndexOf('"') + 1);
                t = t.Substring(0, t.Length - 1);

                results.Add(t);
            }

            return results;
        }

        public static List<String> LookUpFacts(string topic)
        {
            List<String> results = new List<string>();
            string res = POST("http://openmind.media.mit.edu/en/concept/", 
                "concept_name=" + topic
            );

            // <span class="sentence"><a href="/en/concept/Chickens/">Chickens</a> are <a href="/en/concept/animals/">animals</a></span>
            MatchCollection matches = Regex.Matches(res, "<span class=\"sentence\">(.*)</span>");
            foreach (Match match in matches)
            {
                // Sanitize
                string m = match.ToString();
                m = Regex.Replace(m, "<(.*?)>", "");

                // Format
                m = char.ToUpper(m[0]) + m.Substring(1); // Capitalize first letter

                results.Add(m);
            }

            return results;
        }

        static string POST(string url, string post_data)
        {
            string server_response;
            WebResponse response;
            WebRequest request;
            Stream data_stream;
            StreamReader sread;
            byte[] byte_array = Encoding.UTF8.GetBytes(post_data);

            // Prepare the request
            request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byte_array.Length;

            // Open a socket and write the request in
            data_stream = request.GetRequestStream();
            data_stream.Write(byte_array, 0, byte_array.Length);
            data_stream.Close();

            // Read the response
            response = request.GetResponse();
            data_stream = response.GetResponseStream();
            sread = new StreamReader(data_stream);
            server_response = sread.ReadToEnd();

            // Clean up
            sread.Close();
            data_stream.Close();
            response.Close();

            return server_response;
        }
    }
}

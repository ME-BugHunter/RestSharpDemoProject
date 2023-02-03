using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace RestSharpDemoProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.github.com");

            client.Authenticator = new HttpBasicAuthenticator("ME-BugHunter", "");

            //Requesting just the first issue
            RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues/{id}", Method.Get);
            request.AddUrlSegment("user", "ME-BugHunter");
            request.AddUrlSegment("id", "1");
            request.AddUrlSegment("repoName", "Postman");

            var response = client.Execute(request);

            Console.WriteLine("Status code: " + response.StatusCode);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Console.WriteLine("Issue name: " + issue.title);
            Console.WriteLine("Issue number: " + issue.number);
            Console.WriteLine();

            //Requesting list of all issues
            RestRequest request2 = new RestRequest("/repos/ME-BugHunter/Postman/issues", Method.Get);
            var response2 = client.Execute(request2);

            var issues = JsonSerializer.Deserialize<List<Issue>>(response2.Content);

            /*foreach (var iss in issues)
            {
                Console.WriteLine("Issue title: " + iss.title);
                Console.WriteLine("Issue number: " + iss.number);
            }*/

            //Get the issue labels
            RestRequest request3 = new RestRequest("/repos/{user}/{repoName}/issues/{id}/labels", Method.Get);
            request3.AddUrlSegment("user", "ME-BugHunter");
            request3.AddUrlSegment("repoName", "Postman");
            request3.AddUrlSegment("id", "25");

            var response3 = client.Execute(request3);

            var labels = JsonSerializer.Deserialize<List<Labels>>(response3.Content);

            foreach(var label in labels)
            {
                Console.WriteLine("Label ID: " + label.id);
                Console.WriteLine("Label Node ID: " + label.node_id);
                Console.WriteLine("Label name: " + label.name);
                Console.WriteLine();
            }

            RestRequest request4 = new RestRequest("/repos/{user}/{repoName}/issues", Method.Post);

            var issueBody = new
            {
                title = "Test issue from RestSharp" + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] {"bug", "critical", "release"}
            };
            request4.AddBody(issueBody);

            request4.AddUrlSegment("user", "ME-BugHunter");
            request4.AddUrlSegment("repoName", "Postman");

            var response4 = client.Execute(request4);

            Console.WriteLine("Status code: " + response4.StatusCode);
            Console.WriteLine("Response status: " + response4.ResponseStatus);

            var issue1 = JsonSerializer.Deserialize<Issue>(response4.Content);

            Console.WriteLine("Issue title: " + issue1.title);
            Console.WriteLine("Issue number: " + issue1.number);
        }
    }
}
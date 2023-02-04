using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics.Contracts;
using System.Net;
using System.Text.Json;

namespace GitHubApiTests
{
    public class ApiTests

    {
        private RestClient client;
        private const string baseUrl = ("https://api.github.com");
        private const string partialUrl = ("/repos/ME-BugHunter/Postman/issues");
        private const string username = "BugHunter";
        private const string password = "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";


        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        [Test]
        public void Test_GetSingleIssue()
        { 
            var request = new RestRequest($"{partialUrl}/1", Method.Get);

            var response = client.Execute(request);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status code property");
            Assert.That(issue.title, Is.EqualTo("Issue with labels"));
            Assert.That(issue.number, Is.EqualTo(1));
        }

        [Test]
        [Timeout (3000)]
        public void Test_AllIssues()
        {
            var request = new RestRequest($"{partialUrl}", Method.Get);

            var response = client.Execute(request);

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status code property");
            foreach (var issue in issues)
            {
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.number, Is.GreaterThan(0));
            }
        }

       /* [Test]
        public void Test_GetSingleIssueWithLabels()
        {
            var request = new RestRequest("${partialUrl}/1", Method.Get);

            var response = client.Execute(request);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status code property");
            
            for (int i=0; i<issue.labels.Count; i++)
            {
                Assert.That(labels.id, Is.Not.Empty);
            }
        }*/

        [Test]
        public void Test_CreateNewIssue()
        {    
            var issueBody = new
            {
                title = "RestSharp API Test1",
                body = "some body for my test issue",
                labels = new string[] { "bug", "critical", "release" }
            };

            var issue = CreateIssue(issueBody);

            //Assert
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo("RestSharp API Test1"));
            Assert.That(issue.body, Is.EqualTo("some body for my test issue"));
        }

        [Test]
        public void Test_EditIssue()
        {
            var issueBody = new
            {
                title = "EDITED: Test issue from RestSharp",
            };

            var issue = CreateIssue(issueBody);

            //Assert
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo("EDITED: Test issue from RestSharp"));
        }

        private Issue CreateIssue(object body)
        {
            var request = new RestRequest($"{partialUrl}", Method.Post);
            request.AddBody(body);

            //Act
            var response = client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            return issue;
        }

        private Issue EditIssue(object body)
        {
            var request = new RestRequest($"{partialUrl}/1", Method.Patch);
            request.AddBody(body);

            //Act
            var response = client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            return issue;
        }
    }
}
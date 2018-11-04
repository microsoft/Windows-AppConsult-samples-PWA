using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Alexa.NET.Response;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Linq;
using System;

namespace AlexaSkill
{
    public static class Alexa
    {
        [FunctionName("Alexa")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            bool isValid = await ValidateRequest(req, log, skillRequest);
            if (!isValid)
            {
                return new BadRequestResult();
            }

            var requestType = skillRequest.GetRequestType();

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Welcome to AppConsult!");
                response.Response.ShouldEndSession = false;
            }

            else if (requestType == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;

                if (intentRequest.Intent.Name == "LastPosts")
                {
                    string rss = "https://blogs.msdn.microsoft.com/appconsult/feed/";
                    string output = string.Empty;
                    List<string> news = null;
                    try
                    {
                        news = await ParseFeed(rss);
                    }
                    catch (Exception exc)
                    {
                        output = "An error has occured, please try again later";
                    }

                    output = $"The title of the last article is {news.FirstOrDefault()}";

                    response = ResponseBuilder.Tell(output);
                }
                else if (intentRequest.Intent.Name == "AMAZON.CancelIntent")
                {
                    response = ResponseBuilder.Tell("Cancelling.");
                }
                else if (intentRequest.Intent.Name == "AMAZON.HelpIntent")
                {
                    response = ResponseBuilder.Tell("You can ask which are the latest article or when the last article was published.");

                    response.Response.ShouldEndSession = false;
                }
                else if (intentRequest.Intent.Name == "AMAZON.StopIntent")
                {
                    response = ResponseBuilder.Tell("Bye");
                }
            }
            else if (requestType == typeof(SessionEndedRequest))
            {
                log.LogInformation("Session ended");
                response = ResponseBuilder.Empty();
                response.Response.ShouldEndSession = true;
            }

            return new OkObjectResult(response);
        }

        private static async Task<List<string>> ParseFeed(string url)
        {
            List<string> news = new List<string>();
            using (var xmlReader = XmlReader.Create(url, new XmlReaderSettings { Async = true }))
            {
                RssFeedReader feedReader = new RssFeedReader(xmlReader);
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == Microsoft.SyndicationFeed.SyndicationElementType.Item)
                    {
                        var item = await feedReader.ReadItem();
                        news.Add(item.Title);
                    }
                }
            }

            return news;
        }

        private static async Task<bool> ValidateRequest(HttpRequest request, ILogger log, SkillRequest skillRequest)
        {
            request.Headers.TryGetValue("SignatureCertChainUrl", out var signatureChainUrl);
            if (string.IsNullOrWhiteSpace(signatureChainUrl))
            {
                log.LogError("Validation failed. Empty SignatureCertChainUrl header");
                return false;
            }

            Uri certUrl;
            try
            {
                certUrl = new Uri(signatureChainUrl);
            }
            catch
            {
                log.LogError($"Validation failed. SignatureChainUrl not valid: {signatureChainUrl}");
                return false;
            }

            request.Headers.TryGetValue("Signature", out var signature);
            if (string.IsNullOrWhiteSpace(signature))
            {
                log.LogError("Validation failed - Empty Signature header");
                return false;
            }

            request.Body.Position = 0;
            var body = await request.ReadAsStringAsync();
            request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
            {
                log.LogError("Validation failed - the JSON is empty");
                return false;
            }

            bool isTimestampValid = RequestVerification.RequestTimestampWithinTolerance(skillRequest);
            bool valid = await RequestVerification.Verify(signature, certUrl, body);

            if (!valid && !isTimestampValid)
            {
                log.LogError("Validation failed - RequestVerification failed");
            }
            return valid;
        }
    }
}

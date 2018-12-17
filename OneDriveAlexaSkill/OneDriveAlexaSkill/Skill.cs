using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using OneDriveAlexaSkill.Extensions;
using Microsoft.Graph;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace OneDriveAlexaSkill
{
    public static class Skill
    {
        [FunctionName("OneDriveAlexaSkill")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            // Verifies that the request is indeed coming from Alexa.
            var isValid = await skillRequest.ValidateRequest(req, log);
            if (!isValid)
            {
                return new BadRequestResult();
            }

            var request = skillRequest.Request;
            var token = skillRequest.Session.User.AccessToken;

            var client = GetAuthenticatedClientForUser(token);

            SkillResponse response = null;

            try
            {
                if (request is LaunchRequest launchRequest)
                {
                    log.LogInformation("Session started");
                    var me = await client.Me.Request().GetAsync();
                    response = ResponseBuilder.Tell($"Hello {me.DisplayName}");
                    response.Response.ShouldEndSession = false;
                }
                else if (request is IntentRequest intentRequest)
                {
                    // Checks whether to handle system messages defined by Amazon.
                    var systemIntentResponse = HandleSystemIntentRequest(intentRequest);
                    if (systemIntentResponse.IsHandled)
                    {
                        response = systemIntentResponse.Response;
                    }
                    else
                    {
                        if (intentRequest.Intent.Name == "OneDriveQuota")
                        {
                            log.LogInformation($"Request url: {client.Me.Drive.Request().RequestUrl}");
                            var drive = await client.Me.Drive.Request().GetAsync();
                            int used = ConvertBytesToTereabytes(drive.Quota.Used);
                            int total = ConvertBytesToTereabytes(drive.Quota.Total);

                            response = ResponseBuilder.Tell($"You are using {used} GB out of a total of {total} GB");
                        }
                    }
                }
                else if (request is SessionEndedRequest sessionEndedRequest)
                {
                    log.LogInformation("Session ended");
                    response = ResponseBuilder.Empty();
                    response.Response.ShouldEndSession = true;
                }
            }
            catch (Exception exc)
            {
                log.LogError(exc, "Error getting the response");
                response = ResponseBuilder.Tell("I'm sorry, there was an unexpected error. Please, try again later.");
            }

            return new OkObjectResult(response);
        }

        private static (bool IsHandled, SkillResponse Response) HandleSystemIntentRequest(IntentRequest request)
        {
            SkillResponse response = null;

            if (request.Intent.Name == "AMAZON.CancelIntent")
            {
                response = ResponseBuilder.Tell("Canceling...");
            }
            else if (request.Intent.Name == "AMAZON.HelpIntent")
            {
                response = ResponseBuilder.Tell("Help...");
                response.Response.ShouldEndSession = false;
            }
            else if (request.Intent.Name == "AMAZON.StopIntent")
            {
                response = ResponseBuilder.Tell("Stopping...");
            }

            return (response != null, response);
        }

        public static GraphServiceClient GetAuthenticatedClientForUser(string token)
        {
            GraphServiceClient graphClient = null;
            
            // Create Microsoft Graph client.
            try
            {
                graphClient = new GraphServiceClient(
                    "https://graph.microsoft.com/v1.0",
                    new DelegateAuthenticationProvider(
                        (requestMessage) =>
                        {
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                            return Task.CompletedTask;
                        }));

                return graphClient;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not create a graph client: " + ex.Message);
            }

            return graphClient;
        }

        private static int ConvertBytesToTereabytes(long? bytes)
        {
            double result = bytes.Value / 1024 / 1024 / 1024;
            return (int)result;
        }

    }
}

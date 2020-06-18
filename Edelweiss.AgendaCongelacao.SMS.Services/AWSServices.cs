using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;

namespace Edelweiss.AgendaCongelacao.SMS.Services
{
    public static class AWSServices
    {
        public static String SendSMS(String recipientPhoneNumber, String message)
        {
            BasicAWSCredentials awsCredentials =
                new BasicAWSCredentials("", "");

            AmazonSimpleNotificationServiceClient snsClient =
                new AmazonSimpleNotificationServiceClient(awsCredentials, Amazon.RegionEndpoint.USEast1);

            PublishRequest pubRequest = new PublishRequest();
            pubRequest.PhoneNumber = recipientPhoneNumber;
            pubRequest.Message = message;

            PublishResponse pubResponse = snsClient.Publish(pubRequest);
            return pubResponse.MessageId;
        }
    }
}

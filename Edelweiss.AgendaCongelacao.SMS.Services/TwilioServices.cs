using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Edelweiss.AgendaCongelacao.SMS.Services
{
    public static class TwilioServices
    {
        public static String SendSMS(String recipientPhoneNumber, String message)
        {
            String twilioPublicNumber = "";
            TwilioClient.Init("", "");

            MessageResource mr = MessageResource.Create
                (
                    body: message
                    , from: new Twilio.Types.PhoneNumber(twilioPublicNumber)
                    , to: new Twilio.Types.PhoneNumber(recipientPhoneNumber)
                );

            return mr.Sid;
        }
    }
}

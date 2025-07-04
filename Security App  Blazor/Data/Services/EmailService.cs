using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;

namespace Security_App__Blazor.Data.Services;

public class EmailService
{
    private readonly string _awsAccessKeyId;
    private readonly string _awsSecretKey;
    private readonly string _senderAddress;

    public EmailService(IConfiguration configuration)
    {
        _awsAccessKeyId = configuration["AwsSes:AccessKeyId"] ?? string.Empty;
        _awsSecretKey = configuration["AwsSes:SecretKey"] ?? string.Empty;
        _senderAddress = configuration["AwsSes:SenderAddress"] ?? string.Empty;
    }

    public async Task SendEmailAsync(string recipient, string subject, string htmlBody, string textBody)
    {
        using (var client = new AmazonSimpleEmailServiceClient(_awsAccessKeyId, _awsSecretKey, Amazon.RegionEndpoint.USEast1))
        {
            var sendRequest = new SendEmailRequest
            {
                Source = _senderAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { recipient }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = textBody
                        }
                    }
                }

            };
            try
            {
                var response = await client.SendEmailAsync(sendRequest);
                Console.WriteLine("Email sent! Message ID = " + response.MessageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }

    }
}

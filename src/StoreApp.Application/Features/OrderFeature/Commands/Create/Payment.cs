using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace StoreApp.Application.Features.OrderFeature.Commands.Create
{
    public class PaymentRequestResponse
    {
        [JsonProperty("authority")]
        public string Authority { get; set; }

        [JsonProperty("payment_url")] // ممکن است Link در JSON این نام را داشته باشد
        public string Link { get; set; }
    }

    public class PaymentVerificationResponse
    {
        public int Status { get; set; }
        public string RefId { get; set; }
    }

    public class PaymentVerificationWithExtraResponse : PaymentVerificationResponse { }


    public class Payment
    {
        private const string MerchantId = "c632f574-bd37-15e7-99ca-000c295eb9d3";

        private readonly int _amount;

        public Payment(int amount)
        {
            _amount = amount;
        }

        public async Task<PaymentRequestResponse> PaymentRequest(string description, string callbackUrl, string email = null, string mobile = null)
        {

            Console.WriteLine("📢 Entered PaymentRequest method");
            PaymentRequestResponse result;
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(new
                {
                    MerchantID = MerchantId,
                    Amount = _amount,
                    Description = description,
                    Email = email,
                    Mobile = mobile,
                    CallbackURL = callbackUrl
                });

                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<Payment>();
                logger.LogInformation("🔹 JSON Sent to Zarinpal: {content}", content);

                using HttpResponseMessage httpResponseMessage =
                    await httpClient.PostAsync("https://sandbox.zarinpal.com/pg/rest/WebGate/PaymentRequest.json",
                                               new StringContent(content, Encoding.UTF8, "application/json"));

                var raw = await httpResponseMessage.Content.ReadAsStringAsync();
                logger.LogInformation("🔸 Response From Zarinpal: {response}", raw);

                result = JsonConvert.DeserializeObject<PaymentRequestResponse>(raw);
            }

            return result;
        }

        public async Task<PaymentVerificationResponse> Verification(string authority)
        {
            PaymentVerificationResponse result;
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(new
                {
                    MerchantID = "c632f574-bd37-15e7-99ca-000c295eb9d3",
                    Amount = _amount,
                    Authority = authority
                });
                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", new StringContent(content, Encoding.UTF8, "application/json"));
                result = JsonConvert.DeserializeObject<PaymentVerificationResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            }

            return result;
        }

        public async Task<PaymentRequestResponse> PaymentRequestWithExtra(string description, string additionalData, string callbackUrl, string email = null, string mobile = null)
        {
            PaymentRequestResponse result;
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(new
                {
                    MerchantID = "c632f574-bd37-15e7-99ca-000c295eb9d3",
                    Amount = _amount,
                    Description = description,
                    AdditionalData = additionalData,
                    Email = email,
                    Mobile = mobile,
                    CallbackURL = callbackUrl
                });
                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", new StringContent(content, Encoding.UTF8, "application/json"));
                result = JsonConvert.DeserializeObject<PaymentRequestResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            }

            return result;
        }

        public async Task<PaymentVerificationWithExtraResponse> VerificationWithExtra(string authority)
        {
            PaymentVerificationWithExtraResponse result;
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(new
                {
                    MerchantID = "c632f574-bd37-15e7-99ca-000c295eb9d3",
                    Amount = _amount,
                    Authority = authority
                });
                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", new StringContent(content, Encoding.UTF8, "application/json"));
                result = JsonConvert.DeserializeObject<PaymentVerificationWithExtraResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jaeger;
using Jaeger.Samplers;
using JaegerWrapper;
using Microsoft.Extensions.Logging;

namespace DistributedTracingDemo
{
    class Program
    {
        private static int _testId = 22;

        static void Main(string[] args)
        {
            using (var loggerFactory = new LoggerFactory())
            {
                using (var tracer = InitTracer("DistributedTraceClient", loggerFactory))
                {
                    //TrialCall(tracer);
                    //SingleRequest(tracer);
                    #region Multiple request
                    MultipleRequests(tracer);
                    #endregion
                }
            }
            Console.WriteLine("---Done---");
            Console.Read();
        }

        private static Tracer InitTracer(string serviceName, ILoggerFactory loggerFactory)
        {
            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
                .WithType(ConstSampler.Type)
                .WithParam(1);

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
                .WithLogSpans(true);

            return (Tracer)new Configuration(serviceName, loggerFactory)
                .WithSampler(samplerConfiguration)
                .WithReporter(reporterConfiguration)
                .GetTracer();
        }

        static void TrialCall(Tracer tracer)
        {
            var traceBuilder = new TraceBuilder(tracer);
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44340")
            };

            var url = $"/api/aworld/{_testId}";

            traceBuilder.WithSpanName("TrialCallWork")
                .WithHttpCall(client, url, HttpMethod.Get)
                .TraceIt(() =>
                {
                    var response = client.GetAsync(url).Result;

                    if (!response.IsSuccessStatusCode)
                        throw new Exception("uncovered area for the demo.");

                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseBody);
                });
        }

        static void SingleRequest(Tracer tracer)
        {

            //Guid guid = Guid.NewGuid();
            //IDictionary<string, object> keyValues = new Dictionary<string, object>();
            //keyValues.Add("MyTraceId", guid.ToString());

            var url = $"/api/aworld/{_testId}";
            string spanName = "MainWork";
            Guid guid = Guid.NewGuid();
            Console.WriteLine("guid:{0}", guid);

            var traceBuilder = new TraceBuilder(tracer);
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44340")
            };

            traceBuilder.WithSpanName(spanName)
               .WithTag(new OpenTracing.Tag.StringTag("MyCorelationId"), guid.ToString())
               .WithHttpCall(client, url, HttpMethod.Get)               
               .TraceIt(() =>
               {
                   CallServiceA(client, url);
               });
        }

        private static void MultipleRequests(Tracer tracer)
        {
            int numberOfRequest = 100;
            var actualWorkCount = new int[numberOfRequest];
            int threadCount = 30;
            Parallel.ForEach(actualWorkCount, new ParallelOptions() { MaxDegreeOfParallelism = threadCount }, doWork =>
            {
                _testId = new Random().Next(1, 1000);
                SingleRequest(tracer);
            });
        }

        private static void CallServiceA(HttpClient client, string url)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
        }
    }
}

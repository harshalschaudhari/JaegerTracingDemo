# JaegerTracingDemo
Distributed Tracing using jaeger (C#)

This is a sample application demonstrating how to use an Jaeger (using C#) to apply Distributed Tracing.

## How to Setup

Setup `Jaeger UI` on local computer.

<a href="http://azure-blaze.blogspot.com/2019/08/jaeger-local-setup.html">How to Setup Jaeger on local using docker</a>

## Application Run sequence as below
<ol>
  <li>Run Jaeger UI Docker. (Steps as above) </li>
  <li>Run B Service.</li>
  <li>Run A Service.</li>
  <li>Run Distributed Application </li>
</ol>

**Note: Jaeger UI <a href="http://localhost:16686/">http://localhost:16686/</a>

Application Flow: Distributed Console App -> A Service -> B Service

## Reference

<ul>
  <li><a href="https://github.com/jaegertracing/jaeger-client-csharp">jaeger client C#</a></li>
  <li><a href="https://github.com/opentracing/opentracing-csharp">opentracing C#</a></li>
</ul>

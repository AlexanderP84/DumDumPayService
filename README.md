# DumDumPayService
Test assignment: an integration with DumDumPay test API.
The solution consists of class library that implements API calling and exposing an interface.
For testing purposes created two projects: one is a simple set of tests for calling API and another one is a test web application that simulates processing of payments
(perfomed at pipeline).
Client projects use interface from library via DI.

Added Serilog for logging (configured in test web app, class library uses DI with ILogger).

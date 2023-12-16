---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `02`
author:
- "Oliver Prip Hagmund <ohag@itu.dk>"
- "Casper Pilgaard <cpil@itu.dk>"
- "Marius Stokkebro <masto@itu.dk>"'
- "Mads Orfelt <orfe@itu.dk>"
- "Jerome Rahal <jrah@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

Here comes a description of our Process. 

## Build, test, release, and deployment

In this section we will discuss our continuous deployment of the Chirp software using GitHub Actions. We will provide an in depth explanation of how our continuous deployment tackles building the program, testing, deploying to the web platform, and ensuring working releases for all relevant platforms. Lastly we will discuss how dependencies are tackled using dependabot.

### Building

To build the program we use the "build_and_test.yml" workflow. This workflow runs on pushes and pulls to the main branch, this is to view the status of our tests whenever something gets added or possibly merged. In the workflow, the 3 first steps are responsible for building the program before testing. The first step is "Setup .NET" which uses "actions/setup-dotnet@v3", with dotnet version 7.0. This is responsible for setting up the program before building it, and is a part of the "action/checkout@v3" collection. The next step is to restore dependencies, which is done by running "dotnet restore src/Chirp.Razor" this simply runs this in the commandline as a necessary build step to ensure all our dependencies are restored. Lastly we run the "Build" step which runs the "dotnet build src/Chirp.Razor --no-restore" command, which essentially responsible for building the program so that it now can be tested upon.   

### Testing

Running the tests is done with the last step in the "build_and_test.yml" workflow file. This step is called "Test" and runs the command "dotnet test test/test.Chirp --no-build --verbosity normal", which runs our tests. We have put alot of thought and effort into having the correct and necessary tests such as; end to end (also called E2E), Integration, and unit tests. These tests are done with the Triple A principle in mind (arrange, act and assert).

### Release 


### Deployment


### Handling dependencies


## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

Here we talk about our Ethics. 

## License

## LLMs, ChatGPT, CoPilot, and others
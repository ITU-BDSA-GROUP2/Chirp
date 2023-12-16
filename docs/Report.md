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

In this segment we will show 3 different user-activity diagrams that shows how a user would use our website.

The diagrams below show how a non-authorized user will use the "Chirp!" application there are 2 different but very similar looking diagrams. The left one is how a user who has registered on the site but is not logged in yet. The right one show a user who has not yet registered themselves as a user.

![User activity diagram of a non-authorized user that has registered](https://www.plantuml.com/plantuml/svg/JOx13i8m28RlVGgUOxlCnADNunaxmiYwqhHKeqylEcCyaO-V3vYQORM-9SYQiTkYLPuqdnlLYzXoKfPyY2OtiSTHa2jkuQE48QckW0Pn8Ifj34DC4bT8RyM95KntWe9COsWbYNjxwov-gnkmrtxzXe2Calz7VZdb6VO5pRI4oTc_3Yy0) ![user activity diagram of a non-authorized user that has not registered](https://www.plantuml.com/plantuml/svg/NOx13SCm24NlJC4SoIMhK1SH5DTO1glh5xML3pa1ZmVwXmUDcAyAtmQsQdOX1PQJkViEMwbQBXmiwi4ZWHY6BO2TX7VmNZMl5trhd3O39Ujy_FT8GjfkrbAuFV7tniCw73bdhWy0)

The diagram below shows how an authorized user will use the "Chirp!" application it is shown as a loop since most of the tasks that a user can do will still end up with them at the front page looking at cheeps. We omitted how they interact with the "about me" page and the "my timeline" page. For the "my timeline" page what a user can interact with is about the same as public timeline the cheeps are just different. For the "about me" page we omitted it since the things a user can can change their information and the diagram would look almost identical to the 2 previous diagrams in this section.

![User activity diagram of an authorized user](https://www.plantuml.com/plantuml/svg/VP11ReGm34NtFeMNp1MOpLp51JDO63jA78suVO6ehQOgtKL-J_z9UPIW77LLE1-GEJ45zEg-80KECtCgToX99K2cZchCdb4AJgxgBnvl63CRTXkN6_I3oh1WjKRH3QcDt86rS6V-BVsbs2XJYo4zIUn8dWn9CzEueShorNnBZb8ET1KxsV-f_fKb-CTuOst53TSDGrPMCRvIwCDhuFgRgp_t-tuCJ_3O-yUtVm80)

## Sequence of functionality/calls through _Chirp!_

# Process

Here comes a description of our Process.

## Build, test, release, and deployment

## Team work

We do not have any unfinished issues on our project board we have made all the features that we set out to make, one part of our program that we could have worked more on would have been to add more test though, specifically for our wild style features.

The process we went through was we would meet when new tasks where given to make new issues. Usually it was 2 people who sat and made the new issues who these where changed between weeks. The other people would work on previous tasks we maybe hadn't completed yet. We would then delegate the new issues between us sometimes 2 or 3 people would work on one issue using pair programming otherwise it would be 1 person working on 1 issue. We would also make a new branch, specifically for that issue. We where not great at using the project board and would often put an issue up on the project board in the in progress column and then forget about the project board. Then 2 weeks later put the issue over in the done column if the issue was solved. If we felt that an issue was solved we would then make a pull request to merge to main. Then most of the time another member of the team who hadn't worked on the issue would review and approve the merge, but if no one answered the messages for a pull request. One of the team members who worked on the issue would approve and merge themselves. We did this, so we wouldn't have branches that didn't get used for more than a day.

## How to make _Chirp!_ work locally

## How to run test suite locally
To run our in memory tests and unit tests, start by standing at the root of the directory called Chirp. Then from the terminal type ->
cd test/test.Chirp after that run the command -> dotnet test. Within the inMemoryTests.cs file you will find test suites that tests various database methods which lies within the AuthorRepository, CheepRepository and FollowerListRepository. It starts by creating a new database after that it establishes a connection to the database and then seeds it with various cheeps and authors. We use the typical Arrange, Act and Assert notation to test the different repository methods. Once the tests are completed we dispose of the database and all the cheeps and authors within it. 

In our UnitTestChirp.cs file you will find some out-commented unit tests. These are all methods that are not in use anymore or they fail which then stops our action workflow from deploying. We chose to use the inLineData format for the tests, this was to test different values on a single method at once. Some of the tests consist of testing the web application such as being able to see the public/private timeline or finding specific users cheeps.   

To see our playwright tests stand at the root of our directory Chirp then type in the terminal -> cd test/PlaywrightTests once you are in the directory type -> dotnet test this will run various Razor web application tests. all of the methods starts by connecting to our Azure front webpage. The PlayWright tests works by calling different methods from the Playwright library. these methods are for example GetByRole() or GetByPlaceholder() which essentially finds the desired text field/button and then calls the methods such as FillAsync() or ClickAsync() which fills the field with text or clicks a button respectively. Once that is done and it is on the correct page with the correct information we call the Expect method to check that the correct text is found and if so the test succeeds. 
 
# Ethics

Here we talk about our Ethics.

## License

## LLMs, ChatGPT, CoPilot, and others

```

```

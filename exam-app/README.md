# Web Application Deployment Task
This application is part of the exam for Cloud Computing with Microsoft Azure.

To complete the task you must deploy the application and configure the following storage connection string from the Azure Portal.

`DefaultEndpointsProtocol=https;AccountName=examstorageaubg;AccountKey=3EKvPoTZYVJ22uW1i2EkM3a38fhcWz8z4r7gptlWyQdIyp0xCHoW8jgwdeJX/z9Bp4c6unRVjGCJFcb1PAGu6Q==;EndpointSuffix=core.windows.net`

The application should be deployed using External Git deployment option using the following URL - https://github.com/AzureAubg/2018Spring.git  
Since the source repository contains multiple applications you need to specify which applications should be deployed. 
This can be done by adding a special Application Setting to your Web App with key PROJECT and value the path of the target folder - in this case it should be "exam-app/"

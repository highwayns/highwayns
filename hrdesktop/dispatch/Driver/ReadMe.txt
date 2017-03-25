1. Open the solution file MySpaceID.SDK.sln. 

2. In folder Samples you will find a project named "MySpaceID-OAuth", set this as the startup Project and make Default as startup page. 

3. Under the same project goto the class "Core/Constants.cs" and update the values of all constants.   

4. In the Project \MyspaceUnitTest goto the class "Constants.cs" and set all the values of constants so that test cases will run.

5. From the menu of VS 2008 goto "TEST" and run all tests in entire solution. For more details regarding Test run check the file on the following location "~\c# SDK\MyspaceUnitTest\AuthoringTests.txt"

6. After Giving Key and secret to the constants Class and then run the solution. 

7. The sample app will call the functions from mySpace.cs class. 

Endpoints Classes (Config folder): 
i. V2Endpoints.cs
ii. V1Endpoints.cs
iii. RoaEndpoints.cs
iv. RealTimeStreamEndpoints.cs
v. OpenSearchEndpoints.cs
vi.ActivityStreamEndpoints.cs

Implementation: (Api folder)
i. ActivityStream.cs
ii. OpenSearch.cs
iii. PortableContacts.cs
iv. RealTimeStream.cs
v. RestV1.cs
vi. RoaApi.cs

Rapper Class: MySpace.cs



	
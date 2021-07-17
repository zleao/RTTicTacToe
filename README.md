# RTTicTacToe
Sample of a fullstack solution with the implementation of an online/realtime TicTacToe game. The purpose is to test out some tools and frameworks. The solution provides a WebApi (currently hosted on [Azure](https://rttictactoe.azurewebsites.net/swagger)) and a frontend with a basic implementation of the TicTacToe game.

### **Always refer to the readme file of the [master](https://github.com/zleao/RTTicTacToe/tree/master) branch, to get the most up-to-date info.**

## Backend
Simple Asp.NetCore API with a CQRS/ES pattern. For real time communication, it uses SignalR. The data is persisted in a SQLite database with the use of EFCore (both for EventSourceing and read datamodel).

**Key technologies:**
- Asp.NetCore v2.2
- CQRSLite v0.23.0
- NSwag.AspNetCore v12.1.0
- Microsoft.AspNetCore.SignalR.Core (1.1.0)
- EFCore v2.2.4
- EFCore.Sqlite v2.2.4

## Frontend
Support for **iOS** and **Android**
- Xamarin Forms 3.4.0 ([XamForms3.4](https://github.com/zleao/RTTicTacToe/tree/XamForms3.4) branch)
- Xamarin Forms 4.0 pre8 ([master](https://github.com/zleao/RTTicTacToe/tree/master) and [XamForms4](https://github.com/zleao/RTTicTacToe/tree/XamForms4) branch)


## Run the code
Use VS2019+ to open the solution. The solution contains the projects for frontend and backend.
You can try out the code by running the whole system locally or host the api in any online hosting service. Currently I'm hosting the api in Azure ([https://rttictactoe.azurewebsites.net/swagger](https://rttictactoe.azurewebsites.net/swagger)) and will continue to do so for testing. 

**Note:** at any given time this endpoint might be deleted without prior notice.

### Backend
The main api project is the `RTTicTacToe.WebApi`. You can run the api locally (it should work without any special configuration) or publish it to a hosting service. Take a note of the endpoint where it's going to run (by default, if running locally, it should be on http://localhost:5000 or http://localhost:7243). The endpoint will be important to configure the Frontend app.

### Frontend
Since the frontend is implemented with Xamarin Forms, you can choose one of the implemented platforms and run in a simulator or real device. The only change that is needed to be checked, is the base url for the api endpoint. To configure it, just go `RTTicTacToe.Forms/App.cs` and set the static string `AzureBackendUrl`. By default it's pointing to the Azure hosted web app.

## Future tryouts
### Backend
- Migrate to .net 5
- Try [EventFlow](https://eventflow.readthedocs.io/) for the CQRS/ES handling
- Use a specialized framework to store the data from EventSourcing, (e.g. [NEventStore](https://github.com/NEventStore/NEventStore))
- Add logic to revert movements using CQRS features
### Frontend
- Migrate to MAUI